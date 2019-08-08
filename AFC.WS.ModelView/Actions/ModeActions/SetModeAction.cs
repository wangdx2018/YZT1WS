using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.ModeActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.BR;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.Model.Const;

    public class SetModeAction: IAction,IDoublePrimissionHandler
    {


        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            try
            {
                uint stationId = actionParamsList.Single(temp => temp.bindingData.Equals("stationId")).value.ToString().ConvertNumberStringToUint(); ;
                uint modeCode = actionParamsList.Single(temp => temp.bindingData.Equals("modeCode")).value.ToString().ConvertNumberStringToUint();
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.Show("数据不合法", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            uint stationId = actionParamsList.Single(temp => temp.bindingData.Equals("stationId")).value.ToString().ConvertNumberStringToUint(); ;
            uint modeCode=actionParamsList.Single(temp=>temp.bindingData.Equals("modeCode")).value.ToString().ConvertNumberStringToUint();
            int res= BuinessRule.GetInstace().commProcess.ModeChange(stationId, modeCode);
            if (res == 0)
            {
                MessageDialog.Show("模式设置成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Set_Mode_Action, "0", "模式设置成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            else
            {
                MessageDialog.Show("模式设置失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Set_Mode_Action, "1", "模式设置失败");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Set_Mode_Action, operatorId, "模式设置");
            return res == 0;
        }

        #endregion
    }
}
