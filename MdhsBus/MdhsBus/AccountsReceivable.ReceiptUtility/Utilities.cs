using System;
using System.Collections.Generic;
using System.Text;
using Aspose.BarCode;
using System.Drawing;
using System.IO;
using Aspose.Words;

namespace AccountsReceivable.ReceiptUtility
{
    internal static class Utilities
    {
        public static string GetMinGuoDateString(DateTime dt)
        {
            int year = (dt.Year >= 1911) ? (dt.Year - 1911) : dt.Year;
            string month = GetWellFormedString(dt.Month.ToString(), 2, "");
            string day = GetWellFormedString(dt.Day.ToString(), 2, "");
            StringBuilder sb = new StringBuilder(" ");
            sb.Append(year.ToString());
            sb.Append("年 ");
            sb.Append(month);
            sb.Append("月 ");
            sb.Append(day);
            sb.Append("日");
            return sb.ToString();
        }

        /// <summary>
        /// 將輸入的字串，靠右左補零，直到指定的位數。
        /// </summary>
        /// <param name="str">輸入字串</param>
        /// <param name="power">要填滿的位數</param>
        /// <param name="exceptionMessage">例外時的訊息</param>
        /// <returns>補零完後的字串</returns>
        public static string GetWellFormedString(string strOrig, int power, string exceptionMessage)
        {
            string str = strOrig;

            if (str.Length > power)
                throw new Exception(exceptionMessage);

            if (str.Length < power)
            {
                int size = power - str.Length;
                for (int i = 0; i < size; i++)
                {
                    str = "0" + str;
                }
            }
            return str;
        }

        public static Image CreateBarCode(string codeText)
        {
            if (string.IsNullOrEmpty(codeText)) return null;

            BarCodeBuilder barcode = new BarCodeBuilder(codeText, Symbology.Code39Standard);
            barcode.BarHeight = 7.5f;
            barcode.xDimension = 0.2f;
            barcode.WideNarrowRatio = 1.8f;
            barcode.CodeTextAlignment = StringAlignment.Near;
            barcode.Resolution = new Resolution(300, 300, ResolutionMode.Printer);

            Image bitmap = barcode.GenerateBarCodeImage();
            Bitmap sbitmap = new Bitmap(bitmap, new Size((int)(bitmap.Width), bitmap.Height));
            return sbitmap;
        }

        public static string GetBase64String(Document doc)
        {
            Stream stream = new MemoryStream();
            doc.Save(stream, SaveFormat.Doc);
            string result = GetBase64String(stream);
            stream.Close();
            return result;
        }

        public static string GetBase64String(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            byte[] rawdata = new byte[stream.Length];
            stream.Read(rawdata, 0, rawdata.Length);
            stream.Close();
            return Convert.ToBase64String(rawdata);
        }

        public static Document GetDocumentObject(string base64String)
        {
            byte[] rawdata = Convert.FromBase64String(base64String);
            Stream stream = new MemoryStream(rawdata);
            Document doc = new Document(stream);
            stream.Close();

            return doc;
        }
    }
}
