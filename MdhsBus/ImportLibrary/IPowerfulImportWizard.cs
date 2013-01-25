using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace ImportLibrary
{
    /// <summary>
    /// 比以前的匯入流程再利害一點點的匯入流程
    /// </summary>
    public interface IPowerfulImportWizard
    {
        /// <summary>
        /// 開啟匯入檔案
        /// </summary>
        event EventHandler<LoadSourceEventArgs> LoadSource;

        /// <summary>
        /// 點擊Help按鈕
        /// </summary>
        event EventHandler HelpButtonClick;

        /// <summary>
        /// 控制面版開啟
        /// </summary>
        event EventHandler ControlPanelOpen;

        /// <summary>
        /// 控制面版關閉
        /// </summary>
        event EventHandler ControlPanelClose;

        /// <summary>
        /// 載入資料表內容
        /// </summary>
        event EventHandler<RowDataLoadEventArgs> RowsLoad;

        /// <summary>
        /// 識別資料
        /// </summary>
        event EventHandler<IdentifyRowEventArgs> IdentifyRow;

        /// <summary>
        /// 開始驗證
        /// </summary>
        event System.EventHandler<ValidateStartEventArgs> ValidateStart;

        /// <summary>
        /// 檢驗資料欄位
        /// </summary>
        event System.EventHandler<ValidateRowEventArgs> ValidateRow;

        /// <summary>
        /// 驗證結束
        /// </summary>
        event System.EventHandler ValidateComplete;

        /// <summary>
        /// 開始匯入
        /// </summary>
        event System.EventHandler ImportStart;

        /// <summary>
        /// 進行資料匯入
        /// </summary>
        event System.EventHandler<ImportPackageEventArgs> ImportPackage;

        /// <summary>
        /// 結束匯入
        /// </summary>
        event System.EventHandler ImportComplete;

        /// <summary>
        /// 必需的欄位(通常為匯入資料的索引欄位)
        /// </summary>
        FieldsCollection RequiredFields
        {
            get;
        }

        /// <summary>
        /// 允許匯入的欄位
        /// </summary>
        FieldsCollection ImportableFields
        {
            get;
        }

        /// <summary>
        /// 始用者選取要匯入的欄位
        /// </summary>
        FieldsCollection SelectedFields
        {
            get;
        }

        /// <summary>
        /// 精靈視窗的高度
        /// </summary>
        int Height
        {
            get;
            set;
        }

        /// <summary>
        /// 精靈視窗的寬度
        /// </summary>
        int Width
        {
            get;
            set;
        }

        /// <summary>
        /// 分割封包的期望值
        /// </summary>
        /// <remarks>
        /// 系統將會自動依此期望值做分割資料進行匯入，
        /// 分割時會將屬於同一人(班、課程)之資料分至同一Package，
        /// 所以若期望值為300而匯入資料中有500比資料屬於同一人時，
        /// 則將有一封包會超過500筆資料。
        /// </remarks>
        int PackageLimit
        {
            get;
            set;
        }

        /// <summary>
        /// 顯示Help按鈕
        /// </summary>
        bool HelpButtonVisible
        {
            get;
            set;
        }

        /// <summary>
        /// 設定項目
        /// </summary>
        OptionCollection Options
        {
            get;
        }

        /// <summary>
        /// 新增警告
        /// </summary>
        /// <param name="rowData">警告的資料行</param>
        /// <param name="message">警告內容</param>
        void AddWarning(RowData rowData, string message);

        /// <summary>
        /// 新增錯誤
        /// </summary>
        /// <param name="rowData">錯誤的資料行</param>
        /// <param name="message">錯誤內容</param>
        void AddError(RowData rowData, string message);
    }

    public class LoadSourceEventArgs : EventArgs
    {
        private List<string> _Fields = new List<string>();
        /// <summary>
        /// 資料來源所包含的所有欄位名稱
        /// </summary>
        public List<string> Fields { get { return _Fields; } }
    }
    public class RowDataLoadEventArgs : EventArgs
    {
        public RowDataLoadEventArgs() { RowDatas = new List<RowData>(); }
        public List<RowData> RowDatas { get; private set; }
    }

    public class IdentifyRowEventArgs : EventArgs
    {
        public RowData RowData { get; set; }
        public string ErrorMessage { get; set; }
        public string WarningMessage { get; set; }
    }
    /// <summary>
    /// 開始驗證
    /// </summary>
    public class ValidateStartEventArgs : EventArgs
    {
        private string[] _IDList = new string[0];
        /// <summary>
        /// 所有相關資料的系統編號
        /// </summary>
        public string[] List
        {
            get { return _IDList; }
            set { _IDList = value; }
        }
    }
    /// <summary>
    /// 驗證資料
    /// </summary>
    public class ValidateRowEventArgs : EventArgs
    {
        private RowData _Data;
        private List<string> _SelectedFields = new List<string>();
        private Dictionary<string, string> _WarningFields = new Dictionary<string, string>();
        private Dictionary<string, string> _ErrorFields = new Dictionary<string, string>();
        private string _ErrorMessage = "";
        /// <summary>
        /// 驗證中的資料
        /// </summary>
        public RowData Data { get { return _Data; } set { _Data = value; } }
        /// <summary>
        /// 被選取要匯入的欄位
        /// </summary>
        public List<string> SelectFields { get { return _SelectedFields; } }
        /// <summary>
        /// 各欄位警示資訊
        /// </summary>
        public Dictionary<string, string> WarningFields { get { return _WarningFields; } }
        /// <summary>
        /// 各欄位錯誤資訊
        /// </summary>
        public Dictionary<string, string> ErrorFields { get { return _ErrorFields; } }
        /// <summary>
        /// 整筆資料錯誤資訊
        /// </summary>
        public string ErrorMessage { get { return _ErrorMessage; } set { _ErrorMessage = value; } }
    }
    /// <summary>
    /// 分批次匯入資料的單一封包資訊
    /// </summary>
    public class ImportPackageEventArgs : EventArgs
    {
        private List<string> _ImportFields = new List<string>();
        private List<RowData> _Items = new List<RowData>();
        /// <summary>
        /// 要匯入的資料
        /// </summary>
        public List<RowData> Items { get { return _Items; } }
        /// <summary>
        /// 被選取要匯入的欄位
        /// </summary>
        public List<string> ImportFields { get { return _ImportFields; } }
    }
}
