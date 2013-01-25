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
using AccountsReceivable.API;
using FISCA.Presentation;

namespace MdhsBus
{
    public partial class NewStudentBus : BaseForm
    {
        public NewStudentBus()
        {
            InitializeComponent();
        }

        private void NewStudentBus_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;
            //RefreshDataGrid();
            this.cboClass.Enabled = false;
            this.cboYear.Items.Clear();
            this.cboYear.DisplayMember = "BusYear";
            this.cboRange.Items.Clear();
            this.cboRange.DisplayMember = "BusRange";
            this.cboClass.Items.Clear();
            this.cboClass.DisplayMember = "ClassName";
            this.StudentBusView.Items.Clear();
            if (this.cboYear.Text == "")
                this.pay_btn.Enabled = false;

            List<string> busyears = new List<string>();
            //取得所有校車設定清單
            List<BusSetup> bussetup = BusSetupDAO.GetSortByBusStartDateList();

            foreach (BusSetup var in bussetup)
            {
                if (!busyears.Contains(var.BusYear.ToString()))
                    busyears.Add(var.BusYear.ToString());
            }
            for (int i = 0; i < busyears.Count; i++)
                this.cboYear.Items.Add(busyears[i]);
        }

        private void RefreshDataGrid()
        {
            this.StudentBusView.Items.Clear();
            this.studentdataGridView.Rows.Clear();
            this.studentdataGridView.Refresh();
            AccessHelper udtHelper = new AccessHelper();
            List<NewStudentRecord> students = udtHelper.Select<NewStudentRecord>("學年度='" + this.cboYear.Text + "' And 科別='" + this.cboClass.Text + "'");
            this.label4.Text = "共" + students.Count.ToString() + "人";

            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(int.Parse(this.cboYear.Text), this.cboRange.Text);
            List<BusStop> buses = BusStopDAO.SelectByBusTimeNameOrderByStopID(bussetup.BusTimeName);

            int ii = 0;
            List<StudentByBus> _Source = StudentByBusDAO.SelectByBusYearAndTimeNameAndClass(int.Parse(this.cboYear.Text), this.cboRange.Text, this.cboClass.Text);
            Dictionary<string, StudentByBus> StudentBuses = new Dictionary<string, StudentByBus>();
            foreach (var item in _Source)
            {
                if (!StudentBuses.ContainsKey(item.StudentID))
                    StudentBuses.Add(item.StudentID, item);
            }
            Column3.Items.Clear();
            Dictionary<string, BusStop> busStopNames = new Dictionary<string, BusStop>();
            foreach (BusStop var in buses)
            {
                if (!Column3.Items.Contains(var.BusStopID + " " + var.BusStopName))
                    Column3.Items.Add(var.BusStopID + " " + var.BusStopName);
                if (!busStopNames.ContainsKey(var.BusStopID))
                    busStopNames.Add(var.BusStopID, var);
            }
            List<string> newStudentIDList = new List<string>();
            foreach (NewStudentRecord var in students)
                if (!newStudentIDList.Contains(var.UID))
                    newStudentIDList.Add(var.UID);
            List<PaymentDetail> Studentdetail = PaymentDetail.GetByTarget("NewStudent", newStudentIDList);
            Dictionary<string, PaymentDetail> StudentdetailRecord = new Dictionary<string, PaymentDetail>();
            foreach (PaymentDetail pdetail in Studentdetail)
            {
                if (!StudentdetailRecord.ContainsKey(pdetail.RefTargetID))
                    StudentdetailRecord.Add(pdetail.RefTargetID, pdetail);
            }

            foreach (NewStudentRecord var in students)
            {
                studentdataGridView.Rows.Add(this.cboClass.Text, var.Number, "", var.Name, var.Cellphone == "" ? var.Telephone : var.Cellphone, false, "", bussetup.DateCount, "", var.UID, "");
                if (StudentBuses.ContainsKey(var.UID))
                {
                    studentdataGridView[2, ii].Value = StudentBuses[var.UID].BusStopID + " " + busStopNames[StudentBuses[var.UID].BusStopID].BusStopName;
                    if (StudentdetailRecord.ContainsKey(var.UID))
                    {
                        studentdataGridView[5, ii].Value = (StudentdetailRecord[var.UID].TotalPaid == StudentdetailRecord[var.UID].Amount ? true : false);
                        StudentdetailRecord[var.UID].FillHistories();
                        foreach (PaymentHistory history in StudentdetailRecord[var.UID].Histories)
                        {
                            if (history.Receipt.Cancelled && history.PayDate == null)
                                continue;
                            studentdataGridView[5, ii].Value = (history.PayDate == null ? false : true);
                            studentdataGridView[6, ii].Value = (history.PayDate == null ? "" : DateTime.Parse(history.PayDate.ToString()).ToShortDateString());
                        }                        
                    }
                    else
                    {
                        studentdataGridView[5, ii].Value = StudentBuses[var.UID].PayStatus;
                        studentdataGridView[6, ii].Value = (StudentBuses[var.UID].PayDate.ToShortDateString() == "0001/1/1" ? "" : StudentBuses[var.UID].PayDate.ToShortDateString());
                    }
                    studentdataGridView[7, ii].Value = StudentBuses[var.UID].DateCount;
                    studentdataGridView[8, ii].Value = StudentBuses[var.UID].comment;
                    studentdataGridView[10, ii].Value = StudentBuses[var.UID].BusMoney.ToString();
                    studentdataGridView.Rows[ii].Tag = StudentBuses[var.UID];
                }
                else
                {
                    //studentdataGridView.Rows[studentdataGridView.Rows.Add(cls.Name, var.StudentNumber, "", var.Name, pr[ii].Cell == "" ? pr[ii].Contact : pr[ii].Cell, "", "", "", "")].Tag = var;
                }
                ii++;
            }

            studentdataGridView.EndEdit();
            studentdataGridView.CancelEdit();
        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            List<Data.StudentByBus> _Source = new List<Data.StudentByBus>();
            Data.StudentByBus sbr = new Data.StudentByBus();
            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(int.Parse(this.cboYear.Text), this.cboRange.Text);
            _Source.Clear();
            for (int i = studentdataGridView.Rows.Count - 1; i >= 0; i--)
            {
                sbr = (Data.StudentByBus)studentdataGridView.Rows[i].Tag;
                if (sbr == null && (studentdataGridView[2, i].Value == null || studentdataGridView[2, i].Value.ToString() == ""))
                    continue;
                if (sbr == null)
                    sbr = new Data.StudentByBus();
                sbr.StudentID = (string)studentdataGridView[9, i].Value;
                sbr.ClassName = (string)studentdataGridView[0, i].Value;
                sbr.BusStopID = studentdataGridView[2, i].Value.ToString().Substring(0, 4);
                BusStop buses = BusStopDAO.SelectByBusTimeNameAndByStopID(bussetup.BusTimeName, sbr.BusStopID);
                sbr.PayStatus = (bool)studentdataGridView[5, i].Value;
                DateTime PayDate;
                if (studentdataGridView[6, i].Value.ToString() != "" && DateTime.TryParse(studentdataGridView[6, i].Value.ToString(), out PayDate))
                    sbr.PayDate = DateTime.Parse(studentdataGridView[6, i].Value.ToString());
                sbr.DateCount = int.Parse(studentdataGridView[7, i].Value.ToString());
                int total = buses.BusMoney * sbr.DateCount;
                int div_value = total / 10;
                sbr.comment = (string)studentdataGridView[8, i].Value;
                sbr.BusRangeName = bussetup.BusRangeName;
                sbr.BusTimeName = bussetup.BusTimeName;
                if ((total - div_value * 10) < 5)
                    sbr.BusMoney = div_value * 10;
                else
                    sbr.BusMoney = div_value * 10 + 10;
                sbr.SchoolYear = int.Parse(this.cboYear.Text);

                _Source.Add(sbr);
            }
            _Source.SaveAll();
            RefreshDataGrid();
            MessageBox.Show("儲存成功！");
        }

