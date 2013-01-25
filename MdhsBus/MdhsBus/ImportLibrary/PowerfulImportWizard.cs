﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Aspose.Cells;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Rendering;
using System.Collections.ObjectModel;

namespace ImportLibrary
{
    public partial class PowerfulImportWizard : FISCA.Presentation.Controls.BaseForm, IPowerfulImportWizard
    {
        private string _Title;
        private IntelliSchool.DSA.ClientFramework.ControlCommunication.ListViewCheckAllManager _CheckAllManager = new IntelliSchool.DSA.ClientFramework.ControlCommunication.ListViewCheckAllManager();
        private Dictionary<string, int> _ImportFields = new Dictionary<string, int>();
        private Workbook _WorkBook;
        private Workbook _ErrorWB = null;
        Dictionary<RowData, string> _ErrorRows, _WarningRows;
        private ButtonX advButton;
        private DevComponents.DotNetBar.ControlContainerItem advContainer;
        private int _PackageLimint = 500;
        private LinkLabel helpButton;
        private FieldsCollection _RequiredFields;
        private FieldsCollection _ImportableFields;
        private FieldsCollection _SelectedFields;
        private OptionCollection _Options;
        private PanelEx _OptionsContainer;
        private bool _Trim = true;


        public PowerfulImportWizard(string title, Image img)
        {
            InitializeComponent();

            #region 加入進階按紐跟HELP按鈕
            _OptionsContainer = new PanelEx();
            _OptionsContainer.Font = this.Font;
            _OptionsContainer.ColorSchemeStyle = eDotNetBarStyle.Office2007;
            _OptionsContainer.Size = new Size(100, 100);
            _OptionsContainer.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            _OptionsContainer.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            _OptionsContainer.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            _OptionsContainer.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            _OptionsContainer.Style.GradientAngle = 90;
            _Options = new OptionCollection();
            _Options.ItemsChanged += new EventHandler(_Options_ItemsChanged);

            advContainer = new ControlContainerItem();
            advContainer.AllowItemResize = false;
            advContainer.GlobalItem = false;
            advContainer.MenuVisibility = eMenuVisibility.VisibleAlways;
            advContainer.Control = _OptionsContainer;

            ItemContainer itemContainer2 = new ItemContainer();
            itemContainer2.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            itemContainer2.MinimumSize = new System.Drawing.Size(0, 0);
            itemContainer2.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            advContainer});

            advButton = new ButtonX();
            advButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            advButton.Text = "    進階";
            advButton.Top = this.wizard1.Controls[1].Controls[0].Top;
            advButton.Left = 5;
            advButton.Size = this.wizard1.Controls[1].Controls[0].Size;
            advButton.Visible = true;
            advButton.SubItems.Add(itemContainer2);
            advButton.PopupSide = ePopupSide.Top;
            advButton.SplitButton = true;
            advButton.Enabled = false;
            advButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            advButton.AutoExpandOnClick = true;
            advButton.SubItemsExpandWidth = 16;
            advButton.FadeEffect = false;
            advButton.FocusCuesEnabled = false;
            this.wizard1.Controls[1].Controls.Add(advButton);

            helpButton = new LinkLabel();
            helpButton.AutoSize = true;
            helpButton.BackColor = System.Drawing.Color.Transparent;
            helpButton.Location = new System.Drawing.Point(81, 10);
            helpButton.Size = new System.Drawing.Size(69, 17);
            helpButton.TabStop = true;
            helpButton.Text = "Help";
            helpButton.Visible = false;
            helpButton.Click += delegate { if (HelpButtonClick != null)HelpButtonClick(this, new EventArgs()); };
            helpButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            this.wizard1.Controls[1].Controls.Add(helpButton);
            #endregion

