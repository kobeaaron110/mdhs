using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using FISCA.UDT;
using SmartSchool;
using FISCA.Permission;
using 定期評量成績單.UDT;
//using SmartSchool.Customization.Data;

namespace 定期評量成績單
{
    public partial class TeacherWord : FISCA.Presentation.DetailContent
    {
        private BackgroundWorker _Loader = new BackgroundWorker();
        private string _RunningKey = "";
        private List<UDTTeacherWords> _Source = new List<UDTTeacherWords>();
        private SmartSchool.Customization.Data.AccessHelper accessHelper = new SmartSchool.Customization.Data.AccessHelper();
        private FISCA.UDT.AccessHelper udtHelper = new FISCA.UDT.AccessHelper();
        public delegate void AddListItem();
        public AddListItem myDelegate;
        private List<SmartSchool.Customization.Data.ClassRecord> classRecords = new List<SmartSchool.Customization.Data.ClassRecord>();
        
        public TeacherWord()
        {
            InitializeComponent();

            this.Group = "導師的話";
            this.PrimaryKeyChanged += new EventHandler(TeacherWord_PrimaryKeyChanged);
            if (FISCA.Permission.UserAcl.Current["TeacherWord"].Editable)
            {
                //this.addbutton.Visible = true;
                //this.editbutton.Visible = true;
                //this.delbutton.Visible = true;
            }
            else if (FISCA.Permission.UserAcl.Current["TeacherWord"].Viewable)
            {
                //this.addbutton.Visible = false;
                //this.editbutton.Visible = false;
                //this.delbutton.Visible = false;
            }

            //--------
            _Loader.DoWork += new DoWorkEventHandler(_Loader_DoWork);
            _Loader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_Loader_RunWorkerCompleted);
            //NewStudent.Instance.ItemUpdated +=
            //然候按兩下tab
            //NewStudent.Instance.ItemUpdated += new EventHandler<Framework.ItemUpdatedEventArgs>(Instance_ItemUpdated);
            myDelegate = new AddListItem(changeText);
        }

        void _Loader_DoWork(object sender, DoWorkEventArgs e)
        {
            _Source = udtHelper.Select<UDTTeacherWords>("班級編號='" + PrimaryKey + "'");//where 誰='student_UID'
            //UDTTeacherWords teacherWordvar = new UDTTeacherWords();
            //string whereClause = string.Format("班級編號='{0}'", PrimaryKey);
            //_Source = UDTAccess.Select(whereClause);

            //this.myDelegate;
            this.Invoke(this.myDelegate);
        }

        void changeText()
        {
            if (_Source.Count > 0)
            {
                this.textBox1.Text = _Source[0].Heading;
                this.textBox2.Text = _Source[0].TeacherName;
                this.richTextBox1.Text = _Source[0].TeacherWord;
            }
            else
            {
                this.textBox1.Text = "";
                this.textBox2.Text = "";
                this.richTextBox1.Text = "";
            }
        }

