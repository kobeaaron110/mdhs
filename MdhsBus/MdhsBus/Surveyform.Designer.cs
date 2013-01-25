namespace MdhsBus
{
    partial class Surveyform
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
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.lnkCancel = new System.Windows.Forms.LinkLabel();
            this.lnkSelAll = new System.Windows.Forms.LinkLabel();
            this.SelectView = new DevComponents.DotNetBar.Controls.ListViewEx();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnRollbook = new System.Windows.Forms.Button();
            this.btnTicket = new System.Windows.Forms.Button();
            this.btnBus = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cboRange
            // 
            this.cboRange.FormattingEnabled = true;
            this.cboRange.Location = new System.Drawing.Point(260, 12);
            this.cboRange.Name = "cboRange";
            this.cboRange.Size = new System.Drawing.Size(204, 25);
            this.cboRange.TabIndex = 43;
            this.cboRange.SelectedIndexChanged += new System.EventHandler(this.cboRange_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(191, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 42;
            this.label2.Text = "期間名稱：";
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(96, 12);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(78, 25);
            this.cboYear.TabIndex = 44;
            this.cboYear.SelectedIndexChanged += new System.EventHandler(this.cboYear_SelectedIndexChanged);
            this.cboYear.TextChanged += new System.EventHandler(this.cboYear_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 17);
            this.label1.TabIndex = 41;
            this.label1.Text = "校車搭車年度：";
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            this.labelX1.Location = new System.Drawing.Point(13, 44);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(103, 23);
            this.labelX1.TabIndex = 48;
            this.labelX1.Text = "請選擇列印班級";
            // 
            // lnkCancel
            // 
            this.lnkCancel.AutoSize = true;
            this.lnkCancel.BackColor = System.Drawing.Color.Transparent;
            this.lnkCancel.LinkColor = System.Drawing.SystemColors.Desktop;
            this.lnkCancel.Location = new System.Drawing.Point(197, 48);
            this.lnkCancel.Name = "lnkCancel";
            this.lnkCancel.Size = new System.Drawing.Size(60, 17);
            this.lnkCancel.TabIndex = 47;
            this.lnkCancel.TabStop = true;
            this.lnkCancel.Text = "全部取消";
            this.lnkCancel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCancel_LinkClicked);
            // 
            // lnkSelAll
            // 
            this.lnkSelAll.AutoSize = true;
            this.lnkSelAll.BackColor = System.Drawing.Color.Transparent;
            this.lnkSelAll.LinkColor = System.Drawing.SystemColors.Desktop;
            this.lnkSelAll.Location = new System.Drawing.Point(135, 48);
            this.lnkSelAll.Name = "lnkSelAll";
            this.lnkSelAll.Size = new System.Drawing.Size(60, 17);
            this.lnkSelAll.TabIndex = 46;
            this.lnkSelAll.TabStop = true;
            this.lnkSelAll.Text = "全部選取";
            this.lnkSelAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSelAll_LinkClicked);
            // 
            // SelectView
            // 
            this.SelectView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            // 
            // 
            // 
            this.SelectView.Border.Class = "ListViewBorder";
            this.SelectView.CheckBoxes = true;
            this.SelectView.ForeColor = System.Drawing.SystemColors.Desktop;
            this.SelectView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.SelectView.LabelWrap = false;
            this.SelectView.Location = new System.Drawing.Point(6, 73);
            this.SelectView.Name = "SelectView";
            this.SelectView.Size = new System.Drawing.Size(354, 434);
            this.SelectView.TabIndex = 45;
            this.SelectView.UseCompatibleStateImageBehavior = false;
            this.SelectView.View = System.Windows.Forms.View.List;
            this.SelectView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.SelectView_ItemChecked);
            // 
            // btnReport
            // 
            this.btnReport.BackColor = System.Drawing.Color.Transparent;
            this.btnReport.Location = new System.Drawing.Point(373, 280);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(91, 22);
            this.btnReport.TabIndex = 49;
            this.btnReport.Text = "匯出調查表";
            this.btnReport.UseVisualStyleBackColor = false;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnRollbook
            // 
            this.btnRollbook.BackColor = System.Drawing.Color.Transparent;
            this.btnRollbook.Location = new System.Drawing.Point(373, 325);
            this.btnRollbook.Name = "btnRollbook";
            this.btnRollbook.Size = new System.Drawing.Size(91, 22);
            this.btnRollbook.TabIndex = 49;
            this.btnRollbook.Text = "匯出點名表";
            this.btnRollbook.UseVisualStyleBackColor = false;
            this.btnRollbook.Click += new System.EventHandler(this.btnRollbook_Click);
            // 
            // btnTicket
            // 
            this.btnTicket.BackColor = System.Drawing.Color.Transparent;
            this.btnTicket.Location = new System.Drawing.Point(373, 369);
            this.btnTicket.Name = "btnTicket";
            this.btnTicket.Size = new System.Drawing.Size(91, 22);
            this.btnTicket.TabIndex = 49;
            this.btnTicket.Text = "匯出月票";
            this.btnTicket.UseVisualStyleBackColor = false;
            this.btnTicket.Click += new System.EventHandler(this.btnTicket_Click);
            // 
            // btnBus
            // 
            this.btnBus.BackColor = System.Drawing.Color.Transparent;
            this.btnBus.Location = new System.Drawing.Point(373, 414);
            this.btnBus.Name = "btnBus";
            this.btnBus.Size = new System.Drawing.Size(100, 22);
            this.btnBus.TabIndex = 49;
            this.btnBus.Text = "匯出乘車資訊";
            this.btnBus.UseVisualStyleBackColor = false;
            this.btnBus.Click += new System.EventHandler(this.btnBus_Click);
            // 
            // Surveyform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 519);
            this.Controls.Add(this.btnTicket);
            this.Controls.Add(this.btnRollbook);
            this.Controls.Add(this.btnBus);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.lnkCancel);
            this.Controls.Add(this.lnkSelAll);
            this.Controls.Add(this.SelectView);
            this.Controls.Add(this.cboRange);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.label1);
            this.Name = "Surveyform";
            this.Text = "列印乘車調查表";
            this.Load += new System.EventHandler(this.Surveyform_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboRange;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label1;
        private DevComponents.DotNetBar.LabelX labelX1;
        private System.Windows.Forms.LinkLabel lnkCancel;
        private System.Windows.Forms.LinkLabel lnkSelAll;
        private DevComponents.DotNetBar.Controls.ListViewEx SelectView;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnRollbook;
        private System.Windows.Forms.Button btnTicket;
        private System.Windows.Forms.Button btnBus;
    }
}