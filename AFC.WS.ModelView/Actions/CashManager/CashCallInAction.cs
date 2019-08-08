using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.Model.Const;
using AFC.WS.ModelView.Actions.CommonActions;

namespace AFC.WS.ModelView.Actions.CashManager
{
    public class CashCallInAction : IAction, IDoublePrimissionHandler
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("没有调入现金信息，请添加现金", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            return true;
            
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            Util.DataBase.BeginTransaction();
            for (int i = 0; i < actionParamsList.Count; i++)
            {
                string moneyType = actionParamsList[i].bindingData;             
                decimal  reduceNumber = Convert.ToDecimal(actionParamsList[i].value.ToString());
                int res = TickMonyBoxHelp.Instance.updateStorageInfo(moneyType, reduceNumber, 0);
                if (res != 1)
                {
                    Util.DataBase.Rollback();
                    //记错误日志
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Call_In, "1", "现金调入失败");
                    return null;
                }
               
            }
            Util.DataBase.Commit();
            MessageDialog.Show("现金调入成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Call_In, "0", "现金调入成功");

            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Cash_Call_In, operatorId, "现金调入");
            return res == 0;
        }

        #endregion
    }
}
