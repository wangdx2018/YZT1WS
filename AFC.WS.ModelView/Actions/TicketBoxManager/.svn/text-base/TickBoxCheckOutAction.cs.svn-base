using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.TicketBoxManager
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.RfidRW;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;

    /// <summary>
    /// added by wangdx 20110520
    /// 
    /// 票箱领用Action需要传递参数
    /// 1.rfidInfo
    /// </summary>
    public class TickBoxCheckOutAction: IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请先读取票箱的RFID信息!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            RfidTicketboxInfo info = actionParamsList.Single(temp => temp.bindingData.Equals("rfidInfo")).value as RfidTicketboxInfo;
            if (info == null)
            {
                MessageDialog.Show("请先读取票箱的RFID信息!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (info.ticketboxLoactionStatus != 1)
            {
                MessageDialog.Show("票箱已领用!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (!BuinessRule.GetInstace().tickMan.CheckTickBoxHasRegister(info.TicketboxId))
            {
                MessageDialog.Show("票箱在本车站未登记,请进行登记!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            RfidTicketboxInfo info = actionParamsList.Single(temp => temp.bindingData.Equals("rfidInfo")).value as RfidTicketboxInfo;
            info.lastOpeatorTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            info.ticketboxLoactionStatus = 2;
            int res = 0;

            for (int i = 0; i < 3; i++)
            {
                res = BuinessRule.GetInstace().rfidRw.WriteTicketBoxRFID(info, 1);
                if (res == 0)
                    break;
                if (i == 2)
                {
                    MessageDialog.Show("RFID标签写入失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_CheckOut_Action, "1", "票箱领用时RFID标签写入失败");
                    return null;
                }
            }
            res = BuinessRule.GetInstace().tickMan.TickBoxCheckOut(info.ticketboxId);
            if (res != 0)
            {
                MessageDialog.Show("票箱领用失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_CheckOut_Action, "1", "票箱领用失败");
                return null;
            }
            else
            {
                MessageDialog.Show("票箱领用成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_CheckOut_Action, "0", "票箱领用成功");
            }
            return new ResultStatus { resultCode = 0, resultData = 0 };
            return null;
        }

        #endregion
    }
}
