namespace MdhsBus
{
    partial class MdhsBusBalance
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
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.cboRange = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pay_btn = new DevComponents.DotNetBar.ButtonX();
            this.label3 = new System.Windows.Forms.Label();
            this.cboStudentType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(101, 40);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(78, 25);
            this.cboYear.TabIndex = 53;
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // cboRange
            // 
            this.cboRange.FormattingEnabled = true;
            this.cboRange.Location = new System.Drawing.Point(101, 71);
            this.cboRange.Name = "cboRange";
            this.cboRange.Size = new System.Drawing.Size(204, 25);
            this.cboRange.TabIndex = 54;
            this.cboRange.SelectedIndexChanged += new System.EventHandler(this.cboRange_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(8, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 51;
            this.label1.Text = "校車搭車年度：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(9, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 17);
            this.label2.TabIndex = 52;
            this.label2.Text = "校車期間名稱：";
            // 
            // pay_btn
            // 
            this.pay_btn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.pay_btn.BackColor = System.Drawing.Color.Transparent;
            this.pay_btn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.pay_btn.Location = new System.Drawing.Point(237, 102);
            this.pay_btn.Name = "pay_btn";
            this.pay_btn.Size = new System.Drawing.Size(68, 23);
            this.pay_btn.TabIndex = 55;
            this.pay_btn.Text = "對帳";
            this.pay_btn.Click += new System.EventHandler(this.pay_btn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(8, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 51;
            this.label3.Text = "學生類別：";
            // 
            // cboStudentType
            // 
            this.cboStudentType.FormattingEnabled = true;
            this.cboStudentType.Items.AddRange(new object[] {
            "一般學生",
            "新生"});
            this.cboStudentType.Location = new System.Drawing.Point(101, 9);
            this.cboStudentType.Name = "cboStudentType";
            this.cboStudentType.Size = new System.Drawing.Size(78, 25);
            this.cboStudentType.TabIndex = 53;
            this.cboStudentType.SelectedIndexChanged += new System.EventHandler(this.cboStudentType_SelectedIndexChanged);
            // 
            // MdhsBusBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 133);
            this.Controls.Add(this.pay_btn);
            this.Controls.Add(this.cboStudentType);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.cboRange);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "MdhsBusBalance";
            this.Text = "校車收費對帳";
            this.Load += new System.EventHandler(this.MdhsBusBalance_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.ComboBox cboRange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.ButtonX pay_btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboStudentType;
    }
}