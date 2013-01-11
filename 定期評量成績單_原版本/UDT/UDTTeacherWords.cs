using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FISCA.UDT;

namespace 定期評量成績單.UDT
{
    
       
    /// <summary>
    /// 親愛的孩子:導師的話 , 導師筆
    /// </summary>
    [TableName("導師的話")]
    public class UDTTeacherWords : ActiveRecord
    {
        public UDTTeacherWords()
        {

            //KeyInDate = "";
            //KeyInTime = "";
            //Heading = "Heading";
            //TeacherWord = "TeacherWord";
            //TeacherName = "TeacherName";
            //Exam = "第一次段考";
            //Heading = "親愛的孩子";
            //TeacherWord = "aaaaaaaaa\naaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            //TeacherName = "蔡志鴻";
            //ClassID = "276";
        }

        /// <summary>
        /// 班級編號
        /// </summary>
        [Field(Field = "班級編號", Indexed = false)]
        public string ClassID { get; set; }

        /// <summary>
        /// 考試名稱 : 第一次段考=1, 第二次段考=2, 第三次段考=3, 期末考=4
        /// </summary>
        //[Field(Field = "考試名稱", Indexed = false)]
        public string Exam { get; set; }

        

        /// <summary>
        /// 標題
        /// </summary>
        [Field(Field = "標題", Indexed = false)]
        public string Heading { get; set; }

        /// <summary>
        /// 導師的話
        /// </summary>
        [Field(Field = "導師的話", Indexed = false)]
        public string TeacherWord { get; set; }

        /// <summary>
        /// 導師名子
        /// </summary>
        [Field(Field = "導師名子", Indexed = false)]
        public string TeacherName { get; set; }

        


        /// <summary>
        /// 在儲存前，把資料填入儲存欄位中
        /// </summary>
        public void Encode()
        {
                
        }
        /// <summary>
        /// 在資料取出後，把資料從儲存欄位轉換至資料欄位
        /// </summary>
        public void Decode()
        {
                
        }

    }
    
}
