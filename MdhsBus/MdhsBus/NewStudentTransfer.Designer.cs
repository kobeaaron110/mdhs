namespace MdhsBus
{
    partial class NewStudentTransfer
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboRange = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.savebtn = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(105, 16);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(78, 25);
            this.cboYear.TabIndex = 42;
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 41;
            this.label1.Text = "校車搭車年度：";
            // 
            // cboRange
            // 
            this.cboRange.FormattingEnabled = true;
            this.cboRange.Location = new System.Drawing.Point(267, 16);
            this.cboRange.Name = "cboRange";
            this.cboRange.Size = new System.Drawing.Size(204, 25);
            this.cboRange.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(198, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 43;
            this.label2.Text = "期間名稱：";
            // 
            // savebtn
            // 
            this.savebtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.savebtn.BackColor = System.Drawing.Color.Transparent;
            this.savebtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.savebtn.Location = new System.Drawing.Point(403, 69);
            this.savebtn.Name = "savebtn";
            this.savebtn.Size = new System.Drawing.Size(68, 23);
            this.savebtn.TabIndex = 45;
            this.savebtn.Text = "轉換";
            this.savebtn.Click += new System.EventHandler(this.savebtn_Click);
            // 
            // NewStudentTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 108);
            this.Controls.Add(this.savebtn);
            this.Controls.Add(this.cboRange);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.label1);
            this.Name = "NewStudentTransfer";
            this.Text = "新生搭乘校車轉換成高一學生";
            this.Load += new System.EventHandler(this.NewStudentTransfer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboRange;
        private System.Windows.Forms.Label label2;
        private DevComponents.DotNetBar.ButtonX savebtn;
    }
}