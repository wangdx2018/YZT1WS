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
    /// added by wangdx  20110511
    /// LC运营开始的Action
    /// </summary>
    public class LCRunStartAction: IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
           //todo Check LC run status if has been run start return false;
            //todo
            //if (BR.BuinessRule.GetInstace().rm.CheckHasRunStart())
            //{
            //    MessageDialog.Show("目前处在运营开始状态!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
            //    return false;
            //}
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
          //  throw new NotImplementedException();
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            //todo 001send msg to LC
            //todoif succesuful
            //todostart BR asyn message thread
            int res = BR.BuinessRule.GetInstace().commProcess.RunStart();
            if (res != 0)
            {
                MessageDialog.Show("发送运营开始命令失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Run_Start, "1", "LC运营开始指令发送失败");
                return null;
            }
            MessageDialog.Show("发送运营开始命令成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Run_Start, "0", "LC运营开始指令发送成功");
            BR.BuinessRule.GetInstace().rm.StartRunMonitorThread(AsynMessageType.RunStart);
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion
    }
}
