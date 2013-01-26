using System;
using System.Collections.Generic;
using System.Text;
using Aspose.Words;
using AccountsReceivable.API;
using System.Drawing;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace AccountsReceivable.ReceiptUtility
{
    public class ReceiptDocument
    {
        private Document Template { get; set; }

        internal static Document DefaultTemplate { get; private set; }

        static ReceiptDocument()
        {
            MemoryStream ms = null;

            //ms = new MemoryStream(PrivateRC.AsposeLic);
            //ms.Seek(0, SeekOrigin.Begin);
            //Aspose.Words.License wordlic = new Aspose.Words.License();
            //wordlic.SetLicense(ms);

            //ms.Seek(0, SeekOrigin.Begin);
            //Aspose.BarCode.License barcodelic = new Aspose.BarCode.License();
            //barcodelic.SetLicense(ms);

            //ms.Close();

            //ms = new MemoryStream(PrivateRC.ReceiptTemplate);
            //ms.Seek(0, SeekOrigin.Begin);
            //ms = new MemoryStream(new FileStream(Application.StartupPath + "\\Customize\\校車繳費單樣版.doc", FileMode.Open));
            DefaultTemplate = new Document(Application.StartupPath + "\\Customize\\校車繳費單樣版.doc");
            //ms.Close();
        }

        /// <summary>
        /// 使用內鍵的樣版產生繳費單。
        /// </summary>
        public ReceiptDocument()
        {
            Template = DefaultTemplate;
        }

        /// <summary>
        /// 使用指定的樣版產生繳費單。
        /// </summary>
        /// <param name="template"></param>
        public ReceiptDocument(Document template)
        {
            Template = template;
        }

        public Document Generate(PaymentReceipt receipt)
        {
            Dictionary<string, object> datasource = new Dictionary<string, object>();

            datasource.Add("UniqueNumber", receipt.Sequence);
            datasource.Add("金額", receipt.Amount);
            datasource.Add("截止日", GetDisplayDateString(receipt.Expiration));
            datasource.Add("虛擬帳號", receipt.VirtualAccount);
            //datasource.Add("姓名", GetValueOrEmpty(receipt.Extensions, "姓名"));
            //datasource.Add("班級", GetValueOrEmpty(receipt.Extensions, "班級"));
            //datasource.Add("座號", GetValueOrEmpty(receipt.Extensions, "座號"));
            //datasource.Add("學號", GetValueOrEmpty(receipt.Extensions, "學號"));
            //datasource.Add("科別名稱", GetValueOrEmpty(receipt.Extensions, "科別名稱"));
            datasource.Add("超商條碼一", Utilities.CreateBarCode(GetValueOrEmpty(receipt.Codes, "Shop1")));
            datasource.Add("超商條碼二", Utilities.CreateBarCode(GetValueOrEmpty(receipt.Codes, "Shop2")));
            datasource.Add("超商條碼三", Utilities.CreateBarCode(GetValueOrEmpty(receipt.Codes, "Shop3")));
            datasource.Add("郵局條碼一", Utilities.CreateBarCode(GetValueOrEmpty(receipt.Codes, "Post1")));
            datasource.Add("郵局條碼二", Utilities.CreateBarCode(GetValueOrEmpty(receipt.Codes, "Post2")));
            datasource.Add("郵局條碼三", Utilities.CreateBarCode(GetValueOrEmpty(receipt.Codes, "Post3")));
            //datasource.Add("繳款明細", GetPaymentItemsString(receipt.Extensions));
            datasource.Add("繳款明細", "校車收費");
            AddCustomeMergeField(datasource, receipt.Extensions);

            Document payForm = Template.Clone();
            payForm.MailMerge.MergeField += new Aspose.Words.Reporting.MergeFieldEventHandler(MailMerge_MergeField);
            payForm.MailMerge.Execute(new List<string>(datasource.Keys).ToArray(), new List<object>(datasource.Values).ToArray());
            payForm.MailMerge.MergeField -= new Aspose.Words.Reporting.MergeFieldEventHandler(MailMerge_MergeField);
            return payForm;
        }

        public Document Generate1(List<PaymentReceipt> receipt)
        {
            //產生新生校車用繳費單
            DataTable dt = new DataTable();
            Dictionary<string, object> datasource = new Dictionary<string, object>();
            string rangename = "";

            dt.Columns.Add("UniqueNumber", typeof(string));
            dt.Columns.Add("金額", typeof(string));
            dt.Columns.Add("截止日", typeof(string));
            dt.Columns.Add("虛擬帳號", typeof(string));
            dt.Columns.Add("超商條碼一", typeof(object));
            dt.Columns.Add("超商條碼二", typeof(object));
            dt.Columns.Add("超商條碼三", typeof(object));
            dt.Columns.Add("郵局條碼一", typeof(object));
            dt.Columns.Add("郵局條碼二", typeof(object));
            dt.Columns.Add("郵局條碼三", typeof(object));
            dt.Columns.Add("繳款明細", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("科別", typeof(string));
            dt.Columns.Add("學號", typeof(string));
            dt.Columns.Add("代碼", typeof(string));
            dt.Columns.Add("站名", typeof(string));
            dt.Columns.Add("繳款期限", typeof(string));
            dt.Columns.Add("開始日期", typeof(string));
            dt.Columns.Add("結束日期", typeof(string));
            dt.Columns.Add("搭車天數", typeof(string));
            dt.Columns.Add("年度", typeof(string));
            dt.Columns.Add("學期", typeof(string));
            dt.Columns.Add("名稱", typeof(string));

            foreach (PaymentReceipt var in receipt)
            {
                rangename = var.Extensions["校車收費名稱"].Substring(var.Extensions["校車收費名稱"].IndexOf("日校校車") + 4, var.Extensions["校車收費名稱"].IndexOf("月") - var.Extensions["校車收費名稱"].IndexOf("日校校車") - 3);
                dt.Rows.Add(var.Sequence,
                    var.Amount,
                    GetDisplayDateString(var.Expiration),
                    var.VirtualAccount,
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Shop1")),
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Shop2")),
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Shop3")),
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Post1")),
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Post2")),
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Post3")),
                    "校車收費",
                    var.Extensions["MergeField::姓名"],
                    var.Extensions["MergeField::科別"],
                    var.Extensions["MergeField::學號"],
                    var.Extensions["MergeField::代碼"],
                    var.Extensions["MergeField::站名"],
                    (var.Expiration.Value.Year - 1911).ToString() + "年" + var.Expiration.Value.Month.ToString() + "月" + var.Expiration.Value.Day.ToString() + "日",
                    var.Extensions.ContainsKey("開始日期") == true ? (DateTime.Parse(var.Extensions["開始日期"]).Year - 1911).ToString() + "年" + DateTime.Parse(var.Extensions["開始日期"]).Month.ToString() + "月" + DateTime.Parse(var.Extensions["開始日期"]).Day.ToString() + "日" : "",
                    var.Extensions.ContainsKey("結束日期") == true ? (DateTime.Parse(var.Extensions["結束日期"]).Year - 1911).ToString() + "年" + DateTime.Parse(var.Extensions["結束日期"]).Month.ToString() + "月" + DateTime.Parse(var.Extensions["結束日期"]).Day.ToString() + "日" : "",
                    var.Extensions.ContainsKey("搭車天數") == true ? var.Extensions["搭車天數"] : "",
                    var.Extensions.ContainsKey("校車收費年度") == true ? var.Extensions["校車收費年度"] : "",
                    var.Extensions.ContainsKey("校車收費學期") == true ? var.Extensions["校車收費學期"] : "",
                    var.Extensions.ContainsKey("校車收費名稱") == true ? rangename : ""
                    );
            }

            Document payForm = Template.Clone();
            payForm.MailMerge.MergeField += new Aspose.Words.Reporting.MergeFieldEventHandler(MailMerge_MergeField);
            payForm.MailMerge.Execute(dt);
            payForm.MailMerge.MergeField -= new Aspose.Words.Reporting.MergeFieldEventHandler(MailMerge_MergeField);
            return payForm;
        }

        // BMK 產生現有學生校車用繳費單
        public Document Generate2(List<PaymentReceipt> receipt)
        {
            //產生現有學生校車用繳費單
            DataTable dt = new DataTable();
            Dictionary<string, object> datasource = new Dictionary<string, object>();
            string rangename = "";

            dt.Columns.Add("UniqueNumber", typeof(string));
            dt.Columns.Add("金額", typeof(string));
            dt.Columns.Add("截止日", typeof(string));
            dt.Columns.Add("虛擬帳號", typeof(string));
            dt.Columns.Add("超商條碼一", typeof(object));
            dt.Columns.Add("超商條碼二", typeof(object));
            dt.Columns.Add("超商條碼三", typeof(object));
            dt.Columns.Add("郵局條碼一", typeof(object));
            dt.Columns.Add("郵局條碼二", typeof(object));
            dt.Columns.Add("郵局條碼三", typeof(object));
            dt.Columns.Add("繳款明細", typeof(string));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("班級", typeof(string));
            dt.Columns.Add("學號", typeof(string));
            dt.Columns.Add("代碼", typeof(string));
            dt.Columns.Add("站名", typeof(string));
            dt.Columns.Add("繳款期限", typeof(string));
            dt.Columns.Add("開始日期", typeof(string));
            dt.Columns.Add("結束日期", typeof(string));
            dt.Columns.Add("搭車天數", typeof(string));
            dt.Columns.Add("年度", typeof(string));
            dt.Columns.Add("學期", typeof(string));
            dt.Columns.Add("名稱", typeof(string));

            foreach (PaymentReceipt var in receipt)
            {
                rangename = var.Extensions["校車收費名稱"].Substring(var.Extensions["校車收費名稱"].IndexOf("日校校車") + 4, var.Extensions["校車收費名稱"].IndexOf("月") - var.Extensions["校車收費名稱"].IndexOf("日校校車") - 3);
                dt.Rows.Add(var.Sequence,
                    var.Amount,
                    GetDisplayDateString(var.Expiration),
                    var.VirtualAccount,
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Shop1")),
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Shop2")),
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Shop3")),
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Post1")),
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Post2")),
                    Utilities.CreateBarCode(GetValueOrEmpty(var.Codes, "Post3")),
                    "校車收費",
                    var.Extensions["MergeField::姓名"],
                    var.Extensions["MergeField::班級"],
                    var.Extensions["MergeField::學號"],
                    var.Extensions["MergeField::代碼"],
                    var.Extensions["MergeField::站名"],
                    (var.Expiration.Value.Year - 1911).ToString() + "年" + var.Expiration.Value.Month.ToString() + "月" + var.Expiration.Value.Day.ToString() + "日",
                    var.Extensions.ContainsKey("開始日期") == true ? (DateTime.Parse(var.Extensions["開始日期"]).Year - 1911).ToString() + "年" + DateTime.Parse(var.Extensions["開始日期"]).Month.ToString() + "月" + DateTime.Parse(var.Extensions["開始日期"]).Day.ToString() + "日" : "",
                    var.Extensions.ContainsKey("結束日期") == true ? (DateTime.Parse(var.Extensions["結束日期"]).Year - 1911).ToString() + "年" + DateTime.Parse(var.Extensions["結束日期"]).Month.ToString() + "月" + DateTime.Parse(var.Extensions["結束日期"]).Day.ToString() + "日" : "",
                    var.Extensions.ContainsKey("搭車天數") == true ? var.Extensions["搭車天數"] : "",
                    var.Extensions.ContainsKey("校車收費年度") == true ? var.Extensions["校車收費年度"] : "",
                    var.Extensions.ContainsKey("校車收費學期") == true ? var.Extensions["校車收費學期"] : "",
                    //var.Extensions.ContainsKey("校車收費名稱") == true ? var.Extensions["校車收費名稱"].Substring(var.Extensions["校車收費名稱"].IndexOf("日校校車") + 4, var.Extensions["校車收費名稱"].IndexOf("月份") - var.Extensions["校車收費名稱"].IndexOf("日校校車") + 1) : ""
                    var.Extensions.ContainsKey("校車收費名稱") == true ? rangename : ""
                    );
            }

            Document payForm = Template.Clone();
            payForm.MailMerge.MergeField += new Aspose.Words.Reporting.MergeFieldEventHandler(MailMerge_MergeField);
            payForm.MailMerge.Execute(dt);
            payForm.MailMerge.MergeField -= new Aspose.Words.Reporting.MergeFieldEventHandler(MailMerge_MergeField);
            return payForm;
        }

        private static string GetValueOrEmpty(Dictionary<string, string> dic, string key)
        {
            if (dic.ContainsKey(key))
                return dic[key];
            else
                return string.Empty;
        }

        //MergeField::
        private static void AddCustomeMergeField(Dictionary<string, object> datasource, Dictionary<string, string> exts)
        {
            foreach (KeyValuePair<string, string> each in exts)
            {
                if (!each.Key.StartsWith("MergeField::")) continue;

                datasource.Add(each.Key.Split(new string[] { "MergeField::" }, StringSplitOptions.RemoveEmptyEntries)[0], each.Value);
            }
        }

        //PayItem::
        private static string GetPaymentItemsString(Dictionary<string, string> exts)
        {
            StringBuilder items = new StringBuilder();
            foreach (KeyValuePair<string, string> each in exts)
            {
                if (!each.Key.StartsWith("PayItem::")) continue;

                if (items.Length > 0)
                    items.AppendLine();

                items.Append(each.Key.Split(new string[] { "PayItem::" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                items.Append("\t"); //一個 Tab
                items.Append(each.Value);
            }

            return items.ToString();
        }

        private static void MailMerge_MergeField(object sender, Aspose.Words.Reporting.MergeFieldEventArgs e)
        {
            if (string.IsNullOrEmpty(e.FieldValue + ""))
                e.Field.Remove();

            if (e.FieldValue is Image)
            {
                DocumentBuilder builder = new DocumentBuilder(e.Document);
                builder.MoveToField(e.Field, false);
                double width1 = (builder.CurrentParagraph.ParentNode as Aspose.Words.Cell).CellFormat.Width;
                double width2 = (e.FieldValue as Image).Width / 6;
                double width = Math.Min(width1, width2);
                builder.InsertImage(e.FieldValue as Image, width, 40);
                e.Field.Remove();
            }
        }

        #region GetDisplayDateString
        private static string GetDisplayDateString(DateTime? dt)
        {
            if (dt == null)
                return null;
            else
                return Utilities.GetMinGuoDateString(dt.Value);
        }
        #endregion
    }
}
