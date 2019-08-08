using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.RfidRW;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.ModelView.Convertors;
using AFC.WS.BR;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.UI.Config;
using AFC.WS.UI.CommonControls;
using System.Windows;
using AFC.WS.BR.LogManager;
using AFC.WS.BR.TickBoxManager;
using AFC.WS.Model.Const;

namespace AFC.WS.ModelView.Actions.MoneyStoreActions
{
    public class CashStoreUpdate  :IAction
    {
        #region IAction 成员

        string CashStoreType = string.Empty;
        string CashName = string.Empty;
        string txtIndex = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("CashStoreType")).value != null)
            {
                CashStoreType = actionParamsList.Single(temp => temp.bindingData.Equals("CashStoreType")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("CashName")).value != null)
            {
                CashName = actionParamsList.Single(temp => temp.bindingData.Equals("CashName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("txtIndex")).value != null)
            {
                txtIndex = actionParamsList.Single(temp => temp.bindingData.Equals("txtIndex")).value.ToString();
            }
            if (string.IsNullOrEmpty(txtIndex))
            {
                Wrapper.ShowDialog("请填写货币库存面值。");
                return false;
            }
            if (string.IsNullOrEmpty(CashName))
            {
                Wrapper.ShowDialog("请填写货币库存名称。");
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
            //开启事务
            Util.DataBase.BeginTransaction();
            int res = 0;
            BasiMoneyTypeInfo info = new BasiMoneyTypeInfo();
            info.currency_code = CashStoreType;
            info.currency_name = CashName;
            info.currency_value = txtIndex;

            //插入新票卡种类
            res = DBCommon.Instance.UpdateTable(info, "basi_money_type_info", new KeyValuePair<string, string>("currency_code", CashStoreType));
      
            if (res == 1)
            {
                Wrapper.ShowDialog("货币类型重命名成功。");
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Store_Update, "0", "货币类型重命名成功");
                Util.DataBase.Commit();
            }
            else 
            {
                Wrapper.ShowDialog("货币类型重命名失败。");
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Store_Update, "1", "货币类型重命名失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }

        #endregion
    }

}