using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using SmartSchool.Customization.Data;
using System.Threading;
using System.Xml;
using System.IO;
//using Aspose.Words;
using DevComponents.DotNetBar.Rendering;
using Aspose.Cells;

namespace 考試成績單範例
{
    public partial class SelectExamSubject : Office2007Form
    {
        private bool checkedOnChange = false;

        private AccessHelper accessHelper = new AccessHelper();

        private List<ClassRecord> selectedClasses = new List<ClassRecord>();

        private bool summaryFieldsChanging = false;

        //
        // 2012-12-19 aaron modified 
        // 因為 國英數  數學普科自然組稱數學甲   數學普科社會組稱數學乙
        //
        private string[] chiEngMathStrArray;

        public SelectExamSubject()
        {
            //馬路如虎口請看紅綠燈
            ManualResetEvent _waitInit = new ManualResetEvent(true);
            //變紅燈
            _waitInit.Reset();
            #region 背景載入學生修課級課程考試資訊
            BackgroundWorker bkwLoader = new BackgroundWorker();
            bkwLoader.DoWork += new DoWorkEventHandler(bkwLoader_DoWork);
            bkwLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkwLoader_RunWorkerCompleted);
            bkwLoader.RunWorkerAsync(_waitInit);
            #endregion
            //初始化表單
            InitializeComponent();
            this.UseWaitCursor = true;
            comboBoxEx1.Items.Add("資料下載中...");
            comboBoxEx1.SelectedIndex = 0;
            #region 如果系統的Renderer是Office2007Renderer，同化_ClassTeacherView,_CategoryView的顏色
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                ((Office2007Renderer)GlobalManager.Renderer).ColorTableChanged += new EventHandler(ExamScoreListSubjectSelector_ColorTableChanged);
                SetForeColor(this);
            }
            #endregion
            //this.controlContainerItem1.Control = this.panelEx3;
            this.controlContainerItem2.Control = this.panelEx2;
            #region 讀取Preference
            XmlElement config = SmartSchool.Customization.Data.SystemInformation.Preference["班級考試成績單"];
            if (config != null)
            {
                //#region 自訂樣板
                //XmlElement customize1 = (XmlElement)config.SelectSingleNode("自訂樣板");

                //if ( customize1 != null )
                //{
                //    string templateBase64 = customize1.InnerText;
                //    customizeTemplateBuffer = Convert.FromBase64String(templateBase64);
                //    radioBtn2.Enabled = linkLabel2.Enabled = true;
                //} 
                //#endregion
                //if ( config.HasAttribute("列印樣板") && config.GetAttribute("列印樣板") == "自訂" )
                //    radioBtn2.Checked = true;

                #region 統計欄位
                summaryFieldsChanging = true;
                bool check = false;
                if (bool.TryParse(config.GetAttribute("總分"), out check)) checkBox1.Checked = check;
                if (bool.TryParse(config.GetAttribute("平均"), out check)) checkBox2.Checked = check;
                if (bool.TryParse(config.GetAttribute("總分排名"), out check)) checkBox6.Checked = check;
                if (bool.TryParse(config.GetAttribute("加權總分"), out check)) checkBox3.Checked = check;
                if (bool.TryParse(config.GetAttribute("加權平均"), out check)) checkBox4.Checked = check;
                if (bool.TryParse(config.GetAttribute("加權平均排名"), out check)) checkBox5.Checked = check;
                summaryFieldsChanging = false;
                #endregion
            }
            #endregion
            //變綠燈
            _waitInit.Set();
        }

        #region 初始化相關

        void ExamScoreListSubjectSelector_ColorTableChanged(object sender, EventArgs e)
        {
            SetForeColor(this);
        }

        private void SetForeColor(Control parent)
        {
            foreach (Control var in parent.Controls)
            {
                if (var is System.Windows.Forms.RadioButton || var is System.Windows.Forms.CheckBox)
                    var.ForeColor = ((Office2007Renderer)GlobalManager.Renderer).ColorTable.CheckBoxItem.Default.Text;
                SetForeColor(var);
            }
        }

        void bkwLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //comboBoxEx1.DroppedDown=false;
            comboBoxEx1.SelectedItem = null;
            comboBoxEx1.Items.Clear();
            List<string> exams = (List<string>)e.Result;
            comboBoxEx1.Items.AddRange(exams.ToArray());
            #region 回覆上次選取的試別
            XmlElement PreferenceData = SmartSchool.Customization.Data.SystemInformation.Preference["列印班級考試成績單"];
            if (PreferenceData != null)
            {
                if (comboBoxEx1.Items.Contains(PreferenceData.GetAttribute("LastPrintExam")))
                {
                    comboBoxEx1.SelectedIndex = comboBoxEx1.Items.IndexOf(PreferenceData.GetAttribute("LastPrintExam"));
                }
            }
            #endregion
            this.UseWaitCursor = false;
        }

        void bkwLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            ManualResetEvent _waitInit = (ManualResetEvent)e.Argument;
            #region 取得選取學生修課
            //儲存選取班級中包含學生的修課資料
            List<CourseRecord> courseRecs = new List<CourseRecord>();

            List<StudentRecord> students = new List<StudentRecord>();
            selectedClasses.AddRange(accessHelper.ClassHelper.GetSelectedClass());
            foreach (ClassRecord c in selectedClasses)
            {
                foreach (StudentRecord s in c.Students)
                {
                    if (!students.Contains(s))
                        students.Add(s);
                }
            }

            MultiThreadWorker<StudentRecord> courseLoader = new MultiThreadWorker<StudentRecord>();
            courseLoader.MaxThreads = 3;
            courseLoader.PackageSize = 125;
            courseLoader.PackageWorker += new EventHandler<PackageWorkEventArgs<StudentRecord>>(courseLoader_PackageWorker);
            courseLoader.Run(students, courseRecs);
            #endregion
            #region 取得課程考試
            MultiThreadWorker<CourseRecord> examLoader = new MultiThreadWorker<CourseRecord>();
            examLoader.MaxThreads = 2;
            examLoader.PackageSize = 200;
            examLoader.PackageWorker += new EventHandler<PackageWorkEventArgs<CourseRecord>>(examLoader_PackageWorker);
            examLoader.Run(courseRecs);
            #endregion
            #region 整理試別
            List<string> exams = new List<string>();
            foreach (CourseRecord c in courseRecs)
            {
                for (int i = 0; i < c.ExamList.Count; i++)
                {
                    if (!exams.Contains(c.ExamList[i]))
                    {
                        exams.Add(c.ExamList[i]);
                    }
                }
            }
            exams.Sort();
            #endregion
            e.Result = exams;
            //等變綠燈才可以繼續
            _waitInit.WaitOne();
        }

        void examLoader_PackageWorker(object sender, PackageWorkEventArgs<CourseRecord> e)
        {
            accessHelper.CourseHelper.FillExam(e.List);
        }

        void courseLoader_PackageWorker(object sender, PackageWorkEventArgs<StudentRecord> e)
        {
            accessHelper.StudentHelper.FillAttendCourse(SmartSchool.Customization.Data.SystemInformation.SchoolYear, SmartSchool.Customization.Data.SystemInformation.Semester, e.List);//抓當學年度學期
            List<CourseRecord> courseRecs = (List<CourseRecord>)e.Argument;
            //整理每個學生的修課至courseRecs
            foreach (StudentRecord studentRec in e.List)
            {
                foreach (StudentAttendCourseRecord attendRec in studentRec.AttendCourseList)
                {
                    CourseRecord courseRec = accessHelper.CourseHelper.GetCourse("" + attendRec.CourseID)[0];
                    lock (courseRec)
                    {
                        if (!courseRecs.Contains(courseRec))
                            courseRecs.Add(courseRec);
                    }
                }
            }
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedOnChange = true;
            this.UseWaitCursor = true;
            listView1.Items.Clear();
            List<string> subjects = new List<string>();
            foreach (ClassRecord c in selectedClasses)
            {
                foreach (StudentRecord s in c.Students)
                {
                    foreach (CourseRecord course in s.AttendCourseList)
                    {
                        if (course.ExamList.Contains(comboBoxEx1.Text) && !subjects.Contains(course.Subject))
                            subjects.Add(course.Subject);
                    }
                }
            }
            //照科目重新排序
            subjects.Sort(StringComparer.Comparer);
            foreach (string s in subjects)
            {
                listView1.Items.Add(s);
            }
            #region 存入選取試別
            if (comboBoxEx1.Text != "" && comboBoxEx1.Text != "資料下載中...")
            {
                XmlElement PreferenceData = SmartSchool.Customization.Data.SystemInformation.Preference["列印班級考試成績單"];
                if (PreferenceData == null)
                {
                    PreferenceData = new XmlDocument().CreateElement("列印班級考試成績單");
                }
                PreferenceData.SetAttribute("LastPrintExam", comboBoxEx1.Text);
                SmartSchool.Customization.Data.SystemInformation.Preference["列印班級考試成績單"] = PreferenceData;
            }
            #endregion
            SetListViewChecked();
            this.UseWaitCursor = false;
            checkedOnChange = false;
        }

        private void SetListViewChecked()
        {
            Dictionary<string, bool> checkedSubjects = new Dictionary<string, bool>();
            XmlElement PreferenceData = SmartSchool.Customization.Data.SystemInformation.Preference["列印班級考試成績單"];
            if (PreferenceData != null)
            {
                foreach (XmlNode node in PreferenceData.SelectNodes("Subject"))
                {
                    XmlElement element = (XmlElement)node;
                    if (!checkedSubjects.ContainsKey(element.GetAttribute("Name")))
                        checkedSubjects.Add(element.GetAttribute("Name"), bool.Parse(element.GetAttribute("Checked")));
                }
            }
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = (checkedSubjects.ContainsKey(item.Text) ? checkedSubjects[item.Text] : true);
            }
        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //變色
            e.Item.ForeColor = e.Item.Checked ? listView1.ForeColor : Color.BlueViolet;
            //允許列印
            buttonX1.Enabled = listView1.CheckedItems.Count > 0;

            #region 儲存選取狀態
            if (checkedOnChange) return;
            XmlElement PreferenceData = SmartSchool.Customization.Data.SystemInformation.Preference["列印班級考試成績單"];
            if (PreferenceData == null)
            {
                PreferenceData = new XmlDocument().CreateElement("列印班級考試成績單");
            }
            XmlElement node = (XmlElement)PreferenceData.SelectSingleNode("Subject[@Name='" + e.Item.Text + "']");
            if (node == null)
            {
                node = (XmlElement)PreferenceData.AppendChild(PreferenceData.OwnerDocument.CreateElement("Subject"));
                node.SetAttribute("Name", e.Item.Text);
            }
            node.SetAttribute("Checked", "" + e.Item.Checked);
            SmartSchool.Customization.Data.SystemInformation.Preference["列印班級考試成績單"] = PreferenceData;
            #endregion
        }

        //設定統計欄位
        private void summaryChanged(object sender, EventArgs e)
        {
            if (summaryFieldsChanging) return;
            #region 將設定存入帳號的個人資料內，以後只要這個是帳號登入就可以用這些個人資料還原設定值
            XmlElement config = SmartSchool.Customization.Data.SystemInformation.Preference["班級考試成績單"];
            if (config == null)
                config = new XmlDocument().CreateElement("班級考試成績單");

            config.SetAttribute("總分", "" + checkBox1.Checked);
            config.SetAttribute("平均", "" + checkBox2.Checked);
            config.SetAttribute("總分排名", "" + checkBox6.Checked);
            config.SetAttribute("加權總分", "" + checkBox3.Checked);
            config.SetAttribute("加權平均", "" + checkBox4.Checked);
            config.SetAttribute("加權平均排名", "" + checkBox5.Checked);

            SmartSchool.Customization.Data.SystemInformation.Preference["班級考試成績單"] = config;
            #endregion
        }

        #endregion

        #region 列印報表

        private void buttonX1_Click(object sender, EventArgs e)
        {
            List<string> subjects = new List<string>();
            foreach (ListViewItem var in listView1.CheckedItems)
            {
                subjects.Add(var.Text);
            }
            BackgroundWorker bkw = new BackgroundWorker();
            bkw.WorkerReportsProgress = true;
            bkw.DoWork += new DoWorkEventHandler(bkw_DoWork);
            bkw.ProgressChanged += new ProgressChangedEventHandler(bkw_ProgressChanged);
            bkw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkw_RunWorkerCompleted);
            bkw.RunWorkerAsync(new object[] { comboBoxEx1.Text, subjects });
            this.Close();
        }

        void bkw_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bkw = (BackgroundWorker)sender;
            bkw.ReportProgress(1);

            #region 取得傳入的資料(選取的試別、選取要列印的科目)
            //選取的試別
            string printExam = (string)((object[])e.Argument)[0];
            //要列印的科目
            List<string> printSubjects = (List<string>)((object[])e.Argument)[1];
            #endregion

            //#region 抓取要合併列印的樣板，並建立一個空的Word檔來存放最後的結果
            ////樣版
            //MemoryStream template = new MemoryStream(radioBtn1.Checked ? Properties.Resources.班級考試成績單 : customizeTemplateBuffer);
            ////空白的Word檔
            //Document doc = new Document();
            ////清空資料(新建的Document會預設有一頁空白頁。)
            //doc.Sections.Clear(); 
            //#endregion

            List<string> otherList = new List<string>();
            foreach (System.Windows.Forms.CheckBox var in new System.Windows.Forms.CheckBox[] { checkBox1, checkBox2, checkBox3, checkBox4, checkBox5, checkBox6 })
            {
                if (var.Checked)
                    otherList.Add(var.Text);
            }

            string classyear = "";
            foreach (ClassRecord classRec in selectedClasses)
            {
                classyear = classRec.GradeYear;
                if (classyear != "")
                    break;
            }
            //最多列印12科
            int maxSubject = 7;
            Workbook template = new Workbook();
            if (classyear == "1")
                template.Open(new MemoryStream(月考成績排名_期末加權.Properties.Resources.月考成績排名_1), FileFormatType.Excel2003);
            else if (classyear == "2")
                template.Open(new MemoryStream(月考成績排名_期末加權.Properties.Resources.月考成績排名_2), FileFormatType.Excel2003);
            else
                template.Open(new MemoryStream(月考成績排名_期末加權.Properties.Resources.月考成績排名_3), FileFormatType.Excel2003);

            //設定儲存格樣式
            Style hamid = template.Styles[template.Styles.Add()];
            hamid.Font.Name = template.Worksheets[0].Cells[3, 0].Style.Font.Name;
            hamid.Font.Size = 9;
            hamid.HorizontalAlignment = TextAlignmentType.Center;
            hamid.VerticalAlignment = TextAlignmentType.Top;
            Style hamidleft = template.Styles[template.Styles.Add()];
            hamidleft.Font.Name = template.Worksheets[0].Cells[3, 0].Style.Font.Name;
            hamidleft.Font.Size = 10;
            hamidleft.HorizontalAlignment = TextAlignmentType.Left;
            hamidleft.VerticalAlignment = TextAlignmentType.Top;
            Style haleft = template.Styles[template.Styles.Add()];
            haleft.Font.Name = template.Worksheets[0].Cells[3, 0].Style.Font.Name;
            haleft.Font.Size = 12;
            haleft.HorizontalAlignment = TextAlignmentType.Left;
            Style haright = template.Styles[template.Styles.Add()];
            haright.Font.Name = template.Worksheets[0].Cells[3, 0].Style.Font.Name;
            haright.Font.Size = 12;
            haright.HorizontalAlignment = TextAlignmentType.Right;

            int classIndex = 0;
            int classIndex1 = 0;
            int maxNumberStudentsInClass = 55;
            #region 每一個班級會用樣板與成績單資料做一次合併列印，並將結果整合到一份Word檔中
            //計算回報進度用
            double progress = 0;
            //班級數目
            int classcnt = 0;
            //
            // 2012-12-13 aaron
            // 因為 忠信 已將 bug 修正 不會有班級順序顛倒的問題
            // 將 Reverse() 拿掉 
            //
            //selectedClasses.Reverse();
            foreach (ClassRecord classRec in selectedClasses)
            {
                // 2012-12-19 aaron
                InitailChiEngMathStrArray(classRec);
                int indexAvg = 0;
                string sheetName = classRec.ClassName.Replace(":", "_");//避免在sheetName中出現":"(Eexcel不支援)
                if (classRec.GradeYear == "" || classRec.GradeYear == "未分年級")
                    continue;
                if (classRec.ClassName.IndexOf("選修") >= 0 || classRec.ClassName.IndexOf("夜輔") >= 0 || classRec.ClassName.IndexOf("26") > 0)
                    continue;
                if (int.Parse(classRec.GradeYear) > 3)
                    continue;
                #region 抓取整班級學生的考試成績。並整理成成績單的內容
                #region 整理學生考試成績資料
                //學生的各欄位資料
                Dictionary<StudentRecord, Dictionary<string, string>> classExamScoreTable0 = new Dictionary<StudentRecord, Dictionary<string, string>>();
                Dictionary<StudentRecord, Dictionary<string, string>> classExamScoreTable = new Dictionary<StudentRecord, Dictionary<string, string>>();
                //抓學生考試成績
                accessHelper.StudentHelper.FillExamScore(SmartSchool.Customization.Data.SystemInformation.SchoolYear, SmartSchool.Customization.Data.SystemInformation.Semester, classRec.Students);
                //整理列印科目+級別+學分數
                List<string> groups = new List<string>();
                List<string> pro_groups = new List<string>();
                Dictionary<string, List<StudentRecord>> FirstStudentRec = new Dictionary<string, List<StudentRecord>>();
                Dictionary<string, List<StudentRecord>> advanceStudentRec = new Dictionary<string, List<StudentRecord>>();
                #region 整理學生修課紀錄
                //參予排序的學生
                Dictionary<StudentRecord, decimal> canRankList = new Dictionary<StudentRecord, decimal>();
                Dictionary<StudentRecord, decimal> canRankList2 = new Dictionary<StudentRecord, decimal>();
                //記錄每個學生有未評分的KEY(如果整班都未評分就不管，但如果發現有評分就要把學生從可排序清單中移除)
                Dictionary<StudentRecord, List<string>> nonScoreKeys = new Dictionary<StudentRecord, List<string>>();
                //上次考試學生的各欄位資料
                Dictionary<StudentRecord, Dictionary<string, string>> last_classExamScoreTable = new Dictionary<StudentRecord, Dictionary<string, string>>();
                //上次考試參予排序的學生
                Dictionary<StudentRecord, decimal> last_canRankList = new Dictionary<StudentRecord, decimal>();
                //考科的考試次數
                Dictionary<StudentRecord, Dictionary<string, int>> classExamScoreTime = new Dictionary<StudentRecord, Dictionary<string, int>>();
                //考科的考試次數加權
                Dictionary<StudentRecord, Dictionary<string, decimal>> classExamScoreTime0 = new Dictionary<StudentRecord, Dictionary<string, decimal>>();
                // aaron 課程_學分數
                Dictionary<string, decimal> subjectCredit = new Dictionary<string, decimal>(); 
                foreach (StudentRecord studentRec in classRec.Students)
                {
                    //加入table
                    classExamScoreTable0.Add(studentRec, new Dictionary<string, string>());
                    classExamScoreTable.Add(studentRec, new Dictionary<string, string>());
                    last_classExamScoreTable.Add(studentRec, new Dictionary<string, string>());
                    classExamScoreTime.Add(studentRec, new Dictionary<string, int>());
                    classExamScoreTime0.Add(studentRec, new Dictionary<string, decimal>());
                    //加權總分
                    decimal scoreCount = 0;
                    //加權量
                    decimal scoreAdd = 1;
                    //參加排名
                    bool canRank = true;
                    //總學分數
                    int CreditCount = 0;
                    //總分
                    decimal sum = 0;
                    //總科目數
                    int SbjCount = 0;
                    //上次之總分
                    decimal last_sum = 0;
                    bool last_canRank = true;

                    foreach (StudentAttendCourseRecord attendRec in studentRec.AttendCourseList)
                    {
                        if (printSubjects.Contains(attendRec.Subject) && (attendRec.ExamList.Contains("第一次段考") || attendRec.ExamList.Contains("第二次段考") || attendRec.ExamList.Contains("期末考")))
                        {
                            //把科目、級別、學分數兜成 "_科目_級別_學分數_"的字串，這個字串在不同科目級別學分數會成為唯一值
                            //string key = attendRec.Subject + "^_^" + attendRec.SubjectLevel + "^_^" + attendRec.Credit;
                            // BMK ---- aaron 2013-01-24 key = 科目
                            string key = attendRec.Subject ;
                            bool hasScore = false;
                            //5/29列印全部包含教師未輸入的科目
                            if (!groups.Contains(key))
                                groups.Add(key);
                            #region 檢查這個KEY有沒有評分同時計算總分平均及是否可排名
                            foreach (ExamScoreInfo examScore in studentRec.ExamScoreList)
                            {
                                if ((examScore.ExamName == "第一次段考" || examScore.ExamName == "第二次段考" || examScore.ExamName == "期末考") && key == examScore.Subject )
                                {
                                    //是要列印的科目
                                    //if ( !groups.Contains(key) )
                                    //    groups.Add(key);
                                    // BMK 一般正常成績
                                    hasScore = true;
                                    if (examScore.SpecialCase == "")//一般正常成績
                                    {
                                        if (!classExamScoreTable[studentRec].ContainsKey(key))
                                        {
                                            classExamScoreTable0[studentRec].Add(key, examScore.ExamScore.ToString());
                                            classExamScoreTable[studentRec].Add(key, "");
                                            classExamScoreTime[studentRec].Add(key, 1);
                                            classExamScoreTime0[studentRec].Add(key, 1);

                                            //aaron  subjectCredit
                                            if (!subjectCredit.ContainsKey(key))
                                            {
                                                subjectCredit.Add(key, examScore.Credit);
                                            }
                                        }
                                        else
                                        {
                                            if (classExamScoreTable[studentRec][key] != "缺")
                                            {
                                                classExamScoreTime[studentRec][key]++;
                                                if (classExamScoreTime[studentRec][key] == 2)
                                                    scoreAdd = 1.1M;
                                                else if (classExamScoreTime[studentRec][key] == 3)
                                                    scoreAdd = 1.2M;
                                                else if (classExamScoreTime[studentRec][key] == 4)
                                                    scoreAdd = 1.3M;
                                                else if (classExamScoreTime[studentRec][key] == 5)
                                                    scoreAdd = 1.4M;
                                                classExamScoreTable0[studentRec][key] = ((decimal.Parse(classExamScoreTable0[studentRec][key]) + examScore.ExamScore * scoreAdd)).ToString();
                                                classExamScoreTime0[studentRec][key] += scoreAdd;
                                            }
                                            else
                                            {
                                                classExamScoreTable0[studentRec][key] = examScore.ExamScore.ToString();
                                                classExamScoreTime[studentRec][key] = 1;
                                                classExamScoreTime0[studentRec][key] = 1;
                                            }
                                        }
                                        //加權總分
                                        scoreCount += examScore.ExamScore * examScore.Credit;
                                        //學分
                                        CreditCount += examScore.Credit;
                                        //總分
                                        sum += examScore.ExamScore;
                                        //總科目數
                                        SbjCount++;
                                    }
                                    else//特殊成績狀況
                                    {
                                        canRank = false;
                                        if (!classExamScoreTable[studentRec].ContainsKey(key))
                                            classExamScoreTable[studentRec].Add(key, examScore.SpecialCase);
                                        else
                                            classExamScoreTable[studentRec][key] = examScore.SpecialCase;
                                    }
                                }
                            }
                            #endregion
                            //發現沒有評分
                            if (!hasScore)
                            {
                                #region 加入學生未評分科目
                                if (!nonScoreKeys.ContainsKey(studentRec))
                                    nonScoreKeys.Add(studentRec, new List<string>());
                                if (!nonScoreKeys[studentRec].Contains(key))
                                    nonScoreKeys[studentRec].Add(key);
                                #endregion
                                classExamScoreTable[studentRec].Add(key, "未輸入");
                            }
                        }
                    }
                    sum = 0;
                    SbjCount = classExamScoreTable0[studentRec].Keys.Count;
                    foreach (string var in classExamScoreTable0[studentRec].Keys)
                    {
                        // 2013-01-24 aaron
                        // BMK 應林主任要求 科目 = 科目成績 * (1/1.1/1.2) * 學分數  , 不除 (1/1.1/1.2) * 學分數
                        // 2013-01-24 pm:5:00  應林主任要求 科目 = 科目成績 * (1/1.1/1.2) * 學分數(只有國英數)  , 不除 (1/1.1/1.2) * 學分數(只有國英數)
                        // 第一次月考權重 1
                        // 第二次月考權重 1.1
                        // 第三次月考權重 1.2
                        //classExamScoreTable[studentRec][var] = (decimal.Parse(classExamScoreTable0[studentRec][var]) / classExamScoreTime0[studentRec][var]).ToString();
                        //classExamScoreTable[studentRec][var] = (decimal.Parse(classExamScoreTable0[studentRec][var]) * subjectCredit[var]).ToString();
                        classExamScoreTable[studentRec][var] = (decimal.Parse(classExamScoreTable0[studentRec][var]) ).ToString();
                        sum += decimal.Parse(classExamScoreTable[studentRec][var]);
                    }
                    classExamScoreTable[studentRec].Add("加權總分", scoreCount.ToString());
                    classExamScoreTable[studentRec].Add("加權平均", (scoreCount / (CreditCount == 0 ? 1 : CreditCount)).ToString(".00"));
                    classExamScoreTable[studentRec].Add("平均", (sum / (SbjCount == 0 ? 1 : SbjCount)).ToString(".00"));
                    classExamScoreTable[studentRec].Add("總分", sum.ToString());
                    if (canRank)
                    {
                        canRankList.Add(studentRec, decimal.Parse((scoreCount / (CreditCount == 0 ? 1 : CreditCount)).ToString(".00")));
                        canRankList2.Add(studentRec, sum);
                    }
                    if (!classExamScoreTable[studentRec].ContainsKey("總分排名"))
                        classExamScoreTable[studentRec].Add("總分排名", "");
                }
                //如果學生在要列印科目中發現未評分項目則從可排名清單中移除
                #region 如果學生在要列印科目中發現未評分項目則從可排名清單中移除
                //5/29列印全部包含教師未輸入的科目，先將此段取消
                //foreach ( StudentRecord stu in nonScoreKeys.Keys )
                //{
                //    foreach ( string key in nonScoreKeys[stu] )
                //    {
                //        if ( groups.Contains(key) && canRankList.ContainsKey(stu) )
                //        {
                //            canRankList.Remove(stu);
                //            canRankList2.Remove(stu);
                //            last_canRankList.Remove(stu);
                //        }
                //    }
                //}
                #endregion
                List<decimal> rankScoreList = new List<decimal>();
                rankScoreList.AddRange(canRankList.Values);
                rankScoreList.Sort();
                rankScoreList.Reverse();
                List<decimal> rankScoreList2 = new List<decimal>();
                rankScoreList2.AddRange(canRankList2.Values);
                rankScoreList2.Sort();
                rankScoreList2.Reverse();
                //foreach (StudentRecord stuRec in classExamScoreTable.Keys)
                //{
                //    int k = 0;
                //    if (stuRec.SeatNo == "45")
                //        k = 0;
                //    if (canRankList.ContainsKey(stuRec))
                //        classExamScoreTable[stuRec].Add("加權平均排名", "" + ( rankScoreList.IndexOf(decimal.Parse(classExamScoreTable[stuRec]["加權平均"])) + 1 ));
                //    else
                //        classExamScoreTable[stuRec].Add("加權平均排名", "");

                //    if ( canRankList2.ContainsKey(stuRec) )
                //        classExamScoreTable[stuRec].Add("總分排名", "" + ( rankScoreList2.IndexOf(decimal.Parse(classExamScoreTable[stuRec]["總分"])) + 1 ));
                //    else
                //        classExamScoreTable[stuRec].Add("總分排名", "");
                //    stuRec.Fields.Add("總分排名", classExamScoreTable[stuRec]["總分排名"]);
                //    if (classExamScoreTable[stuRec]["總分排名"] != "")
                //    {
                //        //if (int.Parse(classExamScoreTable[stuRec]["總分排名"]) <= 3)
                //        //{
                //            stuRec.Fields.Add("總分", classExamScoreTable[stuRec]["總分"]);
                //            if (!FirstStudentRec.ContainsKey(stuRec.RefClass.ClassName))
                //                FirstStudentRec.Add(stuRec.RefClass.ClassName, new List<StudentRecord>());
                //            FirstStudentRec[stuRec.RefClass.ClassName].Add(stuRec);
                //        //}
                //    }
                //}
                #endregion

                //排序要列印的科目
                //groups.Sort(StringComparer.Comparer);
                //FirstStudentRec[classRec.ClassName].Sort(名次排序);
                foreach (string key in groups)
                {
                    //if (key != "國文" && key != "英文" && key != "數學")
                    if (!IsArrayContains(chiEngMathStrArray, key))
                    {
                        pro_groups.Add(key);
                    }
                }

                #endregion

                // BMK 改用Excel列印
                #region 改用Excel列印
                #region 建立樣板
                // 新增一個Sheet
                int newSheetIndex = template.Worksheets.AddCopy(0);
                //這一個新增的sheet
                Worksheet sheet1 = template.Worksheets[newSheetIndex];
                //設定名稱
                if (sheetName != "")
                    sheet1.Name = sheetName;
                #endregion

                for (int i = 0; i < otherList.Count; i++)
                {
                    if (otherList[i] == "總分排名")
                    {
                        sheet1.Cells[1, groups.Count + 2 + i].PutValue("班排名");
                        sheet1.Cells[1, groups.Count + 3 + i].PutValue("科排名");
                    }
                    else
                        sheet1.Cells[1, groups.Count + 2 + i].PutValue(otherList[i]);
                }
                int studentIndex = 0;
                #region 欄位英文
                string eng_str = "BCDEFGHIJKLMNOPQRSTUVWXYZAAABACADAEAFAGAH";
                List<string> column_str = new List<string>();
                for (int i = 0; i < 25; i++)
                {
                    column_str.Add(eng_str.Substring(i, 1));
                }
                for (int i = 25; i < 33; i++)
                {
                    column_str.Add(eng_str.Substring(i + (i - 25), 2));
                }
                #endregion
                #region 表頭
                sheet1.Cells[studentIndex, 0].PutValue(SmartSchool.Customization.Data.SystemInformation.SchoolYear + "學年度" + ((SmartSchool.Customization.Data.SystemInformation.Semester == 1) ? "上學期 " : "下學期 ") + sheetName + " 第一、二次月考與期末考成績排名");
                sheet1.Cells.CreateRange(studentIndex, 1, false).RowHeight = 20;
                sheet1.Cells.CreateRange(studentIndex, 0, 1, groups.Count + 6).Merge();
                sheet1.Cells[studentIndex, 0].Style.HorizontalAlignment = TextAlignmentType.Center;
                sheet1.Cells[studentIndex, 0].Style.Font.Size = 16;
                for (byte j = 0; j <= groups.Count + 5; j++)
                {
                    //sheet1.Cells.SetColumnWidth(j, 10);
                    //各儲存格畫格線
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                }
                studentIndex++;

                //標題
                //sheet1.Cells[studentIndex, 0].PutValue("班級");
                sheet1.Cells[studentIndex, 0].PutValue("學號");
                sheet1.Cells[studentIndex, 1].PutValue("姓名");
                //sheet1.Cells[studentIndex, 3].PutValue("總分");
                //sheet1.Cells[studentIndex, 4].PutValue("班排名");
                //sheet1.Cells[studentIndex, 5].PutValue("科排名");
                //科目
                //for (int i = 0; i < groups.Count; i++)
                //{
                //    string[] list = groups[i].Split(new string[] { "^_^" }, StringSplitOptions.None);
                //    sheet1.Cells[studentIndex, i + 2].PutValue(list[0]);
                //}
                int ii = 2;
                foreach (string subj in chiEngMathStrArray)
                {
                    sheet1.Cells[studentIndex, ii].PutValue(subj);
                    ii++;
                }
                foreach (string subj in pro_groups)
                {
                    sheet1.Cells[studentIndex, ii].PutValue(subj);
                    ii++;
                }

                for (byte j = 0; j <= groups.Count + 5; j++)
                {
                    //各儲存格畫格線
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                }
                studentIndex++;
                #endregion

                //學生
                for (int i = 0; i < classRec.Students.Count; i++)
                {
                    //複製列
                    sheet1.Cells.CopyRow(sheet1.Cells, 2, studentIndex);
                    //複製列高
                    sheet1.Cells.SetRowHeight(studentIndex, sheet1.Cells.GetRowHeight(2));
                    if (classRec.Students.Count > i)
                    {
                        #region 如果這一列有學生
                        //學號
                        sheet1.Cells[studentIndex, 0].PutValue(classRec.Students[i].StudentNumber);
                        //姓名
                        sheet1.Cells[studentIndex, 1].PutValue(classRec.Students[i].StudentName);
                        int index = 2;
                        #region 各科成績
                        foreach (string key in chiEngMathStrArray)
                        {
                            decimal score;
                            if (classExamScoreTable[classRec.Students[i]].ContainsKey(key))
                            {
                                decimal.TryParse(classExamScoreTable[classRec.Students[i]][key], out score);
                                // 四捨五入到小數第二位
                                score = Math.Round(score, 2, MidpointRounding.AwayFromZero);
                                sheet1.Cells[studentIndex, index].PutValue(score);
                            }
                            else
                                sheet1.Cells[studentIndex, index].PutValue("--");
                            index++;
                        }
                        foreach (string key in pro_groups)
                        {
                            decimal score;
                            if (classExamScoreTable[classRec.Students[i]].ContainsKey(key))
                            {
                                decimal.TryParse(classExamScoreTable[classRec.Students[i]][key], out score);
                                // 四捨五入到小數第二位
                                score = Math.Round(score, 2, MidpointRounding.AwayFromZero);
                                sheet1.Cells[studentIndex, index].PutValue(score);
                            }
                            else
                                sheet1.Cells[studentIndex, index].PutValue("--");
                            index++;
                        }
                        #endregion

                        index = groups.Count + 2;
                        foreach (string key in otherList)
                        {
                            #region 統計欄位
                            //decimal score;
                            int startindex = 3;
                            string Formula = "";
                            
                            //startindex += classcnt * 70;
                            if (classExamScoreTable[classRec.Students[i]].ContainsKey(key))
                            {
                                //decimal.TryParse(classExamScoreTable[classRec.Students[i]][key], out score);
                                //sheet.Cells[classIndex + studentIndex, index].PutValue(score);
                                int sbjcount = 0;
                                if (key == "總分")
                                {
                                    //sheet1.Cells[studentIndex, index].R1C1Formula = "=SUM(C" + (studentIndex + 1) + ":" + column_str[groups.Count] + (studentIndex + 1) + ")";
                                    sbjcount = 0;
                                    Formula = "=SUM(";
                                    foreach (string subj in chiEngMathStrArray)
                                    {
                                        //string subj = key1.Substring(0, key1.IndexOf("^_^"));
                                        sbjcount++;
                                        if (subj == "國文")
                                            Formula += column_str[sbjcount] + (studentIndex + 1);
                                        else if (subj == "英文")
                                            Formula += ":";
                                        else
                                        {
                                            //Formula += column_str[sbjcount] + (studentIndex + 1) + ")*1.2+SUM(";
                                            Formula += column_str[sbjcount] + (studentIndex + 1) + ")*4+SUM(";
                                        }
                                    }

                                    foreach (string subj in pro_groups)
                                    {
                                        sbjcount++;
                                        if (sbjcount == groups.Count)
                                            Formula += column_str[sbjcount] + (studentIndex + 1);
                                        else if (sbjcount == 4)
                                            Formula += column_str[sbjcount] + (studentIndex + 1) + ":";
                                    }
                                    sheet1.Cells[studentIndex, index].R1C1Formula = Formula + ")";
                                    //sheet1.Cells[studentIndex, index].R1C1Formula = "=SUM(C" + (studentIndex + 1) + ":" + column_str[groups.Count] + (studentIndex + 1) + ")";
                                }
                                else if (key == "平均")
                                {
                                    sbjcount = 0;
                                    indexAvg = index;
                                    //Formula = "=(SUM(";
                                    //foreach (string subj in chiEngMathStrArray)
                                    //{
                                    //    //string subj = key1.Substring(0, key1.IndexOf("^_^"));
                                    //    sbjcount++;
                                    //    if (subj == "國文")
                                    //        Formula += column_str[sbjcount] + (studentIndex + 1);
                                    //    else if (subj == "英文")
                                    //        Formula += ":";
                                    //    else
                                    //        Formula += column_str[sbjcount] + (studentIndex + 1) + ")*1.2+SUM(";
                                    //}

                                    //foreach (string subj in pro_groups)
                                    //{
                                    //    sbjcount++;
                                    //    if (sbjcount == groups.Count)
                                    //        Formula += column_str[sbjcount] + (studentIndex + 1);
                                    //    else if (sbjcount == 4)
                                    //        Formula += column_str[sbjcount] + (studentIndex + 1) + ":";
                                    //}
                                    //Formula += "))/(3.6+" + (groups.Count - 3) + ")";
                                    //sheet1.Cells[studentIndex, index].R1C1Formula = Formula;
                                    //sheet1.Cells[studentIndex, index].Style.Number = 2;
                                }
                                else if (key == "總分排名")
                                {
                                    sheet1.Cells[studentIndex, index].R1C1Formula = "=RANK(" + column_str[groups.Count + 1] + (studentIndex + 1) + ",$" + column_str[groups.Count + 1] + "$" + startindex + ":$" + column_str[groups.Count + 1] + "$" + (classRec.Students.Count + 2) + ")";
                                }
                                else
                                {
                                    sheet1.Cells[studentIndex, index].PutValue(classExamScoreTable[classRec.Students[i]][key]);
                                }
                            }
                            else
                                sheet1.Cells[studentIndex, index].PutValue("--");
                            #endregion
                            index++;
                        }
                        #endregion
                    }
                    else
                    {
                        //清空補齊的列
                        for (int j = 0; j <= groups.Count + otherList.Count; j++)
                        {
                            sheet1.Cells[studentIndex, j].PutValue(null);
                        }
                    }
                    for (byte j = 0; j <= groups.Count + 5; j++)
                    {
                        //各儲存格畫格線
                        sheet1.Cells[studentIndex, j].Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                        sheet1.Cells[studentIndex, j].Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                        sheet1.Cells[studentIndex, j].Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                        sheet1.Cells[studentIndex, j].Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    }
                    #region 每5列就加特殊邊
                    if (studentIndex > 2 && (studentIndex - 2) % 5 == 0)
                    {
                        Aspose.Cells.Range eachFiveRow = sheet1.Cells.CreateRange(studentIndex, 0, 1, groups.Count + 3 + otherList.Count);
                        eachFiveRow.SetOutlineBorder(Aspose.Cells.BorderType.TopBorder, CellBorderType.Medium, Color.Black);
                    }
                    #endregion
                    studentIndex++;
                }

                //foreach (StudentRecord stuRec in FirstStudentRec[classRec.ClassName])
                //{
                //    if (int.Parse(stuRec.Fields["總分排名"].ToString()) > 0)
                //    {
                //        #region 如果這一列有學生
                //        sheet1.Cells.CopyRow(sheet1.Cells, 2, studentIndex);
                //        sheet1.Cells.SetRowHeight(studentIndex, 15);
                //        sheet1.Cells[studentIndex, 0].PutValue(stuRec.RefClass.ClassName);
                //        //學號
                //        sheet1.Cells[studentIndex, 1].PutValue(stuRec.StudentNumber);
                //        //姓名
                //        sheet1.Cells[studentIndex, 2].PutValue(stuRec.StudentName);
                //        int index = 3;
                //        foreach (string key in otherList)
                //        {
                //            #region 統計欄位
                //            //decimal score;
                //            int startindex = 3;
                //            startindex += classcnt * 68;
                //            if (stuRec.Fields.ContainsKey(key))
                //            {
                //                //decimal.TryParse(classExamScoreTable[classRec.Students[i]][key], out score);
                //                //report.Worksheets[0].Cells[classIndex + studentIndex, index].PutValue(score);
                //                if (key == "總分")
                //                {
                //                    sheet1.Cells[studentIndex, index].PutValue(stuRec.Fields["總分"]);
                //                }
                //                else if (key == "總分排名")
                //                {
                //                    sheet1.Cells[studentIndex, index].PutValue(stuRec.Fields["總分排名"]);
                //                    sheet1.Cells[studentIndex, index].Style.Number = 2;
                //                }
                //            }
                //            //else
                //            //    sheet1.Cells[classIndex + studentIndex, index].PutValue("--");
                //            #endregion
                //            for (byte j = 0; j <= 5; j++)
                //            {
                //                //各儲存格畫格線
                //                sheet1.Cells[studentIndex, j].Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                //                sheet1.Cells[studentIndex, j].Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                //                sheet1.Cells[studentIndex, j].Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                //                sheet1.Cells[studentIndex, j].Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                //            }
                //            if (key != "平均")
                //                index++;
                //        }
                //        #endregion
                //        studentIndex++;
                //    }
                //else
                //{
                //    //清空補齊的列
                //    for (int j = 0; j < maxSubject + otherList.Count; j++)
                //    {
                //        sheet1.Cells[classIndex + studentIndex, j].PutValue(null);
                //    }
                //}
                #region 每5列就加特殊邊
                //if (studentIndex > 2 && (studentIndex - 2) % 5 == 0)
                //{
                //    Aspose.Cells.Range eachFiveRow = sheet1.Cells.CreateRange(studentIndex, 0, 1, 5);
                //    if (comboBoxEx1.Text == "第二次段考")
                //        eachFiveRow = sheet1.Cells.CreateRange(classIndex + studentIndex, 0, 1, maxSubject + 5 + otherList.Count);
                //    eachFiveRow.SetOutlineBorder(Aspose.Cells.BorderType.TopBorder, CellBorderType.Double, Color.Black);
                //}
                #endregion
                //}

                //sheet1.HPageBreaks.Add(classIndex + studentIndex, maxSubject + 1 + otherList.Count);
                classIndex += studentIndex;
                classIndex1++;
                classcnt++;
                #endregion

                #endregion
                bkw.ReportProgress((int)(++progress * 100.0d / selectedClasses.Count));
                sheet1.Cells.DeleteColumn(indexAvg);
            }
            #endregion
            //將R欄刪除 
            //sheet1.Cells.DeleteColumn(17);
            template.Worksheets.RemoveAt(0);
            e.Result = template;
        }

        void bkw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //回報到下方狀態列
            SmartSchool.Customization.PlugIn.Global.SetStatusBarMessage("月考(含期末)加權成績產生中...", e.ProgressPercentage);
        }

        void bkw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SmartSchool.Customization.PlugIn.Global.SetStatusBarMessage("月考(含期末)加權成績產生完成");
            Workbook book = (Workbook)e.Result;
            Common.SaveToFile("單學期月考成績-加權", book);
            //Document doc = (Document)e.Result;
            //#region 儲存並開啟檔案

            //string reportName = "班級考試成績單";
            //string path = Path.Combine(Application.StartupPath, "Reports");
            //if ( !Directory.Exists(path) )
            //    Directory.CreateDirectory(path);
            //path = Path.Combine(path, reportName + ".doc");

            //if ( File.Exists(path) )
            //{
            //    int i = 1;
            //    while ( true )
            //    {
            //        string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + ( i++ ) + Path.GetExtension(path);
            //        if ( !File.Exists(newPath) )
            //        {
            //            path = newPath;
            //            break;
            //        }
            //    }
            //}

            //try
            //{
            //    doc.Save(path, SaveFormat.Doc);
            //    System.Diagnostics.Process.Start(path);
            //}
            //catch
            //{
            //    SaveFileDialog sd = new SaveFileDialog();
            //    sd.Title = "另存新檔";
            //    sd.FileName = reportName + ".doc";
            //    sd.Filter = "Word檔案 (*.doc)|*.doc|所有檔案 (*.*)|*.*";
            //    if ( sd.ShowDialog() == DialogResult.OK )
            //    {
            //        try
            //        {
            //            doc.Save(sd.FileName, SaveFormat.AsposePdf);
            //        }
            //        catch
            //        {
            //            MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return;
            //        }
            //    }
            //} 
            //#endregion
        }

        private int 名次排序(StudentRecord c1, StudentRecord c2)
        {
            object o1 = "0", o2 = "0";
            decimal d1 = 0, d2 = 0;
            c1.Fields.TryGetValue("總分", out o1);
            c2.Fields.TryGetValue("總分", out o2);
            decimal.TryParse("" + o1, out d1);
            decimal.TryParse("" + o2, out d2);
            return d2.CompareTo(d1);
        }

        private int 進步排名排序(StudentRecord c1, StudentRecord c2)
        {
            object o1 = "0", o2 = "0";
            decimal d1 = 0, d2 = 0;
            c1.Fields.TryGetValue("進步排名", out o1);
            c2.Fields.TryGetValue("進步排名", out o2);
            decimal.TryParse("" + o1, out d1);
            decimal.TryParse("" + o2, out d2);
            return d2.CompareTo(d1);
        }

        //
        // 2012-12-19 aaron modified 
        // 因為 國英數  數學普科自然組稱數學甲   數學普科社會組稱數學乙
        // 初始化  chiEngMathStrArray
        //
        private void InitailChiEngMathStrArray(ClassRecord varClassRecord)
        {


            if (varClassRecord.GradeYear == "3")
            {
                if (varClassRecord.ClassName.IndexOf("普") >= 0
                        && (varClassRecord.ClassName.IndexOf("自") >= 0
                        || varClassRecord.ClassName.IndexOf("乙") >= 0))
                {
                    chiEngMathStrArray = new string[] { "國文", "英文", "數學甲" };
                }
                else if (varClassRecord.ClassName.IndexOf("普") >= 0
                        && (varClassRecord.ClassName.IndexOf("社") >= 0
                        || varClassRecord.ClassName.IndexOf("丙") >= 0))
                {
                    chiEngMathStrArray = new string[] { "國文", "英文", "數學乙" };
                }
                else
                {
                    chiEngMathStrArray = new string[] { "國文", "英文", "數學" };
                }
            }
            else
            {
                chiEngMathStrArray = new string[] { "國文", "英文", "數學" };
            }
        }
        private bool IsArrayContains(string[] varArrStr, string varStr)
        {
            foreach (string var in varArrStr)
            {
                if (var == varStr)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
}

