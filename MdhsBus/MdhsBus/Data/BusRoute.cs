using System;
using System.Collections.Generic;
using System.Text;
using FISCA.UDT;

namespace MdhsBus.Data
{
    [TableName("校車系統.校車路線")]
    public class BusRoute : ActiveRecord
    {
        [Field(Field = "路線名稱", Indexed = false)]
        public string BusRouteName { get; set; }

        [Field(Field = "車號", Indexed = false)]
        public string BusNumber { get; set; }

    }
}
