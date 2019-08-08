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

    public class TickCheckInAction : IAction, IDoublePrimissionHandler
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count <=1)
            {
                MessageDialog.Show("没有归还散票信息，请添加领用信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            try
            {
                string operatorId=actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
                AFC.WS.BR.Primission.OperatorManager om = new AFC.WS.BR.Primission.OperatorManager();
               bool res= om.IsExistCurrentOperator(operatorId);
               if (!res)
               {
                   MessageDialog.Show("该操作员不存在", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                   return false;
               }
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.Show("请输入归还操作员信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
        //    string tickStatus=actionParamsList.Single(temp=>temp.bindingData.Equals("tickStatus")).value.ToString();

            for (int i = 0; i < actionParamsList.Count; i++)
            {
                if (!actionParamsList[i].bindingData.Equals("operator_id"))
                {
                  //  string tickManType=actionParamsList.Single(temp=>temp.bindingData.Equals("
                  ResultStatus resultStatus=  actionParamsList[i].value as ResultStatus;


                  int res = BuinessRule.GetInstace().tickMan.TickOperatorCheckIn(actionParamsList[i].bindingData,
                        operator_id, resultStatus.resultCode,resultStatus.resultData.ToString(),actionParamsList[i].controlName);
                   if (res != 0)
                   {
                       MessageDialog.Show("操作员" + operator_id + "散票归还失败","提示",MessageBoxIcon.Information,MessageBoxButtons.Ok);
                       BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_CheckIn_Action, "1", "散票归还失败");
                       return null;
                   }
                }
            }
            MessageDialog.Show("操作员" + operator_id + "散票归还成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Tick_CheckIn_Action, "0", "散票归还成功");
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Tick_CheckIn_Action, operatorId, "散票归还");
            return res == 0;
        }

        #endregion
    }
}
