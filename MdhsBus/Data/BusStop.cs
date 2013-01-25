using System;
using System.Collections.Generic;
using System.Text;
using FISCA.UDT;

namespace SHSchoolBus.Data
{
    [TableName("校車系統.校車站名")]
    public class BusStop : ActiveRecord
    {
        [Field(Field = "代碼", Indexed = false)]
        public string BusStopID { get; set; }

        [Field(Field = "站名", Indexed = false)]
        public string BusStopName { get; set; }

        [Field(Field = "停車地點", Indexed = false)]
        public string BusStopAddr { get; set; }

        [Field(Field = "金額", Indexed = false)]
        public string BusMoney { get; set; }

        [Field(Field = "車號", Indexed = false)]
        public string BusNumber { get; set; }

        [Field(Field = "站序", Indexed = false)]
        public int BusStopNo { get; set; }
    }
}
