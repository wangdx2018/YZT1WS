using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.BR;
using AFC.WS.UI.Config;
using AFC.WS.Model.DB;
using AFC.WS.UI.RfidRW;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.UI.CommonControls;
using AFC.WS.ModelView.Convertors;
using AFC.WS.BR.LogManager;
using AFC.WS.Model.Const;

namespace AFC.WS.ModelView.Actions.TickMonyBoxManager
{
    public class MoneyBoxRegisterAction: IAction
    {
        #region IAction 成员

        string moneyBoxID = string.Empty;
        string moneyBoxRFID = string.Empty;
        string moenyBoxType = string.Empty;
        MoneyBoxRFID rfid = null;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {

          
            if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxID")).value != null)
            {
                moneyBoxID = actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxRFID")).value != null)
            {
                moneyBoxRFID = actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxRFID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("moenyBoxType")).value != null)
            {
                moenyBoxType = actionParamsList.Single(temp => temp.bindingData.Equals("moenyBoxType")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("rfid")).value != null)
            {
                rfid = (MoneyBoxRFID)actionParamsList.Single(temp => temp.bindingData.Equals("rfid")).value;
            }
           
            if (!TickMonyBoxHelp.Instance.JudgeMoneyBoxIsLegality(moneyBoxID))
            {
                Wrapper.Instance.ConsoleWriteLine("错误的钱箱编码是：" + moneyBoxID, LogFlag.InfoFormat);
                Wrapper.ShowDialog("请输入正确的钱箱编码。");
                return false;
            }
            if (string.IsNullOrEmpty(moneyBoxRFID))
            {
                Wrapper.ShowDialog("请填写电子标签。");
                return false;
            }
            if (moenyBoxType == "%")
            {
                MessageDialog.Show("请选择钱箱类型。", "警告", MessageBoxIcon.Warning, MessageBoxButtons.Ok);
                return false;
            }
            if (!moneyBoxID.Substring(2, 2).Equals(moenyBoxType))
            {
                MessageDialog.Show("钱箱编码类型与所选择的钱箱类型不符。", "警告", MessageBoxIcon.Warning, MessageBoxButtons.Ok);
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
            try
            {
               
                CashBoxRegistorInfo cashBoxRegInfo = new CashBoxRegistorInfo();
                cashBoxRegInfo.money_box_id = moneyBoxID;
                cashBoxRegInfo.electronic_tag_id = moneyBoxRFID;
                cashBoxRegInfo.operator_id = BuinessRule.GetInstace().OperatorId;
                cashBoxRegInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                cashBoxRegInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                cashBoxRegInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
                cashBoxRegInfo.update_time = DateTime.Now.ToString("HHmmss");


                CashBoxStatusInfo cashBoxStatusInfo = new CashBoxStatusInfo();
                cashBoxStatusInfo.box_position = ((byte)TickMonyBoxHelp.MoneyBoxPositionState.在库).ToString("x2");
                cashBoxStatusInfo.currency_code = "00"; //多币种
                cashBoxStatusInfo.currency_num = 0;
                cashBoxStatusInfo.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                cashBoxStatusInfo.money_box_id = moneyBoxID;
                cashBoxStatusInfo.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                cashBoxStatusInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
                cashBoxStatusInfo.update_time = DateTime.Now.ToString("HHmmss");

                try
                {
                    //写RDIF成功
                    if (WriteRfid())
                    {
                        //插入钱箱登记信息表
                        int regResult = TickMonyBoxHelp.Instance.insertMoneyBoxRegInfo(cashBoxRegInfo);
                        if (regResult != 1)
                        {
                            if (regResult == -2)
                            {
                                Wrapper.ShowDialog("钱箱已登记。");
                                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_Register_Action, "1", "钱箱已登记");
                                Util.DataBase.Rollback();
                                return null;
                            }
                            else
                            {
                                Wrapper.ShowDialog("登记钱箱失败,失败原因是数据库操作错误。");
                                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_Register_Action, "1", "登记钱箱失败,失败原因是数据库操作错误");
                                return null;
                            }
                        }
                        //记录是否存在
                        string id = TickMonyBoxHelp.Instance.GetCashMoneyBoxStatusInfo(cashBoxRegInfo.money_box_id).money_box_id;
                        if (id.JudgeIsNullOrEmpty() == true)
                        {
                            int statusResult = TickMonyBoxHelp.Instance.insertCashMoneyStatusInfo(cashBoxStatusInfo);
                            if (statusResult != 1)
                            {
                                Wrapper.ShowDialog("登记钱箱失败,失败原因是数据库操作错误。");
                                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_Register_Action, "1", "登记钱箱失败,失败原因是数据库操作错误");
                                Util.DataBase.Rollback();
                                return null;
                            }
                        }
                        //记录操作流水
                        LogManager log = new LogManager();
                        log.WriteMoneyBoxOperation(moneyBoxID, "08");
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_Register_Action, "0", "登记钱箱成功");
                        Wrapper.ShowDialog("登记钱箱成功。");
                    }
                    else
                    {
                        Wrapper.ShowDialog("登记钱箱失败,失败原因是RDID写入失败。");
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_Register_Action, "1", "登记钱箱失败,失败原因是RDID写入失败");
                        return null;
                    }
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return null;
            }
            return null;
        }

        /// <summary>
        /// 写RFID操作。
        /// </summary>
        public bool WriteRfid()
        {
            if (rfid == null || moenyBoxType.Equals("21") || moenyBoxType.Equals("22"))
            {
                return true;
            }
            bool isSucessWrite = false;
            rfid.moneyBoxLocationId = (byte)TickMonyBoxHelp.MoneyBoxPositionState.在库;
            rfid.stationCode = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            rfid.lastOperatorTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            rfid.operatorId = BuinessRule.GetInstace().brConext.CurrentOperatorId; ;
            int writeCount = 0;
            bool isWriteContinue = true;
            //连续写入三次
            while (writeCount < 3 && isWriteContinue == true)
            {
                int result = BuinessRule.GetInstace().rfidRw.WriteMoneyBoxRFID(rfid, (byte)TickMonyBoxHelp.RfidSerialPortsChannelNumber.一通道);
                writeCount = writeCount + 1;
                if (result == 0)
                {
                    {
                        isWriteContinue = false;
                        isSucessWrite = true;
                    }
                }
            }

            //写入成功
            if (isSucessWrite == true)
            {
            }
            else
            {
                Wrapper.ShowDialog("RFID写入失败。");
            }
            return isSucessWrite;

        }


        #endregion
    }
}
