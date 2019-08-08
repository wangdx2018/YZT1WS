using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.BR;
using AFC.WS.UI.Config;
using AFC.WS.Model.DB;
using AFC.WS.BR.Maintenance;

namespace AFC.WS.ModelView.Actions.Maintenance
{
    public class PartsInventoryRegAction:  IAction
    {
        string provider = string.Empty;
        string partsID = string.Empty;
        string partsType = string.Empty;
        string operatorID = string.Empty;

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("provider")).value != null)
            {
                provider = actionParamsList.Single(temp => temp.bindingData.Equals("provider")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("partID")).value != null)
            {
                partsID = actionParamsList.Single(temp => temp.bindingData.Equals("partID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("partType")).value != null)
            {
                partsType = actionParamsList.Single(temp => temp.bindingData.Equals("partType")).value.ToString();
            }

            if (actionParamsList.Single(temp => temp.bindingData.Equals("operatorID")).value != null)
            {
                operatorID = actionParamsList.Single(temp => temp.bindingData.Equals("operatorID")).value.ToString();
            }

            if (string.IsNullOrEmpty(partsID))
            {
                Wrapper.ShowDialog("请填写部件唯一标识。");
                return false;
            }
            if (string.IsNullOrEmpty(provider))
            {
                Wrapper.ShowDialog("请选择供应商ID。");
                return false;
            }

            if (string.IsNullOrEmpty(partsType))
            {
                Wrapper.ShowDialog("请选择部件类型。");
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
            try
            {
                MatainLablePartStore store = new MatainLablePartStore();
                store.check_out_operator = "";
                store.part_id = partsID;
                store.part_type_id = partsType;
                store.provider_id = provider;
                store.status = "00";   //在库
                store.update_date = DateTime.Now.ToString("yyyyMMdd");
                store.update_time = DateTime.Now.ToString("HHmmss");
                store.update_operator = operatorID;
                bool isExist = MaintenanceManager.Instance.IsExistsReg(partsID);
                if (isExist)
                {
                    Wrapper.ShowDialog("此部件库存已调入。");
                    return null;
                }

                int insertReg = MaintenanceManager.Instance.insertLablePartStore(store);
                if (insertReg != 1)
                {
                    Wrapper.ShowDialog("部件库存调入失败。");
                    return null;
                }

                Wrapper.ShowDialog("部件库存调入成功。");
                return null;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return null;
            }
        }

        #endregion
    }
}
