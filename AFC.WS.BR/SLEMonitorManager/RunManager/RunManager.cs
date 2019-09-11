using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace AFC.WS.BR.RunManager
{
    
    using System.Threading;
    using AFC.WS.UI.Common;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.DB;
    using AFC.WS.Model.Const;
    using System.Data;

    /// <summary>
    /// 定义运行相关的操作
    /// </summary>
    public class RunManager
    {
        public const string DoublePrimissonQuery = "select bli.line_name  线路名称, bsi.station_cn_name  车站名称, t.device_id  设备ID, t.operator1_id  操作员ID, poi1.operator_name 操作员姓名, t.operator2_id  其他操作员ID, poi2.operator_name 其他操作员姓名, t.update_date  操作日期,t.update_time  操作时间,t.remark_128  操作内容 from dev_double_oper_log_info t left join basi_line_id_info bli on bli.line_id=t.line_id left join basi_station_info bsi on bsi.station_id=t.station_id left join priv_operator_info poi1 on  poi1.operator_id = t.operator1_id left join priv_operator_info poi2 on  poi2.operator_id = t.operator2_id where t.update_date='{0}' order by t.update_date,t.update_time desc";
        /// <summary>
        /// Srv 轮询SQL
        /// </summary>
        public const string LCTastRun = "select task_name as 任务名称,case task_name when 'TimingStartRun' then '运营开始' when 'TimingEndRun' then '运营结束' end 运营状态,case exec_status  when 0 then '执行成功' when 1 then '正在执行' when 2 then '执行失败' end 执行状态,t.start_exec_time as 开始时间,t.end_exec_time as 结束时间 from task_manage t where t.task_enable = '1'";

        /// <summary>
        /// 设备轮询SQL
        /// </summary>
        public const string DevRunStatus = "select sta.station_cn_name as 车站,t.device_id as 设备编码,devType.device_name as 设备类型,case t.status_value when '00' then '运营开始' when '01' then '运营开始进行中' when '02' then '运营开始失败' when '03' then '运营结束' when '04' then '运营结束进行中' when '05' then '运营结束失败' end 运营状态，t.update_date as 更新日期,t.update_time as 更新时间 from dev_run_status_detail t left join basi_station_info sta on sta.station_id=t.station_id inner join basi_dev_type_info devType on devType.Device_Type=subStr(t.device_id,5,2) and subStr(t.device_id, 5, 2) in  ('01','04','02','06') ";

        /// <summary>
        /// 服务器运营开始结束时监听数据表线程
        /// </summary>
        private Thread runStartThread = null;

        /// <summary>
        /// 设备运营开始结束时监听数据表线程
        /// </summary>
        private Thread devRunStartThread = null;

        /// <summary>
        /// 停止线程
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        public int AbortRunMonitorThread()
        {
            if (runStartThread == null)
            {
                WriteLog.Log_Error("plase call start thread  first!");
                return -1;
            }
            try
            {
                runStartThread.Abort();
                runStartThread = null;

                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
      
        }

        /// <summary>
        /// 启动运营监听线程
        /// </summary>
        /// <param name="MessageType">RunStart,RunEnd</param>
        /// <param name="flag">如果是需要轮询设备需要写1</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int StartRunMonitorThread(string MessageType)
        {
            int res=0;
            runStartThread = new Thread(new ThreadStart(() =>
            {
               
                Message msg = new Message();
                msg.MessageType =MessageType;
                int time=0;
                while (true)
                {
                    Thread.Sleep(5000);// 为了避免线程死锁
                    msg.Content = GetRunTaskList(MessageType);
                    MessageManager.SendMessasge(msg);
                    //todo:need send devStatus
                }
            }));
            runStartThread.Name = MessageType+"Thread";
            runStartThread.IsBackground = true;
            runStartThread.Start(); 
            return 0;
        }


        public DataTable GetDoublePrimissionOperation()
        {
            try
            {
                string queryCmd = string.Format(DoublePrimissonQuery, DateTime.Now.ToString("yyyyMMdd"));
                return DBCommon.Instance.GetDatatable(queryCmd);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 得到状态为启用的运营任务
        /// </summary>
        /// <returns>返回运营任务的集合</returns>
        public DataTable GetRunTaskList(string messageType)
        {
            try
            {
                string cmd = LCTastRun;
                //运营开始命令
                if (messageType == AsynMessageType.RunStart)
                {
                    cmd = cmd + "and t.TASK_NAME ='TimingStartRun'";
                }
                //运营结束命令
                if (messageType == AsynMessageType.RunEnd)
                {
                    cmd = cmd + "and t.TASK_NAME ='TimingEndRun'";
                }
                return DBCommon.Instance.GetDatatable(cmd);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 返回所有的设备运行状态
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetAllDevRunStatus(string stationId)
        {
            try
            {
                string cmd = DevRunStatus + "where t.station_id ='" + stationId + "'" + " and t.status_id='0A01' order by t.device_id";
                return DBCommon.Instance.GetDatatable(cmd);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
            return null;
        }

        /// <summary>
        /// 返回所有的票卡库存状态
        /// </summary>
        /// <param name="rundate">运营日</param>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetAllTikcetRunDateStatus(string stationId, string rundate)
        {
            try
            {
                string cmd = "select btmt.tick_mana_type_name as 票卡类型,"
                  +" decode(t.ticket_status, '00', '正常', '01', '废票') as 票卡状态,"
                  +" t.in_store_num || '张' as 当日在库总库存"
                  +" from tick_storage_info t"
                  +" left join (select t1.tick_mana_type, t1.tick_mana_type_name "
                  +" from basi_tick_mana_type_info t1 "
                  +" union all "
                  +" select t2.tick_mana_type, t2.tick_mana_type_name "
                  + " from tick_valued_product_info t2) btmt on btmt.tick_mana_type = "
                  + " t.tick_mana_type where t.station_id like '" + stationId + "' and t.update_date||t.update_time between '" + rundate + "020000' and '" + DateTime.ParseExact(rundate, "yyyyMMdd", null).AddDays(1).ToString("yyyyMMdd") + "020000'";
                return DBCommon.Instance.GetDatatable(cmd);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
            return null;
        }

        /// <summary>
        /// 返回所有的现金库存状态
        /// </summary>
        /// <param name="rundate">运营日</param>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetAllMoneyRunDateStatus(string stationId, string rundate)
        {
            try
            {
                string cmd = "select btmt.currency_name as 货币名称,"
               + "nvl(t.currency_num,0) as 当前在库货币数量,"
               + "to_char(nvl(t.currency_num,0) * btmt.currency_value) || '元' as 当前在库货币金额 "
               + "from cash_storage_info t "
               + "left join basi_money_type_info btmt on btmt.currency_code ="
               + "t.currency_code where t.station_id like '" + stationId + "' and t.update_date||t.update_time between '" + rundate + "020000' and '" + DateTime.ParseExact(rundate, "yyyyMMdd", null).AddDays(1).ToString("yyyyMMdd") + "020000'";
                return DBCommon.Instance.GetDatatable(cmd);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
            return null;
        }


        /// <summary>
        /// 返回BOM交易详细信息
        /// </summary>
        /// <param name="rundate">运营日</param>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetBOMRunDateBussDetail(string stationId, string rundate)
        {
            try
            {
                string cmd = " select bsi.station_cn_name as 车站名称, "
               + " t1.run_date_tran as 运营日, "
               + " t1.device_id as 设备编号, "
               + " sum(decode(t1.tick_card_type,'06', "
                          + " '0','08', "
                          + " '0','09', "
                          + " '0',t1.sail_value))/100 as 售票金额, "
              + " sum(decode(t1.tick_card_type,'06', "
                          + " '0','08', "
                          + " '0','09', "
                          + " '0',t1.recharge_value))/100 as 充值金额, "
              + " sum(decode(t1.tick_card_type,'06', "
                          + " '0','08', "
                          + " '0','09', "
                          + " '0',t1.update_value))/100 as 补票金额, "
              + " sum(decode(t1.tick_card_type,'06', "
                          + " '0','08', "
                          + " '0','09', "
                          + " '0',t1.refund_value))/100 as 退票金额 "
          + " from (select t.station_id,  "
                       + " t.run_date_tran, "
                       + " t.device_id, "
                       + " t.tick_card_type, "
                       + " decode(t.afc_type, '01', nvl(t.tran_value,0), 0) as sail_value, "
                       + " decode(t.afc_type, '02', nvl(t.tran_value,0), 0) as recharge_value, "
                       + " decode(t.afc_type, '05', nvl(t.tran_value,0), 0) as update_value, "
                       + " decode(t.afc_type, '06', nvl(-t.tran_value,0), 0) as refund_value "
                  + " from rpt_tran_daily t where t.payment_means = '00' "
                  + " and t.station_id = '" + stationId + "' and substr(t.device_id,5,2)='02' "
                  + " and t.run_date_tran = '" + rundate + "') t1 "
         + " left join basi_station_info bsi on bsi.station_id = t1.station_id "
         + " group by bsi.station_cn_name, t1.device_id, t1.run_date_tran ";
                return DBCommon.Instance.GetDatatable(cmd);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
            return null;
        }

        /// <summary>
        /// 返回TVM交易详细信息
        /// </summary>
        /// <param name="rundate">运营日</param>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetTVMRunDateBussDetail(string stationId, string rundate)
        {
            try
            {
                string cmd = " select bsi.station_cn_name as 车站名称, "
               + " t1.run_date_tran as 运营日, "
               + " t1.device_id as 设备编号, "
               + " sum(t1.sail_value) as 售票金额, "
               + " sum(t1.recharge_value) as 充值金额 "
               + " from (select t.station_id,  "
                       + " t.device_id, "
                       + " t.run_date_tran, "
                       + " decode(t.afc_type, '01', nvl(t.tran_value,0), 0)/100 as sail_value, "
                       + " decode(t.afc_type, '02', nvl(t.tran_value,0), 0)/100 as recharge_value "
                  + " from rpt_tran_daily t where t.payment_means = '00' "
                  + " and t.station_id = '" + stationId + "' and substr(t.device_id,5,2)='01' "
                  + " and t.run_date_tran = '" + rundate + "') t1 "
         + " left join basi_station_info bsi on bsi.station_id = t1.station_id "
         + " group by bsi.station_cn_name, t1.device_id, t1.run_date_tran "; return DBCommon.Instance.GetDatatable(cmd);
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
            return null;
        }

        /// <summary>
        /// 检查是否处在运营开始状态
        /// </summary>
        /// <returns>如果处在运营开始返回true，否则返回false</returns>
        public bool CheckHasRunStart()
        {
            BasiStationInfo staInfo = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            string deviceId = staInfo.device_id;
            string cmd = string.Format("select t.status_value from dev_run_status_detail t where t.status_id='0A01' and t.device_id='{0}'", deviceId);
            try
            {
                DataTable dt= DBCommon.Instance.GetDatatable(cmd);
                int res = 0;
                int.TryParse(dt.Rows[0][0].ToString(), out res);
                return res == 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查是否全部设备处在运营开始状态
        /// </summary>
        /// <returns>如果处在运营开始返回true，否则返回false</returns>
        public bool CheckHasDevStart()
        {
            bool isStart = true;
            string cmd = string.Format("select t.* from dev_run_status_detail t where t.status_id='0A01' and t.status_value='00' and t.t.station_id='{0}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                if (dt != null &&
                    dt.Rows.Count > 0)
                {
                    return dt.Rows.Count == GetCurrentStationDeviceCount();
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private int GetCurrentStationDeviceCount()
        {
            string cmd = string.Format("select  count(*) from basi_dev_info t where t.station_id='{0}'  and subStr(t.device_id,5,2) in ('01', '04', '06', '02')", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            DataTable dt = DBCommon.Instance.GetDatatable(cmd);
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0][0]);
            return 0;
        }

        /// <summary>
        /// 检查是否部份设备处在运营开始状态
        /// </summary>
        /// <returns>如果处在运营开始返回true，否则返回false</returns>
        public bool CheckPartDevStart(out int devicesNum,out int totalNum)
        {
            bool isStart = false;
            devicesNum = 0;
            totalNum = 0;
            string cmd = string.Format("select t.* from dev_run_status_detail t where t.status_id='0A01' and t.status_value='00' and t.station_id='{0}' and subStr(t.device_id, 5, 2) in ('01', '06', '04','02') ", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                if (dt != null && 
                    dt.Rows.Count > 0&&
                    dt.Rows.Count<GetCurrentStationDeviceCount())
                {
                    devicesNum = dt.Rows.Count;
                    totalNum = GetCurrentStationDeviceCount();
                    isStart = true;
                }

                return isStart;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 检查是否部份设备处在运营结束状态
        /// </summary>
        /// <returns>如果处在运营结束返回true，否则返回false</returns>
        public bool CheckPartDevEnd(out int deviceNum,out int totalNum)
        {
            deviceNum = 0;
            totalNum = 0;
            bool isStart = false;
            string cmd = string.Format("select t.* from dev_run_status_detail t where t.status_id='0A01' and t.status_value='03' and t.station_id='{0}' and subStr(t.device_id, 5, 2) in ('01', '06', '04','02')", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                if (dt != null && 
                    dt.Rows.Count > 0&&
                    dt.Rows.Count<GetCurrentStationDeviceCount())
                {
                    isStart = true;
                    deviceNum = dt.Rows.Count;
                    totalNum = GetCurrentStationDeviceCount();
                }

                return isStart;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查是否全部设备处在运营结束状态
        /// </summary>
        /// <returns>如果处在运营开始返回true，否则返回false</returns>
        public bool CheckHasDevEnd()
        {
            //bool isStart = true;
            string cmd = string.Format("select t.* from dev_run_status_detail t where t.status_id='0A01' and t.status_value='03' and t.station_id='{0}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                if (dt != null &&
                    dt.Rows.Count > 0)
                {
                    return dt.Rows.Count == GetCurrentStationDeviceCount();
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查是否处在运营结束状态
        /// </summary>
        /// <returns>如果处在运营结束返回true，否则返回false</returns>
        public bool CheckHasRunEnd()
        {
            BasiStationInfo staInfo = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            string deviceId = staInfo.device_id;
            string cmd = string.Format("select t.status_value from dev_run_status_detail t where t.status_id='0A01' and t.device_id='{0}'", deviceId);
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                int res = 0;
                int.TryParse(dt.Rows[0][0].ToString(), out res);
                return res == 3;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获得运营日
        /// </summary>
        /// <returns>返回运营日</returns>
        public string GetRunDate()
        {
            //todo: get data from basi_run_param_info
            string cmd = string.Format("select t.param_value from basi_run_param_info t where t.param_code='0003'");
            string runDate = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                runDate = DateTime.ParseExact(dt.Rows[0][0].ToString(), "yyyyMMdd", null).ToString("yyyy年MM月dd日");
            }
            catch (Exception ex)
            {
                return DateTime.Now.ToString("yyyy年MM月dd日");
            }
            return runDate;
        }

        /// <summary>
        /// 获得运营日库存现金总金额
        /// </summary>
        /// <returns>返回运营日库存现金总金额</returns>
        public string GetCashTotalStoreValue(string stationId, string rundate)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select sum(nvl(t.currency_num, 0) * nvl(bmti.currency_value, 0)*100) as all_total_currency_value "
                  + "from cash_storage_info t "
                  + "left join basi_money_type_info bmti on bmti.currency_code = "
                  + "t.currency_code where t.station_id like '" + stationId + "' " 
                  + "and t.currency_code = '00' ";
            string CashTotalStoreValue = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                CashTotalStoreValue = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return CashTotalStoreValue == "" ? "0" : CashTotalStoreValue;
        }

        /// <summary>
        /// 获得运营日库存硬币总金额
        /// </summary>
        /// <returns>返回运营日库存硬币总金额</returns>
        public string GetCoinTotalStoreValue(string stationId, string rundate)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select sum(nvl(t.currency_num, 0) * nvl(bmti.currency_value, 0)*100) as all_total_currency_value "
                  + "from cash_storage_info t "
                  + "left join basi_money_type_info bmti on bmti.currency_code = "
                  + "t.currency_code where t.station_id like '" + stationId 
                  + "' and t.currency_code = '11'";
            string CoinTotalStoreValue = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                CoinTotalStoreValue = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return CoinTotalStoreValue == "" ? "0" : CoinTotalStoreValue;
        }

        /// <summary>
        /// 获得待解行现金总金额
        /// </summary>
        /// <returns>返回待解行现金总金额</returns>
        public string GetCashBankTotalValue(string stationId)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select sum(t.total_value) as all_total_currency_value from cash_waiting_to_bank_info t left join basi_money_type_info btmt on btmt.currency_code = t.currency_code where t.station_id like '"
                + stationId + "'";
            string CashTotalStoreValue = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                CashTotalStoreValue = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return CashTotalStoreValue == "" ? "0" : CashTotalStoreValue;
        }

        /// <summary>
        /// 获得今日待解行现金总金额
        /// </summary>
        /// <returns>返回今日待解行现金总金额</returns>
        public string GetTodayCashBankTotalValue(string stationId,string runDate)
        {  /*
            *  if(DateTime.Now>=RunDate+"0200" && DateTime.Now<RunDate+"0600")
            * 
            * 
            * */
            //todo: get data from basi_run_param_info
            string cmd = "select sum(t.original_money-t.later_money) from cash_store_change_log t where t.change_type = '04' and t.station_id like '" + stationId + "' and t.run_date = '" + runDate + "'";
            string CashTotalStoreValue = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                CashTotalStoreValue = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return CashTotalStoreValue == "" ? "0" : CashTotalStoreValue;
        }

        /// <summary>
        /// 通过TVM硬币钱箱清点和BOM的操作员归还相加获得壹元硬币数量
        /// </summary>
        /// <returns>返回今壹元硬币数量</returns>
        public string GetTodayCoinTotalValue(string stationId, string runDate)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select sum(t.original_money - t.later_money) as total_money "
               + "from cash_store_change_log t"
               + "where t.currency_code = '11'"
               + "and t.change_type = '06'"
               + "and t.station_id like '" + stationId + "' and t.run_date = '" + runDate + "'";
            string TodayCoinTotalValue = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                TodayCoinTotalValue = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return TodayCoinTotalValue;
        }

        /// <summary>
        /// 判断今天是否解行
        /// </summary>
        /// <returns>判断今天是否解行，返回‘空’代表没有解行，有值代表已经解行</returns>
        public string GetTodayIsOrNotBank(string stationId, string runDate)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select sum(t.original_money-t.later_money) from cash_store_change_log t where t.change_type='03'"
               + " and t.station_id like '" + stationId + "' and t.run_date = '" + runDate + "'";
            string TomorrowIncomeAmount = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                TomorrowIncomeAmount = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return TomorrowIncomeAmount;
        }

        /// <summary>
        /// 获得明日解行金额
        /// </summary>
        /// <returns>获得明日解行金额</returns>
        public string GetTomorrowIncomeAmount(string stationId, string runDate)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select sum(nvl(t.key_fld2,0)) from dev_double_oper_log_info t where t.busi_code = '1615'"
               + " and t.station_id like '" + stationId + "' and t.oper_date||t.oper_time between '" + runDate + "020000' and '" + DateTime.ParseExact(runDate, "yyyyMMdd", null).AddDays(1).ToString("yyyyMMdd") + "020000'";
            string TomorrowIncomeAmount = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                TomorrowIncomeAmount = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return TomorrowIncomeAmount == "" ? "0" : TomorrowIncomeAmount;
        }

        /// <summary>
        /// 昨日累计盘盈：从清算日到昨天的“今日差异合计”的总和
        /// </summary>
        /// <returns>昨日累计盘盈</returns>
        public string GetYesterdayIncomeAmount(string stationId, string runDate)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select t1.today_diff_amount "
            + " from (select t.today_diff_amount "
            + " from cash_date_settlement_info t "
            + " where t.station_id like '" + stationId + "' "
            + " and t.run_date <= '" + runDate + "' "
            + " order by t.run_date desc)t1 where rownum = 1";
            string YesterdayIncomeAmount = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                YesterdayIncomeAmount = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return YesterdayIncomeAmount == "" ? "0" : YesterdayIncomeAmount;
        }
        /// <summary>
        /// 所有待解行的钱。cash_waiting_to_bank_info中的total_value值
        /// </summary>
        /// <returns>所有待解行的钱</returns>
        public string GetCashWaitingToBankTotalValue(string stationId)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select t.total_value "
               + "from cash_waiting_to_bank_info t "
               + "where t.station_id like '" + stationId + "'";
            string CashWaitingToBankTotalValue = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                CashWaitingToBankTotalValue = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return CashWaitingToBankTotalValue == "" ? "0" : CashWaitingToBankTotalValue;
        }

        /// <summary>
        /// 获得运营日库存票卡总数量
        /// </summary>
        /// <returns>返回运营日库存票卡总数量</returns>
        public string GetTicketsTotalStoreNum(string stationId, string rundate)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select sum(nvl(t.in_store_num,0)) as all_total_num from tick_storage_info t where t.station_id like '" + stationId + "' and t.run_date = '" + rundate + "'";
            string TicketsTotalStoreNum = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                TicketsTotalStoreNum = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return TicketsTotalStoreNum;
        }

        /// <summary>
        /// 获得运营日TVM收益总金额
        /// </summary>
        /// <returns>返回运营日TVM收益总金额</returns>
        public string GetTVMIncomeValue(string stationId, string rundate)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select sum(nvl(t.cash_sail_sum,0) + nvl(t.fault_trans_sum,0)) as income_sum from data_tvm_settlement_info t where t.settlement_date = '" + rundate + "' and t.station_id like '" + stationId + "'";
            string CashTotalStoreValue = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                CashTotalStoreValue = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return CashTotalStoreValue==""?"0":CashTotalStoreValue;
        }

        /// <summary>
        /// 获得运营日BOM收益总金额
        /// </summary>
        /// <returns>返回运营日BOM收益总金额</returns>
        public string GetBOMIncomeValue(string stationId, string rundate)
        {
            //todo: get data from basi_run_param_info
            string cmd = "select sum(decode(t.tran_type,'06',-t.deposit_amount,'07',-t.deposit_amount,'09',t.deposit_amount,'81',t.deposit_amount,'0')+decode(t.tran_type,'06',-t.tran_amount,'07',-t.tran_amount,'EF',-t.tran_amount,'EE',-t.tran_amount,t.tran_amount)+t.fee_amount+t.cost_amount) as income_sum from data_dev_settlement_info t where t.station_id like '" + stationId + "' and t.settlement_date = '" + rundate + "'";
            string CashTotalStoreValue = string.Empty;
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                CashTotalStoreValue = dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
            return CashTotalStoreValue == "" ? "0" : CashTotalStoreValue;
        }

        /// <summary>
        /// SCWS获得运营日结算各值
        /// </summary>
        /// <returns>返回运营日结算各值/returns>
        public CashDateSettlementInfo GetStationCashDateSettlementInfoValue(string stationId, string rundate)
        {
            //todo: get data from basi_run_param_info
            CashDateSettlementInfo cds = new CashDateSettlementInfo();
            string cmd =
"select t.tickets_remain," +
       "t.today_cash_bank_total," +
       "t.today_diff_amount," +
       "t.coin_store_amount," +
       "t.tvm_income," +
       "t.urgency_tikets_income," +
       "t.others_income," +
       "t.bom_income," +
       "t.group_tickets_income," +
       "t.account_income," +
       "t.tomorrow_bank_income," +
       "t.today_income_amount," +
       "t.income_store," +
       "t.yesterday_income_amount," +
       "t.today_subway_income " +
       "from cash_date_settlement_info t where t.station_id = '" + stationId + "' and t.run_date = '" + rundate + "'";
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                cds.tickets_remain = (dt.Rows[0][0].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][0]);
                cds.today_cash_bank_total = (dt.Rows[0][1].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][1]);
                cds.today_diff_amount = (dt.Rows[0][2].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][2]);
                cds.coin_store_amount = (dt.Rows[0][3].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][3]);
                cds.tvm_income = (dt.Rows[0][4].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][4]);
                cds.urgency_tikets_income = (dt.Rows[0][5].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][5]);
                cds.others_income = (dt.Rows[0][6].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][6]);
                cds.bom_income = (dt.Rows[0][7].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][7]);

                cds.group_tickets_income = (dt.Rows[0][8].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][8]);
                cds.account_income = (dt.Rows[0][9].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][9]);
                cds.tomorrow_bank_income = (dt.Rows[0][10].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][10]);
                cds.today_income_amount = (dt.Rows[0][11].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][11]);
                cds.income_store = (dt.Rows[0][12].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][12]);
                cds.yesterday_income_amount = (dt.Rows[0][13].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][13]);
                cds.today_subway_income = (dt.Rows[0][14].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][14]);
            }
            catch (Exception ex)
            {
                return null;
            }
            return cds;
        }

        /// <summary>
        /// LCWS获得运营日结算各值
        /// </summary>
        /// <returns>返回运营日结算各值/returns>
        public CashDateSettlementInfo GetCashDateSettlementInfoValue(string stationId, string rundate)
        {
            //todo: get data from basi_run_param_info
            CashDateSettlementInfo cds = new CashDateSettlementInfo();
            string cmd = 
"select sum(t.tickets_remain) as tickets_remain,"+
       "sum(t.today_cash_bank_total) as today_cash_bank_total,"+
       "sum(t.today_diff_amount) as today_diff_amount,"+
       "sum(t.coin_store_amount) as coin_store_amount,"+
       "sum(t.tvm_income) as tvm_income,"+
       "sum(t.urgency_tikets_income) as urgency_tikets_income,"+
       "sum(t.others_income) as others_income,"+
       "sum(t.bom_income) as bom_income,"+ 
       "sum(t.group_tickets_income) as group_tickets_income,"+
       "sum(t.account_income) as account_income,"+
       "sum(t.tomorrow_bank_income) as tomorrow_bank_income,"+
       "sum(t.today_income_amount) as today_income_amount,"+
       "sum(t.income_store) as income_store,"+
       "sum(t.yesterday_income_amount) as yesterday_income_amount,"+
       "sum(t.today_subway_income) as today_subway_income "+
       "from cash_date_settlement_info t where t.station_id != '" + stationId + "' and t.run_date = '" + rundate + "'";
            try
            {
                DataTable dt = DBCommon.Instance.GetDatatable(cmd);
                cds.tickets_remain = (dt.Rows[0][0].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][0]);
                cds.today_cash_bank_total = (dt.Rows[0][1].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][1]);
                cds.today_diff_amount = (dt.Rows[0][2].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][2]);
                cds.coin_store_amount = (dt.Rows[0][3].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][3]);
                cds.tvm_income = (dt.Rows[0][4].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][4]);
                cds.urgency_tikets_income = (dt.Rows[0][5].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][5]);
                cds.others_income = (dt.Rows[0][6].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][6]);
                cds.bom_income = (dt.Rows[0][7].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][7]);

                cds.group_tickets_income = (dt.Rows[0][8].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][8]);
                cds.account_income = (dt.Rows[0][9].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][9]);
                cds.tomorrow_bank_income = (dt.Rows[0][10].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][10]);
                cds.today_income_amount = (dt.Rows[0][11].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][11]);
                cds.income_store = (dt.Rows[0][12].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][12]);
                cds.yesterday_income_amount = (dt.Rows[0][13].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][13]);
                cds.today_subway_income = (dt.Rows[0][14].ToString() == "") ? 0 : Convert.ToDecimal(dt.Rows[0][14]);
            }
            catch (Exception ex)
            {
                return null;
            }
            return cds;
        }

        /// <summary>
        /// 返回运营状态，运营开始 或者运营失败
        /// </summary>
        /// <returns></returns>
        public string GetRunStatus()
        {
            //todo:get data from stats_value_detail_info
            return string.Empty;
        }


        /// <summary>
        /// 监听设备轮询数据表线程
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int StartDevRunMonitor(string messageType)
        {
            int res = 0;
            devRunStartThread = new Thread(new ThreadStart(() =>
            {

                Message msg = new Message();
                msg.MessageType = messageType;
                while (true)
                {
                    Thread.Sleep(5000);// 为了避免线程死锁
                    msg.Content = this.GetAllDevRunStatus(SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
                    MessageManager.SendMessasge(msg);
                    //todo:need send devStatus
                }
            }));
            this.devRunStartThread.Name = messageType + "Thread";
            this.devRunStartThread.IsBackground = true;
            this.devRunStartThread.Start();
            return 0;
        }

        /// <summary>
        /// 停止设备论表线程
        /// </summary>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int AbortDevRunMonitor()
        {
            if (this.devRunStartThread == null)
            {
                WriteLog.Log_Error("plase call start thread  first!");
                return -1;
            }
            try
            {
                devRunStartThread.Abort();

                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
       
        }
    }





}
