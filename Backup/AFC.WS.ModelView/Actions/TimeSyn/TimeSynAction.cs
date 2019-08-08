using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.TimeSyn
{
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    using AFC.WS.ModelView.UIContext;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.Const;
    using AFC.WS.ModelView.Actions.CommonActions;

    public class TimeSynAction : IAction, IDoublePrimissionHandler
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            try
            {
                uint currentTime = actionParamsList.Single(temp => temp.bindingData.Equals("currentTime")).value.ToString().ConvertNumberStringToUint();
                bool isForce = bool.Parse(actionParamsList.Single(temp => temp.bindingData.Equals("isForce")).value.ToString());
                return true;
            }
            catch (Exception ex)
            {
                //todolog here and show dialog
              
                return false;
            }
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            int res = 0;
            string msg;
            uint currentTime = actionParamsList.Single(temp => temp.bindingData.Equals("currentTime")).value.ToString().ConvertNumberStringToUint();
            bool isForce = bool.Parse(actionParamsList.Single(temp => temp.bindingData.Equals("isForce")).value.ToString());
            if (isForce)
            {
                res = BuinessRule.GetInstace().commProcess.ForceTimeSyn(currentTime);
                msg = MessageCfg.getMessageContent("1333", res.ToString());
                if (res != 0)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Force_Time_Syn_Action, "1", "强制时钟同步指令发送失败");
                }
                else
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Force_Time_Syn_Action, "0", "强制时钟同步指令发送成功");
                }
            }
            else
            {
                res = BuinessRule.GetInstace().commProcess.TimeSyn(currentTime);
                msg = MessageCfg.getMessageContent("1334", res.ToString());
                if (res != 0)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Auto_Time_Syn_Action, "1", "自动时钟同步指令发送失败");
                }
                else
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Auto_Time_Syn_Action, "0", "自动时钟同步指令发送成功");
                }
            }

           
            MessageDialog.Show(msg, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            return null;
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Force_Time_Syn_Action, operatorId, "时钟同步");
            return res == 0;
        }

        #endregion
    }
}
