using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace MdhsBus.Data
{
    /// <summary>
    /// 搭乘校車之時間就代表一筆登錄紀錄
    /// </summary>
    [TableName("校車系統.校車時間設定紀錄")]
    public class BusSetup : ActiveRecord
    {
        [Field(Field = "搭車年度", Indexed = false)]
        public int BusYear { get; set; }

        [Field(Field = "期間名稱", Indexed = false)]
        public string BusRangeName { get; set; }

        [Field(Field = "搭車起始日期", Indexed = false)]
        public DateTime BusStartDate { get; set; }

        [Field(Field = "搭車截止日期", Indexed = false)]
        public DateTime BusEndDate { get; set; }

        [Field(Field = "繳費項目", Indexed = false)]
        public string BusPaymentName { get; set; }

        [Field(Field = "繳費截止日", Indexed = false)]
        public DateTime PayEndDate { get; set; }

        [Field(Field = "搭車天數", Indexed = false)]
        public int DateCount { get; set; }

        [Field(Field = "校車時段", Indexed = false)]
        public string BusTimeName { get; set; }

        //[Field(Field = "是否為新生", Indexed = false)]
        //public bool BusByNewStudent { get; set; }
    }
}
