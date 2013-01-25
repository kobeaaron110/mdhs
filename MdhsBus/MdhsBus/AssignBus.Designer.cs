namespace MdhsBus
{
    partial class AssignBus
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
            this.cboBusTime = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BusStopView = new System.Windows.Forms.ListView();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.exitbtn = new DevComponents.DotNetBar.ButtonX();
            this.delbtn = new DevComponents.DotNetBar.ButtonX();
            this.editbtn = new DevComponents.DotNetBar.ButtonX();
            this.addbtn = new DevComponents.DotNetBar.ButtonX();
            this.label2 = new System.Windows.Forms.Label();
            this.cboBus = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cboBusTime
            // 
            this.cboBusTime.FormattingEnabled = true;
            this.cboBusTime.Location = new System.Drawing.Point(78, 12);
            this.cboBusTime.Name = "cboBusTime";
            this.cboBusTime.Size = new System.Drawing.Size(110, 25);
            this.cboBusTime.TabIndex = 6;
            this.cboBusTime.SelectedIndexChanged += new System.EventHandler(this.cboBusTime_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "校車時段：";
            // 
            // BusStopView
            // 
            this.BusStopView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader5,
            this.columnHeader3,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader7});
            this.BusStopView.FullRowSelect = true;
            this.BusStopView.Location = new System.Drawing.Point(12, 43);
            this.BusStopView.Name = "BusStopView";
            this.BusStopView.Size = new System.Drawing.Size(480, 286);
            this.BusStopView.TabIndex = 34;
            this.BusStopView.UseCompatibleStateImageBehavior = false;
            this.BusStopView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.DisplayIndex = 5;
            this.columnHeader6.Text = "UID";
            this.columnHeader6.Width = 0;
            // 
            // columnHeader5
            // 
            this.columnHeader5.DisplayIndex = 0;
            this.columnHeader5.Text = "電腦代號";
            this.columnHeader5.Width = 70;
            // 
            // columnHeader3
            // 
            this.columnHeader3.DisplayIndex = 1;
            this.columnHeader3.Text = "車號";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 45;
            // 
            // columnHeader2
            // 
            this.columnHeader2.DisplayIndex = 2;
            this.columnHeader2.Text = "金額";
            // 
            // columnHeader4
            // 
            this.columnHeader4.DisplayIndex = 3;
            this.columnHeader4.Text = "站序";
            this.columnHeader4.Width = 50;
            // 
            // columnHeader1
            // 
            this.columnHeader1.DisplayIndex = 4;
            this.columnHeader1.Text = "站名";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "停車地點";
            this.columnHeader7.Width = 200;
            // 
            // exitbtn
            // 
            this.exitbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.exitbtn.BackColor = System.Drawing.Color.Transparent;
            this.exitbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.exitbtn.Location = new System.Drawing.Point(512, 307);
            this.exitbtn.Name = "exitbtn";
            this.exitbtn.Size = new System.Drawing.Size(68, 23);
            this.exitbtn.TabIndex = 33;
            this.exitbtn.Text = "離開";
            this.exitbtn.Click += new System.EventHandler(this.exitbtn_Click);
            // 
            // delbtn
            // 
            this.delbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.delbtn.BackColor = System.Drawing.Color.Transparent;
            this.delbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.delbtn.Location = new System.Drawing.Point(512, 155);
            this.delbtn.Name = "delbtn";
            this.delbtn.Size = new System.Drawing.Size(75, 23);
            this.delbtn.TabIndex = 32;
            this.delbtn.Text = "刪除";
            this.delbtn.Click += new System.EventHandler(this.delbtn_Click);
            // 
            // editbtn
            // 
            this.editbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.editbtn.BackColor = System.Drawing.Color.Transparent;
            this.editbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.editbtn.Location = new System.Drawing.Point(512, 117);
            this.editbtn.Name = "editbtn";
            this.editbtn.Size = new System.Drawing.Size(75, 23);
            this.editbtn.TabIndex = 31;
            this.editbtn.Text = "修改";
            this.editbtn.Click += new System.EventHandler(this.editbtn_Click);
            // 
            // addbtn
            // 
            this.addbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.addbtn.BackColor = System.Drawing.Color.Transparent;
            this.addbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.addbtn.Location = new System.Drawing.Point(512, 79);
            this.addbtn.Name = "addbtn";
            this.addbtn.Size = new System.Drawing.Size(75, 23);
            this.addbtn.TabIndex = 30;
            this.addbtn.Text = "新增";
            this.addbtn.Click += new System.EventHandler(this.addbtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(207, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "校車車號：";
            // 
            // cboBus
            // 
            this.cboBus.FormattingEnabled = true;
            this.cboBus.Location = new System.Drawing.Point(275, 12);
            this.cboBus.Name = "cboBus";
            this.cboBus.Size = new System.Drawing.Size(79, 25);
            this.cboBus.TabIndex = 6;
            this.cboBus.SelectedIndexChanged += new System.EventHandler(this.cboBus_SelectedIndexChanged);
            // 
            // AssignBus
            // 
            this.ClientSize = new System.Drawing.Size(600, 337);
            this.Controls.Add(this.BusStopView);
            this.Controls.Add(this.exitbtn);
            this.Controls.Add(this.delbtn);
            this.Controls.Add(this.editbtn);
            this.Controls.Add(this.addbtn);
            this.Controls.Add(this.cboBus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboBusTime);
            this.Controls.Add(this.label1);
            this.Name = "AssignBus";
            this.Text = "校車路線站名維護";
            this.Load += new System.EventHandler(this.AssignBus_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboBusTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView BusStopView;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private DevComponents.DotNetBar.ButtonX exitbtn;
        private DevComponents.DotNetBar.ButtonX delbtn;
        private DevComponents.DotNetBar.ButtonX editbtn;
        private DevComponents.DotNetBar.ButtonX addbtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboBus;
        private System.Windows.Forms.ColumnHeader columnHeader7;
    }
}