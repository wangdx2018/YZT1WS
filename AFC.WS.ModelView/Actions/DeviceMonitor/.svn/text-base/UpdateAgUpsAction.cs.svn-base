using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Components;
using AFC.WS.UI.Config;

namespace AFC.WS.ModelView.Actions.DeviceMonitor
{
    public class UpdateAgUpsAction : IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要编辑的信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
                DevUpsStatus upsMap = new DevUpsStatus();
                upsMap.ups_id = actionParamsList.Single(temp => temp.bindingData.Equals("t.ups_id")).value.ToString();
                upsMap.device_id = actionParamsList.Single(temp => temp.bindingData.Equals("t.device_id")).value.ToString();
                upsMap.operator_id = actionParamsList.Single(temp => temp.bindingData.Equals("t.operator_id")).value.ToString();

                InteractiveControl ic = new InteractiveControl();
                InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\LogQuery\ui_UpdateAgUpsStatus.xml");
                if (icRule != null)
                {

                    InitRunParamDetails(icRule, upsMap);

                    ic.Initialize(icRule);
                }

                ShowDetailsDialog.ShowDetails("选中的信息" + upsMap.ups_id.ToString(), ic, 400, 500);
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        /// <summary>
        /// 将某个表中的具体的信息显示到交互界面上
        /// </summary>
        /// <param name="icRule">Para4314AutorunTime的实体信息表</param>
        private void InitRunParamDetails(InteractiveControlRule icRule, DevUpsStatus upsMap)
        {
            if (icRule == null || upsMap == null)
                return;
            for (int i = 0; i < icRule.ControlList.Count; i++)
            {
                ReBindingInteraciveControlConfig(icRule.ControlList[i], upsMap);
            }
        }
        /// <summary>
        /// 需要将初始数据付给控件信息
        /// </summary>
        /// <param name="controlInfo">控件信息</param>
        /// <param name="upsMap">操作员信息</param>
        private void ReBindingInteraciveControlConfig(ControlProperty controlInfo, DevUpsStatus upsMap)
        {
            try
            {
                if (upsMap == null)
                    return;
                switch (controlInfo.BindingField.Split('.')[0].ToUpper())
                {
                    case "UPS_ID":
                        controlInfo.InitValue = upsMap.ups_id;
                        return;
                    case "DEVICE_ID":
                        controlInfo.InitValue = upsMap.device_id;
                        return;
                    case "POWER_PERCENT":
                        controlInfo.InitValue = upsMap.power_percent;
                        return;
                    case "POWER_STATUS":
                        controlInfo.InitValue = upsMap.power_status;
                        return;
                    case "UPS_STATUS":
                        controlInfo.InitValue = upsMap.ups_status;
                        return;
                    case "IS_OFF":
                        controlInfo.InitValue = upsMap.is_off;
                        return;
                    case "SHUT_DATE":
                        controlInfo.InitValue = upsMap.shut_date;
                        return;
                    case "SHUT_TIME":
                        controlInfo.InitValue = upsMap.shut_time;
                        return;
                    case "OPERATOR_ID":
                        controlInfo.InitValue = upsMap.operator_id;
                        return;
                    case "UPDATE_DATE":
                        controlInfo.InitValue = upsMap.update_date;
                        return;
                    case "UPDATE_TIME":
                        controlInfo.InitValue = upsMap.update_time;
                        return;

                }
            }
            catch (Exception ex)
            {
                //todo
            }
        }

        #endregion
    }
}
