using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.DSAUtil;
using System.Xml;
using K12.Data;

namespace 班級學生電子報表
{
    public partial class DataGridViewForm : Form
    {
        List<string> classIDList { get; set; }
        public DataGridViewForm()
        {
            InitializeComponent();
        }

        private void DataGridViewForm_Load(object sender, EventArgs e)
        {
            classIDList = K12.Presentation.NLDPanels.Class.SelectedSource;
            ReLoad();
        }

        private void ReLoad()
        {
            dataGridView1.Rows.Clear();

            // ----------- 載入 Class ---------
            foreach (string each in classIDList)
            {
                //傳入 class 類型 / classID 系統編號
                //取得該班級的所有電子報表
                DSXmlHelper helper = QueryElectronicPaper.GetPaperItemByViewer("Class", each).GetContent();

                foreach (XmlElement paper in helper.GetElements("PaperItem"))
                {
                    DSXmlHelper paperHelper = new DSXmlHelper(paper);
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);
                    //儲存電子報表編號
                    row.Cells[0].Value = paperHelper.GetText("@ID");
                    //班級名稱
                    ClassRecord cr = K12.Data.Class.SelectByID(each);
                    row.Cells[1].Value = cr.Name;
                    //電子報表名稱
                    row.Cells[2].Value = paperHelper.GetText("PaperName");
                    //製表日期
                    row.Cells[3].Value = paperHelper.GetText("Timestamp");
                    dataGridView1.Rows.Add(row);
                }
            }

            // ----------- 載入 Student ---------
            foreach (string each in classIDList)
            {
                //傳入 Student 類型 / StudentID 系統編號
                //取得該班級的所有電子報表
                //班級名稱下 所有學生
                List<StudentRecord> srList = K12.Data.Student.SelectByClassID(each);
                foreach (StudentRecord eachSr in srList)
                {
                    DSXmlHelper helper = QueryElectronicPaper.GetPaperItemByViewer("Student", eachSr.ID).GetContent();
                    foreach (XmlElement paper in helper.GetElements("PaperItem"))
                    {
                        DSXmlHelper paperHelper = new DSXmlHelper(paper);
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataGridView1);
                        //儲存電子報表編號
                        row.Cells[0].Value = paperHelper.GetText("@ID");
                        row.Cells[1].Value = eachSr.Name;
                        //電子報表名稱
                        row.Cells[2].Value = paperHelper.GetText("PaperName");
                        //製表日期
                        row.Cells[3].Value = paperHelper.GetText("Timestamp");
                        dataGridView1.Rows.Add(row);
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("您確定要刪除電子報表?", "注意", MessageBoxButtons.YesNo);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    string id = "" + row.Cells[0].Value;
                    EditElectronicPaper.DeletePaperItem(id);
                }

                MessageBox.Show("已刪除成功!!");
                ReLoad();
            }
            else
            {
                MessageBox.Show("中止操作!!");
            }
        }
    }
}
