﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using AFC.WS.UI.RfidRW;
using AFC.WS.UI.CommonControls;
using System.Data;
using AFC.WS.BR.LogManager;
using AFC.WS.UI.BR.Data;

namespace AFC.WS.BR.TickMonyBoxManager
{
  
    /// <summary>
    /// 插入钱箱登记表
    /// </summary>
    public class TickMonyBoxHelp
    {
        /// <summary>
        /// 创建唯一管理类对象
        /// </summary>
        private static TickMonyBoxHelp _Instance;

        /// <summary>
        /// 创建唯一管理类对象
        /// </summary>
       public static TickMonyBoxHelp Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new TickMonyBoxHelp();
                }
                return _Instance;
            }
        }

       //多币种
       public const string MultipleCurrency = "00";
       //硬币
       public const string CoinCurrency = "11";

        /// <summary>
        /// 插入钱箱注册表
        /// </summary>
        /// <param name="cashMoneyBoxInfo"></param>
        /// <returns></returns>
       public int insertMoneyBoxRegInfo(CashBoxRegistorInfo cashMoneyBoxInfo)
       {
           if (cashMoneyBoxInfo == null)
               return 0;
           //检索数据是否存在
           CashBoxRegistorInfo r = GetCashMoneyBoxRegInfo(cashMoneyBoxInfo.money_box_id);
           if (!string.IsNullOrEmpty(r.money_box_id) == true)
           {
               return -2;
           }
           int result = DBCommon.Instance.InsertTable(cashMoneyBoxInfo, "cash_box_registor_info");
           return result;
       }

       /// <summary>
       /// 获取钱箱登记信息。
       /// </summary>
       /// <param name="moneyBoxId">钱箱ID。</param>
       /// <returns>获取钱箱登记信息</returns>
       public CashBoxRegistorInfo GetCashMoneyBoxRegInfo(string moneyBoxId)
       {
           try
           {
               string sqlQuery = string.Format("select t.* from cash_box_registor_info t ");
               sqlQuery += string.Format(" where t.money_box_id = '{0}'", moneyBoxId);
               return DBCommon.Instance.SetModelValue<CashBoxRegistorInfo>(sqlQuery);
           }
           catch (Exception ee)
           {
               Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
               return null;
           }

       }


       /// <summary>
       /// 插入现金库存表
       /// </summary>
       /// <param name="moneyStorage"></param>
       /// <param name="method"></param>
       /// <returns></returns>
       public int insertCashStorageInfo(CashStorageInfo moneyStorage,int method)
        {
            int result =0;

            CashStorageInfo storage = GetMoneyTypeCodeInfo(moneyStorage);
            if (storage == null || string.IsNullOrEmpty (storage.currency_code))
            {
                if (method == (int)MoneyBoxPutOrClear.清点)
                {
                    moneyStorage.currency_num = moneyStorage.currency_num;
                }
                //压钱
                else
                {
                    moneyStorage.currency_num = -moneyStorage.currency_num;
                }
                result = DBCommon.Instance.InsertTable(moneyStorage, "cash_storage_info");
                if (result != 1)
                {
                    result = - 1;
                }
            }
            else
            {
                if (method == (int)MoneyBoxPutOrClear.清点)
                {
                    moneyStorage.currency_num = moneyStorage.currency_num + storage.currency_num;
                }
                //压钱
                else
                {
                    moneyStorage.currency_num = storage.currency_num - moneyStorage.currency_num;
                }
                List<KeyValuePair<string, string>>  tempList = new List<KeyValuePair<string,string>>();
                tempList.Add(new KeyValuePair<string, string>("LINE_ID",moneyStorage.line_id));
                tempList.Add(new KeyValuePair<string, string>("STATION_ID",moneyStorage.station_id));
                tempList.Add(new KeyValuePair<string, string>("CURRENCY_CODE",moneyStorage.currency_code));
                result = DBCommon.Instance.UpdateTable(moneyStorage, "cash_storage_info", tempList.ToArray());
                if (result != 1)
                {
                    result= - 1;
                }
            }
            return result;
        }
        /// <summary>
        /// 取得现金库存表信息
        /// </summary>
        /// <param name="moneyType"></param>
        /// <returns></returns>
        public CashStorageInfo GetMoneyTypeCodeInfo(CashStorageInfo moneyStorage)
        {
            try
            {
                string sqlQuery = string.Format("select t.* from cash_storage_info t where t.line_id='{0}'  and t.station_id ='{1}' and t.currency_code='{2}'", moneyStorage.line_id, moneyStorage.station_id, moneyStorage.currency_code);
                return DBCommon.Instance.SetModelValue<CashStorageInfo>(sqlQuery);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return null;
            }
        }

       

       /// <summary>
       /// 获取钱箱状态。
       /// </summary>
       /// <param name="moneyBoxId">钱箱ID。</param>
       /// <returns>获取钱箱状态信息</returns>
        public CashBoxStatusInfo GetCashMoneyBoxStatusInfo(string moneyBoxId)
       {
           try
           {
               string lineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
               string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
               string sqlQuery = string.Format("select t.* from cash_box_status_info t where t.money_box_id = '{0}' and t.line_id='{1}' and t.station_id='{2}'", moneyBoxId, lineID, stationID);
               return DBCommon.Instance.SetModelValue<CashBoxStatusInfo>(sqlQuery);
           }
           catch (Exception ee)
           {
               Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
               return null;
           }
       }

        public CashBoxReplaceInfo GetCashBoxReplace(string moneyBoxId)
        {
            try
            {
                string lineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                string sqlQuery = string.Format("select * from(select t.* from cash_box_replace_info t where t.replace_type='01' and t.money_box_id = '{0}' and t.line_id='{1}' and t.station_id='{2}' order by t.update_date||t.update_time desc) where rownum=1", moneyBoxId, lineID, stationID);
                return DBCommon.Instance.SetModelValue<CashBoxReplaceInfo>(sqlQuery);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return null;
            }
        }

        /// <summary>
        /// 获取所有在库钱箱状态。
        /// </summary>
        /// <param name="moneyBoxId">钱箱ID。</param>
        /// <returns>获取钱箱状态信息</returns>
        public List<CashBoxStatusInfo> GetAllMoneyOutInfo()
        {
            try
            {
                string lineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                string sqlQuery = string.Format("select t.* from cash_box_status_info t where t.box_position='01' and t.currency_num=0 and t.line_id='{0}' and t.station_id='{1}'", lineID, stationID);
                return DBCommon.Instance.SetTModelValue<CashBoxStatusInfo>(sqlQuery);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return null;
            }
        }

       /// <summary>
       /// 检查钱箱的合法性。
       /// </summary>
       /// <param name="moneyBoxId">钱箱</param>
       /// <param name="status"></param>
       /// <returns>true-合法；false-非法</returns>
        public bool CheckedMoneyBoxId(string moneyBoxId, out CashBoxStatusInfo status)
       {
           if (JudgeMoneyBoxIsLegality(moneyBoxId) == false)
           {
               Wrapper.ShowDialog("钱箱编码输入错误，请重新输入。");
               status = null;
               return false;
           }
           //-->判断钱箱是否登记。
           //-->判断钱箱的状态表中是否存在此记录。
           CashBoxRegistorInfo r = GetCashMoneyBoxRegInfo(moneyBoxId);
           CashBoxStatusInfo s = GetCashMoneyBoxStatusInfo(moneyBoxId);
           if ((r.money_box_id.JudgeIsNullOrEmpty() == true) ||
               r.station_id.Equals(SysConfig.GetSysConfig().LocalParamsConfig.StationCode) == false ||
               s == null ||
               s.money_box_id.JudgeIsNullOrEmpty() == true)
           {
               Wrapper.ShowDialog("请确认该钱箱是否登记或是否为本站钱箱!");
               status = s;
               return false;
           }
           status = s;
           return true;
       }



        /// <summary>
        /// 插入钱箱状态表
        /// </summary>
        /// <param name="cashMoneyStatusInfo"></param>
        /// <returns></returns>
       public int insertCashMoneyStatusInfo(CashBoxStatusInfo cashMoneyStatusInfo)
       {
           if (cashMoneyStatusInfo == null)
               return 0;
           int result = DBCommon.Instance.InsertTable(cashMoneyStatusInfo, "cash_box_status_info");
           return result;
       }

        /// <summary>
        /// 插入结帐流水表
        /// </summary>
        /// <param name="recMoney"></param>
        /// <param name="operationInMoney"></param>
        /// <param name="realMoney"></param>
        /// <returns></returns>

       public int insertDataOperSettlementInfo(string operationCode,string settleDate,int recMoney, int operationInMoney, int realMoney)
       {
           int result = 0;
           DataOperSettlementInfo  info= BuinessRule.GetInstace().GetDataOperSettlementInfo(operationCode, settleDate);
           if (info != null && !string.IsNullOrEmpty(info.operator_id))
           {
               info.in_oper_money = operationInMoney;
               info.real_rece_money = info.real_rece_money + realMoney;
               info.sys_rece_money = info.sys_rece_money + recMoney;
               info.update_date = DateTime.Now.ToString("yyyyMMdd");
               info.update_time = DateTime.Now.ToString("HHmmss");

               List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
               tempList.Add(new KeyValuePair<string, string>("OPERATOR_ID", operationCode));
               tempList.Add(new KeyValuePair<string, string>("RUN_DATE", settleDate));

               result = DBCommon.Instance.UpdateTable(info, "data_oper_settlement_info", tempList.ToArray());
           }
           else
           {

               DataOperSettlementInfo operSettleInfo = new DataOperSettlementInfo();
               operSettleInfo.operator_id = operationCode;
               operSettleInfo.run_date = settleDate;
               operSettleInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
               operSettleInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
               operSettleInfo.in_oper_money = operationInMoney;
               operSettleInfo.real_rece_money = realMoney;
               operSettleInfo.sys_rece_money = recMoney;
               operSettleInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
               operSettleInfo.update_time = DateTime.Now.ToString("HHmmss");
               result= DBCommon.Instance.InsertTable(operSettleInfo, "data_oper_settlement_info");
           }
           return result == 1 ? 1 : -1;            
       }
        /// <summary>
        /// 修改结帐流水表
        /// </summary>
        /// <param name="operationCode"></param>
        /// <param name="settleDate"></param>
        /// <param name="recMoney"></param>
        /// <param name="operationInMoney"></param>
        /// <param name="realMoney"></param>
        /// <returns></returns>
       public int updateDataOperSettlementInfo(string operationCode, string settleDate, int realMoney)
       {
           int result = 0;
           DataOperSettlementInfo  info= BuinessRule.GetInstace().GetDataOperSettlementInfo(operationCode, settleDate);
           if (info != null && !string.IsNullOrEmpty(info.operator_id))
           {
               info.real_rece_money = info.real_rece_money + realMoney;
               info.update_date = DateTime.Now.ToString("yyyyMMdd");
               info.update_time = DateTime.Now.ToString("HHmmss");
               List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
               tempList.Add(new KeyValuePair<string, string>("OPERATOR_ID", operationCode));
               tempList.Add(new KeyValuePair<string, string>("RUN_DATE", settleDate));

               result = DBCommon.Instance.UpdateTable(info, "data_oper_settlement_info", tempList.ToArray());
           }

           return result == 1 ? 1 : -1;
       }


        /// <summary>
        /// 删除钱箱状态表
        /// </summary>
        /// <param name="cashMoneyStatusInfo"></param>
        /// <returns></returns>
       public int delCashMoneyStatusInfo(CashBoxStatusInfo cashMoneyStatusInfo)
       {
            int res = 0;
            string delSql = string.Format("delete cash_box_status_info t where t.line_id='{0}' and t.station_id='{1}' and t.money_box_id ='{2}'", cashMoneyStatusInfo.line_id,cashMoneyStatusInfo.station_id,cashMoneyStatusInfo.money_box_id);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                if (res == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return -1;
            }
       }

       /// <summary>
       /// 删除钱箱状态表
       /// </summary>
       /// <param name="cashMoneyStatusInfo"></param>
       /// <returns></returns>
       public int delCashMoneyStatusInfo(string moneyboxID)
       {
           int res = 0;
           string lineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
           string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
           string delSql = string.Format("delete cash_box_status_info t where t.line_id='{0}' and t.station_id='{1}' and t.money_box_id ='{2}'", lineID, stationID, moneyboxID);
           try
           {
               Util.DataBase.SqlCommand(out res, delSql);
               if (res == 0)
               {
                   return 1;
               }
               else
               {
                   return 0;
               }
           }
           catch (Exception ex)
           {
               AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
               return 0;
           }
       }

       /// <summary>
       /// 删除钱箱登记表
       /// </summary>
       /// <param name="cashMoneyStatusInfo"></param>
       /// <returns></returns>
       public int delCashMoneyResgister(string moneyboxID)
       {
           int res = 0;
           string lineID = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
           string stationID = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
           string delSql = string.Format("delete cash_box_registor_info t where t.line_id ='{0}' and t.station_id='{1}' and t.money_box_id ='{2}'", lineID, stationID, moneyboxID);
           try
           {
               Util.DataBase.SqlCommand(out res, delSql);
               if (res == 0)
               {
                   return 1;
               }
               else
               {
                   return 0;
               }
           }
           catch (Exception ex)
           {
               AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
               return 0;
           }
       }

       /// <summary>
       /// 更新钱箱状态表
       /// </summary>
       /// <param name="cashMoneyStatusInfo"></param>
       /// <returns></returns>
       public int updateCashMoneyStatusInfo(CashBoxStatusInfo cashMoneyStatusInfo)
       {

           int result = 0;
           if (cashMoneyStatusInfo == null)
               return 0;
        //   string cmd = string.Format("update cash_box_status_info t set t.box_position='{3}' ,t.currency_num='{4}' ,t.TOTAL_MONEY_VALUE  where t.line_id='{0}' and t.station_id='{1}' and t.money_box_id='{2}'", cashMoneyStatusInfo.line_id, cashMoneyStatusInfo.station_id, cashMoneyStatusInfo.money_box_id, cashMoneyStatusInfo.box_position);
        // int ret = 0;
        //return  Util.DataBase.SqlCommand(out ret, cmd);

           CashBoxStatusInfo status = GetCashMoneyBoxStatusInfo(cashMoneyStatusInfo.money_box_id);
           if (status != null && !string.IsNullOrEmpty(status.money_box_id))
           {
               List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
               tempList.Add(new KeyValuePair<string, string>("LINE_ID", cashMoneyStatusInfo.line_id));
               tempList.Add(new KeyValuePair<string, string>("STATION_ID", cashMoneyStatusInfo.station_id));
               tempList.Add(new KeyValuePair<string, string>("MONEY_BOX_ID", cashMoneyStatusInfo.money_box_id));
               result = DBCommon.Instance.UpdateTable(cashMoneyStatusInfo, "cash_box_status_info", tempList.ToArray());
           }
           else
           {
               result = DBCommon.Instance.InsertTable(cashMoneyStatusInfo, "cash_box_status_info");
               if (result != 1)
               {
                   result = -1;
               }
           }
           return result;
       }

       /// <summary>
       /// 修改钱币库存表(现金调入调出解行和操作员领用归还现金时调用)
       /// </summary>
       /// <param name="moneyType">钱币类型</param>
       /// <param name="moneyNum">数量</param>
       /// <param name="mothod">方法(0:现金调入；1：现金调出；2：操作员归还；3：操作员领用 4:现金待解行(操作方式同现金调出) 5:现金解行)</param>
       /// <returns></returns>
       public int updateStorageInfo(string moneyType,decimal moneyNum,int mothod)
       {
           ////////////////////////////////////////////
           //2012.08.03 增加库存变化日志原因
           //以前：00调入，01调出，02调整，03解行
           //现在：00调入，01调出，02调整，03解行， 04待解行，05钱箱补充，06钱箱清点，07操作员领用,08，操作员归还
           ////////////////////////////////////////////
           int result = 1;
           int logResult = 1;
          
           decimal beforNum = 0;
           string changeType= string.Empty;
           if (moneyType == null)
               return 0;
           uint currValue = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo().Where(p => p.currency_code == moneyType).GetTContext<BasiMoneyTypeInfo>().currency_value.ConvertNumberStringToUint();
           CashStorageInfo storage = BuinessRule.GetInstace().GetCashStorageInfoByCashCode(moneyType);

           switch (mothod)
           {
               case 0:
                   changeType = "00";
                   break;
               case 1:
                   changeType = "01";
                   break;
               case 2:
                   changeType = "08";
                   break;
               case 3:
                   changeType = "07";
                   break;
               case 4:
                   changeType = "04";
                   break;
               case 5:
                   changeType = "03";
                   break;
           }
           //车站现金库存信息表有此币种代码
           if (storage != null && !string.IsNullOrEmpty(storage.currency_code))
           {
               beforNum = storage.currency_num;
               //现金调入和操作员归还增加库存
               if (mothod == 0 || mothod==2) 
               {
                   storage.currency_num = storage.currency_num + moneyNum;
                 
               }
               //现金调出和操作员领用减少库存
               if (mothod == 1 || mothod ==3)
               {
                   storage.currency_num = storage.currency_num - moneyNum >0 ? (storage.currency_num - moneyNum):0;
               }
               //现金待解行
               if (mothod == 4)
               {
                   storage.currency_num = storage.currency_num - moneyNum > 0 ? (storage.currency_num - moneyNum) : 0;
               }
               storage.update_date = DateTime.Now.ToString("yyyyMMdd");
               storage.update_time = DateTime.Now.ToString("HHmmss");
               List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
               tempList.Add(new KeyValuePair<string, string>("LINE_ID", storage.line_id));
               tempList.Add(new KeyValuePair<string, string>("STATION_ID", storage.station_id));
               tempList.Add(new KeyValuePair<string, string>("CURRENCY_CODE", storage.currency_code));
               
               //处理业务不是解行更新现金库存
               if (mothod != 5)
               {
                   result = DBCommon.Instance.UpdateTable(storage, "cash_storage_info", tempList.ToArray());
                   //记录现金变化日志
                   logResult = BuinessRule.GetInstace().logManager.AddCashStoreLog(changeType, currValue * beforNum * 100, currValue * storage.currency_num * 100, moneyType, "");
               }
               else
               {
                   //记录现金变化日志
                   logResult = BuinessRule.GetInstace().logManager.AddCashStoreLog(changeType, currValue * beforNum * 100, currValue * beforNum * 100, moneyType, "");
               }

               
           }
           //车站现金库存信息表没有此币种代码 
           else
           {
               //现金调入和操作员归还增加库存
               if (mothod == 0 || mothod == 2)
               {
                   changeType = "00";
                   beforNum = 0;
                   CashStorageInfo inStorage = new CashStorageInfo();
                   inStorage.currency_code = moneyType;
                   inStorage.currency_num = moneyNum;
                   inStorage.total_currency_num = moneyNum;
                   inStorage.yesterday_total_num = 0;
                   inStorage.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                   inStorage.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                   inStorage.update_date = DateTime.Now.ToString("yyyyMMdd");
                   inStorage.update_time = DateTime.Now.ToString("HHmmss");
                   result = DBCommon.Instance.InsertTable(inStorage, "cash_storage_info");
                   logResult = BuinessRule.GetInstace().logManager.AddCashStoreLog(changeType, 0, currValue * inStorage.currency_num * 100, moneyType, "");
               }
           }
          
           return result * logResult  == 1 ? 1 : -1;
       }





       /// <summary>
       /// 修改操作员现金表
       /// </summary>
       /// <param name="cashMoneyStatusInfo"></param>
       /// <returns></returns>
       public int updateCashInOperatorInfo(string operatorCode,string moneyType, decimal moneyNum, int mothod)
       {

           int result = 0;
           if (string.IsNullOrEmpty(moneyType))
               return 0;
           if (string.IsNullOrEmpty(operatorCode))
               return 0;
           CashInOperatorInfo cashInOperator = BuinessRule.GetInstace().GetCahInOperatorByKey(operatorCode, moneyType);
           if (cashInOperator != null && !string.IsNullOrEmpty(cashInOperator.operator_id))
           {
               //增加库存,操作员现金领用
               if (mothod == 0)
               {
                   cashInOperator.cash_in_hand = cashInOperator.cash_in_hand + moneyNum;
               }
               //减少库存,操作员现金归还
               if (mothod == 1)
               {
                   cashInOperator.cash_in_hand = 0;
               }
               cashInOperator.update_date = DateTime.Now.ToString("yyyyMMdd");
               cashInOperator.update_time = DateTime.Now.ToString("HHmmss");
               List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
               tempList.Add(new KeyValuePair<string, string>("LINE_ID", cashInOperator.line_id));
               tempList.Add(new KeyValuePair<string, string>("STATION_ID", cashInOperator.station_id));
               tempList.Add(new KeyValuePair<string, string>("CURRENCY_CODE", cashInOperator.currency_code));
               tempList.Add(new KeyValuePair<string, string>("OPERATOR_ID", cashInOperator.operator_id));
               result = DBCommon.Instance.UpdateTable(cashInOperator, "cash_in_operator_info", tempList.ToArray());
           }
           else
           {
               //增加库存,操作员现金领用
               if (mothod == 0)
               {
                   CashInOperatorInfo inOperator = new CashInOperatorInfo();
                   inOperator.currency_code = moneyType;
                   inOperator.operator_id = operatorCode;
                   inOperator.cash_in_hand = moneyNum;
                   inOperator.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                   inOperator.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                   inOperator.update_date = DateTime.Now.ToString("yyyyMMdd");
                   inOperator.update_time = DateTime.Now.ToString("HHmmss");
                   result = DBCommon.Instance.InsertTable(inOperator, "cash_in_operator_info");
                   if (result != 1)
                   {
                       result = -1;
                   }
               }
               else
               {
                   return 1;
               }

           }
           return result;
       }

       public int updateCashInOperatorInfo(string operatorCode)
       {
           string line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
           string station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
           string cmd = string.Format("update cash_in_operator_info t set t.cash_in_hand = 0  where t.line_id='{0}'  and t.station_id='{1}'  and t.operator_id ='{2}'", line_id, station_id, operatorCode);
           int res = 0;
           Util.DataBase.SqlCommand(out res, cmd);
           if (res < 0)
           {
               res = -1;
           }
           else
           {
               res = 1;
           }
           return res;
       }

       public int UpdateDataSettlementInfo(string operationCode, string settleDate)
       {
           string cmd = string.Format("update data_dev_settlement_info t set t.settlement_status='00' where t.operator_id ='{0}' and  t.settlement_date='{1}'", operationCode, settleDate);
           int res = 0;
           Util.DataBase.SqlCommand(out res, cmd);
           if (res < 0)
           {
               res = -1;
           }
           else
           {
               res = 1;
           }
           return res;
       }

        /// <summary>
        /// 修改现金库存量(库存调整时调用)
        /// </summary>
        /// <param name="moneyCode"></param>
        /// <param name="updateNum"></param>
        /// <returns></returns>
       public int UpdateCashSotreInfo(string moneyCode, decimal updateNum, string remark, string adjustMethod)
       {
           int res = 0;
           int logRes = 0;
           decimal beforeNum = 0;
           if (string.IsNullOrEmpty(moneyCode))
           {
               WriteLog.Log_Error("input params error moneyCode is null or empty");
               return -1;
           }

           //开启事务
           Util.DataBase.BeginTransaction();

           uint currValue = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo().Where(p => p.currency_code == moneyCode).GetTContext<BasiMoneyTypeInfo>().currency_value.ConvertNumberStringToUint();
           CashStorageInfo info = BuinessRule.GetInstace().GetCashStorageById(moneyCode);
           if (info != null && !string.IsNullOrEmpty(info.currency_code))
           {
               beforeNum = info.currency_num;

                switch (adjustMethod)
                {
                        //总量调整
                    case "0":
                        info.currency_num = updateNum;
                        break;
                        //正向调整
                    case "1":
                        info.currency_num = info.currency_num + updateNum;
                        break;
                        //负向调整
                    case "2":
                        info.currency_num = info.currency_num - updateNum > 0 ? (info.currency_num - updateNum) : 0;
                        break;
                }
               info.update_date = DateTime.Now.ToString("yyyyMMdd");
               info.update_time = DateTime.Now.ToString("HHmmss");
               List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
               tempList.Add(new KeyValuePair<string, string>("LINE_ID", info.line_id));
               tempList.Add(new KeyValuePair<string, string>("STATION_ID", info.station_id));
               tempList.Add(new KeyValuePair<string, string>("CURRENCY_CODE", info.currency_code));

               res = DBCommon.Instance.UpdateTable<CashStorageInfo>(info, "cash_storage_info", tempList.ToArray());
           }
           else
           {
               beforeNum = 0;
               CashStorageInfo insCashStorage = new CashStorageInfo();
               insCashStorage.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
               insCashStorage.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
               insCashStorage.currency_code = moneyCode;
               insCashStorage.currency_num = updateNum;
               insCashStorage.total_currency_num = updateNum;
               insCashStorage.yesterday_total_num = 0;
               insCashStorage.update_date = DateTime.Now.ToString("yyyyMMdd");
               insCashStorage.update_time = DateTime.Now.ToString("HHmmss");
               res = DBCommon.Instance.InsertTable<CashStorageInfo>(insCashStorage, "cash_storage_info");
           }
           //库存调整
           logRes = BuinessRule.GetInstace().logManager.AddCashStoreLog("02", beforeNum * currValue * 100, info.currency_num * currValue * 100, moneyCode, remark);

           
           if (res * logRes != 1)
           {
               Util.DataBase.Rollback();
               return -1;
           }

           if (!string.IsNullOrEmpty(remark))
           {
               AFC.WS.BR.LogManager.LogManager log = new AFC.WS.BR.LogManager.LogManager();
               res = log.AddRemarkInfo(BuinessRule.GetInstace().OperatorId, "现金调整", remark);
               if (res != 0)
               {
                   WriteLog.Log_Error("insert remark_log info error!");
                   Util.DataBase.Rollback();
                   return -1;
               }
               res = 1;
           }

           Util.DataBase.Commit();
           return res * logRes == 1 ? 1 : -1;
       }



       /// <summary>
       /// 判断钱箱是否合法；true-合法；false-非法。
       /// </summary>
       /// <param name="moneyBoxId">钱箱ID</param>
       /// <returns>true-合法；false-非法</returns>
       public bool JudgeMoneyBoxIsLegality(string moneyBoxId)
       {
           bool result = true;
           string content = moneyBoxId;
           if (string.IsNullOrEmpty(content))
           {
               Wrapper.Instance.ConsoleWriteLine("钱箱编码为空。", LogFlag.ErrorFormat);
               return false;
           }
          
           if (content.Length == 8)
           {
               
               if (!content.Substring(0, 2).Equals(SysConfig.GetSysConfig().LocalParamsConfig.LineCode))
               {
                   Wrapper.Instance.ConsoleWriteLine(content.Substring(0, 2) + " 非本线路的钱箱。", LogFlag.ErrorFormat);
                   result = false;
               }
           }
           else
           {
               Wrapper.Instance.ConsoleWriteLine("钱箱长度不正确。", LogFlag.ErrorFormat);
               result = false;
           }

           return result;
       }
       /// <summary>
       /// 获取钱箱、票箱的安装位置对应的名称。
       /// </summary>
       /// <param name="setupLocation">安装位置</param>
       /// <returns></returns>
       public string GetTicketMoneyBoxInstallPosition(byte setupLocation)
       {
           return Enum.GetName(typeof(TicketMoneyBoxInstallPosition), setupLocation);
       }
       /// <summary>
       /// 获取钱箱RFID信息。 
       /// </summary>
       /// <returns>返回钱箱RFID结构体</returns>
       public MoneyBoxRFID ReadMoneyBoxRFIDInfo()
       {
           int retCode = -1;
           MoneyBoxRFID mb = BuinessRule.GetInstace().rfidRw.ReadMoneyBoxRFID(1, out retCode);
           if (mb != null && retCode == 0)
           {
               return mb;
           }
           else
           {
               return null;
           }
       }

       /// <summary>
       /// 获取钱箱位置状态
       /// 1-在库、2-在操作员手中、3-设备上。
       /// </summary>
       /// <param name="b">钱箱位置状态</param>
       /// <returns>返回钱箱位置状态名称</returns>
       public string GetMoneyBoxLocationState(byte b)
       {
           string result = "";
           switch (b)
           {
               case 0x01:
                   result = "在库";
                   break;
               case 0x02:
                   result = "操作员手中";
                   break;
               case 0x03:
                   result = "设备上";
                   break;
               default:
                   break;
           }
           return result;
       }

       /// <summary>
       /// 获取钱箱操作状态。
       /// </summary>
       /// <param name="b">钱箱操作状态</param>
       /// <returns>返回钱箱操作状态名称</returns>
       public string GetMoneyBoxOperateState(byte b)
       {
           string result = "";
           switch (b)
           {
               case 0x01:
                   result = "正常安装";
                   break;
               case 0x02:
                   result = "非法安装";
                   break;
               case 0x03:
                   result = "正常卸下";
                   break;
               case 0x04:
                   result = "非法卸下";
                   break;
               default:
                   break;
           }
           return result;
       }


       public string GetMoneyBoxType(int typeCode)
       {
           string result = "";
           switch (typeCode)
           {
               case 0x11:
                   result = "硬币回收箱";
                   break;
               case 0x22:
                   result = "纸币回收箱";
                   break;
               case 0x21:
                   result = "纸币补充箱";
                   break;
               default:
                   break;
           }
           return result;
       }

       /// <summary>
       /// 获取币种名称。
       /// </summary>
       /// <param name="b">币种代码</param>
       /// <returns>返回币种名称</returns>
       public string GetMoneyTypeCodeName(string b)
       {
           string result = "";
           if (b == "00")
           {
               result = "多币种";
           }
           else
           {
               List<BasiMoneyTypeInfo> item =BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo();
               if (item != null)
               {
                   foreach (var a in item)
                   {
                       if (a.currency_code == b)
                       {
                           result = a.currency_name;
                           break;
                       }
                   }
               }
           }
           return result;
       }
       /// <summary>
       /// 获取钱的数量。
       /// </summary>
       /// <param name="tb">TextBox控件。</param>
       /// <returns>返回输入钱的数量</returns>
       public int GetMoneyValue(TextBoxExtend tb)
       {
           int result = 0;
           string name = tb.Name;
           try
           {
               string FactValue = name.Remove(0, 7);
               string value = tb.Text;
               result = FactValue.ToInt32() * value.ToInt32();
           }
           catch (Exception ee)
           {
               Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
           }
           return result;
       }


        /// <summary>
        /// 从BOM结算表单中取得结算金额
        /// </summary>
        /// <param name="operationCode"></param>
        /// <param name="settleDate"></param>
        /// <returns></returns>
       public decimal getSettleMoney(string operationCode, string settleDate)
       {
           decimal settleMoney = 0;
           //结算金额
         //List<DataDevSettlementInfo> settleInfo = BuinessRule.GetInstace().GetDevSettlementInfo(operationCode, settleDate);
           //if (settleInfo != null && settleInfo.Count > 0)
           //{
           //    foreach (DataDevSettlementInfo info in settleInfo)
           //    {
           //        settleMoney = settleMoney + info.cost_amount + info.deposit_amount + info.fee_amount + info.tran_amount;
           //    }
           //}

           string cmd = "select sum(decode(t.tran_type,'06',-t.deposit_amount,'07',-t.deposit_amount,'EF',-t.deposit_amount,'EE',-t.deposit_amount,t.deposit_amount)+decode(t.tran_type,'06',-t.tran_amount,'07',-t.tran_amount,'EF',-t.tran_amount,'EE',-t.tran_amount,t.tran_amount)+t.fee_amount+t.cost_amount) as income_sum from data_dev_settlement_info t where t.settlement_status='01' and t.operator_id ='" + operationCode + "' and t.settlement_date = '" + settleDate + "'";

           try
           {
               DataTable dt = DBCommon.Instance.GetDatatable(cmd);
               settleMoney = Convert.ToDecimal(dt.Rows[0][0].ToString());
           }
           catch (Exception ex)
           {
               return settleMoney;
           }
           return settleMoney;

       }

        /// <summary>
        /// 取得操作员备用金
        /// </summary>
        /// <param name="operationCode"></param>
        /// <param name="settleDate"></param>
        /// <returns></returns>
        public decimal getOperationInMoney(string operationCode, string settleDate)
        {
            decimal settleMoney = 0;
           List<CashInOperatorInfo> cashOperationInfo = BuinessRule.GetInstace().GetCahInOperatorInfo(operationCode);
           if (cashOperationInfo != null && cashOperationInfo.Count > 0)
           {
               foreach (CashInOperatorInfo cashInfo in cashOperationInfo)
               {
                   int currValue = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo().Where(p => p.currency_code == cashInfo.currency_code).GetTContext<BasiMoneyTypeInfo>().currency_value.ToInt32();
                   settleMoney = settleMoney + currValue * cashInfo.cash_in_hand*100;
               }
           }
           return settleMoney;

       }
        /// <summary>
        /// 取得操作员归还现金时实际归还与应归还的差额
        /// </summary>
        /// <param name="operationCode"></param>
        /// <param name="settleDate"></param>
        /// <returns></returns>
        public decimal getDifference(string operationCode, string settleDate)
        {
            decimal settleMoney = 0;


            DataOperSettlementInfo dataOperationInfo = BuinessRule.GetInstace().GetDataOperSettlementInfo(operationCode, settleDate);
            if (dataOperationInfo != null && !string.IsNullOrEmpty(dataOperationInfo.operator_id))
            {
                //差额=营收金额+备用金-实际归还金额
                //settleMoney = settleMoney + (dataOperationInfo.sys_rece_money + dataOperationInfo.in_oper_money - dataOperationInfo.real_rece_money);         
                //2011.7.20 modify
                //差额= 实际归还金额-营收金额-备用金
                settleMoney = settleMoney + (dataOperationInfo.real_rece_money - dataOperationInfo.sys_rece_money - dataOperationInfo.in_oper_money);         
            }
            return settleMoney/100;

        }

       /// <summary>
       /// 获取钱的数量。
       /// </summary>
       /// <param name="tb">TextBox控件。</param>
       /// <returns>返回输入钱的数量</returns>
       public int GetMoneyValue(TextBoxExtend tb, int index)
       {
           int result = 0;
           string name = tb.Name;
           try
           {
               string FactValue = name.Remove(0, index);
               string value = tb.Text;
               result = FactValue.ToInt32() * value.ToInt32();
           }
           catch (Exception ee)
           {
               Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
           }
           return result;
       }

       public BasiMoneyTypeInfo GetMoneyTypeValueByID(string moneyCode)
       {
           return DBCommon.Instance.GetModelValue<BasiMoneyTypeInfo>(string.Format("select t.* from basi_money_type_info t where t.currency_code ='{0}'", moneyCode));
       }

       public string GetSequenceNextVal()
       {
           int seq = 0;
           string dateTime = DateTime.Now.ToString("yyyyMMdd");
           seq = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec");
           return dateTime+seq.ToString();
       }

       public List<MoneyCashType> GetMoneyCashTypeList()
       {
           List<MoneyCashType> cashType = new List<MoneyCashType>();
           MoneyCashType type1 = new MoneyCashType();
           type1.name = "硬币回收箱";
           type1.code = "11";
           cashType.Add(type1);

           MoneyCashType type2 = new MoneyCashType();
           type2.name = "纸币补充箱";
           type2.code = "21";
           cashType.Add(type2);

           MoneyCashType type3 = new MoneyCashType();
           type3.name = "纸币回收箱";
           type3.code = "22";
           cashType.Add(type3);

           return cashType;
       }


       /// <summary>
       /// 钱箱位置状态枚举。
       /// </summary>
       public enum MoneyBoxPositionState : byte
       {
           操作员手中 = 0x02,
           设备上 = 0x03,
           在库 = 0x01,
           
          
       }

       /// <summary>
       /// RFID串口通道号
       /// </summary>
       public enum RfidSerialPortsChannelNumber : byte
       {
           一通道 = 1,
           二通道 = 2,
           三通道 = 3,
           四通道 = 4
       }

       /// <summary>
       /// 钱箱类型代码
       /// </summary>
       public enum MoneyBoxTypeCode : int
       {
           硬币回收箱 = 0x11,
           纸币回收箱 = 0x22,
           纸币补充箱= 0x21
       }

       /// <summary>
       /// 清点还是压钱方法
       /// </summary>
       public enum MoneyBoxPutOrClear : int
       {
           压钱 = 1,           
           清点 =2
       }

       public enum TicketMoneyBoxInstallPosition
       {
           未知位置 = 0x00,
           票箱1位置 = 0x01,
           票箱2位置 = 0x02,
           回收箱位置 = 0x03,
           纸币识别回收箱位置 = 0x04,
           纸币补充箱1位置 = 0x05,
           纸币补充箱2位置 = 0x06,
           纸币回收箱位置 = 0x07,
           硬币补充箱1位置 = 0x08,
           硬币补充箱2位置 = 0x09,
           硬币回收箱1位置 = 0x0A,
           硬币回收箱2位置 = 0x0B,
           AG票箱1位置 = 0x0C,
           AG票箱2位置 = 0x0D,
           AG回收箱位置 = 0x0E,
           硬币Hopper1位置 = 0x0F,/*!< */
           硬币Hopper2位置 = 0x10,/*!<  */
           未定义 = 0xFF
       }


       /// <summary>
       /// 待解行和解行时调用
       /// </summary>
       /// <param name="mothod">4:待解行；5：解行</param>
       /// <param name="moneyCode">币种代码</param>
       /// <param name="updateMoney">更新金额</param>
       /// <param name="adjustMethod">0:总数调整、1:正向调整、2:负向调整</param>
       /// <param name="moneyWait">原有待解行金额</param>
       /// <returns></returns>
       public int  UpdateCashWaitingInfo(int mothod,string moneyCode, decimal updateMoney,string adjustMethod,decimal moneyWait,params string[] remark)
       {
           int res = 0;
           int logResult = 0;
           decimal beforeNum = 0;
           decimal afterNum = 0;
           decimal beforeWaitNum = 0;
           decimal changNum = 0;
           if (string.IsNullOrEmpty(moneyCode))
           {
               WriteLog.Log_Error("input params error moneyCode is null or empty");
               return -1;
           }
           //开启事务
           Util.DataBase.BeginTransaction();

           uint currValue = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo().Where(p => p.currency_code == moneyCode).GetTContext<BasiMoneyTypeInfo>().currency_value.ConvertNumberStringToUint() * 100;
           CashWaitingToBankInfo info = BuinessRule.GetInstace().GetCashWaitingById(moneyCode);
           //获得待解行金额，修改待解行库存表
           if (info != null && !string.IsNullOrEmpty(info.currency_code))
           {
               beforeWaitNum = info.total_value;
               //待解行时待解行库存增加
               if (mothod == 4)
               {
                   switch (adjustMethod)
                   {
                          
                       //总量调整
                       case "0":
                           info.total_value = Convert.ToInt32(currValue * updateMoney);
                           changNum = updateMoney - moneyWait;
                           break;
                       //正向调整
                       case "1":
                           info.total_value = Convert.ToInt32(info.total_value + currValue * updateMoney);
                           changNum = updateMoney;
                           break;
                       //负向调整
                       case "2":
                           info.total_value = Convert.ToInt32(info.total_value - currValue * updateMoney) > 0 ? Convert.ToInt32(info.total_value - currValue * updateMoney) : 0;
                           changNum = -updateMoney;
                           break;
                   }                   
                 
               }
               //解行时待解行库存减少
               else
               {
                   info.total_value = Convert.ToInt32(info.total_value - currValue * updateMoney) > 0 ? Convert.ToInt32(info.total_value - currValue * updateMoney) : 0;
             
               }
               
               info.run_date = DateTime.Now.ToString("yyyyMMdd");
               info.update_date = DateTime.Now.ToString("yyyyMMdd");
               info.update_time = DateTime.Now.ToString("HHmmss");
               List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
               tempList.Add(new KeyValuePair<string, string>("LINE_ID", info.line_id));
               tempList.Add(new KeyValuePair<string, string>("STATION_ID", info.station_id));
               tempList.Add(new KeyValuePair<string, string>("CURRENCY_CODE", info.currency_code));

                res = DBCommon.Instance.UpdateTable<CashWaitingToBankInfo>(info, "cash_waiting_to_bank_info", tempList.ToArray());
           }
           //业务是只允许做待解行
           else
           {
              // beforeWaitNum = currValue * updateMoney;
               changNum = updateMoney;
               CashWaitingToBankInfo insCashStorage = new CashWaitingToBankInfo();
               insCashStorage.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
               insCashStorage.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
               insCashStorage.currency_code = moneyCode;
               insCashStorage.total_value = currValue * updateMoney;
               insCashStorage.run_date = DateTime.Now.ToString("yyyyMMdd");
               insCashStorage.update_date = DateTime.Now.ToString("yyyyMMdd");
               insCashStorage.update_time = DateTime.Now.ToString("HHmmss");
               res = DBCommon.Instance.InsertTable<CashWaitingToBankInfo>(insCashStorage, "cash_waiting_to_bank_info");
           }
           if (res != 1)
           {
               WriteLog.Log_Error("insert cash_waiting_to_bank_info  error!");
               Util.DataBase.Rollback();
               return -1;
           }
           //获得库存金额,修改库存
           CashStorageInfo storageInfo = BuinessRule.GetInstace().GetCashStorageById(moneyCode);
           if (storageInfo != null && !string.IsNullOrEmpty(storageInfo.currency_code))
           {
               beforeNum = storageInfo.currency_num;
               //现在库存=过去的库存量-解行增量

               storageInfo.currency_num = beforeNum - changNum;
               
              
               storageInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
               storageInfo.update_time = DateTime.Now.ToString("HHmmss");
               List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
               tempList.Add(new KeyValuePair<string, string>("LINE_ID", storageInfo.line_id));
               tempList.Add(new KeyValuePair<string, string>("STATION_ID", storageInfo.station_id));
               tempList.Add(new KeyValuePair<string, string>("CURRENCY_CODE", storageInfo.currency_code));
               afterNum = storageInfo.currency_num;
               //记录解行现金变化日志
               if (mothod == 4)
               {
                   res = DBCommon.Instance.UpdateTable<CashStorageInfo>(storageInfo, "cash_storage_info", tempList.ToArray());
                   logResult = BuinessRule.GetInstace().logManager.AddCashStoreLogForWaitBank("04", currValue * beforeNum, currValue * afterNum, moneyCode, "");

                   if (remark!=null&&
                       remark.Length>0&&
                       !string.IsNullOrEmpty(remark[0]))
                   {
                       AFC.WS.BR.LogManager.LogManager log = new AFC.WS.BR.LogManager.LogManager();
                       res = log.AddRemarkInfo(BuinessRule.GetInstace().OperatorId, "现金待解行", remark[0]);
                       if (res != 0)
                       {
                           WriteLog.Log_Error("insert remark_log info error!");
                           Util.DataBase.Rollback();
                           return -1;
                       }
                       res = 1;
                   }
               }
               //记录待解行现金变化日志（不变化）
               //2013.1.6 解行金额的变化前为变化金额+当前金额
               //2013.1.24 增加changNum = updateMoney;
               else
               {
                   changNum = updateMoney;
                   logResult = BuinessRule.GetInstace().logManager.AddCashStoreLog("03", currValue * (changNum + beforeNum), currValue * beforeNum, moneyCode, "");

                   if (remark != null &&
                    remark.Length > 0 &&
                    !string.IsNullOrEmpty(remark[0]))
                   {
                       AFC.WS.BR.LogManager.LogManager log = new AFC.WS.BR.LogManager.LogManager();
                       res = log.AddRemarkInfo(BuinessRule.GetInstace().OperatorId, "现金解行", remark[0]);
                       if (res != 0)
                       {
                           WriteLog.Log_Error("insert remark_log info error!");
                           Util.DataBase.Rollback();
                           return -1;
                       }
                       res = 1;
                   }
               }
            
           }
           else
           {
               beforeNum = 0;
               afterNum = updateMoney;
               CashStorageInfo insCashStorage = new CashStorageInfo();
               insCashStorage.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
               insCashStorage.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
               insCashStorage.currency_code = moneyCode;
               insCashStorage.currency_num = updateMoney;
               insCashStorage.total_currency_num = updateMoney;
               insCashStorage.yesterday_total_num = 0;
               insCashStorage.update_date = DateTime.Now.ToString("yyyyMMdd");
               insCashStorage.update_time = DateTime.Now.ToString("HHmmss");
               //记录解行现金变化日志
               if (mothod == 4)
               {
                   res = DBCommon.Instance.InsertTable<CashStorageInfo>(insCashStorage, "cash_storage_info");
                   logResult = BuinessRule.GetInstace().logManager.AddCashStoreLog("04", 0, currValue * afterNum, moneyCode, "");
               }
               //记录待解行现金变化日志（不变化）
               else
               {
                   logResult = BuinessRule.GetInstace().logManager.AddCashStoreLog("03", 0, 0, moneyCode, "");
               }

              
           }


           if (res * logResult != 1)
           {
               Util.DataBase.Rollback();
               return -1;
           }
           Util.DataBase.Commit();
           return res * logResult == 1 ? 1 : -1;
       }       
    }
}
