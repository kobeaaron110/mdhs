using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace MdhsBus.Data
{
    public class BusSetupDAO
    {
        //有任何校車基本資料被更動時，就會觸發此事件。
        public static event EventHandler ItemChanged;

        //可存取 UDT 的工具類別
        private static AccessHelper udtHelper = new AccessHelper();

        public static List<BusSetup> SelectAll()
        {
            List<BusSetup> routes = udtHelper.Select<BusSetup>();
            return routes;
        }

        public static List<BusSetup> GetSortByBusStartDateList()
        {
            List<BusSetup> busSetups = udtHelper.Select<BusSetup>();
            busSetups.Sort(CompareBusTimeStartDate);
            return busSetups;
        }

        public static string Insert(BusSetup route)
        {
            List<ActiveRecord> logs = new List<ActiveRecord>();
            logs.Add(route);
            List<string> newIDs = udtHelper.InsertValues(logs);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
            return newIDs[0];
        }

        public static string Update(BusSetup route)
        {
            List<ActiveRecord> schs = new List<ActiveRecord>();
            schs.Add(route);
            udtHelper.UpdateValues(schs);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
            return route.UID;
        }

        public static void Delete(BusSetup route)
        {
            List<ActiveRecord> schs = new List<ActiveRecord>();
            schs.Add(route);
            udtHelper.DeletedValues(schs);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
        }

        public static List<BusSetup> SelectByBusYear(int busyear)
        {
            List<BusSetup> wills = udtHelper.Select<BusSetup>("搭車年度=" + busyear + "");
            wills.Sort(CompareBusTimeStartDate);

            return wills;
        }

        public static List<BusSetup> SelectByBusYearNewStudent(int busyear)
        {
            List<BusSetup> wills = udtHelper.Select<BusSetup>("搭車年度=" + busyear + " and 是否為新生=true");
            wills.Sort(CompareBusTimeStartDate);

            return wills;
        }

        public static List<BusSetup> SelectByBusYearAndTime(int busyear, string bustime)
        {
            List<BusSetup> wills = udtHelper.Select<BusSetup>("搭車年度=" + busyear + " and 校車時段='" + bustime + "'");
            wills.Sort(CompareBusStartDate);

            return wills;
        }

        public static BusSetup SelectByBusYearAndRange(int busyear, string busrange)
        {
            List<BusSetup> wills = udtHelper.Select<BusSetup>("搭車年度=" + busyear + " and 期間名稱='" + busrange + "'");
            //wills.Sort(CompareBusStartDate);

            return wills[0];
        }

        /// <summary>
        /// 取得符合條件的校車紀錄,並放到 Dictionary中，以 BusSetup.UID當作key
        /// </summary>
        /// <param name="conditionClause"></param>
        /// <returns></returns>
        public static Dictionary<string, BusSetup> SelectToDictionary(string conditionClause)
        {
            Dictionary<string, BusSetup> result = new Dictionary<string, BusSetup>();
            List<BusSetup> busrec = udtHelper.Select<BusSetup>(conditionClause);
            foreach (BusSetup bus in busrec)
            {
                result.Add(bus.UID, bus);
            }
            return result;
        }

        //依校車時段、搭車起始日期排序副程式
        static int CompareBusTimeStartDate(BusSetup a, BusSetup b)
        {
            if (a.BusTimeName == b.BusTimeName)
                return a.BusStartDate.CompareTo(b.BusStartDate);
            else
                return a.BusTimeName.CompareTo(b.BusTimeName);
        }

        //依搭車起始日期排序副程式
        static int CompareBusStartDate(BusSetup a, BusSetup b)
        {
            return a.BusStartDate.CompareTo(b.BusStartDate);
        }
    }
}
