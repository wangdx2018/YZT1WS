using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.BR;
using AFC.WS.Model.Const;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using TJComm;

namespace AFC.WS.ModelView.Actions.DeviceMonitor
{
    public class AgUpsStatusAction : IAction
    {
        #region IAction 成员
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择UPS", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            //if (actionParamsList.Select(temp => temp.bindingData.Equals("t.power_status")).Equals("00") &&
            //    actionParamsList.Select(temp => temp.bindingData.Equals("t.power_percent")).ToString().ToInt32()
            //    > BuinessRule.GetInstace().GetRunParamByCode().param_value.ToInt32())
            //{
            //    MessageDialog.Show("该UPS不需要关机", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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

            //todo: 001: get upsId from ui params
            // todo: 002: get relactions deviceid accroding to upsId. 
            // call controlCmd functionl;\
            
            string upsId = actionParamsList.First(temp => temp.bindingData.Equals("t.ups_id")).value.ToString();
            var collection = actionParamsList.Where(temp => temp.bindingData.Equals("t.device_id")).ToList();
            List<string> deviceId = new List<string>();
            for (int i = 0; i < collection.Count(); i++)
            {
                string currentdeviceId = collection[i].value.ToString();
                deviceId.Add(currentdeviceId);
            }
            //List<DevUpsMap> deviceId = actionParamsList.Select(temp => temp.bindingData.Equals("")).ToList();
            
                //List<DevUpsMap> deviceId = BuinessRule.GetInstace().GetDevUpsMaps(upsId);
                // get devieId 
                List<DeviceRange> list = new List<DeviceRange>();
                DeviceRange dr = new DeviceRange();
                dr.stationId = SysConfig.GetSysConfig().LocalParamsConfig.StationCode.ConvertHexStringToUshort();
                dr.special_flag = 2;
                dr.deviceRange = new List<uint>();
                
                if (deviceId != null)
                {
                    foreach (var temp in deviceId)
                    {
                        dr.deviceRange.Add(temp.ToString().ConvertHexStringToUint());
                    }
                }
                list.Add(dr);
                int res = BuinessRule.GetInstace().commProcess.ControlCmd(0x01, 0x0101, list);
            

                if (res == 0)
                {
                    DevUpsStatus upsStatus = new DevUpsStatus();
                    upsStatus.ups_id = upsId;
                    upsStatus.device_id = BuinessRule.GetInstace().GetDevUpsStatusdev(upsId).device_id;
                    upsStatus.power_percent = BuinessRule.GetInstace().GetDevUpsStatusdev(upsId).power_percent;
                    upsStatus.power_status = BuinessRule.GetInstace().GetDevUpsStatusdev(upsId).power_status;
                    upsStatus.ups_status = "00";
                    upsStatus.is_off = "01";
                    upsStatus.shut_date = DateTime.Now.ToString("yyyyMMdd");
                    upsStatus.shut_time = DateTime.Now.ToString("HHmmss");
                    upsStatus.update_date = DateTime.Now.ToString("yyyyMMdd");
                    upsStatus.update_time = DateTime.Now.ToString("HHmmss");
                    upsStatus.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                    DBCommon.Instance.UpdateTable(upsStatus, "dev_ups_status", new KeyValuePair<string, string>("UPS_ID", upsStatus.ups_id));
                    //todo: MessageDialog
                    MessageDialog.Show("该UPS关机命令设置成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
                else
                {
                    MessageDialog.Show("该UPS关机命令设置失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    //todo:
                }
                return null;
            
        }
        #endregion
    }
}
