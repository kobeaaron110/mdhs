using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using FISCA.UDT;

namespace MdhsBus
{
    [TableName("新生")]
    public class NewStudentRecord : ActiveRecord, IComparable<NewStudentRecord>
    {
        [Field(Field = "學年度", Indexed = false)]
        public string SchoolYear { get; set; }

        [Field(Field = "編號", Indexed = true)]
        public string Number { get; set; }

        [Field(Field = "報到序號", Indexed = true)]
        public string CheckNumber { get; set; }

        [Field(Field = "姓名", Indexed = false)]
        public string Name { get; set; }

        [Field(Field = "生日", Indexed = false)]
        public DateTime Birthday { get; set; }

        [Field(Field = "身份證字號", Indexed = true)]
        public string IDNumber { get; set; }

        [Field(Field = "性別", Indexed = false)]
        public string Gender { get; set; }

        [Field(Field = "身份別", Indexed = false)]
        public string StuCategory { get; set; }

        [Field(Field = "畢業國中縣市", Indexed = false)]
        public string JHSchoolCity { get; set; }

        [Field(Field = "畢業國中", Indexed = false)]
        public string JHSchool { get; set; }

        [Field(Field = "畢業年月", Indexed = false)]
        public string GraduateDate { get; set; }

        [Field(Field = "畢業狀況", Indexed = false)]
        public string GraduateStatus { get; set; }

        [Field(Field = "畢業證書繳交", Indexed = false)]
        public Boolean IsDiploma { get; set; }

        [Field(Field = "家長姓名", Indexed = false)]
        public string Parent { get; set; }

        [Field(Field = "郵遞區號", Indexed = false)]
        public string ZipCode { get; set; }

        [Field(Field = "縣市", Indexed = false)]
        public string City { get; set; }

        [Field(Field = "鄉鎮市區", Indexed = false)]
        public string Area { get; set; }

        [Field(Field = "村里路鄰號", Indexed = false)]
        public string Street { get; set; }

        [Field(Field = "電話", Indexed = false)]
        public string Telephone { get; set; }

        [Field(Field = "手機", Indexed = false)]
        public string Cellphone { get; set; }

        [Field(Field = "科別", Indexed = false)]
        public string Dept { get; set; }

        [Field(Field = "學號", Indexed = true)]
        public string StuNumber { get; set; }

        [Field(Field = "班級", Indexed = true)]
        public string ClassName { get; set; }

        [Field(Field = "報名日期", Indexed = false)]
        public DateTime PreserveDate { get; set; }

        [Field(Field = "取回日期", Indexed = false)]
        public DateTime? WithDrawDate { get; set; }

        [Field(Field = "報名輔導老師", Indexed = false)]
        public string TeacherRegistration { get; set; }

        [Field(Field = "報名輔導老師1", Indexed = false)]
        public string TeacherRegistration1 { get; set; }

        [Field(Field = "報名輔導老師2", Indexed = false)]
        public string TeacherRegistration2 { get; set; }

        [Field(Field = "入學方式", Indexed = false)]
        public string EntMode { get; set; }

        [Field(Field = "入學成績", Indexed = false)]
        public decimal EntScore { get; set; }

        [Field(Field = "國文成績", Indexed = false)]
        public decimal ChiScore { get; set; }

        [Field(Field = "數學成績", Indexed = false)]
        public decimal MathScore { get; set; }

        [Field(Field = "英語成績", Indexed = false)]
        public decimal EngScore { get; set; }

        [Field(Field = "社會成績", Indexed = false)]
        public decimal SocScore { get; set; }

        [Field(Field = "自然成績", Indexed = false)]
        public decimal SciScore { get; set; }

        [Field(Field = "寫作成績", Indexed = false)]
        public decimal WriteScore { get; set; }

        [Field(Field = "PR值", Indexed = false)]
        public decimal PRValue { get; set; }

        [Field(Field = "國保", Indexed = false)]
        public bool Promise { get; set; }

        [Field(Field = "菁英", Indexed = false)]
        public bool Excellent { get; set; }

        [Field(Field = "報到", Indexed = false)]
        public bool CheckIn { get; set; }

        [Field(Field = "預繳費用", Indexed = false)]
        public string ApplyWorkStudy { get; set; }

        [Field(Field = "註冊", Indexed = false)]
        public bool Registered { get; set; }

        [Field(Field = "點數", Indexed = false)]
        public decimal Point { get; set; }

        [Field(Field = "學生狀態", Indexed = false)]
        public bool Active { get; set; }

        #region IComparable<NewStudentRecord> 成員

        public int CompareTo(NewStudentRecord other)
        {
            int a = int.MinValue;
            int b = int.MinValue;
            int.TryParse(this.Number, out a);
            int.TryParse(other.Number, out b);
            return a.CompareTo(b);
        }

        #endregion
    }
}
