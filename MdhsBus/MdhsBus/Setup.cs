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

namespace MdhsBus
{
    public partial class Setup : BaseForm
    {
        public Setup()
        {
            InitializeComponent();
        }

        private void Setup_Load(object sender, EventArgs e)
        {
            fillBusSetup();
        }

        private void fillBusSetup()
        {
            this.addbtn.Enabled = false;
            this.cboYear.Items.Clear();
            this.cboYear.DisplayMember = "BusYear";
            this.BusSetupView.Items.Clear();

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

        private void reloadData()
        {
            this.BusSetupView.Items.Clear();
            //取得所有校車設定清單
            List<BusSetup> bussetup = new List<BusSetup>();
            if (this.cboYear.Text != "")
                bussetup = BusSetupDAO.SelectByBusYear(int.Parse(this.cboYear.Text));

            foreach (BusSetup var in bussetup)
            {
                ListViewItem lvi = new ListViewItem((var.UID == null) ? "" : var.UID);
                lvi.SubItems.Add(var.BusRangeName);
                lvi.SubItems.Add(var.BusStartDate.ToShortDateString());
                lvi.SubItems.Add(var.BusEndDate.ToShortDateString());
                lvi.SubItems.Add(var.PayEndDate.ToShortDateString());
                lvi.SubItems.Add(var.DateCount.ToString());
                lvi.SubItems.Add(var.BusTimeName);
                lvi.Tag = var;
                //把設定資訊填入ListView中
                this.BusSetupView.Items.Add(lvi);
            }

        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            //取得被選取的校車設定物件
            BusSetup bus = new BusSetup();
            bus.BusYear = int.Parse(this.cboYear.Text);
            BusSetupDetail frm = new BusSetupDetail();
            frm.SetBus(bus);
            frm.ShowDialog();
            this.reloadData();
        }

        private void editbtn_Click(object sender, EventArgs e)
        {
            if (this.BusSetupView.SelectedItems.Count == 0)
            {
                MessageBox.Show("請選擇要修改的校車設定！");
                return;
            }
            //取得被選取的校車設定物件
            BusSetup bus = (BusSetup)this.BusSetupView.SelectedItems[0].Tag;

            BusSetupDetail frm = new BusSetupDetail();
            frm.SetBus(bus);
            frm.ShowDialog();
            this.reloadData();
        }

        private void delbtn_Click(object sender, EventArgs e)
        {
            if (this.BusSetupView.SelectedItems.Count == 0)
            {
                MessageBox.Show("請選擇要刪除的校車設定！");
                return;
            }

            for (int i = 0; i < this.BusSetupView.SelectedItems.Count; i++)
            {
                BusSetup busitem = (BusSetup)this.BusSetupView.SelectedItems[i].Tag;
                List<StudentByBus> StudentBuses = StudentByBusDAO.SelectByBusYearAndTimeName(int.Parse(this.cboYear.Text), busitem.BusRangeName);
                if (StudentBuses.Count > 0)
                {
                    MessageBox.Show("選擇的『" + busitem.BusRangeName + "』校車設定有搭乘學生資料，不可刪除！");
                    return;
                }
            }

            //取得被選取的校車物件
            List<BusSetup> selectedBus = new List<BusSetup>();
            BusSetup delitem0 = (BusSetup)this.BusSetupView.SelectedItems[0].Tag;
            string delitem = delitem0.BusRangeName;
            for (int i = 0; i < this.BusSetupView.SelectedItems.Count; i++)
            {
                selectedBus.Add((BusSetup)this.BusSetupView.SelectedItems[i].Tag);
                if (i > 0)
                    delitem += "、" + selectedBus[i].BusRangeName;
            }

            if (MessageBox.Show("確定要刪除所選取的『" + delitem + "』 ?", "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                //刪除校車紀錄
                for (int i = 0; i < selectedBus.Count; i++)
                    BusSetupDAO.Delete(selectedBus[i]);

                //更新畫面上的校車紀錄資料
                this.reloadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("刪除校車站名失敗 \n" + ex.Message);
            }
        }

        private void exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.addbtn.Enabled = true;
            this.BusSetupView.Items.Clear();
            //取得所有校車設定清單
            List<BusSetup> bussetup = BusSetupDAO.SelectByBusYear(int.Parse(this.cboYear.Text));

            foreach (BusSetup var in bussetup)
            {
                ListViewItem lvi = new ListViewItem((var.UID == null) ? "" : var.UID);
                lvi.SubItems.Add(var.BusRangeName);
                lvi.SubItems.Add(var.BusStartDate.ToShortDateString());
                lvi.SubItems.Add(var.BusEndDate.ToShortDateString());
                lvi.SubItems.Add(var.PayEndDate.ToShortDateString());
                lvi.SubItems.Add(var.DateCount.ToString());
                lvi.SubItems.Add(var.BusTimeName);
                lvi.Tag = var;
                //把設定資訊填入ListView中
                this.BusSetupView.Items.Add(lvi);
            }
        }
    }
}
