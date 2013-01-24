using System;
using System.Collections.Generic;
using System.Text;
using SmartSchool.Customization.PlugIn;
using SmartSchool.Customization.Data;
using Aspose.Words;
using System.IO;
using System.Data;
using Aspose.Cells;
using System.Windows.Forms;

namespace 考試成績單範例
{
    public static class Program
    {
        [MainMethod()]
        public static void Main()
        {
            ButtonAdapter newReportButton = new ButtonAdapter();
            newReportButton.Text = "第一、二次與期末考加權成績排名";
            newReportButton.Path = "明德女中/成績相關報表";
            newReportButton.OnClick += new EventHandler(newReportButton_OnClick);
            SmartSchool.Customization.PlugIn.Report.ClassReport.AddReport(newReportButton);

        }

        static void newReportButton_OnClick(object sender, EventArgs e)
        {
            new SelectExamSubject().ShowDialog();
        }
    }
}
