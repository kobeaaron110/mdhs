using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;
using FISCA.Presentation;

namespace MdhsBus.Data
{
    public class StudentByBusDAO
    {
        //有任何學生搭校車資料被更動時，就會觸發此事件。
        public static event EventHandler ItemChanged;

        //可存取 UDT 的工具類別
        private static AccessHelper udtHelper = new AccessHelper();

        public static List<StudentByBus> SelectAll()
        {
            List<StudentByBus> routes = udtHelper.Select<StudentByBus>();
            return routes;
        }

        public static List<StudentByBus> GetSortByStudentNumberList()
        {
            List<StudentByBus> busstus = udtHelper.Select<StudentByBus>();
            busstus.Sort(CompareSchoolYear);
            return busstus;
        }

        public static string Insert(StudentByBus route)
        {
            List<ActiveRecord> logs = new List<ActiveRecord>();
            logs.Add(route);
            List<string> newIDs = udtHelper.InsertValues(logs);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
            return newIDs[0];
        }

        public static string Update(StudentByBus route)
        {
            List<ActiveRecord> schs = new List<ActiveRecord>();
            schs.Add(route);
            udtHelper.UpdateValues(schs);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
            return route.UID;
        }

        public static void Delete(StudentByBus route)
        {
            List<ActiveRecord> schs = new List<ActiveRecord>();
            schs.Add(route);
            udtHelper.DeletedValues(schs);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
        }

        public static List<StudentByBus> SelectByBusYear(int SchoolYear)
        {
            List<StudentByBus> wills = udtHelper.Select<StudentByBus>("搭車年度=" + SchoolYear + "");
            wills.Sort(CompareBusRange);

            return wills;
        }

        public static List<StudentByBus> SelectByBusYearAndTimeName(int SchoolYear, string busTimeName)
        {
            List<StudentByBus> wills = udtHelper.Select<StudentByBus>("搭車年度=" + SchoolYear + " and 期間名稱='" + busTimeName + "'");
            wills.Sort(CompareBusClass);

            return wills;
        }

        public static StudentByBus SelectByBusYearAndTimeNameAndStudntID(int SchoolYear, string busTimeName, string studentid)
        {
            List<StudentByBus> wills = udtHelper.Select<StudentByBus>("搭車年度=" + SchoolYear + " and 期間名稱='" + busTimeName + "' and 學生編號='" + studentid + "'");
            //wills.Sort(CompareBusClass);

            if (wills.Count > 0)
                return wills[0];
            else
                return null;
        }

        public static List<StudentByBus> SelectByBusYearAndTimeNameAndStudntList(int SchoolYear, string busTimeName, List<string> studentids)
        {
            List<StudentByBus> wills=new List<StudentByBus>();
            List<StudentByBus> selects = new List<StudentByBus>();
            int nowSet = 0;
            foreach (string var in studentids)
            {
                MotherForm.SetStatusBarMessage("正在讀取校車資料副程式", nowSet++ * 50 / studentids.Count);
                wills = udtHelper.Select<StudentByBus>("搭車年度=" + SchoolYear + " and 期間名稱='" + busTimeName + "' and 學生編號='" + var + "'");
                selects.AddRange(wills);
            }

            if (selects.Count > 0)
                return selects;
            else
                return null;
        }

        public static List<StudentByBus> SelectByBusYearAndTimeNameAndClass(int SchoolYear, string busTimeName, string className)
        {
            List<StudentByBus> wills = udtHelper.Select<StudentByBus>("搭車年度=" + SchoolYear + " and 期間名稱='" + busTimeName + "'" + " and 班級='" + className + "'");
            wills.Sort(CompareBusClass);

            return wills;
        }

        public static List<StudentByBus> SelectByBusYearAndTimeNameAndClassID(int SchoolYear, string busTimeName, string classID)
        {
            List<StudentByBus> wills = udtHelper.Select<StudentByBus>("搭車年度=" + SchoolYear + " and 期間名稱='" + busTimeName + "'" + " and 班級ID='" + classID + "'");
            wills.Sort(CompareBusClass);

            return wills;
        }

        public static List<StudentByBus> SelectByStudntID(string studentid)
        {
            List<StudentByBus> wills = udtHelper.Select<StudentByBus>("學生編號='" + studentid + "'");
            wills.Sort(CompareYearRange);

            return wills;
        }

        /// <summary>
        /// 取得符合條件的校車紀錄,並放到 Dictionary中，以 BusStop.UID當作key
        /// </summary>
        /// <param name="conditionClause"></param>
        /// <returns></returns>
        public static Dictionary<string, StudentByBus> SelectToDictionary(string conditionClause)
        {
            Dictionary<string, StudentByBus> result = new Dictionary<string, StudentByBus>();
            List<StudentByBus> busrec = udtHelper.Select<StudentByBus>(conditionClause);
            foreach (StudentByBus bus in busrec)
            {
                result.Add(bus.UID, bus);
            }
            return result;
        }

        //依搭車年度、期間名稱、班級、學生ID排序副程式
        static int CompareSchoolYear(StudentByBus a, StudentByBus b)
        {
            if (a.SchoolYear == b.SchoolYear)
            {
                if (a.BusRangeName == b.BusRangeName)
                {
                    if (a.ClassName == b.ClassName)
                    {
                        return a.StudentID.CompareTo(b.StudentID);
                    }
                    else
                        return a.ClassName.CompareTo(b.ClassName);
                }
                else
                    return a.BusRangeName.CompareTo(b.BusRangeName);
            }
            else
                return a.SchoolYear.CompareTo(b.SchoolYear);
        }

        //依期間名稱、班級、學生ID排序副程式
        static int CompareBusRange(StudentByBus a, StudentByBus b)
        {
            if (a.BusRangeName == b.BusRangeName)
            {
                if (a.ClassName == b.ClassName)
                {
                    return a.StudentID.CompareTo(b.StudentID);
                }
                else
                    return a.ClassName.CompareTo(b.ClassName);
            }
            else
                return a.BusRangeName.CompareTo(b.BusRangeName);
        }

        //依班級、學生ID排序副程式
        static int CompareBusClass(StudentByBus a, StudentByBus b)
        {
            if (a.ClassName == b.ClassName)
            {
                return a.StudentID.CompareTo(b.StudentID);
            }
            else
                return a.ClassName.CompareTo(b.ClassName);
        }

        //依搭車年度、期間名稱排序副程式
        static int CompareYearRange(StudentByBus a, StudentByBus b)
        {
            if (a.SchoolYear == b.SchoolYear)
            {
                return a.BusRangeName.CompareTo(b.BusRangeName);
            }
            else
                return a.SchoolYear.CompareTo(b.SchoolYear);
        }
    }
}
