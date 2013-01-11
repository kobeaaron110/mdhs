using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace 定期評量成績單.UDT
{
    public class UDTAccess
    {
        //宣告一個事件。有任何 UDTAccess 基本資料被更動時，就會觸發此事件。
        public static event EventHandler ItemChanged;

        //Helper class to invoke UDT services.
        private static AccessHelper udtHelper = new AccessHelper();

        /// <summary>
        /// 取得所有 UDTAccess 資料,並且放在一個List的集合中
        /// </summary>
        /// <returns></returns>
        public static List<UDTTeacherWords> SelectAll()
        {
            return udtHelper.Select<UDTTeacherWords>();
        }

        /// <summary>
        /// 取得所有 UDTAccess 資料,並且放在一個Dictionary的集合中, 而且以 UDTTeacherWords.UID 當作Key
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, UDTTeacherWords> SelectAllToDictionary()
        {
            Dictionary<string, UDTTeacherWords> result = new Dictionary<string, UDTTeacherWords>();
            List<UDTTeacherWords> healthys = UDTAccess.SelectAll();
            foreach (UDTTeacherWords healthy in healthys)
            {
                result.Add(healthy.UID, healthy);
            }
            return result;
        }


        /// <summary>
        /// 取得符合條件的 UDTAccess 紀錄,並放到 List中。
        /// </summary>
        /// <param name="conditionClause"></param>
        /// <returns></returns>
        public static List<UDTTeacherWords> Select(string conditionClause)
        {
            return udtHelper.Select<UDTTeacherWords>(conditionClause);
        }

        /// <summary>
        /// 取得符合條件的 UDTAccess 紀錄,並放到 Dictionary中，以 ClubRecord.UID當作key
        /// </summary>
        /// <param name="conditionClause"></param>
        /// <returns></returns>
        public static Dictionary<string, UDTTeacherWords> SelectToDictionary(string conditionClause)
        {
            Dictionary<string, UDTTeacherWords> result = new Dictionary<string, UDTTeacherWords>();
            List<UDTTeacherWords> healthys = Select(conditionClause);
            foreach (UDTTeacherWords healthy in healthys)
            {
                result.Add(healthy.UID, healthy);
            }
            return result;
        }

        /// <summary>
        /// 修改一筆 UDTAccess 紀錄
        /// </summary>
        /// <param name="club"></param>
        public static void Update(UDTTeacherWords healthy)
        {
            List<ActiveRecord> healthys = new List<ActiveRecord>();
            healthys.Add(healthy);

            udtHelper.UpdateValues(healthys);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// 刪除一筆 UDTAccess 紀錄
        /// </summary>
        /// <param name="club"></param>
        public static void Delete(UDTTeacherWords healthy)
        {
            List<ActiveRecord> healthys = new List<ActiveRecord>();
            healthys.Add(healthy);

            udtHelper.DeletedValues(healthys);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// 新增一筆 UDTAccess 紀錄
        /// </summary>
        /// <param name="club"></param>
        public static void Insert(UDT.UDTTeacherWords healthy)
        {
            List<ActiveRecord> healthys = new List<ActiveRecord>();
            healthys.Add(healthy);

            udtHelper.InsertValues(healthys);
            if (ItemChanged != null)
                ItemChanged(null, EventArgs.Empty);
        }
    }
}
