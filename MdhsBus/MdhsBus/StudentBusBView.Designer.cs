namespace MdhsBus
{
    partial class StudentBusBView
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.StudntBusTView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // StudntBusTView
            // 
            this.StudntBusTView.Location = new System.Drawing.Point(3, 3);
            this.StudntBusTView.Name = "StudntBusTView";
            this.StudntBusTView.Size = new System.Drawing.Size(218, 356);
            this.StudntBusTView.TabIndex = 0;
            // 
            // StudentBusBView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StudntBusTView);
            this.Name = "StudentBusBView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView StudntBusTView;
    }
}
