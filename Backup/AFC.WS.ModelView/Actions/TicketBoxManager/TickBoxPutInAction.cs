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
    /// 票箱压票Action
    /// need input params 
    /// 1.lastNo 
    /// 2.rfidInfo
    /// </summary>
    public class TickBoxPutInAction: IAction
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
                MessageDialog.Show("票箱为非在库状态，请先清点!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (!BuinessRule.GetInstace().tickMan.CheckTickBoxHasRegister(info.TicketboxId))
            {
                MessageDialog.Show("票箱在本车站未登记,请进行登记!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            int lastNum = Convert.ToInt32(actionParamsList.Single(temp => temp.bindingData.Equals("lastNo")).value.ToString());

            if (!BuinessRule.GetInstace().tickMan.CheckTickStroeNum(info.cardIssueId.ToString("d2"), info.ticketNumber-lastNum))
            {
                MessageDialog.Show("当前库存总量不足"+(info.ticketNumber-lastNum).ToString()+"!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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

            int lastNum=Convert.ToInt32(actionParamsList.Single(temp=>temp.bindingData.Equals("lastNo")).value.ToString());
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
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.TickBox_PutIn_Action, "1", "票箱登记时RFID标签写入失败");
                    return null;
                }
            }
            res = BuinessRule.GetInstace().tickMan.TickBoxPutIn(info, lastNum, info.ticketNumber, info.cardIssueId.ToString("d2"));
            if (res != 0)
            {
                MessageDialog.Show("票箱补充失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
            }
            MessageDialog.Show("票箱补充成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            return new ResultStatus { resultCode = 0, resultData = 0 };

            
        }

        #endregion
    }
}