        void _Loader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_RunningKey != PrimaryKey)
            {
                _RunningKey = PrimaryKey;
                _Loader.RunWorkerAsync();
            }
            else
            {
                //dataGridView1.DataSource = _Source;
                //if (!CurrentUser.Acl["TeacherWordDetail"].Editable)
                if (!UserAcl.Current["TeacherWordDetail"].Editable)
                {
                    //btnAdd.Visible = false;
                    //foreach (DataGridViewColumn col in dataGridView1.Columns)
                        //col.ReadOnly = true;
                    ShowButton(false);
                }

                this.Loading = false;
            }
        }

        void TeacherWord_PrimaryKeyChanged(object sender, EventArgs e)
        {
            OnPrimaryKeyChanged(new EventArgs());
        }

        private void ShowButton(bool isVisible)
        {
            this.SaveButtonVisible = isVisible;
            this.CancelButtonVisible = isVisible;

        }

        static private int TeacherWordSort(定期評量成績單.UDT.UDTTeacherWords healthy1, 定期評量成績單.UDT.UDTTeacherWords healthy2)
        {
            int d1 = 0, d2 = 0;
            int.TryParse("" + healthy1.UID, out d1);
            int.TryParse("" + healthy2.UID, out d2);
            return d1.CompareTo(d2);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TeacherWord_SaveButtonClick(object sender, EventArgs e)
        {
            //dataGridView1.EndEdit();
            //dataGridView1.CancelEdit();
            _Source.SaveAll();
            // 先清再加
            // delete all
            if (_Source.Count > 0)
            {
                foreach (var var_Source in _Source)
                {
                    UDTAccess.Delete(var_Source);
                }
            }
            // add one record
            UDTTeacherWords teacherWord = new UDTTeacherWords();
            teacherWord.ClassID = PrimaryKey;
            teacherWord.TeacherWord = this.richTextBox1.Text;
            teacherWord.Heading = this.textBox1.Text.Equals("") ? "親愛的孩子" : this.textBox1.Text;
            
            // 取班導師姓名
            classRecords = accessHelper.ClassHelper.GetClass(new string[] { PrimaryKey });
            if (this.textBox2.Text.Equals("") && classRecords.Count > 0)
            {
                teacherWord.TeacherName = classRecords[0].RefTeacher.TeacherName;
            }
            else
            {
                teacherWord.TeacherName = this.textBox2.Text;
            }
            
            UDTAccess.Insert(teacherWord);

            OnPrimaryKeyChanged(new EventArgs());
            ShowButton(false);
        }

        private void TeacherWord_CancelButtonClick(object sender, EventArgs e)
        {
            OnPrimaryKeyChanged(new EventArgs());
        }
       

        void Instance_ItemUpdated(object sender, Framework.ItemUpdatedEventArgs e)
        {
            if (e.PrimaryKeys.Contains(PrimaryKey))
                OnPrimaryKeyChanged(new EventArgs());
        }

        //妳先拉一個DataGridView在Log的毛毛蟲上
        //然後點這個DataGridView
        //右上角應該有個小鍵頭
        //點開那個小鍵頭有選單
        //把啟用編輯打勾其他不勾
        //最上面有個"選擇資料來源"
        //這個選"加入專案資料來源"
        //會跳出一個精靈
        //一開始先選"物件"
        //然候在選單中選到LogRecord
        //然後完成
        //小鍵頭的編輯資料行可修改欄位名稱及屬性
        /// <summary>
        /// 當主畫面檢視不同學生時，此毛毛蟲就被指定新的學生ID，就會呼叫此方法。
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPrimaryKeyChanged(EventArgs e)
        {
            this.Loading = true;    //畫面會呈現資料下載中的狀態(圓圈一直轉)
            ShowButton(false);

            //dataGridView1.EndEdit();
            //dataGridView1.CancelEdit();
            
            if (!_Loader.IsBusy)
            {
                _RunningKey = PrimaryKey;
                _Loader.RunWorkerAsync();
            }
            //base.OnPrimaryKeyChanged(e);
        }

        // 
        // text 有打字 則 顯示 "儲存" & "取消"
        //
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool hasChanged = false;
            this.ContentValidated = true;

            if (_Source.Count > 0)
            {
                if (!this.textBox1.Text.Equals(_Source[0].Heading))
                    hasChanged = true;
            }
            else
            {
                if (!this.textBox1.Text.Equals(""))
                    hasChanged = true;
            }

            ShowButton(hasChanged);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bool hasChanged = false;
            this.ContentValidated = true;

            if (_Source.Count > 0)
            {
                if (!this.textBox2.Text.Equals(_Source[0].TeacherName))
                    hasChanged = true;
            }else
            {
                if (!this.textBox2.Text.Equals(""))
                    hasChanged = true;
            }

            ShowButton(hasChanged);
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            bool hasChanged = false;
            this.ContentValidated = true;

            if (_Source.Count > 0)
            {
                if (!this.richTextBox1.Text.Equals(_Source[0].TeacherWord))
                    hasChanged = true;
            }else
            {
                if (!this.richTextBox1.Text.Equals(""))
                    hasChanged = true;
            }

            ShowButton(hasChanged);
        }
    }
}
