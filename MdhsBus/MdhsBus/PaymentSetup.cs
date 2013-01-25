using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using AccountsReceivable.API;

namespace MdhsBus
{
    public partial class PaymentSetup : BaseForm
    {
        public PaymentSetup()
        {
            InitializeComponent();
        }

        private void PaymentSetup_Load(object sender, EventArgs e)
        {
            this.intSchoolYear.Value = int.Parse(K12.Data.School.DefaultSchoolYear);
            this.cboSemester.Text = K12.Data.School.DefaultSemester == "1" ? "上學期" : "下學期";

            this.cboPayment.Items.Clear();

            //讀取系統中所有的銀行設定。
            List<BarcodeConfiguration> Barcodes = BarcodeConfiguration.GetAll();
            if (Barcodes.Count > 0)
            {
                foreach (BarcodeConfiguration Barcode in Barcodes)
                    this.cboPayment.Items.Add(Barcode);
            }

            this.txtBank.Text = "";
            this.dtDueDate.Value = DateTime.Today;
        }

        private void cboPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            BarcodeConfiguration selectBarcode = cboPayment.SelectedItem as BarcodeConfiguration;

            this.txtBank.Text = selectBarcode.BankModuleCode;

            this.dtDueDate.Enabled = true;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            if (this.cboPayment.SelectedIndex < 0)
            {
                MessageBox.Show("請先選擇收費模組！");
                return;
            }

            if (this.cboSemester.Text == "")
            {
                MessageBox.Show("請先選擇學期別！");
                return;
            }

            if (this.txtPayment.Text == "")
            {
                MessageBox.Show("請先填入收費名稱！");
                return;
            }

            //讀取系統中所有的收費模組(日期設定)。
            List<Payment> pays = Payment.GetAll();
            if (pays.Count > 0)
            {
                foreach (Payment pay in pays)
                    if (pay.SchoolYear == this.intSchoolYear.Value && pay.Semester == (this.cboSemester.Text == "上學期" ? 1 : 2))
                    {
                        if (this.txtPayment.Text == pay.Name)
                        {
                            MessageBox.Show("此學期此收費名稱已存在！");
                            return;
                        }
                    }
            }

            //新增收費，這個收費是每次收費都要新增一個，例如：第一季車費、第二季車費....。
            Payment BusPayment = new Payment(txtPayment.Text, intSchoolYear.Value, cboSemester.Text == "上學期" ? 1 : 2);

            //設定收費的預設繳款到期日，如果在計算收費時沒有額外指定，就會使用這裡的設定。
            //DefaultExpiration 是關鍵字，程式會抓這個 Key，所以是固定不變的。
            //demoPayment.Extensions.Add(DateTime.Parse(((Payment)cboPayment.SelectedItem).Extensions["DefaultExpiration"]));
            BusPayment.Extensions.Add("DefaultExpiration", dtDueDate.Value.ToShortDateString());

            //BarcodeConfiguration 屬性是 Payment 的「條碼計算設定」，所以是一定要指定的
            //在畫面上就是「銀行設定」裡面的項目，可以透過 BarcodeConfiguration 類別列出與取得
            //使用者在銀行設定裡的所有設定項目，通常可能設計成讓使用者可以選擇要用那個設定。
            //demoPayment.BarcodeConfiguration = GetBarcodeConfiguration();
            //demoPayment.BarcodeConfiguration = ((BarcodeConfiguration)cboPayment.SelectedItem).BarcodeConfiguration;
            BusPayment.BarcodeConfiguration = cboPayment.SelectedItem as BarcodeConfiguration;

            //將此收費新增到資料庫中，回傳的 Payment 物件會包含新增後的 UID。
            BusPayment = Payment.Insert(BusPayment);

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PaymentSheetTemplate frm = new PaymentSheetTemplate();
            frm.ShowDialog();
        }
    }
}