            #region 設定Wizard會跟著Style跑
            //this.wizard1.FooterStyle.ApplyStyle(( GlobalManager.Renderer as Office2007Renderer ).ColorTable.GetClass(ElementStyleClassKeys.RibbonFileMenuBottomContainerKey));
            //this.wizard1.HeaderStyle.ApplyStyle((GlobalManager.Renderer as Office2007Renderer).ColorTable.GetClass(ElementStyleClassKeys.RibbonFileMenuBottomContainerKey));
            this.wizard1.FooterStyle.BackColorGradientAngle = -90;
            this.wizard1.FooterStyle.BackColorGradientType = eGradientType.Linear;
            //this.wizard1.FooterStyle.BackColor = (GlobalManager.Renderer as Office2007Renderer).ColorTable.RibbonBar.Default.TopBackground.Start;
            //this.wizard1.FooterStyle.BackColor2 = (GlobalManager.Renderer as Office2007Renderer).ColorTable.RibbonBar.Default.TopBackground.End;
            //this.wizard1.BackColor = (GlobalManager.Renderer as Office2007Renderer).ColorTable.RibbonBar.Default.TopBackground.Start;
            //this.wizard1.BackgroundImage = null;
            //for (int i = 0; i < 6; i++)
            //{
            //    (this.wizard1.Controls[1].Controls[i] as ButtonX).ColorTable = eButtonColor.OrangeWithBackground;
            //}
            //(this.wizard1.Controls[0].Controls[1] as System.Windows.Forms.Label).ForeColor = (GlobalManager.Renderer as Office2007Renderer).ColorTable.RibbonBar.MouseOver.TitleText;
            //(this.wizard1.Controls[0].Controls[2] as System.Windows.Forms.Label).ForeColor = (GlobalManager.Renderer as Office2007Renderer).ColorTable.RibbonBar.Default.TitleText;
            _CheckAllManager.TargetComboBox = this.checkBox1;
            _CheckAllManager.TargetListView = this.listView1;
            #endregion

            _Title = this.Text = title;

            lblReqFields.Text = "";

