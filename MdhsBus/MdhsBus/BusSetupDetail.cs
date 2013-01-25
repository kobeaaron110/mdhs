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
using AccountsReceivable.API;

namespace MdhsBus
{
    public partial class BusSetupDetail : BaseForm
    {
        private Setup _masterForm;        //做 call back , refresh 畫面用的
        private Data.BusSetup _busset;                  //若要修改某個站名，就會把該站名物件傳過來。

        public BusSetupDetail()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 若要修改某個站名，就會把該站名物件傳進來。
        /// </summary>
        /// <param name="bus"></param>
        public void SetBus(Data.BusSetup busset)
        {
            this._busset = busset;
        }

        /// <summary>
        /// 設定可以call back 的表單
        /// </summary>
        /// <param name="masterForm"></param>
        public void SetMasterForm(Setup masterForm)
        {
            this._masterForm = masterForm;
        }

        private void BusSetupDetail_Load(object sender, EventArgs e)
        {
            //this.cboBusPayment.Items.Clear();
            ////讀取系統中所有的銀行設定。
            //List<Payment> BusPayments = Payment.GetAll();
            //if (BusPayments.Count > 0)
            //{
            //    foreach (Payment BusPayment in BusPayments)
            //        this.cboBusPayment.Items.Add(BusPayment);
            //}

            this.cboPayment.Items.Clear();
            //讀取系統中所有的銀行設定。
            List<BarcodeConfiguration> Barcodes = BarcodeConfiguration.GetAll();
            if (Barcodes.Count > 0)
            {
                foreach (BarcodeConfiguration Barcode in Barcodes)
                    this.cboPayment.Items.Add(Barcode);
            }

            if (this._busset.BusRangeName == null)
            {
                this.nrYear.Minimum = DateTime.Today.Year - 1912;
                this.nrYear.Maximum = DateTime.Today.Year - 1906;
                this.nrYear.Value = decimal.Parse(K12.Data.School.DefaultSchoolYear);
                this.nrSchoolYear.Minimum = decimal.Parse(K12.Data.School.DefaultSchoolYear) - 3;
                this.nrSchoolYear.Maximum = decimal.Parse(K12.Data.School.DefaultSchoolYear) + 3;
                this.nrSchoolYear.Value = decimal.Parse(K12.Data.School.DefaultSchoolYear);
                this.nrSemester.Value = decimal.Parse(K12.Data.School.DefaultSemester);
            }
            else
            {
                //this.nrYear.Minimum = DateTime.Today.Year - 1912;
                this.nrYear.Minimum = 99;
                this.nrYear.Maximum = DateTime.Today.Year - 1906;
                this.nrYear.Value = this._busset.BusYear;
                this.nrSchoolYear.Minimum = decimal.Parse(K12.Data.School.DefaultSchoolYear) - 3;
                this.nrSchoolYear.Maximum = decimal.Parse(K12.Data.School.DefaultSchoolYear) + 3;
                this.cboBusTime.Text = this._busset.BusTimeName;
                if (this._busset.BusPaymentName != "")
                {
                    this.nrSchoolYear.Value = Payment.GetByID(this._busset.BusPaymentName).SchoolYear;
                    this.nrSemester.Value = Payment.GetByID(this._busset.BusPaymentName).Semester;
                    this.cboPayment.SelectedIndex = 0;
                }
                
                this.txtBusRange.Text = this._busset.BusRangeName;
                this.dateTime_start.Value = this._busset.BusStartDate;
                this.dateTime_end.Value = this._busset.BusEndDate;
                this.dateTime_payend.Value = this._busset.PayEndDate;
                this.nrBusDay.Value = this._busset.DateCount;

                //this.chkNew.Checked = this._busset.BusByNewStudent;
                //List<StudentByBus> StudentBuses = StudentByBusDAO.SelectByBusYearAndTimeName(this._busset.BusYear, this._busset.BusRangeName);
                //if (StudentBuses.Count > 0)
                //    this.txtBusRange.Enabled = false;
            }

            //取得所有校車時段清單
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool repeat = false;
            Data.BusSetup bus = new Data.BusSetup();
            if (this.txtBusRange.Text == "")
            {
                MessageBox.Show("期間名稱不可為空，請重新輸入！");
                repeat = true;
            }
            if (this.txtBusRange.Text.IndexOf("-" + this.cboBusTime.Text) < 0)
            {
                MessageBox.Show("期間名稱開頭需為『-" + this.cboBusTime.Text + "』，請重新輸入！");
                repeat = true;
            }

            if (this.cboBusTime.Text == "")
            {
                MessageBox.Show("校車時段不可為空，請重新輸入！");
                repeat = true;
            }
            if (this.cboPayment.Text == "")
            {
                MessageBox.Show("收費銀行設定不可為空，請重新輸入！");
                repeat = true;
            }

            if (this.dateTime_start.Value > this.dateTime_end.Value)
            {
                MessageBox.Show("搭車起始日期不可為大於搭車截止日期，請重新輸入！");
                repeat = true;
            }
            //if (this.dateTime_start.Value < DateTime.Today)
            //{
            //    MessageBox.Show("搭車起始日期不可為小於今日，請重新輸入！");
            //    repeat = true;
            //}
            if (repeat)
                return;

            if (this._busset.BusRangeName != null)
            {
                if (this._busset.BusPaymentName != "")
                {
                    //讀取系統中所有的收費模組(日期設定)。
                    List<Payment> pays = Payment.GetAll();
                    if (pays.Count > 0)
                    {
                        foreach (Payment pay in pays)
                        {
                            if (pay.SchoolYear == this.nrSchoolYear.Value && pay.Semester == this.nrSemester.Value)
                            {
                                if (Payment.GetByID(this._busset.BusPaymentName).SchoolYear == pay.SchoolYear && Payment.GetByID(this._busset.BusPaymentName).Semester == pay.Semester)
                                {
                                    if (this.txtBusRange.Text != pay.Name)
                                        continue;
                                    pay.Name = this.txtBusRange.Text;
                                    if (!pay.Extensions.ContainsKey("DefaultExpiration"))
                                        pay.Extensions.Add("DefaultExpiration", this.dateTime_payend.Value.ToShortDateString());
                                    else
                                        pay.Extensions["DefaultExpiration"] = this.dateTime_payend.Value.ToShortDateString();
                                    if (pay.BarcodeConfiguration == null)
                                        pay.BarcodeConfiguration = cboPayment.SelectedItem as BarcodeConfiguration;
                                    Payment.Update(pay);
                                    this._busset.BusPaymentName = pay.UID;
                                    continue;
                                }
                                else
                                {
                                    if (this.txtBusRange.Text == pay.Name)
                                    {
                                        MessageBox.Show("此學期此收費名稱已存在！");
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<BarcodeConfiguration> barcodes = BarcodeConfiguration.GetAll();
                    string barcodeid = "";
                    foreach (BarcodeConfiguration barcode in barcodes)
                        if (barcode.Name == this.cboPayment.Text)
                            barcodeid = barcode.UID;

                    //讀取系統中所有的收費模組(日期設定)。
                    List<Payment> pays = Payment.GetAll();
                    if (pays.Count > 0)
                    {
                        foreach (Payment pay in pays)
                        {
                            if (pay.SchoolYear == this.nrSchoolYear.Value && pay.Semester == this.nrSemester.Value)
                            {
                                if (this.txtBusRange.Text != pay.Name)
                                    continue;
                                pay.Name = this.txtBusRange.Text;
                                if (!pay.Extensions.ContainsKey("DefaultExpiration"))
                                    pay.Extensions.Add("DefaultExpiration", this.dateTime_payend.Value.ToShortDateString());
                                else
                                    pay.Extensions["DefaultExpiration"] = this.dateTime_payend.Value.ToShortDateString();
                                if (pay.BarcodeConfiguration == null)
                                    pay.BarcodeConfiguration = cboPayment.SelectedItem as BarcodeConfiguration;
                                Payment.Update(pay);
                                this._busset.BusPaymentName = pay.UID;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        Payment BusPayment = new Payment(txtBusRange.Text, int.Parse(nrSchoolYear.Value.ToString()), int.Parse(nrSemester.Value.ToString()));
                        BusPayment.Extensions.Add("DefaultExpiration", this.dateTime_payend.Value.ToShortDateString());
                        BusPayment.BarcodeConfiguration = cboPayment.SelectedItem as BarcodeConfiguration;
                        BusPayment = Payment.Insert(BusPayment);
                        this._busset.BusPaymentName = BusPayment.UID;
                    }
                }

                this._busset.BusYear = int.Parse(this.nrYear.Value.ToString());
                this._busset.BusRangeName = this.txtBusRange.Text;
                this._busset.BusStartDate = this.dateTime_start.Value;
                this._busset.BusEndDate = this.dateTime_end.Value;
                this._busset.PayEndDate = this.dateTime_payend.Value;
                this._busset.DateCount = int.Parse(this.nrBusDay.Value.ToString());
                this._busset.BusTimeName = this.cboBusTime.Text;
                //Payment selectPayment = this.cboBusPayment.SelectedItem as Payment;
                //this._busset.BusPaymentName = selectPayment.UID;
                //this._busset.BusByNewStudent = this.chkNew.Checked;
                if (repeat == false)
                    Data.BusSetupDAO.Update(_busset);
            }
            else
            {
                Payment BusPayment = new Payment(txtBusRange.Text, int.Parse(nrSchoolYear.Value.ToString()), int.Parse(nrSemester.Value.ToString()));
                BusPayment.Extensions.Add("DefaultExpiration", this.dateTime_payend.Value.ToShortDateString());
                BusPayment.BarcodeConfiguration = cboPayment.SelectedItem as BarcodeConfiguration;
                BusPayment = Payment.Insert(BusPayment);
                bus.BusPaymentName = BusPayment.UID;

                bus.BusYear = int.Parse(this.nrYear.Value.ToString());
                bus.BusRangeName = this.txtBusRange.Text;
                bus.BusStartDate = this.dateTime_start.Value;
                bus.BusEndDate = this.dateTime_end.Value;
                bus.PayEndDate = this.dateTime_payend.Value;
                bus.DateCount = int.Parse(this.nrBusDay.Value.ToString());
                bus.BusTimeName = this.cboBusTime.Text;
                //Payment selectPayment = this.cboBusPayment.SelectedItem as Payment;
                //this._busset.BusPaymentName = selectPayment.UID;
                //bus.BusByNewStudent = this.chkNew.Checked;
                if (repeat == false)
                    Data.BusSetupDAO.Insert(bus);
            }
            if (repeat == false)
                this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboBusTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBusRange.Text = nrSchoolYear.Value.ToString() + "第" + nrSemester.Value.ToString() + "學期" + "-" + cboBusTime.Text;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PaymentSheetTemplate frm = new PaymentSheetTemplate();
            frm.ShowDialog();
        }

        private void nrSchoolYear_ValueChanged(object sender, EventArgs e)
        {
            txtBusRange.Text = nrSchoolYear.Value.ToString() + "第" + nrSemester.Value.ToString() + "學期" + "-" + cboBusTime.Text;
        }

        private void nrSemester_ValueChanged(object sender, EventArgs e)
        {
            txtBusRange.Text = nrSchoolYear.Value.ToString() + "第" + nrSemester.Value.ToString() + "學期" + "-" + cboBusTime.Text;
        }
    }
}
