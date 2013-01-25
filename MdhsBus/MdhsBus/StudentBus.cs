using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation;
using FISCA.Presentation.Controls;
using K12.Data;
using MdhsBus.Data;

namespace MdhsBus
{
    public partial class StudentBus : BaseForm
    {
        public StudentBus()
        {
            InitializeComponent();
        }

        private void StudentBus_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;
            //RefreshDataGrid();
            this.cboClass.Enabled = false;
            this.savebtn.Enabled = false;
            this.Paymentbtn.Enabled = false;
            this.cboYear.Items.Clear();
            this.cboYear.DisplayMember = "BusYear";
            this.cboRange.Items.Clear();
            this.cboRange.DisplayMember = "BusRange";
            this.cboClass.Items.Clear();
            this.cboClass.DisplayMember = "ClassName";
            this.StudentBusView.Items.Clear();

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
            List<string> classIDs = new List<string>();
            Dictionary<int, ClassRecord> orderClasses = new Dictionary<int, ClassRecord>();
            int nowSet = 0;
            if (this.cboClass.Text != "全部班級")
            {
                ClassRecord cls = new ClassRecord();
                cls = (ClassRecord)this.cboClass.SelectedItem;
                classIDs.Add(cls.ID);
            }
            else
            {
                List<ClassRecord> classes = K12.Data.Class.SelectAll();
                int orderval = 0;
                foreach (ClassRecord var in classes)
                {
                    if (var.GradeYear == null)
                        continue;
                    else if (var.GradeYear > 3)
                        continue;
                    else if (var.Name.IndexOf("夜輔") >= 0 || var.Name.IndexOf("轉學") >= 0 || var.Name.IndexOf("選修") >= 0)
                        continue;
                    else
                    {
                        if (int.Parse(var.GradeYear.ToString()) == 3)
                            orderval = 47 + int.Parse(var.DisplayOrder);
                        else
                            orderval = (int.Parse(var.GradeYear.ToString()) - 1) * 23 + int.Parse(var.DisplayOrder);
                        if (!orderClasses.ContainsKey(orderval))
                            orderClasses.Add(orderval, new ClassRecord());
                        orderClasses[orderval] = var;
                    }
                }
                for (int i = 1; i <= orderClasses.Count; i++)
                    classIDs.Add(orderClasses[i].ID);
            }

            List<StudentRecord> studentclass = K12.Data.Student.SelectByClassIDs(classIDs);
            List<StudentRecord> Activestudentclass = new List<StudentRecord>();
            List<string> studentids = new List<string>();
            foreach (StudentRecord var in studentclass)
            {
                MotherForm.SetStatusBarMessage("正在讀取學生資料", nowSet++ * 20 / studentclass.Count);
                if (var.Status.ToString() != "一般")
                    continue;
                if (!studentids.Contains(var.ID))
                {
                    studentids.Add(var.ID);
                    Activestudentclass.Add(var);
                }
            }
            this.label4.Text = "共" + Activestudentclass.Count.ToString() + "人";
            List<PhoneRecord> pr = Phone.SelectByStudentIDs(studentids);
            Dictionary<string, PhoneRecord> Studentpr = new Dictionary<string, PhoneRecord>();

            nowSet = 0;
            foreach (PhoneRecord var in pr)
            {
                MotherForm.SetStatusBarMessage("正在讀取電話資料", 20 + nowSet++ * 10 / pr.Count);
                if (!Studentpr.ContainsKey(var.RefStudentID))
                    Studentpr.Add(var.RefStudentID, var);
            }
            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(int.Parse(this.cboYear.Text), this.cboRange.Text);
            List<BusStop> buses = BusStopDAO.SelectByBusTimeNameOrderByStopID(bussetup.BusTimeName);

