﻿namespace MdhsBus
{
    partial class StudnetPaymentSheets
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CancelExist = new System.Windows.Forms.CheckBox();
            this.textBusStopName = new System.Windows.Forms.TextBox();
            this.textBusStop = new System.Windows.Forms.TextBox();
            this.dtDueDate = new DevComponents.Editors.DateTimeAdv.DateTimeInput();
            this.cboBusStopID = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX7 = new DevComponents.DotNetBar.LabelX();
            this.cboBusRange = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX6 = new DevComponents.DotNetBar.LabelX();
            this.cboPayment = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.btnReport = new DevComponents.DotNetBar.ButtonX();
            this.cboSemester = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.comboItem1 = new DevComponents.Editors.ComboItem();
            this.comboItem2 = new DevComponents.Editors.ComboItem();
            this.intSchoolYear = new DevComponents.Editors.IntegerInput();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.labelX5 = new DevComponents.DotNetBar.LabelX();
            this.labelX8 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxEx1 = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX9 = new DevComponents.DotNetBar.LabelX();
            this.cboDept = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.dtDueDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSchoolYear)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelExist
            // 
            this.CancelExist.AutoSize = true;
            this.CancelExist.BackColor = System.Drawing.Color.Transparent;
            this.CancelExist.Location = new System.Drawing.Point(19, 197);
            this.CancelExist.Name = "CancelExist";
            this.CancelExist.Size = new System.Drawing.Size(118, 21);
            this.CancelExist.TabIndex = 84;
            this.CancelExist.Text = "取消原有繳費單";
            this.CancelExist.UseVisualStyleBackColor = false;
            // 
            // textBusStopName
            // 
            this.textBusStopName.Location = new System.Drawing.Point(133, 106);
            this.textBusStopName.MaxLength = 4;
            this.textBusStopName.Name = "textBusStopName";
            this.textBusStopName.ReadOnly = true;
            this.textBusStopName.Size = new System.Drawing.Size(141, 25);
            this.textBusStopName.TabIndex = 82;
            // 
            // textBusStop
            // 
            this.textBusStop.Location = new System.Drawing.Point(77, 106);
            this.textBusStop.MaxLength = 4;
            this.textBusStop.Name = "textBusStop";
            this.textBusStop.ReadOnly = true;
            this.textBusStop.Size = new System.Drawing.Size(55, 25);
            this.textBusStop.TabIndex = 83;
            // 
            // dtDueDate
            // 
            this.dtDueDate.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.dtDueDate.BackgroundStyle.Class = "DateTimeInputBackground";
            this.dtDueDate.ButtonDropDown.Shortcut = DevComponents.DotNetBar.eShortcut.AltDown;
            this.dtDueDate.ButtonDropDown.Visible = true;
            this.dtDueDate.Enabled = false;
            this.dtDueDate.Format = DevComponents.Editors.eDateTimePickerFormat.Long;
            this.dtDueDate.Location = new System.Drawing.Point(106, 135);
            // 
            // 
            // 
            this.dtDueDate.MonthCalendar.AnnuallyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dtDueDate.MonthCalendar.BackgroundStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dtDueDate.MonthCalendar.ClearButtonVisible = true;
            // 
            // 
            // 
            this.dtDueDate.MonthCalendar.CommandsBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.dtDueDate.MonthCalendar.CommandsBackgroundStyle.BackColorGradientAngle = 90;
            this.dtDueDate.MonthCalendar.CommandsBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.dtDueDate.MonthCalendar.CommandsBackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.dtDueDate.MonthCalendar.CommandsBackgroundStyle.BorderTopColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.dtDueDate.MonthCalendar.CommandsBackgroundStyle.BorderTopWidth = 1;
            this.dtDueDate.MonthCalendar.DisplayMonth = new System.DateTime(2009, 7, 1, 0, 0, 0, 0);
            this.dtDueDate.MonthCalendar.MarkedDates = new System.DateTime[0];
            this.dtDueDate.MonthCalendar.MonthlyMarkedDates = new System.DateTime[0];
            // 
            // 
            // 
            this.dtDueDate.MonthCalendar.NavigationBackgroundStyle.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.dtDueDate.MonthCalendar.NavigationBackgroundStyle.BackColorGradientAngle = 90;
            this.dtDueDate.MonthCalendar.NavigationBackgroundStyle.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.dtDueDate.MonthCalendar.TodayButtonVisible = true;
            this.dtDueDate.MonthCalendar.WeeklyMarkedDays = new System.DayOfWeek[0];
            this.dtDueDate.Name = "dtDueDate";
            this.dtDueDate.Size = new System.Drawing.Size(168, 25);
            this.dtDueDate.TabIndex = 78;
            // 
            // cboBusStopID
            // 
            this.cboBusStopID.DisplayMember = "Name";
            this.cboBusStopID.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboBusStopID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBusStopID.FormattingEnabled = true;
            this.cboBusStopID.ItemHeight = 19;
            this.cboBusStopID.Location = new System.Drawing.Point(156, 106);
            this.cboBusStopID.Name = "cboBusStopID";
            this.cboBusStopID.Size = new System.Drawing.Size(118, 25);
            this.cboBusStopID.TabIndex = 76;
            this.cboBusStopID.Visible = false;
            // 
            // labelX7
            // 
            this.labelX7.BackColor = System.Drawing.Color.Transparent;
            this.labelX7.Location = new System.Drawing.Point(15, 106);
            this.labelX7.Name = "labelX7";
            this.labelX7.Size = new System.Drawing.Size(75, 23);
            this.labelX7.TabIndex = 74;
            this.labelX7.Text = "站名代碼：";
            // 
            // cboBusRange
            // 
            this.cboBusRange.DisplayMember = "Name";
            this.cboBusRange.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboBusRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBusRange.FormattingEnabled = true;
            this.cboBusRange.ItemHeight = 19;
            this.cboBusRange.Location = new System.Drawing.Point(77, 74);
            this.cboBusRange.Name = "cboBusRange";
            this.cboBusRange.Size = new System.Drawing.Size(197, 25);
            this.cboBusRange.TabIndex = 77;
            this.cboBusRange.SelectedIndexChanged += new System.EventHandler(this.cboBusRange_SelectedIndexChanged);
            // 
            // labelX6
            // 
            this.labelX6.BackColor = System.Drawing.Color.Transparent;
            this.labelX6.Location = new System.Drawing.Point(15, 76);
            this.labelX6.Name = "labelX6";
            this.labelX6.Size = new System.Drawing.Size(75, 23);
            this.labelX6.TabIndex = 73;
            this.labelX6.Text = "校車期間：";
            // 
            // cboPayment
            // 
            this.cboPayment.DisplayMember = "Name";
            this.cboPayment.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboPayment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPayment.FormattingEnabled = true;
            this.cboPayment.ItemHeight = 19;
            this.cboPayment.Location = new System.Drawing.Point(77, 43);
            this.cboPayment.Name = "cboPayment";
            this.cboPayment.Size = new System.Drawing.Size(197, 25);
            this.cboPayment.TabIndex = 75;
            this.cboPayment.SelectedIndexChanged += new System.EventHandler(this.cboPayment_SelectedIndexChanged);
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.Location = new System.Drawing.Point(15, 45);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(75, 23);
            this.labelX4.TabIndex = 72;
            this.labelX4.Text = "收費項目：";
            // 
            // btnReport
            // 
            this.btnReport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnReport.BackColor = System.Drawing.Color.Transparent;
            this.btnReport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnReport.Location = new System.Drawing.Point(199, 197);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(75, 23);
            this.btnReport.TabIndex = 71;
            this.btnReport.Text = "產生報表";
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // cboSemester
            // 
            this.cboSemester.DisplayMember = "Text";
            this.cboSemester.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSemester.FormattingEnabled = true;
            this.cboSemester.ItemHeight = 19;
            this.cboSemester.Items.AddRange(new object[] {
            this.comboItem1,
            this.comboItem2});
            this.cboSemester.Location = new System.Drawing.Point(201, 10);
            this.cboSemester.Name = "cboSemester";
            this.cboSemester.Size = new System.Drawing.Size(73, 25);
            this.cboSemester.TabIndex = 68;
            this.cboSemester.SelectedIndexChanged += new System.EventHandler(this.cboSemester_SelectedIndexChanged);
            // 
            // comboItem1
            // 
            this.comboItem1.Text = "上學期";
            // 
            // comboItem2
            // 
            this.comboItem2.Text = "下學期";
            // 
            // intSchoolYear
            // 
            this.intSchoolYear.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.intSchoolYear.BackgroundStyle.Class = "DateTimeInputBackground";
            this.intSchoolYear.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2;
            this.intSchoolYear.Location = new System.Drawing.Point(77, 12);
            this.intSchoolYear.MaxValue = 120;
            this.intSchoolYear.MinValue = 98;
            this.intSchoolYear.Name = "intSchoolYear";
            this.intSchoolYear.ShowUpDown = true;
            this.intSchoolYear.Size = new System.Drawing.Size(73, 25);
            this.intSchoolYear.TabIndex = 67;
            this.intSchoolYear.Value = 98;
            this.intSchoolYear.ValueChanged += new System.EventHandler(this.intSchoolYear_ValueChanged);
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(19, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(71, 23);
            this.labelX1.TabIndex = 69;
            this.labelX1.Text = "學年度：";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.Location = new System.Drawing.Point(156, 14);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(55, 23);
            this.labelX2.TabIndex = 70;
            this.labelX2.Text = "學期：";
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            this.labelX3.ForeColor = System.Drawing.Color.Red;
            this.labelX3.Location = new System.Drawing.Point(143, 195);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(129, 23);
            this.labelX3.TabIndex = 80;
            // 
            // labelX5
            // 
            this.labelX5.BackColor = System.Drawing.Color.Transparent;
            this.labelX5.Location = new System.Drawing.Point(15, 135);
            this.labelX5.Name = "labelX5";
            this.labelX5.Size = new System.Drawing.Size(103, 23);
            this.labelX5.TabIndex = 81;
            this.labelX5.Text = "繳款截止日期：";
            // 
            // labelX8
            // 
            this.labelX8.BackColor = System.Drawing.Color.Transparent;
            this.labelX8.Location = new System.Drawing.Point(13, 166);
            this.labelX8.Name = "labelX8";
            this.labelX8.Size = new System.Drawing.Size(75, 23);
            this.labelX8.TabIndex = 72;
            this.labelX8.Text = "收費項目：";
            // 
            // comboBoxEx1
            // 
            this.comboBoxEx1.DisplayMember = "Name";
            this.comboBoxEx1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxEx1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxEx1.FormattingEnabled = true;
            this.comboBoxEx1.ItemHeight = 19;
            this.comboBoxEx1.Location = new System.Drawing.Point(75, 164);
            this.comboBoxEx1.Name = "comboBoxEx1";
            this.comboBoxEx1.Size = new System.Drawing.Size(197, 25);
            this.comboBoxEx1.TabIndex = 75;
            this.comboBoxEx1.SelectedIndexChanged += new System.EventHandler(this.cboPayment_SelectedIndexChanged);
            // 
            // labelX9
            // 
            this.labelX9.BackColor = System.Drawing.Color.Transparent;
            this.labelX9.Location = new System.Drawing.Point(15, 166);
            this.labelX9.Name = "labelX9";
            this.labelX9.Size = new System.Drawing.Size(75, 23);
            this.labelX9.TabIndex = 72;
            this.labelX9.Text = "科　別：";
            // 
            // cboDept
            // 
            this.cboDept.DisplayMember = "Name";
            this.cboDept.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDept.FormattingEnabled = true;
            this.cboDept.ItemHeight = 19;
            this.cboDept.Location = new System.Drawing.Point(77, 164);
            this.cboDept.Name = "cboDept";
            this.cboDept.Size = new System.Drawing.Size(197, 25);
            this.cboDept.TabIndex = 75;
            this.cboDept.SelectedIndexChanged += new System.EventHandler(this.cboPayment_SelectedIndexChanged);
            // 
            // StudnetPaymentSheets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 229);
            this.Controls.Add(this.CancelExist);
            this.Controls.Add(this.textBusStopName);
            this.Controls.Add(this.textBusStop);
            this.Controls.Add(this.dtDueDate);
            this.Controls.Add(this.cboBusStopID);
            this.Controls.Add(this.labelX7);
            this.Controls.Add(this.cboBusRange);
            this.Controls.Add(this.labelX6);
            this.Controls.Add(this.cboDept);
            this.Controls.Add(this.labelX9);
            this.Controls.Add(this.comboBoxEx1);
            this.Controls.Add(this.labelX8);
            this.Controls.Add(this.cboPayment);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.cboSemester);
            this.Controls.Add(this.intSchoolYear);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.labelX5);
            this.Name = "StudnetPaymentSheets";
            this.Text = "繳費單列印(批次)";
            this.Load += new System.EventHandler(this.StudnetPaymentSheets_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtDueDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intSchoolYear)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CancelExist;
        private System.Windows.Forms.TextBox textBusStopName;
        private System.Windows.Forms.TextBox textBusStop;
        private DevComponents.Editors.DateTimeAdv.DateTimeInput dtDueDate;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboBusStopID;
        private DevComponents.DotNetBar.LabelX labelX7;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboBusRange;
        private DevComponents.DotNetBar.LabelX labelX6;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboPayment;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.ButtonX btnReport;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboSemester;
        private DevComponents.Editors.ComboItem comboItem1;
        private DevComponents.Editors.ComboItem comboItem2;
        private DevComponents.Editors.IntegerInput intSchoolYear;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX5;
        private DevComponents.DotNetBar.LabelX labelX8;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxEx1;
        private DevComponents.DotNetBar.LabelX labelX9;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboDept;

    }
}