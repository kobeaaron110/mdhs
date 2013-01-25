using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AccountsReceivable.API;
using FISCA.Presentation.Controls;
using MdhsBus.Data;
using MySchoolModule;
using FISCA.Presentation;
using Aspose.Words;
using AccountsReceivable.ReceiptUtility;

namespace MdhsBus
{
    public partial class PaymentSheet : BaseForm
    {
        private TemplatePreference mPreference;

        public PaymentSheet()
        {
            InitializeComponent();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            StudentByBus studentbus = new StudentByBus();
            StudentByBus CurrentStudentbus = new StudentByBus();
            string CurrentBusStopName = "";
            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(this.intSchoolYear.Value, cboBusRange.Text);
            MySchoolModule.NewStudentRecord newStu = new MySchoolModule.NewStudentRecord();
            foreach (MySchoolModule.NewStudentRecord nsr in NewStudent.Instance.SelectedList)
            {
                studentbus = StudentByBusDAO.SelectByBusYearAndTimeNameAndStudntID(this.intSchoolYear.Value, cboBusRange.Text, nsr.UID);
                newStu = nsr;
            }
            if (studentbus != null)
            {
                //if (cboBusStopID.SelectedItem.ToString() != "" && cboBusStopID.SelectedItem.ToString().Substring(0, 4) != studentbus.BusStopID)
                if (textBusStop.Text != "" && textBusStop.Text != studentbus.BusStopID)
                    studentbus.BusStopID = textBusStop.Text;
                //BusStop buses = BusStopDAO.SelectByBusTimeNameAndByStopID(bussetup.BusTimeName, cboBusStopID.SelectedItem.ToString().Substring(0, 4));
                BusStop buses = BusStopDAO.SelectByBusTimeNameAndByStopID(bussetup.BusTimeName, textBusStop.Text);
                CurrentBusStopName = buses.BusStopName;
                int total = buses.BusMoney * bussetup.DateCount;
                int div_value = total / 10;
                if ((total - div_value * 10) < 5)
                    studentbus.BusMoney = div_value * 10;
                else
                    studentbus.BusMoney = div_value * 10 + 10;
                studentbus.Save();
                //this.Close();
                //return;
                CurrentStudentbus = studentbus;
            }
            else
            {
                StudentByBus newstudentbus = new StudentByBus();
                //if (cboBusStopID.SelectedItem.ToString() != "")
                if (textBusStop.Text != "")
                    newstudentbus.BusStopID = textBusStop.Text;
                BusStop buses = BusStopDAO.SelectByBusTimeNameAndByStopID(bussetup.BusTimeName, textBusStop.Text);
                CurrentBusStopName = buses.BusStopName;
                newstudentbus.BusRangeName = cboBusRange.Text;
                newstudentbus.BusStopID = textBusStop.Text;
                newstudentbus.BusTimeName = bussetup.BusTimeName;
                newstudentbus.ClassName = newStu.Dept;
                newstudentbus.ClassID = newStu.SchoolYear;
                newstudentbus.DateCount = bussetup.DateCount;
                newstudentbus.SchoolYear = bussetup.BusYear;
                newstudentbus.StudentID = newStu.UID;
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

            //if (!File.Exists(Application.StartupPath + "\\Customize\\校車繳費單樣版-新生.doc"))
            //{
            //    MessageBox.Show("『" + Application.StartupPath + "\\Customize\\校車繳費單樣版-新生.doc』檔案不存在，請確認後重新執行！");
            //    return;
            //}

            //Document Template = new Document(Application.StartupPath + "\\Customize\\校車繳費單樣版-新生.doc");
            Document Template = new Aspose.Words.Document(new MemoryStream(Properties.Resources.校車繳費單樣版_新生));
            //Document Template = new Document();
            //mPreference = TemplatePreference.GetInstance();
            //if (mPreference.UseDefaultTemplate)
            //    Template = new Aspose.Words.Document(new MemoryStream(Properties.Resources.校車繳費單樣版_新生));
            //else
            //    Template = new Aspose.Words.Document(mPreference.CustomizeTemplate);

            Document doc = new Document();

            Payment SelectPayment = cboPayment.SelectedItem as Payment;

            //這裡展示如何新增三筆收費明細。
            List<PaymentDetail> StudentDetails = new List<PaymentDetail>();

            SelectPayment.FillFull();
            List<PaymentDetail> Studentdetail = PaymentDetail.GetByTarget("NewStudent", NewStudent.Instance.SelectedList[0].UID);
            List<PaymentDetail> CurrentDetails = new List<PaymentDetail>();
            List<PaymentHistory> historys = new List<PaymentHistory>();
            List<string> ids = new List<string>();
            if (Studentdetail.Count == 0)
            {
                //return;
                PaymentDetail detail = new PaymentDetail(SelectPayment);    //設定「收費明細」所屬的收費。

                detail.RefTargetID = NewStudent.Instance.SelectedList[0].UID;//搭乘校車新生ID。
                detail.RefTargetType = "NewStudent";
                detail.Amount = CurrentStudentbus.BusMoney;                  //要收多少錢。
                detail.Extensions.Add("校車收費年度", SelectPayment.SchoolYear.ToString());
                detail.Extensions.Add("校車收費學期", SelectPayment.Semester.ToString());
                detail.Extensions.Add("校車收費名稱", cboBusRange.SelectedItem.ToString());
                detail.Extensions.Add("MergeField::代碼", textBusStop.Text);
                detail.Extensions.Add("MergeField::站名", CurrentBusStopName);
                detail.Extensions.Add("MergeField::" + "學號", NewStudent.Instance.SelectedList[0].Number);
                detail.Extensions.Add("MergeField::" + "姓名", NewStudent.Instance.SelectedList[0].Name);
                detail.Extensions.Add("MergeField::" + "科別", NewStudent.Instance.SelectedList[0].Dept);
                detail.Extensions.Add("開始日期", bussetup.BusStartDate.ToString());
                detail.Extensions.Add("結束日期", bussetup.BusEndDate.ToString());
                detail.Extensions.Add("搭車天數", CurrentStudentbus.DateCount.ToString());
                StudentDetails.Add(detail);                                 //先加到一個 List 中。
                List<string> detailIDs = new List<string>();
                detailIDs = PaymentDetail.Insert(StudentDetails.ToArray()); //新增到資料庫中。



                //如果要馬上使用物件的話，需要用回傳的 UID 清單再將資料 Select 出來
                //因為這樣物件中才會包含 UID 資料。
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
                            //pdetail.RefTargetID = NewStudent.Instance.SelectedList[0].UID;
                            //pdetail.RefTargetType = "NewStudent";
                            pdetail.Amount = CurrentStudentbus.BusMoney;
                            pdetail.Extensions["MergeField::代碼"] = textBusStop.Text;
                            pdetail.Extensions["MergeField::站名"] = CurrentBusStopName;
                            pdetails.Add(pdetail);
                            PaymentDetail.Update(pdetails.ToArray()); //更新到資料庫中。

                            //註銷先前之繳費單
                            //PaymentDetail.CancelExistReceipts(details);
                            pdetail.CancelExistReceipts();

                            //Payment.Balance();

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
                            //historys = pdetail.Histories as List<PaymentHistory>;
                            IList<PaymentHistory> historyss = pdetail.Histories;
                            if (historyss == null)
                                return;
                            foreach (PaymentHistory history in historyss)
                            {
                                //MotherForm.SetStatusBarMessage("條碼產生中....");
                                //Application.DoEvents();
                                //((Payment)cboPayment.SelectedItem).CalculateReceiptBarcode(history); //計算條碼資料，計算時需要「Payment」物件。
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
            foreach (PaymentHistory history in historys)
            {
                MotherForm.SetStatusBarMessage("繳費單產生中....");
                Application.DoEvents();
                doc = rdoc.Generate(history.Receipt);
                //mdoc.Sections.Add(mdoc.ImportNode(doc.Sections[0], true));
            }


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

        private void PaymentSheet_Load(object sender, EventArgs e)
        {
            List<MySchoolModule.NewStudentRecord> newstu = NewStudent.Instance.SelectedList;
            if (newstu.Count == 0)
            {
                MessageBox.Show("新生請選一人");
                this.Close();
                return;
            }

            if (newstu.Count > 1)
            {
                MessageBox.Show("新生一次請選一人");
                this.Close();
                return;
            }

            //this.intSchoolYear.Value = int.Parse(K12.Data.School.DefaultSchoolYear);
            //this.cboSemester.Text = K12.Data.School.DefaultSemester == "1" ? "上學期" : "下學期";

            switch (newstu[0].Dept)
            {
                case "普通科":
                    this.labelX8.Text = "學生：普" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "商業經營科":
                    this.labelX8.Text = "學生：商" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "國際貿易科":
                    this.labelX8.Text = "學生：貿" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "資料處理科":
                    this.labelX8.Text = "學生：資" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "應用外語科(英文)":
                    this.labelX8.Text = "學生：英" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "幼兒保育科":
                    this.labelX8.Text = "學生：幼" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "美容科":
                    this.labelX8.Text = "學生：美" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "廣告設計科":
                    this.labelX8.Text = "學生：廣" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "多媒體動畫科":
                    this.labelX8.Text = "學生：畫" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "多媒體設計科":
                    this.labelX8.Text = "學生：多" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "餐飲管理科":
                    this.labelX8.Text = "學生：餐" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
                case "觀光事業科":
                    this.labelX8.Text = "學生：觀" + (newstu[0].CheckNumber == "" ? newstu[0].Number : newstu[0].CheckNumber) + "-" + newstu[0].Name;
                    break;
            }

            this.intSchoolYear.Value = int.Parse(K12.Data.School.DefaultSchoolYear) + 1;
            this.cboSemester.Text = K12.Data.School.DefaultSemester == "2" ? "上學期" : "下學期";
            ////讀取系統中所有的收費模組(日期設定)。
            //List<Payment> pays = Payment.GetAll();
            //this.cboPayment.Items.Clear();
            //if (pays.Count > 0)
            //{
            //    foreach (Payment pay in pays)
            //        if (pay.SchoolYear == this.intSchoolYear.Value && pay.Semester == (this.cboSemester.Text == "上學期" ? 1 : 2))
            //            this.cboPayment.Items.Add(pay);
            //}
            this.cboBusRange.Items.Clear();
            //this.cboBusStopID.Items.Clear();
            List<BusSetup> bussetup = BusSetupDAO.SelectByBusYear(this.intSchoolYear.Value);
            foreach (BusSetup bus in bussetup)
                this.cboBusRange.Items.Add(bus.BusRangeName);
            if (this.cboBusRange.Items.Count > 0)
            {
                this.cboBusRange.SelectedIndex = 0;
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
                foreach (MySchoolModule.NewStudentRecord nsr in NewStudent.Instance.SelectedList)
                    studentbus = StudentByBusDAO.SelectByBusYearAndTimeNameAndStudntID(this.intSchoolYear.Value, cboBusRange.Text, nsr.UID);

                BusSetup bussetup = new BusSetup();
                if (BusSetupDAO.SelectByBusYear(this.intSchoolYear.Value).Count == 0)
                {
                    MessageBox.Show("此年度無校車紀錄，請重新選擇！");
                    return;
                }
                else
                    bussetup = BusSetupDAO.SelectByBusYearAndRange(this.intSchoolYear.Value, cboBusRange.Text);

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
                foreach (MySchoolModule.NewStudentRecord nsr in NewStudent.Instance.SelectedList)
                    studentbus = StudentByBusDAO.SelectByBusYearAndTimeNameAndStudntID(this.intSchoolYear.Value, cboBusRange.Text, nsr.UID);

                BusSetup bussetup = new BusSetup();
                if (BusSetupDAO.SelectByBusYear(this.intSchoolYear.Value).Count == 0)
                {
                    MessageBox.Show("此年度無校車紀錄，請重新選擇！");
                    return;
                }
                else
                    bussetup = BusSetupDAO.SelectByBusYearAndRange(this.intSchoolYear.Value, cboBusRange.Text);
                List<BusStop> busstops = BusStopDAO.SelectByBusTimeNameOrderByStopID(bussetup.BusTimeName);
                //Dictionary<string, BusStop> busstopRecord = new Dictionary<string, BusStop>();
                //this.cboBusStopID.Items.Clear();
                //foreach (BusStop busstop in busstops)
                //{
                //    this.cboBusStopID.Items.Add(busstop.BusStopID + "  " + busstop.BusStopName);
                //    busstopRecord.Add(busstop.BusStopID, busstop);
                //}

                if (studentbus == null)
                {
                }
                else
                {
                    BusStop stu_busstop = BusStopDAO.SelectByBusTimeNameAndByStopID(bussetup.BusTimeName, studentbus.BusStopID);
                    //if (busstopRecord.ContainsKey(stu_busstop.BusStopID))
                    //    this.cboBusStopID.SelectedIndex = busstopRecord.Keys.ToList().IndexOf(stu_busstop.BusStopID);
                    this.textBusStop.Text = stu_busstop.BusStopID;
                    this.textBusStopName.Text = stu_busstop.BusStopName;
                }

                //讀取系統中所有的收費模組(日期設定)。
                List<Payment> pays = Payment.GetAll();

                this.cboPayment.Items.Clear();
                if (pays.Count > 0)
                {
                    foreach (Payment pay in pays)
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

                        List<PaymentDetail> Studentdetail = PaymentDetail.GetByTarget("NewStudent", NewStudent.Instance.SelectedList[0].UID);
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

        private void cboBusStopID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboBusRange.Text != "")
            {
                if (this.cboBusStopID.Text != "")
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
        }

        private void intSchoolYear_ValueChanged(object sender, EventArgs e)
        {
            this.btnReport.Enabled = false;
            this.dtDueDate.Enabled = false;
            this.labelX3.Text = "";
            
            this.cboBusRange.Items.Clear();
            //this.cboBusStopID.Items.Clear();
            List<BusSetup> bussetup = BusSetupDAO.SelectByBusYear(this.intSchoolYear.Value);
            foreach (BusSetup bus in bussetup)
                this.cboBusRange.Items.Add(bus.BusRangeName);
            this.textBusStop.Text = "";
            this.textBusStopName.Text = "";
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.textBusStop.Text.Length == 4)
                this.btnReport.Enabled = true;
            else
                this.btnReport.Enabled = false;

            //this.cboBusStopID.Items.Clear();
            if (this.cboBusRange.Items.Count == 0)
            {
                this.labelX3.Text = "";
                this.textBusStop.Text = "";
                this.textBusStopName.Text = "";
            }
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

        //private BarcodeConfiguration GetBarcodeConfiguration()
        //{
        //    //intSchoolYear.Value = int.Parse(K12.Data.School.DefaultSchoolYear);
        //    //cboSemester.Text = K12.Data.School.DefaultSemester;

        //    ////讀取系統中所有的收費模組(日期設定)。
        //    //List<Payment> pays = Payment.GetAll();
        //    //cboPayment.Items.Clear();
        //    //if (pays.Count > 0)
        //    //{
        //    //    foreach (Payment pay in pays)
        //    //        if (pay.SchoolYear == intSchoolYear.Value && pay.Semester == (cboSemester.Text == "上學期" ? 1 : 2))
        //    //            cboPayment.Items.Add(pay);
        //    //}
        //    //if (cboPayment.Items.Count == 0)
        //    //{
        //    //    MessageBox.Show("請先設定收費模組");
        //    //    this.Close();
        //    //    return;
        //    //}
        //    //cboPayment.SelectedIndex = 0;
        //    //txtBank.Text = ((Payment)cboPayment.SelectedItem).BarcodeConfiguration.Name;
        //    //dtDueDate.Value = DateTime.Parse(((Payment)cboPayment.SelectedItem).Extensions["DefaultExpiration"]);
        //}
    }
}
