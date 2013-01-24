using System;
using System.Collections.Generic;
using System.Text;
using Aspose.Cells;
using System.IO;
using System.Threading;

namespace 考試成績單範例
{
    class Common
    {
        public  static void SaveToFile(string inputReportName, Workbook inputWorkbook)
        {
            string reportName = inputReportName;

            string path = Path.Combine(System.Windows.Forms.Application.StartupPath, "Reports");
            if ( !Directory.Exists(path) )
                Directory.CreateDirectory(path);
            path = Path.Combine(path, reportName + ".xls");

            Workbook wb = inputWorkbook;

            if ( File.Exists(path) )
            {
                int i = 1;
                while ( true )
                {
                    string newPath = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + ( i++ ) + Path.GetExtension(path);
                    if ( !File.Exists(newPath) )
                    {
                        path = newPath;
                        break;
                    }
                }
            }

            try
            {
                wb.Save(path, FileFormatType.Excel2003);
                System.Diagnostics.Process.Start(path);
            }
            catch
            {
                System.Windows.Forms.SaveFileDialog sd = new System.Windows.Forms.SaveFileDialog();
                sd.Title = "另存新檔";
                sd.FileName = reportName + ".xls";
                sd.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                if ( sd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    try
                    {
                        wb.Save(sd.FileName, FileFormatType.Excel2003);

                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        public static string GetNumberString(string p)
        {
            string levelNumber;
            switch ( p.Trim() )
            {
                #region 對應levelNumber
                case "1":
                    levelNumber = "Ⅰ";
                    break;
                case "2":
                    levelNumber = "Ⅱ";
                    break;
                case "3":
                    levelNumber = "Ⅲ";
                    break;
                case "4":
                    levelNumber = "Ⅳ";
                    break;
                case "5":
                    levelNumber = "Ⅴ";
                    break;
                case "6":
                    levelNumber = "Ⅵ";
                    break;
                case "7":
                    levelNumber = "Ⅶ";
                    break;
                case "8":
                    levelNumber = "Ⅷ";
                    break;
                case "9":
                    levelNumber = "Ⅸ";
                    break;
                case "10":
                    levelNumber = "Ⅹ";
                    break;
                default:
                    levelNumber = p;
                    break;
                #endregion
            }
            return levelNumber;
        }
    }

    public class StringComparer
    {
        private static string[] keys = new string[] { 
            "國文","英文","數學","物理","化學","生物","地理","歷史","公民",
        };

        public static int Comparer(string s1, string s2)
        {
            return Comparer(s1, s2, keys);
        }
        public static int Comparer(string s1, string s2, string[] keys)
        {
            if ( s1 == s2 ) return 0;
            if ( s1.Length == 0 ) return -1;
            if ( s2.Length == 0 ) return 1;
            int length = s1.Length > s2.Length ? s2.Length : s1.Length;
            string ls1 = "", ls2 = "";
            for ( int i = 0 ; i < length ; i++ )
            {
                //先用兩個字串罪的開頭比關鍵字
                foreach ( string key in keys )
                {
                    bool b1 = false, b2 = false;
                    b1 = s1.StartsWith(key);
                    b2 = s2.StartsWith(key);
                    if ( b1 && !b2 )
                        return -1;
                    if ( b2 && !b1 )
                        return 1;
                }
                //如果兩個字串第一個字相同就砍掉第一個再比一次
                if ( s1.Substring(0, 1) == s2.Substring(0, 1) )
                {
                    s1 = s1.Substring(1);
                    s2 = s2.Substring(1);
                }
                else
                {
                    return s1.Substring(0, 1).CompareTo(s2.Substring(0, 1));
                }
            }
            if ( string.IsNullOrEmpty(s1) ) return -1;
            if ( string.IsNullOrEmpty(s2) ) return 1;
            return s1.CompareTo(s2);
        }
    }

    public class MultiThreadWorker<T>
    {
        private int _PackageSize = 500;
        private int _MaxThreads = 2;

        private void doWork(object obj)
        {
            List<PackageWorkEventArgs<T>> packages = (List<PackageWorkEventArgs<T>>)obj;
            foreach ( PackageWorkEventArgs<T> package in packages )
            {
                try
                {
                    if ( PackageWorker != null )
                        PackageWorker.Invoke(this, package);
                }
                catch ( Exception ex )
                {
                    package.Exception = ex;
                    package.HasException = true;
                }
            }
        }

        public int PackageSize { get { return _PackageSize; } set { _PackageSize = value; } }
        public int MaxThreads { get { return _MaxThreads; } set { if ( value <= 0 )throw new Exception("最好是可以小魚0啦"); _MaxThreads = value; } }

        public event EventHandler<PackageWorkEventArgs<T>> PackageWorker;
        public List<PackageWorkEventArgs<T>> Run(IEnumerable<T> list, object argument)
        {
            #region 切封包執行
            List<PackageWorkEventArgs<T>>[] packages = new List<PackageWorkEventArgs<T>>[_MaxThreads];
            for ( int i = 0 ; i < packages.Length ; i++ )
            {
                packages[i] = new List<PackageWorkEventArgs<T>>();
            }
            List<T> package = null;
            int packagecount = 0;
            int p = 0;
            foreach ( T var in list )
            {
                if ( packagecount == 0 )
                {
                    package = new List<T>(_PackageSize);
                    packagecount = _PackageSize;
                    PackageWorkEventArgs<T> pw = new PackageWorkEventArgs<T>();
                    pw.List = package;
                    pw.Argument = argument;
                    packages[p % _MaxThreads].Add(pw);
                    p++;
                }
                package.Add(var);
                packagecount--;
            }
            #region 開多個執行緒跑
            List<Thread> otherThreads = new List<Thread>();
            for ( int i = 1 ; i < _MaxThreads ; i++ )
            {
                if ( packages[i].Count > 0 )
                {
                    Thread backThread = new Thread(new ParameterizedThreadStart(doWork));
                    backThread.IsBackground = true;
                    backThread.Start(packages[i]);
                    otherThreads.Add(backThread);
                }
            }
            if ( packages[0].Count > 0 )
                doWork(packages[0]);
            foreach ( Thread thread in otherThreads )
            {
                thread.Join();
            }
            #endregion
            List<PackageWorkEventArgs<T>> result = new List<PackageWorkEventArgs<T>>();
            foreach ( List<PackageWorkEventArgs<T>> var in packages )
            {
                result.AddRange(var);
            }
            return result;
            #endregion
        }
        public List<PackageWorkEventArgs<T>> Run(IEnumerable<T> list)
        {
            return Run(list, null);
        }
    }

    public class PackageWorkEventArgs<T> : EventArgs
    {
        private bool _HasException = false;
        private Exception _Exception = null;
        private List<T> _List = new List<T>();
        private object _Result = null;
        private object _Argument = null;

        public bool HasException { get { return _HasException; } set { _HasException = value; } }
        public Exception Exception { get { return _Exception; } set { _Exception = value; } }
        public List<T> List { get { return _List; } set { _List = value; } }
        public object Result { get { return _Result; } set { _Result = value; } }
        public object Argument { get { return _Argument; } set { _Argument = value; } }

    }
}
