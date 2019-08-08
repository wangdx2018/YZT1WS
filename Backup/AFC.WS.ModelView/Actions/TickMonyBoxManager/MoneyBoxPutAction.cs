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
    /// <summary>
    /// edit by wangdx 20121001 
    /// 
    /// 删除了领用钱箱流水
    /// </summary>
    public class MoneyBoxPutAction : IAction
    {

        #region IAction 成员

        string moneyBoxID = string.Empty;
        string moneyType = string.Empty;
        string moneyNum = string.Empty;
        string operatorID = string.Empty;
        string deviceID = string.Empty;
        MoneyBoxRFID rfid = null;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            try
            {
                TicketOrMoneyBoxIdConvetor coverHex = new TicketOrMoneyBoxIdConvetor();
                if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxID")).value != null)
                {
                    moneyBoxID = actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxID")).value.ToString();
                }
                if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyType")).value != null)
                {
                    moneyType = actionParamsList.Single(temp => temp.bindingData.Equals("moneyType")).value.ToString();
                }
                if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyNum")).value != null)
                {
                    moneyNum = actionParamsList.Single(temp => temp.bindingData.Equals("moneyNum")).value.ToString();
                }
                if (actionParamsList.Single(temp => temp.bindingData.Equals("rfid")).value != null)
                {
                    rfid = (MoneyBoxRFID)actionParamsList.Single(temp => temp.bindingData.Equals("rfid")).value;
                }
                if (actionParamsList.Single(temp => temp.bindingData.Equals("operatorID")).value != null)
                {
                    operatorID = actionParamsList.Single(temp => temp.bindingData.Equals("operatorID")).value.ToString();
                }
                //2012.12.25 dusj modify begin
                if (actionParamsList.Single(temp => temp.bindingData.Equals("deviceID")).value != null)
                {
                    deviceID = actionParamsList.Single(temp => temp.bindingData.Equals("deviceID")).value.ToString();
                }
                if (string.IsNullOrEmpty(deviceID) || "00000000".Equals(deviceID))
                {
                    Wrapper.ShowDialog("请选择设备。");
                    return false;
                }
                //2012.12.25 dusj modfify end
                if (string.IsNullOrEmpty(moneyType))
                {
                    Wrapper.ShowDialog("请选择币种信息。");
                    return false;
                }
                if (moneyBoxID.Length != 8)
                {
                    Wrapper.ShowDialog("请输入八位钱箱编码。");
                    return false;
                }
                //20120817 dusj add begin
                CashBoxStatusInfo status = null;
                bool isNotExist = TickMonyBoxHelp.Instance.CheckedMoneyBoxId(moneyBoxID, out status);
                if (isNotExist == false)
                {
                    Wrapper.ShowDialog("[" + moneyBoxID + "]此钱箱未登记。");
                    return false;
                }
                string typeCode = moneyBoxID.Substring(2, 2);
                if (typeCode.ToHexNumberInt32() == (int)AFC.WS.BR.TickMonyBoxManager.TickMonyBoxHelp.MoneyBoxTypeCode.纸币补充箱)
                {
                }
                else
                {
                    if (!"0".Equals(moneyNum))
                    {
                        Wrapper.ShowDialog("非纸币补充箱不能补充，请输入正确的钱箱。");
                        return false;
                    }
                }
                //if (!CheckRfidMoneyBoxLocationIdIsValid(moneyBoxID))
                //{
                //    return false;
                //}

                return true;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return false;
            }
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {

            WriteLog.Log_Info("WriteRfid开始" + DateTime.Now.ToString());
            //写RDID
            if (WriteRfid())
            {
                WriteLog.Log_Info("WriteRfid结束" + DateTime.Now.ToString());
                //开启事务
                Util.DataBase.BeginTransaction();
                int dbRsult = 1;

                ConvertYuanToFen yanToFen = new ConvertYuanToFen();
                //画面传过来的币种代码*数量
                int moneyValue = yanToFen.Convert(TickMonyBoxHelp.Instance.GetMoneyTypeValueByID(moneyType).currency_value, null, null, null).ToString().ToInt32();
                //需要压到钱箱中的钱
                int putmoney = moneyValue * moneyNum.ToInt32();
                //多币种值
                int MultipleValue = yanToFen.Convert(TickMonyBoxHelp.Instance.GetMoneyTypeValueByID(TickMonyBoxHelp.MultipleCurrency).currency_value, null, null, null).ToString().ToInt32();
                //多币种的数量
                int putMutipleNum = putmoney / MultipleValue;
                //更新状态表的钱币种类和数量
                CashBoxStatusInfo cashBoxStatus = new CashBoxStatusInfo();
                cashBoxStatus.money_box_id = moneyBoxID;
                cashBoxStatus.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                cashBoxStatus.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                cashBoxStatus.update_date = DateTime.Now.ToString("yyyyMMdd");
                cashBoxStatus.update_time = DateTime.Now.ToString("HHmmss");
                cashBoxStatus.box_position = ((byte)TickMonyBoxHelp.MoneyBoxPositionState.操作员手中).ToString("x2");
                cashBoxStatus.currency_code = TickMonyBoxHelp.MultipleCurrency;
                cashBoxStatus.currency_num = putMutipleNum;
                cashBoxStatus.total_money_value = putmoney;
                dbRsult = TickMonyBoxHelp.Instance.updateCashMoneyStatusInfo(cashBoxStatus);


                ////edit by wangdx 
                ////记钱箱操作流水
                //BuinessRule.GetInstace().logManager.WriteMoneyBoxOperation(moneyBoxID, "05");

                //20120921 dusj modify begin
                //如果要补充的钱不等于零
                // if (!"0".Equals(moneyNum))
                // {

                CashStorageInfo storageInfo = new CashStorageInfo();
                storageInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                storageInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                storageInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
                storageInfo.update_time = DateTime.Now.ToString("HHmmss");
                //dusj modify 金额核算修改
                //多币种
                storageInfo.currency_code = TickMonyBoxHelp.MultipleCurrency;

                //查询多币种库存
                CashStorageInfo tempStore = TickMonyBoxHelp.Instance.GetMoneyTypeCodeInfo(storageInfo);
                if (tempStore != null && !string.IsNullOrEmpty(tempStore.currency_code))
                {
                    //比较库存的钱和需要压到钱箱中的钱
                    if (MultipleValue * tempStore.currency_num < putmoney)
                    {
                        Util.DataBase.Rollback();
                        Wrapper.ShowDialog("补充数量大天库存数量。");
                        return null;
                    }
                }
                else
                {
                    Util.DataBase.Rollback();
                    Wrapper.ShowDialog("此币种无库存。");
                    return null;
                }
                //压到钱箱中的数量用金额/多币种面值
                storageInfo.currency_num = putmoney / MultipleValue;

                //更新钱库存表
                dbRsult = TickMonyBoxHelp.Instance.insertCashStorageInfo(storageInfo, (int)TickMonyBoxHelp.MoneyBoxPutOrClear.压钱);
                if (dbRsult != 1)
                {
                    Wrapper.ShowDialog("钱箱补充失败,失败原因是数据库操作失败。");
                    Util.DataBase.Rollback();
                    return null;
                }


                //记录操作流水

                LogManager log = new LogManager();
                int totalNum = moneyValue * moneyNum.ToInt32();
                int logRes = log.WriteMoneyBoxOperation(moneyBoxID, "04", TickMonyBoxHelp.MultipleCurrency, 0, putmoney / MultipleValue, totalNum,deviceID);
                //dusj 20120807 modify begin 记录现金变化日志
                decimal beforMoney = MultipleValue * tempStore.currency_num;
                decimal afterMoney = MultipleValue * tempStore.currency_num - putmoney;
                int changeRes = log.AddCashStoreLog("05", beforMoney, afterMoney, TickMonyBoxHelp.MultipleCurrency, string.Empty);
                //dusj 20120807 modify end 记录现金变化日志
                if (dbRsult * logRes * changeRes < 1)
                {
                    Wrapper.ShowDialog("钱箱补充失败,失败原因是数据库操作失败。");
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_Put_Action, "1", "钱箱补充失败,失败原因是数据库操作失败");
                    Util.DataBase.Rollback();
                    return null;
                }
                //}

                Wrapper.ShowDialog("钱箱补充成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_Put_Action, "0", "钱箱补充成功");
                Util.DataBase.Commit();
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }


            return null;

        }
        /// <summary>
        /// 检查钱箱状态是否合法。
        /// </summary>
        /// <param name="moneyBoxId">钱箱ID</param>
        /// <returns>return bool值
        /// true - 成功
        /// false - 失败
        /// </returns>
        bool CheckRfidMoneyBoxLocationIdIsValid(string moneyBoxId)
        {
            CashBoxStatusInfo info = TickMonyBoxHelp.Instance.GetCashMoneyBoxStatusInfo(moneyBoxId);
            if (info != null && !string.IsNullOrEmpty(info.box_position))
            {
                if (info.box_position.Equals(((byte)TickMonyBoxHelp.MoneyBoxPositionState.在库).ToString("x2")))
                {
                    return true;
                }
                else
                {
                    Wrapper.ShowDialog("此钱箱不在库，不能进行补充操作。");
                    return false;
                }
            }
            else
            {
                Wrapper.ShowDialog("请检查钱箱是否已登记。");
                return false;
            }
        }

        /// <summary>
        /// 写RFID操作。
        /// </summary>
        /// <param name="body">钱箱补充归还结构体</param>

        public bool WriteRfid()
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
                rfid.moneyBoxLocationId = (byte)TickMonyBoxHelp.MoneyBoxPositionState.操作员手中;
                Wrapper.Instance.ConsoleWriteLine("rfid补充时的状态：" + rfid.moneyBoxLocationId, LogFlag.DebugFormat);

                rfid.operatorId = operatorID.Trim();
                rfid.lastOperatorTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                //2012.12.25 dusj modify begin
                rfid.deviceId = deviceID;
                //2012.12.25 dusj modify end

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
                    rfid.moneyBoxLocationId = (byte)TickMonyBoxHelp.MoneyBoxPositionState.在库;

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
    }
}
