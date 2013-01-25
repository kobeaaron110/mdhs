namespace MdhsBus
{
    partial class Setup
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
            this.BusSetupView = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.exitbtn = new DevComponents.DotNetBar.ButtonX();
            this.delbtn = new DevComponents.DotNetBar.ButtonX();
            this.editbtn = new DevComponents.DotNetBar.ButtonX();
            this.addbtn = new DevComponents.DotNetBar.ButtonX();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(122, 12);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(110, 25);
            this.cboYear.TabIndex = 8;
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(17, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "校車搭車年度：";
            // 
            // BusSetupView
            // 
            this.BusSetupView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.BusSetupView.FullRowSelect = true;
            this.BusSetupView.Location = new System.Drawing.Point(12, 52);
            this.BusSetupView.Name = "BusSetupView";
            this.BusSetupView.Size = new System.Drawing.Size(485, 229);
            this.BusSetupView.TabIndex = 9;
            this.BusSetupView.UseCompatibleStateImageBehavior = false;
            this.BusSetupView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "UID";
            this.columnHeader1.Width = 0;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "期間名稱";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "搭車起始日期";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "搭車截止日期";
            this.columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "繳費截止日";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "天數";
            // 
            // exitbtn
            // 
            this.exitbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.exitbtn.BackColor = System.Drawing.Color.Transparent;
            this.exitbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.exitbtn.Location = new System.Drawing.Point(503, 258);
            this.exitbtn.Name = "exitbtn";
            this.exitbtn.Size = new System.Drawing.Size(68, 23);
            this.exitbtn.TabIndex = 37;
            this.exitbtn.Text = "離開";
            this.exitbtn.Click += new System.EventHandler(this.exitbtn_Click);
            // 
            // delbtn
            // 
            this.delbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.delbtn.BackColor = System.Drawing.Color.Transparent;
            this.delbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.delbtn.Location = new System.Drawing.Point(503, 128);
            this.delbtn.Name = "delbtn";
            this.delbtn.Size = new System.Drawing.Size(75, 23);
            this.delbtn.TabIndex = 36;
            this.delbtn.Text = "刪除";
            this.delbtn.Click += new System.EventHandler(this.delbtn_Click);
            // 
            // editbtn
            // 
            this.editbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.editbtn.BackColor = System.Drawing.Color.Transparent;
            this.editbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.editbtn.Location = new System.Drawing.Point(503, 90);
            this.editbtn.Name = "editbtn";
            this.editbtn.Size = new System.Drawing.Size(75, 23);
            this.editbtn.TabIndex = 35;
            this.editbtn.Text = "修改";
            this.editbtn.Click += new System.EventHandler(this.editbtn_Click);
            // 
            // addbtn
            // 
            this.addbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addbtn.BackColor = System.Drawing.Color.Transparent;
            this.addbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addbtn.Location = new System.Drawing.Point(503, 52);
            this.addbtn.Name = "addbtn";
            this.addbtn.Size = new System.Drawing.Size(75, 23);
            this.addbtn.TabIndex = 34;
            this.addbtn.Text = "新增";
            this.addbtn.Click += new System.EventHandler(this.addbtn_Click);
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "校車時段";
            this.columnHeader7.Width = 80;
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 307);
            this.Controls.Add(this.exitbtn);
            this.Controls.Add(this.delbtn);
            this.Controls.Add(this.editbtn);
            this.Controls.Add(this.addbtn);
            this.Controls.Add(this.BusSetupView);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.label1);
            this.Name = "Setup";
            this.Text = "校車時間新增設定";
            this.Load += new System.EventHandler(this.Setup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView BusSetupView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private DevComponents.DotNetBar.ButtonX exitbtn;
        private DevComponents.DotNetBar.ButtonX delbtn;
        private DevComponents.DotNetBar.ButtonX editbtn;
        private DevComponents.DotNetBar.ButtonX addbtn;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
    }
}