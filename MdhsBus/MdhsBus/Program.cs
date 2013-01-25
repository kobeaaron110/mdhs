using System;
using System.Collections.Generic;
using System.Text;
using FISCA.Permission;
using SmartSchool;
using FISCA.Presentation;
using FISCA.UDT;
using FISCA;
using Aspose.Cells;
using System.Windows.Forms;
using K12.Data;

namespace MdhsBus
{
    public class Program
    {
        static List<Data.BusStop> _DeletedBusStops = new List<Data.BusStop>();
        static List<Data.StudentByBus> _DeletedBusStudents = new List<Data.StudentByBus>();
        static Dictionary<string, Dictionary<string, Data.BusStop>> 校車站名資料庫 = new Dictionary<string,Dictionary<string, Data.BusStop>>();
        static Dictionary<string, Data.BusStop> 檢查校車站名資料庫 = new Dictionary<string, Data.BusStop>();
        static Dictionary<string, Dictionary<string, Data.BusSetup>> 校車時間紀錄資料庫 = new Dictionary<string, Dictionary<string, Data.BusSetup>>();
        static Dictionary<string, Dictionary<string, Data.StudentByBus>> 學生搭車紀錄資料庫 = new Dictionary<string, Dictionary<string, Data.StudentByBus>>();
        static Dictionary<string, Data.StudentByBus> 搭車紀錄資料庫 = new Dictionary<string, Data.StudentByBus>();
        static Dictionary<string, Data.StudentByBus> 新生搭車紀錄資料庫 = new Dictionary<string, Data.StudentByBus>();
        static Dictionary<string, StudentRecord> 學生資料庫 = new Dictionary<string, StudentRecord>();
        static Dictionary<string, NewStudentRecord> 新生資料庫 = new Dictionary<string, NewStudentRecord>();
        static Dictionary<string, ClassRecord> 班級資料庫 = new Dictionary<string, ClassRecord>();
        static List<string> 新生科別資料庫 = new List<string>();

