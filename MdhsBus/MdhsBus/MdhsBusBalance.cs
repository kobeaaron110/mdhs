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
using FISCA.UDT;
using FISCA.Presentation;
using AccountsReceivable.API;
using K12.Data;

namespace MdhsBus
{
    public partial class MdhsBusBalance : BaseForm
    {
        public MdhsBusBalance()
        {
            InitializeComponent();
        }

        private void MdhsBusBalance_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;
            this.cboYear.Items.Clear();
            this.cboYear.DisplayMember = "BusYear";
            this.cboRange.Items.Clear();
            this.cboRange.DisplayMember = "BusRange";
            if (this.cboYear.Text == "")
                this.pay_btn.Enabled = false;

            List<string> busyears = new List<string>();
            //取得所有校車在時效內設定清單
            List<BusSetup> bussetup = BusSetupDAO.GetSortByBusStartDateList();

            foreach (BusSetup var in bussetup)
            {
                if (DateTime.Today.Year > var.PayEndDate.Year)
                {
                    if (DateTime.Today.Year - var.PayEndDate.Year > 1)
                        continue;
                    else
                    {
                        if (DateTime.Today.DayOfYear < var.PayEndDate.DayOfYear)
                            if (DateTime.Today.DayOfYear + 365 - var.PayEndDate.DayOfYear > 31)
                                continue;
                    }
                }
                if (DateTime.Today.DayOfYear > var.PayEndDate.DayOfYear)
                    if (DateTime.Today.DayOfYear - var.PayEndDate.DayOfYear > 31)
                        continue;
                if (!busyears.Contains(var.BusYear.ToString()))
                    busyears.Add(var.BusYear.ToString());
            }
            for (int i = 0; i < busyears.Count; i++)
                this.cboYear.Items.Add(busyears[i]);
        }

