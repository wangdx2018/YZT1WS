using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.RfidRW;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.ModelView.Convertors;
using AFC.WS.BR;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.Config;
using AFC.WS.UI.CommonControls;
using System.Windows;
using AFC.WS.BR.LogManager;
using AFC.WS.Model.Const;

namespace AFC.WS.ModelView.Actions.TickMonyBoxManager
{
    public class MoneyBoxClearAction : IAction, IDoublePrimissionHandler
    {

        #region IAction 成员


        string moneyBoxID = string.Empty;
        string operatorID = string.Empty;
        string deviceID = string.Empty;
        int beforeMoney = 0;
        int totalMoney = 0;

        MoneyBoxRFID rfid = null;
        MoneyBoxInOrOutBody body = null;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            ConvertYuanToFen convertYuanToFen = new ConvertYuanToFen();
            TicketOrMoneyBoxIdConvetor coverHex = new TicketOrMoneyBoxIdConvetor();
            if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxID")).value != null)
            {
                moneyBoxID = coverHex.ConvertBack(actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxID")).value.ToString(), null, null, null).ToString();
            }

            if (actionParamsList.Single(temp => temp.bindingData.Equals("operatorID")).value != null)
            {
                operatorID = actionParamsList.Single(temp => temp.bindingData.Equals("operatorID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("beforeMoney")).value != null)
            {
                beforeMoney = convertYuanToFen.Convert(actionParamsList.Single(temp => temp.bindingData.Equals("beforeMoney")).value.ToString(), null, null, null).ToString().ToInt32();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("totalMoney")).value != null)
            {
                totalMoney = convertYuanToFen.Convert(actionParamsList.Single(temp => temp.bindingData.Equals("totalMoney")).value.ToString(), null, null, null).ToString().ToInt32();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("rfid")).value != null)
            {
                rfid = (MoneyBoxRFID)actionParamsList.Single(temp => temp.bindingData.Equals("rfid")).value;
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxInOrOutBody")).value != null)
            {
                body = (MoneyBoxInOrOutBody)actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxInOrOutBody")).value;
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("deviceID")).value != null)
            {
                deviceID = actionParamsList.Single(temp => temp.bindingData.Equals("deviceID")).value.ToString();
            }

            if (moneyBoxID.Length != 8)
            {
                Wrapper.ShowDialog("请输入八位钱箱编码。");
                return false;
            }
            if (body.Body.Count == 0)
            {
                Wrapper.ShowDialog("请输入钱箱清点实际数量。");
                return false;
            }
            if (string.IsNullOrEmpty(deviceID) || "00000000".Equals(deviceID))
            {
                Wrapper.ShowDialog("请输入设备编码。");
                return false;
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            int dbRsult = 0;

            int logRest = 0;
            string logCurrencyCode = TickMonyBoxHelp.MultipleCurrency;
            WriteLog.Log_Info("WriteRfid开始" + DateTime.Now.ToString());
            //写RDID
            if (WriteRfid(body))
            {

                WriteLog.Log_Info("WriteRfid结束" + DateTime.Now.ToString());
                /////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////
                //2011.7.12 delete
                //if (rfid == null)
                //{

                //开启事务
                Util.DataBase.BeginTransaction();

                CashBoxStatusInfo status = TickMonyBoxHelp.Instance.GetCashMoneyBoxStatusInfo(moneyBoxID);
                if (status == null || string.IsNullOrEmpty(status.money_box_id))
                {
                    Wrapper.ShowDialog("钱箱清点失败,失败原因是钱箱状态表不存在。");
                    return null;
                }

                status.currency_num = 0;
                /////////////////////////
                ///总金额赋值
                //////////////////////////
                status.total_money_value = 0;
                status.box_position = "01";
                ConvertYuanToFen yanToFen = new ConvertYuanToFen();
                //记录操作流水
                LogManager log = new LogManager();
                dbRsult = TickMonyBoxHelp.Instance.updateCashMoneyStatusInfo(status);
                if (dbRsult != 1)
                {
                    Util.DataBase.Rollback();
                    Wrapper.ShowDialog("钱箱清点失败,失败原因是钱箱状态表操作失败。");
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Clear_Action, "1", "钱箱清点失败,失败原因是数据库操作失败");
                    return null;
                }

                int seq = 0;
                int replaceSN = Util.DataBase.GetSequenceNextVal(out seq, "busi_udsn_sec");

                //20120807 dusj modify  begin金额核算不区分币种
                CashStorageInfo storageInfo = new CashStorageInfo();
                decimal currencyNum = 0;
                for (int i = 0; i < body.Body.Count; i++)
                {

                    MoneyTypeCodeInfo moneyTypeCodeInfo = body.Body[i];
                    //CashStorageInfo storageInfo = new CashStorageInfo();
                    //storageInfo.currency_code = moneyTypeCodeInfo.MoneyTypeCode;
                    //storageInfo.currency_num = moneyTypeCodeInfo.Cash;
                    //storageInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                    //storageInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                    //storageInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
                    //storageInfo.update_time = DateTime.Now.ToString("HHmmss");
                    currencyNum = currencyNum + moneyTypeCodeInfo.Cash;

                }

                storageInfo.currency_code = TickMonyBoxHelp.MultipleCurrency;
                storageInfo.currency_num = currencyNum;
                storageInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                storageInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                storageInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
                storageInfo.update_time = DateTime.Now.ToString("HHmmss");


                //多币种值
                int MultipleValue = yanToFen.Convert(TickMonyBoxHelp.Instance.GetMoneyTypeValueByID(TickMonyBoxHelp.MultipleCurrency).currency_value, null, null, null).ToString().ToInt32();
                //查询多币种库存
                CashStorageInfo tempStore = TickMonyBoxHelp.Instance.GetMoneyTypeCodeInfo(storageInfo);

                //更新现金库存表
                dbRsult = TickMonyBoxHelp.Instance.insertCashStorageInfo(storageInfo, (int)TickMonyBoxHelp.MoneyBoxPutOrClear.清点);

                //记录库存变化流水
                int changeRest = log.AddCashStoreLog("06", MultipleValue * tempStore.currency_num, MultipleValue * tempStore.currency_num + totalMoney, TickMonyBoxHelp.MultipleCurrency, string.Empty);





                //20120828 dusj modify begin 如果是硬币增加对硬币的有关记录(硬币库存只是数量，不在统计总库存之内。多币种为库存总量，硬币只是记录当前车站的数据)
                string typeCode = moneyBoxID.Substring(2, 2);
                if (typeCode.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.硬币回收箱)
                {
                    //一元硬币面值
                    int CoinValue = yanToFen.Convert(TickMonyBoxHelp.Instance.GetMoneyTypeValueByID(TickMonyBoxHelp.CoinCurrency).currency_value, null, null, null).ToString().ToInt32();


                    storageInfo.currency_code = TickMonyBoxHelp.CoinCurrency;
                    //查询硬币库存
                    CashStorageInfo CoinStore = TickMonyBoxHelp.Instance.GetMoneyTypeCodeInfo(storageInfo);

                    CashStorageInfo insertStore = new CashStorageInfo();
                    insertStore.currency_code = TickMonyBoxHelp.CoinCurrency;
                    insertStore.currency_num = currencyNum;
                    insertStore.line_id = storageInfo.line_id;
                    insertStore.station_id = storageInfo.station_id;
                    insertStore.total_currency_num = 0;
                    insertStore.update_date = DateTime.Now.ToString("yyyyMMdd");
                    insertStore.update_time = DateTime.Now.ToString("HHmmss");
                    insertStore.total_currency_num = CoinStore.total_currency_num;
                    insertStore.yesterday_total_num = CoinStore.total_currency_num;

                    //更新现金库存表
                    int coinRsult = TickMonyBoxHelp.Instance.insertCashStorageInfo(insertStore, (int)TickMonyBoxHelp.MoneyBoxPutOrClear.清点);
                    int coinChangeRest = log.AddCashStoreLog("06", CoinValue * CoinStore.currency_num, CoinValue * CoinStore.currency_num + totalMoney, TickMonyBoxHelp.CoinCurrency, string.Empty);
                    logCurrencyCode = TickMonyBoxHelp.CoinCurrency;
                }
                //2012.12.25 dusj modify begin 设备编码取得操作员输入
                //2012.10.12 dusj modify begin 设备编码取得
                //string deviceCode = TickMonyBoxHelp.Instance.GetCashBoxReplace(moneyBoxID).device_id;
                //if (string.IsNullOrEmpty(deviceCode))
                //{
                //    deviceCode = SysConfig.GetSysConfig().LocalParamsConfig.DeviceCode;
                //}

                //logRest = log.WriteMoneyBoxOperation(replaceSN, moneyBoxID, "03", logCurrencyCode, beforeMoney, currencyNum, totalMoney, deviceCode);
                //20120828 dusj modify end
                //2012.10.12 dusj modify end 
              
                logRest = log.WriteMoneyBoxOperation(replaceSN, moneyBoxID, "03", logCurrencyCode, beforeMoney, currencyNum, totalMoney, deviceID);
                //2012.12.25 dusj modify end 设备编码取得操作员输入

                if (dbRsult <1 || logRest<1 || changeRest < 1)
                {
                    Wrapper.ShowDialog("钱箱清点失败,失败原因是数据库操作失败。");
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Clear_Action, "1", "钱箱清点失败,失败原因是数据库操作失败");
                    Util.DataBase.Rollback();
                    return null;
                }

                //}
                //20120807 dusj modify  end金额核算不区分币种
                WriteLog.Log_Info("插入车站现金库存信息表" + DateTime.Now.ToString());

                Util.DataBase.Commit();
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Clear_Action, "0", "清点钱箱成功");
                Wrapper.ShowDialog("清点钱箱成功。");
                return new ResultStatus { resultCode = 0, resultData = 0 };

                //2011.7.12 delete  
                //}

            }
            return null;

        }
        /// <summary>
        /// 写RFID操作。
        /// </summary>
        /// <param name="body">钱箱领用归还结构体</param>

