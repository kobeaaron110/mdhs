namespace MdhsBus
{
    partial class AssignBusNew
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
            this.cboBus = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboBusTime = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BusStopView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.savebtn = new DevComponents.DotNetBar.ButtonX();
            this.exitbtn = new DevComponents.DotNetBar.ButtonX();
            ((System.ComponentModel.ISupportInitialize)(this.BusStopView)).BeginInit();
            this.SuspendLayout();
            // 
            // cboBus
            // 
            this.cboBus.FormattingEnabled = true;
            this.cboBus.Location = new System.Drawing.Point(276, 12);
            this.cboBus.Name = "cboBus";
            this.cboBus.Size = new System.Drawing.Size(79, 25);
            this.cboBus.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(208, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "校車車號：";
            // 
            // cboBusTime
            // 
            this.cboBusTime.FormattingEnabled = true;
            this.cboBusTime.Location = new System.Drawing.Point(79, 12);
            this.cboBusTime.Name = "cboBusTime";
            this.cboBusTime.Size = new System.Drawing.Size(110, 25);
            this.cboBusTime.TabIndex = 10;
            this.cboBusTime.SelectedIndexChanged += new System.EventHandler(this.cboBusTime_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "校車時段：";
            // 
            // BusStopView
            // 
            this.BusStopView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BusStopView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9});
            this.BusStopView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.BusStopView.Location = new System.Drawing.Point(4, 43);
            this.BusStopView.Name = "BusStopView";
            this.BusStopView.RowTemplate.Height = 24;
            this.BusStopView.Size = new System.Drawing.Size(909, 236);
            this.BusStopView.TabIndex = 42;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "電腦代號";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "站名";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "月費";
            this.Column3.Name = "Column3";
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column3.Width = 60;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "車號";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 60;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "站序";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 60;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "到站時間";
            this.Column6.Name = "Column6";
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 50;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "放學上車地點";
            this.Column7.Name = "Column7";
            this.Column7.Width = 70;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "停車地址";
            this.Column8.Name = "Column8";
            this.Column8.Width = 300;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "UID";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(396, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 17);
            this.label4.TabIndex = 43;
            // 
            // savebtn
            // 
            this.savebtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.savebtn.BackColor = System.Drawing.Color.Transparent;
            this.savebtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.savebtn.Location = new System.Drawing.Point(287, 287);
            this.savebtn.Name = "savebtn";
            this.savebtn.Size = new System.Drawing.Size(68, 23);
            this.savebtn.TabIndex = 45;
            this.savebtn.Text = "儲存";
            // 
            // exitbtn
            // 
            this.exitbtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.exitbtn.BackColor = System.Drawing.Color.Transparent;
            this.exitbtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.exitbtn.Location = new System.Drawing.Point(479, 287);
            this.exitbtn.Name = "exitbtn";
            this.exitbtn.Size = new System.Drawing.Size(68, 23);
            this.exitbtn.TabIndex = 44;
            this.exitbtn.Text = "離開";
            // 
            // AssignBusNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(923, 322);
            this.Controls.Add(this.savebtn);
            this.Controls.Add(this.exitbtn);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BusStopView);
            this.Controls.Add(this.cboBus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboBusTime);
            this.Controls.Add(this.label1);
            this.Name = "AssignBusNew";
            this.Text = "校車路線站名維護";
            this.Load += new System.EventHandler(this.AssignBusNew_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BusStopView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboBus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboBusTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView BusStopView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.Label label4;
        private DevComponents.DotNetBar.ButtonX savebtn;
        private DevComponents.DotNetBar.ButtonX exitbtn;
    }
}