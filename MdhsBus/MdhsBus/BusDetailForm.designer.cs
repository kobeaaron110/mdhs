namespace MdhsBus
{
    partial class BusDetailForm
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
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBusName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMoney = new System.Windows.Forms.TextBox();
            this.txtBusStopAddr = new System.Windows.Forms.TextBox();
            this.nrBusStopNo = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBusNumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCometime = new System.Windows.Forms.TextBox();
            this.cboUpAddr = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nrBusStopNo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Location = new System.Drawing.Point(102, 266);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 15;
            this.btnExit.Text = "離開";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Location = new System.Drawing.Point(21, 266);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "儲存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(24, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "車號";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(24, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "金額";
            // 
            // txtBusName
            // 
            this.txtBusName.Location = new System.Drawing.Point(92, 133);
            this.txtBusName.MaxLength = 16;
            this.txtBusName.Name = "txtBusName";
            this.txtBusName.Size = new System.Drawing.Size(143, 25);
            this.txtBusName.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(24, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "站名名稱";
            // 
            // txtMoney
            // 
            this.txtMoney.Location = new System.Drawing.Point(92, 71);
            this.txtMoney.MaxLength = 3;
            this.txtMoney.Name = "txtMoney";
            this.txtMoney.Size = new System.Drawing.Size(68, 25);
            this.txtMoney.TabIndex = 9;
            // 
            // txtBusStopAddr
            // 
            this.txtBusStopAddr.Location = new System.Drawing.Point(92, 164);
            this.txtBusStopAddr.MaxLength = 30;
            this.txtBusStopAddr.Name = "txtBusStopAddr";
            this.txtBusStopAddr.Size = new System.Drawing.Size(227, 25);
            this.txtBusStopAddr.TabIndex = 9;
            // 
            // nrBusStopNo
            // 
            this.nrBusStopNo.Location = new System.Drawing.Point(92, 102);
            this.nrBusStopNo.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nrBusStopNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nrBusStopNo.Name = "nrBusStopNo";
            this.nrBusStopNo.Size = new System.Drawing.Size(68, 25);
            this.nrBusStopNo.TabIndex = 16;
            this.nrBusStopNo.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(24, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 17);
            this.label4.TabIndex = 13;
            this.label4.Text = "站序";
            // 
            // txtBusNumber
            // 
            this.txtBusNumber.Location = new System.Drawing.Point(92, 40);
            this.txtBusNumber.MaxLength = 2;
            this.txtBusNumber.Name = "txtBusNumber";
            this.txtBusNumber.Size = new System.Drawing.Size(68, 25);
            this.txtBusNumber.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(24, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "電腦代碼";
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(92, 9);
            this.txtID.MaxLength = 4;
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(68, 25);
            this.txtID.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(24, 167);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "停車地點";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(0, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 17);
            this.label7.TabIndex = 8;
            this.label7.Text = "放學上車地點";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(24, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 17);
            this.label8.TabIndex = 8;
            this.label8.Text = "到站時間";
            // 
            // txtCometime
            // 
            this.txtCometime.Location = new System.Drawing.Point(92, 226);
            this.txtCometime.MaxLength = 5;
            this.txtCometime.Name = "txtCometime";
            this.txtCometime.Size = new System.Drawing.Size(85, 25);
            this.txtCometime.TabIndex = 9;
            // 
            // cboUpAddr
            // 
            this.cboUpAddr.FormattingEnabled = true;
            this.cboUpAddr.Items.AddRange(new object[] {
            "明樓",
            "明德街",
            "行政大樓前",
            "停車場"});
            this.cboUpAddr.Location = new System.Drawing.Point(92, 195);
            this.cboUpAddr.Name = "cboUpAddr";
            this.cboUpAddr.Size = new System.Drawing.Size(85, 25);
            this.cboUpAddr.TabIndex = 17;
            this.cboUpAddr.Text = "明樓";
            // 
            // BusDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 296);
            this.Controls.Add(this.cboUpAddr);
            this.Controls.Add(this.nrBusStopNo);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBusNumber);
            this.Controls.Add(this.txtCometime);
            this.Controls.Add(this.txtBusStopAddr);
            this.Controls.Add(this.txtMoney);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.txtBusName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Name = "BusDetailForm";
            this.Text = "校車資訊";
            this.Load += new System.EventHandler(this.BusDetailForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nrBusStopNo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBusName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMoney;
        private System.Windows.Forms.TextBox txtBusStopAddr;
        private System.Windows.Forms.NumericUpDown nrBusStopNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtBusNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCometime;
        private System.Windows.Forms.ComboBox cboUpAddr;
    }
}