            int ii = 0;
            nowSet = 0;
            int now_percent = 30;
            int will_percent = 65;
            //原由班級查詢搭校車狀況，改為由學生編號查詢
            //List<StudentByBus> _Source = StudentByBusDAO.SelectByBusYearAndTimeNameAndClassID(int.Parse(this.cboYear.Text), this.cboRange.Text, cls.ID);
            List<StudentByBus> _Source = StudentByBusDAO.SelectByBusYearAndTimeNameAndStudntList(int.Parse(this.cboYear.Text), this.cboRange.Text, studentids);
            Dictionary<string, StudentByBus> StudentBuses = new Dictionary<string, StudentByBus>();
            if (_Source != null)
            {
                foreach (var item in _Source)
                {
                    MotherForm.SetStatusBarMessage("正在讀取學生乘車資料", 30 + nowSet++ * 30 / _Source.Count);
                    if (!StudentBuses.ContainsKey(item.StudentID))
                        StudentBuses.Add(item.StudentID, item);
                }
                now_percent = 60;
                will_percent = 35;
            }
            Column3.Items.Clear();
            Dictionary<string, BusStop> busStopNames = new Dictionary<string, BusStop>();
            nowSet = 0;
            foreach (BusStop var in buses)
            {
                MotherForm.SetStatusBarMessage("正在讀取校車資料", now_percent + nowSet++ * will_percent / buses.Count);
                if (!Column3.Items.Contains(var.BusStopID + " " + var.BusStopName))
                    Column3.Items.Add(var.BusStopID + " " + var.BusStopName);
                if (!busStopNames.ContainsKey(var.BusStopID))
                    busStopNames.Add(var.BusStopID, var);
            }

            nowSet = 0;
            foreach (StudentRecord var in Activestudentclass)
            {
                MotherForm.SetStatusBarMessage("正在加入學生乘車資料", 95 + nowSet++ * 5 / Activestudentclass.Count);
                studentdataGridView.Rows.Add(var.Class.Name, var.StudentNumber, "", var.Name, Studentpr[var.ID].Cell == "" ? Studentpr[var.ID].Contact : Studentpr[var.ID].Cell, false, "", bussetup.DateCount, "", var.ID, "", var.Class.ID);
                if (StudentBuses.ContainsKey(var.ID))
                {
                    //studentdataGridView.Rows[studentdataGridView.Rows.Add(cls.Name, var.StudentNumber, StudentBuses[var.ID].BusStopID, var.Name, pr[ii].Cell == "" ? pr[ii].Contact : pr[ii].Cell, StudentBuses[var.ID].PayStatus, StudentBuses[var.ID].PayDate, StudentBuses[var.ID].DateCount, StudentBuses[var.ID].comment)].Tag = StudentBuses[var.ID];
                    //增加判斷是否含有校車代碼的站名
                    studentdataGridView[2, ii].Value = StudentBuses[var.ID].BusStopID + " " + (busStopNames.ContainsKey(StudentBuses[var.ID].BusStopID) ? busStopNames[StudentBuses[var.ID].BusStopID].BusStopName : "");
                    studentdataGridView[5, ii].Value = StudentBuses[var.ID].PayStatus;
                    studentdataGridView[6, ii].Value = (StudentBuses[var.ID].PayDate.ToShortDateString() == "0001/1/1" ? "" : StudentBuses[var.ID].PayDate.ToShortDateString());
                    studentdataGridView[7, ii].Value = StudentBuses[var.ID].DateCount;
                    studentdataGridView[8, ii].Value = StudentBuses[var.ID].comment;
                    studentdataGridView[10, ii].Value = StudentBuses[var.ID].BusMoney.ToString();
                    studentdataGridView.Rows[ii].Tag = StudentBuses[var.ID];
                }
                else
                {
                    //studentdataGridView.Rows[studentdataGridView.Rows.Add(cls.Name, var.StudentNumber, "", var.Name, pr[ii].Cell == "" ? pr[ii].Contact : pr[ii].Cell, "", "", "", "")].Tag = var;
                }
                //studentdataGridView[0, ii].Value = cls.Name;
                //studentdataGridView[1, ii].Value = var.StudentNumber;
                //studentdataGridView[3, ii].Value = var.Name;
                //studentdataGridView[4, ii].Value = pr[ii].Cell == "" ? pr[ii].Contact : pr[ii].Cell;
                //ListViewItem lvi = new ListViewItem((var.ID == null) ? "" : var.ID);
                //lvi.SubItems.Add(cls.Name);
                //lvi.SubItems.Add(var.StudentNumber);
                //lvi.SubItems.Add("");
                //lvi.SubItems.Add(var.Name);
                //lvi.SubItems.Add(pr.Cell == "" ? pr.Contact : pr.Cell);
                //lvi.SubItems.Add("");
                //lvi.SubItems.Add("");
                //lvi.SubItems.Add("");
                //lvi.SubItems.Add("");
                //lvi.Tag = var;
                ////把設定資訊填入ListView中
                //this.StudentBusView.Items.Add(lvi);
                ii++;
            }
            //List<StudentByBus> _Source = StudentByBusDAO.SelectByBusYearAndTimeNameAndClass(int.Parse(this.cboYear.Text), this.cboRange.Text, this.cboClass.Text);
            //studentdataGridView.Rows.Clear();
            //foreach (var item in _Source)
            //{
            //    studentdataGridView.Rows[studentdataGridView.Rows.Add(item.UID, item.PayStatus, item.PayDate, item.comment, item.Deleted)].Tag = item;
            //}
            MotherForm.SetStatusBarMessage("資料讀取完成", 100);
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
                sbr.ClassID = (string)studentdataGridView[11, i].Value;
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

