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
using MySchoolModule;
using AccountsReceivable.API;
using AccountsReceivable.ReceiptUtility;
using System.IO;
using Aspose.Words;
using FISCA.Presentation;

namespace MdhsBus
{
    public partial class PaymentSheets : BaseForm
    {
        private TemplatePreference mPreference;

        public PaymentSheets()
        {
            InitializeComponent();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            List<StudentByBus> studentbus = new List<StudentByBus>();
            StudentByBus CurrentStudentbus = new StudentByBus();
            string CurrentBusStopName = "";
            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(this.intSchoolYear.Value, cboBusRange.Text);
            //List<string> newStuNumberList = new List<string>();
            List<string> newStudentIDList = new List<string>();
            List<MySchoolModule.NewStudentRecord> newStu = new List<MySchoolModule.NewStudentRecord>();
            Dictionary<string, StudentByBus> studentbusRecord = new Dictionary<string, StudentByBus>();
            foreach (MySchoolModule.NewStudentRecord nsr in NewStudent.Instance.SelectedList)
            {
                //if (!newStuNumberList.Contains(nsr.Number))
                //    newStuNumberList.Add(nsr.Number);
                if (!newStudentIDList.Contains(nsr.UID))
                    newStudentIDList.Add(nsr.UID);
                newStu.Add(nsr);
            }

            studentbus = StudentByBusDAO.SelectByBusYearAndTimeNameAndStudntList(this.intSchoolYear.Value, cboBusRange.Text, newStudentIDList);
            foreach (StudentByBus var in studentbus)
                if (!studentbusRecord.ContainsKey(var.StudentID))
                    studentbusRecord.Add(var.StudentID, var);
            


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

            //新增收費明細。
            List<PaymentDetail> StudentDetails = new List<PaymentDetail>();

            SelectPayment.FillFull();
            List<PaymentDetail> Studentdetail = PaymentDetail.GetByTarget("NewStudent", newStudentIDList);
            List<PaymentDetail> CurrentDetails = new List<PaymentDetail>();
            List<PaymentHistory> historys = new List<PaymentHistory>();
            List<string> ids = new List<string>();
            if (Studentdetail.Count == 0)
            {
                foreach (MySchoolModule.NewStudentRecord nsr in NewStudent.Instance.SelectedList)
                {
                    PaymentDetail detail = new PaymentDetail(SelectPayment);    //設定「收費明細」所屬的收費。

                    detail.RefTargetID = NewStudent.Instance.SelectedList[0].UID;//搭乘校車新生ID。
                    detail.RefTargetType = "NewStudent";
                    detail.Amount = studentbusRecord[nsr.UID].BusMoney;                  //要收多少錢。
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
                    detail.Extensions.Add("搭車天數", studentbusRecord[nsr.UID].DateCount.ToString());
                    StudentDetails.Add(detail);                                 //先加到一個 List 中。
                }
                List<string> detailIDs = new List<string>();
                detailIDs = PaymentDetail.Insert(StudentDetails.ToArray());     //新增到資料庫中。



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
                                pdetail.Amount = studentbusRecord.ContainsKey(pdetail.RefTargetID) ? studentbusRecord[pdetail.RefTargetID].BusMoney : 0;
                                PaymentDetail.Update(pdetail);
                            }

                            MotherForm.SetStatusBarMessage("條碼產生中....");
                            Application.DoEvents();
                            //新增一筆繳費記錄。
                            PaymentHistory history = new PaymentHistory(pdetail);
                            history.Amount = pdetail.Amount;                                        //通常會與金額與繳費明細一樣。
                            ((Payment)cboPayment.SelectedItem).CalculateReceiptBarcode(history);    //計算條碼資料，計算時需要「Payment」物件。
                            historys.Add(history);
                        }
                        else
                        {
                            IList<PaymentHistory> historyss = pdetail.Histories;

                            if (historyss == null)
                                return;
                            foreach (PaymentHistory history in historyss)
                            {
                                ((Payment)cboPayment.SelectedItem).CalculateReceiptBarcode(history); //計算條碼資料，計算時需要「Payment」物件。
                                ids.Add(history.UID);
                            }
                        }
                    }
                }
                if (CancelExist.Checked)
                    ids = PaymentHistory.Insert(historys);
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
            doc = rdoc.Generate1(prRecood);


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


            this.intSchoolYear.Value = int.Parse(K12.Data.School.DefaultSchoolYear) + 1;
            this.cboSemester.Text = K12.Data.School.DefaultSemester == "2" ? "上學期" : "下學期";

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
            this.btnReport.Enabled = true;

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
