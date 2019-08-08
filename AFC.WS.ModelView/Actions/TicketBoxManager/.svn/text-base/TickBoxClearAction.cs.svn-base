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
    /// 票箱清点Action
    /// 输入参数：rfidInfo,lastNo
    /// </summary>
    public class TickBoxClearAction: IAction
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
            //if (info.ticketboxLoactionStatus != 1)
            //{
            //    MessageDialog.Show("票箱为非在库状态，请先归还!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            if (!BuinessRule.GetInstace().tickMan.CheckTickBoxHasRegister(info.TicketboxId))
            {
                MessageDialog.Show("票箱在本车站未登记,请进行登记!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            try
            {
                int lastNum = Convert.ToInt32(actionParamsList.Single(temp => temp.bindingData.Equals("lastNo")).value.ToString());
            }
            catch (Exception)
            {
                MessageDialog.Show("请输入票箱实际清点张数!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
          //  BuinessRule.GetInstace().brConext.operatorDevId = info.DeviceId;
            int lastNum = Convert.ToInt32(actionParamsList.Single(temp => temp.bindingData.Equals("lastNo")).value.ToString());
            ushort rfidNum = info.TicketNumber;

            int res = 0;

            info.TicketNumber = 0;
            info.ticketboxLoactionStatus = 1;

            for (int i = 0; i < 3; i++)
            {
                res = BuinessRule.GetInstace().rfidRw.WriteTicketBoxRFID(info, 1);
                if (res == 0)
                    break;
                if (i == 2)
                {
                    MessageDialog.Show("RFID标签写入失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Clear_Action, "1", "票箱清点时RFID标签写入失败");
                    return null;
                }
            }
            info.TicketNumber = rfidNum;
            res = BuinessRule.GetInstace().tickMan.TickBoxClear(info, lastNum, info.cardIssueId.ToString("d2"),info.TicketNumber);
            if (res != 0)
            {
                MessageDialog.Show("票箱清点失败,请检查是否存在该票卡类型的库存！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Clear_Action, "1", "票箱清点失败");
                return null;
            }
            else
            {
                MessageDialog.Show("票箱清点成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_Clear_Action, "0", "票箱清点成功");
            }
            return new ResultStatus { resultCode = 0, resultData = 0 };
            return null;
        }

        #endregion
    }
}
