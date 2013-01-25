using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using K12.Data;
using MdhsBus.Data;

namespace MdhsBus
{
    public partial class BusMoneyMitigate : BaseForm
    {
        public BusMoneyMitigate()
        {
            InitializeComponent();
        }

        private void BusMoneyMitigate_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;
            
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
                if (var.BusTimeName.IndexOf("夜輔") == 0)
                    continue;
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

            List<BusSetup> bussetups = BusSetupDAO.SelectByBusYear(int.Parse(this.cboYear.Text));
            BusSetup bussetup = new BusSetup();
            List<StudentByBus> _Source = StudentByBusDAO.SelectByBusYearAndTimeName(int.Parse(this.cboYear.Text), this.cboRange.Text);
            Dictionary<string, StudentByBus> studentids = new Dictionary<string, StudentByBus>();
            //找到所選校車撘乘之資料
            foreach (StudentByBus item in _Source)
            {
                if (!studentids.ContainsKey(item.StudentID))
                    studentids.Add(item.StudentID, item);
            }
            //找到夜輔設定之資料
            bussetups.Reverse();
            foreach (BusSetup item in bussetups)
            {
                if (item.BusTimeName == "夜輔")
                {
                    bussetup = item;
                    continue;
                }
            }
            if (bussetup.BusRangeName == null)
            {
                MessageBox.Show("夜輔無資料，請確認後重新輸入！");
                return;
            }

            List<StudentByBus> Source_night = StudentByBusDAO.SelectByBusYearAndTimeName(int.Parse(this.cboYear.Text), bussetup.BusRangeName);
            List<StudentByBus> StudentBuses = new List<StudentByBus>();
            foreach (StudentByBus item in Source_night)
            {
                if (studentids.ContainsKey(item.StudentID))
                {
                    //item.BusMoney = 0;
                    StudentBuses.Add(item);
                }
            }

            if (StudentBuses.Count > 0)
            {
                //StudentBuses.SaveAll();
                MessageBox.Show("夜輔共" + StudentBuses.Count + "筆儲存成功！");
            }
            else
                MessageBox.Show("無任何符合條件夜輔！");
        }
    }
}
