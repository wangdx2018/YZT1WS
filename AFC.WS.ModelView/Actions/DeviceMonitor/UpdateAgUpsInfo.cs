using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.Const;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;
using AFC.WS.BR;

namespace AFC.WS.ModelView.Actions.DeviceMonitor
{
    public class UpdateAgUpsInfo : IAction
    {
        #region IAction 成员

        string deviceId = string.Empty;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            deviceId = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            if (string.IsNullOrEmpty(deviceId))
            {
                MessageDialog.Show("请输入设备编号", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (deviceId.Substring(0, 4) != SysConfig.GetSysConfig().LocalParamsConfig.StationCode || deviceId.Substring(4, 2) != "06")
            {
                MessageDialog.Show("输入的设备编号不属于闸机", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (BuinessRule.GetInstace().GetBasiDeviceIdInfo(deviceId).device_id == null)
            {
                MessageDialog.Show("输入的设备编号不存在", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            DevUpsStatus upsMap = new DevUpsStatus();
            upsMap.ups_id = actionParamsList.Single(temp => temp.bindingData.Equals("ups_id")).value.ToString();
            upsMap.device_id = deviceId;
            upsMap.power_percent = "100";
            upsMap.power_status = "00";
            upsMap.ups_status = "00";
            upsMap.is_off = "00";
            upsMap.shut_date =string.Empty;
            upsMap.shut_time = string.Empty;
            upsMap.update_date = DateTime.Now.ToString("yyyyMMdd");
            upsMap.update_time = DateTime.Now.ToString("HHmmss");
            upsMap.operator_id = BuinessRule.GetInstace().OperatorId;
            int res = DBCommon.Instance.UpdateTable(upsMap, "dev_ups_status", new KeyValuePair<string, string>("UPS_ID", upsMap.ups_id));
            if (res != 1)
            {
                MessageDialog.Show("修改失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.dev_ups_status_update, "1", "修改UPS失败");
                return null;
            }
            else
            {
                MessageDialog.Show("修改成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.dev_ups_status_update, "0", "修改UPS成功");
                return null;
            }
            return null;
        }

        #endregion
    }
}