        private void cboStudentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboStudentType.Text != "新生" && this.cboStudentType.Text != "一般學生")
            {
                MessageBox.Show("學生類別有誤，請重新選擇！");
                return;
            }

            this.cboYear.Items.Clear();
            this.cboYear.DisplayMember = "BusYear";
            this.cboRange.Items.Clear();
            this.cboRange.DisplayMember = "BusRange";
            if (this.cboYear.Text == "")
                this.pay_btn.Enabled = false;

            List<string> busyears = new List<string>();
            //取得所有校車在時效內設定清單
            List<BusSetup> bussetup = BusSetupDAO.GetSortByBusStartDateList();

            foreach (BusSetup var in bussetup)
            {
                if (DateTime.Today.Year > var.PayEndDate.Year)
                {
                    if (DateTime.Today.Year - var.PayEndDate.Year > 1)
                        continue;
                    else
                    {
                        if (DateTime.Today.DayOfYear < var.PayEndDate.DayOfYear)
                            if (DateTime.Today.DayOfYear + 365 - var.PayEndDate.DayOfYear > 31)
                                continue;
                    }
                }
                if (DateTime.Today.DayOfYear > var.PayEndDate.DayOfYear)
                    if (DateTime.Today.DayOfYear - var.PayEndDate.DayOfYear > 31)
                        continue;
                if (!busyears.Contains(var.BusYear.ToString()))
                    busyears.Add(var.BusYear.ToString());
            }
            for (int i = 0; i < busyears.Count; i++)
                this.cboYear.Items.Add(busyears[i]);
            this.cboYear.Text = "";
            this.cboRange.Text = "";

            if (this.cboStudentType.Text == "" || this.cboYear.Text == "" || this.cboRange.Text == "")
                this.pay_btn.Enabled = false;
            else
                this.pay_btn.Enabled = true;
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboRange.Items.Clear();
            this.cboRange.Text = "";
            List<string> busranges = new List<string>();
            //取得單一年度校車在時效內設定清單
            List<BusSetup> bussetup = BusSetupDAO.SelectByBusYear(int.Parse(this.cboYear.Text));

            foreach (BusSetup var in bussetup)
            {
                if (DateTime.Today.Year > var.PayEndDate.Year)
                {
                    if (DateTime.Today.Year - var.PayEndDate.Year > 1)
                        continue;
                    else
                    {
                        if (DateTime.Today.DayOfYear < var.PayEndDate.DayOfYear)
                            if (DateTime.Today.DayOfYear + 365 - var.PayEndDate.DayOfYear > 31)
                                continue;
                    }
                }
                if (DateTime.Today.DayOfYear > var.PayEndDate.DayOfYear)
                    if (DateTime.Today.DayOfYear - var.PayEndDate.DayOfYear > 31)
                        continue;
                if (!busranges.Contains(var.BusRangeName))
                    busranges.Add(var.BusRangeName);
            }
            for (int i = 0; i < busranges.Count; i++)
            {
                if (this.cboStudentType.Text == "新生")
                    if (busranges[i].IndexOf("9") < 0)
                        continue;
                this.cboRange.Items.Add(busranges[i]);
            }

            if (this.cboStudentType.Text == "" || this.cboYear.Text == "" || this.cboRange.Text == "")
                this.pay_btn.Enabled = false;
            else
                this.pay_btn.Enabled = true;
        }

        private void cboRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboStudentType.Text == "" || this.cboYear.Text == "" || this.cboRange.Text == "")
                this.pay_btn.Enabled = false;
            else
                this.pay_btn.Enabled = true;
        }

        private void pay_btn_Click(object sender, EventArgs e)
        {
            MotherForm.SetStatusBarMessage("校車對帳資料產生中....");

            //List<PaymentTransaction> PaymentTransactions = PaymentTransaction.GetByStatus(TransactionStatus.New);
            //Dictionary<string, PaymentTransaction> PaymentTransactionsByID = new Dictionary<string, PaymentTransaction>();
            //foreach (PaymentTransaction var in PaymentTransactions)
            //    if (!PaymentTransactionsByID.ContainsKey(var.UID))
            //        PaymentTransactionsByID.Add(var.UID, var);

            #region 對帳
            Payment.Balance();
            #endregion

            if (this.cboStudentType.Text == "新生")
                NewStudentBusBalance();
            else
                StudentBusBalance();
        }

        private void StudentBusBalance()
        {
            List<StudentRecord> students = K12.Data.Student.SelectAll();
            //Dictionary<string, string> classStudents = new Dictionary<string, string>();
            List<string> StudentIDList = new List<string>();
            int nowSet = 0;
            foreach (StudentRecord var in students)
            {
                MotherForm.SetStatusBarMessage("正在讀取學生資料", nowSet++ * 10 / students.Count);
                if (var.Status != StudentRecord.StudentStatus.一般)
                    continue;
                if (var.Class == null)
                    continue;
                if (var.Class.Name == "轉學班")
                    continue;
                if (!StudentIDList.Contains(var.ID))
                    StudentIDList.Add(var.ID);
                //if (!classStudents.ContainsKey(var.ID))
                //    classStudents.Add(var.ID, var.Class.Name);
            }

            List<PaymentDetail> Studentdetails = PaymentDetail.GetByTarget("Student", StudentIDList);
            List<StudentByBus> _Source = new List<StudentByBus>();
            List<StudentByBus> StudentByBuses = StudentByBusDAO.SelectByBusYearAndTimeNameAndStudntList(int.Parse(this.cboYear.Text), this.cboRange.Text, StudentIDList);
            Dictionary<string, StudentByBus> EveryStudentByBuses = new Dictionary<string, StudentByBus>();
            nowSet = 0;
            foreach (StudentByBus var in StudentByBuses)
            {
                MotherForm.SetStatusBarMessage("正在讀取校車資料庫", 10 + nowSet++ * 40 / students.Count);
                if (!EveryStudentByBuses.ContainsKey(var.StudentID))
                    EveryStudentByBuses.Add(var.StudentID, var);
            }

            DateTime day;

            nowSet = 0;
            int Cancelled_cnt = 0;
            foreach (PaymentDetail pd in Studentdetails)
            {
                MotherForm.SetStatusBarMessage("正在更新校車資料庫", 50 + nowSet++ * 50 / Studentdetails.Count);
                //List<StudentByBus> StudentByBuses = StudentByBusDAO.SelectByStudntID(pd.Extensions["MergeField::學號"]);

                if (!EveryStudentByBuses.ContainsKey(pd.RefTargetID))
                    continue;

                StudentByBus sbb = EveryStudentByBuses[pd.RefTargetID];
                if (sbb.PayStatus == true)
                    continue;

                pd.FillHistories();
                IList<PaymentHistory> historyss = pd.Histories;
                foreach (PaymentHistory history in historyss)
                {
                    if (history.Receipt.Cancelled)
                    {
                        if (DateTime.TryParse(history.PayDate.ToString(), out day))
                        {
                            if (sbb.SchoolYear == int.Parse(pd.Extensions["校車收費年度"]) && sbb.BusRangeName == pd.Extensions["校車收費名稱"])
                            {
                                if (sbb.comment != "改單：原為" + history.Receipt.Extensions["MergeField::代碼"] + "　" + history.Receipt.Extensions["MergeField::站名"] + "，金額為" + history.Receipt.Amount)
                                {
                                    sbb.PayStatus = true;
                                    sbb.PayDate = DateTime.Parse(history.PayDate.ToString());
                                    sbb.comment += "改單：原為" + history.Receipt.Extensions["MergeField::代碼"] + "　" + history.Receipt.Extensions["MergeField::站名"] + "，金額為" + history.Receipt.Amount;
                                    _Source.Add(sbb);
                                    Cancelled_cnt++;
                                }
                                break;
                            }
                        }
                        else
                            continue;
                    }
                    else
                    {
                        if (DateTime.TryParse(history.PayDate.ToString(), out day))
                        {
                            if (sbb.SchoolYear == int.Parse(pd.Extensions["校車收費年度"]) && sbb.BusRangeName == pd.Extensions["校車收費名稱"])
                            {
                                if (sbb.PayStatus == false || sbb.PayDate != DateTime.Parse(history.PayDate.ToString()))
                                {
                                    sbb.PayStatus = true;
                                    sbb.PayDate = DateTime.Parse(history.PayDate.ToString());
                                    if (history.Receipt.PaidAmount != sbb.BusMoney)
                                        sbb.comment = "繳費金額為" + history.Receipt.PaidAmount.ToString();
                                    _Source.Add(sbb);
                                }
                                break;
                            }
                        }
                        else
                            continue;
                    }
                }
            }
            if (_Source.Count > 0)
                _Source.SaveAll();

            MotherForm.SetStatusBarMessage("校車(一般學生)對帳資料完成", 100);
            MessageBox.Show("已新增 " + _Source.Count + " 筆記錄，其中改單 " + Cancelled_cnt + " 筆記錄，校車對帳資料完成！");
        }

        private void NewStudentBusBalance()
        {
            //List<string> PaymentTransactionIDs=new List<string>();
            //foreach (string var in PaymentTransactionsByID.Keys)
            //{
            //    if (!PaymentTransactionIDs.Contains(var))
            //        PaymentTransactionIDs.Add(var);
            //}
            //List<PaymentTransaction> PaymentTransactions = PaymentTransaction.GetByIDs(PaymentTransactionIDs);


            AccessHelper udtHelper = new AccessHelper();
            List<NewStudentRecord> students = udtHelper.Select<NewStudentRecord>("學年度='" + this.cboYear.Text + "'");
            List<string> newStudentIDList = new List<string>();
            //List<string> newStudentNumberList = new List<string>();
            int nowSet = 0;
            foreach (NewStudentRecord var in students)
            {
                MotherForm.SetStatusBarMessage("正在讀取新生資料庫", nowSet++ * 10 / students.Count);
                if (!newStudentIDList.Contains(var.UID))
                {
                    newStudentIDList.Add(var.UID);
                    //newStudentNumberList.Add(var.Number);
                }
            }

            List<PaymentDetail> Studentdetails = PaymentDetail.GetByTarget("NewStudent", newStudentIDList);
            //List<PaymentDetail> Studentdetails = PaymentDetail.GetAll();
            List<StudentByBus> _Source = new List<StudentByBus>();
            List<StudentByBus> StudentByBuses = StudentByBusDAO.SelectByBusYearAndTimeNameAndStudntList(int.Parse(this.cboYear.Text), this.cboRange.Text, newStudentIDList);
            Dictionary<string, StudentByBus> EveryStudentByBuses = new Dictionary<string, StudentByBus>();
            nowSet = 0;
            foreach (StudentByBus var in StudentByBuses)
            {
                MotherForm.SetStatusBarMessage("正在讀取校車資料庫", 10 + nowSet++ * 40 / StudentByBuses.Count);
                if (!EveryStudentByBuses.ContainsKey(var.StudentID))
                    EveryStudentByBuses.Add(var.StudentID, var);
            }

            DateTime day;

            nowSet = 0;

            foreach (PaymentDetail pd in Studentdetails)
            {
                MotherForm.SetStatusBarMessage("正在更新校車資料庫", 50 + nowSet++ * 50 / Studentdetails.Count);
                //List<StudentByBus> StudentByBuses = StudentByBusDAO.SelectByStudntID(pd.Extensions["MergeField::學號"]);

                if (!EveryStudentByBuses.ContainsKey(pd.RefTargetID))
                    continue;

                StudentByBus sbb = EveryStudentByBuses[pd.RefTargetID];
                if (sbb.PayStatus == true)
                    continue;

                pd.FillHistories();
                IList<PaymentHistory> historyss = pd.Histories;
                foreach (PaymentHistory history in historyss)
                {
                    if (history.Receipt.Cancelled)
                    {
                        if (DateTime.TryParse(history.PayDate.ToString(), out day))
                        {
                            if (sbb.SchoolYear == int.Parse(pd.Extensions["校車收費年度"]) && sbb.BusRangeName == pd.Extensions["校車收費名稱"])
                            {
                                if (sbb.comment != "改單：原為" + history.Receipt.Extensions["MergeField::代碼"] + "　" + history.Receipt.Extensions["MergeField::站名"] + "，金額為" + history.Receipt.Amount)
                                {
                                    sbb.PayStatus = true;
                                    sbb.PayDate = DateTime.Parse(history.PayDate.ToString());
                                    sbb.comment += "改單：原為" + history.Receipt.Extensions["MergeField::代碼"] + "　" + history.Receipt.Extensions["MergeField::站名"] + "，金額為" + history.Receipt.Amount;
                                    _Source.Add(sbb);
                                }
                                break;
                            }
                        }
                        else
                            continue;
                    }
                    else
                    {
                        if (DateTime.TryParse(history.PayDate.ToString(), out day))
                        {
                            if (sbb.SchoolYear == int.Parse(pd.Extensions["校車收費年度"]) && sbb.BusRangeName == pd.Extensions["校車收費名稱"])
                            {
                                if (sbb.PayStatus == false || sbb.PayDate != DateTime.Parse(history.PayDate.ToString()))
                                {
                                    sbb.PayStatus = true;
                                    sbb.PayDate = DateTime.Parse(history.PayDate.ToString());
                                    if (history.Receipt.PaidAmount != sbb.BusMoney)
                                        sbb.comment = "繳費金額為" + history.Receipt.PaidAmount.ToString();
                                    _Source.Add(sbb);
                                }
                                break;
                            }
                        }
                        else
                            continue;
                    }
                }
            }
            if (_Source.Count > 0)
                _Source.SaveAll();

            MotherForm.SetStatusBarMessage("校車(新生)對帳資料完成", 100);
            MessageBox.Show("已新增 " + _Source.Count + " 筆記錄，校車對帳資料完成！");
        }
    }
}
