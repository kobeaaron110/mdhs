using System;
using System.Collections.Generic;
using System.Text;
using FISCA.UDT;

namespace SHSchoolBus.Data
{
    public class BusStopDAO
    {
        //有任何校車基本資料被更動時，就會觸發此事件。
        public static event EventHandler ItemChanged;

        //可存取 UDT 的工具類別
        private static AccessHelper udtHelper = new AccessHelper();

        public static List<BusStop> SelectAll()
        {
            List<BusStop> routes = udtHelper.Select<BusStop>();
            return routes;
        }

        public static string Insert(BusStop route)
        {
            List<ActiveRecord> logs = new List<ActiveRecord>();
            logs.Add(route);
            List<string> newIDs = udtHelper.InsertValues(logs);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
            return newIDs[0];
        }

        public static string Update(BusStop route)
        {
            List<ActiveRecord> schs = new List<ActiveRecord>();
            schs.Add(route);
            udtHelper.UpdateValues(schs);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
            return route.UID;
        }

        public static void Delete(BusStop route)
        {
            List<ActiveRecord> schs = new List<ActiveRecord>();
            schs.Add(route);
            udtHelper.DeletedValues(schs);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
        }

        public static List<BusStop> SelectByBusNumber(string busnumber)
        {
            //BusStop result = null;
            List<BusStop> wills = udtHelper.Select<BusStop>("車號='" + busnumber + "'");
            //if (wills.Count > 0)
                //result = wills[0];

            return wills;
        }

        /// <summary>
        /// 取得符合條件的校車紀錄,並放到 Dictionary中，以 BusStop.UID當作key
        /// </summary>
        /// <param name="conditionClause"></param>
        /// <returns></returns>
        public static Dictionary<string, BusStop> SelectToDictionary(string conditionClause)
        {
            Dictionary<string, BusStop> result = new Dictionary<string, BusStop>();
            List<BusStop> busrec = udtHelper.Select<BusStop>(conditionClause);
            foreach (BusStop bus in busrec)
            {
                result.Add(bus.UID, bus);
            }
            return result;
        }
    }
}
