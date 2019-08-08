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
    public class CashStoreAdjustAction : IAction, IDoublePrimissionHandler
    {
        #region IAction 成员
        string moneyCode = string.Empty;
        string moneyNo = string.Empty;
        string moneyReal = string.Empty;
        string remark = string.Empty;
        string buttonMethod = string.Empty;
        string adjustMethod = string.Empty;
        string moneyWait = string.Empty;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("buttonMethod")).value != null)
            {
                buttonMethod = actionParamsList.Single(temp => temp.bindingData.Equals("buttonMethod")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("adjustMethod")).value != null)
            {
                adjustMethod = actionParamsList.Single(temp => temp.bindingData.Equals("adjustMethod")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyCode")).value != null)
            {
                moneyCode = actionParamsList.Single(temp => temp.bindingData.Equals("moneyCode")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyNo")).value != null)
            {
                moneyNo = actionParamsList.Single(temp => temp.bindingData.Equals("moneyNo")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyReal")).value != null)
            {
                moneyReal = actionParamsList.Single(temp => temp.bindingData.Equals("moneyReal")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("remark")).value != null)
            {
                remark = actionParamsList.Single(temp => temp.bindingData.Equals("remark")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("moneyWait")).value != null)
            {
                moneyWait = actionParamsList.Single(temp => temp.bindingData.Equals("moneyWait")).value.ToString();
            }
            if (string.IsNullOrEmpty(moneyCode))
            {
                Wrapper.ShowDialog("请选择钱币种类。");
                return false;
            }
            if (string.IsNullOrEmpty(moneyReal))
            {
                Wrapper.ShowDialog("请填写库存调整数量。");
                return false;
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(System.Collections.Generic.List<QueryCondition> actionParamsList)
        {
            //库存调整
            if (buttonMethod.Equals("0"))
            {

                int res = TickMonyBoxHelp.Instance.UpdateCashSotreInfo(moneyCode, Convert.ToDecimal(moneyReal), remark, adjustMethod);

                if (res == 1)
                {
                    Wrapper.ShowDialog("库存调整成功。");
                    BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Store_Adjust_Action, "0", "库存调整成功");
                }
                else
                {
                    Wrapper.ShowDialog("库存调整失败。");
                    BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Store_Adjust_Action, "0", "库存调整失败");
                }
            }
             //待解行、解行
            else if (buttonMethod.Equals("4"))
            {
                int waitRes = TickMonyBoxHelp.Instance.UpdateCashWaitingInfo(Convert.ToInt16(buttonMethod), moneyCode, Convert.ToDecimal(moneyReal), adjustMethod, Convert.ToDecimal(moneyWait));
                if (waitRes == 1)
                {
                    Wrapper.ShowDialog("操作成功。");
                    BR.BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Cash_Wait_Solution, "0", "库存调整成功");
                }
                else
                {
                    Wrapper.ShowDialog("操作失败。");
                    BR.BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Cash_Wait_Solution, "0", "库存调整成功");
                }
            }
            else if (buttonMethod.Equals("5"))
            {
                int waitRes = TickMonyBoxHelp.Instance.UpdateCashWaitingInfo(Convert.ToInt16(buttonMethod), moneyCode, Convert.ToDecimal(moneyReal), adjustMethod,Convert.ToDecimal(moneyWait),remark);
                if (waitRes == 1)
                {
                    Wrapper.ShowDialog("操作成功。");
                    BR.BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Cash_Solution, "0", "库存调整成功", moneyReal.ConvertYuanToFen());
                }
                else
                {
                    Wrapper.ShowDialog("操作失败。");
                    BR.BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Cash_Solution, "0", "库存调整成功", moneyReal.ConvertYuanToFen());
                }
            }
           return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Cash_Store_Adjust_Action, operatorId, "现金库存调整");
            return res == 0;
        }

        #endregion
    }
}
