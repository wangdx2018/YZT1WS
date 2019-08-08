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
   public class PartsInoutAction: IAction
    {

        #region IAction 成员
       string partsID = string.Empty;
       string operatorID = string.Empty;
       string partsStatus = string.Empty;

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
            if (actionParamsList.Single(temp => temp.bindingData.Equals("partsStatus")).value != null)
            {
                partsStatus = actionParamsList.Single(temp => temp.bindingData.Equals("partsStatus")).value.ToString();
            }

            if (string.IsNullOrEmpty(partsID))
            {
                Wrapper.ShowDialog("请填写部件唯一标识。");
                return false;
            }
            //领用
            if (partsStatus == "01")
            {
                if (string.IsNullOrEmpty(operatorID))
                {
                    Wrapper.ShowDialog("请输入领用的操作人员编码。");
                    return false;
                }
                if (MaintenanceManager.Instance.isNotExistPrivOperatorInfo(operatorID))
                {
                    Wrapper.ShowDialog("请输入正确的领用人员编码。");
                    return false;
                }
            }

            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            MatainLablePartStore store = MaintenanceManager.Instance.GetMatainLableInfo(partsID);
            if (store != null && !string.IsNullOrEmpty(store.part_id))
            {
                string currStatus = store.status;

                if (currStatus.Equals("03"))
                {
                    Wrapper.ShowDialog("此部件已作废，不能操作！");
                    return null;
                }

                //领用
                if (partsStatus == "01")
                {
                    if (currStatus.Equals("00") == false)
                    {
                        Wrapper.ShowDialog("此部件不在库，不能领用！");
                        return null;
                    }
                }
                //归还
                if (partsStatus == "00")
                {
                    if (currStatus.Equals("00"))
                    {
                        Wrapper.ShowDialog("此部件已在库，不用归还！");
                        return null;
                    }
                }

                if (!string.IsNullOrEmpty(operatorID))
                {
                    store.check_out_operator = operatorID;
                }
                store.status = partsStatus;
                store.update_date = DateTime.Now.ToString("yyyyMMdd");
                store.update_time = DateTime.Now.ToString("HHmmss");
                int updateRes = MaintenanceManager.Instance.updateLablePartStore(store);
                if (updateRes == 1)
                {
                    Wrapper.ShowDialog("部件" + GetStatus() + "成功。");
                    return new ResultStatus { resultCode = 0, resultData = 0 };
                }
                else
                {
                    Wrapper.ShowDialog("部件" + GetStatus() + "失败。");
                    return null;
                }

            }
            else
            {
                Wrapper.ShowDialog("此部件不存在，请检查是否登记。");
                return null;
            }
                
        }

        private string GetStatus()
        {
            string strStatus = string.Empty;
            switch (partsStatus)
            {
                case "00":
                    strStatus = "归还";
                    return strStatus;
                case "01":
                    strStatus = "领用";
                    return strStatus;
                case "02":
                    strStatus = "调出";
                    return strStatus;
                case "03":
                    strStatus = "作废";
                    return strStatus;
                default:
                    return null;
            }
        }

        #endregion
    }
}
