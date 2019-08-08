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

    /// <summary>
    /// edit by wangdx 20110628
    /// 
    /// 发送运营开始命令后发送启动刷新线程
    /// </summary>
   public class SCRunStartAction : IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            //2011.7.15 bug3930 delete
            //if (BR.BuinessRule.GetInstace().rm.CheckHasRunStart())
            //{
            //    MessageDialog.Show("目前处在运营开始状态!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
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
            int res = BR.BuinessRule.GetInstace().commProcess.RunStart();
            if (res != 0)
            {
                MessageDialog.Show("发送运营开始命令失败!", "错误", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Run_Start, "1", "SC运营开始指令发送失败");
                return null;
            }
            else
            {
                MessageDialog.Show("发送运营开始命令成功!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Run_Start, "0", "SC运营开始指令发送成功");
                BR.BuinessRule.GetInstace().rm.StartRunMonitorThread(AsynMessageType.RunStart);
            }
          
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion
    }
}
