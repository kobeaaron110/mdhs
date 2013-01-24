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

namespace �Ҹզ��Z��d��
{
    public partial class SelectExamSubject : Office2007Form
    {
        private bool checkedOnChange = false;

        private AccessHelper accessHelper = new AccessHelper();

        private List<ClassRecord> selectedClasses = new List<ClassRecord>();

        private bool summaryFieldsChanging = false;

        //
        // 2012-12-19 aaron modified 
        // �]�� ��^��  �ƾǴ���۵M�պټƾǥ�   �ƾǴ�����|�պټƾǤA
        //
        private string[] chiEngMathStrArray;

        public SelectExamSubject()
        {
            //�����p��f�Ьݬ���O
            ManualResetEvent _waitInit = new ManualResetEvent(true);
            //�ܬ��O
            _waitInit.Reset();
            #region �I�����J�ǥͭ׽үŽҵ{�Ҹո�T
            BackgroundWorker bkwLoader = new BackgroundWorker();
            bkwLoader.DoWork += new DoWorkEventHandler(bkwLoader_DoWork);
            bkwLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bkwLoader_RunWorkerCompleted);
            bkwLoader.RunWorkerAsync(_waitInit);
            #endregion
            //��l�ƪ��
            InitializeComponent();
            this.UseWaitCursor = true;
            comboBoxEx1.Items.Add("��ƤU����...");
            comboBoxEx1.SelectedIndex = 0;
            #region �p�G�t�Ϊ�Renderer�OOffice2007Renderer�A�P��_ClassTeacherView,_CategoryView���C��
            if (GlobalManager.Renderer is Office2007Renderer)
            {
                ((Office2007Renderer)GlobalManager.Renderer).ColorTableChanged += new EventHandler(ExamScoreListSubjectSelector_ColorTableChanged);
                SetForeColor(this);
            }
            #endregion
            //this.controlContainerItem1.Control = this.panelEx3;
            this.controlContainerItem2.Control = this.panelEx2;
            #region Ū��Preference
            XmlElement config = SmartSchool.Customization.Data.SystemInformation.Preference["�Z�ŦҸզ��Z��"];
            if (config != null)
            {
                //#region �ۭq�˪O
                //XmlElement customize1 = (XmlElement)config.SelectSingleNode("�ۭq�˪O");

                //if ( customize1 != null )
                //{
                //    string templateBase64 = customize1.InnerText;
                //    customizeTemplateBuffer = Convert.FromBase64String(templateBase64);
                //    radioBtn2.Enabled = linkLabel2.Enabled = true;
                //} 
                //#endregion
                //if ( config.HasAttribute("�C�L�˪O") && config.GetAttribute("�C�L�˪O") == "�ۭq" )
                //    radioBtn2.Checked = true;

                #region �έp���
                summaryFieldsChanging = true;
                bool check = false;
                if (bool.TryParse(config.GetAttribute("�`��"), out check)) checkBox1.Checked = check;
                if (bool.TryParse(config.GetAttribute("����"), out check)) checkBox2.Checked = check;
                if (bool.TryParse(config.GetAttribute("�`���ƦW"), out check)) checkBox6.Checked = check;
                if (bool.TryParse(config.GetAttribute("�[�v�`��"), out check)) checkBox3.Checked = check;
                if (bool.TryParse(config.GetAttribute("�[�v����"), out check)) checkBox4.Checked = check;
                if (bool.TryParse(config.GetAttribute("�[�v�����ƦW"), out check)) checkBox5.Checked = check;
                summaryFieldsChanging = false;
                #endregion
            }
            #endregion
            //�ܺ�O
            _waitInit.Set();
        }

