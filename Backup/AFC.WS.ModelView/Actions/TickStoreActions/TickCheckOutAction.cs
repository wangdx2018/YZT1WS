using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.TickStoreActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;
    using AFC.WS.ModelView.Actions.CommonActions;

    public class TickCheckOutAction: IAction,IDoublePrimissionHandler
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count<=1)
            {
                MessageDialog.Show("没有领用散票信息，请添加归还信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            
                return false;
            }
            try
            {
                actionParamsList.Single(temp => temp.bindingData.Equals("operator_id"));
                string operatorId = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
                AFC.WS.BR.Primission.OperatorManager om = new AFC.WS.BR.Primission.OperatorManager();
                bool res = om.IsExistCurrentOperator(operatorId);
                if (!res)
                {
                    MessageDialog.Show("该操作员不存在", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.Show("请输入领用操作员信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
          
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            string operator_id = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();

            for (int i = 0; i < actionParamsList.Count; i++)
            {
                if (!actionParamsList[i].bindingData.Equals("operator_id"))
                {
                    int res = BuinessRule.GetInstace().tickMan.TickOperatorCheckOut(actionParamsList[i].bindingData.ToString(),
                         operator_id, int.Parse(actionParamsList[i].value.ToString()));
                    if (res != 0)
                    {
                        MessageDialog.Show("操作员" + operator_id + "散票领用失败","提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_CheckOut_Action, "1", "散票领用失败");
                        return null;
                    }
                }
                continue;
            }
            MessageDialog.Show("操作员" + operator_id + "散票领用成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_CheckOut_Action, "0", "散票领用成功");
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Tick_CallOut_Action, operatorId, "票卡领用");
            return res == 0;
        }

        #endregion
    }
}
