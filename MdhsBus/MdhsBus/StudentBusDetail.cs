using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation;
using FISCA.UDT;
using MdhsBus.Data;

namespace MdhsBus
{
    public partial class StudentBusDetail : DetailContent
    {
        public StudentBusDetail()
        {
            InitializeComponent();
            this.Group = "搭乘校車狀況";
            this.PrimaryKeyChanged += new EventHandler(StudentBusDetail_PrimaryKeyChanged);
            if (FISCA.Permission.UserAcl.Current["MdhsBusView"].Editable)
            {
                //this.addbutton.Visible = true;
                this.editbutton.Visible = true;
                this.delbutton.Visible = true;
            }
            else if (FISCA.Permission.UserAcl.Current["MdhsBusView"].Viewable)
            {
                //this.addbutton.Visible = false;
                this.editbutton.Visible = false;
                this.delbutton.Visible = false;
            }
        }

        private void StudentBusDetail_Load(object sender, EventArgs e)
        {
            ReloadStudentBus();
        }

        void StudentBusDetail_PrimaryKeyChanged(object sender, EventArgs e)
        {
            ReloadStudentBus();
        }

        private void ReloadStudentBus()
        {
            this.BusView.Items.Clear();
            if (this.PrimaryKey != "")
            {
                List<Data.StudentByBus> StuBuses = Data.StudentByBusDAO.SelectByStudntID(this.PrimaryKey);
                foreach (Data.StudentByBus var in StuBuses)
                {
                    ListViewItem lvi = new ListViewItem((var.UID == null) ? "" : var.UID);
                    lvi.SubItems.Add(var.SchoolYear.ToString());
                    lvi.SubItems.Add(var.BusRangeName);
                    lvi.SubItems.Add(var.BusTimeName);
                    lvi.SubItems.Add(var.BusStopID);
                    BusStop BusStopList = BusStopDAO.SelectByBusTimeNameAndByStopID(var.BusTimeName, var.BusStopID);
                    lvi.SubItems.Add(BusStopList.BusStopName);
                    lvi.SubItems.Add((var.PayStatus == true) ? "是" : "否");
                    lvi.SubItems.Add(var.PayDate.ToShortDateString() == "0001/1/1" ? "" : var.PayDate.ToShortDateString());
                    lvi.SubItems.Add(var.DateCount.ToString());
                    lvi.SubItems.Add(var.BusMoney.ToString());
                    lvi.Tag = var;

                    this.BusView.Items.Add(lvi);
                }
            }
        }

        private void addbutton_Click(object sender, EventArgs e)
        {

        }

        private void editbutton_Click(object sender, EventArgs e)
        {
            if (!FISCA.Permission.UserAcl.Current["MdhsBusView"].Editable)
                return;
        }

        private void delbutton_Click(object sender, EventArgs e)
        {
            if (!FISCA.Permission.UserAcl.Current["MdhsBusView"].Editable)
                return;

            if (this.BusView.SelectedItems.Count == 0)
            {
                MessageBox.Show("請選擇要刪除的紀錄！");
                return;
            }

            List<Data.StudentByBus> stubuses = new List<Data.StudentByBus>();
            Data.StudentByBus delitem0 = (Data.StudentByBus)this.BusView.SelectedItems[0].Tag;
            string delitem = delitem0.SchoolYear.ToString() + "年" + delitem0.BusRangeName;
            for (int i = 0; i < this.BusView.SelectedItems.Count; i++)
            {
                stubuses.Add((Data.StudentByBus)this.BusView.SelectedItems[i].Tag);
                if (i > 0)
                    delitem += "、" + stubuses[i].SchoolYear.ToString() + "年" + stubuses[i].BusRangeName;
            }

            if (MessageBox.Show("確定要刪除所選取的『" + delitem + "』 ?", "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                //刪除StudentByBus紀錄
                for (int i = 0; i < stubuses.Count; i++)
                    Data.StudentByBusDAO.Delete(stubuses[i]);

                //更新畫面上的資料
                ReloadStudentBus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("刪除搭校車紀錄失敗 \n" + ex.Message);
            }
        }
    }
}
