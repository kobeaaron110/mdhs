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
    public partial class AssignBus : BaseForm
    {
        public AssignBus()
        {
            InitializeComponent();
        }

        private void AssignBus_Load(object sender, EventArgs e)
        {
            fillBuses();
        }

        private void fillBuses()
        {
            this.addbtn.Enabled = false;
            this.cboBusTime.Items.Clear();
            this.cboBusTime.DisplayMember = "BusTimeName";
            this.cboBus.Items.Clear();
            this.cboBus.DisplayMember = "BusNumber";
            this.BusStopView.Items.Clear();

            //List<BusRoute> routes = BusRouteDAO.GetAll();
            //foreach (BusRoute route in routes)
            //{
            //    this.BusStopView.Items.Add(route);
            //}
            List<string> bustimes = new List<string>();
            //取得所有校車站名清單
            List<BusStop> busstop = BusStopDAO.GetSortByBusNumberList();

            foreach (BusStop var in busstop)
            {
                if (!bustimes.Contains(var.BusTimeName))
                    bustimes.Add(var.BusTimeName);
                //ListViewItem lvi = new ListViewItem((var.UID == null) ? "" : var.UID);
                //lvi.SubItems.Add(var.BusStopID.ToString());
                //lvi.SubItems.Add(var.BusNumber);
                //lvi.SubItems.Add(var.BusMoney.ToString());
                //lvi.SubItems.Add(var.BusStopNo);
                //lvi.SubItems.Add(var.BusStopName);
                //lvi.Tag = var;
                ////把站名資訊填入ListView中
                //this.BusStopView.Items.Add(lvi);
            }

            for (int i = 0; i < bustimes.Count; i++)
                this.cboBusTime.Items.Add(bustimes[i]);
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            if (this.cboBusTime.Text == "" || this.cboBus.Text == "")
            {
                MessageBox.Show("校車時段或校車車號不可為空，請重新選擇！");
                return;
            }
            else
            {
                //取得被選取的校車站名物件
                BusStop bus = new BusStop();
                bus.BusTimeName = this.cboBusTime.Text;
                bus.BusNumber = this.cboBus.Text;

                //開啟畫面
                BusDetailForm frm = new BusDetailForm();
                frm.SetBus(bus);
                frm.ShowDialog();
                this.reloadData();
            }
        }

        private void editbtn_Click(object sender, EventArgs e)
        {
            if (this.BusStopView.SelectedItems.Count == 0)
            {
                MessageBox.Show("請選擇要修改的校車站名！");
                return;
            }

            //取得被選取的校車站名物件
            BusStop bus = (BusStop)this.BusStopView.SelectedItems[0].Tag;

            //開啟畫面
            BusDetailForm frm = new BusDetailForm();
            frm.SetBus(bus);
            frm.ShowDialog();
            this.reloadData();
        }

        private void delbtn_Click(object sender, EventArgs e)
        {
            if (this.BusStopView.SelectedItems.Count == 0)
            {
                MessageBox.Show("請選擇要刪除的校車站名！");
                return;
            }

            //取得被選取的校車物件
            List<BusStop> selectedBus = new List<BusStop>();
            BusStop delitem0 = (BusStop)this.BusStopView.SelectedItems[0].Tag;
            string delitem = delitem0.BusStopID;
            for (int i = 0; i < this.BusStopView.SelectedItems.Count; i++)
            {
                selectedBus.Add((BusStop)this.BusStopView.SelectedItems[i].Tag);
                if (i > 0)
                    delitem += "、" + selectedBus[i].BusStopID;
            }

            if (MessageBox.Show("確定要刪除所選取的校車電腦代號『" + delitem + "』 ?", "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                //刪除校車紀錄
                for (int i = 0; i < selectedBus.Count; i++)
                    BusStopDAO.Delete(selectedBus[i]);

                //更新畫面上的社團資料
                this.reloadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("刪除校車站名失敗 \n" + ex.Message);
            }
        }

        private void reloadData()
        {
            this.BusStopView.Items.Clear();
            //取得所有校車站名清單
            List<BusStop> busstop = new List<BusStop>();
            if (this.cboBusTime.Text == "" && this.cboBus.Text == "")
                busstop = BusStopDAO.GetSortByBusNumberList();
            else if (this.cboBusTime.Text != "" && this.cboBus.Text == "")
                busstop = BusStopDAO.SelectByBusTimeName(this.cboBusTime.Text);
            else if (this.cboBusTime.Text != "" && this.cboBus.Text != "")
                busstop = BusStopDAO.SelectByBusTimeNameAndNumber(this.cboBusTime.Text, this.cboBus.Text);

            foreach (BusStop var in busstop)
            {
                ListViewItem lvi = new ListViewItem((var.UID == null) ? "" : var.UID);
                lvi.SubItems.Add(var.BusStopID.ToString());
                lvi.SubItems.Add(var.BusNumber);
                lvi.SubItems.Add(var.BusMoney.ToString());
                lvi.SubItems.Add(var.BusStopNo);
                lvi.SubItems.Add(var.BusStopName);
                lvi.SubItems.Add(var.BusStopAddr);
                lvi.Tag = var;
                //把站名資訊填入ListView中
                this.BusStopView.Items.Add(lvi);
            }
        }

        private void exitbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboBusTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.cboBusTime.Text == "")
            //{
            //    MessageBox.Show("校車時段不可為空，請重新選擇！");
            //    return;
            //}
            //else
            //{
                this.BusStopView.Items.Clear();
                this.cboBus.Items.Clear();
                this.cboBus.Text = "";
                this.addbtn.Enabled = false;
                //取得所有校車站名清單
                List<BusStop> busstop = BusStopDAO.SelectByBusTimeName(this.cboBusTime.Text);
                List<string> busnum = new List<string>();

                foreach (BusStop var in busstop)
                {
                    if (!busnum.Contains(var.BusNumber))
                        busnum.Add(var.BusNumber);
                    ListViewItem lvi = new ListViewItem((var.UID == null) ? "" : var.UID);
                    lvi.SubItems.Add(var.BusStopID.ToString());
                    lvi.SubItems.Add(var.BusNumber);
                    lvi.SubItems.Add(var.BusMoney.ToString());
                    lvi.SubItems.Add(var.BusStopNo);
                    lvi.SubItems.Add(var.BusStopName);
                    lvi.SubItems.Add(var.BusStopAddr);
                    lvi.Tag = var;
                    //把站名資訊填入ListView中
                    this.BusStopView.Items.Add(lvi);
                }

                for (int i = 0; i < busnum.Count; i++)
                    this.cboBus.Items.Add(busnum[i]);
            //}
        }

        private void cboBus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BusStopView.Items.Clear();
            //取得所有校車站名清單
            List<BusStop> busstop = new List<BusStop>();
            if (this.cboBus.Text == "")
                busstop = BusStopDAO.SelectByBusTimeName(this.cboBusTime.Text);
            else
            {
                busstop = BusStopDAO.SelectByBusTimeNameAndNumber(this.cboBusTime.Text, this.cboBus.Text);
                this.addbtn.Enabled = true;
            }

            foreach (BusStop var in busstop)
            {
                ListViewItem lvi = new ListViewItem((var.UID == null) ? "" : var.UID);
                lvi.SubItems.Add(var.BusStopID.ToString());
                lvi.SubItems.Add(var.BusNumber);
                lvi.SubItems.Add(var.BusMoney.ToString());
                lvi.SubItems.Add(var.BusStopNo);
                lvi.SubItems.Add(var.BusStopName);
                lvi.SubItems.Add(var.BusStopAddr);
                lvi.Tag = var;
                //把站名資訊填入ListView中
                this.BusStopView.Items.Add(lvi);
            }
        }
    }
}
