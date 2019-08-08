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
    using System.Windows;
    using AFC.WS.Model.Const;


    public class TickBoxCheckInAction: IAction
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
            if (info.ticketboxLoactionStatus == 1)
            {
                MessageDialog.Show("该票箱票箱已归还，不能重复归还!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (info.ticketboxLoactionStatus == 3)
            {
               MessageBoxResult  res= MessageDialog.Show("是否强制归还该票箱?", "提示", MessageBoxIcon.Information, MessageBoxButtons.YesNo);
               if (res != MessageBoxResult.Yes)
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
            info.ticketboxLoactionStatus = 1;
            int res = 0;

            for (int i = 0; i < 3; i++)
            {
                res = BuinessRule.GetInstace().rfidRw.WriteTicketBoxRFID(info, 1);
                if (res == 0)
                    break;
                if (i == 2)
                {
                    MessageDialog.Show("RFID标签写入失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_CheckIn_Action, "1", "票箱归还时RFID标签写入失败");
                    return null;
                }
            }
            res = BuinessRule.GetInstace().tickMan.TickBoxCheckIn(info.ticketboxId);
            if (res != 0)
            {
                MessageDialog.Show("票箱归还失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_CheckIn_Action, "1", "票箱归还失败");
                return null;
            }
            else
            {
                MessageDialog.Show("票箱归还成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_CheckIn_Action, "0", "票箱归还成功");
            }
            return new ResultStatus { resultCode = 0, resultData = 0 };
            return null;
        }

        #endregion
    }
}
