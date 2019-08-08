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
    public class CashStoreAdd : IAction
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
            if (string.IsNullOrEmpty(CashStoreType))
            {
                Wrapper.ShowDialog("请填写货币库存类型。");
                return false;
            }
            if (string.IsNullOrEmpty(CashName))
            {
                Wrapper.ShowDialog("请填写货币库存名称。");
                return false;
            }
            if (string.IsNullOrEmpty(txtIndex))
            {
                Wrapper.ShowDialog("请填写货币面值。");
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
            res = DBCommon.Instance.InsertTable(info, "basi_money_type_info");
            if (res != 1)
            {
                Wrapper.ShowDialog("货币种类表操作失败。");
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Store_Add, "1", "货币种类表操作失败");
                Util.DataBase.Rollback();
                return null;
            }

            CashStorageInfo info1 = new CashStorageInfo();
            int res1 = 0;
            info1.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            info1.station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            info1.currency_code = CashStoreType;
            info1.currency_num = 0;
            info1.update_date = DateTime.Now.ToString("yyyyMMdd");
            info1.update_time = DateTime.Now.ToString("HHmmss");
            res1 = DBCommon.Instance.InsertTable(info1, "cash_storage_info");
            if (res1 == 1)
            {
                Wrapper.ShowDialog("新货币类型添加成功。");
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Store_Add, "0", "新货币类型添加成功");
                Util.DataBase.Commit();
            }
            else 
            {
                Wrapper.ShowDialog("货币库存表操作失败。");
                BR.BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Cash_Store_Add, "1", "货币库存表操作失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }

        #endregion
    }

}