            foreach (WizardPage page in wizard1.WizardPages)
            {
                page.PageTitle = _Title;
                if (img != null)
                {
                    Bitmap b = new Bitmap(48, 48);
                    using (Graphics g = Graphics.FromImage(b))
                        g.DrawImage(img, 0, 0, 48, 48);
                    page.PageHeaderImage = b;
                }
            }
            _RequiredFields = new FieldsCollection();
            _ImportableFields = new FieldsCollection();
            _SelectedFields = new FieldsCollection();
            _RequiredFields.ItemsChanged += new EventHandler(FieldsChanged);
            _ImportableFields.ItemsChanged += new EventHandler(FieldsChanged);
        }
        Dictionary<VirtualCheckBox, DevComponents.DotNetBar.Controls.CheckBoxX> _CheckBoxs = new Dictionary<VirtualCheckBox, DevComponents.DotNetBar.Controls.CheckBoxX>();
        Dictionary<VirtualRadioButton, System.Windows.Forms.RadioButton> _RadioButtons = new Dictionary<VirtualRadioButton, System.Windows.Forms.RadioButton>();
        List<VirtualCheckItem> _AllItems = new List<VirtualCheckItem>();
        void _Options_ItemsChanged(object sender, EventArgs e)
        {
            _CheckBoxs.Clear();
            _RadioButtons.Clear();
            int width = 0;
            int Y = 1;
            int speace = 1;
            int visibleCount = 0;
            foreach (Control control in _OptionsContainer.Controls)
            {
                control.Dispose();
            }
            _OptionsContainer.Controls.Clear();
            foreach (VirtualCheckItem item in _Options)
            {
                if (!_AllItems.Contains(item))
                {
                    _AllItems.Add(item);
                    item.VisibleChanged += new EventHandler(item_VisibleChanged);
                }
                if (item.Visible)
                {
                    visibleCount++;
                    if (item is VirtualCheckBox)
                    {
                        #region 加入CheckBox
                        DevComponents.DotNetBar.Controls.CheckBoxX checkbox = new DevComponents.DotNetBar.Controls.CheckBoxX();
                        checkbox.TabIndex = 0;
                        checkbox.TabStop = true;
                        checkbox.AutoSize = true;
                        checkbox.Text = item.Text;
                        checkbox.Checked = item.Checked;
                        checkbox.Enabled = item.Enabled;
                        checkbox.Tag = item;
                        checkbox.CheckedChanged += new EventHandler(checkbox_CheckedChanged);
                        item.CheckedChanged += new EventHandler(syncCheckBox);
                        item.TextChanged += new EventHandler(syncCheckBox);
                        item.EnabledChanged += new EventHandler(syncCheckBox);
                        checkbox.Location = new Point(9, Y);
                        _OptionsContainer.Controls.Add(checkbox);//要先加入Panel後抓Size才準
                        Y += checkbox.Height + speace;
                        if (checkbox.PreferredSize.Width + 25 > width)
                            width = checkbox.PreferredSize.Width + 25;
                        _CheckBoxs.Add(item as VirtualCheckBox, checkbox);
                        #endregion
                    }
                    if (item is VirtualRadioButton)
                    {
                        #region 加入RadioButton
                        System.Windows.Forms.RadioButton radioButton = new System.Windows.Forms.RadioButton();
                        radioButton.TabIndex = 0;
                        radioButton.TabStop = true;
                        radioButton.AutoSize = true;
                        radioButton.Text = item.Text;
                        radioButton.Checked = item.Checked;
                        radioButton.Enabled = item.Enabled; ;
                        radioButton.Tag = item;
                        radioButton.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                        item.CheckedChanged += new EventHandler(syncRadioButton);
                        item.TextChanged += new EventHandler(syncRadioButton);
                        item.EnabledChanged += new EventHandler(syncRadioButton);
                        radioButton.Location = new Point(12, Y);
                        _OptionsContainer.Controls.Add(radioButton);
                        //radioButton.Invalidate();
                        //radioButton.PerformLayout();
                        Y += radioButton.Height + speace;
                        if (radioButton.PreferredSize.Width + 25 > width)
                            width = radioButton.PreferredSize.Width + 25;
                        _RadioButtons.Add(item as VirtualRadioButton, radioButton);
                        #endregion
                    }
                }
            }
            _OptionsContainer.Size = new Size(width, Y);
            advButton.Enabled = visibleCount > 0;
            advContainer.RecalcSize();
            SetForeColor(_OptionsContainer);
        }

        void checkbox_CheckedChanged(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.Controls.CheckBoxX checkbox = (DevComponents.DotNetBar.Controls.CheckBoxX)sender;
            VirtualCheckItem item = (VirtualCheckItem)checkbox.Tag;
            if (item.Checked != checkbox.Checked)
                item.Checked = checkbox.Checked;
        }

        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.RadioButton radioButton = (System.Windows.Forms.RadioButton)sender;
            VirtualCheckItem item = (VirtualCheckItem)radioButton.Tag;
            if (item.Checked != radioButton.Checked)
                item.Checked = radioButton.Checked;
        }

        void item_VisibleChanged(object sender, EventArgs e)
        {
            _Options_ItemsChanged(null, null);
        }

        void syncCheckBox(object sender, EventArgs e)
        {
            VirtualCheckBox item = (VirtualCheckBox)sender;
            if (!_CheckBoxs.ContainsKey(item)) return;
            DevComponents.DotNetBar.Controls.CheckBoxX checkbox = _CheckBoxs[item];
            checkbox.Text = item.Text;
            checkbox.Enabled = item.Enabled;
            if (item.Checked != checkbox.Checked)
                checkbox.Checked = item.Checked;
            if (checkbox.PreferredSize.Width + 25 > _OptionsContainer.Width)
                _OptionsContainer.Width = checkbox.PreferredSize.Width + 25;
        }

        void syncRadioButton(object sender, EventArgs e)
        {
            VirtualRadioButton item = (VirtualRadioButton)sender;
            if (!_RadioButtons.ContainsKey(item)) return;
            System.Windows.Forms.RadioButton radioButton = _RadioButtons[item];
            radioButton.Text = item.Text;
            radioButton.Enabled = item.Enabled;
            if (item.Checked != radioButton.Checked)
                radioButton.Checked = item.Checked;
            if (radioButton.PreferredSize.Width + 25 > _OptionsContainer.Width)
                _OptionsContainer.Width = radioButton.PreferredSize.Width + 25;
        }

        private void SetForeColor(Control parent)
        {
            foreach (Control var in parent.Controls)
            {
                if (var is System.Windows.Forms.RadioButton)
                    var.ForeColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.CheckBoxItem.Default.Text;
                SetForeColor(var);
            }
        }

        void FieldsChanged(object sender, EventArgs e)
        {
            this.wizardPage1.NextButtonEnabled = eWizardButtonState.False;
            linkLabel2.Visible = false;
            errorFile.Clear();
            Application.DoEvents();
            foreach (string field in _RequiredFields)
            {
                if (!_ImportableFields.Contains(field))
                {
                    _ImportableFields.Add(field);
                }
            }
            lblReqFields.Text = "";
            foreach (string req in _RequiredFields)
            {
                lblReqFields.Text += (lblReqFields.Text == "" ? "" : " 、 ") + "<font color=\"#7A4E2B\"><b>" + req + "</b></font>";
            }
            if (txtFile.Text != "")
                CheckFile();
        }

        private void CheckFile()
        {
            this.wizardPage1.NextButtonEnabled = eWizardButtonState.False;
            string messingFields = "";
            foreach (string key in _RequiredFields)
            {
                if (!_ImportFields.ContainsKey(key))
                {
                    messingFields += (messingFields == "" ? "" : ",") + key;
                }
            }
            if (messingFields != "")
            {
                errorFile.SetIconAlignment(txtFile, ErrorIconAlignment.MiddleLeft);
                errorFile.SetError(txtFile, "缺少必要欄位:\n" + messingFields);
                return;
            }
            listView1.SuspendLayout();
            listView1.Items.Clear();
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (string field in _ImportableFields)
            {
                if (!_RequiredFields.Contains(field) && _ImportFields.ContainsKey(field))
                {
                    ListViewItem item = new ListViewItem(field);
                    item.Checked = true;
                    items.Add(item);
                }
            }
            listView1.Items.AddRange(items.ToArray());
            listView1.ResumeLayout();
            this.wizardPage1.NextButtonEnabled = eWizardButtonState.True;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (SelectSourceFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (txtFile.Text == SelectSourceFileDialog.FileName)
                    txtFile_TextChanged(null, null);
                else
                    txtFile.Text = SelectSourceFileDialog.FileName;
            }
        }

        private void txtFile_TextChanged(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            linkLabel2.Visible = false;
            errorFile.Clear();
            Application.DoEvents();
            _ImportFields.Clear();
            _WorkBook = new Workbook();
            try
            {
                _WorkBook.Open(txtFile.Text);
            }
            catch
            {
                errorFile.SetIconAlignment(txtFile, ErrorIconAlignment.MiddleLeft);
                errorFile.SetError(txtFile, "檔案不存在、格式錯誤或檔案開啟中無法讀取。");
                this.UseWaitCursor = false;
                return;
            }
            linkLabel2.Visible = true;
            this.UseWaitCursor = false;
            //讀取檔案內所有的欄位
            for (int i = 0; i <= (int)_WorkBook.Worksheets[0].Cells.MaxColumn; i++)
            {
                string fieldName = GetTrimText("" + _WorkBook.Worksheets[0].Cells[0, i].StringValue);
                if (fieldName != "" && !_ImportFields.ContainsKey(fieldName))
                    _ImportFields.Add(GetTrimText("" + _WorkBook.Worksheets[0].Cells[0, i].StringValue), i);
            }
            if (LoadSource != null)
            {
                LoadSourceEventArgs args = new LoadSourceEventArgs();
                args.Fields.AddRange(_ImportFields.Keys);
                LoadSource(this, args);
            }
            CheckFile();
        }

        private BackgroundWorker _BKWValidate;

        private void wizardPage3_AfterPageDisplayed(object sender, WizardPageChangeEventArgs e)
        {
            this.progressBarX1.Value = 0;
            lblWarningCount.Text = lblErrCount.Text = "0";
            this.wizardPage3.FinishButtonEnabled = eWizardButtonState.False;
            linkLabel1.Visible = false;
            labelX2.Text = "資料驗證中";
            linkLabel3.Tag = null;
            linkLabel3.Visible = false;
            Application.DoEvents();

            _BKWValidate = new BackgroundWorker();
            _BKWValidate.WorkerReportsProgress = true;
            _BKWValidate.WorkerSupportsCancellation = true;
            _BKWValidate.DoWork += new DoWorkEventHandler(_BKWValidate_DoWork);
            _BKWValidate.ProgressChanged += new ProgressChangedEventHandler(_BKWValidate_ProgressChanged);
            _BKWValidate.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_BKWValidate_RunWorkerCompleted);

            List<string> fields = new List<string>();
            string fileName = txtFile.Text;
            fields.AddRange(_RequiredFields);
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                    fields.Add(item.Text);
            }
            _ErrorRows = new Dictionary<RowData, string>();
            _WarningRows = new Dictionary<RowData, string>();
            Workbook wb = new Workbook();
            wb.Copy(_WorkBook);
            _BKWValidate.RunWorkerAsync(new object[] { fields, _ImportFields, wb });
        }

        private void wizardPage3_BackButtonClick(object sender, CancelEventArgs e)
        {
            if (_BKWValidate != null && _BKWValidate.IsBusy)
                _BKWValidate.CancelAsync();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_BKWValidate != null && _BKWValidate.IsBusy)
                _BKWValidate.CancelAsync();
            linkLabel3.Tag = "!";
        }

        private void ImportStudent_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_BKWValidate != null && _BKWValidate.IsBusy)
                _BKWValidate.CancelAsync();
        }

        private void _BKWValidate_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bkw = (BackgroundWorker)sender;
            List<string> selectedFields = (List<string>)((object[])e.Argument)[0];
            Dictionary<string, int> importFields = (Dictionary<string, int>)((object[])e.Argument)[1];
            Workbook wb = (Workbook)((object[])e.Argument)[2];
            Style passStyle = wb.Styles[wb.Styles.Add()];
            passStyle.Font.Color = Color.Green;
            Dictionary<RowData, int> rowDataIndex = new Dictionary<RowData, int>();

            double progress = 0.0;

            #region 產生RowData資料
            for (int i = 1; i <= wb.Worksheets[0].Cells.MaxDataRow; i++)
            {
                RowData rowdata = new RowData();
                foreach (string fieldName in importFields.Keys)
                {
                    int index = importFields[fieldName];
                    if (wb.Worksheets[0].Cells[i, index].Type == CellValueType.IsDateTime)
                    {
                        rowdata.Add(fieldName, wb.Worksheets[0].Cells[i, index].DateTimeValue.ToString());
                    }
                    else
                        rowdata.Add(fieldName, GetTrimText("" + wb.Worksheets[0].Cells[i, index].StringValue));
                }
                wb.Worksheets[0].Cells[i, 0].Style = passStyle;
                rowDataIndex.Add(rowdata, i);
            }
            if (RowsLoad != null)
            {
                var args = new RowDataLoadEventArgs();
                args.RowDatas.AddRange(rowDataIndex.Keys);
                RowsLoad(this, args);
            }
            #endregion

            #region 識別RowData資料
            foreach (RowData row in rowDataIndex.Keys)
            {
                IdentifyRowEventArgs args = new IdentifyRowEventArgs();
                args.RowData = row;
                if (IdentifyRow != null)
                    IdentifyRow(this, args);
                if (!string.IsNullOrEmpty(args.ErrorMessage))
                    AddError(row, args.ErrorMessage);
                if (!string.IsNullOrEmpty(args.WarningMessage))
                    AddWarning(row, args.WarningMessage);
            }
            #endregion

            #region 驗證資料
            List<string> list = new List<string>();
            foreach (RowData row in rowDataIndex.Keys)
            {
                if (!list.Contains(row.ID))
                    list.Add(row.ID);
            }
            if (ValidateStart != null)
            {
                ValidateStartEventArgs args = new ValidateStartEventArgs();
                args.List = list.ToArray();
                ValidateStart(this, args);
            }
            double totleCount = (double)rowDataIndex.Count;
            double count = 0.0;
            foreach (RowData row in rowDataIndex.Keys)
            {
                #region 驗證
                string rowError = "";
                Dictionary<string, string> errorFields, warningFields;
                ValidateRowEventArgs args = new ValidateRowEventArgs();
                args.Data = row;
                args.SelectFields.AddRange(selectedFields);
                if (ValidateRow != null)
                {
                    ValidateRow(this, args);
                }
                errorFields = args.ErrorFields;
                warningFields = args.WarningFields;
                rowError = args.ErrorMessage;

                if (rowError != "" || errorFields.Count != 0)
                {
                    string message = "";
                    message += (message == "" ? "" : "\n") + "";
                    foreach (string key in errorFields.Keys)
                    {
                        message += (message == "" ? "" : "\n") + "  " + key + "：" + errorFields[key];
                    }
                    AddError(row, message);
                }
                if (warningFields.Count != 0)
                {
                    string message = "";
                    message += (message == "" ? "" : "\n") + "";
                    foreach (string key in warningFields.Keys)
                    {
                        message += (message == "" ? "" : "\n") + "  " + key + "：" + warningFields[key];
                    }
                    AddWarning(row, message);
                }
                #endregion
                if (bkw.CancellationPending)
                {
                    e.Cancel = true;
                    //填入驗證結果
                    FillReport(wb, rowDataIndex);
                    _ErrorWB = wb;
                    return;
                }
                count++;
                bkw.ReportProgress((int)(progress + count * (100.0 - progress) / totleCount), new int[] { GetErrorCount(rowDataIndex), GetWarningCount(rowDataIndex) });
            }


            if (ValidateComplete != null)
                ValidateComplete(this, new EventArgs());
            #endregion

            List<RowData> rows = new List<RowData>();
            rows.AddRange(rowDataIndex.Keys);

            bkw.ReportProgress(100, new int[] { GetErrorCount(rowDataIndex), GetWarningCount(rowDataIndex) });

            //填入驗證結果
            FillReport(wb, rowDataIndex);
            e.Result = new object[] { wb, GetErrorCount(rowDataIndex) == 0, rows, selectedFields };
        }

        private int GetErrorCount(Dictionary<RowData, int> rowDataIndex)
        {
            int count = 0;
            foreach (var row in _ErrorRows.Keys)
            {
                if (rowDataIndex.ContainsKey(row)) count++;
            }
            return count;
        }

        private int GetWarningCount(Dictionary<RowData, int> rowDataIndex)
        {
            int count = 0;
            foreach (var row in _WarningRows.Keys)
            {
                if (rowDataIndex.ContainsKey(row) && !_ErrorRows.ContainsKey(row)) count++;
            }
            return count;
        }

        private void FillReport(Workbook wb, Dictionary<RowData, int> rowDataIndex)
        {

            int errorSheetIndex = wb.Worksheets.Add();
            {
                int errc = 0;
                #region 命名
                for (; ; errc++)
                {
                    bool pass = true;
                    string n = "錯誤&警告說明" + (errc == 0 ? "" : "(" + errc + ")");
                    foreach (Aspose.Cells.Worksheet var in wb.Worksheets)
                    {
                        if (n == var.Name)
                        {
                            pass = false;
                            break;
                        }
                    }
                    if (pass) break;
                }
                #endregion
                wb.Worksheets[errorSheetIndex].Name = "錯誤&警告說明" + (errc == 0 ? "" : "(" + errc + ")");
            }
            string errorSheetName = wb.Worksheets[errorSheetIndex].Name;
            Worksheet errorSheet = wb.Worksheets[errorSheetIndex];
            errorSheet.Cells[0, 0].PutValue("行號");
            errorSheet.Cells[0, 1].PutValue("種類");
            errorSheet.Cells[0, 2].PutValue("說明");
            int errorSheetRowIndex = 1;

            Style errorStyle = wb.Styles[wb.Styles.Add()];
            Style warningStyle = wb.Styles[wb.Styles.Add()];

            Style errorStyle2 = wb.Styles[wb.Styles.Add()];
            Style warningStyle2 = wb.Styles[wb.Styles.Add()];

            errorStyle.Font.Color = Color.Red;
            errorStyle.Font.Underline = FontUnderlineType.Single;

            warningStyle.Font.Color = wb.GetMatchingColor(Color.Goldenrod);
            warningStyle.Font.Underline = FontUnderlineType.Single;

            warningStyle2.Font.Color = wb.GetMatchingColor(Color.Goldenrod);
            errorStyle2.Font.Color = Color.Red;


            #region 填入驗證結果
            SortedList<int, RowData> markedRow = new SortedList<int, RowData>();
            foreach (var row in _ErrorRows.Keys)
            {
                if (rowDataIndex.ContainsKey(row))
                    markedRow.Add(rowDataIndex[row], row);
            }
            foreach (var row in _WarningRows.Keys)
            {
                if (rowDataIndex.ContainsKey(row) && !markedRow.ContainsKey(rowDataIndex[row]))
                    markedRow.Add(rowDataIndex[row], row);
            }
            foreach (var index in markedRow.Keys)
            {
                RowData row = markedRow[index];
                if (_ErrorRows.ContainsKey(row))
                {
                    errorSheet.Cells[errorSheetRowIndex, 0].PutValue(index + 1);
                    errorSheet.Cells[errorSheetRowIndex, 1].PutValue("錯誤");
                    errorSheet.Cells[errorSheetRowIndex, 2].PutValue(_ErrorRows[row]);
                    errorSheet.Cells[errorSheetRowIndex, 0].Style = errorStyle;
                    errorSheet.Cells[errorSheetRowIndex, 1].Style = errorStyle2;
                    errorSheet.Cells[errorSheetRowIndex, 2].Style = errorStyle2;
                    errorSheet.Hyperlinks.Add(errorSheetRowIndex, 0, 1, 1, "'" + wb.Worksheets[0].Name + "'!" + wb.Worksheets[0].Cells[index, 0].Name);
                    wb.Worksheets[0].Cells[index, 0].Style = errorStyle;
                    wb.Worksheets[0].Hyperlinks.Add(index, 0, 1, 1, "'" + errorSheetName + "'!" + errorSheet.Cells[errorSheetRowIndex, 0].Name);
                    errorSheet.AutoFitRow(errorSheetRowIndex);
                    errorSheetRowIndex++;
                }
                if (_WarningRows.ContainsKey(row))
                {
                    errorSheet.Cells[errorSheetRowIndex, 0].PutValue(index + 1);
                    errorSheet.Cells[errorSheetRowIndex, 1].PutValue("警告");
                    errorSheet.Cells[errorSheetRowIndex, 2].PutValue(_WarningRows[row]);
                    errorSheet.Cells[errorSheetRowIndex, 0].Style = warningStyle;
                    errorSheet.Cells[errorSheetRowIndex, 1].Style = warningStyle2;
                    errorSheet.Cells[errorSheetRowIndex, 2].Style = warningStyle2;
                    errorSheet.Hyperlinks.Add(errorSheetRowIndex, 0, 1, 1, "'" + wb.Worksheets[0].Name + "'!" + wb.Worksheets[0].Cells[index, 0].Name);
                    if (!_ErrorRows.ContainsKey(row))
                    {
                        wb.Worksheets[0].Cells[index, 0].Style = errorStyle;
                        wb.Worksheets[0].Hyperlinks.Add(index, 0, 1, 1, "'" + errorSheetName + "'!" + errorSheet.Cells[errorSheetRowIndex, 0].Name);
                    }
                    errorSheet.AutoFitRow(errorSheetRowIndex);
                    errorSheetRowIndex++;
                }
            }
            #endregion


            errorSheet.AutoFitColumn(0);
            errorSheet.AutoFitColumn(1, 1, 500);
            errorSheet.AutoFitColumn(2, 1, 500);
        }

        private void _BKWValidate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BackgroundWorker bkw = (BackgroundWorker)sender;
            if (bkw.CancellationPending)
                return;
            progressBarX1.Value = e.ProgressPercentage;
            lblErrCount.Text = "" + (int)((int[])e.UserState)[0];
            lblWarningCount.Text = "" + (int)((int[])e.UserState)[1];
            if (linkLabel3.Visible == false && (int)((int[])e.UserState)[0] > 0 || (int)((int[])e.UserState)[1] > 0)
                linkLabel3.Visible = true;
        }

        private void _BKWValidate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker bkw = (BackgroundWorker)sender;

            linkLabel3.Visible = false;
            if (!bkw.CancellationPending && !e.Cancelled)
            {
                linkLabel1.Visible = true;
                linkLabel1.Tag = ((object[])e.Result)[0];
                wizardPage3.FinishButtonEnabled = ("" + linkLabel3.Tag != "!" && (bool)((object[])e.Result)[1]) ? eWizardButtonState.True : eWizardButtonState.False;
                this.Tag = ((object[])e.Result)[2];
                wizard1.Tag = ((object[])e.Result)[3];
                if (wizardPage3.FinishButtonEnabled == eWizardButtonState.True)
                    labelX2.Text = "資料驗證完成";
                else
                    labelX2.Text = "資料驗證失敗";
            }
            else
            {
                if (_ErrorWB != null)
                {
                    linkLabel1.Visible = true;
                    linkLabel1.Tag = _ErrorWB;
                    wizardPage3.FinishButtonEnabled = eWizardButtonState.False;
                    labelX2.Text = "資料驗證失敗";
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Workbook wb = new Workbook();
            wb.Copy((Workbook)((Control)sender).Tag);
            Completed(_Title + "_驗證", wb);
        }

        private void Completed(string inputReportName, Workbook inputDoc)
        {
            string reportName = inputReportName;

            string path = Path.Combine(Application.StartupPath, "Reports");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".xls");

            Workbook doc = inputDoc;

            if (File.Exists(path))
            {
                int i = 1;
                while (true)
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + (i++) + Path.GetExtension(path);
                    if (!File.Exists(newPath))
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            try
            {
                doc.Save(path);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                SaveFileDialog sd = new SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = reportName + ".xls";
                sd.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        doc.Save(sd.FileName);

                    }
                    catch
                    {
                        DevComponents.DotNetBar.MessageBoxEx.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void wizardPage3_FinishButtonClick(object sender, CancelEventArgs e)
        {
            Dictionary<string, List<RowData>> ID_Rows = new Dictionary<string, List<RowData>>();
            #region 將資料依ID群組
            List<RowData> rows = (List<RowData>)this.Tag;
            foreach (RowData row in rows)
            {
                if (!ID_Rows.ContainsKey(row.ID))
                    ID_Rows.Add(row.ID, new List<RowData>());
                ID_Rows[row.ID].Add(row);
            }
            #endregion
            List<List<RowData>> packages = new List<List<RowData>>();
            #region 將資料分割成數個Package
            {
                List<RowData> package = null;
                int packageCount = 0;
                foreach (string id in ID_Rows.Keys)
                {
                    if (packageCount <= 0)
                    {
                        package = new List<RowData>();
                        packages.Add(package);
                        packageCount = _PackageLimint;
                    }
                    package.AddRange(ID_Rows[id]);
                    packageCount -= ID_Rows[id].Count;
                }
            }
            #endregion
            BackgroundWorker bkwImport = new BackgroundWorker();
            bkwImport.WorkerReportsProgress = true;
            bkwImport.DoWork += new DoWorkEventHandler(bkwImport_DoWork);
            bkwImport.ProgressChanged += new ProgressChangedEventHandler(bkwImport_ProgressChanged);
            bkwImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkwImport_RunWorkerCompleted);
            bkwImport.RunWorkerAsync(new object[] { packages, wizard1.Tag });
            this.Close();
        }

        void bkwImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                throw e.Error;
            else
            {
                //_Process.FinishImport();
                if (ImportComplete != null)
                    ImportComplete(this, new EventArgs());
                FISCA.Presentation.MotherForm.SetStatusBarMessage(_Title + " 完成", 100);
            }
        }

        void bkwImport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FISCA.Presentation.MotherForm.SetStatusBarMessage(_Title + "...", e.ProgressPercentage);
        }

        void bkwImport_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bkw = (BackgroundWorker)sender;
            List<List<RowData>> packages = (List<List<RowData>>)((object[])e.Argument)[0];
            List<string> importFields = (List<string>)((object[])e.Argument)[1];
            //_Process.StartImport();
            if (ImportStart != null)
                ImportStart(this, new EventArgs());
            double totle = 0.0;
            double packageProgress = 100.0 / packages.Count;
            bkw.ReportProgress(1);
            foreach (List<RowData> package in packages)
            {
                //_Process.ImportPackage(package, importFields);
                if (ImportPackage != null)
                {
                    ImportPackageEventArgs args = new ImportPackageEventArgs();
                    args.ImportFields.AddRange(importFields);
                    args.Items.AddRange(package);
                    ImportPackage(this, args);
                }
                totle += packageProgress;
                bkw.ReportProgress((int)totle);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtFile_TextChanged(null, null);
        }

        #region ImportWizard 成員

        public void AddError(RowData rowData, string message)
        {
            lock (_ErrorRows)
            {
                if (_ErrorRows.ContainsKey(rowData))
                {
                    _ErrorRows[rowData] += "、" + message;
                }
                else
                {
                    _ErrorRows.Add(rowData, message);
                }
            }
        }

        public void AddWarning(RowData rowData, string message)
        {
            lock (_WarningRows)
            {
                if (_WarningRows.ContainsKey(rowData))
                {
                    _WarningRows[rowData] += "、" + message;
                }
                else
                {
                    _WarningRows.Add(rowData, message);
                }
            }
        }

        //public Control ControlPanel
        //{
        //    get
        //    {
        //        return advContainer.Control;
        //    }
        //    set
        //    {
        //        advContainer.Control = value;
        //        advButton.Enabled = ( advContainer.Control != null );
        //    }
        //}
        public OptionCollection Options
        {
            get { return _Options; }
        }

        public event EventHandler ControlPanelClose;

        public event EventHandler ControlPanelOpen;

        public event EventHandler HelpButtonClick;

        public bool HelpButtonVisible
        {
            get
            {
                return helpButton.Visible;
            }
            set
            {
                helpButton.Visible = value;
            }
        }

        public event EventHandler ImportComplete;

        public event EventHandler<ImportPackageEventArgs> ImportPackage;

        public event EventHandler ImportStart;

        public FieldsCollection ImportableFields
        {
            get { return _ImportableFields; }
        }

        public event EventHandler<LoadSourceEventArgs> LoadSource;

        public int PackageLimit
        {
            get
            {
                return _PackageLimint;
            }
            set
            {
                _PackageLimint = value;
            }
        }

        public FieldsCollection RequiredFields
        {
            get { return _RequiredFields; }
        }

        public FieldsCollection SelectedFields
        {
            get { return _SelectedFields; }
        }

        public event EventHandler ValidateComplete;

        public event EventHandler<ValidateRowEventArgs> ValidateRow;

        public event EventHandler<ValidateStartEventArgs> ValidateStart;

        public event EventHandler<RowDataLoadEventArgs> RowsLoad;

        public event EventHandler<IdentifyRowEventArgs> IdentifyRow;

        #endregion

        private void wizard1_WizardPageChanged(object sender, WizardPageChangeEventArgs e)
        {
            advButton.Visible = (e.NewPage == wizardPage1);
        }

        private void chkTrim_CheckedChanged(object sender, EventArgs e)
        {
            _Trim = chkTrim.Checked;
            txtFile_TextChanged(null, null);
        }

        private string GetTrimText(string text)
        {
            return _Trim ? text.Trim() : text;
        }
    }
}