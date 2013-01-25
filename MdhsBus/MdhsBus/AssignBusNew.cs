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
using FISCA.Presentation;

namespace MdhsBus
{
    public partial class AssignBusNew : BaseForm
    {
        public AssignBusNew()
        {
            InitializeComponent();
        }

        private void AssignBusNew_Load(object sender, EventArgs e)
        {
            fillBuses();
        }

        private void fillBuses()
        {
            //this.addbtn.Enabled = false;
            this.cboBusTime.Items.Clear();
            this.cboBusTime.DisplayMember = "BusTimeName";
            this.cboBus.Items.Clear();
            this.cboBus.DisplayMember = "BusNumber";
            //this.BusStopView.Items.Clear();

            List<string> bustimes = new List<string>();
            //取得所有校車站名清單
            List<BusStop> busstop = BusStopDAO.GetSortByBusNumberList();

            foreach (BusStop var in busstop)
            {
                if (!bustimes.Contains(var.BusTimeName))
                    bustimes.Add(var.BusTimeName);
            }

            for (int i = 0; i < bustimes.Count; i++)
                this.cboBusTime.Items.Add(bustimes[i]);
        }

        private void cboBusTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboBus.Items.Clear();
            this.cboBus.Items.Add("全部");
            List<BusStop> busstops = new List<BusStop>();
            List<string> busnums = new List<string>();
            busstops = BusStopDAO.SelectByBusTimeName(cboBusTime.Text);
            foreach (BusStop var in busstops)
            {
                if (!busnums.Contains(var.BusNumber))
                    busnums.Add(var.BusNumber);
            }
            for (int i = 0; i < busnums.Count; i++)
                this.cboBus.Items.Add(busnums[i]);

            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            this.BusStopView.Rows.Clear();
            this.BusStopView.Refresh();
            List<string> BusStopIDs = new List<string>();
            List<BusStop> busstops = new List<BusStop>();
            Dictionary<string, List<BusStop>> busstopRecord = new Dictionary<string, List<BusStop>>();
            List<BusStop> ActivebusstopRecord = new List<BusStop>();
            int nowSet = 0;
            if (this.cboBus.Text != "全部")
            {
                busstops = BusStopDAO.SelectByBusTimeNameAndNumber(cboBusTime.Text, cboBus.Text);
                //BusStopIDs.Add(cls.ID);
            }
            else
            {
                busstops = BusStopDAO.SelectByBusTimeName(cboBusTime.Text);
                //for (int i = 1; i <= orderClasses.Count; i++)
                //    classIDs.Add(orderClasses[i].ID);
            }

            foreach (BusStop var in busstops)
            {
                MotherForm.SetStatusBarMessage("正在讀取校車資料", nowSet++ * 50 / busstops.Count);
                //if (var.Status.ToString() != "一般")
                //    continue;
                if (!busstopRecord.ContainsKey(var.BusNumber))
                    busstopRecord.Add(var.BusNumber, new List<BusStop>());
                busstopRecord[var.BusNumber].Add(var);
                ActivebusstopRecord.Add(var);
            }
            this.label4.Text = (cboBus.Text == "全部" ? "全部" : cboBus.Text + "車") + "：共" + ActivebusstopRecord.Count.ToString() + "站";

            nowSet = 0;
            int now_percent = 50;
            int ii = 0;

            nowSet = 0;
            foreach (BusStop var in ActivebusstopRecord)
            {
                MotherForm.SetStatusBarMessage("正在加入校車站名資料", 50 + nowSet++ * 50 / ActivebusstopRecord.Count);
                BusStopView.Rows.Add(var.BusStopID, var.BusStopName, var.BusMoney, var.BusNumber, var.BusStopNo, var.ComeTime, var.BusUpAddr, var.BusStopAddr, var.UID);
                //if (StudentBuses.ContainsKey(var.ID))
                //{
                //    //增加判斷是否含有校車代碼的站名
                //    BusStopView[2, ii].Value = StudentBuses[var.ID].BusStopID + " " + (busStopNames.ContainsKey(StudentBuses[var.ID].BusStopID) ? busStopNames[StudentBuses[var.ID].BusStopID].BusStopName : "");
                //    BusStopView[5, ii].Value = StudentBuses[var.ID].PayStatus;
                //    BusStopView[6, ii].Value = (StudentBuses[var.ID].PayDate.ToShortDateString() == "0001/1/1" ? "" : StudentBuses[var.ID].PayDate.ToShortDateString());
                //    BusStopView[7, ii].Value = StudentBuses[var.ID].DateCount;
                //    BusStopView[8, ii].Value = StudentBuses[var.ID].comment;
                //    BusStopView[10, ii].Value = StudentBuses[var.ID].BusMoney.ToString();
                //    BusStopView.Rows[ii].Tag = StudentBuses[var.ID];
                //}
                //else
                //{
                //    //BusStopView.Rows[BusStopView.Rows.Add(cls.Name, var.StudentNumber, "", var.Name, pr[ii].Cell == "" ? pr[ii].Contact : pr[ii].Cell, "", "", "", "")].Tag = var;
                //}
                ii++;
            }
            MotherForm.SetStatusBarMessage("資料讀取完成", 100);
            BusStopView.EndEdit();
            BusStopView.CancelEdit();
        }
    }
}
