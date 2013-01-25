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
using Aspose.Cells;
using FISCA.Presentation;
using FISCA.UDT;
using K12.Data;
using AccountsReceivable.API;

namespace MdhsBus
{
    public partial class StudentByBusDetail : BaseForm
    {
        public StudentByBusDetail()
        {
            InitializeComponent();
        }

        private void StudentByBusDetail_Load(object sender, EventArgs e)
        {
            this.btnExport.Enabled = false;
            fillBusYear();
        }

        private void fillBusYear()
        {
            this.cboYear.Items.Clear();
            this.cboYear.DisplayMember = "BusYear";

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

            checkValue();
        }

        private void cboYear_TextChanged(object sender, EventArgs e)
        {
            checkValue();
        }

        private void cboRange_TextChanged(object sender, EventArgs e)
        {
            checkValue();
        }

        private void cboRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkValue();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Dictionary<string, NewStudentRecord> NewStudentByID = new Dictionary<string, NewStudentRecord>();
            Dictionary<string, StudentRecord> StudentByID = new Dictionary<string, StudentRecord>();
            int nowSet = 0;
            if (this.cboStudent.Text == "新生")
            {
                AccessHelper udtHelper = new AccessHelper();
                List<NewStudentRecord> NewStudentRecords = udtHelper.Select<NewStudentRecord>("學年度='" + this.cboYear.Text + "'");
                foreach (NewStudentRecord var in NewStudentRecords)
                {
                    MotherForm.SetStatusBarMessage("正在存取學生資料", nowSet++ * 20 / NewStudentRecords.Count);
                    if (var.SchoolYear != this.cboYear.Text)
                        continue;
                    if (!NewStudentByID.ContainsKey(var.UID))
                        NewStudentByID.Add(var.UID, var);
                }
            }
            else
            {
                List<StudentRecord> StudentRecords = K12.Data.Student.SelectAll();
                foreach (StudentRecord var in StudentRecords)
                {
                    MotherForm.SetStatusBarMessage("正在存取學生資料", nowSet++ * 20 / StudentRecords.Count);
                    //if (var.Status != StudentRecord.StudentStatus.一般)
                    //    continue;
                    if (!StudentByID.ContainsKey(var.ID))
                        StudentByID.Add(var.ID, var);
                }
            }

            List<BusStop> BusStops = BusStopDAO.SelectAll();
            Dictionary<string, BusStop> BusStopRecord = new Dictionary<string, BusStop>();
            foreach (BusStop var in BusStops)
            {
                if (!BusStopRecord.ContainsKey(var.BusStopID))
                    BusStopRecord.Add(var.BusStopID, var);
            }

            List<StudentByBus> StudentByBuses = StudentByBusDAO.SelectByBusYearAndTimeName(int.Parse(this.cboYear.Text), this.cboRange.Text);
            //foreach (StudentByBus var in StudentByBuses)
            //{
            //    StudentByBusDAO.Delete(var);
            //}

            Workbook wb = new Workbook();
            Style defaultStyle = wb.DefaultStyle;
            defaultStyle.Font.Name = "標楷體";
            defaultStyle.Font.Size = 12;
            wb.DefaultStyle = defaultStyle;
            int row = 0;
            nowSet = 0;
            wb.Worksheets[0].Name = "學生搭乘校車資料";

            //校車路線站名資料
            wb.Worksheets[0].Cells[row, 0].PutValue("搭車系統編號");
            wb.Worksheets[0].Cells[row, 1].PutValue("搭車年度");
            wb.Worksheets[0].Cells[row, 2].PutValue("校車時段");
            wb.Worksheets[0].Cells[row, 3].PutValue("期間名稱");
            if (this.cboStudent.Text == "新生")
                wb.Worksheets[0].Cells[row, 4].PutValue("科別");
            else
                wb.Worksheets[0].Cells[row, 4].PutValue("班級");
            wb.Worksheets[0].Cells[row, 5].PutValue("學生系統編號");
            wb.Worksheets[0].Cells[row, 6].PutValue("學生編號");
            wb.Worksheets[0].Cells[row, 7].PutValue("學生姓名");
            wb.Worksheets[0].Cells[row, 8].PutValue("代碼");    //電腦代號
            wb.Worksheets[0].Cells[row, 9].PutValue("站名");
            wb.Worksheets[0].Cells[row, 10].PutValue("天數");
            wb.Worksheets[0].Cells[row, 11].PutValue("車費");
            wb.Worksheets[0].Cells[row, 12].PutValue("是否繳費");
            wb.Worksheets[0].Cells[row, 13].PutValue("繳費日期");
            wb.Worksheets[0].Cells[row, 14].PutValue("備註");
            row++;
            //buses.Sort(CompareBusNumber);


