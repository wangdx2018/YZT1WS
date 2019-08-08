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
    public class NoLabelPartsRegAction : IAction
    {
        string provider = string.Empty;
        string partsID = string.Empty;
        string partsType = string.Empty;
        string operatorID = string.Empty;
        string partsNum = string.Empty;

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
            if (actionParamsList.Single(temp => temp.bindingData.Equals("partsNum")).value != null)
            {
                partsNum = actionParamsList.Single(temp => temp.bindingData.Equals("partsNum")).value.ToString();
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
            if (string.IsNullOrEmpty(partsNum))
            {
                Wrapper.ShowDialog("请填写调入数量。");
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
            MaintainNoLablePartStore store = new MaintainNoLablePartStore();
            store.instore_num = partsNum.ToInt32();
            store.part_id = partsID;
            store.part_type_id = partsType;
            store.provider_id = provider;
            store.update_date = DateTime.Now.ToString("yyyyMMdd");
            store.update_time = DateTime.Now.ToString("HHmmss");
            store.update_operator = operatorID;
            MaintainNoLablePartStore existStore = MaintenanceManager.Instance.GetNoLablePartStore(partsID);
            if (existStore != null && !string.IsNullOrEmpty(existStore.part_id))
            {
                Wrapper.ShowDialog("此部件库存已调入。");
                return null;
            }
            //无标签调入调出日志表
            MaintainNoLableOperLog log = new MaintainNoLableOperLog();
            log.num = partsNum.ToInt32();
            log.status = "00"; //调入
            log.part_id = partsID;
            log.part_type_id = partsType;
            log.provider_id = provider;
            log.operaotr = operatorID;
            log.update_date = DateTime.Now.ToString("yyyyMMdd");
            log.update_operator = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            log.update_time = DateTime.Now.ToString("HHmmss");

            int logRes = MaintenanceManager.Instance.insertNoLableOperLog(log);
            int insertRes = MaintenanceManager.Instance.insertNoLablePartStore(store);
            if (logRes * insertRes == 1)
            {
                Wrapper.ShowDialog("此部件调入成功。");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            else
            {
                Wrapper.ShowDialog("此部件调入失败。");
                return null;
            }
            
        }

        #endregion
    }
}
