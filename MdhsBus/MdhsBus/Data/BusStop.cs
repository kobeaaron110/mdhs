using System;
using System.Collections.Generic;
using System.Text;
using FISCA.UDT;

namespace MdhsBus.Data
{
    [TableName("校車系統.校車站名")]
    public class BusStop : ActiveRecord
    {
        [Field(Field = "校車時段", Indexed = false)]
        public string BusTimeName { get; set; }

        [Field(Field = "代碼", Indexed = false)]
        public string BusStopID { get; set; }

        [Field(Field = "站名", Indexed = false)]
        public string BusStopName { get; set; }

        [Field(Field = "停車地址", Indexed = false)]
        public string BusStopAddr { get; set; }

        [Field(Field = "車費", Indexed = false)]
        public int BusMoney { get; set; }

        [Field(Field = "車號", Indexed = false)]
        public string BusNumber { get; set; }

        [Field(Field = "站序", Indexed = false)]
        public string BusStopNo { get; set; }

        [Field(Field = "放學上車地點", Indexed = false)]
        public string BusUpAddr { get; set; }

        [Field(Field = "到站時間", Indexed = false)]
        public string ComeTime { get; set; }
    }
}