            foreach (StudentByBus var in StudentByBuses)
            {
                MotherForm.SetStatusBarMessage("正在產生報表", 20 + nowSet++ * 80 / StudentByBuses.Count);
                if (this.cboStudent.Text == "新生")
                {
                    if (var.ClassName.IndexOf("科") < 0)
                        continue;
                }
                else
                    if (var.ClassName.IndexOf("科") >= 0)
                        continue;


                wb.Worksheets[0].Cells[row, 0].PutValue(var.UID);
                wb.Worksheets[0].Cells[row, 1].PutValue(var.SchoolYear);
                wb.Worksheets[0].Cells[row, 2].PutValue(var.BusTimeName);
                wb.Worksheets[0].Cells[row, 3].PutValue(var.BusRangeName);
                wb.Worksheets[0].Cells[row, 4].PutValue(var.ClassName);
                if (this.cboStudent.Text == "新生")
                {
                    if (NewStudentByID.ContainsKey(var.StudentID))
                    {
                        wb.Worksheets[0].Cells[row, 5].PutValue(NewStudentByID[var.StudentID].UID);
                        wb.Worksheets[0].Cells[row, 6].PutValue(NewStudentByID[var.StudentID].Number);
                        wb.Worksheets[0].Cells[row, 7].PutValue(NewStudentByID[var.StudentID].Name);
                    }
                    else
                        wb.Worksheets[0].Cells[row, 6].PutValue(var.StudentID);
                }
                else
                {
                    wb.Worksheets[0].Cells[row, 5].PutValue(var.StudentID);
                    if (StudentByID.ContainsKey(var.StudentID))
                    {
                        wb.Worksheets[0].Cells[row, 6].PutValue(StudentByID[var.StudentID].StudentNumber);
                        wb.Worksheets[0].Cells[row, 7].PutValue(StudentByID[var.StudentID].Name);
                    }
                }
                wb.Worksheets[0].Cells[row, 8].PutValue(var.BusStopID);
                if (BusStopRecord.ContainsKey(var.BusStopID))
                    wb.Worksheets[0].Cells[row, 9].PutValue(BusStopRecord[var.BusStopID].BusStopName);
                wb.Worksheets[0].Cells[row, 10].PutValue(var.DateCount);
                wb.Worksheets[0].Cells[row, 11].PutValue(var.BusMoney);
                if (var.PayStatus)
                {
                    wb.Worksheets[0].Cells[row, 12].PutValue("是");
                    wb.Worksheets[0].Cells[row, 13].PutValue(var.PayDate.ToShortDateString());
                }
                else
                    wb.Worksheets[0].Cells[row, 12].PutValue("否");
                wb.Worksheets[0].Cells[row, 14].PutValue(var.comment);
                row++;
            }
            wb.Worksheets[0].AutoFitColumns();

            try
            {
                wb.Save(Application.StartupPath + "\\Reports\\匯出學生搭乘校車資料.xls", FileFormatType.Excel2003);
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Reports\\匯出學生搭乘校車資料.xls");
                MotherForm.SetStatusBarMessage("學生搭乘校車資料已匯出完成", 100);
            }
            catch
            {
                System.Windows.Forms.SaveFileDialog sd1 = new System.Windows.Forms.SaveFileDialog();
                sd1.Title = "另存新檔";
                sd1.FileName = "匯出學生搭乘校車資料.xls";
                sd1.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        wb.Save(sd1.FileName, FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(sd1.FileName);
                        MotherForm.SetStatusBarMessage("學生搭乘校車資料已匯出完成", 100);
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void checkValue()
        {
            if (this.cboYear.Text != "" && this.cboRange.Text != "" && this.cboStudent.Text != "")
                this.btnExport.Enabled = true;
            else
                this.btnExport.Enabled = false;
        }

        private void cboStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkValue();
        }

        private void cboStudent_TextChanged(object sender, EventArgs e)
        {
            if (this.cboStudent.Text == "")
            {
                checkValue();
            }
            else if (this.cboStudent.Text != this.cboStudent.Items[0].ToString() && this.cboStudent.Text != this.cboStudent.Items[1].ToString())
            {
                this.btnExport.Enabled = false;
                MessageBox.Show("學生類別有誤，請重新輸入！");
                return;
            }
        }
    }
}
