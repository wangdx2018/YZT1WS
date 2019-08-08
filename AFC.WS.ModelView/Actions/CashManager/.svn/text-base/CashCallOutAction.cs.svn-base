using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;
using AFC.WS.BR;
using AFC.WS.Model.Const;
using AFC.WS.ModelView.Actions.CommonActions;

namespace AFC.WS.ModelView.Actions.CashManager
{
    public class CashCallOutAction : IAction, IDoublePrimissionHandler
    {
        #region IAction 成员
       string operatorCode = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("没有调出现金信息，请添加现金", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("operatorCode")).value != null)
            {
                operatorCode = actionParamsList.Single(temp => temp.bindingData.Equals("operatorCode")).value.ToString();
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            string messageInform = string.Empty;
            int stroageChangeType;
            if (operatorCode == OperationCode.Cash_Call_Out)
            {
                messageInform = "现金调出";
                stroageChangeType = 1;
            }
            else
            {
                messageInform = "现金解行";
                stroageChangeType = 4;
            }
            Util.DataBase.BeginTransaction();

            for (int i = 0; i < actionParamsList.Count-1; i++)
            {
                string moneyType = actionParamsList[i].bindingData;
                decimal  reduceNumber = Convert.ToDecimal(actionParamsList[i].value.ToString());
                CashStorageInfo storage = BuinessRule.GetInstace().GetCashStorageInfoByCashCode(moneyType);
                decimal cashOut = Convert.ToDecimal(storage.currency_num.ToString());
                if (reduceNumber > cashOut)
                {
                    MessageDialog.Show("币种" + TickMonyBoxHelp.Instance.GetMoneyTypeCodeName(moneyType) + messageInform +"数量大于在库数", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    Util.DataBase.Rollback();
                    BR.BuinessRule.GetInstace().logManager.AddLogInfo(operatorCode, "1", messageInform+"数量大于在库数");
                    //记错误日志
                    return null;
                }
                int res = TickMonyBoxHelp.Instance.updateStorageInfo(moneyType, reduceNumber, stroageChangeType);
                if (res != 1)
                {
                    Util.DataBase.Rollback();
                    BR.BuinessRule.GetInstace().logManager.AddLogInfo(operatorCode, "1", messageInform+"更新数据库失败");
                    //记错误日志
                    return null;
                }

            }
            Util.DataBase.Commit();

            MessageDialog.Show(messageInform+"成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BR.BuinessRule.GetInstace().logManager.AddLogInfo(operatorCode, "0", messageInform + "成功");
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Cash_Call_Out, operatorId, "现金调出");
            return res == 0;
        }

        #endregion
    }
}
