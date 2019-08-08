using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.Const;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR;

namespace AFC.WS.ModelView.Actions.DeviceMonitor
{
    public class AddAgUpsAction : IAction
    {
        #region IAction 成员
        string upsID = string.Empty;
        string deviceID = string.Empty;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            upsID = actionParamsList.Single(temp => temp.bindingData.Equals("ups_id")).value.ToString();
            deviceID = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();

            if (string.IsNullOrEmpty(upsID) || BuinessRule.GetInstace().GetDevUpsMap(upsID).ups_id == upsID)
            {
                MessageDialog.Show("UPS编号为空或UPS编号已存在", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (BuinessRule.GetInstace().GetDevUpsStatus(deviceID).device_id != null)
            {
                MessageDialog.Show("该设备已使用", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            //if (string.IsNullOrEmpty(deviceID))
            //{
            //    MessageDialog.Show("请输入设备编号", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            //if (deviceID.Substring(0, 4)!=SysConfig.GetSysConfig().LocalParamsConfig.StationCode || deviceID.Substring(4,2)!="06")
            //{
            //    MessageDialog.Show("输入的设备编号不属于闸机", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            //if (BuinessRule.GetInstace().GetBasiDeviceIdInfo(deviceID).device_id==null)
            //{
            //    MessageDialog.Show("输入的设备编号不存在", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            DevUpsMap dum=new DevUpsMap();
            dum.ups_id = upsID;
            dum.device_id = deviceID;
            DevUpsStatus upsMap = new DevUpsStatus();
            upsMap.ups_id = upsID;
            upsMap.device_id = deviceID;
            upsMap.power_percent = "100";
            upsMap.power_status = "00";
            upsMap.ups_status = "00";
            upsMap.is_off = "00";
            upsMap.shut_date = string.Empty;
            upsMap.shut_time = string.Empty;
            upsMap.update_date = DateTime.Now.ToString("yyyyMMdd");
            upsMap.update_time = DateTime.Now.ToString("HHmmss");
            upsMap.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            int result = DBCommon.Instance.InsertTable(upsMap, "dev_ups_status");
            int res = DBCommon.Instance.InsertTable(dum, "dev_ups_map");
            if (result != 1 && res!=1)
            {
                MessageDialog.Show("添加失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.dev_ups_status_add, "1", "添加UPS失败");
            }
            else
            {
                MessageDialog.Show("添加成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.dev_ups_status_add, "0", "添加UPS成功");
            }

            return null;
        }

        #endregion
    }
}