        private void exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboRange.Items.Clear();
            this.studentdataGridView.Rows.Clear();
            List<string> busranges = new List<string>();
            //取得所有校車設定清單
            List<BusSetup> bussetup = BusSetupDAO.SelectByBusYear(int.Parse(this.cboYear.Text));

            foreach (BusSetup var in bussetup)
            {
                if (!busranges.Contains(var.BusRangeName))
                    busranges.Add(var.BusRangeName);
            }
            for (int i = 0; i < busranges.Count; i++)
                this.cboRange.Items.Add(busranges[i]);

            if (this.cboYear.Text == "" || this.cboRange.Text == "")
                this.pay_btn.Enabled = false;
            else
                this.pay_btn.Enabled = true;
        }

        private void cboRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboRange.Text != "")
                this.cboClass.Enabled = true;

            this.cboClass.Items.Clear();
            this.studentdataGridView.Rows.Clear();
            this.cboClass.DisplayMember = "Name";
            AccessHelper udtHelper = new AccessHelper();
            List<string> orderDept = new List<string>();
            //Dictionary<string, List<NewStudentRecord>> orderDeptStudnet = new Dictionary<string, List<NewStudentRecord>>();
            List<NewStudentRecord> students = udtHelper.Select<NewStudentRecord>("學年度='" + this.cboYear.Text + "'");
            foreach (NewStudentRecord newstu in students)
            {
                if (newstu.Active == false)
                    continue;
                else
                {
                    if (!orderDept.Contains(newstu.Dept))
                        orderDept.Add(newstu.Dept);
                    //if (!orderDeptStudnet.ContainsKey(newstu.Dept))
                    //    orderDeptStudnet.Add(newstu.Dept, new List<NewStudentRecord>());
                    //orderDeptStudnet[newstu.Dept].Add(newstu);
                }
            }
            //orderDept.Sort();
            orderDept.Sort(new Framework.StringComparer("普通科", "商業經營科", "國際貿易科", "資料處理科", "應用外語科", "幼兒保育科", "美容科", "廣告設計科", "多媒體設計科", "餐飲管理科", "觀光事業科"));

            for (int i = 0; i < orderDept.Count; i++)
                this.cboClass.Items.Add(orderDept[i]);

            if (this.cboYear.Text == "" || this.cboRange.Text == "")
                this.pay_btn.Enabled = false;
            else
                this.pay_btn.Enabled = true;
        }

        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDataGrid();
        }

        private void studentdataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            studentdataGridView[e.ColumnIndex, e.RowIndex].ErrorText = e.Exception.Message;
            studentdataGridView.UpdateCellErrorText(e.ColumnIndex, e.RowIndex);
            studentdataGridView[e.ColumnIndex, e.RowIndex].Value = null;
        }

        //依班級年級、序號排序副程式
        static int CompareClass(NewStudentRecord a, NewStudentRecord b)
        {
            //if (a.GradeYear == b.GradeYear)
            //    return int.Parse(a.DisplayOrder).CompareTo(int.Parse(b.DisplayOrder));
            //else
                return a.Dept.CompareTo(b.Dept);
        }

        private void pay_btn_Click(object sender, EventArgs e)
        {
            MotherForm.SetStatusBarMessage("校車對帳資料產生中....");
            #region 對帳
            Payment.Balance();
            #endregion

            AccessHelper udtHelper = new AccessHelper();
            List<NewStudentRecord> students = udtHelper.Select<NewStudentRecord>("學年度='" + this.cboYear.Text + "'");
            List<string> newStudentIDList = new List<string>();
            //List<string> newStudentNumberList = new List<string>();
            int nowSet = 0;
            foreach (NewStudentRecord var in students)
            {
                MotherForm.SetStatusBarMessage("正在讀取校車資料庫", nowSet++ * 10 / students.Count);
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
            Dictionary<string, StudentByBus> EveryStudentByBuses = new Dictionary<string,StudentByBus>();
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
                //if (pd.Extensions["MergeField::學號"] == "39002" || pd.Extensions["MergeField::學號"] == "39001")
                //    nowSet = nowSet;
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
                            //foreach (StudentByBus sbb in StudentByBuses)
                            //{
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
                            //}
                        }
                        else
                            continue;
                    }
                    else
                    {
                        if (DateTime.TryParse(history.PayDate.ToString(), out day))
                        {
                            //foreach (StudentByBus sbb in StudentByBuses)
                            //{
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
                            //}
                        }
                        else
                            continue;
                    }
                }
            }
            if (_Source.Count > 0)
                _Source.SaveAll();

            MotherForm.SetStatusBarMessage("校車對帳資料完成", 100);
            MessageBox.Show("已新增 " + _Source.Count + " 筆記錄，校車對帳資料完成！");
        }
    }
}
