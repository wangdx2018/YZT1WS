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
    public class MoneyBoxInAction : IAction, IDoublePrimissionHandler
    {

        #region IAction 成员

        string moneyBoxID = string.Empty;
        string operatorID = string.Empty;
        string boxPosition = string.Empty;
        MoneyBoxRFID rfid = null;
     

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            try
            {
                string currentboxPosition = string.Empty;
                TicketOrMoneyBoxIdConvetor coverHex = new TicketOrMoneyBoxIdConvetor();
                if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxID")).value != null)
                {
                    moneyBoxID = coverHex.ConvertBack(actionParamsList.Single(temp => temp.bindingData.Equals("moneyBoxID")).value.ToString(),null,null,null).ToString();
                }
                if (actionParamsList.Single(temp => temp.bindingData.Equals("boxPosition")).value != null)
                {
                    boxPosition = actionParamsList.Single(temp => temp.bindingData.Equals("boxPosition")).value.ToString();
                }
                if (actionParamsList.Single(temp => temp.bindingData.Equals("operatorID")).value != null)
                {
                    operatorID = actionParamsList.Single(temp => temp.bindingData.Equals("operatorID")).value.ToString();
                }
                if (actionParamsList.Single(temp => temp.bindingData.Equals("rfid")).value != null)
                {
                    rfid = (MoneyBoxRFID)actionParamsList.Single(temp => temp.bindingData.Equals("rfid")).value;
                }
          
                if (moneyBoxID.Length != 8)
                {
                    Wrapper.ShowDialog("请输入八位钱箱编码。");
                    return false;
                }
                //增加对是否是已登记钱箱的判断
                CashBoxStatusInfo status = null;
                bool isNotExist = TickMonyBoxHelp.Instance.CheckedMoneyBoxId(moneyBoxID, out status);
                if (isNotExist == false)
                {
                    Wrapper.ShowDialog("[" + moneyBoxID + "]此钱箱未登记。");
                    return false;
                }
                //如果是纸币钱箱从数据库中取得状态
                if (rfid == null)
                {

                    currentboxPosition = status.box_position;

                }
                //如果是硬币钱箱从RFID中取得状态
                else
                {
                    currentboxPosition = rfid.moneyBoxLocationId.ToString("x2");
                }

                string codeType = moneyBoxID.Substring(2, 2);
                bool isChecked = false;
 
                //如果方法是领用
                if (boxPosition.Equals(((byte)TickMonyBoxHelp.MoneyBoxPositionState.操作员手中).ToString("x2")))
                {
                    switch (currentboxPosition.ToHexNumberInt32())
                    {
                        case 1: //-->在库
                            isChecked = true;
                            break;
                        case 2: //-->在人
                            Wrapper.ShowDialog("[" + moneyBoxID + "]此钱箱已被领用。");
                            isChecked = false;
                            break;
                        case 3: //-->在设备
                            Wrapper.ShowDialog("[" + moneyBoxID + "]此钱箱在设备上,不能领用。");
                            isChecked = false;
                            break;
                        default:
                            if (Wrapper.ShowDialog("[" + moneyBoxID + "]此钱箱未知状态，是否领用。", false) == MessageBoxResult.Yes)
                            {
                                isChecked = true;
                            }
                            else
                            {
                                isChecked = false;
                            }
                            break;
                    }
                    return isChecked;
                }
                //如果方法是归还
                if (boxPosition.Equals(((byte)TickMonyBoxHelp.MoneyBoxPositionState.在库).ToString("x2")))
                {
                    switch (currentboxPosition.ToHexNumberInt32())
                    {
                        case 1: //-->在库
                            Wrapper.ShowDialog("[" + moneyBoxID + "]此钱箱处于在库状态，不用归还。");
                            return false;
                        case 2: //-->在人
                            return true;
                        case 3: //-->在设备
                            if (Wrapper.ShowDialog("[" + moneyBoxID + "]此钱箱在设备上，是否强制归还。", false) == MessageBoxResult.Yes)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        default:
                            if (Wrapper.ShowDialog("[" + moneyBoxID + "]此钱箱未知状态，是否强制归还。", false) == MessageBoxResult.Yes)
                            {
                                isChecked = true;
                            }
                            else
                            {
                                isChecked = false;
                            }
                            break;
                    }
                    return isChecked;
                }
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
           
            int dbRsult = 0;
            
            WriteLog.Log_Info("WriteRfid开始" + DateTime.Now.ToString());
            //写RDID
            if (WriteRfid())
            
            {

                WriteLog.Log_Info("WriteRfid结束" + DateTime.Now.ToString());

                //修改状态表
                CashBoxStatusInfo cashBoxStatus = TickMonyBoxHelp.Instance.GetCashMoneyBoxStatusInfo(moneyBoxID);
                cashBoxStatus.box_position = boxPosition;           

                dbRsult = TickMonyBoxHelp.Instance.updateCashMoneyStatusInfo(cashBoxStatus);

                if (dbRsult != 1)
                {
                    if (boxPosition.Equals(((byte)TickMonyBoxHelp.MoneyBoxPositionState.操作员手中).ToString("x2")))
                    {
                        Wrapper.ShowDialog("钱箱补充失败,失败原因是数据库操作失败。");
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_CheckOut_Action, "1", "补充空钱箱失败");
                        return new ResultStatus { resultCode = -1, resultData = 0 };
                    }
                    else 
                    {
                        Wrapper.ShowDialog("钱箱归还失败,失败原因是数据库操作失败。");
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_CheckIn_Action, "1", "归还空钱箱失败");
                        return new ResultStatus { resultCode = -1, resultData = 0 };
                    }
                }

                //记录操作流水
                LogManager log = new LogManager();
                string mothod = string.Empty;
                string show = string.Empty;
                //领用
                if (boxPosition.Equals(((byte)TickMonyBoxHelp.MoneyBoxPositionState.操作员手中).ToString("x2")))
                {
                    mothod = "05";
                    show = "领用";
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_CheckOut_Action, "0", "领用空钱箱成功");
                }
                  //归还
                else
                {
                    mothod = "06";
                    show = "归还";
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.MoneyBox_CheckIn_Action, "0", "归还空钱箱成功");
                }

                log.WriteMoneyBoxOperation(moneyBoxID, mothod);
                Wrapper.ShowDialog(show+"钱箱成功。");
            }
             return new ResultStatus { resultCode = 0, resultData = 0 };
         
        }
        /// <summary>
        /// 写RFID操作。
        /// </summary>
        /// <param name="body">钱箱领用归还结构体</param>

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

                byte LocationId = rfid.moneyBoxLocationId;


                rfid.moneyBoxLocationId = boxPosition.ToByte();

                Wrapper.Instance.ConsoleWriteLine("rfid领用时的状态：" + rfid.moneyBoxLocationId, LogFlag.DebugFormat);

                rfid.operatorId = operatorID.Trim();
                rfid.lastOperatorTime = DateTime.Now.ToString("yyyyMMddHHmmss");
               
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
                    rfid.moneyBoxLocationId = LocationId;

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
