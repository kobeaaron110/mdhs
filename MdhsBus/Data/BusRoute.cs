using System;
using System.Collections.Generic;
using System.Text;
using FISCA.UDT;

namespace SHSchoolBus.Data
{
    [TableName("校車系統.校車路線")]
    public class BusRoute : ActiveRecord
    {
        [Field(Field = "路線名稱", Indexed = false)]
        public string BusName { get; set; }

        //[Field(Field = "站名", Indexed = false)]
        //public string Name { get; set; }
    }
}
