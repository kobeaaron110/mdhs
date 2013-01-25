using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using MdhsBus.Data;
using Aspose.Words;
using System.IO;
using MySchoolModule;
using AccountsReceivable.API;
using FISCA.Presentation;
using AccountsReceivable.ReceiptUtility;
using K12.Data;

namespace MdhsBus
{
    public partial class StudnetPaymentSheet : BaseForm
    {
        public StudnetPaymentSheet()
        {
            InitializeComponent();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            StudentByBus studentbus = new StudentByBus();
            StudentByBus CurrentStudentbus = new StudentByBus();
            string CurrentBusStopName = "";
            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(this.intSchoolYear.Value, cboBusRange.Text);
            //List<string> newStuNumberList = new List<string>();
            List<string> StudentIDList = K12.Presentation.NLDPanels.Student.SelectedSource;
            List<StudentRecord> stuRec = Student.SelectByIDs(StudentIDList);
            Dictionary<string, StudentByBus> studentbusRecord = new Dictionary<string, StudentByBus>();

            studentbus = StudentByBusDAO.SelectByBusYearAndTimeNameAndStudntID(this.intSchoolYear.Value, this.cboBusRange.Text, StudentIDList[0]);
            if (studentbus != null)
            {
                if (textBusStop.Text != "" && textBusStop.Text != studentbus.BusStopID)
                    studentbus.BusStopID = textBusStop.Text;
                BusStop buses = BusStopDAO.SelectByBusTimeNameAndByStopID(bussetup.BusTimeName, textBusStop.Text);
                CurrentBusStopName = buses.BusStopName;
                int total = 0;
                if (studentbus.DateCount > 0)
                    total = buses.BusMoney * studentbus.DateCount;
                else
                    total = buses.BusMoney * bussetup.DateCount;
                int div_value = total / 10;
                if ((total - div_value * 10) < 5)
                    studentbus.BusMoney = div_value * 10;
                else
                    studentbus.BusMoney = div_value * 10 + 10;
                studentbus.Save();
                CurrentStudentbus = studentbus;
            }
            else
            {
                StudentByBus newstudentbus = new StudentByBus();
                if (textBusStop.Text != "")
                    newstudentbus.BusStopID = textBusStop.Text;
                BusStop buses = BusStopDAO.SelectByBusTimeNameAndByStopID(bussetup.BusTimeName, textBusStop.Text);
                CurrentBusStopName = buses.BusStopName;
                newstudentbus.BusRangeName = cboBusRange.Text;
                newstudentbus.BusStopID = textBusStop.Text;
                newstudentbus.BusTimeName = bussetup.BusTimeName;
                newstudentbus.ClassName = stuRec[0].Class.Name;
                newstudentbus.ClassID = stuRec[0].Class.ID;
                newstudentbus.DateCount = bussetup.DateCount;
                newstudentbus.SchoolYear = bussetup.BusYear;
                newstudentbus.StudentID = stuRec[0].ID;
                int total = buses.BusMoney * bussetup.DateCount;
                int div_value = total / 10;
                if ((total - div_value * 10) < 5)
                    newstudentbus.BusMoney = div_value * 10;
                else
                    newstudentbus.BusMoney = div_value * 10 + 10;
                newstudentbus.Save();

                CurrentStudentbus = newstudentbus;
            }



            //if (!File.Exists(Application.StartupPath + "\\Customize\\校車繳費單樣版.doc"))
            //{
            //    MessageBox.Show("『" + Application.StartupPath + "\\Customize\\校車繳費單樣版.doc』檔案不存在，請確認後重新執行！");
            //    return;
            //}

            //if (!File.Exists(Application.StartupPath + "\\Customize\\校車繳費單樣版-現有學生.doc"))
            //{
            //    MessageBox.Show("『" + Application.StartupPath + "\\Customize\\校車繳費單樣版-現有學生.doc』檔案不存在，請確認後重新執行！");
            //    return;
            //}

            //Document Template = new Document(Application.StartupPath + "\\Customize\\校車繳費單樣版-現有學生.doc");
            Document Template = new Aspose.Words.Document(new MemoryStream(Properties.Resources.校車繳費單樣版_現有學生));
            //Document Template = new Document();
            //mPreference = TemplatePreference.GetInstance();
            //if (mPreference.UseDefaultTemplate)
            //    Template = new Aspose.Words.Document(new MemoryStream(Properties.Resources.校車繳費單樣版_新生));
            //else
            //    Template = new Aspose.Words.Document(mPreference.CustomizeTemplate);

            Document doc = new Document();

            Payment SelectPayment = cboPayment.SelectedItem as Payment;

            //新增收費明細。
            List<PaymentDetail> StudentDetails = new List<PaymentDetail>();

            SelectPayment.FillFull();
            List<PaymentDetail> AllStudentdetail = PaymentDetail.GetByTarget("Student", StudentIDList);
            List<PaymentDetail> Studentdetail = new List<PaymentDetail>();
            List<PaymentDetail> CurrentDetails = new List<PaymentDetail>();
            List<PaymentHistory> historys = new List<PaymentHistory>();
            List<string> ids = new List<string>();
            foreach (PaymentDetail var in AllStudentdetail)
            {
                if (var.Extensions["校車收費年度"] != intSchoolYear.Value.ToString() || var.Extensions["校車收費名稱"] != cboBusRange.SelectedItem.ToString())
                    continue;
                Studentdetail.Add(var);
            }

            if (Studentdetail.Count == 0)
            {
                PaymentDetail detail = new PaymentDetail(SelectPayment);    //設定「收費明細」所屬的收費。

                detail.RefTargetID = stuRec[0].ID;//搭乘校車新生ID。
                detail.RefTargetType = "Student";
                detail.Amount = CurrentStudentbus.BusMoney;                  //要收多少錢。
                //detail.Extensions.Add("校車收費年度", intSchoolYear.Value.ToString());
                detail.Extensions.Add("校車收費年度", SelectPayment.SchoolYear.ToString());
                detail.Extensions.Add("校車收費學期", SelectPayment.Semester.ToString());
                detail.Extensions.Add("校車收費名稱", cboBusRange.SelectedItem.ToString());
                detail.Extensions.Add("MergeField::代碼", textBusStop.Text);
                detail.Extensions.Add("MergeField::站名", CurrentBusStopName);
                detail.Extensions.Add("MergeField::" + "學號", stuRec[0].StudentNumber);
                detail.Extensions.Add("MergeField::" + "姓名", stuRec[0].Name);
                detail.Extensions.Add("MergeField::" + "班級", stuRec[0].Class.Name);
                detail.Extensions.Add("開始日期", bussetup.BusStartDate.ToString());
                detail.Extensions.Add("結束日期", bussetup.BusEndDate.ToString());
                detail.Extensions.Add("搭車天數", CurrentStudentbus.DateCount.ToString());
                StudentDetails.Add(detail);                                 //先加到一個 List 中。

                List<string> detailIDs = new List<string>();
                detailIDs = PaymentDetail.Insert(StudentDetails.ToArray());     //新增到資料庫中。
                

                CurrentDetails = PaymentDetail.GetByIDs(detailIDs.ToArray());

                foreach (PaymentDetail pd in CurrentDetails)
                {
                    MotherForm.SetStatusBarMessage("條碼產生中....");
                    Application.DoEvents();
                    //新增一筆繳費記錄。
                    PaymentHistory history = new PaymentHistory(pd);
                    history.Amount = pd.Amount; //通常會與金額與繳費明細一樣。
                    ((Payment)cboPayment.SelectedItem).CalculateReceiptBarcode(history); //計算條碼資料，計算時需要「Payment」物件。
                    historys.Add(history);
                }
                ids = PaymentHistory.Insert(historys);
            }
            else
            {
                foreach (PaymentDetail pdetail in Studentdetail)
                {
                    CurrentDetails.Add(pdetail);
                    if (pdetail.Extensions["校車收費名稱"] == cboBusRange.SelectedItem.ToString() && int.Parse(pdetail.Extensions["校車收費年度"]) == intSchoolYear.Value)
                    {
                        if ((pdetail.Extensions["MergeField::代碼"] != textBusStop.Text && pdetail.Extensions["MergeField::站名"] != CurrentBusStopName) || int.Parse(pdetail.Extensions["搭車天數"]) != CurrentStudentbus.DateCount)
                        {
                            if (CurrentStudentbus.DateCount > 0)
                            {
                                pdetail.Extensions["搭車天數"] = CurrentStudentbus.DateCount.ToString();
                                pdetail.Amount = CurrentStudentbus.BusMoney;
                            }

                            List<PaymentDetail> pdetails = new List<PaymentDetail>();
                            pdetail.Amount = CurrentStudentbus.BusMoney;
                            pdetail.Extensions["MergeField::代碼"] = textBusStop.Text;
                            pdetail.Extensions["MergeField::站名"] = CurrentBusStopName;
                            pdetails.Add(pdetail);
                            PaymentDetail.Update(pdetails.ToArray()); //更新到資料庫中。

                            //註銷先前之繳費單
                            pdetail.CancelExistReceipts();


                            foreach (PaymentDetail pd in CurrentDetails)
                            {
                                MotherForm.SetStatusBarMessage("條碼產生中....");
                                Application.DoEvents();
                                //新增一筆繳費記錄。
                                PaymentHistory history = new PaymentHistory(pd);
                                history.Amount = pd.Amount; //通常會與金額與繳費明細一樣。
                                ((Payment)cboPayment.SelectedItem).CalculateReceiptBarcode(history); //計算條碼資料，計算時需要「Payment」物件。
                                historys.Add(history);
                            }
                            ids = PaymentHistory.Insert(historys);
                            continue;
                        }
                        else
                        {
                            pdetail.FillHistories();
                            
                            IList<PaymentHistory> historyss = pdetail.Histories;
                            if (historyss == null)
                                return;
                            foreach (PaymentHistory history in historyss)
                            {
                                if (history.Receipt.Cancelled)
                                    continue;
                                else
                                {
                                    ids.Add(history.UID);
                                    break;
                                }
                            }
                        }
                    }
                }
            }


            historys = PaymentHistory.GetByIDs(ids.ToArray());
            //產生繳費單
            ReceiptDocument rdoc = new ReceiptDocument(Template); //如果要自定樣版，可以用這個建構式。
            List<PaymentReceipt> prRecood = new List<PaymentReceipt>();
            foreach (PaymentHistory history in historys)
            {
                MotherForm.SetStatusBarMessage("繳費單產生中....");
                Application.DoEvents();
                prRecood.Add(history.Receipt);
            }
            doc = rdoc.Generate2(prRecood);


            if (doc.Sections.Count != 0)
            {
                string path = Path.Combine(Application.StartupPath, "Reports");
                string reportName = "繳費單";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path = Path.Combine(path, reportName + ".doc");

                if (File.Exists(path))
                {
                    int i = 1;
                    while (true)
                    {
                        string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                        if (!File.Exists(newPath))
                        {
                            path = newPath;
                            break;
                        }
                    }
                }

                try
                {
                    doc.Save(path, SaveFormat.Doc);
                    System.Diagnostics.Process.Start(path);
                }
                catch
                {
                    System.Windows.Forms.SaveFileDialog sd1 = new System.Windows.Forms.SaveFileDialog();
                    sd1.Title = "另存新檔";
                    sd1.FileName = "繳費單.doc";
                    sd1.Filter = "Word檔案 (*.doc)|*.doc|所有檔案 (*.*)|*.*";
                    if (sd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        try
                        {
                            doc.Save(sd1.FileName, SaveFormat.Doc);
                            System.Diagnostics.Process.Start(sd1.FileName);
                        }
                        catch
                        {
                            System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            MotherForm.SetStatusBarMessage("繳費單產生完成。");
            this.Close();
        }

        private void StudnetPaymentSheet_Load(object sender, EventArgs e)
        {
            List<string> stuid = K12.Presentation.NLDPanels.Student.SelectedSource;
            if (stuid.Count == 0)
            {
                MessageBox.Show("學生請選一人");
                this.Close();
                return;
            }

            if (stuid.Count > 1)
            {
                MessageBox.Show("學生一次請選一人");
                this.Close();
                return;
            }

            List<StudentRecord> stuRec = Student.SelectByIDs(stuid);

            this.labelX8.Text = "學生：" + stuRec[0].Class.Name + stuRec[0].StudentNumber + stuRec[0].Name;
            this.intSchoolYear.Value = int.Parse(K12.Data.School.DefaultSchoolYear);
            this.cboSemester.Text = K12.Data.School.DefaultSemester == "1" ? "上學期" : "下學期";

            this.cboBusRange.Items.Clear();

            List<BusSetup> bussetup = BusSetupDAO.SelectByBusYear(this.intSchoolYear.Value);
            foreach (BusSetup bus in bussetup)
            {
                if (Payment.GetByID(bus.BusPaymentName).SchoolYear == this.intSchoolYear.Value && Payment.GetByID(bus.BusPaymentName).Semester == (this.cboSemester.Text == "上學期" ? 1 : 2))
                    this.cboBusRange.Items.Add(bus.BusRangeName);
            }
            
            if (this.cboBusRange.Items.Count > 0)
            {
                this.cboBusRange.SelectedIndex = this.cboBusRange.Items.Count - 1;
                //reloadstatus();
            }
            else
            {
                this.textBusStop.Text = "";
                this.textBusStopName.Text = "";
            }
        }

        private void reloadstatus()
        {
            if (cboBusRange.Text != "")
            {
                StudentByBus studentbus = new StudentByBus();
                studentbus = StudentByBusDAO.SelectByBusYearAndTimeNameAndStudntID(this.intSchoolYear.Value, cboBusRange.Text, K12.Presentation.NLDPanels.Student.SelectedSource[0]);

                BusSetup bussetup = new BusSetup();
                if (BusSetupDAO.SelectByBusYear(this.intSchoolYear.Value).Count == 0)
                {
                    MessageBox.Show("此年度無校車紀錄，請重新選擇！");
                    return;
                }
                else
                    bussetup = BusSetupDAO.SelectByBusYearAndRange(this.intSchoolYear.Value, this.cboBusRange.Text);

                List<BusStop> busstops = BusStopDAO.SelectByBusTimeNameOrderByStopID(bussetup.BusTimeName);

                if (studentbus == null)
                {
                }
                else
                {
                    BusStop stu_busstop = BusStopDAO.SelectByBusTimeNameAndByStopID(bussetup.BusTimeName, studentbus.BusStopID);
                    this.textBusStop.Text = stu_busstop.BusStopID;
                    this.textBusStopName.Text = stu_busstop.BusStopName;
                }
            }
        }

        private void cboBusRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region cboBusRange異動
            this.labelX3.Text = "";
            if (cboBusRange.Text != "")
            {
                StudentByBus studentbus = new StudentByBus();
                studentbus = StudentByBusDAO.SelectByBusYearAndTimeNameAndStudntID(this.intSchoolYear.Value, cboBusRange.Text, K12.Presentation.NLDPanels.Student.SelectedSource[0]);

                BusSetup bussetup = new BusSetup();
                if (BusSetupDAO.SelectByBusYear(this.intSchoolYear.Value).Count == 0)
                {
                    MessageBox.Show("此年度無校車紀錄，請重新選擇！");
                    return;
                }
                else
                    bussetup = BusSetupDAO.SelectByBusYearAndRange(this.intSchoolYear.Value, this.cboBusRange.Text);
                List<BusStop> busstops = BusStopDAO.SelectByBusTimeNameOrderByStopID(bussetup.BusTimeName);


                if (studentbus == null)
                {
                }
                else
                {
                    BusStop stu_busstop = BusStopDAO.SelectByBusTimeNameAndByStopID(bussetup.BusTimeName, studentbus.BusStopID);
                    this.textBusStop.Text = stu_busstop.BusStopID;
                    this.textBusStopName.Text = stu_busstop.BusStopName;
                }

                //讀取系統中所有的收費模組(日期設定)。
                List<Payment> pays = Payment.GetAll();
                int next_schoolYear = 0;
                string next_semester = "上學期";
                if (this.cboSemester.Text == "上學期")
                {
                    next_schoolYear = this.intSchoolYear.Value;
                    next_semester = "下學期";
                }
                else
                    next_schoolYear = this.intSchoolYear.Value + 1;
                this.cboPayment.Items.Clear();
                if (pays.Count > 0)
                {
                    foreach (Payment pay in pays)
                        //if ((pay.SchoolYear == this.intSchoolYear.Value && pay.Semester == (this.cboSemester.Text == "上學期" ? 1 : 2)) || (pay.SchoolYear == next_schoolYear && pay.Semester == (next_semester == "上學期" ? 1 : 2)))
                        if (pay.SchoolYear == this.intSchoolYear.Value && pay.Semester == (this.cboSemester.Text == "上學期" ? 1 : 2))
                            if (pay.Name == this.cboBusRange.Text)
                                this.cboPayment.Items.Add(pay);
                }
                if (cboPayment.Items.Count == 0)
                {
                    this.btnReport.Enabled = false;
                    MessageBox.Show("請先設定收費模組");
                    this.Close();
                    return;
                }
                else
                {
                    this.cboPayment.SelectedIndex = 0;
                    this.cboPayment.Enabled = false;
                    Payment selectPayment = cboPayment.SelectedItem as Payment;

                    if (!selectPayment.Extensions.ContainsKey("DefaultExpiration"))
                    {
                        this.btnReport.Enabled = false;
                        MessageBox.Show("請先設定收費之截止日期");
                        this.Close();
                        return;
                    }
                    else
                    {
                        DateTime dutedate;
                        if (!DateTime.TryParse(selectPayment.Extensions["DefaultExpiration"], out dutedate))
                        {
                            this.btnReport.Enabled = false;
                            MessageBox.Show("請先設定收費之截止日期");
                            this.Close();
                            return;
                        }
                        else
                            this.dtDueDate.Value = DateTime.Parse(selectPayment.Extensions["DefaultExpiration"]);

                        List<PaymentDetail> Studentdetail = PaymentDetail.GetByTarget("Student", K12.Presentation.NLDPanels.Student.SelectedSource);
                        if (Studentdetail.Count > 0)
                        {
                            foreach (PaymentDetail pdetail in Studentdetail)
                            {
                                if (pdetail.Extensions["校車收費名稱"] == cboBusRange.SelectedItem.ToString() && int.Parse(pdetail.Extensions["校車收費年度"]) == intSchoolYear.Value)
                                {
                                    pdetail.FillHistories();
                                    IList<PaymentHistory> historyss = pdetail.Histories;
                                    if (historyss == null)
                                        this.labelX3.Text = "未產生過繳費單";
                                    else
                                        this.labelX3.Text = "已產生過繳費單";
                                }
                            }
                        }
                        else
                            this.labelX3.Text = "未產生過繳費單";
                    }
                }


                if (this.textBusStop.Text.Length == 4)
                {
                    this.btnReport.Enabled = true;
                    this.dtDueDate.Enabled = false;
                }
                else
                {
                    this.btnReport.Enabled = false;
                    this.dtDueDate.Enabled = false;
                }
            }
            #endregion
        }

        private void intSchoolYear_ValueChanged(object sender, EventArgs e)
        {
            this.btnReport.Enabled = false;
            this.dtDueDate.Enabled = false;
            this.labelX3.Text = "";

            this.cboBusRange.Items.Clear();
            
            List<BusSetup> bussetup = BusSetupDAO.SelectByBusYear(this.intSchoolYear.Value);
            foreach (BusSetup bus in bussetup)
            {
                if (Payment.GetByID(bus.BusPaymentName).SchoolYear == this.intSchoolYear.Value && Payment.GetByID(bus.BusPaymentName).Semester == (this.cboSemester.Text == "上學期" ? 1 : 2))
                    this.cboBusRange.Items.Add(bus.BusRangeName);
            }
            if (this.cboBusRange.Items.Count > 0)
                this.cboBusRange.SelectedIndex = this.cboBusRange.Items.Count - 1;

            this.textBusStop.Text = "";
            this.textBusStopName.Text = "";
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.textBusStop.Text.Length == 4)
                this.btnReport.Enabled = true;
            else
                this.btnReport.Enabled = false;

            this.cboBusRange.Items.Clear();
            List<BusSetup> bussetup = BusSetupDAO.SelectByBusYear(this.intSchoolYear.Value);
            foreach (BusSetup bus in bussetup)
            {
                if (Payment.GetByID(bus.BusPaymentName).SchoolYear == this.intSchoolYear.Value && Payment.GetByID(bus.BusPaymentName).Semester == (this.cboSemester.Text == "上學期" ? 1 : 2))
                    this.cboBusRange.Items.Add(bus.BusRangeName);
            }
            if (this.cboBusRange.Items.Count == 0)
            {
                this.labelX3.Text = "";
                this.textBusStop.Text = "";
                this.textBusStopName.Text = "";
            }
            else
                this.cboBusRange.SelectedIndex = this.cboBusRange.Items.Count - 1;
        }

        private void cboPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtDueDate.Value = DateTime.Parse(((Payment)cboPayment.SelectedItem).Extensions["DefaultExpiration"]);
        }

        private void textBusStop_Leave(object sender, EventArgs e)
        {
            CheeckBusStop();
        }

        private void CheeckBusStop()
        {
            if (this.cboBusRange.Text != "")
            {
                if (textBusStop.Text.Length != 4)
                {
                    this.textBusStopName.Text = "";
                    this.btnReport.Enabled = false;
                    this.dtDueDate.Enabled = false;
                    MessageBox.Show("電腦代碼為四位，請確認！");
                    return;
                }
                else
                {
                    BusSetup bussetup = new BusSetup();
                    bussetup = BusSetupDAO.SelectByBusYearAndRange(this.intSchoolYear.Value, this.cboBusRange.Text);
                    BusStop busstops = BusStopDAO.SelectByBusTimeNameAndByStopID(bussetup.BusTimeName, textBusStop.Text);
                    if (busstops.BusStopID == null)
                    {
                        this.textBusStopName.Text = "";
                        this.btnReport.Enabled = false;
                        this.dtDueDate.Enabled = false;
                        MessageBox.Show("電腦代碼錯誤，請確認！");
                        return;
                    }
                    else
                        this.textBusStopName.Text = busstops.BusStopName;

                    this.btnReport.Enabled = true;
                    this.dtDueDate.Enabled = false;
                }
            }
        }
    }
}
