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
using Aspose.Cells;
using FISCA.Presentation;
using System.IO;

namespace MdhsBus
{
    public partial class PaymentBar : BaseForm
    {
        Dictionary<string, Payment> paymentRecord = new Dictionary<string, Payment>();
        List<string> paymentKeys = new List<string>();
        int old_SelectedIndex = -1;
        public PaymentBar()
        {
            InitializeComponent();
        }

        private void PaymentBar_Load(object sender, EventArgs e)
        {
            fillSchoolYear();
            //listBox_payment.ScrollAlwaysVisible = true;
        }

        private void fillSchoolYear()
        {
            this.cboSchoolYear.Items.Clear();
            this.cboSchoolYear.DisplayMember = "SchoolYear";
            this.cboSemester.Items.Clear();

            for (int i = 0; i < 5; i++)
                this.cboSchoolYear.Items.Add(int.Parse(K12.Data.School.DefaultSchoolYear) - 2 + i);

            for (int i = 0; i < 2; i++)
                this.cboSemester.Items.Add(i + 1);
        }

        private void cboSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox_payment.Items.Clear();
            paymentRecord.Clear();
            paymentKeys.Clear();
            label1.Text = "收費總進度(%)";
            progressBar1.Value = 0;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            old_SelectedIndex = -1;

