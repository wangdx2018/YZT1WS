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

namespace AFC.WS.ModelView.Actions.MaintainAreaManager
{
    public class BasiProviderInfoUpdate : IAction
    {
        #region IAction 成员

        string ProviderID = string.Empty;
        string ProviderName = string.Empty;
        string ProviderAddress = string.Empty;
        string ProviderContector = string.Empty;
        string ProviderPhone = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("ProviderID")).value != null)
            {
                ProviderID = actionParamsList.Single(temp => temp.bindingData.Equals("ProviderID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("ProviderName")).value != null)
            {
                ProviderName = actionParamsList.Single(temp => temp.bindingData.Equals("ProviderName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("ProviderAddress")).value != null)
            {
                ProviderAddress = actionParamsList.Single(temp => temp.bindingData.Equals("ProviderAddress")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("ProviderContector")).value != null)
            {
                ProviderContector = actionParamsList.Single(temp => temp.bindingData.Equals("ProviderContector")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("ProviderPhone")).value != null)
            {
                ProviderPhone = actionParamsList.Single(temp => temp.bindingData.Equals("ProviderPhone")).value.ToString();
            }
            if (string.IsNullOrEmpty(ProviderID))
            {
                Wrapper.ShowDialog("请填写供应商编码。");
                return false;
            }
            if (string.IsNullOrEmpty(ProviderName))
            {
                Wrapper.ShowDialog("请填写供应商名称。");
                return false;
            }
            if (string.IsNullOrEmpty(ProviderAddress))
            {
                Wrapper.ShowDialog("请填写供应商地址。");
                return false;
            }
            if (string.IsNullOrEmpty(ProviderName))
            {
                Wrapper.ShowDialog("请填写供应商联系人。");
                return false;
            }
            if (string.IsNullOrEmpty(ProviderAddress))
            {
                Wrapper.ShowDialog("请填写供应商联系电话。");
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
            BasiProviderInfo info = new BasiProviderInfo();
            info.provider_id = ProviderID;
            info.mc_dep_name = ProviderName;
            info.provider_address = ProviderAddress;
            info.contract_person = ProviderContector;
            info.phone = ProviderPhone;
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");

            //更新设备部件名称
            res = DBCommon.Instance.UpdateTable(info, "basi_provider_info", new KeyValuePair<string, string>("provider_id", ProviderID));
      
            if (res == 1)
            {
                Wrapper.ShowDialog("更新供应商信息成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Basi_Provider_Info_Update, "0", "更新供应商信息成功");
                Util.DataBase.Commit();
            }
            else 
            {
                Wrapper.ShowDialog("更新供应商信息失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Basi_Provider_Info_Update, "1", "更新供应商信息失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }

        #endregion
    }

}