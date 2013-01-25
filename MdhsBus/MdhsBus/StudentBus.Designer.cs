namespace MdhsBus
{
    partial class StudentBus
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
            this.StudentBusView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader8 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader9 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader10 = new System.Windows.Forms.ColumnHeader();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.exitbtn = new DevComponents.DotNetBar.ButtonX();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cboRange = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.studentdataGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.savebtn = new DevComponents.DotNetBar.ButtonX();
            this.label4 = new System.Windows.Forms.Label();
            this.Paymentbtn = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.studentdataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // StudentBusView
            // 
            this.StudentBusView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.StudentBusView.FullRowSelect = true;
            this.StudentBusView.HoverSelection = true;
            this.StudentBusView.LabelEdit = true;
            this.StudentBusView.Location = new System.Drawing.Point(12, 38);
            this.StudentBusView.Name = "StudentBusView";
            this.StudentBusView.Size = new System.Drawing.Size(641, 45);
            this.StudentBusView.TabIndex = 0;
            this.StudentBusView.UseCompatibleStateImageBehavior = false;
            this.StudentBusView.View = System.Windows.Forms.View.Details;
            this.StudentBusView.Visible = false;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "SUID";
            this.columnHeader1.Width = 0;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "班級";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "學號";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "代碼";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "姓名";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "電話";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "是否繳費";
            this.columnHeader7.Width = 80;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "繳費日期";
            this.columnHeader8.Width = 100;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "天數";
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "備註";
            this.columnHeader10.Width = 120;
            // 
            // cboClass
            // 
            this.cboClass.FormattingEnabled = true;
            this.cboClass.Location = new System.Drawing.Point(557, 7);
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(96, 25);
            this.cboClass.TabIndex = 1;
            this.cboClass.SelectedIndexChanged += new System.EventHandler(this.cboClass_SelectedIndexChanged);
            // 
            // exitbtn
            // 
            this.exitbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.exitbtn.BackColor = System.Drawing.Color.Transparent;
            this.exitbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.exitbtn.Location = new System.Drawing.Point(692, 280);
            this.exitbtn.Name = "exitbtn";
            this.exitbtn.Size = new System.Drawing.Size(68, 23);
            this.exitbtn.TabIndex = 38;
            this.exitbtn.Text = "離開";
            this.exitbtn.Click += new System.EventHandler(this.exitbtn_Click);
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(106, 7);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(78, 25);
            this.cboYear.TabIndex = 40;
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 39;
            this.label1.Text = "校車搭車年度：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(201, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 39;
            this.label2.Text = "期間名稱：";
            // 
            // cboRange
            // 
            this.cboRange.FormattingEnabled = true;
            this.cboRange.Location = new System.Drawing.Point(270, 7);
            this.cboRange.Name = "cboRange";
            this.cboRange.Size = new System.Drawing.Size(204, 25);
            this.cboRange.TabIndex = 40;
            this.cboRange.SelectedIndexChanged += new System.EventHandler(this.cboRange_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(515, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 17);
            this.label3.TabIndex = 39;
            this.label3.Text = "班級：";
            // 
            // studentdataGridView
            // 
            this.studentdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.studentdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column12});
            this.studentdataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.studentdataGridView.Location = new System.Drawing.Point(12, 38);
            this.studentdataGridView.Name = "studentdataGridView";
            this.studentdataGridView.RowTemplate.Height = 24;
            this.studentdataGridView.Size = new System.Drawing.Size(841, 236);
            this.studentdataGridView.TabIndex = 41;
            this.studentdataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.studentdataGridView_DataError);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "班級";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 80;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "學號";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "代碼";
            this.Column3.Items.AddRange(new object[] {
            "1001",
            "1002",
            "5001"});
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column3.Width = 80;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "姓名";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 80;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "電話";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "是否繳費";
            this.Column6.Name = "Column6";
            this.Column6.Width = 50;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "繳費日期";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "天數";
            this.Column8.Name = "Column8";
            this.Column8.Width = 50;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "備註";
            this.Column9.Name = "Column9";
            // 
            // Column10
            // 
            this.Column10.HeaderText = "StudentID";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Visible = false;
            this.Column10.Width = 5;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "車費";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.Width = 50;
            // 
            // Column12
            // 
            this.Column12.HeaderText = "ClassID";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            this.Column12.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column12.Visible = false;
            // 
            // savebtn
            // 
            this.savebtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.savebtn.BackColor = System.Drawing.Color.Transparent;
            this.savebtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.savebtn.Location = new System.Drawing.Point(142, 280);
            this.savebtn.Name = "savebtn";
            this.savebtn.Size = new System.Drawing.Size(68, 23);
            this.savebtn.TabIndex = 38;
            this.savebtn.Text = "儲存";
            this.savebtn.Click += new System.EventHandler(this.savebtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(689, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 17);
            this.label4.TabIndex = 42;
            // 
            // Paymentbtn
            // 
            this.Paymentbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.Paymentbtn.BackColor = System.Drawing.Color.Transparent;
            this.Paymentbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.Paymentbtn.Enabled = false;
            this.Paymentbtn.Location = new System.Drawing.Point(406, 280);
            this.Paymentbtn.Name = "Paymentbtn";
            this.Paymentbtn.Size = new System.Drawing.Size(83, 23);
            this.Paymentbtn.TabIndex = 43;
            this.Paymentbtn.Text = "產生繳費單";
            this.Paymentbtn.Click += new System.EventHandler(this.Paymentbtn_Click);
            // 
            // StudentBus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 315);
            this.Controls.Add(this.Paymentbtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.studentdataGridView);
            this.Controls.Add(this.cboClass);
            this.Controls.Add(this.cboRange);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.savebtn);
            this.Controls.Add(this.exitbtn);
            this.Controls.Add(this.StudentBusView);
            this.Name = "StudentBus";
            this.Text = "設定學生搭乘校車資訊";
            this.Load += new System.EventHandler(this.StudentBus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.studentdataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView StudentBusView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ComboBox cboClass;
        private DevComponents.DotNetBar.ButtonX exitbtn;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboRange;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView studentdataGridView;
        private DevComponents.DotNetBar.ButtonX savebtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewComboBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private DevComponents.DotNetBar.ButtonX Paymentbtn;
    }
}