using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.Presentation;
using FISCA;

namespace 班級學生電子報表
{
    public class Program
    {
        [MainMethod]
        public static void Main()
        {

            RibbonBarItem item = MotherForm.RibbonBarItems["班級", "資料統計"];
            item["報表"]["明德女中"]["電子報表"]["上傳電子報表"].Click += delegate
            {
                Form1 F = new Form1();
                F.ShowDialog();
            };

            item = MotherForm.RibbonBarItems["班級", "資料統計"];
            item["報表"]["明德女中"]["電子報表"]["刪除班級電子報表"].Click += delegate
            {
                DataGridViewForm F = new DataGridViewForm();
                F.ShowDialog();
            };

        }
    }
}
