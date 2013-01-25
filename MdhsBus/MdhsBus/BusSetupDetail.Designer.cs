namespace MdhsBus
{
    partial class BusSetupDetail
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
            this.nrBusDay = new System.Windows.Forms.NumericUpDown();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBusRange = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTime_start = new System.Windows.Forms.DateTimePicker();
            this.dateTime_end = new System.Windows.Forms.DateTimePicker();
            this.dateTime_payend = new System.Windows.Forms.DateTimePicker();
            this.cboBusTime = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.nrYear = new System.Windows.Forms.NumericUpDown();
            this.chkNew = new System.Windows.Forms.CheckBox();
            this.cboBusPayment = new System.Windows.Forms.ComboBox();
            this.nrSchoolYear = new System.Windows.Forms.NumericUpDown();
            this.nrSemester = new System.Windows.Forms.NumericUpDown();
            this.cboPayment = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX4 = new DevComponents.DotNetBar.LabelX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.nrBusDay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nrYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nrSchoolYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nrSemester)).BeginInit();
            this.SuspendLayout();
            // 
            // nrBusDay
            // 
            this.nrBusDay.Location = new System.Drawing.Point(95, 253);
            this.nrBusDay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nrBusDay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nrBusDay.Name = "nrBusDay";
            this.nrBusDay.Size = new System.Drawing.Size(68, 25);
            this.nrBusDay.TabIndex = 30;
            this.nrBusDay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(247, 313);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 29;
            this.btnExit.Text = "離開";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(166, 313);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 28;
            this.btnSave.Text = "儲存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(12, 255);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 17);
            this.label4.TabIndex = 26;
            this.label4.Text = "天數";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(12, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 17);
            this.label3.TabIndex = 27;
            this.label3.Text = "搭車截止日期";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(12, 222);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 17);
            this.label2.TabIndex = 25;
            this.label2.Text = "繳費截止日期";
            // 
            // txtBusRange
            // 
            this.txtBusRange.Location = new System.Drawing.Point(95, 69);
            this.txtBusRange.MaxLength = 30;
            this.txtBusRange.Name = "txtBusRange";
            this.txtBusRange.Size = new System.Drawing.Size(227, 25);
            this.txtBusRange.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(12, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 17);
            this.label5.TabIndex = 17;
            this.label5.Text = "搭車起始日期";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(12, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 17);
            this.label6.TabIndex = 18;
            this.label6.Text = "期間名稱";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "校車時段";
            // 
            // dateTime_start
            // 
            this.dateTime_start.Location = new System.Drawing.Point(95, 160);
            this.dateTime_start.Name = "dateTime_start";
            this.dateTime_start.Size = new System.Drawing.Size(134, 25);
            this.dateTime_start.TabIndex = 31;
            // 
            // dateTime_end
            // 
            this.dateTime_end.Location = new System.Drawing.Point(95, 191);
            this.dateTime_end.Name = "dateTime_end";
            this.dateTime_end.Size = new System.Drawing.Size(134, 25);
            this.dateTime_end.TabIndex = 31;
            // 
            // dateTime_payend
            // 
            this.dateTime_payend.Location = new System.Drawing.Point(95, 222);
            this.dateTime_payend.Name = "dateTime_payend";
            this.dateTime_payend.Size = new System.Drawing.Size(134, 25);
            this.dateTime_payend.TabIndex = 31;
            // 
            // cboBusTime
            // 
            this.cboBusTime.FormattingEnabled = true;
            this.cboBusTime.Location = new System.Drawing.Point(95, 38);
            this.cboBusTime.Name = "cboBusTime";
            this.cboBusTime.Size = new System.Drawing.Size(121, 25);
            this.cboBusTime.TabIndex = 32;
            this.cboBusTime.SelectedIndexChanged += new System.EventHandler(this.cboBusTime_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(12, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 17);
            this.label7.TabIndex = 26;
            this.label7.Text = "校車年度";
            // 
            // nrYear
            // 
            this.nrYear.Location = new System.Drawing.Point(95, 7);
            this.nrYear.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nrYear.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nrYear.Name = "nrYear";
            this.nrYear.Size = new System.Drawing.Size(68, 25);
            this.nrYear.TabIndex = 30;
            this.nrYear.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkNew
            // 
            this.chkNew.AutoSize = true;
            this.chkNew.BackColor = System.Drawing.Color.Transparent;
            this.chkNew.Location = new System.Drawing.Point(15, 284);
            this.chkNew.Name = "chkNew";
            this.chkNew.Size = new System.Drawing.Size(118, 21);
            this.chkNew.TabIndex = 33;
            this.chkNew.Text = "是否為新生搭乘";
            this.chkNew.UseVisualStyleBackColor = false;
            this.chkNew.Visible = false;
            // 
            // cboBusPayment
            // 
            this.cboBusPayment.DisplayMember = "Name";
            this.cboBusPayment.FormattingEnabled = true;
            this.cboBusPayment.Location = new System.Drawing.Point(95, 100);
            this.cboBusPayment.Name = "cboBusPayment";
            this.cboBusPayment.Size = new System.Drawing.Size(134, 25);
            this.cboBusPayment.TabIndex = 32;
            this.cboBusPayment.Visible = false;
            // 
            // nrSchoolYear
            // 
            this.nrSchoolYear.Location = new System.Drawing.Point(95, 100);
            this.nrSchoolYear.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nrSchoolYear.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nrSchoolYear.Name = "nrSchoolYear";
            this.nrSchoolYear.Size = new System.Drawing.Size(57, 25);
            this.nrSchoolYear.TabIndex = 30;
            this.nrSchoolYear.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.nrSchoolYear.ValueChanged += new System.EventHandler(this.nrSchoolYear_ValueChanged);
            // 
            // nrSemester
            // 
            this.nrSemester.Location = new System.Drawing.Point(235, 101);
            this.nrSemester.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nrSemester.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nrSemester.Name = "nrSemester";
            this.nrSemester.Size = new System.Drawing.Size(42, 25);
            this.nrSemester.TabIndex = 30;
            this.nrSemester.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nrSemester.ValueChanged += new System.EventHandler(this.nrSemester_ValueChanged);
            // 
            // cboPayment
            // 
            this.cboPayment.DisplayMember = "Name";
            this.cboPayment.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboPayment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPayment.FormattingEnabled = true;
            this.cboPayment.ItemHeight = 19;
            this.cboPayment.Location = new System.Drawing.Point(95, 131);
            this.cboPayment.Name = "cboPayment";
            this.cboPayment.Size = new System.Drawing.Size(197, 25);
            this.cboPayment.TabIndex = 57;
            // 
            // labelX4
            // 
            this.labelX4.BackColor = System.Drawing.Color.Transparent;
            this.labelX4.ForeColor = System.Drawing.Color.Red;
            this.labelX4.Location = new System.Drawing.Point(12, 131);
            this.labelX4.Name = "labelX4";
            this.labelX4.Size = new System.Drawing.Size(93, 23);
            this.labelX4.TabIndex = 56;
            this.labelX4.Text = "收費銀行設定";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.ForeColor = System.Drawing.Color.Red;
            this.labelX1.Location = new System.Drawing.Point(12, 100);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 56;
            this.labelX1.Text = "收費學年度";
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            this.labelX2.ForeColor = System.Drawing.Color.Red;
            this.labelX2.Location = new System.Drawing.Point(168, 100);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(61, 23);
            this.labelX2.TabIndex = 56;
            this.labelX2.Text = "收費學期";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Location = new System.Drawing.Point(14, 316);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(73, 17);
            this.linkLabel1.TabIndex = 61;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "繳費單樣版";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // BusSetupDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 339);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.cboPayment);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.labelX4);
            this.Controls.Add(this.chkNew);
            this.Controls.Add(this.cboBusTime);
            this.Controls.Add(this.dateTime_payend);
            this.Controls.Add(this.dateTime_end);
            this.Controls.Add(this.dateTime_start);
            this.Controls.Add(this.nrSemester);
            this.Controls.Add(this.nrSchoolYear);
            this.Controls.Add(this.nrYear);
            this.Controls.Add(this.nrBusDay);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBusRange);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboBusPayment);
            this.Name = "BusSetupDetail";
            this.Text = "校車時間設定明細";
            this.Load += new System.EventHandler(this.BusSetupDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nrBusDay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nrYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nrSchoolYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nrSemester)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nrBusDay;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBusRange;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTime_start;
        private System.Windows.Forms.DateTimePicker dateTime_end;
        private System.Windows.Forms.DateTimePicker dateTime_payend;
        private System.Windows.Forms.ComboBox cboBusTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nrYear;
        private System.Windows.Forms.CheckBox chkNew;
        private System.Windows.Forms.ComboBox cboBusPayment;
        private System.Windows.Forms.NumericUpDown nrSchoolYear;
        private System.Windows.Forms.NumericUpDown nrSemester;
        private DevComponents.DotNetBar.Controls.ComboBoxEx cboPayment;
        private DevComponents.DotNetBar.LabelX labelX4;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}