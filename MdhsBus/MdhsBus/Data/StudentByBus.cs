using System;
using System.Collections.Generic;
using System.Text;
using FISCA.UDT;

namespace MdhsBus.Data
{
    /// <summary>
    /// 一個學生搭乘校車就代表一筆登錄紀錄
    /// </summary>
    [TableName("校車系統.搭乘校車紀錄")]
    public class StudentByBus : ActiveRecord
    {
        [Field(Field = "校車時段", Indexed = false)]
        public string BusTimeName { get; set; }

        [Field(Field = "電腦代號", Indexed = false)]
        public string BusStopID { get; set; }

        [Field(Field = "搭車年度", Indexed = false)]
        public int SchoolYear { get; set; }

        [Field(Field = "期間名稱", Indexed = false)]
        public string BusRangeName { get; set; }

        [Field(Field = "搭車天數", Indexed = false)]
        public int DateCount { get; set; }

        [Field(Field = "車費", Indexed = false)]
        public int BusMoney { get; set; }

        [Field(Field = "是否繳費", Indexed = false)]
        public Boolean PayStatus { get; set; }

        [Field(Field = "繳費日期", Indexed = false)]
        public DateTime PayDate { get; set; }

        [Field(Field = "備註", Indexed = false)]
        public string comment { get; set; }

        [Field(Field = "班級ID", Indexed = false)]
        public string ClassID { get; set; }

        [Field(Field = "班級", Indexed = false)]
        public string ClassName { get; set; }

        [Field(Field = "學生編號", Indexed = false)]
        public string StudentID { get; set; }
    }
}
