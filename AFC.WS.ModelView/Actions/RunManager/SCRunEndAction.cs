using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.Const;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR;

namespace AFC.WS.ModelView.Actions.RunManager
{
    public class SCRunEndAction : IAction
    {
        #region IAction 成员
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            //2011.7.15 bug3930 delete
            //if (BR.BuinessRule.GetInstace().rm.CheckHasRunEnd())
            //{
            //    MessageDialog.Show("目前处在运营结束状态!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
            //    return false;
            //}
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            int res = BR.BuinessRule.GetInstace().commProcess.RunEnd();
            if (res != 0)
            {
                MessageDialog.Show("发送运营结束命令失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Run_End, "1", "SC运营结束指令发送失败");
            }
            else
            {
                MessageDialog.Show("发送运营结束命令成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Run_End, "0", "SC运营结束指令发送成功");
                BR.BuinessRule.GetInstace().rm.StartRunMonitorThread(AsynMessageType.RunEnd);
            }
           
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }
        #endregion
    }
}