        public bool WriteRfid(MoneyBoxInOrOutBody body)
        {
            try
            {
                if (rfid == null)
                {
                    return true;
                }
                string boxType = moneyBoxID.Substring(2, 2);
                if (boxType.ToHexNumberInt32() != (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.硬币回收箱)
                {
                    return true;
                }

                rfid.moneyBoxLocationId = (byte)TickMonyBoxHelp.MoneyBoxPositionState.在库;
                rfid.lastOperatorTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                rfid.MoneyTotalNumber = 0;
                rfid.MoneyTotalCount = 0;
                bool isSucessWrite = false;
                int writeCount = 0;
                bool isWriteContinue = true;
                int result = 0;
                //连续写入三次
                while (writeCount < 3 && isWriteContinue == true)
                {
                    result = BuinessRule.GetInstace().rfidRw.WriteMoneyBoxRFID(rfid, (byte)TickMonyBoxHelp.RfidSerialPortsChannelNumber.一通道);
                    writeCount = writeCount + 1;
                    if (result == 0)
                    {
                        {

                            isWriteContinue = false;
                            isSucessWrite = true;
                        }
                    }
                }
                if (isSucessWrite == true)
                {
                }
                else
                {
                    Wrapper.ShowDialog("RFID写入失败。");
                    rfid.moneyBoxLocationId = (byte)TickMonyBoxHelp.MoneyBoxPositionState.操作员手中;

                }

                return isSucessWrite;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return false;
            }
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            return true;
        }

        #endregion
    }
}
