using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation;

namespace MdhsBus
{
    public partial class StudentBusBView : NavView
    {
        public StudentBusBView()
        {
            InitializeComponent();

            this.NavText = "依搭乘校車檢視";
            //註冊當NLDPanel指定給這個View的資料內容發生改變時,應當呼叫的副程式。
            this.SourceChanged += new EventHandler(StudentBusBView_SourceChanged);
            MdhsBus.Data.StudentByBusDAO.ItemChanged += new EventHandler(StudentByBusDAO_ItemChanged);
            //HealthyCenter.Data.HealthyWill.ItemChanged += new EventHandler(HealthyWill_ItemChanged);
            this.StudntBusTView.NodeMouseClick += new TreeNodeMouseClickEventHandler(StudntBusTView_NodeMouseClick);
        }

        void StudntBusTView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        void StudentByBusDAO_ItemChanged(object sender, EventArgs e)
        {
            this.RedrawTree();
        }

        void StudentBusBView_SourceChanged(object sender, EventArgs e)
        {
            this.RedrawTree();
        }

        private void RedrawTree()
        {
            
        }
    }
}
