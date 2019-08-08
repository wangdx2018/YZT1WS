using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.BR.Maintenance;
using AFC.WS.BR;

namespace AFC.WS.ModelView.Actions.Maintenance
{
    public class NoLabelPartsOutAction:  IAction
    {
        #region IAction 成员

        string partsID = string.Empty;
        string operatorID = string.Empty;
        string partsNum = string.Empty;
        string instoreNum = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("partsID")).value != null)
            {
                partsID = actionParamsList.Single(temp => temp.bindingData.Equals("partsID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("operatorID")).value != null)
            {
                operatorID = actionParamsList.Single(temp => temp.bindingData.Equals("operatorID")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("partsNum")).value != null)
            {
                partsNum = actionParamsList.Single(temp => temp.bindingData.Equals("partsNum")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("instoreNum")).value != null)
            {
                instoreNum = actionParamsList.Single(temp => temp.bindingData.Equals("instoreNum")).value.ToString();
            }
            if (string.IsNullOrEmpty(partsID))
            {
                Wrapper.ShowDialog("请填写部件唯一标识。");
                return false;
            }
            if (string.IsNullOrEmpty(partsNum))
            {
                Wrapper.ShowDialog("请填写调出数量。");
                return false;
            }
            if (string.IsNullOrEmpty(operatorID))
            {
                Wrapper.ShowDialog("请填写操作员编码。");
                return false;
            }
            if (partsNum.ToInt32() > instoreNum.ToInt32())
            {
                Wrapper.ShowDialog("调出数量不能大于在库数量。");
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
            MaintainNoLablePartStore store = MaintenanceManager.Instance.GetNoLablePartStore(partsID);
            if (store != null && !string.IsNullOrEmpty(store.part_id))
            {
                if (!string.IsNullOrEmpty(operatorID))
                {
                    store.update_operator = operatorID;
                }
                store.instore_num = (store.instore_num - partsNum.ToInt32());
                store.update_date = DateTime.Now.ToString("yyyyMMdd");
                store.update_time = DateTime.Now.ToString("HHmmss");
                int updateRes = MaintenanceManager.Instance.updateNoLablePartStore(store);


                MaintainNoLableOperLog log = new MaintainNoLableOperLog();
                log.num = (store.instore_num - partsNum.ToInt32());
                log.operaotr = operatorID;
                log.part_id = partsID;
                log.part_type_id = store.part_type_id;
                log.provider_id = store.provider_id;
                log.update_date = DateTime.Now.ToString("yyyyMMdd");
                log.update_operator = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                log.update_time = DateTime.Now.ToString("HHmmss");

                int logRes = MaintenanceManager.Instance.updateNoLableOperLog(log);

                if (updateRes * logRes == 1)
                {
                    Wrapper.ShowDialog("部件调出成功");
                    return new ResultStatus { resultCode = 0, resultData = 0 };
                }
                else
                {
                    Wrapper.ShowDialog("部件调出失败。");
                    return null;
                }

            }
            else
            {
                Wrapper.ShowDialog("此部件不存在，请检查是否登记。");
                return null;
            }
        }

        #endregion
    }
}
