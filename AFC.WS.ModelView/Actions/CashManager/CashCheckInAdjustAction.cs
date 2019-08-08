using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.BR;
using AFC.WS.Model.DB;

namespace AFC.WS.ModelView.Actions.CashManager
{
   public class CashCheckInAdjustAction : IAction
    {

        #region IAction 成员
        string operationCode = string.Empty;
        string settleDate = string.Empty;
        int realMoney = 0;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("没有归还现金信息，请添加现金", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("operationCode")).value != null)
            {
                operationCode = actionParamsList.Single(temp => temp.bindingData.Equals("operationCode")).value.ToString();
                if (string.IsNullOrEmpty(operationCode))
                {
                    MessageDialog.Show("请输入归还现金的操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
            }
            else
            {
                MessageDialog.Show("请输入归还现金的操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("settleDate")).value == null)
            {
                MessageDialog.Show("请输入结算日期", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            settleDate = actionParamsList.Single(temp => temp.bindingData.Equals("settleDate")).value.ToString();

            PrivOperatorInfo operatorInfo = BuinessRule.GetInstace().operationManager.GetOperatorInfoByOperatorId(operationCode);
            if (operatorInfo == null || string.IsNullOrEmpty(operatorInfo.operator_id))
            {
                MessageDialog.Show("请输入正确的操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            decimal totalMoney = 0;
            for (int i = 0; i < actionParamsList.Count - 2; i++)
            {
                string moneyType = actionParamsList[i].bindingData;
                decimal reduceNumber = Convert.ToDecimal(actionParamsList[i].value.ToString());
                //库存增加
                int resStorage = TickMonyBoxHelp.Instance.updateStorageInfo(moneyType, reduceNumber, 2);
                int currValue = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo().Where(p => p.currency_code == moneyType).GetTContext<BasiMoneyTypeInfo>().currency_value.ToInt32();
                //计算金额
                totalMoney =totalMoney + currValue * reduceNumber * 100;

                if (resStorage  != 1)
                {
                    Util.DataBase.Rollback();
                    MessageDialog.Show("操作员现金归还失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    //记错误日志
                    
                    return null;
                }

            }


           //操作员归还日志
            int resLog = BuinessRule.GetInstace().logManager.AddCashOperatorLog("03", totalMoney, operationCode);

            //插入或修改结帐流水表
            int updRes = TickMonyBoxHelp.Instance.updateDataOperSettlementInfo(operationCode, settleDate, Convert.ToInt32(totalMoney));

            if (resLog * updRes != 1)
            {
                Util.DataBase.Rollback();
                MessageDialog.Show("操作员现金归还失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                //记错误日志
                return null;
            }
            Util.DataBase.Commit();
            MessageDialog.Show("操作员现金归还成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            return new ResultStatus { resultCode = 0, resultData = 0 };
            
        }

        #endregion
    }
}
