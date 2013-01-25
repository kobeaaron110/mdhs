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
using K12.Data;

namespace MdhsBus
{
    public partial class NewStudentTransfer : BaseForm
    {
        public NewStudentTransfer()
        {
            InitializeComponent();
        }

        private void NewStudentTransfer_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;
            //RefreshDataGrid();
            this.cboYear.Items.Clear();
            this.cboYear.DisplayMember = "BusYear";
            this.cboRange.Items.Clear();
            this.cboRange.DisplayMember = "BusRange";

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

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboRange.Items.Clear();
            this.cboRange.Text = "";
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

        private void savebtn_Click(object sender, EventArgs e)
        {
            if (this.cboYear.Text == "" || this.cboRange.Text == "")
            {
                MessageBox.Show("搭車年度與期間名稱不可為空！");
                return;
            }

            AccessHelper udtHelper = new AccessHelper();
            Dictionary<string, List<NewStudentRecord>> NewStudnets = new Dictionary<string, List<NewStudentRecord>>();
            List<NewStudentRecord> students = udtHelper.Select<NewStudentRecord>("學年度='" + this.cboYear.Text + "'");
            Dictionary<string, NewStudentRecord> NewStudents = new Dictionary<string, NewStudentRecord>();
            foreach (NewStudentRecord var in students)
            {
                if (!NewStudents.ContainsKey(var.UID))
                    NewStudents.Add(var.UID, var);
            }
            List<ClassRecord> cls = K12.Data.Class.SelectAll();
            List<string> clsid = new List<string>();
            foreach (ClassRecord var in cls)
            {
                if (var.GradeYear.ToString() == null)
                    continue;
                if (var.Name.Substring(0, 2) == "夜輔" || var.Name.Substring(0, 2) == "轉學")
                    continue;
                if (!clsid.Contains(var.ID))
                    clsid.Add(var.ID);
            }
            List<StudentRecord> studentclass = K12.Data.Student.SelectByClassIDs(clsid);
            Dictionary<string, StudentRecord> studentids = new Dictionary<string, StudentRecord>();
            foreach (StudentRecord var in studentclass)
            {
                if (var.Status.ToString() != "一般")
                    continue;
                if (!studentids.ContainsKey(var.IDNumber))
                    studentids.Add(var.IDNumber, var);
            }

            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(int.Parse(this.cboYear.Text), this.cboRange.Text);
            //List<BusStop> buses = BusStopDAO.SelectByBusTimeNameOrderByStopID(bussetup.BusTimeName);
            List<StudentByBus> _Source = StudentByBusDAO.SelectByBusYearAndTimeName(int.Parse(this.cboYear.Text), this.cboRange.Text);
            //轉換後之資料
            List<StudentByBus> StudentBuses = new List<StudentByBus>();
            foreach (var item in _Source)
            {
                if (NewStudents.ContainsKey(item.StudentID))
                {
                    if (studentids.ContainsKey(NewStudents[item.StudentID].IDNumber))
                    {
                        item.StudentID = studentids[NewStudents[item.StudentID].IDNumber].ID;
                        item.ClassName = studentids[NewStudents[item.StudentID].IDNumber].Class.Name;
                        StudentBuses.Add(item);
                    }
                }
            }

            if (StudentBuses.Count > 0)
            {
                StudentBuses.SaveAll();
                MessageBox.Show("新生共" + StudentBuses.Count + "筆儲存成功！");
            }
            else
                MessageBox.Show("無任何符合條件新生！");
        }
    }
}
