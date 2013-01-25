using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;

namespace MdhsBus
{
    public partial class BusDetailForm : BaseForm
    {
        private AssignBus _masterForm;        //做 call back , refresh 畫面用的
        private Data.BusStop _bus;                  //若要修改某個站名，就會把該站名物件傳過來。
        private string thisBusStopNo;

        public BusDetailForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 若要修改某個站名，就會把該站名物件傳進來。
        /// </summary>
        /// <param name="bus"></param>
        public void SetBus(Data.BusStop bus)
        {
            this._bus = bus;
        }

        /// <summary>
        /// 設定可以call back 的表單
        /// </summary>
        /// <param name="masterForm"></param>
        public void SetMasterForm(AssignBus masterForm)
        {
            this._masterForm = masterForm;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool repeat = false;
            int dollars = 0;
            //if (this._bus != null)
            if (thisBusStopNo != null)
            {
                this._bus.BusStopID = this.txtID.Text;
                this._bus.BusStopName = this.txtBusName.Text;
                if (this.txtMoney.Text == "")
                {
                    MessageBox.Show("金額不可為空，請重新輸入！");
                    repeat = true;
                }
                else if (!int.TryParse(this.txtMoney.Text, out dollars))
                {
                    MessageBox.Show("金額需為數字，請重新輸入！");
                    repeat = true;
                }
                else
                    this._bus.BusMoney = int.Parse(this.txtMoney.Text);
                this._bus.BusNumber = this.txtBusNumber.Text;
                if (this.nrBusStopNo.Value.ToString().Length == 2)
                    this._bus.BusStopNo = this._bus.BusNumber + this.nrBusStopNo.Value.ToString();
                else if (this.nrBusStopNo.Value.ToString().Length == 1)
                    this._bus.BusStopNo = this._bus.BusNumber + "0" + this.nrBusStopNo.Value.ToString();
                this._bus.BusStopAddr = this.txtBusStopAddr.Text;
                this._bus.BusUpAddr = this.cboUpAddr.Text;
                this._bus.ComeTime = this.txtCometime.Text;
                //this._bus.BusTimeName="夜輔";
                List<Data.BusStop> bustmp = Data.BusStopDAO.SelectByBusTimeNameAndNumber(this._bus.BusTimeName, this._bus.BusNumber);
                if (!repeat)
                {
                    if (this._bus.BusStopID == "" || this._bus.BusStopName == "" || this._bus.BusNumber == "")
                    {
                        MessageBox.Show("電腦代號、車號、站名名稱不可為空，請重新輸入！");
                        repeat = true;
                    }
                    else if (this._bus.BusMoney < 0)
                    {
                        MessageBox.Show("金額不可小於0，請重新輸入！");
                        repeat = true;
                    }
                    else
                    {
                        if (thisBusStopNo != this._bus.BusStopNo)
                        {
                            if (this._bus.BusStopNo.Substring(0, 2) != this._bus.BusNumber)
                            {
                                MessageBox.Show(this._bus.BusNumber + "號車與站序前兩碼不同，請重新輸入！");
                                repeat = true;
                            }
                            //foreach (Data.BusStop busrec in bustmp)
                            //{
                            //    if (this._bus.BusStopNo == busrec.BusStopNo && this._bus.BusStopName != busrec.BusStopName)
                            //    {
                            //        MessageBox.Show("『" + this._bus.BusStopName + "』站名或『" + this._bus.BusStopNo + "』站序重複，請重新輸入！");
                            //        repeat = true;
                            //        break;
                            //    }
                            //}
                            string new_busstopno = "";
                            if (this._bus.BusStopNo.Substring(0, 2) == thisBusStopNo.Substring(0, 2) && !repeat)
                            {
                                if (int.Parse(thisBusStopNo.Substring(2, 2)) < int.Parse(this._bus.BusStopNo.Substring(2, 2)))
                                {
                                    foreach (Data.BusStop busrec in bustmp)
                                    {
                                        if (int.Parse(busrec.BusStopNo.Substring(2, 2)) <= int.Parse(this._bus.BusStopNo.Substring(2, 2)) && int.Parse(busrec.BusStopNo.Substring(2, 2)) > int.Parse(thisBusStopNo.Substring(2, 2)) && busrec.BusStopNo != thisBusStopNo)
                                        {
                                            new_busstopno = (int.Parse(busrec.BusStopNo.Substring(2, 2)) - 1).ToString();
                                            busrec.BusStopNo = this._bus.BusNumber + (new_busstopno.Length == 2 ? new_busstopno : "0" + new_busstopno);
                                            Data.BusStopDAO.Update(busrec);
                                        }
                                    }
                                }
                                else if (int.Parse(thisBusStopNo.Substring(2, 2)) > int.Parse(this._bus.BusStopNo.Substring(2, 2)))
                                {
                                    foreach (Data.BusStop busrec in bustmp)
                                    {
                                        if (int.Parse(busrec.BusStopNo.Substring(2, 2)) >= int.Parse(this._bus.BusStopNo.Substring(2, 2)) && int.Parse(busrec.BusStopNo.Substring(2, 2)) < int.Parse(thisBusStopNo.Substring(2, 2)) && busrec.BusStopNo != thisBusStopNo)
                                        {
                                            new_busstopno = (int.Parse(busrec.BusStopNo.Substring(2, 2)) + 1).ToString();
                                            busrec.BusStopNo = this._bus.BusNumber + (new_busstopno.Length == 2 ? new_busstopno : "0" + new_busstopno);
                                            Data.BusStopDAO.Update(busrec);
                                        }
                                    }
                                }
                            }
                            else if (this._bus.BusStopNo.Substring(0, 2) != thisBusStopNo.Substring(0, 2) && !repeat)
                            {
                                List<Data.BusStop> bustmp2 = Data.BusStopDAO.SelectByBusTimeNameAndNumber(this._bus.BusTimeName, thisBusStopNo.Substring(0, 2));
                                foreach (Data.BusStop busrec in bustmp2)
                                {
                                    if (int.Parse(busrec.BusStopNo.Substring(2, 2)) > int.Parse(thisBusStopNo.Substring(2, 2)))
                                    {
                                        new_busstopno = (int.Parse(busrec.BusStopNo) - 1).ToString();
                                        busrec.BusStopNo = this._bus.BusNumber + (new_busstopno.Length == 2 ? new_busstopno : "0" + new_busstopno);
                                        Data.BusStopDAO.Update(busrec);
                                    }
                                }
                                foreach (Data.BusStop busrec in bustmp)
                                {
                                    if (int.Parse(busrec.BusStopNo.Substring(2, 2)) >= int.Parse(this._bus.BusStopNo.Substring(2, 2)))
                                    {
                                        new_busstopno = (int.Parse(busrec.BusStopNo) + 1).ToString();
                                        busrec.BusStopNo = this._bus.BusNumber + (new_busstopno.Length == 2 ? new_busstopno : "0" + new_busstopno);
                                        Data.BusStopDAO.Update(busrec);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (this._bus.BusStopNo.Substring(0, 2) != this._bus.BusNumber)
                            {
                                MessageBox.Show(this._bus.BusNumber + "號車與站序前兩碼不同，請重新輸入！");
                                repeat = true;
                            }
                        }
                    }
                }

                if (repeat == false)
                    Data.BusStopDAO.Update(this._bus);
            }
            else
            {
                Data.BusStop bus = new Data.BusStop();
                bus.BusStopID = this.txtID.Text;
                bus.BusStopName = this.txtBusName.Text;

                if (this.txtMoney.Text == "")
                {
                    MessageBox.Show("金額不可為空，請重新輸入！");
                    repeat = true;
                }
                else if (!int.TryParse(this.txtMoney.Text, out dollars))
                {
                    MessageBox.Show("金額需為數字，請重新輸入！");
                    repeat = true;
                }
                else
                    bus.BusMoney = int.Parse(this.txtMoney.Text);
                bus.BusNumber = this.txtBusNumber.Text;
                bus.BusStopNo = this.nrBusStopNo.Value.ToString();
                if (this.nrBusStopNo.Value.ToString().Length == 2)
                    bus.BusStopNo = this.txtBusNumber.Text + this.nrBusStopNo.Value.ToString();
                else if (this.nrBusStopNo.Value.ToString().Length == 1)
                    bus.BusStopNo = this.txtBusNumber.Text + "0" + this.nrBusStopNo.Value.ToString();
                bus.BusStopAddr = this.txtBusStopAddr.Text;
                bus.BusTimeName = this._bus.BusTimeName;
                bus.BusUpAddr = this.cboUpAddr.Text;
                bus.ComeTime = this.txtCometime.Text;
                string new_busstopno = "";
                List<Data.BusStop> bustmp = Data.BusStopDAO.SelectByBusTimeNameAndNumber(this._bus.BusTimeName, this._bus.BusNumber);
                if (!repeat)
                {
                    if (bus.BusStopID == "" || bus.BusStopName == "" || bus.BusNumber == "")
                    {
                        MessageBox.Show("電腦代號、車號、站名名稱不可為空，請重新輸入！");
                        repeat = true;
                    }
                    else if (bus.BusMoney < 0)
                    {
                        MessageBox.Show("金額不可小於0，請重新輸入！");
                        repeat = true;
                    }
                    else
                    {
                        //foreach (Data.BusStop busrec in bustmp)
                        //{
                        //    if (bus.BusStopNo == busrec.BusStopNo && bus.BusStopName != busrec.BusStopName)
                        //    {
                        //        MessageBox.Show("『" + bus.BusStopName + "』站名或『" + bus.BusStopNo + "』站序重複，請重新輸入！");
                        //        repeat = true;
                        //        break;
                        //    }
                        //}
                        if (bus.BusStopNo.Substring(0, 2) != bus.BusNumber)
                        {
                            MessageBox.Show(bus.BusNumber + "號車與站序前兩碼不同，請重新輸入！");
                            repeat = true;
                        }
                        if (!repeat)
                        {
                            foreach (Data.BusStop busrec in bustmp)
                            {
                                if (int.Parse(busrec.BusStopNo.Substring(2, 2)) >= int.Parse(bus.BusStopNo.Substring(2, 2)))
                                {
                                    new_busstopno = (int.Parse(busrec.BusStopNo) + 1).ToString();
                                    busrec.BusStopNo = this.txtBusNumber.Text + (new_busstopno.Length == 2 ? new_busstopno : "0" + new_busstopno);
                                    Data.BusStopDAO.Update(busrec);
                                }
                            }
                        }
                    }
                }

                if (repeat == false)
                    Data.BusStopDAO.Insert(bus);
            }

            if (repeat == false)
                this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("確定要關閉？", "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                this.Close();
        }

        private void BusDetailForm_Load(object sender, EventArgs e)
        {
            if (this._bus != null)
            {
                this.txtID.Text = this._bus.BusStopID;
                if (this._bus.BusStopID != null)
                {
                    this.txtID.ReadOnly = true;
                    this.txtBusName.Text = this._bus.BusStopName;
                    this.txtMoney.Text = this._bus.BusMoney.ToString();
                    this.nrBusStopNo.Value = decimal.Parse(this._bus.BusStopNo.Substring(2, 2));
                    this.txtBusStopAddr.Text = this._bus.BusStopAddr;
                    this.cboUpAddr.Text = this._bus.BusUpAddr;
                    this.txtCometime.Text = this._bus.ComeTime;
                    this.txtBusNumber.Text = this._bus.BusNumber;
                    thisBusStopNo = this._bus.BusStopNo;
                }
                else
                {
                    this.txtBusNumber.ReadOnly = true;
                    //ReverseOrderByStopNO(反序)
                    List<Data.BusStop> bustmp = Data.BusStopDAO.SelectByBusTimeNameAndNumberOrderByStopNO(this._bus.BusTimeName, this._bus.BusNumber);
                    int bussort = int.Parse(bustmp[bustmp.Count - 1].BusStopNo.Substring(2, 2)) + 1;
                    if (bussort.ToString().Length == 2)
                        this.nrBusStopNo.Value = decimal.Parse(bussort.ToString());
                    else if (bussort.ToString().Length == 1)
                        this.nrBusStopNo.Value = decimal.Parse("0" + bussort.ToString());
                }
                this.txtBusNumber.Text = this._bus.BusNumber;
            }
        }
    }
}
