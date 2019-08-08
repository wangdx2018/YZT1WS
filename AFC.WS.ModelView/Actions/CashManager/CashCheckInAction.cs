using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;
using AFC.WS.BR.Primission;
using AFC.WS.BR;
using AFC.WS.Model.Const;
using AFC.WS.ModelView.Actions.CommonActions;

namespace AFC.WS.ModelView.Actions.CashManager
{
    public class CashCheckInAction : IAction, IDoublePrimissionHandler
    {
        #region IAction 成员
        string operationCode = string.Empty;
        string settleDate = string.Empty;
        string coinN0 = string.Empty;
        private List<TickManaProductData> tickType = null;

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
            coinN0 = actionParamsList.Single(temp => temp.bindingData.Equals("coinNo")).value.ToString();
                 
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
            for (int i = 0; i < actionParamsList.Count-6; i++)
            {
                string moneyType = actionParamsList[i].bindingData;
                decimal reduceNumber = Convert.ToDecimal(actionParamsList[i].value.ToString());
                //库存增加
                int resStorage = TickMonyBoxHelp.Instance.updateStorageInfo(moneyType, reduceNumber, 2);
                int currValue = BuinessRule.GetInstace().GetAllMoneyTypeCodeInfo().Where(p => p.currency_code == moneyType).GetTContext<BasiMoneyTypeInfo>().currency_value.ToInt32();
                //计算金额
                totalMoney = totalMoney + currValue * reduceNumber * 100;
                if (resStorage != 1)
                {
                    Util.DataBase.Rollback();
                    MessageDialog.Show("操作员现金归还失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Check_In,"1","操作员现金减少或库存增加失败");
                    return null;
                }

            }
            //操作员现金清零
             int resOperator = TickMonyBoxHelp.Instance.updateCashInOperatorInfo(operationCode);
            //操作员归还日志
             int resLog = BuinessRule.GetInstace().logManager.AddCashOperatorLog("01", totalMoney, operationCode);
            //修改结算数据标志
            int updRes = TickMonyBoxHelp.Instance.UpdateDataSettlementInfo(operationCode,settleDate);
            //营收金额
            double receMoney = Convert.ToDouble(actionParamsList.Single(temp => temp.bindingData.Equals("receMoney")).value.ToString()) * 100;
            //备用金
            double operationInMoney = Convert.ToDouble(actionParamsList.Single(temp => temp.bindingData.Equals("operationInMoney")).value.ToString()) * 100;
         

            //插入或修改结帐流水表
            int insRes = TickMonyBoxHelp.Instance.insertDataOperSettlementInfo(operationCode, settleDate, Convert.ToInt32(receMoney), Convert.ToInt32(operationInMoney), Convert.ToInt32(totalMoney));


            //20120828 dusj modify begin 增加硬币库存增加硬币库存日志
            int intCoinNo = Convert.ToInt32(string.IsNullOrEmpty(this.coinN0) ? "0" : this.coinN0);
            if (intCoinNo > 0)
            {
                string coinMoneyCode = TickMonyBoxHelp.CoinCurrency;
                //库存增加
                int coinStorage = TickMonyBoxHelp.Instance.updateStorageInfo(coinMoneyCode, intCoinNo, 2);
            }
            //20120828 dusj modify end 

            if (resOperator*resLog*updRes*insRes  != 1)
            {
                Util.DataBase.Rollback();
                MessageDialog.Show("操作员现金归还失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                //记错误日志
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Check_In, "1", "修改结算数据标志或修改结帐流水表失败");
                return null;
            }

            //dusj modify begin 20121022 增加自定义库种的散票归还
            tickType =
                actionParamsList.Single(temp => temp.bindingData.Equals("tickTypeList")).value as
                List<TickManaProductData>;
            if (tickType != null && tickType.Count>0)
            {
                for(int i=0;i<tickType.Count;i++)
                {
                    int tickRes = BuinessRule.GetInstace().tickMan.TickOperatorCheckIn(tickType[i].TickStoreType,
                                                                                       operationCode,
                                                                                       tickType[i].TickNum);
                    if(tickRes<1)
                    {
                        Util.DataBase.Rollback();
                        MessageDialog.Show("操作员现金归还失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return null;
                    }
                }
            }

            Util.DataBase.Commit();
            MessageDialog.Show("操作员现金归还成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Check_In, "0", "操作员现金归还成功");
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion
        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            int res = BuinessRule.GetInstace().logManager.AddDPLogInfo(OperationCode.Cash_Check_In, operatorId, "操作员现金归还");
            return res == 0;
        }

        #endregion

    }
}
