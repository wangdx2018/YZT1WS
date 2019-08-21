using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Controls;
using AFC.WS.UI.Config;
using AFC.WS.UI.Common;
using System.Data;
using AFC.WS.UI.CommonControls;
using AFC.WorkStation.ExcelReport;
using ExcelApplication = Microsoft.Office.Interop.Excel.Application;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Threading;
using AFC.WS.Model.DB;
using System;


namespace AFC.WS.BR.ReportManager
{
    /// <summary>
    /// 报表管理帮助类(ReportManagerHelper)
    /// </summary>
    public class ReportManagerHelper : IReportPrint
    {
        /// <summary>
        /// 报表打印事件。
        /// </summary>
        public event ReportPrintEventHandle ReportPrintEvent;

        /// <summary>
        /// 打表打印事件方法
        /// </summary>
        /// <param name="sender">默认填写this</param>
        /// <param name="e">报表打印事件类</param>
        void ReportPrintEventMethod(object sender, ReportPrintEventArgs e)
        {
            if (ReportPrintEvent != null)
            {
                ReportPrintEvent(sender, e);
            }
        }

        #region --> 属性。

        /// <summary>
        /// 私有静态的实体类
        /// </summary>
        static ReportManagerHelper _Instance;

        /// <summary>
        /// 创建一个实例
        /// </summary>
        public static ReportManagerHelper Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ReportManagerHelper();
                }
                return _Instance;
            }
        }

        /// <summary>
        /// 获取所有报表类型
        /// </summary>
        List<BasiReportTypeInfo> _ReportTypeInfoItem;

        /// <summary>
        /// 获取所有报表子类型
        /// </summary>
        List<BasiReportSubTypeInfo> _ReportSubTypeInfoItem;

        /// <summary>
        /// 获取所有报表信息
        /// </summary>
        List<BasiReportInfo> _ReportInfoItem;

        /// <summary>
        /// 获取所有报表信息
        /// </summary>
        public List<BasiReportInfo> ReportInfoItem
        {
            get
            {
                try
                {
                    if (_ReportInfoItem == null || _ReportInfoItem.Count == 0)
                    {
                        String sqlQuery = string.Format("select * from basi_report_info t order by t.report_type_id,t.report_sub_type_id,t.report_name");
                        _ReportInfoItem = DBCommon.Instance.SetTModelValue<BasiReportInfo>(sqlQuery);
                    }
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
                return _ReportInfoItem;
            }
        }

        /// <summary>
        /// 获取所有报表子类型
        /// </summary>
        public List<BasiReportSubTypeInfo> ReportSubTypeInfoItem
        {
            get
            {
                try
                {
                    if (null == _ReportSubTypeInfoItem || _ReportSubTypeInfoItem.Count == 0)
                    {
                        String sqlQuery = String.Format("select * from basi_report_sub_type_info t order by t.report_type_id ,t.report_sub_type_id");
                        _ReportSubTypeInfoItem = DBCommon.Instance.SetTModelValue<BasiReportSubTypeInfo>(sqlQuery);
                    }
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
                return _ReportSubTypeInfoItem;
            }
        }

        public DataTable getDecimDeviceID(string stationId)
        {
            DataTable dtDevice = null;
            try
            {
                string sqlQuery = string.Format("select * from basi_dev_info t where t.device_type in ('01','02','06') where t.station_id='{0}'",stationId);
                int res = 0;
                DataSet dsDevice = Util.DataBase.SqlQuery(out res, sqlQuery);
                if (dsDevice.Tables.Count > 0)
                {
                    dtDevice = dsDevice.Tables[0];

                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            finally
            {

            }
            return dtDevice;
        }


        /// <summary>
        /// 获取所有报表类型
        /// </summary>
        public List<BasiReportTypeInfo> ReportTypeInfoItem
        {
            get
            {
                try
                {
                    if (_ReportTypeInfoItem == null || _ReportTypeInfoItem.Count == 0)
                    {
                        String sqlQuery = string.Format("select * from basi_report_type_info t order by t.report_type_id");
                        _ReportTypeInfoItem = DBCommon.Instance.SetTModelValue<BasiReportTypeInfo>(sqlQuery);
                    }
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
                return _ReportTypeInfoItem;
            }
        }

        /// <summary>
        /// 获取所有自动打印报表的记录信息。
        /// </summary>
        /// <returns></returns>
        public List<RptAutoPrintInfo> ReportAutoPrintInfoItem()
        {
            try
            {
                string sqlQuery = string.Format("select * from rpt_auto_print_info");

                return DBCommon.Instance.SetTModelValue<RptAutoPrintInfo>(sqlQuery);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return null;
        }

        ExcelApplication excel = null;

        /// <summary>
        /// 复制后报表的路径
        /// </summary>
        public string LocationReportFilePath = "";

        /// <summary>
        /// 服务器报表路径
        /// </summary>
        public string ServerReportFilePath = "";

        #endregion --> 属性。

        /// <summary>
        /// 清理此类对应的所有数据内容。
        /// </summary>
        public void Clear()
        {
            LocationReportFilePath = "";
            ServerReportFilePath = "";

            if (_ReportSubTypeInfoItem != null)
            {
                _ReportSubTypeInfoItem.Clear();
            }
            if (null != _ReportInfoItem)
            {
                _ReportInfoItem.Clear();
            }
            if (null != _ReportTypeInfoItem)
            {
                _ReportTypeInfoItem.Clear();
            }
        }

        #region --> 类。

        /// <summary>
        /// 获取报表子类型
        /// </summary>
        /// <param name="reportTypeId">报表类型ID</param>
        /// <returns>返回报表子类型</returns>
        public List<BasiReportSubTypeInfo> ReportSubTypeInfoItemByReportTypeId(decimal reportTypeId)
        {
            List<BasiReportSubTypeInfo> item = ReportSubTypeInfoItem;

            if (null != item && item.Count > 0)
            {
                return item.Where(p => p.report_type_id == reportTypeId).GetListTContext<BasiReportSubTypeInfo>();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取报表信息
        /// </summary>
        /// <param name="reportTypeId">报表类型ID</param>
        /// <param name="subTypeId">报表子类型ID</param>
        /// <returns></returns>
        public List<BasiReportInfo> ReportInfoItemByReportTypeIDAndSubTypeID(decimal reportTypeid, decimal subTypeId)
        {
            List<BasiReportInfo> item = this.ReportInfoItem;
            if (null != item && item.Count > 0)
            {
                return item.Where(p => p.report_type_id == reportTypeid &&
                    p.report_sub_type_id == subTypeId).GetListTContext<BasiReportInfo>();
            }
            return null;
        }

        /// <summary>
        /// 获取报表子类型
        /// </summary>
        /// <param name="reportTypeId">报表类型ID</param>
        /// <returns>返回报表子类型</returns>
        public List<BasiReportSubTypeInfo> ReportSubTypeInfoItemByReportTypeId1(decimal reportTypeId)
        {
            List<BasiReportSubTypeInfo> item = ReportSubTypeInfoItem;

            if (null != item && item.Count > 0)
            {
                return item.Where(p => p.report_type_id == reportTypeId && p.report_sub_type_id != 1).GetListTContext<BasiReportSubTypeInfo>();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取报表信息
        /// </summary>
        /// <param name="reportTypeId">报表类型ID</param>
        /// <param name="subTypeId">报表子类型ID</param>
        /// <returns></returns>
        public List<BasiReportInfo> ReportInfoItemByReportTypeIDAndSubTypeID1(decimal reportTypeid, decimal subTypeId)
        {
            List<BasiReportInfo> item = this.ReportInfoItem;
            if (null != item && item.Count > 0)
            {
                return item.Where(p => p.report_type_id == reportTypeid &&
                    p.report_sub_type_id == subTypeId && p.report_frequency_type != "01").GetListTContext<BasiReportInfo>();
            }
            return null;
        }

        /// <summary>
        /// 获取报表信息
        /// </summary>
        /// <param name="reportName">报表名称</param>
        /// <param name="reportTypeId">报表类型ID</param>
        /// <param name="subTypeId">报表子类型ID</param>
        /// <returns></returns>
        public BasiReportInfo GetReportInfoByFK(string reportName, decimal reportTypeId, decimal subTypeId)
        {
            List<BasiReportInfo> item = this.ReportInfoItem;
            if (null != item && item.Count > 0)
            {
                return item.Where(p => p.report_type_id == reportTypeId &&
                    p.report_sub_type_id == subTypeId &&
                    p.report_name == reportName).GetTContext<BasiReportInfo>();
            }
            return null;
        }

        /// <summary>
        /// 获取报表类型信息
        /// </summary>
        /// <param name="reportTypeId">报表类型ID</param>
        /// <returns></returns>
        public BasiReportTypeInfo GetReportTypeInfoByPK(decimal reportTypeId)
        {
            List<BasiReportTypeInfo> item = this.ReportTypeInfoItem;
            if (item != null && item.Count > 0)
            {
                return item.Where(p => p.report_type_id == reportTypeId).GetTContext<BasiReportTypeInfo>();
            }
            return null;
        }

        /// <summary>
        /// 获取报表子类型信息
        /// </summary>
        /// <param name="reportTypeId">报表类型ID</param>
        /// <param name="reportSubTypeId">报表子类型ID</param>
        /// <returns></returns>
        public BasiReportSubTypeInfo GetReportSubTypeInfoByPK(decimal reportTypeId, decimal reportSubTypeId)
        {
            List<BasiReportSubTypeInfo> item = this.ReportSubTypeInfoItem;
            if (item != null && item.Count > 0)
            {
                return item.Where(p => p.report_type_id == reportTypeId &&
                    p.report_sub_type_id == reportSubTypeId).GetTContext<BasiReportSubTypeInfo>();
            }
            return null;
        }

        #endregion --> 类。

        /// <summary>
        /// 报表创建
        /// </summary>
        /// <param name="reportName">创建名称</param>
        /// <param name="pc">创建报表的时候所传入的参数条件</param>
        /// <returns>true-创建成功；false-创建失败</returns>
        public bool ReportCreate(string reportName, ParamCondition pc)
        {
            this.ReportPrintEventMethod(this, new ReportPrintEventArgs(pc, "准备创建[" + reportName + "]报表..."));

            ReportGen gen = GetReportGen(pc);

            LocationReportFilePath = "";
            string filePath = GetReportSavePath(reportName, pc);
            if (filePath.JudgeIsNullOrEmpty())
            {
                return false;
            }

            gen.db = Util.DataBase;
            if (excel != null)
            {
                try
                {
                    excel.DisplayAlerts = false;
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine("Test for Excel instance Failed, will reopen Excel.", LogFlag.InfoFormat);
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                    excel = null;
                }
            }
            //dusj modify begin 业主要求同时打开多个报表
            //if (excel == null)
            //{
                excel = new ExcelApplication();
            //}
                //dusj modify end 业主要求同时打开多个报表

           // gen.ex = excel;

            ReportPrintEventArgs e = new ReportPrintEventArgs();

            #region --> 1、创建报表。

            try
            {
                e.MessageContent = "正在创建[" + reportName + "]报表......";
                e.Source = gen;
                e.Status = ReportStatus.CreatingReport;
                this.ReportPrintEventMethod(this, e);
                gen.LoadReportNoOpen(filePath, true);
                LocationReportFilePath = filePath;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                e.MessageContent = "创建[" + reportName + "]报表失败，失败原因是：" + ee.Message; //"创建[" + reportName + "]报表出现异常，请联系维护人员。";// 
                e.Source = ee;
                e.IsComplete = true;
                e.Status = ReportStatus.CreateReportError;
                this.ReportPrintEventMethod(this, e);
                return false;
            }

            #endregion --> 1、创建报表。

            #region --> 2、打印报表。

            int result = -1;
            try
            {
                e.Source = gen;
                e.MessageContent = "正在打印[" + reportName + "]报表......";
                e.Status = ReportStatus.PrintingReport;

                this.ReportPrintEventMethod(this, e);
                result = ReportPrint(LocationReportFilePath);
                if (result > 0)
                {
                    e.MessageContent = "完成[" + reportName + "]报表打印......";
                    e.Status = ReportStatus.PrintReportSuccess;
                    SaveGenReport(reportName, pc);
                    this.ReportPrintEventMethod(this, e);
                }
                else
                {
                    e.MessageContent = "[" + reportName + "]报表打印失败......";
                    e.Status = ReportStatus.PrintReportFailed;

                    this.ReportPrintEventMethod(this, e);
                }
                return true;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                e.MessageContent = "打印[" + reportName + "]报表失败，失败原因是：" + ee.Message;
                e.Status = ReportStatus.PrintingReportError;

                e.Source = ee;
                e.IsComplete = true;
                this.ReportPrintEventMethod(this, e);
                return false;
            }
            #endregion --> 2、打印报表。

            //#region --> 3、上传报表。
            //try
            //{
            //    e.MessageContent = "开始上传[" + reportName + "]报表到服务器......";
            //    e.Status = ReportStatus.UpLoadingReport;

            //    e.Source = gen;
            //    this.ReportPrintEventMethod(this, e);
            //    //--上传到服务器。
            //    result = ReportSave(pc.HistoryInfo.report_save_name, BuinessRule.GetInstace().brConext.FtpAddress);
            //    if (result == 0)
            //    {
            //        pc.HistoryInfo.report_path = this.ServerReportFilePath;
            //        AddReportHistoryInfo(pc.HistoryInfo);                   //-->添加记录到数据库当中
            //        e.MessageContent = "[" + reportName + "]上传完成......";
            //        e.IsComplete = true;
            //        e.Status = ReportStatus.UpLoadReportSuccess;

            //        this.ReportPrintEventMethod(this, e);
            //    }
            //    else
            //    {
            //        e.MessageContent = "[" + reportName + "]上传失败......";
            //        e.IsComplete = true;
            //        e.Status = ReportStatus.UpLoadReportFailed;

            //        this.ReportPrintEventMethod(this, e);
            //    }
            //}
            //catch (Exception ee)
            //{
            //    e.MessageContent = "[" + reportName + "]上传失败,失败原因:" + ee.Message;
            //    e.Source = ee;
            //    e.IsComplete = true;
            //    e.Status = ReportStatus.UpLoadingReportError;

            //    this.ReportPrintEventMethod(this, e);
            //}
            //return true;
            //#endregion --> 3、上传报表。
        }

        private ReportGen GetReportGen(ParamCondition pc)
        {
            ReportGen gen = new ReportGen();

            //线路车站信息。
            gen.AddParam("LineId", pc.LineID);
            gen.AddParam("LineName", pc.LineName);
            gen.AddParam("StationID", pc.StationID);
            gen.AddParam("StationName", pc.StationName);
            //系统时间。
            gen.AddParam("SystemDateTime", pc.SystemDateTime);
            //客流时间段。
            gen.AddParam("TimeInterval", pc.TimeInterval);
            //Biss_Date
            gen.AddParam("BissDateEnd", pc.TimeScope.BissDateEnd);
            gen.AddParam("BissDateBegin", pc.TimeScope.BissDateBegin);
            //Run_Date
            gen.AddParam("RunDateEnd", pc.TimeScope.RunDateEnd);
            gen.AddParam("RunDateBegin", pc.TimeScope.RunDateBegin);
            //Report_Date
            gen.AddParam("ReportDateEnd", pc.TimeScope.ReportDateEnd);
            gen.AddParam("ReportDateBegin", pc.TimeScope.ReportDateBegin);
            //Tran_Date
            gen.AddParam("TranDateEnd", pc.TimeScope.TranDateEnd);
            gen.AddParam("TranDateBegin", pc.TimeScope.TranDateBegin);
            gen.AddParam("TranDate", pc.TimeScope.TranDate);
            //time
            gen.AddParam("TimeEnd", pc.TimeScope.TimeEnd);
            gen.AddParam("TimeBegin", pc.TimeScope.TimeBegin);
            gen.AddParam("RunDate", pc.TimeScope.RunDate);
            //班次
            gen.AddParam("ClassTimeName", pc.ClassTimeName);
            gen.AddParam("ClassTimeId", pc.ClassTimeId);

            //年、月。
            gen.AddParam("Year", pc.TimeScope.Year);
            gen.AddParam("Month", pc.TimeScope.Month);
            //交易类型
            gen.AddParam("BissType", pc.BissType);
            gen.AddParam("BissTypeName", pc.BissTypeName);
            //交易子类型
            gen.AddParam("BissSubType", pc.BissSubType);
            gen.AddParam("BissSubTypeName", pc.BissSubTypeName);
            //操作员ID
            gen.AddParam("OperatorId", pc.OperatorId);
            gen.AddParam("OperatorIdName", pc.OperatorIdName);
            //登录操作员ID
            gen.AddParam("LoginOperatorId", BuinessRule.GetInstace().brConext.CurrentOperatorId);
            //发行商ID
            gen.AddParam("CardIssuerId", pc.CardIssuerId);
            //票卡ID
            gen.AddParam("ProductTypeId", pc.ProductTypeId);
            //库存管理类型
            gen.AddParam("ProductTypeName", pc.ProductTypeName);
            //设备编码
            gen.AddParam("DeviceID", pc.DeviceID);
            //设备代码
            gen.AddParam("DeviceCode", pc.DeviceCode);
            //统计开始时间
            gen.AddParam("StatisticsBeginDatetime", pc.StatisticsBeginDatetime);
            //统计结束时间
            gen.AddParam("StatisticsEndDatetime", pc.StatisticsEndDatetime);
            //统计时间段:2010-11-01 09:20:10 ~ 2010-11-03 10:10:10
            gen.AddParam("StatisticsDateTimeScope", pc.StatisticsDateTimeScope);
            //
            gen.AddParam("DeviceTypeName", pc.DeviceTypeName);
            //
            gen.AddParam("DeviceType", pc.DeviceType);

            try
            {
                // 测试数据是1；正常数据为 0。
                if (SysConfig.GetSysConfig().ReportCfg.IsStatisticsTestData)
                {
                    gen.AddParam("IsTestData", "1");
                }
                else
                {
                    gen.AddParam("IsTestData", "0");
                }
            }
            catch (Exception ee)
            {
                gen.AddParam("IsTestData", "0");
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return gen;
        }

        /// <summary>
        /// 获取报表保存路径。
        /// </summary>
        /// <param name="reportName">报表名称</param>
        /// <param name="pc">参数条件</param>
        /// <returns>返回报表路径</returns>
        private string GetReportSavePath(string reportName, ParamCondition pc)
        {
            string path = "";
            string path1 = "";
            if (!Directory.Exists(pc.ReportSaveFilePath))
            {
                Directory.CreateDirectory(pc.ReportSaveFilePath);
            }
            string sourceFileName = pc.LocationTemplateFilePath;

            /*string tempReportName1 = pc.ReportName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                + "." + pc.HistoryInfo.report_param_value
                + ".xls";*/
            string tempReportName1 = pc.ReportName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                + ".xls";

            string tempReportName = pc.ReportName + ".xls";
            string directTemp = pc.ReportSaveFilePath;
            string destFileName = pc.ReportSaveFilePath + "\\" + tempReportName;
            string destFileName1 = directTemp + "\\" + tempReportName1;
            if (File.Exists(sourceFileName))
            {
                try
                {
                    if (!Directory.Exists(directTemp))
                    {
                        Directory.CreateDirectory(directTemp);
                    }
                    //File.Copy(sourceFileName, destFileName,true);
                    File.Copy(sourceFileName, destFileName1, true);
                    //path = destFileName;
                    path1 = destFileName1;
                    pc.HistoryInfo.report_save_name = tempReportName1;
                }
                catch (Exception ee)
                {
                    this.ReportPrintEventMethod(this, new ReportPrintEventArgs(this, "获取报表路径出错:" + ee.Message, true));
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
            }
            else
            {
                this.ReportPrintEventMethod(this, new ReportPrintEventArgs(this, "报表路径与实际不一致", true));
            }
            return path1;
        }

        /// <summary>
        /// 按照日期时间车站信息保存报表
        /// </summary>
        /// <param name="reportName">报表名称</param>
        /// <param name="pc"></param>
        private void SaveGenReport(string reportName, ParamCondition pc)
        {
            string path = "";
            if (!Directory.Exists(pc.ReportSaveFilePath))
            {
                Directory.CreateDirectory(pc.ReportSaveFilePath);
            }
            string sourceFileName = "";//pc.LocationTemplateFilePath;

            string tempReportName = pc.ReportName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss")
                + "." + pc.HistoryInfo.report_param_value
                + ".xls";

            string tempReportName1 = pc.ReportName + ".xls";
            string directTemp = pc.ReportSaveFilePath + "/temp";
            string destFileName = pc.ReportSaveFilePath + "\\" + tempReportName;
            sourceFileName = directTemp + "\\" + tempReportName1;
            if (File.Exists(sourceFileName))
            {
                try
                {
                    File.Copy(sourceFileName, destFileName, true);
                }
                catch (Exception ee)
                {
                    this.ReportPrintEventMethod(this, new ReportPrintEventArgs(this, "SaveGenReport复制报表异常:" + ee.Message, true));
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
            }
            else
            {
                this.ReportPrintEventMethod(this, new ReportPrintEventArgs(this, "报表路径与实际不一致", true));
            }

        }

        /// <summary>
        /// 报表打印
        /// </summary>
        /// <param name="reportPath">报表路径</param>
        /// <returns></returns>
        public int ReportPrint(string reportPath)
        {
            if (SysConfig.GetSysConfig().ReportCfg.IsAutoPrint)
            {
                try
                {
                    Type ExcelType = Type.GetTypeFromProgID("Excel.Application");
                    object ExcelApplication = Activator.CreateInstance(ExcelType);
                    object oBook;
                    object fileName = reportPath;
                    object readOnly = true;
                    object missing = System.Reflection.Missing.Value;
                    object[] oParams = new object[1];

                    //获取Excel的工作界面
                    object oDocs = ExcelApplication.GetType().InvokeMember("Workbooks", BindingFlags.GetProperty, null, ExcelApplication, null, CultureInfo.InvariantCulture);
                    oParams = new object[3];
                    oParams[0] = fileName;
                    oParams[1] = missing;
                    oParams[2] = readOnly;

                    //打开第一张工作界面
                    oBook = oDocs.GetType().InvokeMember("Open", BindingFlags.InvokeMethod, null, oDocs, oParams, CultureInfo.InvariantCulture);
                    //开始打印
                    oBook.GetType().InvokeMember("PrintOut", BindingFlags.InvokeMethod, null, oBook, null, CultureInfo.InvariantCulture);
                    //退出Excel
                    ExcelApplication.GetType().InvokeMember("Quit", BindingFlags.InvokeMethod, null, ExcelApplication, null, CultureInfo.InvariantCulture);

                    return 1;

                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }

                return -1;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 报表保存到服务器上去
        /// 如果 result >0 上传成功。
        /// </summary>
        /// <param name="ReportName">报表名称</param>
        /// <param name="SavePath">保存路径</param>
        /// <returns>如果 result >0 上传成功。</returns>
        public int ReportSave(string ReportName, string SavePath)
        {
            if (SysConfig.GetSysConfig().ReportCfg.IsAutoSaveToServer)
            {
                FTPCommon ftpCommon=new FTPCommon(BuinessRule.GetInstace().brConext.FtpUser,
                    BuinessRule.GetInstace().brConext.FtpPwd,
                    BuinessRule.GetInstace().brConext.FtpAddress);
                int result = ftpCommon.FTPUpLoad(LocationReportFilePath, SavePath, ReportName);
                this.ServerReportFilePath = SavePath + "/" + ReportName;
                return result;
            }
            else
            {
                this.ServerReportFilePath = SavePath + "/" + ReportName;
                return 1;
            }
        }

        /// <summary>
        /// 打开报表
        /// </summary>
        /// <param name="ReportName">报表名称</param>
        /// <param name="ReportPath">报表路径</param>
        public void ReportOpen(string ReportName, string ReportPath)
        {
            try
            {
                if (string.IsNullOrEmpty(ReportPath))
                {
                    return;
                }
                this.ReportPrintEventMethod(this, new ReportPrintEventArgs(ReportPath, "正在打开[" + ReportName + "]报表...", true));
                System.Diagnostics.Process.Start(ReportPath);
                this.ReportPrintEventMethod(this, new ReportPrintEventArgs(ReportPath, "打开[" + ReportName + "]报表...", true));
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        #region --> 报表的自动打印。

        /// <summary>
        /// 自动报表打印
        /// </summary>
        /// <param name="pcItem">要打印的报表列表</param>
        /// <returns></returns>
        public bool ReportAutoPrint(List<ParamCondition> pcItem)
        {
            ReportPrintEventArgs e = new ReportPrintEventArgs();
            e.Source = pcItem;
            e.MessageContent = "开始进行报表自动打印...";
            e.IsBatchPrintComplete = false;
            if (pcItem == null || pcItem.Count == 0)
            {
                return false;
            }
            bool Result = true;
            int counter = 0;
            int errorCounter = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var v in pcItem)
            {
                //-->1、创建报表。
                bool result = ReportCreate(v.ReportName, v);
                if (!result)
                {
                    sb.Append(v.ReportName).Append("\r\n");
                    errorCounter++;
                    continue;
                }
                counter++;
            }
            e.IsBatchPrintComplete = true;
            if (counter == 0)
            {
                e.MessageContent = "一张报表都没有创建打印出来...";
                Result = false;
                this.ReportPrintEventMethod(this, e);
            }
            else if (counter < pcItem.Count)
            {
                string content = sb.ToString();
                e.MessageContent = "共有[" + errorCounter + "]张报表打印出错，分别是：" + content;
                this.ReportPrintEventMethod(this, e);
            }
            else
            {
                e.MessageContent = "成功创建[" + counter + "]张报表...";
                this.ReportPrintEventMethod(this, e);
            }
            return Result;
        }

        /// <summary>
        /// 自动报表打印
        /// </summary>
        public void ReportAutoPrint()
        {
            ReportAutoPrint(BatchAutoPrint());
        }
        /// <summary>
        /// 定时自动打印
        /// </summary>
        public void TimeReportAutoPrint()
        {
            try
            {
                while (true)
                {
                    string printTimes = DateTime.Now.ToString("yyyyMMdd") + SysConfig.GetSysConfig().ReportCfg.AutoPrintTime;

                    DateTime printDateTime = DateTime.ParseExact(printTimes, "yyyyMMddHH:mm:ss", null);
                    double ts = printDateTime.Subtract(DateTime.Now).TotalSeconds;
                    if (Math.Abs(ts) < 5)
                    {
                        ReportAutoPrint();
                    }

                    Thread.Sleep(5 * 1000);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        private Thread autoPrintReportThread; //= new Thread();
        /// <summary>
        /// 启动自动打印线程
        /// </summary>
        /// <returns></returns>
        public int StartAutoReportPrintThread()
        {
            try
            {

                this.autoPrintReportThread = new Thread(new ThreadStart(this.TimeReportAutoPrint));
                this.autoPrintReportThread.Name = "autoStartThread";
                this.autoPrintReportThread.Start();
                return 0;
            }
            catch (Exception ex)
            {
                //WriteLog.Log_Error("thread start error! name=[" + this.autoPrintReportThread.Name + "]" + ex.Message);
                Wrapper.Instance.ConsoleWriteLine(ex, LogFlag.ErrorFormat);
                return -1;
            }
        }
        /// <summary>
        /// 停止自动打印线程
        /// </summary>
        /// <returns></returns>
        public int AbortAutoReportPrint()
        {
            if (this.autoPrintReportThread != null
                && this.autoPrintReportThread.ThreadState == ThreadState.Running)
            {
                try
                {
                    this.autoPrintReportThread.Abort();
                    return 0;
                }
                catch (Exception ex)
                {
                    //WriteLog.Log_Error("thread abort error! name=[" + this.autoPrintReportThread.Name + "]" + ex.Message);
                    Wrapper.Instance.ConsoleWriteLine(ex, LogFlag.ErrorFormat);
                    return -1;
                }
            }
            return -1;
        }

        /// <summary>
        /// 批量打印时加载的参数信息。
        /// </summary>
        /// <returns></returns>
        List<ParamCondition> BatchAutoPrint()
        {
            try
            {
                List<RptAutoPrintInfo> autoPrintList = ReportAutoPrintInfoItem();
                if (autoPrintList != null)
                {
                    List<ParamCondition> pcItem = new List<ParamCondition>();

                    string runDate = "";
                    try
                    {
                        runDate = DateTime.Parse(BuinessRule.GetInstace().rm.GetRunDate()).ToString("yyyyMMdd");
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }

                    foreach (var v in autoPrintList)
                    {
                        ParamCondition pc = new ParamCondition();

                        DateTimeScope dts = new DateTimeScope();
                        dts.TranDate = DateTime.Now.ToString("yyyyMMdd");
                        dts.RunDate = runDate;
                        pc.ClassTimeId = "%";
                        pc.ClassTimeName = "全部";
                        BasiReportInfo ri = GetReportInfoByFK(v.report_name, v.report_type_id, v.report_sub_type_id);
                        switch (ri.report_frequency_type)
                        {
                            case "01":  //01-时实
                                continue;

                            case "02":  //02-日报
                                dts.TranDateBegin = DateTime.Now.ToString("yyyyMMdd");
                                dts.TranDateEnd = DateTime.Now.ToString("yyyyMMdd");
                                dts.RunDateBegin = dts.TranDateBegin;
                                dts.RunDateEnd = dts.TranDateEnd;

                                break;
                            case "03":  //03-周报
                                int week = GetWeekOfYear(DateTime.Now);
                                int daysBegin = (week - 2) * 7 - DateTime.Now.DayOfYear + 3;
                                int daysEnd = (week - 1) * 7 - DateTime.Now.DayOfYear + 2;
                                DateTime dtWeekBegin = DateTime.Now.AddDays(daysBegin);
                                DateTime dtWeekEnd = DateTime.Now.AddDays(daysEnd);
                                dts.TranDateBegin = dtWeekBegin.ToString("yyyyMMdd");
                                dts.TranDateEnd = dtWeekEnd.ToString("yyyyMMdd");

                                break;
                            case "04":  //04-月报

                                int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                                dts.TranDateBegin = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + "01";
                                dts.TranDateEnd = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("00") + days.ToString("00");

                                break;
                            case "05":  //05-年报
                                dts.TranDateBegin = DateTime.Now.Year.ToString() + "0101";
                                dts.TranDateEnd = DateTime.Now.Year.ToString() + "1231";

                                break;
                            default:
                                break;
                        }//End swtich;

                        pc.LineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                        pc.LineName = BuinessRule.GetInstace().GetLineInfoById(pc.LineID).line_name;
                        if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName == "SCWS")
                        {
                            pc.StationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                            pc.StationName = BuinessRule.GetInstace().GetStationInfoById(pc.StationID).station_cn_name;
                        }
                        else
                        {
                            pc.StationID = "%";
                        }
                        pc.SystemDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                        pc.ReportName = v.report_name;
                        pc.TimeInterval = 30;

                        dts.RunDateBegin = dts.TranDateBegin;
                        dts.RunDateEnd = dts.TranDateEnd;
                        dts.TimeBegin = "000000";
                        dts.TimeEnd = "235959";
                        pc.TimeScope = dts;

                        RptHistoryInfo rhi = new RptHistoryInfo();
                        rhi.report_add_date = DateTime.Now.ToString("yyyyMMdd");
                        rhi.report_add_time = DateTime.Now.ToString("HHmmss");
                        rhi.report_name = v.report_name;
                      //  rhi.report_path = BRContext.CommonConfig.FtpDirectory.ReportPath + "/" + rhi.report_save_name;
                        rhi.report_sub_type_id = ri.report_sub_type_id;
                        rhi.report_type_id = ri.report_type_id;
                        rhi.station_id = pc.StationID;
                        rhi.report_condition_param = ri.report_condition_param;
                        string paramValue = GetControlvalue(pc, ri.report_condition_param);
                        rhi.report_param_value = paramValue.Substring(0, paramValue.LastIndexOf("_"));

                        //-->获取文件路径。 第一获取报表类型、第二获取报表子类型、第三就是报表名称。
                        BasiReportTypeInfo rti = GetReportTypeInfoByPK(ri.report_type_id);
                        string reportTypeName = rti.report_type_name;
                        BasiReportSubTypeInfo rsti = Instance.GetReportSubTypeInfoByPK(ri.report_type_id, ri.report_sub_type_id);
                        string reportSubTypeName = rsti.report_sub_type_name;

                        pc.LocationTemplateFilePath = Environment.CurrentDirectory +
                           SysConfig.GetSysConfig().ReportCfg.LocalReportPath +      // "\\ReportDir\\" +
                            reportTypeName + "\\" +
                            reportSubTypeName + "\\" +
                            ri.report_name + ".xls";

                        if (!string.IsNullOrEmpty(SysConfig.GetSysConfig().ReportCfg.AutoPrintReportPath))
                        {
                            pc.ReportSaveFilePath = SysConfig.GetSysConfig().ReportCfg.AutoPrintReportPath;

                            if (!SysConfig.GetSysConfig().ReportCfg.AutoPrintReportPath.Contains(':'))
                            {
                                pc.ReportSaveFilePath = Environment.CurrentDirectory + SysConfig.GetSysConfig().ReportCfg.AutoPrintReportPath;
                            }
                        }
                        else
                        {
                            pc.ReportSaveFilePath = Environment.CurrentDirectory + @"/Report/AutoSaverPath/";
                        }
                        if (!Directory.Exists(pc.ReportSaveFilePath))
                        {
                            WriteLog.Log_Info("您设置的路径不存在，显示的路径为默认路径，有任何疑问请联系维护人员!" + pc.ReportSaveFilePath);
                            pc.ReportSaveFilePath = Environment.CurrentDirectory + @"/Report/AutoSaverPath/";
                        }
                        pc.BissSubType = "%";
                        pc.BissType = "%";
                        pc.BissSubTypeName = "全部";
                        pc.BissTypeName = "全部";

                        pc.ProductTypeId = "%";
                        pc.ProductTypeName = "全部";
                        pc.CardIssuerId = "%";

                        pc.DeviceID = "全部";

                        pc.ReportInfo = ri;
                        pc.HistoryInfo = rhi;
                        pc.StatisticsDateTimeScope = DateTimeFormat(dts.TranDateBegin).ToyyyyMMdd() + "~" + DateTimeFormat(dts.TranDateEnd).ToyyyyMMdd();
                        pcItem.Add(pc);
                    }//End foreach;
                    return pcItem;
                }//
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }

            return null;
        }

        /// <summary>
        /// 字符串日期格式化
        /// </summary>
        /// <param name="value">字符串日期</param>
        /// <returns>格式化后的字符串</returns>
        public string DateTimeFormat(string value)
        {
            StringBuilder dateTime = new StringBuilder(value as string);

            if (dateTime != null)
            {
                if (dateTime.Length == 8)
                {
                    dateTime.Insert(4, "年");
                    dateTime.Insert(7, "月");
                    dateTime.Insert(10, "日");
                    return dateTime.ToString();
                }
                else if (dateTime.Length == 6)
                {
                    dateTime.Insert(2, "时");
                    dateTime.Insert(5, "分");
                    dateTime.Insert(8, "秒");
                    return dateTime.ToString();
                }

            }
            return null;
        }

        /// <summary>
        /// 获取当前在本年是第几周。
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>返回第几周</returns>
        public int GetWeekOfYear(DateTime dt)
        {
            CultureInfo myCI = new CultureInfo("zh-cn");
            Calendar myCal = myCI.Calendar;

            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            return myCal.GetWeekOfYear(dt, myCWR, myFirstDOW);
        }

        #endregion --> 报表的自动打印

        /// <summary>
        /// 添加自动打印报表
        /// true-添加成功；false-添加失败。
        /// </summary>
        /// <param name="ReportInfo">添加自动打印报表记录</param>
        /// <returns>true-添加成功；false-添加失败</returns>
        public bool AddAutoPrintReport(RptAutoPrintInfo autoInfo)
        {
           

            int result = DBCommon.Instance.InsertTable<RptAutoPrintInfo>(autoInfo, "rpt_auto_print_info");
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断自动打印报表是否存在
        /// </summary>
        /// <param name="ReportInfo">自动打印报表</param>
        /// <returns>true-已经存在；false-不存在。</returns>
        public bool JudgeAutoPrintReportIsExist(RptAutoPrintInfo autoInfo)
        {
            String sqlString = String.Format("select * from  rpt_auto_print_info t where");
            sqlString += String.Format(" t.report_type_id = '{0}'", autoInfo.report_type_id);
            sqlString += String.Format(" and t.report_sub_type_id = '{0}'", autoInfo.report_sub_type_id);
            sqlString += String.Format(" and t.report_name = '{0}'", autoInfo.report_name);

            DataTable dt = DBCommon.Instance.GetDatatable(sqlString);
            if (dt != null && dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加报表历史记录
        /// </summary>
        /// <param name="content">报表历史信息类</param>
        /// <returns>true-成功;false-失败。</returns>
        public bool AddReportHistoryInfo(RptHistoryInfo content)
        {
            int result=DBCommon.Instance.InsertTable<RptHistoryInfo>(content, "rpt_history_info");
            if (result > 0)
            {
                return true;
            }
            return false;
        }

        #region --> TreeView

        /// <summary>
        /// 设置TreeView报表类型
        /// <summary>
        /// <param name="tvItem">报表类型控件</param>
        /// <param name="item">报表类型记录集合</param>
        public void SetTreeViewReportTypeInfo(TreeView tvItem, List<BasiReportTypeInfo> item)
        {
            if (null == tvItem || null == item || item.Count == 0)
            {
                return;
            }
            tvItem.Items.Clear();
            try
            {
                foreach (BasiReportTypeInfo r in item)
                {
                    TreeViewItem temp = new TreeViewItem();
                    temp.Header = r.report_type_name;
                    temp.Uid = r.report_type_id.ToString();
                    temp.Tag = r;
                    temp.IsExpanded = true;
                    try
                    {
                        List<BasiReportSubTypeInfo> riItem = this.ReportSubTypeInfoItemByReportTypeId(r.report_type_id);
                        SetTreeViewReportSubTypeInfo(temp, riItem);
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    tvItem.Items.Add(temp);
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 设置TreeView报表子类型
        /// <summary>
        /// <param name="parent">报表子类型</param>
        /// <param name="item">报表子类型记录集合</param>
        public void SetTreeViewReportSubTypeInfo(TreeViewItem parent, List<BasiReportSubTypeInfo> item)
        {
            if (null == parent || null == item || item.Count == 0)
            {
                return;
            }

            try
            {
                foreach (BasiReportSubTypeInfo r in item)
                {
                    TreeViewItem temp = new TreeViewItem();
                    temp.Header = r.report_sub_type_name;
                    temp.Uid = r.report_type_id + "," + r.report_sub_type_id;
                    temp.Tag = r;
                    try
                    {
                        List<BasiReportInfo> riItem = this.ReportInfoItemByReportTypeIDAndSubTypeID(r.report_type_id, r.report_sub_type_id);
                        SetTreeViewReportInfo(temp, riItem);
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                    parent.Items.Add(temp);
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 设置TreeView报表信息
        /// <summary>
        /// <param name="parent">报表信息控件</param>
        /// <param name="item">报表记录集合</param>
        public void SetTreeViewReportInfo(TreeViewItem parent, List<BasiReportInfo> item)
        {
            if (null == parent || null == item || item.Count == 0)
            {
                return;
            }
            try
            {
                foreach (BasiReportInfo r in item)
                {
                    TreeViewItem temp = new TreeViewItem();
                    temp.Header = r.report_name;
                    temp.Uid = r.report_type_id + "," + r.report_sub_type_id + "," + r.report_name;
                    temp.Tag = r;
                    parent.Items.Add(temp);
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        #endregion --> TreeView

        #region --> 对Grid 里的控件进行操作。

        /// <summary>
        /// 初始化控件Enabled是否可用。
        /// </summary>
        /// <param name="g">Grid控件</param>
        /// <param name="enabled">true-可用；false-不可用</param>
        public void InitControlIsEnabled(Grid g, bool enabled)
        {
            try
            {
                if (g == null)
                {
                    return;
                }
                foreach (var v in g.Children)
                {
                    if (v is Grid)
                    {
                        Grid temp = v as Grid;
                        if (temp.Children.Count > 0)
                        {
                            InitControlIsEnabled(temp, enabled);
                        }
                    }
                    else if (v is GroupBox)
                    {
                        GroupBox gb = v as GroupBox;
                        if (gb != null && gb.Content != null)
                        {
                            Grid gbg = gb.Content as Grid;
                            InitControlIsEnabled(gbg, enabled);
                        }
                    }
                    else if (v is ComboBoxExtend)
                    {
                        ComboBoxExtend cbb = v as ComboBoxExtend;
                        cbb.IsEnabled = enabled;
                        continue;
                    }
                    else if (v is DateTimePickerExtend)
                    {
                        try
                        {
                            DateTimePickerExtend dtp = v as DateTimePickerExtend;
                            dtp.IsEnabled = enabled;
                            continue;
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                    }
                    else if (v is TimePicker)
                    {
                        TimePicker tp = v as TimePicker;
                        tp.IsEnabled = enabled;
                    }
                    else if (v is Button)
                    {
                        Button btn = v as Button;
                        btn.IsEnabled = enabled;

                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 设置控件是否显示。
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <param name="controlSymbol">控件类型标记</param>
        /// <param name="conditionItem">条件集合</param>
        /// <returns>true-控件显示正常;false-控件显示灰色不可用</returns>
        bool IsEnabledControl(string controlName, string controlSymbol, List<string> conditionItem)
        {
            foreach (var v in conditionItem)
            {
                try
                {
                    if ((controlSymbol + v).ToLower() == controlName.ToLower())
                    {
                        return true;
                    }
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
            }

            return false;
        }

        /// <summary>
        /// 设置控件是否为灰色。
        /// </summary>
        static bool SetControlIsEnabledSymbol = false;

        /// <summary>
        /// 设置控件上的条件
        /// </summary>
        /// <param name="g">Grid控件</param>
        /// <param name="item">TreeViewItem控件</param>
        /// <param name="message">显示的信息控件</param>
        public void SetParamConditionControl(Grid g, TreeViewItem item, LabelExtend message)
        {
            if (item == null)
            {
                return;
            }
            message.Content = item.Header;
            BasiReportInfo ri = item.Tag as BasiReportInfo;

            if (ri != null && ri.report_condition_param != null)
            {
                SetControlIsEnabledSymbol = true;
                SetParamConditionControl(g, ri.report_condition_param.Split('|').ToList<String>());
            }
            else
            {
                if (SetControlIsEnabledSymbol != false)
                {
                    InitControlIsEnabled(g, false);
                    SetControlIsEnabledSymbol = false;
                }
            }
        }

        /// <summary>
        /// 设置控件是否灰色方法
        /// </summary>
        /// <param name="g">Grid控件</param>
        /// <param name="conditionItem">条件集合</param>
        void SetParamConditionControl(Grid g, List<String> conditionItem)
        {
            try
            {
                if (g == null)
                {
                    return;
                }
                foreach (var v in g.Children)
                {
                    if (v is Grid)
                    {
                        Grid temp = v as Grid;
                        if (temp.Children.Count > 0)
                        {
                            SetParamConditionControl(temp, conditionItem);
                        }
                    }
                    else if (v is GroupBox)
                    {
                        GroupBox gb = v as GroupBox;
                        if (gb != null && gb.Content != null)
                        {
                            Grid gbg = gb.Content as Grid;
                            SetParamConditionControl(gbg, conditionItem);
                        }
                    }
                    else if (v is ComboBoxExtend)
                    {
                        ComboBoxExtend cbb = v as ComboBoxExtend;
                        cbb.IsEnabled = IsEnabledControl(cbb.Name, "cbb", conditionItem);
                        continue;
                    }
                    else if (v is DateTimePickerExtend)
                    {
                        try
                        {
                            DateTimePickerExtend dtp = v as DateTimePickerExtend;
                            dtp.IsEnabled = IsEnabledControl(dtp.Name, "dtp", conditionItem);
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        continue;
                    }
                    else if (v is TimePicker)
                    {
                        try
                        {
                            TimePicker tp = v as TimePicker;
                            tp.IsEnabled = IsEnabledControl(tp.Name, "tp", conditionItem);
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                    }
                    else if (v is Button)
                    {
                        Button btn = v as Button;
                        btn.IsEnabled = true;
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        #endregion --> 对Grid 里的控件进行操作。

        #region --> 获取控件里的值

        /// <summary>
        /// 获取控件里的值。
        /// </summary>
        /// <param name="pc">参数条件类</param>
        /// <param name="paramCondition">参数条件字符串</param>
        /// <returns></returns>
        public String GetControlvalue(ParamCondition pc, string paramCondition)
        {
            String[] pcArray = paramCondition.Split('|');

            Type t = pc.GetType();
            MemberInfo[] miList = t.GetMembers();
            string contentValue = "";
            foreach (var mi in miList)
            {
                object value = GetMemberInfoValue(pcArray, mi, pc);
                if (value != null && value.ToString().Trim().Length > 0)
                {
                    contentValue += value + "|";
                }
                if (mi.Name.ToLower() == "TimeScope".ToLower())
                {
                    string reflectType = mi.ToString().Substring(0, mi.ToString().LastIndexOf(" ")) + "," + t.Module.Name.Substring(0, t.Module.Name.LastIndexOf('.'));
                    Type reflectTS = Type.GetType(reflectType);

                    PropertyInfo[] tsList = reflectTS.GetProperties();
                    foreach (var tsv in tsList)
                    {
                        value = GetMemberInfoValue(pcArray, tsv, pc.TimeScope);
                        if (value != null && value.ToString().Trim().Length > 0)
                        {
                            contentValue += value + "|";
                        }
                    }
                }
            }

            contentValue = GetControlValue(contentValue, pcArray);
            return contentValue;

        }

        /// <summary>
        /// 获取控件里的值。
        /// </summary>
        /// <param name="paraItem">参数字符串[条件+值]</param>
        /// <param name="pcItem">参数条件字符串</param>
        /// <returns></returns>
        String GetControlValue(string paraItem, String[] pcItem)
        {
            string value = "";
            try
            {
                string[] tempPcItem = paraItem.Split('|');
                foreach (var v in pcItem)
                {
                    foreach (var pv in tempPcItem)
                    {
                        if (pv.ToLower().Contains(v.ToLower()))
                        {
                            try
                            {
                                value += pv.Split(',').Length > 1 ? pv.Split(',')[1] + "_" : pv.Split(',')[0];
                                break;
                            }
                            catch (Exception ee)
                            {
                                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return value.JudgeIsNullOrEmpty() == true ? paraItem : value;
        }

        /// <summary>
        /// 获取member里的值。
        /// </summary>
        /// <param name="pcItem">参数条件字符串</param>
        /// <param name="mi">成员信息</param>
        /// <param name="pc">要从此中获取数据的对象</param>
        /// <returns></returns>
        object GetMemberInfoValue(String[] pcItem, MemberInfo mi, object pc)
        {
            foreach (var v in pcItem)
            {
                if (v.ToLower() == mi.Name.ToLower())
                {
                    if (mi is FieldInfo)
                    {
                        return mi.Name + "," + (mi as FieldInfo).GetValue(pc);
                    }
                    else if (mi is PropertyInfo)
                    {
                        return mi.Name + "," + (mi as PropertyInfo).GetValue(pc, null);
                    }
                    continue;
                }

            }

            return "";
        }

        #endregion --> 获取控件里的值。

        #region --> 删除报表操作。

        /// <summary>
        /// 删除原先创建的报表
        /// </summary>
        public void DeleteOriginallyReport()
        {
            try
            {
                //-->删除临时报表文件。
                String filePath = Environment.CurrentDirectory + SysConfig.GetSysConfig().ReportCfg.ReportTempPath;
                DeleteOriginallyReport(filePath);
                //-->删除
                filePath = Environment.CurrentDirectory + SysConfig.GetSysConfig().ReportCfg.AutoPrintReportPath;
                DeleteOriginallyReport(filePath);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }
        /// <summary>
        /// 删除原报表
        /// </summary>
        /// <param name="filePath"></param>
        private void DeleteOriginallyReport(String filePath)
        {
            try
            {
                //-->判断文件是否存在。
                DirectoryInfo info = new DirectoryInfo(filePath);
                if (info.Exists)
                {
                    //-->开始进行删除操作。
                    FileInfo[] fiList = info.GetFiles();
                    foreach (var v in fiList)
                    {
                        try
                        {
                            File.Delete(v.FullName);
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        private int saveDay = 10;

        /// <summary>
        ///删除历史报表记录以及文件，保留10天报表，服务器上保留所有。
        /// </summary>
        public void DeleteTempReport()
        {
            try
            {
                DateTime current = DateTime.Now.Date;
                DateTime hisDate = current.AddDays(-saveDay);

                string path = Environment.CurrentDirectory +
                                SysConfig.GetSysConfig().ReportCfg.ReportTempPath;

                if (Directory.Exists(path))
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    FileInfo[] fileInfo = di.GetFiles();
                    foreach (FileInfo f in fileInfo)
                    {
                        string[] date = f.Name.Split('_');
                        DateTime fileDate = DateTime.Parse(DateTimeFormat(date[1].Split('.')[1]));
                        if (fileDate < hisDate)
                        {
                            File.Delete(f.FullName);
                        }
                    }
                }

                DeleteFileFromHistoryReportPath();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("删除临时报表异常：" + ex.ToString());
            }
        }

        #endregion --> 删除报表操作。

        /// <summary>
        /// 获取历史报表记录。
        /// </summary>
        /// <param name="reportName">报表名称</param>
        /// <returns></returns>
        public List<rptLookHistoryReportModel> HistoryReportItemByReportName(string reportName)
        {
            try
            {
                string sqlQuery = string.Format("select ti.report_type_name,si.report_sub_type_name,t.* ");
                sqlQuery += string.Format(" from rpt_history_info t");
                sqlQuery += string.Format(" inner join basi_report_sub_type_info si on t.report_type_id = si.report_type_id and t.report_sub_type_id = si.report_sub_type_id");
                sqlQuery += string.Format(" inner join basi_report_type_info ti on ti.report_type_id = t.report_type_id");
                sqlQuery += string.Format(" where t.report_name = '{0}'", reportName);
                sqlQuery += string.Format(" order by t.report_add_date desc,t.report_add_time desc");

                List<rptLookHistoryReportModel> item = DBCommon.Instance.SetTModelValue<rptLookHistoryReportModel>(sqlQuery);

                if (item != null)
                {
                    foreach (var v in item)
                    {
                        v.add_date = v.report_add_date.ToYearMonthDay() ;
                        v.add_time = v.report_add_time.FormatToTime();
                    }
                }
                return item;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return null;
        }

        /// <summary>
        /// 设置控件上的值。
        /// </summary>
        /// <param name="g">DataGrid控件。</param>
        /// <param name="paramConditionItem">参数条件</param>
        /// <param name="paramValueItem">参数内容</param>
        public void SetParamValueControl(Grid g, string[] paramConditionItem, string[] paramValueItem)
        {

            try
            {
                if (g == null)
                {
                    return;
                }
                foreach (var v in g.Children)
                {
                    if (v is Grid)
                    {
                        Grid temp = v as Grid;
                        if (temp.Children.Count > 0)
                        {
                            SetParamValueControl(temp, paramConditionItem, paramValueItem);
                        }
                    }
                    else if (v is GroupBox)
                    {
                        GroupBox gb = v as GroupBox;
                        if (gb != null && gb.Content != null)
                        {
                            Grid gbg = gb.Content as Grid;
                            SetParamValueControl(gbg, paramConditionItem, paramValueItem);
                        }
                    }
                    else if (v is ComboBoxExtend)
                    {
                        ComboBoxExtend cbb = v as ComboBoxExtend;

                        string value = GetParamValue(cbb.Name, "cbb", paramConditionItem, paramValueItem);
                        if (value.JudgeIsNullOrEmpty() == false)
                        {
                            Wrapper.ComboBoxSelectedItem(cbb, value);
                        }
                        continue;
                    }
                    else if (v is DateTimePickerExtend)
                    {
                        try
                        {
                            DateTimePickerExtend dtp = v as DateTimePickerExtend;

                            string value = GetParamValue(dtp.Name, "dtp", paramConditionItem, paramValueItem);
                            if (value.JudgeIsNullOrEmpty() == false)
                            {
                                DateTime dt = DateTime.ParseExact(value, "yyyyMMdd", null);
                                dtp.DatePickerControl.SelectedDate = dt;
                            }
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        continue;
                    }
                    else if (v is TimePicker)
                    {
                        try
                        {
                            TimePicker tp = v as TimePicker;
                            string value = GetParamValue(tp.Name, "tp", paramConditionItem, paramValueItem);
                            if (value.JudgeIsNullOrEmpty() == false)
                            {
                                tp.Text = value;
                            }
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                    }
                    else if (v is Button)
                    {
                        Button btn = v as Button;
                        btn.IsEnabled = true;
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 获取控件上的内容。
        /// </summary>
        /// <param name="controlName">操作名称</param>
        /// <param name="controlType">操作类型</param>
        /// <param name="paramConditionItem">参数条件</param>
        /// <param name="paramValueItem">参数内容</param>
        /// <returns></returns>
        string GetParamValue(string controlName, string controlType, string[] paramConditionItem, string[] paramValueItem)
        {
            string value = null;

            for (int i = 0; i < paramConditionItem.Length; i++)
            {
                if (controlName.ToLower() == (controlType + paramConditionItem[i]).ToLower())
                {
                    try
                    {
                        value = paramValueItem[i];
                        break;
                    }
                    catch (Exception ee)
                    {
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);

                    }
                }
            }
            return value;
        }

        /// <summary>
        /// 删除历史目录下的报表文件。
        /// </summary>
        public void DeleteFileFromHistoryReportPath()
        {
            try
            {
                //-->获取历史报表的路径 。
                String directoryPath = Environment.CurrentDirectory + SysConfig.GetSysConfig().ReportCfg.HistoryReportPath;
                DirectoryInfo dir = new DirectoryInfo(directoryPath);
                if (!dir.Exists)
                {
                    //-->此目录不存在，直接退出。
                    return;
                }
                //-->循环删除报表里的报表。
                FileInfo[] fiList = dir.GetFiles();
                foreach (FileInfo fi in fiList)
                {
                    try
                    {
                        File.Delete(fi.FullName);
                    }
                    catch (Exception ee)
                    {
                        //-->删除文件的时候，出现异常。
                        Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        /// <summary>
        /// 初始化日志操作
        /// </summary>
        /// <param name="g"></param>
        public void InitDateTimePickerValue(Grid g)
        {

            try
            {
                if (g == null)
                {
                    return;
                }
                foreach (var v in g.Children)
                {
                    if (v is Grid)
                    {
                        Grid temp = v as Grid;
                        if (temp.Children.Count > 0)
                        {
                            InitDateTimePickerValue(temp);
                        }
                    }
                    else if (v is GroupBox)
                    {
                        GroupBox gb = v as GroupBox;
                        if (gb != null && gb.Content != null)
                        {
                            Grid gbg = gb.Content as Grid;
                            InitDateTimePickerValue(gbg);
                        }
                    }
                    else if (v is DateTimePickerExtend)
                    {
                        try
                        {
                            DateTimePickerExtend dtp = v as DateTimePickerExtend;
                            dtp.DatePickerControl.SelectedDate = DateTime.Now;
                        }
                        catch (Exception ee)
                        {
                            Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                        }
                        continue;
                    }
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        //public 
    }
}
