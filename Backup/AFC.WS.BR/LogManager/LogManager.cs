using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace AFC.WS.BR.LogManager
{
    using AFC.WS.Model.Const;
    /// <summary>
    /// 日志的流水号，和日志代码，日志内容需要约定？？
    /// sec 从数据库中什么地方获取？
    /// 
    /// edit by wangdx  date:20110517
    /// 
    /// edit by wangdx date 20120213 增加了key_field字段的录入。
    /// 增加了记录票箱，钱箱操作流水表的通用函数。
    /// 
    /// edit by wangdx date 20120803 增加了双权限认证的日志函数
    /// </summary>

    using AFC.WS.UI.Common;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.BR.Data;
    using Microsoft.Office.Interop.Excel;

    public class LogManager
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const string SUCCESSFUL = "00000000";

        /// <summary>
        /// 失败
        /// </summary>
        public const string FAILED = "00000001";

        private WriteLogComm logErrorCode = null;

        /// <summary>
        /// 记录log信息
        /// </summary>
        /// <returns></returns>
        public int AddLogInfo(string businCode,string resultCode)
        {
            DevOperLogInfo log = new DevOperLogInfo();
            log.busi_code = businCode;
            log.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
            log.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            log.oper_date = DateTime.Now.ToString("yyyyMMdd");
            log.oper_time = DateTime.Now.ToString("HHmmss");
            log.oper_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            log.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            int seq = 0;
            log.busi_udsn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();
            if (resultCode == "0")
            {
                log.oper_status = SUCCESSFUL;
            }
            else
            {
                log.oper_status = FAILED;
            }
            log.remark = string.Empty;
            try
            {
                int res = DBCommon.Instance.InsertTable(log, "dev_oper_log_info");
                if (res != 1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            
        }

        /// <summary>
        /// 记录双权限认证日志
        /// </summary>
        /// <param name="buinCode">日志代码</param>
        /// <param name="secordOperatorId">双权限认证其他操作员ID</param>
        /// <param name="remark">日志描述</param>
        /// <param name="keyField2">为了现金解行应用的数据库表，在keyFiled2中记录现金解行金额</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int AddDPLogInfo(string buinCode, string secordOperatorId, string remark,params string[] keyField2)
        {
            DevDoubleOperLogInfo dpLogInfo = new DevDoubleOperLogInfo();
            dpLogInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            dpLogInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            dpLogInfo.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
            if (keyField2 != null &&
                keyField2.Length > 0)
            {
                dpLogInfo.key_fld2 = keyField2[0];
            }
            dpLogInfo.operator1_id = BuinessRule.GetInstace().OperatorId;
            dpLogInfo.operator2_id = secordOperatorId;
            dpLogInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
            dpLogInfo.update_time = DateTime.Now.ToString("HHmmss");
            dpLogInfo.oper_date = dpLogInfo.update_date;
            dpLogInfo.oper_time = dpLogInfo.update_time;
            dpLogInfo.oper_status = "00";
            int seq = 0;
            dpLogInfo.busi_udsn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();

            dpLogInfo.key_fld1 = secordOperatorId;
            dpLogInfo.busi_code = buinCode;
            dpLogInfo.remark_128 = remark;
            
          int res=   DBCommon.Instance.InsertTable<DevDoubleOperLogInfo>(dpLogInfo, "dev_double_oper_log_info");

          return res == 1 ? 0 : -1;
        }


        public int AddLogInfo(string keyFild1, string keyFild2,string remark,string resultCode)
        {
            DevOperLogInfo log = new DevOperLogInfo();
            log.busi_code = "1706";
            log.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
            log.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            log.oper_date = DateTime.Now.ToString("yyyyMMdd");
            log.oper_time = DateTime.Now.ToString("HHmmss");
            log.oper_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            log.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            log.busi_code = OperationCode.Control_command;
            log.key_fld1 = keyFild1;
            log.key_fld2 = keyFild2;
            log.remark = remark;
            int seq = 0;
            log.busi_udsn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();
            if (resultCode == "0")
            {
                log.oper_status = SUCCESSFUL;
            }
            else
            {
                log.oper_status = FAILED;
            }
            log.remark = string.Empty;
            try
            {
                int res = DBCommon.Instance.InsertTable(log, "dev_oper_log_info");
                if (res != 1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }

        }


     
        /// <summary>
        /// 记录log信息
        /// </summary>
        /// <param name="busiCode">操作代码</param>
        /// <param name="resultCode">操作结果0，成功；1 失败</param>
        /// <param name="remark">内容</param>
        /// <returns>成功为0，否则为错误代码</returns>
        public int AddLogInfo(string busiCode, string resultCode, string remark)
        {
            DevOperLogInfo log = new DevOperLogInfo();
            log.busi_code = busiCode;
            log.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
            log.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            log.oper_date = DateTime.Now.ToString("yyyyMMdd");
            log.oper_time = DateTime.Now.ToString("HHmmss");
            log.update_date = log.oper_date;
            log.update_time = log.oper_time;
          
            log.oper_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            int seq =0;
            log.busi_udsn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();
            log.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            if (resultCode == "0")
            {
                log.oper_status = SUCCESSFUL;
            }
            else
            {
                log.oper_status = FAILED;
            }

            log.key_fld1 = remark;
            log.key_fld2 = remark;
            log.remark = remark;
            try
            {
                return
                    DBCommon.Instance.InsertTable(log, "dev_oper_log_info") == 1 ? 0 : -1;
          
            }
            catch (Exception ex)
            {

                return -1;
            }
            
        }

        /// <summary>
        /// 得到记录到Log的配置文件信息
        /// </summary>
        /// <param name="errorCode">错误代码</param>
        /// <returns>返回记录的错误日志</returns>
        private string GetErrorContent(uint errorCode)
        {
            ErrorLogData data = new ErrorLogData();
            FieldInfo[] fiArray = data.GetType().GetFields();
            foreach (var temp in fiArray)
            {
                if (Convert.ToUInt32(temp.GetValue(data)) == errorCode)
                {
                    object value=temp.GetCustomAttributes(false)[0];
                    DescriptionAttribute description = value as DescriptionAttribute;
                    if (description != null)
                        return string.Format("errorCode=0x{0}, Content= {1}: {2}", errorCode.ToString("x2"), temp.Name, description.Description);
                    else
                        return string.Format("errorCode=0x{0}, Content= {1}", errorCode.ToString("x2"), temp.Name);
                }
            }
            return null;
        }

        /// <summary>
        /// 初始化记录Log的配置信息
        /// </summary>
        /// <param name="configFileName">配置文件</param>
        /// <param name="strInstanceName">标签号</param>
        /// <param name="logFileName">文件路径</param>
        public void  InitLog(string configFileName, string strInstanceName, string logFileName)
        {
            WriteLogApiComm.SetConfigureFile(configFileName, strInstanceName, logFileName, 10485760, 10, "DEBUG");
            IntPtr ptr = WriteLogApiComm.InitLogInstance(configFileName, strInstanceName);
            this.logErrorCode = new WriteLogComm(ptr, true);
            return ;
        }

        /// <summary>
        /// 记录错误代码
        /// </summary>
        /// <param name="errorCode">错误代码<param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int WriteErrorCode(uint errorCode)
        {
            if (logErrorCode != null)
            {
                this.logErrorCode.Log_Error(GetErrorContent(errorCode));
            }
            else
            {
                WriteLog.Log_Error("logErrorCode is null");
            }
            return 0;
        }

        /// <summary>
        /// 记录票箱操作流水
        /// 该函数为票箱初始化，登记用。
        /// </summary>
        /// <param name="tickBoxId">票箱ID，该票箱ID是16进制的8位显示数据</param>
        /// <param name="operationType">操作类型01：安装;02：卸下; 03:清点;04:压钱 05：领用 06：归还 07：rfid初始化 08：票箱登记,09 票箱调出</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int WriteTickBoxOperation(string tickBoxId,string operationType)
        {
            TickBoxReplaceInfo info = new TickBoxReplaceInfo();
            int seq = 0;
            info.replace_sn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();
            info.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            info.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info.occur_date = DateTime.Now.ToString("yyyyMMdd");
            info.occur_time = DateTime.Now.ToString("HHmmss");
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.ticket_box_id = tickBoxId;
            info.position_in_dev = "ff";
            info.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            info.tick_mana_type = "ff";
            info.install_status = "03";
            info.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
            info.before_operation_num = 0;
            info.tickets_num = 0;
            info.replace_type = operationType;
            return DBCommon.Instance.InsertTable<TickBoxReplaceInfo>(info, "tick_box_replace_info") == 1 ? 0 : -1;
        }

        /// <summary>
        /// 增加票箱流水
        /// </summary>
        /// <param name="tickBoxId">票箱ID（8位16进制显示）</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="tickManType">库存管理类型</param>
        /// <param name="beforeOperationNum">操作前张数</param>
        /// <param name="currentNum">当前张数</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int WriteTickBoxOperation(string tickBoxId, string operationType,string tickManType, int beforeOperationNum, int currentNum)
        {
            int seq = 0;
            TickBoxReplaceInfo info = new TickBoxReplaceInfo();
            info.replace_sn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();
            info.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            info.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info.occur_date = DateTime.Now.ToString("yyyyMMdd");
            info.occur_time = DateTime.Now.ToString("HHmmss");
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.ticket_box_id = tickBoxId;
            info.position_in_dev = "ff";
            info.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            info.tick_mana_type = tickManType;
            info.install_status = "03";
          
                info.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
            
           
            info.before_operation_num = beforeOperationNum;
            info.tickets_num = currentNum;
            info.replace_type = operationType;
            return DBCommon.Instance.InsertTable<TickBoxReplaceInfo>(info, "tick_box_replace_info") == 1 ? 0 : -1;
        }

        /// <summary>
        /// 增加票箱流水
        /// </summary>
        /// <param name="tickBoxId">票箱ID（8位16进制显示）</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="tickManType">库存管理类型</param>
        /// <param name="beforeOperationNum">操作前张数</param>
        /// <param name="currentNum">当前张数</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int WriteTickBoxOperation(string tickBoxId, string operationType, string tickManType, int beforeOperationNum, int currentNum,string deviceId)
        {
            int seq = 0;
            TickBoxReplaceInfo info = new TickBoxReplaceInfo();
            info.replace_sn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();
            info.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            info.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info.occur_date = DateTime.Now.ToString("yyyyMMdd");
            info.occur_time = DateTime.Now.ToString("HHmmss");
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.ticket_box_id = tickBoxId;
            info.position_in_dev = "ff";
            info.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            info.tick_mana_type = tickManType;
            info.install_status = "03";

            info.device_id = deviceId;


            info.before_operation_num = beforeOperationNum;
            info.tickets_num = currentNum;
            info.replace_type = operationType;
            return DBCommon.Instance.InsertTable<TickBoxReplaceInfo>(info, "tick_box_replace_info") == 1 ? 0 : -1;
        }

        /// <summary>
        /// 记录钱箱操作流水表
        /// 该函数为钱箱初始化，登记用。
        /// </summary>
        /// <param name="moneyBoxId">钱箱ID(16进制8位显示)</param>
        /// <param name="operationType">操作类型01：安装;02：卸下; 03:清点;04:压钱 05：领用 06：归还 07：rfid初始化 08：钱箱登记 09：钱箱调出</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int WriteMoneyBoxOperation(string moneyBoxId, string operationType)
        {
            int seq = 0;
            CashBoxReplaceInfo info = new CashBoxReplaceInfo();
            info.replace_sn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();
            info.replace_type = operationType;
            info.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            info.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
            info.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            info.money_box_id = moneyBoxId;
            info.occur_date = DateTime.Now.ToString("yyyyMMdd");
            info.occur_time = DateTime.Now.ToString("HHmmss");
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.install_status = "03";
            info.currency_code = "00";
            info.before_operation_money = 0;
            info.total_money_count = 0;
            info.currency_num = 0;
            info.position_in_dev = "ff";
            return DBCommon.Instance.InsertTable<CashBoxReplaceInfo>(info, "cash_box_replace_info") == 1 ? 1 : -1;
        }

        /// <summary>
        /// 记录钱箱操作流水表
        /// </summary>
        /// <param name="moneyBoxId">钱箱ID(16进制8位显示)</param>
        /// <param name="operationType">操作类型01：安装;02：卸下; 03:清点;04:压钱 05：领用 06：归还 07：rfid初始化 08：钱箱登记</param>
        /// <param name="moneyCode">币种代码</param>
        /// <param name="beforeOperationNum">操作前Num</param>
        /// <param name="currentNum">当前金额</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int WriteMoneyBoxOperation(string moneyBoxId, string operationType, string moneyCode, decimal beforeOperationNum, decimal currentNum, decimal totalNum, params string[] deviceCode)
        {
            int seq = 0;
            CashBoxReplaceInfo info = new CashBoxReplaceInfo();
            info.replace_sn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString();
            info.replace_type = operationType;
            info.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            //2012.12.25 dusj modify begin
            //info.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
            if (deviceCode != null && deviceCode.Length > 0)
            {
                info.device_id = deviceCode[0];
            }
            else
            {
                info.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
            }
            //2012.12.25 dusj modify end 
            info.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            info.money_box_id = moneyBoxId;
            info.occur_date = DateTime.Now.ToString("yyyyMMdd");
            info.occur_time = DateTime.Now.ToString("HHmmss");
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.install_status = "03";
            info.currency_code = moneyCode;
            info.total_money_count = totalNum;
            info.before_operation_money = beforeOperationNum;
            info.currency_num = currentNum;
            info.position_in_dev = "ff";
            return DBCommon.Instance.InsertTable<CashBoxReplaceInfo>(info, "cash_box_replace_info") == 1 ? 1 : -1;
        }
        /// <summary>
        /// 记录钱箱操作流水表
        /// </summary>
        /// <param name="seq">更换流水号，多币种时用同一个流水号</param>
        /// <param name="moneyBoxId">钱箱ID(16进制8位显示)</param>
        /// <param name="operationType">操作类型01：安装;02：卸下; 03:清点;04:压钱 05：领用 06：归还 07：rfid初始化 08：钱箱登记</param>
        /// <param name="moneyCode">币种代码</param>
        /// <param name="beforeOperationNum">操作前Num</param>
        /// <param name="currentNum">当前金额</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int WriteMoneyBoxOperation(int seq, string moneyBoxId, string operationType, string moneyCode, decimal beforeOperationNum, decimal currentNum, decimal totalNum, params string[] deviceCode)
        {
            CashBoxReplaceInfo info = new CashBoxReplaceInfo();
            info.replace_sn = seq.ToString();
            info.replace_type = operationType;
            info.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            if (deviceCode != null && deviceCode.Length > 0)
            {
                info.device_id = deviceCode[0];
            }
            else
            {
                info.device_id = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
            }
            info.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            info.money_box_id = moneyBoxId;
            info.occur_date = DateTime.Now.ToString("yyyyMMdd");
            info.occur_time = DateTime.Now.ToString("HHmmss");
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.install_status = "03";
            info.currency_code = moneyCode;
            info.before_operation_money = beforeOperationNum;
            info.total_money_count = totalNum;
            info.currency_num = currentNum;
            info.position_in_dev = "ff";
            return DBCommon.Instance.InsertTable<CashBoxReplaceInfo>(info, "cash_box_replace_info") == 1 ? 1 : -1;
        }


        /// <summary>
        /// 记录钱箱操作流水表
        /// </summary>
        /// <param name="seq">更换流水号，多币种时用同一个流水号</param>
        /// <param name="moneyBoxId">钱箱ID(16进制8位显示)</param>
        /// <param name="operationType">操作类型01：安装;02：卸下; 03:清点;04:压钱 05：领用 06：归还 07：rfid初始化 08：钱箱登记</param>
        /// <param name="moneyCode">币种代码</param>
        /// <param name="beforeOperationNum">操作前Num</param>
        /// <param name="currentNum">当前金额</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int WriteMoneyBoxOperation(int seq, string moneyBoxId, string operationType, string moneyCode, decimal beforeOperationNum, decimal currentNum, decimal totalNum,string deviceId)
        {
            CashBoxReplaceInfo info = new CashBoxReplaceInfo();
            info.replace_sn = seq.ToString();
            info.replace_type = operationType;
            info.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            info.device_id = deviceId;
            info.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            info.money_box_id = moneyBoxId;
            info.occur_date = DateTime.Now.ToString("yyyyMMdd");
            info.occur_time = DateTime.Now.ToString("HHmmss");
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.install_status = "03";
            info.currency_code = moneyCode;
            info.before_operation_money = beforeOperationNum;
            info.total_money_count = totalNum;
            info.currency_num = currentNum;
            info.position_in_dev = "ff";
            return DBCommon.Instance.InsertTable<CashBoxReplaceInfo>(info, "cash_box_replace_info") == 1 ? 1 : -1;
        }


        /// <summary>
        /// 记录现金领用归还历史日志
        /// </summary>
        /// <param name="operatorType">操作类型</param>
        /// <param name="money">操作金额</param>
        /// <returns></returns>
        public int AddCashOperatorLog(string operatorType, decimal money,string operatorID)
        {
            CashOperatorReturnLog log = new CashOperatorReturnLog();
            int seq = 0;
            log.oper_sec = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec");
            log.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            log.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            log.update_date = DateTime.Now.ToString("yyyyMMdd");
            log.update_time = DateTime.Now.ToString("HHmmss");
            //log.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            log.operator_id = operatorID;
            log.total_money = money;
            log.operate_type = operatorType;
            try
            {
                return DBCommon.Instance.InsertTable<CashOperatorReturnLog>(log, "cash_operator_return_log");
            }
            catch (Exception ex)
            {
                return -1;
            }

        }
        /// <summary>
        /// 现金库存变化流水
        /// </summary>
        /// <param name="changeType">00调入，01调出，02调整，03解行， 04待解行，05钱箱补充，06钱箱清点，07操作员领用,08，操作员归还</param>
        /// <param name="beforeMoney"></param>
        /// <param name="afterMoney"></param>
        /// <param name="currencyCode"></param>
        /// <param name="remark"></param>
        /// <returns></returns>

        public int AddCashStoreLog(string changeType, decimal beforeMoney, decimal afterMoney,string currencyCode,string remark)
        {
            CashStoreChangeLog log = new CashStoreChangeLog();
            int seq = 0;
            log.oper_sec = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec");
            log.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            log.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            log.update_date = DateTime.Now.ToString("yyyyMMdd");
            log.update_time = DateTime.Now.ToString("HHmmss");
            log.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            log.change_type = changeType;
        
            log.original_money = beforeMoney;
            log.later_money = afterMoney;
            log.currency_code = currencyCode;
              string cmd = string.Format("select t.param_value from basi_run_param_info t where t.param_code='0003'");
            string runDate = string.Empty;
            try
            {
                System.Data.DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                log.run_date = DateTime.ParseExact(dt.Rows[0][0].ToString(), "yyyyMMdd", null).ToString("yyyyMMdd");
            }
            catch (Exception ex)
            {

            }
            //log.run_date = BuinessRule.GetInstace().rm.GetRunDate();
            log.remark = remark;
            try
            {
                return DBCommon.Instance.InsertTable<CashStoreChangeLog>(log, "cash_store_change_log");
            }
            catch (Exception ex)
            {
                return -1;
            }

        }



        /*added by wangdx 20121003,待解行金额操作，第二个运营日开始2:00到6:00的时，统一为待解行的运营日*/

        /// <summary>
        /// 现金库存变化流水
        /// </summary>
        /// <param name="changeType">00调入，01调出，02调整，03解行， 04待解行，05钱箱补充，06钱箱清点，07操作员领用,08，操作员归还</param>
        /// <param name="beforeMoney"></param>
        /// <param name="afterMoney"></param>
        /// <param name="currencyCode"></param>
        /// <param name="remark"></param>
        /// <returns></returns>

        public int AddCashStoreLogForWaitBank(string changeType, decimal beforeMoney, decimal afterMoney, string currencyCode, string remark)
        {
            CashStoreChangeLog log = new CashStoreChangeLog();
            int seq = 0;
            log.oper_sec = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec");
            log.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            log.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            log.update_date = DateTime.Now.ToString("yyyyMMdd");
            log.update_time = DateTime.Now.ToString("HHmmss");
            log.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            log.change_type = changeType;

            log.original_money = beforeMoney;
            log.later_money = afterMoney;
            log.currency_code = currencyCode;
            log.run_date = GetRunDate();
            //string cmd = string.Format("select t.param_value from basi_run_param_info t where t.param_code='0003'");
            //string runDate = string.Empty;
            //try
            //{
            //    System.Data.DataTable dt = DBCommon.Instance.GetDatatable(cmd);
            //    log.run_date = DateTime.ParseExact(dt.Rows[0][0].ToString(), "yyyyMMdd", null).ToString("yyyyMMdd");
            //}
            //catch (Exception ex)
            //{

            //}
            //log.run_date = BuinessRule.GetInstace().rm.GetRunDate();
            log.remark = remark;
            try
            {
                return DBCommon.Instance.InsertTable<CashStoreChangeLog>(log, "cash_store_change_log");
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        private  string GetRunDate()
        {
            string cmd = string.Format("select t.param_value from basi_run_param_info t where t.param_code='0003'");
            string runDate = string.Empty;
            try
            {
                System.Data.DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                runDate = DateTime.ParseExact(dt.Rows[0][0].ToString(), "yyyyMMdd", null).ToString("yyyyMMdd");
              DateTime beginTime = DateTime.ParseExact(runDate + "020000", "yyyyMMddHHmmss", null);
              DateTime endTime = DateTime.ParseExact(runDate + "060000", "yyyyMMddHHmmss", null);
            if (DateTime.Now <= endTime && 
                DateTime.Now >= beginTime)
            {
                return DateTime.ParseExact(runDate, "yyyyMMdd", null).Subtract(new TimeSpan(24, 0, 0)).ToString("yyyyMMdd");
            }
               return runDate;
            }
            catch (Exception ex)
            {
                return DateTime.Now.ToString("yyyyMMdd");
            }

        


        }

        public int AddRemarkInfo(string operatorId, string projectName, string remark)
        {
            OperContentLogInfo logInfo = new OperContentLogInfo();
            int seq = 0;
            logInfo.content_sn = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec").ToString().ToInt64();
            logInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            logInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            logInfo.project_name = projectName;
            logInfo.content = remark;
            logInfo.operator_id = operatorId;
            logInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
            logInfo.update_time = DateTime.Now.ToString("HHmmss");
            int result = DBCommon.Instance.InsertTable(logInfo, "oper_content_log_info");
            return result == 1 ? 0 : -1;
        }
    }
}
