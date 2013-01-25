using System;
using System.Collections.Generic;
using System.Text;
using FISCA.UDT;
using System.Windows.Forms;

namespace MdhsBus.Data
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

        public static List<BusStop> GetSortByBusNumberList()
        {
            List<BusStop> busstops = udtHelper.Select<BusStop>();
            busstops.Sort(CompareBusNumber);
            return busstops;
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
            wills.Sort(CompareBusNumber);
            //if (wills.Count > 0)
                //result = wills[0];

            return wills;
        }

        public static List<BusStop> SelectByBusTimeName(string busTimeName)
        {
            List<BusStop> wills = udtHelper.Select<BusStop>("校車時段='" + busTimeName + "'");
            wills.Sort(CompareBusNumber);

            return wills;
        }

        public static List<BusStop> SelectByBusTimeNameOrderByStopID(string busTimeName)
        {
            List<BusStop> wills = udtHelper.Select<BusStop>("校車時段='" + busTimeName + "'");
            wills.Sort(CompareBusStopID);
            //wills.Reverse();
            return wills;
        }

        public static List<BusStop> SelectByBusTimeNameAndNumber(string busTimeName, string busnumber)
        {
            List<BusStop> wills = udtHelper.Select<BusStop>("校車時段='" + busTimeName + "' and 車號='" + busnumber + "'");
            wills.Sort(CompareBusNumber);

            return wills;
        }

        public static List<BusStop> SelectByBusTimeNameAndNumberOrderByStopNO(string busTimeName, string busnumber)
        {
            List<BusStop> wills = udtHelper.Select<BusStop>("校車時段='" + busTimeName + "' and 車號='" + busnumber + "'");
            wills.Sort(CompareBusStopNo);
            //wills.Reverse();
            return wills;
        }

        public static BusStop SelectByBusTimeNameAndByStopID(string busTimeName, string busstopid)
        {
            BusStop nulls = new BusStop();
            List<BusStop> wills = udtHelper.Select<BusStop>("校車時段='" + busTimeName + "' and 代碼='" + busstopid + "'");
            if (wills.Count == 0)
                return nulls;
            else
                return wills[0];
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

        //依時段、車號站序排序副程式
        static int CompareBusNumber(BusStop a, BusStop b)
        {
            if (a.BusTimeName == b.BusTimeName)
            {
                if (a.BusNumber == b.BusNumber)
                    return a.BusStopNo.CompareTo(b.BusStopNo);
                else
                    return a.BusNumber.CompareTo(b.BusNumber);
            }
            else
                return a.BusTimeName.CompareTo(b.BusTimeName);
        }

        //依站序排序副程式
        static int CompareBusStopNo(BusStop a, BusStop b)
        {
            return a.BusStopNo.CompareTo(b.BusStopNo);
        }

        //依電腦代號排序副程式
        static int CompareBusStopID(BusStop a, BusStop b)
        {
            return a.BusStopID.CompareTo(b.BusStopID);
        }
    }
}
