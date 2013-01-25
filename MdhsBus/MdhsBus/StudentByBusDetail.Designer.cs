namespace MdhsBus
{
    partial class StudentByBusDetail
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
            this.cboRange = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboStudent = new System.Windows.Forms.ComboBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboRange
            // 
            this.cboRange.FormattingEnabled = true;
            this.cboRange.Location = new System.Drawing.Point(247, 50);
            this.cboRange.Name = "cboRange";
            this.cboRange.Size = new System.Drawing.Size(204, 25);
            this.cboRange.TabIndex = 47;
            this.cboRange.SelectedIndexChanged += new System.EventHandler(this.cboRange_SelectedIndexChanged);
            this.cboRange.TextChanged += new System.EventHandler(this.cboRange_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(178, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 46;
            this.label2.Text = "期間名稱：";
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboYear.Location = new System.Drawing.Point(94, 50);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(78, 25);
            this.cboYear.TabIndex = 48;
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            this.cboYear.TextChanged += new System.EventHandler(this.cboYear_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(1, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 45;
            this.label1.Text = "校車搭車年度：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(3, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 45;
            this.label3.Text = "學生類別：";
            // 
            // cboStudent
            // 
            this.cboStudent.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStudent.FormattingEnabled = true;
            this.cboStudent.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboStudent.Items.AddRange(new object[] {
            "一般生",
            "新生"});
            this.cboStudent.Location = new System.Drawing.Point(73, 10);
            this.cboStudent.Name = "cboStudent";
            this.cboStudent.Size = new System.Drawing.Size(89, 25);
            this.cboStudent.TabIndex = 48;
            this.cboStudent.SelectedIndexChanged += new System.EventHandler(this.cboStudent_SelectedIndexChanged);
            this.cboStudent.TextChanged += new System.EventHandler(this.cboStudent_TextChanged);
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.Transparent;
            this.btnExport.Location = new System.Drawing.Point(377, 86);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(74, 22);
            this.btnExport.TabIndex = 50;
            this.btnExport.Text = "匯出資料";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // StudentByBusDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 120);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.cboRange);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboStudent);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "StudentByBusDetail";
            this.Text = "匯出學生搭乘校車資料";
            this.Load += new System.EventHandler(this.StudentByBusDetail_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboRange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboStudent;
        private System.Windows.Forms.Button btnExport;
    }
}