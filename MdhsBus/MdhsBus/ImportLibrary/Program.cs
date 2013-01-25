using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FISCA;
using FISCA.Presentation;

namespace 匯入資料精靈
{
    public static class Program
    {
        //static bool _ReplaceAll = false;
        //static List<校系資料> _DeletedItems = new List<校系資料>();
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [MainMethod]
        public static void Main()
        {
            //MotherForm.RibbonBarItems["學生","匯出\\匯入"]["匯入"]["匯入校系資料"].Click+=delegate
            //{
            //    ImportLibrary.PowerfulImportWizard wizard = new ImportLibrary.PowerfulImportWizard("匯入校系資料", null);
            //    ImportLibrary.IPowerfulImportWizard iWizard = wizard;
            //    ////增加自訂的選項按鈕
            //    //ImportLibrary.VirtualCheckBox chkReplaceAll = new ImportLibrary.VirtualCheckBox("刪除不在匯入表中的所有資料", false);
            //    //chkReplaceAll.Description = "請小心使用";
            //    //chkReplaceAll.CheckedChanged += new EventHandler(chkReplaceAll_CheckedChanged);
            //    //iWizard.Options.Add(chkReplaceAll);
            //    ////每個匯入會有一個以上的欄位做識別，匯入的資料表一定要有這些欄位
            //    //iWizard.RequiredFields.Add("代碼");
            //    ////除了識別欄位外可以匯入資料的欄位
            //    //iWizard.ImportableFields.AddRange("校名", "系名", "組別", "名額");
            //    ////匯入的主要流程以下
            //    ////wizard會自動處理使用者開啟匯入資料表，並將資料表中內容填成RowData的集合，每個RowData有一個ID的屬性
            //    ////在此事件可以用RowData中識別欄位的資料去找出這筆資料的ID並填入RowData.ID
            //    //iWizard.IdentifyRow += new EventHandler<ImportLibrary.IdentifyRowEventArgs>(iWizard_IdentifyRow);
            //    ////驗證RowData中的資料是否合法，依照先驗證後匯入的原則，所有有可能在匯入資料時發生的問題都應該在驗證時預先發現，只要有填入Error，就不會進入匯入的流程
            //    //iWizard.ValidateRow += new EventHandler<ImportLibrary.ValidateRowEventArgs>(iWizard_ValidateRow);
            //    ////開始匯入的流程，可以在此做一些初始化的動作
            //    //iWizard.ImportStart += new EventHandler(iWizard_ImportStart);
            //    ////真的將資料匯入，精靈會將資料做分批並一批一批的傳入這個事件，可用PackageSize設定每批的資料量。PS.屬於同一個ID的RowData一定會被分在同一批，比如PackageSize設20但是有50筆資料是對應到同一個ID，那就有一批資料會超過20並包含這50筆資料
            //    //iWizard.ImportPackage += new EventHandler<ImportLibrary.ImportPackageEventArgs>(iWizard_ImportPackage);
            //    wizard.ShowDialog();
            //};
        }
        //static void iWizard_IdentifyRow(object sender, ImportLibrary.IdentifyRowEventArgs e)
        //{
        //    if (校系資料庫.Items.ContainsKey(e.RowData["代碼"]))
        //        e.RowData.ID = 校系資料庫.Items[e.RowData["代碼"]].UID;
        //}

        //static void iWizard_ValidateRow(object sender, ImportLibrary.ValidateRowEventArgs e)
        //{
        //    foreach (var field in e.SelectFields)
        //    {
        //        string value = e.Data[field];
        //        switch (field)
        //        {
        //            default:
        //            case "校名":
        //            case "系名":
        //            case "組別":
        //                break;
        //            case "名額":
        //                int i = 0;
        //                if (!int.TryParse(value, out i) || i < 0)
        //                    e.ErrorFields.Add("名額", "請輸入大於0之整數數字");
        //                break;
        //        }
        //    }
        //}

        //static void iWizard_ImportStart(object sender, EventArgs e)
        //{
        //    if (_ReplaceAll)
        //    {
        //        foreach (var item in 校系資料庫.Items.Values)
        //        {
        //            _DeletedItems.Add(item);
        //        }
        //    }
        //}

        //static void iWizard_ImportPackage(object sender, ImportLibrary.ImportPackageEventArgs e)
        //{
        //    List<校系資料> importItems = new List<校系資料>();
        //    foreach (var row in e.Items)
        //    {
        //        校系資料 importData = (row.ID != "") ?
        //            校系資料庫.Items[row["代碼"]] :
        //            new 校系資料() { 代碼 = row["代碼"] };
        //        if (_DeletedItems.Contains(importData))
        //            _DeletedItems.Remove(importData);
        //        foreach (var field in e.ImportFields)
        //        {
        //            string value = row[field];
        //            switch (field)
        //            {
        //                default:
        //                    break;
        //                case "校名":
        //                    importData.校名 = value;
        //                    break;
        //                case "系名":
        //                    importData.系名 = value;
        //                    break;
        //                case "組別":
        //                    importData.組別 = value;
        //                    break;
        //                case "名額":
        //                    importData.名額 = int.Parse(value);
        //                    break;
        //            }
        //        }
        //        importItems.Add(importData);
        //    }
        //    importItems.SaveAll();
        //}


        //static void iWizard_ImportComplete(object sender, EventArgs e)
        //{
        //    if (_ReplaceAll)
        //    {
        //        foreach (var item in _DeletedItems)
        //        {
        //            item.Deleted = true;
        //        }
        //        _DeletedItems.SaveAll();
        //    }
        //    校系資料庫.Sync();
        //}
    }
}
