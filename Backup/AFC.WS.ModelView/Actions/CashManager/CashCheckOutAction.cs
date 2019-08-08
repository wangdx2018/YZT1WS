using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;
using AFC.WS.BR;
using AFC.WS.ModelView.Convertors;
using AFC.WS.Model.Const;
using AFC.WS.ModelView.Actions.CommonActions;

namespace AFC.WS.ModelView.Actions.CashManager
{
    public class CashCheckOutAction : IAction, IDoublePrimissionHandler
    {
        #region IAction 成员
        string operationCode = string.Empty;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("没有领用现金信息，请添加现金", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("operationCode")).value != null)
            {
                operationCode = actionParamsList.Single(temp => temp.bindingData.Equals("operationCode")).value.ToString();
                if (string.IsNullOrEmpty(operationCode))
                {
                    MessageDialog.Show("请输入领用现金的操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
            }
            else
            {
                MessageDialog.Show("请输入领用现金的操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
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
            var ConvertToFen = new ConvertYuanToFen();
            for (var i = 0; i < actionParamsList.Count - 1; i++)
            {
                string moneyType = actionParamsList[i].bindingData;
                decimal reduceNumber = Convert.ToDecimal(actionParamsList[i].value.ToString());
                CashStorageInfo storage = BuinessRule.GetInstace().GetCashStorageInfoByCashCode(moneyType);
                decimal cashOut = storage.currency_num;
                if (reduceNumber > cashOut)
                {
                    MessageDialog.Show("现金调出金额大于在库金额", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    Util.DataBase.Rollback();
                    //记错误日志
                    BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Check_Out, "1", "现金调出数量大于在库数");
                    return null;
                }
                //操作员现金增加
                int resOperator = TickMonyBoxHelp.Instance.updateCashInOperatorInfo(operationCode, moneyType, reduceNumber, 0);
                //库存现金减少
                int resStorage = TickMonyBoxHelp.Instance.updateStorageInfo(moneyType, reduceNumber, 3);
                int currValue = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo().Where(p => p.currency_code == moneyType).GetTContext<BasiMoneyTypeInfo>().currency_value.ToInt32();
                //操作员领用日志
                int resLog = BuinessRule.GetInstace().logManager.AddCashOperatorLog("00", ConvertToFen.Convert(currValue * reduceNumber, null, null, null).ToString().ConvertNumberStringToUint(), operationCode);
                if (resOperator * resStorage * resLog != 1)
                {
                    Util.DataBase.Rollback();
                    //记错误日志
                    MessageDialog.Show("操作员现金领用失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Check_Out, "1", "操作员现金增加或库存现金减少失败");
                    return null;
                }

            }
            Util.DataBase.Commit();
            MessageDialog.Show("操作员现金领用成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Check_Out, "0", "操作员现金领用成功");
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion
        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Cash_Check_Out, operatorId, "操作员现金领用");
            return res == 0;
        }

        #endregion
    }
}
