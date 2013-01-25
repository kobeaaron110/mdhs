using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using K12.Data;
using MdhsBus.Data;
using Aspose.Cells;
using FISCA.Presentation;
using FISCA.UDT;
using System.IO;

namespace MdhsBus
{
    public partial class Surveyform : BaseForm
    {
        public Surveyform()
        {
            InitializeComponent();
        }

        private void lnkSelAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < SelectView.Items.Count; i++)
            {
                this.SelectView.Items[i].Checked = true;
            }
            this.btnReport.Enabled = true;
            if (this.cboYear.Text != "" && this.cboRange.Text != "")
            {
                this.btnRollbook.Enabled = true;
                this.btnTicket.Enabled = true;
                this.btnBus.Enabled = true;
            }
            else
            {
                this.btnRollbook.Enabled = false;
                this.btnTicket.Enabled = false;
                this.btnBus.Enabled = false;
            }
        }

        private void lnkCancel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < SelectView.Items.Count; i++)
            {
                this.SelectView.Items[i].Checked = false;
            }
            this.btnReport.Enabled = false;
            this.btnRollbook.Enabled = false;
            this.btnTicket.Enabled = false;
            this.btnBus.Enabled = false;
        }

        private void SelectView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            bool btn_visible = false;
            bool btn_visible_all = true;
            for (int i = 0; i < SelectView.Items.Count; i++)
            {
                if (this.SelectView.Items[i].Checked == true)
                {
                    btn_visible = true;
                    break;
                }
            }
            for (int i = 0; i < SelectView.Items.Count; i++)
            {
                if (this.SelectView.Items[i].Checked == false)
                    btn_visible_all = false;
            }
            this.btnReport.Enabled = btn_visible_all;
            if (this.cboYear.Text != "" && this.cboRange.Text != "")
            {
                this.btnRollbook.Enabled = btn_visible_all;
                this.btnTicket.Enabled = btn_visible;
                this.btnBus.Enabled = btn_visible;
            }
            else
            {
                this.btnRollbook.Enabled = false;
                this.btnTicket.Enabled = false;
                this.btnBus.Enabled = false;
            }
        }

        private void btnBus_Click(object sender, EventArgs e)
        {
            //可存取 UDT 的工具類別
            //AccessHelper udtHelper = new AccessHelper();
            //List<Data.StudentByBus> busstu = Data.StudentByBusDAO.SelectByBusYearAndTimeName(101, "101-日校校車9.10月份");
            //foreach (Data.StudentByBus var in busstu)
            //{
            //    if (var.UID == "8197" || var.UID == "8983" || var.UID == "9500" || var.UID == "9056")
            //    {
            //        List<ActiveRecord> newStudents = new List<ActiveRecord>();
            //        //var.ClassName = "廣一乙";
            //        //var.StudentID = "19719";
            //        //var.BusMoney = 5310;
            //        var.comment = "改單：原為3015富功國小，金額為4600";
            //        newStudents.Add(var);
            //        udtHelper.UpdateValues(newStudents);
            //    }
            //}

            //List<StudentByBus> updtaeStudentByBuses = new List<StudentByBus>();
            //List<StudentByBus> StudentByBuses = StudentByBusDAO.SelectByBusYearAndTimeName(int.Parse(this.cboYear.Text), "日校校車11.12.1月份");
            //foreach (StudentByBus var in StudentByBuses)
            //{
            //    var.BusRangeName = this.cboRange.Text;
            //    updtaeStudentByBuses.Add(var);
            //}
            //updtaeStudentByBuses.SaveAll();

            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(int.Parse(this.cboYear.Text), this.cboRange.Text);
            List<Data.StudentByBus> _Source = new List<Data.StudentByBus>();
            int Selectcount = 0;
            for (int i = 0; i < SelectView.Items.Count; i++)
                if (this.SelectView.Items[i].Checked == true)
                    Selectcount++;
            if (Selectcount == SelectView.Items.Count)
                _Source = Data.StudentByBusDAO.SelectByBusYearAndTimeName(int.Parse(this.cboYear.Text), this.cboRange.Text);
            else
            {
                for (int i = 0; i < SelectView.Items.Count; i++)
                {
                    List<Data.StudentByBus> _Source1 = new List<Data.StudentByBus>();
                    if (this.SelectView.Items[i].Checked == true)
                    {
                        _Source1 = Data.StudentByBusDAO.SelectByBusYearAndTimeNameAndClass(int.Parse(this.cboYear.Text), this.cboRange.Text, this.SelectView.Items[i].Text);
                        _Source.AddRange(_Source1);
                    }
                }
            }
            List<StudentRecord> studentclass = K12.Data.Student.SelectAll();
            Dictionary<string, StudentRecord> studentids = new Dictionary<string, StudentRecord>();
            int nowSet = 0;
            foreach (StudentRecord var in studentclass)
            {
                MotherForm.SetStatusBarMessage("正在讀取學生資料", nowSet++ * 20 / studentclass.Count);
                //if (var.Status.ToString() != "一般")
                //    continue;
                if (var.Class != null)
                {
                    if (var.Class.GradeYear > 3)
                        continue;
                    if (var.Class.GradeYear == 1)
                    {
                        if (int.Parse(var.Class.DisplayOrder) > 23)
                            continue;
                    }
                    else
                    {
                        if (int.Parse(var.Class.DisplayOrder) > 24)
                            continue;
                    }
                    if (!studentids.ContainsKey(var.ID))
                        studentids.Add(var.ID, var);
                }
                else
                    if (!studentids.ContainsKey(var.ID))
                        studentids.Add(var.ID, var);
            }

            Workbook wb = new Workbook();
            Style defaultStyle = wb.DefaultStyle;
            defaultStyle.Font.Name = "標楷體";
            defaultStyle.Font.Size = 12;
            wb.DefaultStyle = defaultStyle;
            int row = 0;
            nowSet = 0;
            wb.Worksheets[0].Name = "學生搭乘校車明細表";

            //校車路線站名資料
            wb.Worksheets[0].Cells[row, 0].PutValue("搭車年度");
            wb.Worksheets[0].Cells[row, 1].PutValue("校車時段");
            wb.Worksheets[0].Cells[row, 2].PutValue("期間名稱");
            wb.Worksheets[0].Cells[row, 3].PutValue("班級");
            wb.Worksheets[0].Cells[row, 4].PutValue("學號");
            wb.Worksheets[0].Cells[row, 5].PutValue("電腦代號");
            wb.Worksheets[0].Cells[row, 6].PutValue("姓名");
            wb.Worksheets[0].Cells[row, 7].PutValue("是否繳費");
            wb.Worksheets[0].Cells[row, 8].PutValue("繳費日期");
            wb.Worksheets[0].Cells[row, 9].PutValue("天數");
            wb.Worksheets[0].Cells[row, 10].PutValue("備註");
            row++;
            //buses.Sort(CompareBusNumber);
            foreach (Data.StudentByBus var in _Source)
            {
                MotherForm.SetStatusBarMessage("正在產生明細表", 20 + nowSet++ * 100 / _Source.Count);
                if (var.ClassName.IndexOf("科") > 0)
                    continue;
                wb.Worksheets[0].Cells[row, 0].PutValue(int.Parse(this.cboYear.Text));
                wb.Worksheets[0].Cells[row, 1].PutValue(var.BusTimeName);
                wb.Worksheets[0].Cells[row, 2].PutValue(var.BusRangeName);
                wb.Worksheets[0].Cells[row, 3].PutValue(var.ClassName);
                if (studentids.ContainsKey(var.StudentID))
                {
                    wb.Worksheets[0].Cells[row, 4].PutValue(studentids[var.StudentID].StudentNumber);
                    wb.Worksheets[0].Cells[row, 6].PutValue(studentids[var.StudentID].Name);
                }
                else
                {
                    wb.Worksheets[0].Cells[row, 4].PutValue(var.StudentID);
                    wb.Worksheets[0].Cells[row, 6].PutValue("離校或休學");
                }
                wb.Worksheets[0].Cells[row, 5].PutValue(var.BusStopID);
                wb.Worksheets[0].Cells[row, 7].PutValue(var.PayStatus == true ? "是" : "否");
                wb.Worksheets[0].Cells[row, 8].PutValue(var.PayDate.ToShortDateString() == "0001/1/1" ? "" : var.PayDate.ToShortDateString());
                wb.Worksheets[0].Cells[row, 9].PutValue(var.DateCount);
                wb.Worksheets[0].Cells[row, 10].PutValue(var.comment);
                row++;
            }
            wb.Worksheets[0].AutoFitColumns();
            try
            {
                wb.Save(Application.StartupPath + "\\Reports\\學生搭乘校車明細表.xls", FileFormatType.Excel2003);
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Reports\\學生搭乘校車明細表.xls");
                MotherForm.SetStatusBarMessage("學生搭乘校車明細表已匯出完成", 100);
            }
            catch
            {
                System.Windows.Forms.SaveFileDialog sd1 = new System.Windows.Forms.SaveFileDialog();
                sd1.Title = "另存新檔";
                sd1.FileName = "學生搭乘校車明細表.xls";
                sd1.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd1.FileName, FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(sd1.FileName);
                        MotherForm.SetStatusBarMessage("學生搭乘校車明細表已匯出完成", 100);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            MotherForm.SetStatusBarMessage("正在處理資料...");
            List<StudentRecord> studentclass = K12.Data.Student.SelectAll();
            List<StudentRecord> studentActive =new List<StudentRecord>();
            Dictionary<string, StudentRecord> studentActiveRecord = new Dictionary<string, StudentRecord>();
            int nowSet = 0;
            foreach (StudentRecord var in studentclass)
            {
                MotherForm.SetStatusBarMessage("正在讀取學生資料", nowSet++ * 15 / studentclass.Count);
                if (var.Status.ToString() != "一般")
                    continue;
                if (var.Class.GradeYear == null)
                    continue;
                if (var.Class.GradeYear > 3)
                    continue;
                if (var.Class.GradeYear == 1)
                {
                    if (int.Parse(var.Class.DisplayOrder) > 23)
                        continue;
                }
                else
                {
                    if (int.Parse(var.Class.DisplayOrder) > 24)
                        continue;
                }
                studentActive.Add(var);
                if (!studentActiveRecord.ContainsKey(var.ID))
                    studentActiveRecord.Add(var.ID, var);
            }
            studentActive.Sort(CompareClassNumber);

            List<BusSetup> bussetups = BusSetupDAO.SelectAll();
            BusSetup bussetup = new BusSetup();
            bussetups.Sort(CompareBusSetup);
            foreach (BusSetup var in bussetups)
            {
                bussetup = var;
                break;
            }
            List<Data.StudentByBus> _Source = StudentByBusDAO.SelectByBusYearAndTimeName(bussetup.BusYear, bussetup.BusRangeName);
            Dictionary<string, StudentByBus> stuBuses = new Dictionary<string, StudentByBus>();
            nowSet = 0;
            foreach (StudentByBus var in _Source)
            {
                MotherForm.SetStatusBarMessage("正在讀取搭車資料", 15 + nowSet++ * 15 / _Source.Count);
                if (!studentActiveRecord.ContainsKey(var.StudentID))
                    continue;
                if (!stuBuses.ContainsKey(var.StudentID))
                    stuBuses.Add(var.StudentID, var);
            }

            List<BusStop> busstop = BusStopDAO.SelectByBusTimeName(bussetup.BusTimeName);
            Dictionary<string, BusStop> busstops = new Dictionary<string, BusStop>();
            foreach (BusStop var in busstop)
            {
                if (!busstops.ContainsKey(var.BusStopID))
                    busstops.Add(var.BusStopID, var);
            }

            #region 建立樣板
            Workbook template = new Workbook();
            template.Open(new MemoryStream(Properties.Resources.學生搭車調查表), FileFormatType.Excel2003);
            Workbook wb = new Workbook();
            wb.Open(new MemoryStream(Properties.Resources.學生搭車調查表));
            Worksheet sheet = wb.Worksheets[0];
            #endregion
            Style defaultStyle = wb.DefaultStyle;
            defaultStyle.Font.Name = "標楷體";
            defaultStyle.Font.Size = 12;
            wb.DefaultStyle = defaultStyle;
            int row = 0;
            nowSet = 0;
            sheet.Name = "學生搭乘校車調查表";

            #region 第2列加特殊邊
            Aspose.Cells.Range eachFiveRow = sheet.Cells.CreateRange(row, 0, 1, 8);
            eachFiveRow.SetOutlineBorder(Aspose.Cells.BorderType.TopBorder, CellBorderType.Medium, Color.Black);
            eachFiveRow.SetOutlineBorder(Aspose.Cells.BorderType.BottomBorder, CellBorderType.Medium, Color.Black);
            //校車路線站名資料
            sheet.Cells[row, 0].PutValue("班級");
            sheet.Cells[row, 1].PutValue("學號");
            sheet.Cells[row, 2].PutValue("姓名");
            sheet.Cells[row, 3].PutValue("代碼");
            sheet.Cells[row, 4].PutValue("站名");
            sheet.Cells[row, 5].PutValue("不搭乘");
            sheet.Cells[row, 6].PutValue("要搭乘");
            sheet.Cells[row, 7].PutValue("簽名");
            sheet.Cells[row, 4].Style.HorizontalAlignment = TextAlignmentType.Center;
            row++;
            #endregion
            //buses.Sort(CompareBusNumber);
            string oldclass = "";
            int oldclass_stucount = 0;
            foreach (StudentRecord var in studentActive)
            {
                MotherForm.SetStatusBarMessage("正在產生調查表", 30 + nowSet++ * 100 / studentActive.Count);
                if (var.Class.Name != oldclass && oldclass != "")
                {
                    sheet.Cells[row, 0].PutValue("班級人數");
                    sheet.Cells[row, 1].PutValue(oldclass_stucount.ToString() + "人");
                    sheet.Cells.SetRowHeight(row, 15);
                    row++;
                    sheet.HPageBreaks.Add(row, 6);
                    oldclass_stucount = 0;
                }
                eachFiveRow = sheet.Cells.CreateRange(row, 0, 1, 8);
                sheet.Cells[row, 0].PutValue(var.Class.Name);
                sheet.Cells[row, 1].PutValue(var.StudentNumber);
                sheet.Cells[row, 2].PutValue(var.Name);
                if (stuBuses.ContainsKey(var.ID))
                {
                    if (stuBuses[var.ID].BusStopID != "0000")
                    {
                        sheet.Cells[row, 3].PutValue(stuBuses[var.ID].BusStopID);
                        if (busstops.ContainsKey(stuBuses[var.ID].BusStopID))
                            sheet.Cells[row, 4].PutValue(busstops[stuBuses[var.ID].BusStopID].BusStopName);
                    }
                }
                sheet.Cells[row, 5].PutValue("□");
                sheet.Cells[row, 6].PutValue("□");
                sheet.Cells.SetRowHeight(row, 15);
                eachFiveRow.SetOutlineBorder(Aspose.Cells.BorderType.BottomBorder, CellBorderType.Thin, Color.Black);
                oldclass = var.Class.Name;
                oldclass_stucount++;
                row++;
            }
            sheet.Cells[row, 0].PutValue("班級人數");
            sheet.Cells[row, 1].PutValue(oldclass_stucount.ToString() + "人");
            sheet.Cells.SetRowHeight(row, 15);
            sheet.PageSetup.TopMargin = 0.7;
            sheet.PageSetup.BottomMargin = 1.5;
            sheet.PageSetup.LeftMargin = 1.2;
            sheet.PageSetup.RightMargin = 1.2;
            sheet.PageSetup.CenterHorizontally = true;
            sheet.PageSetup.Zoom = 95;
            //sheet.PageSetup.SetFooter(0, "&[日期]");
            //sheet.PageSetup.SetFooter(2, "第 &[頁碼] 頁，共 &[總頁數] 頁");
            sheet.PageSetup.PrintTitleRows = "$1:$1";
            sheet.AutoFitColumns();

            try
            {
                wb.Save(Application.StartupPath + "\\Reports\\學生搭乘校車調查表.xls", FileFormatType.Excel2003);
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Reports\\學生搭乘校車調查表.xls");
                MotherForm.SetStatusBarMessage("學生搭乘校車調查表已匯出完成", 100);
            }
            catch
            {
                System.Windows.Forms.SaveFileDialog sd1 = new System.Windows.Forms.SaveFileDialog();
                sd1.Title = "另存新檔";
                sd1.FileName = "學生搭乘校車調查表.xls";
                sd1.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd1.FileName, FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(sd1.FileName);
                        MotherForm.SetStatusBarMessage("學生搭乘校車調查表已匯出完成", 100);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void btnRollbook_Click(object sender, EventArgs e)
        {
            MotherForm.SetStatusBarMessage("正在處理資料...");
            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(int.Parse(this.cboYear.Text), this.cboRange.Text);
            List<Data.StudentByBus> _Source = new List<Data.StudentByBus>();
            int Selectcount = 0;
            for (int i = 0; i < SelectView.Items.Count; i++)
                if (this.SelectView.Items[i].Checked == true)
                    Selectcount++;
            if (Selectcount == SelectView.Items.Count)
                _Source = Data.StudentByBusDAO.SelectByBusYearAndTimeName(int.Parse(this.cboYear.Text), this.cboRange.Text);

            List<BusStop> busstop = BusStopDAO.SelectByBusTimeName(bussetup.BusTimeName);
            Dictionary<string, BusStop> busstops = new Dictionary<string, BusStop>();
            foreach (BusStop var in busstop)
            {
                if (!busstops.ContainsKey(var.BusStopID))
                    busstops.Add(var.BusStopID, var);
            }
            List<StudentRecord> studentclass = K12.Data.Student.SelectAll();
            Dictionary<string, StudentRecord> studentids = new Dictionary<string, StudentRecord>();
            int nowSet = 0;
            foreach (StudentRecord var in studentclass)
            {
                MotherForm.SetStatusBarMessage("正在讀取學生資料", nowSet++ * 15 / studentclass.Count);
                if (var.Status != StudentRecord.StudentStatus.一般)
                    continue;

                if (var.Class != null)
                {
                    if (var.Class.GradeYear > 3)
                        continue;
                    if (var.Class.GradeYear == 1)
                    {
                        if (int.Parse(var.Class.DisplayOrder) > 23)
                            continue;
                    }
                    else
                    {
                        if (int.Parse(var.Class.DisplayOrder) > 24)
                            continue;
                    }
                    if (!studentids.ContainsKey(var.ID))
                        studentids.Add(var.ID, var);
                }
                else
                    if (!studentids.ContainsKey(var.ID))
                        studentids.Add(var.ID, var);
            }
            Dictionary<string, Dictionary<string, List<StudentByBus>>> busstudents = new Dictionary<string, Dictionary<string, List<StudentByBus>>>();
            List<string> busnoes = new List<string>();
            Dictionary<string, List<string>> stopnoes = new Dictionary<string, List<string>>();
            nowSet = 0;
            foreach (Data.StudentByBus var in _Source)
            {
                MotherForm.SetStatusBarMessage("正在讀取搭車資料", 15 + nowSet++ * 15 / _Source.Count);
                if (var.ClassName.IndexOf("科") > 0)
                    continue;
                if (!studentids.ContainsKey(var.StudentID))
                    continue;   

                if (!busnoes.Contains(busstops[var.BusStopID].BusNumber))
                {
                    busnoes.Add(busstops[var.BusStopID].BusNumber);
                    stopnoes.Add(busstops[var.BusStopID].BusNumber,new List<string>());
                }
                if (!busstudents.ContainsKey(busstops[var.BusStopID].BusNumber))
                    busstudents.Add(busstops[var.BusStopID].BusNumber, new Dictionary<string, List<StudentByBus>>());
                if (!busstudents[busstops[var.BusStopID].BusNumber].ContainsKey(busstops[var.BusStopID].BusStopNo))
                    busstudents[busstops[var.BusStopID].BusNumber].Add(busstops[var.BusStopID].BusStopNo, new List<StudentByBus>());
                busstudents[busstops[var.BusStopID].BusNumber][busstops[var.BusStopID].BusStopNo].Add(var);
                if (!stopnoes[busstops[var.BusStopID].BusNumber].Contains(busstops[var.BusStopID].BusStopNo))
                    stopnoes[busstops[var.BusStopID].BusNumber].Add(busstops[var.BusStopID].BusStopNo);
            }
            busnoes.Sort();

            #region 建立樣板
            Workbook template = new Workbook();
            template.Open(new MemoryStream(Properties.Resources.學生搭車點名單), FileFormatType.Excel2003);
            Workbook wb = new Workbook();
            wb.Open(new MemoryStream(Properties.Resources.學生搭車點名單));
            Worksheet sheet = wb.Worksheets[0];
            #endregion
            Style defaultStyle = wb.DefaultStyle;
            defaultStyle.Font.Name = "標楷體";
            defaultStyle.Font.Size = 12;
            wb.DefaultStyle = defaultStyle;
            int row = 0;
            nowSet = 0;
            wb.Worksheets[0].Name = "學生搭乘校車點名單";

            wb.Worksheets[0].Cells[row, 4].PutValue(int.Parse(this.cboYear.Text) + "年");
            wb.Worksheets[0].Cells[row, 5].PutValue(this.cboRange.Text);
            wb.Worksheets[0].Cells.Merge(row, 5, 1, 5);
            wb.Worksheets[0].Cells[row, 5].Style.ShrinkToFit = true;
            row++;

            string todayweek = DateTime.Today.DayOfWeek.ToString();
            DateTime NextMonday = new DateTime();
            switch (todayweek)
            {
                default:
                case "Monday":
                    NextMonday = DateTime.Today.AddDays(7);
                    break;
                case "Tuesday":
                    NextMonday = DateTime.Today.AddDays(6);
                    break;
                case "Wednesday":
                    NextMonday = DateTime.Today.AddDays(5);
                    break;
                case "Thursday":
                    NextMonday = DateTime.Today.AddDays(4);
                    break;
                case "Friday":
                    NextMonday = DateTime.Today.AddDays(3);
                    break;
                case "Saturday":
                    NextMonday = DateTime.Today.AddDays(2);
                    break;
                case "Sunday":
                    NextMonday = DateTime.Today.AddDays(1);
                    break;
            }

            #region 第2列加特殊邊
            Aspose.Cells.Range eachFiveRow = sheet.Cells.CreateRange(row, 0, 1, 10);
            eachFiveRow.SetOutlineBorder(Aspose.Cells.BorderType.TopBorder, CellBorderType.Medium, Color.Black);
            eachFiveRow.SetOutlineBorder(Aspose.Cells.BorderType.BottomBorder, CellBorderType.Medium, Color.Black);
            //校車路線站名資料
            wb.Worksheets[0].Cells[row, 1].PutValue("班級");
            wb.Worksheets[0].Cells[row, 2].PutValue("學號");
            wb.Worksheets[0].Cells[row, 3].PutValue("姓名");
            wb.Worksheets[0].Cells[row, 4].PutValue("站名");
            wb.Worksheets[0].Cells[row, 5].PutValue(NextMonday.Day + "一");
            wb.Worksheets[0].Cells[row, 6].PutValue(NextMonday.AddDays(1).Day + "二");
            wb.Worksheets[0].Cells[row, 7].PutValue(NextMonday.AddDays(2).Day + "三");
            wb.Worksheets[0].Cells[row, 8].PutValue(NextMonday.AddDays(3).Day + "四");
            wb.Worksheets[0].Cells[row, 9].PutValue(NextMonday.AddDays(4).Day + "五");
            //wb.Worksheets[0].Cells[row, 9].PutValue("聯絡電話");
            row++;
            #endregion
            //string oldclass = "";
            int oldclass_stucount = 0;
            for (int i = 0; i < busnoes.Count; i++)
            {
                stopnoes[busnoes[i]].Sort();
                for (int j = 0; j < stopnoes[busnoes[i]].Count; j++)
                {
                    busstudents[busnoes[i]][stopnoes[busnoes[i]][j]].Sort(CompareSNumber);
                    foreach (StudentByBus var in busstudents[busnoes[i]][stopnoes[busnoes[i]][j]])
                    {
                        MotherForm.SetStatusBarMessage("正在產生點名單", 30 + nowSet++ * 100 / _Source.Count);
                        wb.Worksheets[0].Cells[row, 0].PutValue(var.PayStatus == true ? "" : "未銷帳");
                        wb.Worksheets[0].Cells[row, 1].PutValue(studentids[var.StudentID].Class.Name);
                        wb.Worksheets[0].Cells[row, 2].PutValue(studentids[var.StudentID].StudentNumber);
                        wb.Worksheets[0].Cells[row, 3].PutValue(studentids[var.StudentID].Name);
                        wb.Worksheets[0].Cells[row, 4].PutValue(busstops[var.BusStopID].BusStopName);
                        wb.Worksheets[0].Cells[row, 5].PutValue("□");
                        wb.Worksheets[0].Cells[row, 6].PutValue("□");
                        wb.Worksheets[0].Cells[row, 7].PutValue("□");
                        wb.Worksheets[0].Cells[row, 8].PutValue("□");
                        wb.Worksheets[0].Cells[row, 9].PutValue("□");
                        //wb.Worksheets[0].Cells[row, 9].PutValue(var.DateCount);
                        //oldclass = var.ClassName;
                        oldclass_stucount++;
                        row++;
                    }
                }
                wb.Worksheets[0].Cells[row, 0].PutValue("車號" + busnoes[i]);
                wb.Worksheets[0].Cells[row, 1].PutValue("乘車共");
                wb.Worksheets[0].Cells[row, 2].PutValue(oldclass_stucount.ToString() + "人");
                wb.Worksheets[0].Cells[row, 4].PutValue("車隊長簽名：");
                wb.Worksheets[0].Cells[row, 2].Style.HorizontalAlignment = TextAlignmentType.Left;
                wb.Worksheets[0].Cells[row, 4].Style.HorizontalAlignment = TextAlignmentType.Right;
                row++;
                wb.Worksheets[0].HPageBreaks.Add(row, 10);
                oldclass_stucount = 0;
            }
            wb.Worksheets[0].PageSetup.Zoom = 95;
            wb.Worksheets[0].AutoFitColumns();

            try
            {
                wb.Save(Application.StartupPath + "\\Reports\\學生搭乘校車點名單.xls", FileFormatType.Excel2003);
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Reports\\學生搭乘校車點名單.xls");
                MotherForm.SetStatusBarMessage("學生搭乘校車點名單已匯出完成", 100);
            }
            catch
            {
                System.Windows.Forms.SaveFileDialog sd1 = new System.Windows.Forms.SaveFileDialog();
                sd1.Title = "另存新檔";
                sd1.FileName = "學生搭乘校車點名單.xls";
                sd1.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd1.FileName, FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(sd1.FileName);
                        MotherForm.SetStatusBarMessage("學生搭乘校車點名單已匯出完成", 100);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void btnTicket_Click(object sender, EventArgs e)
        {
            MotherForm.SetStatusBarMessage("正在處理資料...");
            BusSetup bussetup = BusSetupDAO.SelectByBusYearAndRange(int.Parse(this.cboYear.Text), this.cboRange.Text);
            List<Data.StudentByBus> _Source = new List<Data.StudentByBus>();
            int Selectcount = 0;
            for (int i = 0; i < SelectView.Items.Count; i++)
                if (this.SelectView.Items[i].Checked == true)
                    Selectcount++;
            if (Selectcount == SelectView.Items.Count)
                _Source = Data.StudentByBusDAO.SelectByBusYearAndTimeName(int.Parse(this.cboYear.Text), this.cboRange.Text);
            else
            {
                for (int i = 0; i < SelectView.Items.Count; i++)
                {
                    List<Data.StudentByBus> _Source1 = new List<Data.StudentByBus>();
                    if (this.SelectView.Items[i].Checked == true)
                    {
                        _Source1 = Data.StudentByBusDAO.SelectByBusYearAndTimeNameAndClass(int.Parse(this.cboYear.Text), this.cboRange.Text, this.SelectView.Items[i].Text);
                        _Source.AddRange(_Source1);
                    }
                }
            }
            List<BusStop> busstop = BusStopDAO.SelectByBusTimeName(bussetup.BusTimeName);
            Dictionary<string, BusStop> busstops = new Dictionary<string, BusStop>();
            foreach (BusStop var in busstop)
            {
                if (!busstops.ContainsKey(var.BusStopID))
                    busstops.Add(var.BusStopID, var);
            }
            List<StudentRecord> studentclass = K12.Data.Student.SelectAll();
            Dictionary<string, StudentRecord> studentids = new Dictionary<string, StudentRecord>();
            foreach (StudentRecord var in studentclass)
            {
                if (var.Status != StudentRecord.StudentStatus.一般)
                    continue;

                if (var.Class != null)
                {
                    if (var.Class.GradeYear > 3)
                        continue;
                    if (var.Class.GradeYear == 1)
                    {
                        if (int.Parse(var.Class.DisplayOrder) > 23)
                            continue;
                    }
                    else
                    {
                        if (int.Parse(var.Class.DisplayOrder) > 24)
                            continue;
                    }
                    if (!studentids.ContainsKey(var.ID))
                        studentids.Add(var.ID, var);
                }
                else
                    if (!studentids.ContainsKey(var.ID))
                        studentids.Add(var.ID, var);
            }


            #region 建立樣板
            Workbook template = new Workbook();
            template.Open(new MemoryStream(Properties.Resources.學生搭車月票), FileFormatType.Excel2003);
            Workbook wb = new Workbook();
            wb.Open(new MemoryStream(Properties.Resources.學生搭車月票));
            Worksheet sheet = wb.Worksheets[0];
            #endregion
            Style defaultStyle = wb.DefaultStyle;
            defaultStyle.Font.Name = "標楷體";
            defaultStyle.Font.Size = 12;
            wb.DefaultStyle = defaultStyle;
            int row = 0;
            int nowSet = 0;
            sheet.Name = "學生搭乘校車月票";

            string oldclass = "";
            int oldclass_stucount = 0;
            int stucount = 0;
            row = 1;
            foreach (Data.StudentByBus var in _Source)
            {
                //if (var.ClassName != oldclass && oldclass != "")
                //{
                //    wb.Worksheets[0].Cells[row, 0].PutValue("班級人數");
                //    wb.Worksheets[0].Cells[row, 1].PutValue(oldclass_stucount.ToString() + "人");
                //    row++;
                //    wb.Worksheets[0].HPageBreaks.Add(row, 8);
                //    oldclass_stucount = 0;
                //}
                if (var.ClassName.IndexOf("科") > 0)
                    continue;
                if (!studentids.ContainsKey(var.StudentID))
                    continue;   

                if ((stucount % 3) == 0 && stucount > 0)
                {
                    //分辨第幾列
                    if ((stucount % 9) == 0)
                        row = row + 10;
                    else
                        row = row + 11;
                    for (int i = 0; i < 10; i++)
                    {
                        //由於這樣複製資料列，速度很慢，取消此方法
                        //sheet.Cells.CopyRow(sheet.Cells, i, row - 1 + i);
                        if (i == 0 || i == 9)
                            sheet.Cells.SetRowHeight(row - 1 + i, 30);
                        else if (i == 6 || i == 7)
                            sheet.Cells.SetRowHeight(row - 1 + i, 42);
                        else
                            sheet.Cells.SetRowHeight(row - 1 + i, 20);
                    }
                    sheet.Cells.SetRowHeight(row - 1 + 10, 20);
                }

                MotherForm.SetStatusBarMessage("正在產生校車月票", nowSet++ * 100 / _Source.Count);
                sheet.Cells[row - 1, (stucount % 3) * 6].PutValue("明德女中乘車月票");
                sheet.Cells.Merge(row - 1, (stucount % 3) * 6, 1, 5);
                sheet.Cells[row - 1, (stucount % 3) * 6].Style.Font.Size = 16;
                sheet.Cells[row, (stucount % 3) * 6].PutValue("班級");
                sheet.Cells[row, 1 + (stucount % 3) * 6].PutValue(studentids[var.StudentID].Class.Name);
                sheet.Cells.Merge(row, 1 + (stucount % 3) * 6, 1, 2);
                sheet.Cells[row, 3 + (stucount % 3) * 6].PutValue(this.cboYear.Text + "年" + var.BusRangeName);
                sheet.Cells.Merge(row, 3 + (stucount % 3) * 6, 1, 2);
                sheet.Cells[row, 3 + (stucount % 3) * 6].Style.ShrinkToFit = true;
                sheet.Cells[row + 1, (stucount % 3) * 6].PutValue("姓名");
                sheet.Cells[row + 1, 1 + (stucount % 3) * 6].PutValue(studentids[var.StudentID].Name);
                sheet.Cells.Merge(row + 1, 1 + (stucount % 3) * 6, 1, 2);
                sheet.Cells[row + 1, 3 + (stucount % 3) * 6].PutValue("站序");
                sheet.Cells[row + 1, 4 + (stucount % 3) * 6].PutValue(busstops[var.BusStopID].BusStopNo);
                sheet.Cells[row + 2, (stucount % 3) * 6].PutValue("學號");
                sheet.Cells[row + 2, 1 + (stucount % 3) * 6].PutValue(studentids[var.StudentID].StudentNumber);
                sheet.Cells.Merge(row + 2, 1 + (stucount % 3) * 6, 1, 2);
                sheet.Cells[row + 2, 3 + (stucount % 3) * 6].PutValue("票價");
                sheet.Cells[row + 2, 4 + (stucount % 3) * 6].PutValue(var.BusMoney);
                sheet.Cells[row + 3, (stucount % 3) * 6].PutValue("上車站名");
                sheet.Cells.Merge(row + 3, (stucount % 3) * 6, 1, 2);
                sheet.Cells[row + 3, 2 + (stucount % 3) * 6].PutValue(busstops[var.BusStopID].BusStopName);
                sheet.Cells.Merge(row + 3, 2 + (stucount % 3) * 6, 1, 3);
                sheet.Cells[row + 3, 2 + (stucount % 3) * 6].Style.ShrinkToFit = true;
                sheet.Cells[row + 4, (stucount % 3) * 6].PutValue("繳費月份");
                sheet.Cells.Merge(row + 4, (stucount % 3) * 6, 1, 4);
                sheet.Cells[row + 4, 4 + (stucount % 3) * 6].PutValue("承辦人");
                sheet.Cells[row + 5, 0 + (stucount % 3) * 6].PutValue(var.BusRangeName.Replace("日校校車", "").Replace("夜輔", "").Replace("晚自習", "").Replace("週六增廣", ""));
                sheet.Cells.Merge(row + 5, (stucount % 3) * 6, 1, 4);
                sheet.Cells[row + 5, (stucount % 3) * 6].Style.Font.Size = 16;
                sheet.Cells[row + 5, (stucount % 3) * 6].Style.ShrinkToFit = true;
                sheet.Cells[row + 6, 0 + (stucount % 3) * 6].PutValue(busstops[var.BusStopID].BusNumber);
                sheet.Cells.Merge(row + 6, (stucount % 3) * 6, 1, 4);
                sheet.Cells[row + 7, (stucount % 3) * 6].PutValue("未蓋承辦人印章無效");
                sheet.Cells.Merge(row + 7, (stucount % 3) * 6, 1, 5);
                sheet.Cells[row + 8, (stucount % 3) * 6].PutValue("備註");
                sheet.Cells.Merge(row + 8, 1 + (stucount % 3) * 6, 1, 4);
                //wb.Worksheets[0].Cells[row, 9].PutValue(var.DateCount);
                oldclass = var.ClassName;
                oldclass_stucount++;
                stucount++;
                //row++;
            }

            for (int i = 0; i <= sheet.Cells.MaxRow; i++)
            {
                if (((i - 10) % 32) == 0 || ((i - 21) % 32) == 0)
                {
                }
                else
                {
                    for (int j = 0; j <= sheet.Cells.MaxColumn; j++)
                    {
                        sheet.Cells[i, j].Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                        sheet.Cells[i, j].Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                        if (j != 5 && j != 11)
                        {
                            sheet.Cells[i, j].Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                            sheet.Cells[i, j].Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                        }
                    }
                }
            }
            MotherForm.SetStatusBarMessage("產生校車月票完成", 100);

            if ((stucount % 3) > 0)
            {
                //刪除資料區域
                decimal endcol = 5 + 6 * ((stucount % 3) - 1);
                //endcol = byte.Parse(endcol.ToString());
                sheet.Cells.DeleteRange(row - 1, (byte)endcol, row + 9, 16, ShiftType.Left);

            }
            
            try
            {
                wb.Save(Application.StartupPath + "\\Reports\\學生搭乘校車月票.xls", FileFormatType.Excel2003);
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Reports\\學生搭乘校車月票.xls");
                MotherForm.SetStatusBarMessage("學生搭乘校車月票已匯出完成", 100);
            }
            catch
            {
                System.Windows.Forms.SaveFileDialog sd1 = new System.Windows.Forms.SaveFileDialog();
                sd1.Title = "另存新檔";
                sd1.FileName = "學生搭乘校車月票.xls";
                sd1.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd1.FileName, FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(sd1.FileName);
                        MotherForm.SetStatusBarMessage("學生搭乘校車月票已匯出完成", 100);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void Surveyform_Load(object sender, EventArgs e)
        {
            this.SelectView.Items.Clear();
            List<ClassRecord> classes = new List<ClassRecord>();
            foreach (ClassRecord cr in K12.Data.Class.SelectAll())
            {
                if (cr.GradeYear != null)
                {
                    if (cr.GradeYear > 3)
                        continue;
                    if (cr.GradeYear == 1)
                    {
                        if (int.Parse(cr.DisplayOrder) > 23)
                            continue;
                    }
                    else
                    {
                        if (int.Parse(cr.DisplayOrder) > 24)
                            continue;
                    }
                    if (!classes.Contains(cr))
                        classes.Add(cr);
                }
            }
            classes.Sort(CompareNumber);

            foreach (ClassRecord cr in classes)
                this.SelectView.Items.Add(cr.Name);
            this.btnReport.Enabled = false;
            this.btnRollbook.Enabled = false;
            this.btnTicket.Enabled = false;
            this.btnBus.Enabled = false;
            fillBusYear();
        }

        private void fillBusYear()
        {
            this.cboYear.Items.Clear();
            this.cboYear.DisplayMember = "BusYear";
            //this.SelectView.Items.Clear();

            List<string> busyears = new List<string>();
            //取得所有校車設定清單
            List<BusSetup> bussetup = BusSetupDAO.GetSortByBusStartDateList();

            foreach (BusSetup var in bussetup)
            {
                if (!busyears.Contains(var.BusYear.ToString()))
                    busyears.Add(var.BusYear.ToString());
            }

            for (int i = 0; i < busyears.Count; i++)
                this.cboYear.Items.Add(busyears[i]);
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.btnReport.Enabled = false;
            this.btnRollbook.Enabled = false;
            this.btnTicket.Enabled = false;
            this.btnBus.Enabled = false;
            this.cboRange.Text = "";
            this.cboRange.Items.Clear();
            List<string> busranges = new List<string>();
            //取得所有校車設定清單
            List<BusSetup> bussetup = BusSetupDAO.SelectByBusYear(int.Parse(this.cboYear.Text));

            foreach (BusSetup var in bussetup)
            {
                if (!busranges.Contains(var.BusRangeName))
                    busranges.Add(var.BusRangeName);
            }
            for (int i = 0; i < busranges.Count; i++)
                this.cboRange.Items.Add(busranges[i]);
        }

        private void cboYear_TextChanged(object sender, EventArgs e)
        {
            bool chkflag = false;
            bool btn_visible_all = true;
            if (this.cboYear.Text != "" && this.cboRange.Text != "")
            {
                for (int i = 0; i < SelectView.Items.Count; i++)
                {
                    if (this.SelectView.Items[i].Checked == true)
                    {
                        chkflag = true;
                        break;
                    }
                }
                for (int i = 0; i < SelectView.Items.Count; i++)
                {
                    if (this.SelectView.Items[i].Checked == false)
                        btn_visible_all = false;
                }
                if (chkflag)
                {
                    this.btnReport.Enabled = btn_visible_all;
                    this.btnRollbook.Enabled = btn_visible_all;
                    this.btnTicket.Enabled = true;
                    this.btnBus.Enabled = true;
                }
            }
            else
            {
                for (int i = 0; i < SelectView.Items.Count; i++)
                {
                    if (this.SelectView.Items[i].Checked == false)
                        btn_visible_all = false;
                }
                this.btnReport.Enabled = btn_visible_all;
                this.btnRollbook.Enabled = false;
                this.btnTicket.Enabled = false;
                this.btnBus.Enabled = false;
            }
        }

        private void cboRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool chkflag = false;
            bool btn_visible_all = true;
            if (this.cboYear.Text != "" && this.cboRange.Text != "")
            {
                for (int i = 0; i < SelectView.Items.Count; i++)
                {
                    if (this.SelectView.Items[i].Checked == true)
                    {
                        chkflag = true;
                        break;
                    }
                }
                for (int i = 0; i < SelectView.Items.Count; i++)
                {
                    if (this.SelectView.Items[i].Checked == false)
                        btn_visible_all = false;
                }
                if (chkflag)
                {
                    this.btnReport.Enabled = btn_visible_all;
                    this.btnRollbook.Enabled = btn_visible_all;
                    this.btnTicket.Enabled = true;
                    this.btnBus.Enabled = true;
                }
            }
            else
            {
                this.btnReport.Enabled = chkflag;
                this.btnRollbook.Enabled = false;
                this.btnTicket.Enabled = false;
                this.btnBus.Enabled = false;
            }
        }

        //依年級、班序排序副程式
        static int CompareNumber(ClassRecord a, ClassRecord b)
        {
            if (a.GradeYear == b.GradeYear)
                return int.Parse(a.DisplayOrder).CompareTo(int.Parse(b.DisplayOrder));
            else
                return int.Parse(a.GradeYear.ToString()).CompareTo(int.Parse(b.GradeYear.ToString()));
        }

        //依年級、班序、學號排序副程式
        static int CompareClassNumber(StudentRecord a, StudentRecord b)
        {
            if (a.Class.GradeYear == b.Class.GradeYear)
            {
                if (int.Parse(a.Class.DisplayOrder) == int.Parse(b.Class.DisplayOrder))
                    return a.StudentNumber.CompareTo(b.StudentNumber);
                else
                    return int.Parse(a.Class.DisplayOrder).CompareTo(int.Parse(b.Class.DisplayOrder));
            }
            else
                return int.Parse(a.Class.GradeYear.ToString()).CompareTo(int.Parse(b.Class.GradeYear.ToString()));
        }

        //依站代碼、學號排序副程式
        static int CompareSNumber(StudentByBus a, StudentByBus b)
        {
            return a.StudentID.CompareTo(b.StudentID);
        }

        //依設定時間先後排序副程式
        static int CompareBusSetup(BusSetup a, BusSetup b)
        {
            return a.UID.CompareTo(b.UID);
        }
    }
}