        [MainMethod()]
        public static void Main()
        {
            FISCA.Permission.RoleAclSource.Instance.Root.SubCatalogs["明德外掛系統"]["校車模組"].Add(new FISCA.Permission.ReportFeature("MdhsBus", "校車模組"));
            FISCA.Permission.RoleAclSource.Instance.Root.SubCatalogs["明德外掛系統"]["校車模組"].Add(new FISCA.Permission.DetailItemFeature("MdhsBusView", "校車明細"));
            if (!FISCA.Permission.UserAcl.Current["MdhsBus"].Executable) return;

            //建立或引用 『總務作業』 頁簽下的 RibbonBarItem『校車』
            FISCA.Presentation.RibbonBarItem rbItem = FISCA.Presentation.MotherForm.RibbonBarItems["總務作業", "校車作業"];
            FISCA.Presentation.RibbonBarItem rbItem01 = FISCA.Presentation.MotherForm.RibbonBarItems["新生", "報表及統計"];
            FISCA.Presentation.RibbonBarItem rbItem02 = FISCA.Presentation.MotherForm.RibbonBarItems["總務作業", "校車收費"];
            RibbonBarItem StudentReports = K12.Presentation.NLDPanels.Student.RibbonBarItems["統計報表"];

            //建立或引用 RibbonBarItem『校車』裡的按鈕 『班級學生資料』

            //建立或引用 RibbonBarItem『校車』裡的按鈕 『未銷假學生』
            //FISCA.Presentation.RibbonBarButton rbBtnAssign = rbItem["校車未銷假學生"];
            //rbBtnAssign.Click += new EventHandler(rbBtnQuery_Click);

            if (CurrentUser.Acl["MdhsBus"].Executable)
            {
                Catalog ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"];

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車設定"];
                ribbon.Add(new ReportFeature("NewMdhsBusStopSetup", "校車路線站名維護-新"));
                MenuButton rbi36 = rbItem["校車設定"]["校車路線站名維護-新"];
                rbi36.Enable = CurrentUser.Acl["NewMdhsBusStopSetup"].Executable;
                rbi36.Click += delegate
                {
                    AssignBusNew frm = new AssignBusNew();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車設定"];
                ribbon.Add(new ReportFeature("MdhsBusStopSetup", "校車路線站名維護"));
                MenuButton rbi30 = rbItem["校車設定"]["校車路線站名維護"];
                rbi30.Enable = CurrentUser.Acl["MdhsBusStopSetup"].Executable;
                //FISCA.Presentation.RibbonBarButton rbBtnAdd = rbItem["校車路線站名維護"];
                rbi30.Click += new EventHandler(rbBtnAssign_Click);

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["匯入及匯出"];
                ribbon.Add(new ReportFeature("MdhsBusImport", "匯入校車路線站名資料"));
                MenuButton rbi01 = rbItem["匯入及匯出"]["匯入"]["匯入校車路線站名資料"];
                rbi01.Enable = CurrentUser.Acl["MdhsBusImport"].Executable;
                rbi01.Click += delegate
                {
                    ImportLibrary.PowerfulImportWizard wizardBusStop = new ImportLibrary.PowerfulImportWizard("匯入校車路線站名資料", null);
                    ImportLibrary.IPowerfulImportWizard iWizardBusStop = wizardBusStop;

                    List<Data.BusStop> _Source = new List<Data.BusStop>();
                    AccessHelper udtHelper = new AccessHelper();
                    _Source = udtHelper.Select<Data.BusStop>();
                    校車站名資料庫.Clear();
                    foreach (Data.BusStop var in _Source)
                    {
                        if (!校車站名資料庫.ContainsKey(var.BusTimeName))
                            校車站名資料庫.Add(var.BusTimeName, new Dictionary<string, Data.BusStop>());
                        if (!校車站名資料庫[var.BusTimeName].ContainsKey(var.BusStopID))
                            校車站名資料庫[var.BusTimeName].Add(var.BusStopID, var);
                        if (!檢查校車站名資料庫.ContainsKey(var.UID))
                            檢查校車站名資料庫.Add(var.UID, var);
                    }

                    iWizardBusStop.RequiredFields.AddRange("校車時段", "代碼");
                    iWizardBusStop.ImportableFields.AddRange("站名", "月費", "車號", "站序", "停車地址", "到站時間", "放學上車地點");
                    iWizardBusStop.IdentifyRow += new EventHandler<ImportLibrary.IdentifyRowEventArgs>(iWizardBusStop_IdentifyRow);
                    iWizardBusStop.ValidateRow += new EventHandler<ImportLibrary.ValidateRowEventArgs>(iWizardBusStop_ValidateRow);
                    iWizardBusStop.ImportStart += new EventHandler(iWizardBusStop_ImportStart);
                    iWizardBusStop.ImportPackage += new EventHandler<ImportLibrary.ImportPackageEventArgs>(iWizardBusStop_ImportPackage);
                    wizardBusStop.ShowDialog();
                };

                ribbon.Add(new ReportFeature("MdhsBusStudentImport", "匯入校車學生乘車資料"));
                MenuButton rbi02 = rbItem["匯入及匯出"]["匯入"]["匯入校車學生乘車資料"];
                rbi02.Enable = CurrentUser.Acl["MdhsBusStudentImport"].Executable;
                rbi02.Click += delegate
                {
                    ImportLibrary.PowerfulImportWizard wizardBusStudent = new ImportLibrary.PowerfulImportWizard("匯入校車學生乘車資料", null);
                    ImportLibrary.IPowerfulImportWizard iWizardBusStudent = wizardBusStudent;

                    List<Data.BusStop> _BusSource = Data.BusStopDAO.GetSortByBusNumberList();
                    //List<string> bustimes = new List<string>();
                    AccessHelper udtHelper = new AccessHelper();
                    List<Data.BusSetup> _Source = Data.BusSetupDAO.GetSortByBusStartDateList();
                    List<Data.StudentByBus> _StudentSource = udtHelper.Select<Data.StudentByBus>();
                    校車站名資料庫.Clear();
                    foreach (Data.BusStop var in _BusSource)
                    {
                        if (!校車站名資料庫.ContainsKey(var.BusTimeName))
                            校車站名資料庫.Add(var.BusTimeName, new Dictionary<string, Data.BusStop>());
                        if (!校車站名資料庫[var.BusTimeName].ContainsKey(var.BusStopID))
                            校車站名資料庫[var.BusTimeName].Add(var.BusStopID, var);
                    }
                    校車時間紀錄資料庫.Clear();
                    foreach (Data.BusSetup var in _Source)
                    {
                        if (!校車時間紀錄資料庫.ContainsKey(var.BusYear.ToString()))
                            校車時間紀錄資料庫.Add(var.BusYear.ToString(), new Dictionary<string, Data.BusSetup>());
                        if (!校車時間紀錄資料庫[var.BusYear.ToString()].ContainsKey(var.BusRangeName))
                            校車時間紀錄資料庫[var.BusYear.ToString()].Add(var.BusRangeName, var);
                    }
                    學生搭車紀錄資料庫.Clear();
                    foreach (Data.StudentByBus var in _StudentSource)
                    {
                        if (!學生搭車紀錄資料庫.ContainsKey(var.StudentID))
                            學生搭車紀錄資料庫.Add(var.StudentID, new Dictionary<string, MdhsBus.Data.StudentByBus>());
                        if (!學生搭車紀錄資料庫[var.StudentID].ContainsKey(var.SchoolYear.ToString() + var.BusRangeName))
                            學生搭車紀錄資料庫[var.StudentID].Add(var.SchoolYear.ToString() + var.BusRangeName, var);
                    }
                    學生資料庫.Clear();
                    List<StudentRecord> students = K12.Data.Student.SelectAll();
                    foreach (StudentRecord var in students)
                    {
                        if (var.Status.ToString() != "一般")
                            continue;
                        if (!學生資料庫.ContainsKey(var.StudentNumber))
                            學生資料庫.Add(var.StudentNumber, var);
                    }

                    班級資料庫.Clear();
                    List<ClassRecord> classes = K12.Data.Class.SelectAll();
                    foreach (ClassRecord var in classes)
                    {
                        if (var.GradeYear == null)
                            continue;
                        else if (var.GradeYear > 3)
                            continue;
                        else if (var.Name.IndexOf("夜輔") >= 0 || var.Name.IndexOf("轉學") >= 0 || var.Name.IndexOf("選修") >= 0)
                            continue;
                        if (!班級資料庫.ContainsKey(var.Name))
                            班級資料庫.Add(var.Name, var);
                    }

                    iWizardBusStudent.RequiredFields.AddRange("搭車年度", "校車時段", "期間名稱", "代碼", "班級", "學號");
                    iWizardBusStudent.ImportableFields.AddRange("天數", "車費", "是否繳費", "繳費日期", "備註");
                    iWizardBusStudent.IdentifyRow += new EventHandler<ImportLibrary.IdentifyRowEventArgs>(iWizardBusStudent_IdentifyRow);
                    iWizardBusStudent.ValidateRow += new EventHandler<ImportLibrary.ValidateRowEventArgs>(iWizardBusStudent_ValidateRow);
                    iWizardBusStudent.ImportStart += new EventHandler(iWizardBusStudent_ImportStart);
                    iWizardBusStudent.ImportPackage += new EventHandler<ImportLibrary.ImportPackageEventArgs>(iWizardBusStudent_ImportPackage);
                    wizardBusStudent.ShowDialog();
                };


                ribbon.Add(new ReportFeature("MdhsBusNewStudentImport", "匯入校車新生乘車資料"));
                MenuButton rbi03 = rbItem["匯入及匯出"]["匯入"]["匯入校車新生乘車資料"];
                rbi03.Enable = CurrentUser.Acl["MdhsBusNewStudentImport"].Executable;
                rbi03.Click += delegate
                {
                    ImportLibrary.PowerfulImportWizard wizardBusNewStudent = new ImportLibrary.PowerfulImportWizard("匯入校車新生乘車資料", null);
                    ImportLibrary.IPowerfulImportWizard iWizardBusNewStudent = wizardBusNewStudent;

                    List<Data.BusStop> _BusSource = Data.BusStopDAO.GetSortByBusNumberList();
                    //List<string> bustimes = new List<string>();
                    AccessHelper udtHelper = new AccessHelper();
                    List<Data.BusSetup> _Source = Data.BusSetupDAO.GetSortByBusStartDateList();
                    List<Data.StudentByBus> _StudentSource = udtHelper.Select<Data.StudentByBus>();
                    校車站名資料庫.Clear();
                    foreach (Data.BusStop var in _BusSource)
                    {
                        if (!校車站名資料庫.ContainsKey(var.BusTimeName))
                            校車站名資料庫.Add(var.BusTimeName, new Dictionary<string, Data.BusStop>());
                        if (!校車站名資料庫[var.BusTimeName].ContainsKey(var.BusStopID))
                            校車站名資料庫[var.BusTimeName].Add(var.BusStopID, var);
                    }
                    校車時間紀錄資料庫.Clear();
                    foreach (Data.BusSetup var in _Source)
                    {
                        if (!校車時間紀錄資料庫.ContainsKey(var.BusYear.ToString()))
                            校車時間紀錄資料庫.Add(var.BusYear.ToString(), new Dictionary<string, Data.BusSetup>());
                        if (!校車時間紀錄資料庫[var.BusYear.ToString()].ContainsKey(var.BusRangeName))
                            校車時間紀錄資料庫[var.BusYear.ToString()].Add(var.BusRangeName, var);
                    }
                    新生搭車紀錄資料庫.Clear();
                    foreach (Data.StudentByBus var in _StudentSource)
                    {
                        if (!新生搭車紀錄資料庫.ContainsKey(var.StudentID))
                            新生搭車紀錄資料庫.Add(var.StudentID, var);
                    }
                    新生資料庫.Clear();
                    新生科別資料庫.Clear();
                    List<NewStudentRecord> students = udtHelper.Select<NewStudentRecord>("學年度='" + K12.Data.School.DefaultSchoolYear + "'");
                    foreach (NewStudentRecord var in students)
                    {
                        if (var.Active != true)
                            continue;
                        if (!新生資料庫.ContainsKey(var.Number))
                            新生資料庫.Add(var.Number, var);
                        if (!新生科別資料庫.Contains(var.Dept))
                            新生科別資料庫.Add(var.Dept);
                    }

                    iWizardBusNewStudent.RequiredFields.AddRange("搭車年度", "校車時段", "期間名稱", "代碼", "科別", "編號");
                    iWizardBusNewStudent.ImportableFields.AddRange("天數", "車費", "是否繳費", "繳費日期", "備註");
                    iWizardBusNewStudent.IdentifyRow += new EventHandler<ImportLibrary.IdentifyRowEventArgs>(iWizardBusNewStudent_IdentifyRow);
                    iWizardBusNewStudent.ValidateRow += new EventHandler<ImportLibrary.ValidateRowEventArgs>(iWizardBusNewStudent_ValidateRow);
                    iWizardBusNewStudent.ImportStart += new EventHandler(iWizardBusNewStudent_ImportStart);
                    iWizardBusNewStudent.ImportPackage += new EventHandler<ImportLibrary.ImportPackageEventArgs>(iWizardBusNewStudent_ImportPackage);
                    wizardBusNewStudent.ShowDialog();
                };


                ribbon.Add(new ReportFeature("MdhsBusStudentPaymentImport", "匯入校車學生繳費資料"));
                MenuButton rbi04 = rbItem["匯入及匯出"]["匯入"]["匯入校車學生繳費資料"];
                rbi04.Enable = CurrentUser.Acl["MdhsBusStudentPaymentImport"].Executable;
                rbi04.Click += delegate
                {
                    ImportLibrary.PowerfulImportWizard wizardBusStudentPayment = new ImportLibrary.PowerfulImportWizard("匯入校車學生繳費資料", null);
                    ImportLibrary.IPowerfulImportWizard iWizardBusStudentPayment = wizardBusStudentPayment;

                    List<Data.BusStop> _BusSource = Data.BusStopDAO.GetSortByBusNumberList();
                    AccessHelper udtHelper = new AccessHelper();
                    List<Data.BusSetup> _Source = Data.BusSetupDAO.GetSortByBusStartDateList();
                    List<Data.StudentByBus> _StudentSource = udtHelper.Select<Data.StudentByBus>();

                    搭車紀錄資料庫.Clear();
                    foreach (Data.StudentByBus var in _StudentSource)
                    {
                        if (!搭車紀錄資料庫.ContainsKey(var.UID))
                            搭車紀錄資料庫.Add(var.UID, var);
                    }

                    iWizardBusStudentPayment.RequiredFields.AddRange("搭車系統編號");
                    iWizardBusStudentPayment.ImportableFields.AddRange("是否繳費", "繳費日期", "備註");
                    iWizardBusStudentPayment.IdentifyRow += new EventHandler<ImportLibrary.IdentifyRowEventArgs>(iWizardBusStudentPayment_IdentifyRow);
                    iWizardBusStudentPayment.ValidateRow += new EventHandler<ImportLibrary.ValidateRowEventArgs>(iWizardBusStudentPayment_ValidateRow);
                    iWizardBusStudentPayment.ImportStart += new EventHandler(iWizardBusStudentPayment_ImportStart);
                    iWizardBusStudentPayment.ImportPackage += new EventHandler<ImportLibrary.ImportPackageEventArgs>(iWizardBusStudentPayment_ImportPackage);
                    wizardBusStudentPayment.ShowDialog();
                };


                ribbon.Add(new ReportFeature("MdhsBusExport", "匯出校車路線站名資料"));
                MenuButton rbi21 = rbItem["匯入及匯出"]["匯出"]["匯出校車路線站名資料"];
                rbi21.Enable = CurrentUser.Acl["MdhsBusExport"].Executable;
                rbi21.Click += delegate
                {
                    //可存取 UDT 的工具類別
                    AccessHelper udtHelper = new AccessHelper();

                    //List<ClassRecord> Classes = K12.Data.Class.SelectAll();
                    //Dictionary<string, ClassRecord> classrec = new Dictionary<string, ClassRecord>();
                    //foreach (ClassRecord var in Classes)
                    //{
                    //    if (!classrec.ContainsKey(var.Name))
                    //        classrec.Add(var.Name, var);
                    //}
                    //List<Data.StudentByBus> Studentbuses = udtHelper.Select<Data.StudentByBus>();
                    //foreach (Data.StudentByBus var in Studentbuses)
                    //{
                    //    string cName = var.ClassName;
                    //    if (var.ClassName.IndexOf("科") > 0)
                    //        continue;
                    //    if (var.ClassName.Substring(1, 1) == "一")
                    //        cName = cName.Replace("一", "二");
                    //    else if (var.ClassName.Substring(1, 1) == "二")
                    //        cName = cName.Replace("二", "三");
                    //    else
                    //        continue;
                    //    if (cName == "普二甲")
                    //        cName = "普二甲自";
                    //    var.ClassID = classrec[cName].ID;
                    //}
                    //Studentbuses.SaveAll();


                    List<Data.BusStop> buses = udtHelper.Select<Data.BusStop>();
                    Workbook wb = new Workbook();
                    Style defaultStyle = wb.DefaultStyle;
                    defaultStyle.Font.Name = "標楷體";
                    defaultStyle.Font.Size = 12;
                    wb.DefaultStyle = defaultStyle;
                    int row = 0;
                    int nowSet = 0;
                    wb.Worksheets[0].Name = "校車路線站名資料";

                    //校車路線站名資料
                    wb.Worksheets[0].Cells[row, 0].PutValue("代碼");
                    wb.Worksheets[0].Cells[row, 1].PutValue("站名");
                    wb.Worksheets[0].Cells[row, 2].PutValue("車費");
                    wb.Worksheets[0].Cells[row, 3].PutValue("車號");
                    wb.Worksheets[0].Cells[row, 4].PutValue("站序");
                    wb.Worksheets[0].Cells[row, 5].PutValue("到站時間");
                    wb.Worksheets[0].Cells[row, 6].PutValue("放學上車地點");
                    wb.Worksheets[0].Cells[row, 7].PutValue("停車地址");
                    wb.Worksheets[0].Cells[row, 8].PutValue("校車時段");
                    row++;
                    buses.Sort(CompareBusNumber);
                    foreach (Data.BusStop var in buses)
                    {
                        MotherForm.SetStatusBarMessage("正在產生報表", nowSet++ * 100 / buses.Count);
                        wb.Worksheets[0].Cells[row, 0].PutValue(var.BusStopID);
                        wb.Worksheets[0].Cells[row, 1].PutValue(var.BusStopName);
                        wb.Worksheets[0].Cells[row, 2].PutValue(var.BusMoney);
                        wb.Worksheets[0].Cells[row, 3].PutValue(var.BusNumber);
                        wb.Worksheets[0].Cells[row, 4].PutValue(var.BusStopNo);
                        wb.Worksheets[0].Cells[row, 5].PutValue(var.ComeTime);
                        wb.Worksheets[0].Cells[row, 6].PutValue(var.BusUpAddr);
                        wb.Worksheets[0].Cells[row, 7].PutValue(var.BusStopAddr);
                        wb.Worksheets[0].Cells[row, 8].PutValue(var.BusTimeName);
                        row++;
                    }
                    wb.Worksheets[0].AutoFitColumns();
                    try
                    {
                        wb.Save(Application.StartupPath + "\\Reports\\匯出校車路線站名資料.xls", FileFormatType.Excel2003);
                        System.Diagnostics.Process.Start(Application.StartupPath + "\\Reports\\匯出校車路線站名資料.xls");
                        MotherForm.SetStatusBarMessage("校車路線站名資料已匯出完成", 100);
                    }
                    catch
                    {
                        System.Windows.Forms.SaveFileDialog sd1 = new System.Windows.Forms.SaveFileDialog();
                        sd1.Title = "另存新檔";
                        sd1.FileName = "匯出校車路線站名資料.xls";
                        sd1.Filter = "Excel檔案 (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                        if (sd1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            try
                            {
                                wb.Save(sd1.FileName, FileFormatType.Excel2003);
                                System.Diagnostics.Process.Start(sd1.FileName);
                                MotherForm.SetStatusBarMessage("校車路線站名資料已匯出完成", 100);
                            }
                            catch
                            {
                                System.Windows.Forms.MessageBox.Show("指定路徑無法存取。", "建立檔案失敗", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                };

                ribbon.Add(new ReportFeature("MdhsStudnetByBusDetail", "匯出學生搭乘校車資料"));
                MenuButton rbi22 = rbItem["匯入及匯出"]["匯出"]["匯出學生搭乘校車資料"];
                rbi22.Enable = CurrentUser.Acl["MdhsStudnetByBusDetail"].Executable;
                rbi22.Click += delegate
                {
                    StudentByBusDetail frm = new StudentByBusDetail();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車設定"];
                ribbon.Add(new ReportFeature("MdhsBusSetup", "校車時間設定"));
                MenuButton rbi31 = rbItem["校車設定"]["校車時間設定"];
                rbi31.Enable = CurrentUser.Acl["MdhsBusSetup"].Executable;
                rbi31.Click += delegate
                {
                    Setup frm = new Setup();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車設定"];
                ribbon.Add(new ReportFeature("MdhsStudnetByBus", "學生搭乘校車設定"));
                MenuButton rbi32 = rbItem["校車設定"]["學生搭乘校車設定"];
                rbi32.Enable = CurrentUser.Acl["MdhsStudnetByBus"].Executable;
                rbi32.Click += delegate
                {
                    StudentBus frm = new StudentBus();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["新生校車設定"];
                ribbon.Add(new ReportFeature("MdhsNewStudnetByBus", "新生搭乘校車設定"));
                MenuButton rbi33 = rbItem["新生校車設定"]["新生搭乘校車設定"];
                rbi33.Enable = CurrentUser.Acl["MdhsNewStudnetByBus"].Executable;
                rbi33.Click += delegate
                {
                    NewStudentBus frm = new NewStudentBus();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["新生校車設定"];
                ribbon.Add(new ReportFeature("MdhsNewStudnetTransfer", "新生搭乘校車轉高一學生"));
                MenuButton rbi34 = rbItem["新生校車設定"]["新生搭乘校車轉高一學生"];
                rbi34.Enable = CurrentUser.Acl["MdhsNewStudnetTransfer"].Executable;
                rbi34.Click += delegate
                {
                    NewStudentTransfer frm = new NewStudentTransfer();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車設定"];
                ribbon.Add(new ReportFeature("MdhsBusMoneyMitigate", "夜輔撘車學生已搭日校校車車費減免"));
                MenuButton rbi35 = rbItem["校車設定"]["夜輔撘車學生已搭日校校車車費減免"];
                rbi35.Enable = CurrentUser.Acl["MdhsBusMoneyMitigate"].Executable;
                rbi35.Click += delegate
                {
                    BusMoneyMitigate frm = new BusMoneyMitigate();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車報表"];
                ribbon.Add(new ReportFeature("MdhsStudnetSurveyform", "學生搭乘校車調查表/點名表"));
                MenuButton rbi41 = rbItem["校車報表"]["學生搭乘校車調查表/點名表"];
                rbi41.Enable = CurrentUser.Acl["MdhsStudnetSurveyform"].Executable;
                rbi41.Click += delegate
                {
                    Surveyform frm = new Surveyform();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車報表"];
                ribbon.Add(new ReportFeature("MdhsNewStudnetBusPrint", "新生學生校車繳費單"));
                MenuButton rbi42 = rbItem01["報表"]["學費相關報表"]["新生校車繳費單列印"];
                rbi42.Enable = CurrentUser.Acl["MdhsNewStudnetBusPrint"].Executable;
                rbi42.Click += delegate
                {
                    PaymentSheet frm = new PaymentSheet();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車報表"];
                ribbon.Add(new ReportFeature("MdhsNewStudnetsBusPrint", "新生學生校車繳費單(批次)"));
                MenuButton rbi43 = rbItem01["報表"]["學費相關報表"]["新生校車繳費單列印(批次)"];
                rbi43.Enable = CurrentUser.Acl["MdhsNewStudnetsBusPrint"].Executable;
                rbi43.Click += delegate
                {
                    PaymentSheets frm = new PaymentSheets();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車報表"];
                ribbon.Add(new ReportFeature("MdhsStudnetBusPrint", "學生校車繳費單"));
                MenuButton rbi44 = StudentReports["報表"]["明德女中"]["學費相關報表"]["學生校車繳費單列印"];
                rbi44.Enable = CurrentUser.Acl["MdhsNewStudnetBusPrint"].Executable;
                rbi44.Click += delegate
                {
                    StudnetPaymentSheet frm = new StudnetPaymentSheet();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車報表"];
                ribbon.Add(new ReportFeature("MdhsStudnetsBusPrint", "學生校車繳費單(批次)"));
                MenuButton rbi45 = rbItem["校車報表"]["學生校車繳費單列印(批次)"];
                rbi45.Enable = CurrentUser.Acl["MdhsStudnetsBusPrint"].Executable;
                rbi45.Click += delegate
                {
                    StudnetPaymentSheets frm = new StudnetPaymentSheets();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車收費設定"];
                ribbon.Add(new ReportFeature("MdhsBusPaymentSetup", "校車收費設定"));
                MenuButton rbi51 = rbItem["校車設定"]["校車收費設定"];
                rbi51.Enable = CurrentUser.Acl["MdhsBusPaymentSetup"].Executable;
                rbi51.Click += delegate
                {
                    PaymentSetup frm = new PaymentSetup();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車收費設定"];
                ribbon.Add(new ReportFeature("MdhsBusPaymentBar", "校車收費管理"));
                MenuButton rbi52 = rbItem02["校車收費管理"];
                rbi52.Enable = CurrentUser.Acl["MdhsBusPaymentBar"].Executable;
                rbi52.Click += delegate
                {
                    PaymentBar frm = new PaymentBar();
                    frm.ShowDialog();
                };

                ribbon = RoleAclSource.Instance["明德外掛系統"]["校車模組"]["校車收費設定"];
                ribbon.Add(new ReportFeature("MdhsBusBalance", "校車收費對帳"));
                MenuButton rbi53 = rbItem02["校車收費對帳"];
                rbi53.Enable = CurrentUser.Acl["MdhsBusBalance"].Executable;
                rbi53.Click += delegate
                {
                    MdhsBusBalance frm = new MdhsBusBalance();
                    frm.ShowDialog();
                };


                //rollbook
            }


            if (FISCA.Permission.UserAcl.Current["MdhsBusView"].Editable || FISCA.Permission.UserAcl.Current["MdhsBusView"].Viewable)
            {
                K12.Presentation.NLDPanels.Student.AddView(new StudentBusBView());
                K12.Presentation.NLDPanels.Student.AddDetailBulider<StudentBusDetail>();
            }
        }

        static void iWizardBusStop_IdentifyRow(object sender, ImportLibrary.IdentifyRowEventArgs e)
        {
            if (校車站名資料庫.ContainsKey(e.RowData["校車時段"]))
                if (校車站名資料庫[e.RowData["校車時段"]].ContainsKey(e.RowData["代碼"]))
                    e.RowData.ID = 校車站名資料庫[e.RowData["校車時段"]][e.RowData["代碼"]].UID;
        }

        static void iWizardBusStop_ValidateRow(object sender, ImportLibrary.ValidateRowEventArgs e)
        {
            string busnumber = "";
            foreach (var field in e.SelectFields)
            {
                if (field == "")
                    continue;
                string value = e.Data[field];
                if (field == "車號")
                    busnumber = value;
                switch (field)
                {
                    default:
                    case "校車時段":
                        if (value.Length == 0)
                            e.ErrorFields.Add("校車時段", "請確認校車時段不可為空！");
                        break;
                    case "代碼":
                        if (value.Length != 4)
                            e.ErrorFields.Add("代碼", "請確認電腦代碼之長度必須是四碼！");
                        break;
                    case "站名":
                        if (value.Length == 0)
                            e.ErrorFields.Add("站名", "請確認站名不可為空！");
                        break;
                    case "月費":
                        if (value.Length > 0)
                            if (int.Parse(value) < 0)
                                e.ErrorFields.Add("月費", "請確認月費是否有誤！");
                        break;
                    case "車號":
                        if (value.Length != 2)
                            e.ErrorFields.Add("車號", "請確認車號是否有誤！");
                        break;
                    case "站序":
                        if (value.Length == 0)
                            e.ErrorFields.Add("站序", "請確認站序不可為空！");
                        else
                            if (value.Substring(0, 2) != busnumber)
                                e.ErrorFields.Add("站序", "請確認站序前兩碼是否有誤！");
                        break;
                    case "停車地址":
                        break;
                    case "到站時間":
                        break;
                    case "放學上車地點":
                        break;
                }
            }
        }

        static void iWizardBusStop_ImportStart(object sender, EventArgs e)
        {
            //foreach (Data.BusStop item in 檢查校車站名資料庫.Values)
            //{
            //    _DeletedBusStops.Add(item);
            //}
        }

        static void iWizardBusStop_ImportPackage(object sender, ImportLibrary.ImportPackageEventArgs e)
        {
            List<Data.BusStop> importItems = new List<Data.BusStop>();
            foreach (var row in e.Items)
            {
                Data.BusStop importData = (row.ID != "") ? 檢查校車站名資料庫[row.ID] : new Data.BusStop();
                if (_DeletedBusStops.Contains(importData))
                    _DeletedBusStops.Remove(importData);
                foreach (var field in e.ImportFields)
                {
                    string value = row[field];
                    switch (field)
                    {
                        default:
                            break;
                        case "校車時段":
                            importData.BusTimeName = value;
                            break;
                        case "代碼":
                            importData.BusStopID = value;
                            break;
                        case "站名":
                            importData.BusStopName = value;
                            break;
                        case "月費":
                            if (value.Length > 0)
                                importData.BusMoney = int.Parse(value);
                            else
                                importData.BusMoney = 0;
                            break;
                        case "車號":
                            importData.BusNumber = value;
                            break;
                        case "站序":
                            importData.BusStopNo = value;
                            break;
                        case "停車地址":
                            importData.BusStopAddr = value;
                            break;
                        case "到站時間":
                            importData.ComeTime = value;
                            break;
                        case "放學上車地點":
                            importData.BusUpAddr = value;
                            break;
                    }
                }
                importItems.Add(importData);
            }
            importItems.SaveAll();
        }


        static void iWizardBusStop_ImportComplete(object sender, EventArgs e)
        {
            //foreach (var item in _DeletedBusStops)
            //{
            //    item.Deleted = true;
            //}
            //_DeletedBusStops.SaveAll();
        }

        static void iWizardBusStudent_IdentifyRow(object sender, ImportLibrary.IdentifyRowEventArgs e)
        {
            if (校車站名資料庫.ContainsKey(e.RowData["校車時段"]))
                if (校車站名資料庫[e.RowData["校車時段"]].ContainsKey(e.RowData["代碼"]))
                    if (校車時間紀錄資料庫.ContainsKey(e.RowData["搭車年度"]))
                        if (校車時間紀錄資料庫[e.RowData["搭車年度"]].ContainsKey(e.RowData["期間名稱"]))
                            if (學生資料庫.ContainsKey(e.RowData["學號"]))
                                if (學生搭車紀錄資料庫.ContainsKey(學生資料庫[e.RowData["學號"]].ID))
                                    if (學生搭車紀錄資料庫[學生資料庫[e.RowData["學號"]].ID].ContainsKey(e.RowData["搭車年度"] + e.RowData["期間名稱"]))
                                        e.RowData.ID = 學生搭車紀錄資料庫[學生資料庫[e.RowData["學號"]].ID][e.RowData["搭車年度"] + e.RowData["期間名稱"]].UID;
        }

        static void iWizardBusStudent_ValidateRow(object sender, ImportLibrary.ValidateRowEventArgs e)
        {
            string busyear = "";
            string bustime = "";
            foreach (var field in e.SelectFields)
            {
                if (field == "")
                    continue;
                string value = e.Data[field];
                if (field == "搭車年度")
                    busyear = value;
                if (field == "校車時段")
                    bustime = value;

                switch (field)
                {
                    default:
                    case "校車時段":
                        if (value.Length == 0)
                            e.ErrorFields.Add("校車時段", "請確認校車時段不可為空！");
                        else
                            if (!校車站名資料庫.ContainsKey(value))
                                e.ErrorFields.Add("校車時段", "請確認校車時段是否有誤！");
                        break;
                    case "代碼":
                        if (value.Length != 4)
                            e.ErrorFields.Add("代碼", "請確認電腦代碼之長度必須是四碼！");
                        else
                            if (!校車站名資料庫[bustime].ContainsKey(value))
                                e.ErrorFields.Add("代碼", "請確認電腦代碼是否有誤！");
                        break;
                    case "期間名稱":
                        if (value.Length == 0)
                            e.ErrorFields.Add("期間名稱", "請確認期間名稱不可為空！");
                        else
                            if (!校車時間紀錄資料庫[busyear].ContainsKey(value))
                                e.ErrorFields.Add("期間名稱", "請確認期間名稱是否有誤！");
                        break;
                    case "班級":
                        if (value.Length == 0)
                            e.ErrorFields.Add("班級", "請確認班級不可為空！");
                        break;
                    case "學號":
                        if (value.Length != 6)
                            e.ErrorFields.Add("學號", "請確認學號必須是六碼！");
                        else
                            if (!學生資料庫.ContainsKey(value))
                                e.ErrorFields.Add("學號", "請確認學號是否為在學學生！");
                        break;
                    case "搭車年度":
                        int syear;
                        if (value.Length == 0)
                            e.ErrorFields.Add("搭車年度", "請確認搭車年度不可為空！");
                        else
                        {
                            if (!int.TryParse(value, out syear))
                                e.ErrorFields.Add("搭車年度", "請確認搭車年度是否有誤！");
                            else
                                if (!校車時間紀錄資料庫.ContainsKey(value))
                                    e.ErrorFields.Add("搭車年度", "請確認搭車年度是否有誤！");
                        }
                        break;
                    case "天數":
                        int daycount;
                        if (value.Length == 0)
                            e.ErrorFields.Add("搭車天數", "請確認搭車天數不可為空！");
                        else
                            if (!int.TryParse(value, out daycount))
                                e.ErrorFields.Add("搭車天數", "請確認搭車天數是否有誤！");
                        break;
                    case "車費":
                        int money;
                        if (value.Length > 0)
                            if (!int.TryParse(value, out money))
                                e.ErrorFields.Add("車費", "請確認車費是否有誤！");
                        break;
                    case "是否繳費":
                        if (value.Length > 0)
                            if (value != "是" && value != "否" && value != "Yes" && value != "No" && value != "True" && value != "False")
                                e.ErrorFields.Add("是否繳費", "請確認是否繳費需為是或否！");
                        break;
                    case "繳費日期":
                        DateTime day;
                        if (value.Length > 0)
                            if (!DateTime.TryParse(value, out day))
                                e.ErrorFields.Add("繳費日期", "請確認繳費日期是否有誤！");
                        break;
                    case "備註":
                        break;
                }
            }
        }

        static void iWizardBusStudent_ImportStart(object sender, EventArgs e)
        {
            foreach (string var in 學生搭車紀錄資料庫.Keys)
            {
                //foreach (Data.StudentByBus item in 學生搭車紀錄資料庫[var].Values)
                //    _DeletedBusStudents.Add(item);
            }
        }

        static void iWizardBusStudent_ImportPackage(object sender, ImportLibrary.ImportPackageEventArgs e)
        {
            List<Data.StudentByBus> importItems = new List<Data.StudentByBus>();
            foreach (var row in e.Items)
            {
                Data.StudentByBus importData = new Data.StudentByBus();
                if (row.ID != "")
                    importData = 學生搭車紀錄資料庫[學生資料庫[row["學號"]].ID][row["搭車年度"] + row["期間名稱"]];
                //Data.StudentByBus importData = (row.ID != "") ? 學生搭車紀錄資料庫[學生資料庫[row["學號"]].ID][row["搭車年度"] + row["期間名稱"]] : new Data.StudentByBus();
                //if (_DeletedBusStudents.Contains(importData))
                //    _DeletedBusStudents.Remove(importData);
                string bustime = "";
                string busid = "";
                int daycount = 0;
                foreach (var field in e.ImportFields)
                {
                    string value = row[field];
                    if (field == "校車時段")
                        bustime = value;
                    if (field == "代碼")
                        busid = value;
                    if (field == "天數")
                        daycount = int.Parse(value);

                    switch (field)
                    {
                        default:
                            break;
                        case "校車時段":
                            importData.BusTimeName = value;
                            break;
                        case "代碼":
                            importData.BusStopID = value;
                            break;
                        case "期間名稱":
                            importData.BusRangeName = value;
                            break;
                        case "班級":
                            importData.ClassName = value;
                            if (班級資料庫.ContainsKey(value))
                                importData.ClassID = 班級資料庫[value].ID;
                            break;
                        case "學號":
                            if (importData.ClassID == null)
                                importData.ClassID = 學生資料庫[value].Class.ID;
                            importData.StudentID = 學生資料庫[value].ID;
                            break;
                        case "搭車年度":
                            if (value.Length > 0)
                                importData.SchoolYear = int.Parse(value);
                            break;
                        case "天數":
                            if (value.Length > 0)
                                importData.DateCount = int.Parse(value);
                            break;
                        case "車費":
                            if (value.Length > 0)
                                importData.BusMoney = int.Parse(value);
                            else
                                importData.BusMoney = 0;
                            break;
                        case "是否繳費":
                            if (value.Length > 0)
                            {
                                if (value == "是" || value == "Yes" || value == "True")
                                    importData.PayStatus = true;
                                else
                                    importData.PayStatus = false;
                            }
                            else
                                importData.PayStatus = false;
                            break;
                        case "繳費日期":
                            if (value.Length > 0)
                                 importData.PayDate = DateTime.Parse(value);
                            break;
                        case "備註":
                            importData.comment = value;
                            break;
                    }
                }
                int total = 校車站名資料庫[bustime][busid].BusMoney * daycount;
                int div_value = total / 10;
                if ((total - div_value * 10) < 5)
                    importData.BusMoney = div_value * 10;
                else
                    importData.BusMoney = div_value * 10 + 10;
                //importData.BusMoney = 校車站名資料庫[bustime][busid].BusMoney * daycount;
                importItems.Add(importData);
            }
            importItems.SaveAll();
        }


        static void iWizardBusStudent_ImportComplete(object sender, EventArgs e)
        {
            //foreach (var item in _DeletedBusStudents)
            //{
            //    item.Deleted = true;
            //}
            //_DeletedBusStudents.SaveAll();
        }

        static void iWizardBusNewStudent_IdentifyRow(object sender, ImportLibrary.IdentifyRowEventArgs e)
        {
            if (校車站名資料庫.ContainsKey(e.RowData["校車時段"]))
                if (校車站名資料庫[e.RowData["校車時段"]].ContainsKey(e.RowData["代碼"]))
                    if (校車時間紀錄資料庫.ContainsKey(e.RowData["搭車年度"]))
                        if (校車時間紀錄資料庫[e.RowData["搭車年度"]].ContainsKey(e.RowData["期間名稱"]))
                            if (新生資料庫.ContainsKey(e.RowData["編號"]))
                                if (新生搭車紀錄資料庫.ContainsKey(新生資料庫[e.RowData["編號"]].UID))
                                    e.RowData.ID = 新生搭車紀錄資料庫[新生資料庫[e.RowData["編號"]].UID].UID;
        }

        static void iWizardBusNewStudent_ValidateRow(object sender, ImportLibrary.ValidateRowEventArgs e)
        {
            string busyear = "";
            string bustime = "";
            foreach (var field in e.SelectFields)
            {
                if (field == "")
                    continue;
                string value = e.Data[field];
                if (field == "搭車年度")
                    busyear = value;
                if (field == "校車時段")
                    bustime = value;

                switch (field)
                {
                    default:
                    case "校車時段":
                        if (value.Length == 0)
                            e.ErrorFields.Add("校車時段", "請確認校車時段不可為空！");
                        else
                            if (!校車站名資料庫.ContainsKey(value))
                                e.ErrorFields.Add("校車時段", "請確認校車時段是否有誤！");
                        break;
                    case "代碼":
                        if (value.Length != 4)
                            e.ErrorFields.Add("代碼", "請確認電腦代碼之長度必須是四碼！");
                        else
                            if (!校車站名資料庫[bustime].ContainsKey(value))
                                e.ErrorFields.Add("代碼", "請確認電腦代碼是否有誤！");
                        break;
                    case "期間名稱":
                        if (value.Length == 0)
                            e.ErrorFields.Add("期間名稱", "請確認期間名稱不可為空！");
                        else
                            if (!校車時間紀錄資料庫[busyear].ContainsKey(value))
                                e.ErrorFields.Add("期間名稱", "請確認期間名稱是否有誤！");
                        break;
                    case "科別":
                        if (value.Length == 0)
                            e.ErrorFields.Add("科別", "請確認科別不可為空！");
                        else
                            if (!新生科別資料庫.Contains(value))
                                e.ErrorFields.Add("科別", "請確認科別是否有誤！");
                        break;
                    case "編號":
                        if (value.Length < 5)
                            e.ErrorFields.Add("編號", "請確認編號必須至少五碼！");
                        else
                            if (!學生資料庫.ContainsKey(value))
                                e.ErrorFields.Add("編號", "請確認編號是否為正確新生！");
                        break;
                    case "搭車年度":
                        int syear;
                        if (value.Length == 0)
                            e.ErrorFields.Add("搭車年度", "請確認搭車年度不可為空！");
                        else
                        {
                            if (!int.TryParse(value, out syear))
                                e.ErrorFields.Add("搭車年度", "請確認搭車年度是否有誤！");
                            else
                                if (!校車時間紀錄資料庫.ContainsKey(value))
                                    e.ErrorFields.Add("搭車年度", "請確認搭車年度是否有誤！");
                        }
                        break;
                    case "天數":
                        int daycount;
                        if (value.Length == 0)
                            e.ErrorFields.Add("搭車天數", "請確認搭車天數不可為空！");
                        else
                            if (!int.TryParse(value, out daycount))
                                e.ErrorFields.Add("搭車天數", "請確認搭車天數是否有誤！");
                        break;
                    case "車費":
                        int money;
                        if (value.Length > 0)
                            if (!int.TryParse(value, out money))
                                e.ErrorFields.Add("車費", "請確認車費是否有誤！");
                        break;
                    case "是否繳費":
                        if (value.Length > 0)
                            if (value != "是" && value != "否" && value != "Yes" && value != "No" && value != "True" && value != "False")
                                e.ErrorFields.Add("是否繳費", "請確認是否繳費需為是或否！");
                        break;
                    case "繳費日期":
                        DateTime day;
                        if (value.Length > 0)
                            if (!DateTime.TryParse(value, out day))
                                e.ErrorFields.Add("繳費日期", "請確認繳費日期是否有誤！");
                        break;
                    case "備註":
                        break;
                }
            }
        }

        static void iWizardBusNewStudent_ImportStart(object sender, EventArgs e)
        {
            foreach (Data.StudentByBus item in 新生搭車紀錄資料庫.Values)
            {
                //_DeletedBusStudents.Add(item);
            }
        }

        static void iWizardBusNewStudent_ImportPackage(object sender, ImportLibrary.ImportPackageEventArgs e)
        {
            List<Data.StudentByBus> importItems = new List<Data.StudentByBus>();
            foreach (var row in e.Items)
            {
                Data.StudentByBus importData = (row.ID != "") ? 新生搭車紀錄資料庫[新生資料庫[row["編號"]].UID] : new Data.StudentByBus();
                //if (_DeletedBusStudents.Contains(importData))
                //    _DeletedBusStudents.Remove(importData);
                string bustime = "";
                string busid = "";
                int daycount = 0;
                foreach (var field in e.ImportFields)
                {
                    string value = row[field];
                    if (field == "校車時段")
                        bustime = value;
                    if (field == "代碼")
                        busid = value;
                    if (field == "天數")
                        daycount = int.Parse(value);

                    switch (field)
                    {
                        default:
                            break;
                        case "校車時段":
                            importData.BusTimeName = value;
                            break;
                        case "代碼":
                            importData.BusStopID = value;
                            break;
                        case "期間名稱":
                            importData.BusRangeName = value;
                            break;
                        case "科別":
                            importData.ClassName = value;
                            break;
                        case "編號":
                            importData.StudentID = 新生資料庫[value].UID;
                            break;
                        case "搭車年度":
                            if (value.Length > 0)
                                importData.SchoolYear = int.Parse(value);
                            break;
                        case "天數":
                            if (value.Length > 0)
                                importData.DateCount = int.Parse(value);
                            break;
                        case "車費":
                            if (value.Length > 0)
                                importData.BusMoney = int.Parse(value);
                            else
                                importData.BusMoney = 0;
                            break;
                        case "是否繳費":
                            if (value.Length > 0)
                            {
                                if (value == "是" || value == "Yes" || value == "True")
                                    importData.PayStatus = true;
                                else
                                    importData.PayStatus = false;
                            }
                            else
                                importData.PayStatus = false;
                            break;
                        case "繳費日期":
                            if (value.Length > 0)
                                importData.PayDate = DateTime.Parse(value);
                            break;
                        case "備註":
                            importData.comment = value;
                            break;
                    }
                }
                int total = 校車站名資料庫[bustime][busid].BusMoney * daycount;
                int div_value = total / 10;
                if ((total - div_value * 10) < 5)
                    importData.BusMoney = div_value * 10;
                else
                    importData.BusMoney = div_value * 10 + 10;
                //importData.BusMoney = 校車站名資料庫[bustime][busid].BusMoney * daycount;
                importItems.Add(importData);
            }
            importItems.SaveAll();
        }


        static void iWizardBusNewStudent_ImportComplete(object sender, EventArgs e)
        {
            //foreach (var item in _DeletedBusStudents)
            //{
            //    item.Deleted = true;
            //}
            //_DeletedBusStudents.SaveAll();
        }

        static void iWizardBusStudentPayment_IdentifyRow(object sender, ImportLibrary.IdentifyRowEventArgs e)
        {
            if (搭車紀錄資料庫.ContainsKey(e.RowData["搭車系統編號"]))
                e.RowData.ID = 搭車紀錄資料庫[e.RowData["搭車系統編號"]].UID;
        }

        static void iWizardBusStudentPayment_ValidateRow(object sender, ImportLibrary.ValidateRowEventArgs e)
        {
            string busyear = "";
            string bustime = "";
            foreach (var field in e.SelectFields)
            {
                if (field == "")
                    continue;
                string value = e.Data[field];


                switch (field)
                {
                    default:
                    case "搭車天數":
                        int daycount;
                        if (value.Length == 0)
                            e.ErrorFields.Add("搭車天數", "請確認搭車天數不可為空！");
                        else
                            if (!int.TryParse(value, out daycount))
                                e.ErrorFields.Add("搭車天數", "請確認搭車天數是否有誤！");
                        break;
                    case "車費":
                        int money;
                        if (value.Length > 0)
                            if (!int.TryParse(value, out money))
                                e.ErrorFields.Add("車費", "請確認車費是否有誤！");
                        break;
                    case "是否繳費":
                        if (value.Length > 0)
                            if (value != "是" && value != "否" && value != "Yes" && value != "No" && value != "True" && value != "False")
                                e.ErrorFields.Add("是否繳費", "請確認是否繳費需為是或否！");
                        break;
                    case "繳費日期":
                        DateTime day;
                        if (value.Length > 0)
                            if (!DateTime.TryParse(value, out day))
                                e.ErrorFields.Add("繳費日期", "請確認繳費日期是否有誤！");
                        break;
                    case "備註":
                        break;
                }
            }
        }

        static void iWizardBusStudentPayment_ImportStart(object sender, EventArgs e)
        {
            foreach (string var in 搭車紀錄資料庫.Keys)
            {
                //foreach (Data.StudentByBus item in 搭車紀錄資料庫[var].Values)
                //    _DeletedBusStudents.Add(item);
            }
        }

        static void iWizardBusStudentPayment_ImportPackage(object sender, ImportLibrary.ImportPackageEventArgs e)
        {
            List<Data.StudentByBus> importItems = new List<Data.StudentByBus>();
            foreach (var row in e.Items)
            {
                Data.StudentByBus importData = new Data.StudentByBus();
                if (row.ID != "")
                    importData = 搭車紀錄資料庫[row.ID];
                //Data.StudentByBus importData = (row.ID != "") ? 學生搭車紀錄資料庫[學生資料庫[row["學號"]].ID][row["搭車年度"] + row["期間名稱"]] : new Data.StudentByBus();
                //if (_DeletedBusStudents.Contains(importData))
                //    _DeletedBusStudents.Remove(importData);
                string bustime = "";
                string busid = "";
                int daycount = 0;
                foreach (var field in e.ImportFields)
                {
                    string value = row[field];

                    switch (field)
                    {
                        default:
                            break;
                        case "搭車天數":
                            if (value.Length > 0)
                                importData.DateCount = int.Parse(value);
                            break;
                        case "車費":
                            if (value.Length > 0)
                                importData.BusMoney = int.Parse(value);
                            else
                                importData.BusMoney = 0;
                            break;
                        case "是否繳費":
                            if (value.Length > 0)
                            {
                                if (value == "是" || value == "Yes" || value == "True")
                                    importData.PayStatus = true;
                                else
                                    importData.PayStatus = false;
                            }
                            else
                                importData.PayStatus = false;
                            break;
                        case "繳費日期":
                            if (value.Length > 0)
                                importData.PayDate = DateTime.Parse(value);
                            break;
                        case "備註":
                            importData.comment = value;
                            break;
                    }
                }
                importItems.Add(importData);
            }
            importItems.SaveAll();
        }


        static void iWizardBusStudentPayment_ImportComplete(object sender, EventArgs e)
        {

        }

        static void rbBtnAssign_Click(object sender, EventArgs e)
        {
            AssignBus frm = new AssignBus();
            frm.ShowDialog();
        }

        static void rbBtnQuery_Click(object sender, EventArgs e)
        {
            //QueryHealthy frm = new QueryHealthy();
            //frm.ShowDialog();
        }

        //依時段、車號站序排序副程式
        static int CompareBusNumber(MdhsBus.Data.BusStop a, MdhsBus.Data.BusStop b)
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
    }
}
