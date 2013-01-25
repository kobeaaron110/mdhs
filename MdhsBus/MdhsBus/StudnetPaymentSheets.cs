using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using K12.Data;
using MdhsBus.Data;
using FISCA.Presentation;
using AccountsReceivable.API;
using Aspose.Words;
using System.IO;
using AccountsReceivable.ReceiptUtility;
using SHSchool.Data;

namespace MdhsBus
{
    public partial class StudnetPaymentSheets : BaseForm
    {
        public StudnetPaymentSheets()
        {
            InitializeComponent();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (cboDept.Text == "")
            {
                MessageBox.Show("請選擇科別再重新執行！");
                return;
            }
            List<StudentByBus> studentbus = new List<StudentByBus>();
            StudentByBus CurrentStudentbus = new StudentByBus();
            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(this.intSchoolYear.Value, cboBusRange.Text);
            //List<string> newStuNumberList = new List<string>();
            List<string> AllStudentIDList = new List<string>();
            List<string> StudentIDList = new List<string>();
            List<SHStudentRecord> stuRec = new List<SHStudentRecord>();
            Dictionary<string, StudentByBus> studentbusRecord = new Dictionary<string, StudentByBus>();
            Dictionary<string, List<SHStudentRecord>> studentClassRecord = new Dictionary<string, List<SHStudentRecord>>();
            studentbus = StudentByBusDAO.SelectByBusYearAndTimeName(this.intSchoolYear.Value, this.cboBusRange.Text);
            Dictionary<string, BusStop> BusStopRecord = new Dictionary<string, BusStop>();
            List<BusStop> BusStops = new List<BusStop>();
            if (this.cboBusRange.Text.IndexOf("夜輔") >= 0)
                BusStops = BusStopDAO.SelectByBusTimeName("夜輔");
            else
                BusStops = BusStopDAO.SelectByBusTimeName("日校校車");

            foreach (BusStop var in BusStops)
            {
                if (!BusStopRecord.ContainsKey(var.BusStopID))
                    BusStopRecord.Add(var.BusStopID, var);
            }

            int nowSet = 0;
            int nowPercent = 0;
            foreach (StudentByBus var in studentbus)
            {
                MotherForm.SetStatusBarMessage("正在讀取學生搭車資料", nowSet++ * 10 / studentbus.Count);
                if (var.BusStopID == "0000")
                    continue;
                //if (var.comment != "99測試")
                //    continue;

                if (!studentbusRecord.ContainsKey(var.StudentID))
                    studentbusRecord.Add(var.StudentID, var);
                if (!AllStudentIDList.Contains(var.StudentID))
                    AllStudentIDList.Add(var.StudentID);
            }

            stuRec = SHStudent.SelectByIDs(AllStudentIDList);
            Dictionary<int, string> ClassIDs = new Dictionary<int, string>();
            List<int> orderClassIDs = new List<int>();
            int orderval = 0;
            nowPercent = 10;
            nowSet = 0;
            foreach (SHStudentRecord var in stuRec)
            {
                MotherForm.SetStatusBarMessage("正在讀取學生資料", nowPercent + nowSet++ * 50 / stuRec.Count);
                if (var.Status != StudentRecord.StudentStatus.一般)
                    continue;
                if (cboDept.Text != "")
                {
                    if (cboDept.Text == "多媒體設計科")
                    {
                        if (var.Department.Name.IndexOf("多媒體") < 0)
                            continue;
                    }
                    else
                    {
                        if (var.Department.Name != cboDept.Text)
                            continue;
                    }
                }

                if (int.Parse(var.Class.GradeYear.ToString()) == 3)
                    orderval = 47 + int.Parse(var.Class.DisplayOrder);
                else
                    orderval = (int.Parse(var.Class.GradeYear.ToString()) - 1) * 23 + int.Parse(var.Class.DisplayOrder);
                if (!ClassIDs.ContainsKey(orderval))
                {
                    ClassIDs.Add(orderval, var.Class.ID);
                    orderClassIDs.Add(orderval);
                }

                if (!studentClassRecord.ContainsKey(var.Class.ID))
                    studentClassRecord.Add(var.Class.ID, new List<SHStudentRecord>());
                if (!studentClassRecord[var.Class.ID].Contains(var))
                    studentClassRecord[var.Class.ID].Add(var);
                if (!StudentIDList.Contains(var.ID))
                    StudentIDList.Add(var.ID);
            }

            orderClassIDs.Sort();
            int stucount = 0;
            for (int i = 0; i < orderClassIDs.Count; i++)
            {
                studentClassRecord[ClassIDs[orderClassIDs[i]]].Sort(CompareStudentNumber);
                stucount += studentClassRecord[ClassIDs[orderClassIDs[i]]].Count;
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

            int paymentcount = 0;
            nowPercent = 60;
            nowSet = 0;
            string oldclass = "";
            bool loseHistory = false;
            if (Studentdetail.Count == 0)
            {
                for (int i = 0; i < orderClassIDs.Count; i++)
                    foreach (SHStudentRecord nsr in studentClassRecord[ClassIDs[orderClassIDs[i]]])
                    {
                        MotherForm.SetStatusBarMessage("正在產生校車繳費資料", nowPercent + nowSet++ * 30 / stucount);
                        PaymentDetail detail = new PaymentDetail(SelectPayment);    //設定「收費明細」所屬的收費。

                        detail.RefTargetID = nsr.ID;                                //搭乘校車學生ID。
                        detail.RefTargetType = "Student";
                        detail.Amount = studentbusRecord.ContainsKey(nsr.ID) ? studentbusRecord[nsr.ID].BusMoney : 0;  //要收多少錢。
                        //detail.Extensions.Add("校車收費年度", intSchoolYear.Value.ToString());
                        detail.Extensions.Add("校車收費年度", SelectPayment.SchoolYear.ToString());
                        detail.Extensions.Add("校車收費學期", SelectPayment.Semester.ToString());
                        detail.Extensions.Add("校車收費名稱", cboBusRange.SelectedItem.ToString());
                        detail.Extensions.Add("MergeField::代碼", studentbusRecord.ContainsKey(nsr.ID) ? studentbusRecord[nsr.ID].BusStopID : "");
                        detail.Extensions.Add("MergeField::站名", studentbusRecord.ContainsKey(nsr.ID) ? BusStopRecord[studentbusRecord[nsr.ID].BusStopID].BusStopName : "");
                        detail.Extensions.Add("MergeField::" + "學號", nsr.StudentNumber);
                        detail.Extensions.Add("MergeField::" + "姓名", nsr.Name);
                        detail.Extensions.Add("MergeField::" + "班級", nsr.Class.Name.Substring(0, 3));
                        detail.Extensions.Add("開始日期", bussetup.BusStartDate.ToString());
                        detail.Extensions.Add("結束日期", bussetup.BusEndDate.ToString());
                        detail.Extensions.Add("搭車天數", studentbusRecord[nsr.ID].DateCount.ToString());
                        StudentDetails.Add(detail);                                 //先加到一個 List 中。
                        paymentcount++;
                    }
                List<string> detailIDs = new List<string>();
                detailIDs = PaymentDetail.Insert(StudentDetails.ToArray());       //新增到資料庫中。



                //如果要馬上使用物件的話，需要用回傳的 UID 清單再將資料 Select 出來
                //因為這樣物件中才會包含 UID 資料。
                CurrentDetails = PaymentDetail.GetByIDs(detailIDs.ToArray());
                foreach (PaymentDetail pd in CurrentDetails)
                {
                    if (oldclass != pd.Extensions["MergeField::" + "班級"])
                        MotherForm.SetStatusBarMessage(pd.Extensions["MergeField::" + "班級"] + "條碼產生中....", 90);
                    Application.DoEvents();
                    //新增一筆繳費記錄。
                    PaymentHistory history = new PaymentHistory(pd);
                    history.Amount = pd.Amount; //通常會與金額與繳費明細一樣。
                    ((Payment)cboPayment.SelectedItem).CalculateReceiptBarcode(history); //計算條碼資料，計算時需要「Payment」物件。
                    historys.Add(history);
                    oldclass = pd.Extensions["MergeField::" + "班級"];
                }
                ids = PaymentHistory.Insert(historys);
            }
            else
            {
                foreach (PaymentDetail pdetail in Studentdetail)
                {
                    if (paymentcount == 0)
                        MotherForm.SetStatusBarMessage("正在讀取校車繳費資料", nowPercent + nowSet++ * 30 / stucount);
                    CurrentDetails.Add(pdetail);
                    pdetail.FillHistories();
                    if (pdetail.Extensions["校車收費名稱"] == cboBusRange.SelectedItem.ToString() && int.Parse(pdetail.Extensions["校車收費年度"]) == intSchoolYear.Value)
                    {
                        if (CancelExist.Checked)
                        {
                            //註銷先前之繳費單
                            pdetail.CancelExistReceipts();

                            if (studentbusRecord[pdetail.RefTargetID].DateCount > 0)
                            {
                                pdetail.Extensions["搭車天數"] = studentbusRecord[pdetail.RefTargetID].DateCount.ToString();
                                pdetail.Amount = studentbusRecord.ContainsKey(pdetail.RefTargetID) ? studentbusRecord[pdetail.RefTargetID].BusMoney : 0;  //要收多少錢。
                            }

                            if (pdetail.Extensions["MergeField::" + "班級"].Length > 3)
                                pdetail.Extensions["MergeField::" + "班級"] = pdetail.Extensions["MergeField::" + "班級"].Substring(0, 3);
                            PaymentDetail.Update(pdetail);

                            if (oldclass != pdetail.Extensions["MergeField::" + "班級"])
                                MotherForm.SetStatusBarMessage(pdetail.Extensions["MergeField::" + "班級"] + "條碼產生中....", 90);
                            Application.DoEvents();
                            //新增一筆繳費記錄。
                            PaymentHistory history = new PaymentHistory(pdetail);
                            history.Amount = pdetail.Amount;                                        //通常會與金額與繳費明細一樣。
                            ((Payment)cboPayment.SelectedItem).CalculateReceiptBarcode(history);    //計算條碼資料，計算時需要「Payment」物件。
                            historys.Add(history);
                            paymentcount++;
                        }
                        else
                        {
                            //if (!pdetail.Extensions.ContainsKey("MergeField::站名"))
                            //    pdetail.Extensions.Add("MergeField::站名", BusStopRecord[studentbusRecord["19998"].BusStopID].BusStopName);
                            //PaymentDetail.Update(pdetail);

                            if (oldclass != pdetail.Extensions["MergeField::" + "班級"])
                                MotherForm.SetStatusBarMessage(pdetail.Extensions["MergeField::" + "班級"] + "條碼產生中....", 90);

                            IList<PaymentHistory> historyss = pdetail.Histories;

                            if (historyss == null)
                                return;

                            if (historyss.Count == 0)
                            {
                                Application.DoEvents();
                                //新增一筆繳費記錄。
                                PaymentHistory history = new PaymentHistory(pdetail);
                                history.Amount = pdetail.Amount; //通常會與金額與繳費明細一樣。
                                ((Payment)cboPayment.SelectedItem).CalculateReceiptBarcode(history); //計算條碼資料，計算時需要「Payment」物件。
                                historys.Add(history);
                                loseHistory = true;
                            }
                            else
                            {
                                foreach (PaymentHistory history in historyss)
                                {
                                    //if (!history.Receipt.Extensions.ContainsKey("MergeField::站名"))
                                    //    history.Receipt.Extensions.Add("MergeField::站名", BusStopRecord[studentbusRecord["19764"].BusStopID].BusStopName);
                                    //PaymentHistory.Update(history);

                                    ((Payment)cboPayment.SelectedItem).CalculateReceiptBarcode(history); //計算條碼資料，計算時需要「Payment」物件。
                                    ids.Add(history.UID);
                                }
                            }
                            paymentcount++;
                        }
                    }
                    oldclass = pdetail.Extensions["MergeField::" + "班級"];
                }
                if (CancelExist.Checked || (loseHistory && historys.Count > 0))
                    ids = PaymentHistory.Insert(historys);
            }


            historys = PaymentHistory.GetByIDs(ids.ToArray());
            Dictionary<string, Dictionary<string, PaymentHistory>> StudentHistorys = new Dictionary<string, Dictionary<string, PaymentHistory>>();
            Dictionary<string, List<string>> StudentNumberList = new Dictionary<string, List<string>>();
            foreach (PaymentHistory history in historys)
            {
                if (!StudentHistorys.ContainsKey(history.PaymentDetail.Extensions["MergeField::班級"]))
                    StudentHistorys.Add(history.PaymentDetail.Extensions["MergeField::班級"], new Dictionary<string, PaymentHistory>());
                if (!StudentHistorys[history.PaymentDetail.Extensions["MergeField::班級"]].ContainsKey(history.PaymentDetail.Extensions["MergeField::學號"]))
                    StudentHistorys[history.PaymentDetail.Extensions["MergeField::班級"]].Add(history.PaymentDetail.Extensions["MergeField::學號"], history);
                if (!StudentNumberList.ContainsKey(history.PaymentDetail.Extensions["MergeField::班級"]))
                    StudentNumberList.Add(history.PaymentDetail.Extensions["MergeField::班級"], new List<string>());
                if (!StudentNumberList[history.PaymentDetail.Extensions["MergeField::班級"]].Contains(history.PaymentDetail.Extensions["MergeField::學號"]))
                    StudentNumberList[history.PaymentDetail.Extensions["MergeField::班級"]].Add(history.PaymentDetail.Extensions["MergeField::學號"]);
            }
            foreach (string var in StudentNumberList.Keys)
                StudentNumberList[var].Sort();

            //產生繳費單
            ReceiptDocument rdoc = new ReceiptDocument(Template); //如果要自定樣版，可以用這個建構式。
            List<PaymentReceipt> prRecood = new List<PaymentReceipt>();
            nowPercent = 90;
            nowSet = 0;
            foreach (string var in StudentHistorys.Keys)
            {
                for (int i = 0; i < StudentNumberList[var].Count; i++)
                {
                    //MotherForm.SetStatusBarMessage("繳費單產生中....");
                    MotherForm.SetStatusBarMessage(var + "繳費單產生中....", nowPercent + nowSet++ * 10 / historys.Count);
                    Application.DoEvents();
                    prRecood.Add(StudentHistorys[var][StudentNumberList[var][i]].Receipt);
                }
                doc = rdoc.Generate2(prRecood);
                if (doc.Sections.Count != 0)
                {
                    Completed(doc, var);
                }
                prRecood.Clear();
            }


            MotherForm.SetStatusBarMessage("繳費單產生完成共" + paymentcount + "筆。");
            this.Close();

        }

        private void Completed(Document doc, string reportName)
        {
            string path = Path.Combine(Application.StartupPath, "Reports");
            string dirpath = path;
            //string reportName = "繳費單";
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
                System.Diagnostics.Process.Start(dirpath);
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

        private void StudnetPaymentSheets_Load(object sender, EventArgs e)
        {
            this.intSchoolYear.Value = int.Parse(K12.Data.School.DefaultSchoolYear);
            this.cboSemester.Text = K12.Data.School.DefaultSemester == "1" ? "上學期" : "下學期";

            this.cboBusRange.Items.Clear();
            //this.cboBusStopID.Items.Clear();
            List<BusSetup> bussetup = BusSetupDAO.SelectByBusYear(this.intSchoolYear.Value);
            foreach (BusSetup bus in bussetup)
            {
                if (Payment.GetByID(bus.BusPaymentName).SchoolYear == this.intSchoolYear.Value && Payment.GetByID(bus.BusPaymentName).Semester == (this.cboSemester.Text == "上學期" ? 1 : 2))
                    this.cboBusRange.Items.Add(bus.BusRangeName);
            }

            this.cboDept.Items.Clear();
            List<string> Kinds = new List<string>(new string[] { "普通科", "商業經營科", "國際貿易科", "資料處理科", "應用外語科(英文)", "幼兒保育科", "美容科", "廣告設計科", "多媒體設計科", "餐飲管理科", "觀光事業科" });
            foreach (string var in Kinds)
                this.cboDept.Items.Add(var);

            if (this.cboBusRange.Items.Count > 0)
                this.cboBusRange.SelectedIndex = this.cboBusRange.Items.Count - 1;
            else
            {
                this.textBusStop.Text = "";
                this.textBusStopName.Text = "";
            }
        }

        private void cboBusRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region cboBusRange異動
            this.labelX3.Text = "";
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
                }
            }

            if (cboBusRange.Text != "")
            {
                this.btnReport.Enabled = true;
                this.dtDueDate.Enabled = false;
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
            this.btnReport.Enabled = true;

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

        //依學生學號排序副程式
        static int CompareStudentNumber(SHStudentRecord a, SHStudentRecord b)
        {
            return a.StudentNumber.CompareTo(b.StudentNumber);
        }
    }
}