        #region ��l�Ƭ���

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
            #region �^�ФW��������էO
            XmlElement PreferenceData = SmartSchool.Customization.Data.SystemInformation.Preference["�C�L�Z�ŦҸզ��Z��"];
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
            #region ���o����ǥͭ׽�
            //�x�s����Z�Ť��]�t�ǥͪ��׽Ҹ��
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
            #region ���o�ҵ{�Ҹ�
            MultiThreadWorker<CourseRecord> examLoader = new MultiThreadWorker<CourseRecord>();
            examLoader.MaxThreads = 2;
            examLoader.PackageSize = 200;
            examLoader.PackageWorker += new EventHandler<PackageWorkEventArgs<CourseRecord>>(examLoader_PackageWorker);
            examLoader.Run(courseRecs);
            #endregion
            #region ��z�էO
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
            //���ܺ�O�~�i�H�~��
            _waitInit.WaitOne();
        }

        void examLoader_PackageWorker(object sender, PackageWorkEventArgs<CourseRecord> e)
        {
            accessHelper.CourseHelper.FillExam(e.List);
        }

        void courseLoader_PackageWorker(object sender, PackageWorkEventArgs<StudentRecord> e)
        {
            accessHelper.StudentHelper.FillAttendCourse(SmartSchool.Customization.Data.SystemInformation.SchoolYear, SmartSchool.Customization.Data.SystemInformation.Semester, e.List);//���Ǧ~�׾Ǵ�
            List<CourseRecord> courseRecs = (List<CourseRecord>)e.Argument;
            //��z�C�Ӿǥͪ��׽Ҧ�courseRecs
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
            //�Ӭ�ح��s�Ƨ�
            subjects.Sort(StringComparer.Comparer);
            foreach (string s in subjects)
            {
                listView1.Items.Add(s);
            }
            #region �s�J����էO
            if (comboBoxEx1.Text != "" && comboBoxEx1.Text != "��ƤU����...")
            {
                XmlElement PreferenceData = SmartSchool.Customization.Data.SystemInformation.Preference["�C�L�Z�ŦҸզ��Z��"];
                if (PreferenceData == null)
                {
                    PreferenceData = new XmlDocument().CreateElement("�C�L�Z�ŦҸզ��Z��");
                }
                PreferenceData.SetAttribute("LastPrintExam", comboBoxEx1.Text);
                SmartSchool.Customization.Data.SystemInformation.Preference["�C�L�Z�ŦҸզ��Z��"] = PreferenceData;
            }
            #endregion
            SetListViewChecked();
            this.UseWaitCursor = false;
            checkedOnChange = false;
        }

        private void SetListViewChecked()
        {
            Dictionary<string, bool> checkedSubjects = new Dictionary<string, bool>();
            XmlElement PreferenceData = SmartSchool.Customization.Data.SystemInformation.Preference["�C�L�Z�ŦҸզ��Z��"];
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
            //�ܦ�
            e.Item.ForeColor = e.Item.Checked ? listView1.ForeColor : Color.BlueViolet;
            //���\�C�L
            buttonX1.Enabled = listView1.CheckedItems.Count > 0;

            #region �x�s������A
            if (checkedOnChange) return;
            XmlElement PreferenceData = SmartSchool.Customization.Data.SystemInformation.Preference["�C�L�Z�ŦҸզ��Z��"];
            if (PreferenceData == null)
            {
                PreferenceData = new XmlDocument().CreateElement("�C�L�Z�ŦҸզ��Z��");
            }
            XmlElement node = (XmlElement)PreferenceData.SelectSingleNode("Subject[@Name='" + e.Item.Text + "']");
            if (node == null)
            {
                node = (XmlElement)PreferenceData.AppendChild(PreferenceData.OwnerDocument.CreateElement("Subject"));
                node.SetAttribute("Name", e.Item.Text);
            }
            node.SetAttribute("Checked", "" + e.Item.Checked);
            SmartSchool.Customization.Data.SystemInformation.Preference["�C�L�Z�ŦҸզ��Z��"] = PreferenceData;
            #endregion
        }

        //�]�w�έp���
        private void summaryChanged(object sender, EventArgs e)
        {
            if (summaryFieldsChanging) return;
            #region �N�]�w�s�J�b�����ӤH��Ƥ��A�H��u�n�o�ӬO�b���n�J�N�i�H�γo�ǭӤH����٭�]�w��
            XmlElement config = SmartSchool.Customization.Data.SystemInformation.Preference["�Z�ŦҸզ��Z��"];
            if (config == null)
                config = new XmlDocument().CreateElement("�Z�ŦҸզ��Z��");

            config.SetAttribute("�`��", "" + checkBox1.Checked);
            config.SetAttribute("����", "" + checkBox2.Checked);
            config.SetAttribute("�`���ƦW", "" + checkBox6.Checked);
            config.SetAttribute("�[�v�`��", "" + checkBox3.Checked);
            config.SetAttribute("�[�v����", "" + checkBox4.Checked);
            config.SetAttribute("�[�v�����ƦW", "" + checkBox5.Checked);

            SmartSchool.Customization.Data.SystemInformation.Preference["�Z�ŦҸզ��Z��"] = config;
            #endregion
        }

        #endregion

        #region �C�L����

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

            #region ���o�ǤJ�����(������էO�B����n�C�L�����)
            //������էO
            string printExam = (string)((object[])e.Argument)[0];
            //�n�C�L�����
            List<string> printSubjects = (List<string>)((object[])e.Argument)[1];
            #endregion

            //#region ����n�X�֦C�L���˪O�A�ëإߤ@�ӪŪ�Word�ɨӦs��̫᪺���G
            ////�˪�
            //MemoryStream template = new MemoryStream(radioBtn1.Checked ? Properties.Resources.�Z�ŦҸզ��Z�� : customizeTemplateBuffer);
            ////�ťժ�Word��
            //Document doc = new Document();
            ////�M�Ÿ��(�s�ت�Document�|�w�]���@���ťխ��C)
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
            //�̦h�C�L12��
            int maxSubject = 7;
            Workbook template = new Workbook();
            if (classyear == "1")
                template.Open(new MemoryStream(��Ҧ��Z�ƦW_�����[�v.Properties.Resources.��Ҧ��Z�ƦW_1), FileFormatType.Excel2003);
            else if (classyear == "2")
                template.Open(new MemoryStream(��Ҧ��Z�ƦW_�����[�v.Properties.Resources.��Ҧ��Z�ƦW_2), FileFormatType.Excel2003);
            else
                template.Open(new MemoryStream(��Ҧ��Z�ƦW_�����[�v.Properties.Resources.��Ҧ��Z�ƦW_3), FileFormatType.Excel2003);

            //�]�w�x�s��˦�
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
            #region �C�@�ӯZ�ŷ|�μ˪O�P���Z���ư��@���X�֦C�L�A�ñN���G��X��@��Word�ɤ�
            //�p��^���i�ץ�
            double progress = 0;
            //�Z�żƥ�
            int classcnt = 0;
            //
            // 2012-12-13 aaron
            // �]�� ���H �w�N bug �ץ� ���|���Z�Ŷ����A�˪����D
            // �N Reverse() ���� 
            //
            //selectedClasses.Reverse();
            foreach (ClassRecord classRec in selectedClasses)
            {
                // 2012-12-19 aaron
                InitailChiEngMathStrArray(classRec);
                int indexAvg = 0;
                string sheetName = classRec.ClassName.Replace(":", "_");//�קK�bsheetName���X�{":"(Eexcel���䴩)
                if (classRec.GradeYear == "" || classRec.GradeYear == "�����~��")
                    continue;
                if (classRec.ClassName.IndexOf("���") >= 0 || classRec.ClassName.IndexOf("�]��") >= 0 || classRec.ClassName.IndexOf("26") > 0)
                    continue;
                if (int.Parse(classRec.GradeYear) > 3)
                    continue;
                #region �����Z�žǥͪ��Ҹզ��Z�C�þ�z�����Z�檺���e
                #region ��z�ǥͦҸզ��Z���
                //�ǥͪ��U�����
                Dictionary<StudentRecord, Dictionary<string, string>> classExamScoreTable0 = new Dictionary<StudentRecord, Dictionary<string, string>>();
                Dictionary<StudentRecord, Dictionary<string, string>> classExamScoreTable = new Dictionary<StudentRecord, Dictionary<string, string>>();
                //��ǥͦҸզ��Z
                accessHelper.StudentHelper.FillExamScore(SmartSchool.Customization.Data.SystemInformation.SchoolYear, SmartSchool.Customization.Data.SystemInformation.Semester, classRec.Students);
                //��z�C�L���+�ŧO+�Ǥ���
                List<string> groups = new List<string>();
                List<string> pro_groups = new List<string>();
                Dictionary<string, List<StudentRecord>> FirstStudentRec = new Dictionary<string, List<StudentRecord>>();
                Dictionary<string, List<StudentRecord>> advanceStudentRec = new Dictionary<string, List<StudentRecord>>();
                #region ��z�ǥͭ׽Ҭ���
                //�Ѥ��ƧǪ��ǥ�
                Dictionary<StudentRecord, decimal> canRankList = new Dictionary<StudentRecord, decimal>();
                Dictionary<StudentRecord, decimal> canRankList2 = new Dictionary<StudentRecord, decimal>();
                //�O���C�Ӿǥͦ���������KEY(�p�G��Z���������N���ޡA���p�G�o�{�������N�n��ǥͱq�i�ƧǲM�椤����)
                Dictionary<StudentRecord, List<string>> nonScoreKeys = new Dictionary<StudentRecord, List<string>>();
                //�W���Ҹվǥͪ��U�����
                Dictionary<StudentRecord, Dictionary<string, string>> last_classExamScoreTable = new Dictionary<StudentRecord, Dictionary<string, string>>();
                //�W���ҸհѤ��ƧǪ��ǥ�
                Dictionary<StudentRecord, decimal> last_canRankList = new Dictionary<StudentRecord, decimal>();
                //�Ҭ쪺�Ҹզ���
                Dictionary<StudentRecord, Dictionary<string, int>> classExamScoreTime = new Dictionary<StudentRecord, Dictionary<string, int>>();
                //�Ҭ쪺�Ҹզ��ƥ[�v
                Dictionary<StudentRecord, Dictionary<string, decimal>> classExamScoreTime0 = new Dictionary<StudentRecord, Dictionary<string, decimal>>();
                // aaron �ҵ{_�Ǥ���
                Dictionary<string, decimal> subjectCredit = new Dictionary<string, decimal>(); 
                foreach (StudentRecord studentRec in classRec.Students)
                {
                    //�[�Jtable
                    classExamScoreTable0.Add(studentRec, new Dictionary<string, string>());
                    classExamScoreTable.Add(studentRec, new Dictionary<string, string>());
                    last_classExamScoreTable.Add(studentRec, new Dictionary<string, string>());
                    classExamScoreTime.Add(studentRec, new Dictionary<string, int>());
                    classExamScoreTime0.Add(studentRec, new Dictionary<string, decimal>());
                    //�[�v�`��
                    decimal scoreCount = 0;
                    //�[�v�q
                    decimal scoreAdd = 1;
                    //�ѥ[�ƦW
                    bool canRank = true;
                    //�`�Ǥ���
                    int CreditCount = 0;
                    //�`��
                    decimal sum = 0;
                    //�`��ؼ�
                    int SbjCount = 0;
                    //�W�����`��
                    decimal last_sum = 0;
                    bool last_canRank = true;

                    foreach (StudentAttendCourseRecord attendRec in studentRec.AttendCourseList)
                    {
                        if (printSubjects.Contains(attendRec.Subject) && (attendRec.ExamList.Contains("�Ĥ@���q��") || attendRec.ExamList.Contains("�ĤG���q��") || attendRec.ExamList.Contains("������")))
                        {
                            //���ءB�ŧO�B�Ǥ��ư¦� "_���_�ŧO_�Ǥ���_"���r��A�o�Ӧr��b���P��دŧO�Ǥ��Ʒ|�����ߤ@��
                            //string key = attendRec.Subject + "^_^" + attendRec.SubjectLevel + "^_^" + attendRec.Credit;
                            // BMK ---- aaron 2013-01-24 key = ���
                            string key = attendRec.Subject ;
                            bool hasScore = false;
                            //5/29�C�L�����]�t�Юv����J�����
                            if (!groups.Contains(key))
                                groups.Add(key);
                            #region �ˬd�o��KEY���S�������P�ɭp���`�������άO�_�i�ƦW
                            foreach (ExamScoreInfo examScore in studentRec.ExamScoreList)
                            {
                                if ((examScore.ExamName == "�Ĥ@���q��" || examScore.ExamName == "�ĤG���q��" || examScore.ExamName == "������") && key == examScore.Subject )
                                {
                                    //�O�n�C�L�����
                                    //if ( !groups.Contains(key) )
                                    //    groups.Add(key);
                                    // BMK �@�륿�`���Z
                                    hasScore = true;
                                    if (examScore.SpecialCase == "")//�@�륿�`���Z
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
                                            if (classExamScoreTable[studentRec][key] != "��")
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
                                        //�[�v�`��
                                        scoreCount += examScore.ExamScore * examScore.Credit;
                                        //�Ǥ�
                                        CreditCount += examScore.Credit;
                                        //�`��
                                        sum += examScore.ExamScore;
                                        //�`��ؼ�
                                        SbjCount++;
                                    }
                                    else//�S���Z���p
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
                            //�o�{�S������
                            if (!hasScore)
                            {
                                #region �[�J�ǥͥ��������
                                if (!nonScoreKeys.ContainsKey(studentRec))
                                    nonScoreKeys.Add(studentRec, new List<string>());
                                if (!nonScoreKeys[studentRec].Contains(key))
                                    nonScoreKeys[studentRec].Add(key);
                                #endregion
                                classExamScoreTable[studentRec].Add(key, "����J");
                            }
                        }
                    }
                    sum = 0;
                    SbjCount = classExamScoreTable0[studentRec].Keys.Count;
                    foreach (string var in classExamScoreTable0[studentRec].Keys)
                    {
                        // 2013-01-24 aaron
                        // BMK ���L�D���n�D ��� = ��ئ��Z * (1/1.1/1.2) * �Ǥ���  , ���� (1/1.1/1.2) * �Ǥ���
                        // 2013-01-24 pm:5:00  ���L�D���n�D ��� = ��ئ��Z * (1/1.1/1.2) * �Ǥ���(�u����^��)  , ���� (1/1.1/1.2) * �Ǥ���(�u����^��)
                        // �Ĥ@������v�� 1
                        // �ĤG������v�� 1.1
                        // �ĤT������v�� 1.2
                        //classExamScoreTable[studentRec][var] = (decimal.Parse(classExamScoreTable0[studentRec][var]) / classExamScoreTime0[studentRec][var]).ToString();
                        //classExamScoreTable[studentRec][var] = (decimal.Parse(classExamScoreTable0[studentRec][var]) * subjectCredit[var]).ToString();
                        classExamScoreTable[studentRec][var] = (decimal.Parse(classExamScoreTable0[studentRec][var]) ).ToString();
                        sum += decimal.Parse(classExamScoreTable[studentRec][var]);
                    }
                    classExamScoreTable[studentRec].Add("�[�v�`��", scoreCount.ToString());
                    classExamScoreTable[studentRec].Add("�[�v����", (scoreCount / (CreditCount == 0 ? 1 : CreditCount)).ToString(".00"));
                    classExamScoreTable[studentRec].Add("����", (sum / (SbjCount == 0 ? 1 : SbjCount)).ToString(".00"));
                    classExamScoreTable[studentRec].Add("�`��", sum.ToString());
                    if (canRank)
                    {
                        canRankList.Add(studentRec, decimal.Parse((scoreCount / (CreditCount == 0 ? 1 : CreditCount)).ToString(".00")));
                        canRankList2.Add(studentRec, sum);
                    }
                    if (!classExamScoreTable[studentRec].ContainsKey("�`���ƦW"))
                        classExamScoreTable[studentRec].Add("�`���ƦW", "");
                }
                //�p�G�ǥͦb�n�C�L��ؤ��o�{���������ثh�q�i�ƦW�M�椤����
                #region �p�G�ǥͦb�n�C�L��ؤ��o�{���������ثh�q�i�ƦW�M�椤����
                //5/29�C�L�����]�t�Юv����J����ءA���N���q����
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
                //        classExamScoreTable[stuRec].Add("�[�v�����ƦW", "" + ( rankScoreList.IndexOf(decimal.Parse(classExamScoreTable[stuRec]["�[�v����"])) + 1 ));
                //    else
                //        classExamScoreTable[stuRec].Add("�[�v�����ƦW", "");

                //    if ( canRankList2.ContainsKey(stuRec) )
                //        classExamScoreTable[stuRec].Add("�`���ƦW", "" + ( rankScoreList2.IndexOf(decimal.Parse(classExamScoreTable[stuRec]["�`��"])) + 1 ));
                //    else
                //        classExamScoreTable[stuRec].Add("�`���ƦW", "");
                //    stuRec.Fields.Add("�`���ƦW", classExamScoreTable[stuRec]["�`���ƦW"]);
                //    if (classExamScoreTable[stuRec]["�`���ƦW"] != "")
                //    {
                //        //if (int.Parse(classExamScoreTable[stuRec]["�`���ƦW"]) <= 3)
                //        //{
                //            stuRec.Fields.Add("�`��", classExamScoreTable[stuRec]["�`��"]);
                //            if (!FirstStudentRec.ContainsKey(stuRec.RefClass.ClassName))
                //                FirstStudentRec.Add(stuRec.RefClass.ClassName, new List<StudentRecord>());
                //            FirstStudentRec[stuRec.RefClass.ClassName].Add(stuRec);
                //        //}
                //    }
                //}
                #endregion

                //�Ƨǭn�C�L�����
                //groups.Sort(StringComparer.Comparer);
                //FirstStudentRec[classRec.ClassName].Sort(�W���Ƨ�);
                foreach (string key in groups)
                {
                    //if (key != "���" && key != "�^��" && key != "�ƾ�")
                    if (!IsArrayContains(chiEngMathStrArray, key))
                    {
                        pro_groups.Add(key);
                    }
                }

                #endregion

                // BMK ���Excel�C�L
                #region ���Excel�C�L
                #region �إ߼˪O
                // �s�W�@��Sheet
                int newSheetIndex = template.Worksheets.AddCopy(0);
                //�o�@�ӷs�W��sheet
                Worksheet sheet1 = template.Worksheets[newSheetIndex];
                //�]�w�W��
                if (sheetName != "")
                    sheet1.Name = sheetName;
                #endregion

                for (int i = 0; i < otherList.Count; i++)
                {
                    if (otherList[i] == "�`���ƦW")
                    {
                        sheet1.Cells[1, groups.Count + 2 + i].PutValue("�Z�ƦW");
                        sheet1.Cells[1, groups.Count + 3 + i].PutValue("��ƦW");
                    }
                    else
                        sheet1.Cells[1, groups.Count + 2 + i].PutValue(otherList[i]);
                }
                int studentIndex = 0;
                #region ���^��
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
                #region ���Y
                sheet1.Cells[studentIndex, 0].PutValue(SmartSchool.Customization.Data.SystemInformation.SchoolYear + "�Ǧ~��" + ((SmartSchool.Customization.Data.SystemInformation.Semester == 1) ? "�W�Ǵ� " : "�U�Ǵ� ") + sheetName + " �Ĥ@�B�G����һP�����Ҧ��Z�ƦW");
                sheet1.Cells.CreateRange(studentIndex, 1, false).RowHeight = 20;
                sheet1.Cells.CreateRange(studentIndex, 0, 1, groups.Count + 6).Merge();
                sheet1.Cells[studentIndex, 0].Style.HorizontalAlignment = TextAlignmentType.Center;
                sheet1.Cells[studentIndex, 0].Style.Font.Size = 16;
                for (byte j = 0; j <= groups.Count + 5; j++)
                {
                    //sheet1.Cells.SetColumnWidth(j, 10);
                    //�U�x�s��e��u
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                }
                studentIndex++;

                //���D
                //sheet1.Cells[studentIndex, 0].PutValue("�Z��");
                sheet1.Cells[studentIndex, 0].PutValue("�Ǹ�");
                sheet1.Cells[studentIndex, 1].PutValue("�m�W");
                //sheet1.Cells[studentIndex, 3].PutValue("�`��");
                //sheet1.Cells[studentIndex, 4].PutValue("�Z�ƦW");
                //sheet1.Cells[studentIndex, 5].PutValue("��ƦW");
                //���
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
                    //�U�x�s��e��u
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    sheet1.Cells[studentIndex, j].Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                }
                studentIndex++;
                #endregion

                //�ǥ�
                for (int i = 0; i < classRec.Students.Count; i++)
                {
                    //�ƻs�C
                    sheet1.Cells.CopyRow(sheet1.Cells, 2, studentIndex);
                    //�ƻs�C��
                    sheet1.Cells.SetRowHeight(studentIndex, sheet1.Cells.GetRowHeight(2));
                    if (classRec.Students.Count > i)
                    {
                        #region �p�G�o�@�C���ǥ�
                        //�Ǹ�
                        sheet1.Cells[studentIndex, 0].PutValue(classRec.Students[i].StudentNumber);
                        //�m�W
                        sheet1.Cells[studentIndex, 1].PutValue(classRec.Students[i].StudentName);
                        int index = 2;
                        #region �U�즨�Z
                        foreach (string key in chiEngMathStrArray)
                        {
                            decimal score;
                            if (classExamScoreTable[classRec.Students[i]].ContainsKey(key))
                            {
                                decimal.TryParse(classExamScoreTable[classRec.Students[i]][key], out score);
                                // �|�ˤ��J��p�ƲĤG��
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
                                // �|�ˤ��J��p�ƲĤG��
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
                            #region �έp���
                            //decimal score;
                            int startindex = 3;
                            string Formula = "";
                            
                            //startindex += classcnt * 70;
                            if (classExamScoreTable[classRec.Students[i]].ContainsKey(key))
                            {
                                //decimal.TryParse(classExamScoreTable[classRec.Students[i]][key], out score);
                                //sheet.Cells[classIndex + studentIndex, index].PutValue(score);
                                int sbjcount = 0;
                                if (key == "�`��")
                                {
                                    //sheet1.Cells[studentIndex, index].R1C1Formula = "=SUM(C" + (studentIndex + 1) + ":" + column_str[groups.Count] + (studentIndex + 1) + ")";
                                    sbjcount = 0;
                                    Formula = "=SUM(";
                                    foreach (string subj in chiEngMathStrArray)
                                    {
                                        //string subj = key1.Substring(0, key1.IndexOf("^_^"));
                                        sbjcount++;
                                        if (subj == "���")
                                            Formula += column_str[sbjcount] + (studentIndex + 1);
                                        else if (subj == "�^��")
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
                                else if (key == "����")
                                {
                                    sbjcount = 0;
                                    indexAvg = index;
                                    //Formula = "=(SUM(";
                                    //foreach (string subj in chiEngMathStrArray)
                                    //{
                                    //    //string subj = key1.Substring(0, key1.IndexOf("^_^"));
                                    //    sbjcount++;
                                    //    if (subj == "���")
                                    //        Formula += column_str[sbjcount] + (studentIndex + 1);
                                    //    else if (subj == "�^��")
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
                                else if (key == "�`���ƦW")
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
                        //�M�Ÿɻ����C
                        for (int j = 0; j <= groups.Count + otherList.Count; j++)
                        {
                            sheet1.Cells[studentIndex, j].PutValue(null);
                        }
                    }
                    for (byte j = 0; j <= groups.Count + 5; j++)
                    {
                        //�U�x�s��e��u
                        sheet1.Cells[studentIndex, j].Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                        sheet1.Cells[studentIndex, j].Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                        sheet1.Cells[studentIndex, j].Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                        sheet1.Cells[studentIndex, j].Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    }
                    #region �C5�C�N�[�S����
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
                //    if (int.Parse(stuRec.Fields["�`���ƦW"].ToString()) > 0)
                //    {
                //        #region �p�G�o�@�C���ǥ�
                //        sheet1.Cells.CopyRow(sheet1.Cells, 2, studentIndex);
                //        sheet1.Cells.SetRowHeight(studentIndex, 15);
                //        sheet1.Cells[studentIndex, 0].PutValue(stuRec.RefClass.ClassName);
                //        //�Ǹ�
                //        sheet1.Cells[studentIndex, 1].PutValue(stuRec.StudentNumber);
                //        //�m�W
                //        sheet1.Cells[studentIndex, 2].PutValue(stuRec.StudentName);
                //        int index = 3;
                //        foreach (string key in otherList)
                //        {
                //            #region �έp���
                //            //decimal score;
                //            int startindex = 3;
                //            startindex += classcnt * 68;
                //            if (stuRec.Fields.ContainsKey(key))
                //            {
                //                //decimal.TryParse(classExamScoreTable[classRec.Students[i]][key], out score);
                //                //report.Worksheets[0].Cells[classIndex + studentIndex, index].PutValue(score);
                //                if (key == "�`��")
                //                {
                //                    sheet1.Cells[studentIndex, index].PutValue(stuRec.Fields["�`��"]);
                //                }
                //                else if (key == "�`���ƦW")
                //                {
                //                    sheet1.Cells[studentIndex, index].PutValue(stuRec.Fields["�`���ƦW"]);
                //                    sheet1.Cells[studentIndex, index].Style.Number = 2;
                //                }
                //            }
                //            //else
                //            //    sheet1.Cells[classIndex + studentIndex, index].PutValue("--");
                //            #endregion
                //            for (byte j = 0; j <= 5; j++)
                //            {
                //                //�U�x�s��e��u
                //                sheet1.Cells[studentIndex, j].Style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
                //                sheet1.Cells[studentIndex, j].Style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                //                sheet1.Cells[studentIndex, j].Style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                //                sheet1.Cells[studentIndex, j].Style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                //            }
                //            if (key != "����")
                //                index++;
                //        }
                //        #endregion
                //        studentIndex++;
                //    }
                //else
                //{
                //    //�M�Ÿɻ����C
                //    for (int j = 0; j < maxSubject + otherList.Count; j++)
                //    {
                //        sheet1.Cells[classIndex + studentIndex, j].PutValue(null);
                //    }
                //}
                #region �C5�C�N�[�S����
                //if (studentIndex > 2 && (studentIndex - 2) % 5 == 0)
                //{
                //    Aspose.Cells.Range eachFiveRow = sheet1.Cells.CreateRange(studentIndex, 0, 1, 5);
                //    if (comboBoxEx1.Text == "�ĤG���q��")
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
            //�NR��R�� 
            //sheet1.Cells.DeleteColumn(17);
            template.Worksheets.RemoveAt(0);
            e.Result = template;
        }

        void bkw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //�^����U�説�A�C
            SmartSchool.Customization.PlugIn.Global.SetStatusBarMessage("���(�t����)�[�v���Z���ͤ�...", e.ProgressPercentage);
        }

        void bkw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SmartSchool.Customization.PlugIn.Global.SetStatusBarMessage("���(�t����)�[�v���Z���ͧ���");
            Workbook book = (Workbook)e.Result;
            Common.SaveToFile("��Ǵ���Ҧ��Z-�[�v", book);
            //Document doc = (Document)e.Result;
            //#region �x�s�ö}���ɮ�

            //string reportName = "�Z�ŦҸզ��Z��";
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
            //    sd.Title = "�t�s�s��";
            //    sd.FileName = reportName + ".doc";
            //    sd.Filter = "Word�ɮ� (*.doc)|*.doc|�Ҧ��ɮ� (*.*)|*.*";
            //    if ( sd.ShowDialog() == DialogResult.OK )
            //    {
            //        try
            //        {
            //            doc.Save(sd.FileName, SaveFormat.AsposePdf);
            //        }
            //        catch
            //        {
            //            MessageBox.Show("���w���|�L�k�s���C", "�إ��ɮץ���", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            return;
            //        }
            //    }
            //} 
            //#endregion
        }

        private int �W���Ƨ�(StudentRecord c1, StudentRecord c2)
        {
            object o1 = "0", o2 = "0";
            decimal d1 = 0, d2 = 0;
            c1.Fields.TryGetValue("�`��", out o1);
            c2.Fields.TryGetValue("�`��", out o2);
            decimal.TryParse("" + o1, out d1);
            decimal.TryParse("" + o2, out d2);
            return d2.CompareTo(d1);
        }

        private int �i�B�ƦW�Ƨ�(StudentRecord c1, StudentRecord c2)
        {
            object o1 = "0", o2 = "0";
            decimal d1 = 0, d2 = 0;
            c1.Fields.TryGetValue("�i�B�ƦW", out o1);
            c2.Fields.TryGetValue("�i�B�ƦW", out o2);
            decimal.TryParse("" + o1, out d1);
            decimal.TryParse("" + o2, out d2);
            return d2.CompareTo(d1);
        }

        //
        // 2012-12-19 aaron modified 
        // �]�� ��^��  �ƾǴ���۵M�պټƾǥ�   �ƾǴ�����|�պټƾǤA
        // ��l��  chiEngMathStrArray
        //
        private void InitailChiEngMathStrArray(ClassRecord varClassRecord)
        {


            if (varClassRecord.GradeYear == "3")
            {
                if (varClassRecord.ClassName.IndexOf("��") >= 0
                        && (varClassRecord.ClassName.IndexOf("��") >= 0
                        || varClassRecord.ClassName.IndexOf("�A") >= 0))
                {
                    chiEngMathStrArray = new string[] { "���", "�^��", "�ƾǥ�" };
                }
                else if (varClassRecord.ClassName.IndexOf("��") >= 0
                        && (varClassRecord.ClassName.IndexOf("��") >= 0
                        || varClassRecord.ClassName.IndexOf("��") >= 0))
                {
                    chiEngMathStrArray = new string[] { "���", "�^��", "�ƾǤA" };
                }
                else
                {
                    chiEngMathStrArray = new string[] { "���", "�^��", "�ƾ�" };
                }
            }
            else
            {
                chiEngMathStrArray = new string[] { "���", "�^��", "�ƾ�" };
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