        private void Paymentbtn_Click(object sender, EventArgs e)
        {
            int stu_cnt=0;
            PaymentSheets_ext();
            MessageBox.Show("產生繳費單共" + stu_cnt + "筆成功！");
        }

        private void PaymentSheets_ext()
        {
            
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
        }

        private void cboRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboRange.Text != "")
                this.cboClass.Enabled = true;

            this.cboClass.Items.Clear();
            this.studentdataGridView.Rows.Clear();
            this.cboClass.DisplayMember = "Name";
            Dictionary<int, ClassRecord> orderClasses = new Dictionary<int, ClassRecord>();
            List<ClassRecord> classes = K12.Data.Class.SelectAll();
            int orderval = 0;
            classes.Sort(CompareClass);
            foreach (ClassRecord cls in classes)
            {
                if (cls.GradeYear == null)
                    continue;
                else if (cls.GradeYear > 3)
                    continue;
                else if (cls.Name.IndexOf("夜輔") >= 0 || cls.Name.IndexOf("轉學") >= 0 || cls.Name.IndexOf("選修") >= 0)
                    continue;
                else
                {
                    if (int.Parse(cls.GradeYear.ToString()) == 3)
                        orderval = 47 + int.Parse(cls.DisplayOrder);
                    else
                        orderval = (int.Parse(cls.GradeYear.ToString()) - 1) * 23 + int.Parse(cls.DisplayOrder);
                    if (!orderClasses.ContainsKey(orderval))
                        orderClasses.Add(orderval, new ClassRecord());
                    orderClasses[orderval] = cls;
                }
            }

            for (int i = 1; i <= orderClasses.Count; i++)
                this.cboClass.Items.Add(orderClasses[i]);
            //this.cboClass.Items.Add("全部班級");
        }

        private void cboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDataGrid();
            if (cboYear.Text != "" && cboRange.Text != "")
            {
                this.savebtn.Enabled = true;
                this.Paymentbtn.Enabled = false;
            }
            else
            {
                this.savebtn.Enabled = false;
                this.Paymentbtn.Enabled = false;
            }
        }

        private void studentdataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            studentdataGridView[e.ColumnIndex, e.RowIndex].ErrorText = e.Exception.Message;
            studentdataGridView.UpdateCellErrorText(e.ColumnIndex, e.RowIndex);
            studentdataGridView[e.ColumnIndex, e.RowIndex].Value = null;
        }

        //依班級年級、序號排序副程式
        static int CompareClass(ClassRecord a, ClassRecord b)
        {
            if (a.GradeYear == b.GradeYear)
                return int.Parse(a.DisplayOrder).CompareTo(int.Parse(b.DisplayOrder));
            else
                return a.GradeYear.ToString().CompareTo(b.GradeYear.ToString());
        }
    }
}
