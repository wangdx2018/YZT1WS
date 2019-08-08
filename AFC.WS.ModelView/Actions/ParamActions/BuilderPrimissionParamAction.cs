using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;
    
    /// <summary>
    /// added by wangdx 20110513
    /// 
    /// 生成权限参数文件通知
    /// </summary>
   public class BuilderPrimissionParamAction:IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            MessageBoxResult res = MessageDialog.Show("是否要及时生成权限参数文件?", "提示", MessageBoxIcon.Question, MessageBoxButtons.YesNo);
            return (res == MessageBoxResult.Yes);
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            int res=BuinessRule.GetInstace().commProcess.BuildParamFile();
            if (res == 0)
            {
                MessageDialog.Show("发送生成权限参数通知成功！", "提示", MessageBoxIcon.Question, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Builder_Primission_Param_Action, "0", "发送生成权限参数通知成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            MessageDialog.Show("发送生成权限参数通知失败！", "提示", MessageBoxIcon.Question, MessageBoxButtons.Ok);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Builder_Primission_Param_Action, "1", "发送生成权限参数通知失败");
            return null;

        }

        #endregion
    }
}
