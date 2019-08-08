using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.RunManager
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.DB;
    using AFC.WS.Model.Const;
    using AFC.WS.BR;

    /// <summary>
    /// LC
    /// </summary>
    public class LCRunEndAction: IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            //if (BR.BuinessRule.GetInstace().rm.CheckHasRunEnd())
            //{
            //    MessageDialog.Show("目前处在运营结束状态!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
            //    return false;
            //}
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            int res = BR.BuinessRule.GetInstace().commProcess.RunEnd();
            if (res != 0)
            {
                MessageDialog.Show("发送运营指令命令失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Run_End, "1", "LC运营结束指令发送失败");
                return null;
            }
            MessageDialog.Show("发送运营结束命令成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Run_End, "0", "LC运营结束指令发送成功");
            BR.BuinessRule.GetInstace().rm.StartRunMonitorThread(AsynMessageType.RunEnd);
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion
    }
}
