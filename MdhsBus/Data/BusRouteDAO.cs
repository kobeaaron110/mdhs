using System;
using System.Collections.Generic;
using System.Text;
using FISCA.UDT;

namespace SHSchoolBus.Data
{
    public class BusRouteDAO
    {
        //可存取 UDT 的工具類別
        private static AccessHelper udtHelper = new AccessHelper();

        public static List<BusRoute> GetAll()
        {
            List<BusRoute> routes = udtHelper.Select<BusRoute>();
            return routes;
        }

        public static string Insert(BusRoute route)
        {
            List<ActiveRecord> logs = new List<ActiveRecord>();
            logs.Add(route);
            List<string> newIDs = udtHelper.InsertValues(logs);
            return newIDs[0];
        }

        public static string Update(BusRoute route)
        {
            List<ActiveRecord> schs = new List<ActiveRecord>();
            schs.Add(route);
            udtHelper.UpdateValues(schs);
            return route.UID;
        }

        public static void Delete(BusRoute route)
        {
            List<ActiveRecord> schs = new List<ActiveRecord>();
            schs.Add(route);
            udtHelper.DeletedValues(schs);
        }
    }
}