            if (cboSchoolYear.Text != "" && cboSemester.Text != "")
            {
                List<Payment> payments = Payment.GetAll();
                foreach (Payment var in payments)
                {
                    if (var.SchoolYear != int.Parse(cboSchoolYear.Text))
                        continue;
                    if (var.Semester != int.Parse(cboSemester.Text))
                        continue;

                    if (!paymentRecord.ContainsKey(var.UID))
                    {
                        paymentRecord.Add(var.UID, var);
                        paymentKeys.Add(var.UID);
                    }
                    listBox_payment.Items.Add(var.Name);
                }
            }
       }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboSchoolYear.Text == "")
            {
                MessageBox.Show("學年度不可為空！");
                return;
            }
            if (cboSemester.Text == "")
            {
                MessageBox.Show("學期別不可為空！");
                return;
            }

            List<Payment> payments = Payment.GetAll();
            listBox_payment.Items.Clear();
            paymentRecord.Clear();
            paymentKeys.Clear();
            label1.Text = "收費總進度(%)";
            progressBar1.Value = 0;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            old_SelectedIndex = -1;

            foreach (Payment var in payments)
            {
                if (var.SchoolYear != int.Parse(cboSchoolYear.Text))
                    continue;
                if (var.Semester != int.Parse(cboSemester.Text))
                    continue;
                //if (var.Name == "102-日校校車2月")
                //{
                //    Payment.Delete(var);
                //    continue;
                //}

                if (!paymentRecord.ContainsKey(var.UID))
                {
                    paymentRecord.Add(var.UID, var);
                    paymentKeys.Add(var.UID);
                }
                listBox_payment.Items.Add(var.Name);
            }
        }

        private void listBox_payment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (old_SelectedIndex == listBox_payment.SelectedIndex)
                return;

            progressBar1.Value = 0;
            label1.Text = "讀取資料中";
            textBox1.Text = "讀取資料中";
            textBox2.Text = "讀取資料中";
            textBox3.Text = "讀取資料中";
            textBox4.Text = "讀取資料中";
            textBox5.Text = "讀取資料中";
            textBox6.Text = "讀取資料中";
            textBox7.Text = "讀取資料中";
            
            label1.Refresh();
            progressBar1.Refresh();
            textBox1.Refresh();
            textBox2.Refresh();
            textBox3.Refresh();
            textBox4.Refresh();
            textBox5.Refresh();
            textBox6.Refresh();
            textBox7.Refresh();

            Payment payment = Payment.GetByID(paymentKeys[listBox_payment.SelectedIndex]);

            //把所有相關資料都讀取下來。
            payment.FillFull();

            //學生總數
            int studentCount = payment.Details.Count;


            //Workbook wb = new Workbook();
            //Style defaultStyle = wb.DefaultStyle;
            //defaultStyle.Font.Name = "標楷體";
            //defaultStyle.Font.Size = 12;
            //wb.DefaultStyle = defaultStyle;
            //int row = 0;
            int nowSet = 0;
            //wb.Worksheets[0].Name = "學生校車繳費資料";

            ////校車路線站名資料
            //wb.Worksheets[0].Cells[row, 0].PutValue("系統編號");
            //wb.Worksheets[0].Cells[row, 1].PutValue("校車年度");
            //wb.Worksheets[0].Cells[row, 2].PutValue("期間名稱");
            //wb.Worksheets[0].Cells[row, 3].PutValue("科別");
            //wb.Worksheets[0].Cells[row, 4].PutValue("學生編號");
            //wb.Worksheets[0].Cells[row, 5].PutValue("學生姓名");
            //wb.Worksheets[0].Cells[row, 6].PutValue("電腦代號");
            //wb.Worksheets[0].Cells[row, 7].PutValue("站名");
            //wb.Worksheets[0].Cells[row, 8].PutValue("學生類別");
            //wb.Worksheets[0].Cells[row, 9].PutValue("應繳金額");
            //wb.Worksheets[0].Cells[row, 10].PutValue("實繳金額");
            //wb.Worksheets[0].Cells[row, 11].PutValue("繳費日期");
            //row++;


            this.studentdataGridView.Rows.Clear();

            decimal total = 0;      //應收總金額
            decimal received = 0;   //已收金額
            int paidCount = 0;      //已繳費人數
            int unpayCount = 0;     //未繳費人數
            int overflowCount = 0;  //繳費超額人數
            int lowflowCount = 0;   //繳費不足額人數
            decimal percentvalue = 0;
            foreach (PaymentDetail detail in payment.Details)
            {
                total += detail.Amount;
                received += detail.TotalPaid;
                if (detail.Amount == 0)
                    studentCount--;

                if (detail.TotalPaid > detail.Amount)
                    overflowCount++;
                if (detail.TotalPaid < detail.Amount && detail.TotalPaid > 0)
                    lowflowCount++;

                if (detail.TotalPaid > 0)
                    paidCount++;
                else
                    if (detail.Amount > 0)
                        unpayCount++;

                MotherForm.SetStatusBarMessage("正在產生繳費資料", nowSet++ * 100 / payment.Details.Count);

                //wb.Worksheets[0].Cells[row, 0].PutValue(detail.UID);
                //wb.Worksheets[0].Cells[row, 1].PutValue(detail.Extensions["校車收費名稱"]);
                //wb.Worksheets[0].Cells[row, 2].PutValue(detail.Extensions["校車收費年度"]);
                //wb.Worksheets[0].Cells[row, 3].PutValue(detail.Extensions["MergeField::科別"]);
                //wb.Worksheets[0].Cells[row, 4].PutValue(detail.Extensions["MergeField::學號"]);
                //wb.Worksheets[0].Cells[row, 5].PutValue(detail.Extensions["MergeField::姓名"]);
                //wb.Worksheets[0].Cells[row, 6].PutValue(detail.Extensions["MergeField::代碼"]);
                //wb.Worksheets[0].Cells[row, 7].PutValue(detail.Extensions["MergeField::站名"]);
                //wb.Worksheets[0].Cells[row, 8].PutValue(detail.RefTargetType);
                //wb.Worksheets[0].Cells[row, 9].PutValue(detail.Amount);
                //wb.Worksheets[0].Cells[row, 10].PutValue(detail.TotalPaid);
                //IList<PaymentHistory> historyss = detail.Histories;
                //if (historyss == null)
                //    continue;
                //foreach (PaymentHistory history in historyss)
                //{
                //    if (history.PayDate == null)
                //        continue;
                //    else
                //    {
                //        wb.Worksheets[0].Cells[row, 11].PutValue(DateTime.Parse(history.PayDate.ToString()).ToShortDateString());
                //        break;
                //    }
                //}

                if (detail.Amount > 0)
                {
                    if (detail.Extensions.ContainsKey("MergeField::科別"))
                        studentdataGridView.Rows.Add(detail.Extensions["MergeField::科別"], detail.Extensions["MergeField::學號"], detail.Extensions["MergeField::姓名"], detail.Amount, detail.TotalPaid);
                    else if (detail.Extensions.ContainsKey("MergeField::班級"))
                        studentdataGridView.Rows.Add(detail.Extensions["MergeField::班級"], detail.Extensions["MergeField::學號"], detail.Extensions["MergeField::姓名"], detail.Amount, detail.TotalPaid);
                }
               
                //row++;
            }
            if (total > 0)
                percentvalue = received * 100 / total;
            //else
                //return;
            studentdataGridView.EndEdit();
            studentdataGridView.CancelEdit();


            label1.Text = "收費總進度(" + Math.Round(percentvalue, MidpointRounding.AwayFromZero).ToString() + "%)";
            progressBar1.Value = int.Parse(Math.Round(percentvalue, MidpointRounding.AwayFromZero).ToString());
            textBox1.Text = "NT$ " + total.ToString("###,###,###,###");
            textBox2.Text = "NT$ " + received.ToString("###,###,###,###");
            textBox3.Text = studentCount.ToString();
            textBox4.Text = paidCount.ToString();
            textBox5.Text = unpayCount.ToString();
            textBox6.Text = overflowCount.ToString();
            textBox7.Text = lowflowCount.ToString();
            if (total > 0)
                textBox8.Text = payment.BarcodeConfiguration.Name;
            else
                textBox8.Text = "";
            old_SelectedIndex = listBox_payment.SelectedIndex;

            MotherForm.SetStatusBarMessage("繳費資料已完成", 100);

            //wb.Worksheets[0].AutoFitColumns();

            //ExportExcel("匯出學生校車繳費資料", wb);

            //try
            //{
            //    wb.Save(Application.StartupPath + "\\Reports\\匯出學生校車繳費資料.xls", FileFormatType.Excel2003);
            //    System.Diagnostics.Process.Start(Application.StartupPath + "\\Reports\\匯出學生校車繳費資料.xls");
            //    MotherForm.SetStatusBarMessage("學生校車繳費資料已匯出完成", 100);
            //}
            //catch
            //{
            //    System.Windows.Forms.SaveFileDialog sd1 = new System.Windows.Forms.SaveFileDialog();
            //    sd1.Title = "另存新檔";
            //    sd1.FileName = "匯出學生校車繳費資料.xls";
            //    sd1.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
            //    if (sd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        try
            //        {
            //            wb.Save(sd1.FileName, FileFormatType.Excel2003);
            //            System.Diagnostics.Process.Start(sd1.FileName);
            //            MotherForm.SetStatusBarMessage("學生校車繳費資料已匯出完成", 100);
            //        }
            //        catch
            //        {
            //            System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //            return;
            //        }
            //    }
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox_payment.SelectedIndex == -1)
                return;

            Payment payment = Payment.GetByID(paymentKeys[listBox_payment.SelectedIndex]);
            payment.FillFull();

            Workbook wb = new Workbook();
            Style defaultStyle = wb.DefaultStyle;
            defaultStyle.Font.Name = "標楷體";
            defaultStyle.Font.Size = 12;
            wb.DefaultStyle = defaultStyle;
            int row = 0;
            int nowSet = 0;
            wb.Worksheets[0].Name = "繳費狀況表";

            wb.Worksheets[0].Cells[row, 0].PutValue("科別");
            wb.Worksheets[0].Cells[row, 1].PutValue("學生編號");
            wb.Worksheets[0].Cells[row, 2].PutValue("姓名");
            wb.Worksheets[0].Cells[row, 3].PutValue("應繳金額");
            wb.Worksheets[0].Cells[row, 4].PutValue("已繳金額");
            wb.Worksheets[0].Cells[row, 5].PutValue("繳費日期");
            wb.Worksheets[0].Cells[row, 6].PutValue("繳費通路");
            wb.Worksheets[0].Cells[row, 7].PutValue("手續費");
            row++;

            foreach (PaymentDetail detail in payment.Details)
            {
                MotherForm.SetStatusBarMessage("正在產生報表", nowSet++ * 100 / payment.Details.Count);
                if (detail.Extensions.ContainsKey("MergeField::科別"))
                    wb.Worksheets[0].Cells[row, 0].PutValue(detail.Extensions["MergeField::科別"]);
                else if (detail.Extensions.ContainsKey("MergeField::班級"))
                    wb.Worksheets[0].Cells[row, 0].PutValue(detail.Extensions["MergeField::班級"]);

                wb.Worksheets[0].Cells[row, 1].PutValue(detail.Extensions["MergeField::學號"]);
                wb.Worksheets[0].Cells[row, 2].PutValue(detail.Extensions["MergeField::姓名"]);
                wb.Worksheets[0].Cells[row, 3].PutValue(detail.Amount);
                wb.Worksheets[0].Cells[row, 4].PutValue(detail.TotalPaid);

                IList<PaymentHistory> historyss = detail.Histories;
                if (historyss == null)
                    continue;
                foreach (PaymentHistory history in historyss)
                {
                    if (history.PayDate == null)
                        continue;
                    else
                    {
                        wb.Worksheets[0].Cells[row, 5].PutValue(DateTime.Parse(history.PayDate.ToString()).ToShortDateString());

                        switch (history.Receipt.Transactions[0].ChannelCode)
                        {
                            default:
                                wb.Worksheets[0].Cells[row, 6].PutValue("OTHER");
                                break;
                            case "2":
                                wb.Worksheets[0].Cells[row, 6].PutValue("郵局");
                                break;
                            case "3":
                                wb.Worksheets[0].Cells[row, 6].PutValue("超商");
                                break;
                            case "4":
                                wb.Worksheets[0].Cells[row, 6].PutValue("超商");
                                break;
                            case "6":
                                wb.Worksheets[0].Cells[row, 6].PutValue("超商");
                                break;
                            case "7":
                                wb.Worksheets[0].Cells[row, 6].PutValue("超商");
                                break;
                            case "5":
                                wb.Worksheets[0].Cells[row, 6].PutValue("ATM");
                                break;
                        }
                        wb.Worksheets[0].Cells[row, 7].PutValue(history.Receipt.Transactions[0].ChannelCharge);
                        break;
                    }
                }
                
                row++;
            }
            wb.Worksheets[0].AutoFitColumns();

            ExportExcel("繳費狀況表", wb);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox_payment.SelectedIndex == -1)
                return;

            Payment payment = Payment.GetByID(paymentKeys[listBox_payment.SelectedIndex]);
            payment.FillFull();

            Workbook wb = new Workbook();
            Style defaultStyle = wb.DefaultStyle;
            defaultStyle.Font.Name = "標楷體";
            defaultStyle.Font.Size = 12;
            wb.DefaultStyle = defaultStyle;
            int row = 0;
            int nowSet = 0;
            wb.Worksheets[0].Name = "繳費單資料";

            wb.Worksheets[0].Cells[row, 0].PutValue("UniqueNumber");
            wb.Worksheets[0].Cells[row, 1].PutValue("金額");
            wb.Worksheets[0].Cells[row, 2].PutValue("截止日");
            wb.Worksheets[0].Cells[row, 3].PutValue("虛擬帳號");
            wb.Worksheets[0].Cells[row, 4].PutValue("科別");
            wb.Worksheets[0].Cells[row, 5].PutValue("學生編號");
            wb.Worksheets[0].Cells[row, 6].PutValue("姓名");
            row++;

            foreach (PaymentDetail detail in payment.Details)
            {
                MotherForm.SetStatusBarMessage("正在產生報表", nowSet++ * 100 / payment.Details.Count);
                if (detail.Amount == 0)
                    continue;

                wb.Worksheets[0].Cells[row, 0].PutValue(detail.UID);
                wb.Worksheets[0].Cells[row, 1].PutValue(detail.Amount);
                wb.Worksheets[0].Cells[row, 2].PutValue(detail.Payment.Extensions["DefaultExpiration"]);
                if (detail.Extensions.ContainsKey("MergeField::科別"))
                    wb.Worksheets[0].Cells[row, 4].PutValue(detail.Extensions["MergeField::科別"]);
                else if (detail.Extensions.ContainsKey("MergeField::班級"))
                    wb.Worksheets[0].Cells[row, 4].PutValue(detail.Extensions["MergeField::班級"]);
                
                wb.Worksheets[0].Cells[row, 5].PutValue(detail.Extensions["MergeField::學號"]);
                wb.Worksheets[0].Cells[row, 6].PutValue(detail.Extensions["MergeField::姓名"]);

                IList<PaymentHistory> historyss = detail.Histories;
                if (historyss == null)
                    continue;
                string unpaid_VirtualAccount = "";
                string paid_VirtualAccount = "";
                foreach (PaymentHistory history in historyss)
                {
                    if (history.PayDate == null)
                    {
                        if (!history.Receipt.Cancelled)
                            unpaid_VirtualAccount = history.Receipt.VirtualAccount;
                        continue;
                    }
                    else
                    {
                        paid_VirtualAccount = history.Receipt.VirtualAccount;
                        break;
                    }
                }
                wb.Worksheets[0].Cells[row, 3].PutValue(paid_VirtualAccount == "" ? unpaid_VirtualAccount : paid_VirtualAccount);

                row++;
            }
            wb.Worksheets[0].AutoFitColumns();

            ExportExcel("繳費單資料", wb);
        }

        private void ExportExcel(string p, Workbook wb)
        {
            string reportName = p;
            string path = Path.Combine(Application.StartupPath, "Reports");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".xls");

            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            try
            {
                wb.Save(path, FileFormatType.Excel2003);
                System.Diagnostics.Process.Start(path);
                MotherForm.SetStatusBarMessage(reportName + "已匯出完成", 100);
            }
            catch
            {
                System.Windows.Forms.SaveFileDialog sd1 = new System.Windows.Forms.SaveFileDialog();
                sd1.Title = "另存新檔";
                sd1.FileName = reportName + ".xls";
                sd1.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd1.FileName, FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(sd1.FileName);
                        MotherForm.SetStatusBarMessage(reportName + "已匯出完成", 100);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }
    }
}
