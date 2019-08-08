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
using AFC.WS.UI.BR.Data;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    public class UpdatePara4042DeviceInfo:  IAction
    {
        #region IAction 成员

        string station_cn_name = string.Empty;
        string device_id = string.Empty;
        string station_hall_id = string.Empty;
        string device_group_id = string.Empty;
        string device_serial_no = string.Empty;
        string device_group_serial_no = string.Empty;
        string honri_index = string.Empty;
        string vertical_index = string.Empty;
        string display_angle = string.Empty;
        string device_ip = string.Empty;
        string start_flag = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("station_cn_name")).value != null)
            {
                station_cn_name = actionParamsList.Single(temp => temp.bindingData.Equals("station_cn_name")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value != null)
            {
                device_id = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("station_hall_id")).value != null)
            {
                station_hall_id = actionParamsList.Single(temp => temp.bindingData.Equals("station_hall_id")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("device_group_id")).value != null)
            {
                device_group_id = actionParamsList.Single(temp => temp.bindingData.Equals("device_group_id")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("device_serial_no")).value != null)
            {
                device_serial_no = actionParamsList.Single(temp => temp.bindingData.Equals("device_serial_no")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("device_group_serial_no")).value != null)
            {
                device_group_serial_no = actionParamsList.Single(temp => temp.bindingData.Equals("device_group_serial_no")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("honri_index")).value != null)
            {
                honri_index = actionParamsList.Single(temp => temp.bindingData.Equals("honri_index")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("vertical_index")).value != null)
            {
                vertical_index = actionParamsList.Single(temp => temp.bindingData.Equals("vertical_index")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("display_angle")).value != null)
            {
                display_angle = actionParamsList.Single(temp => temp.bindingData.Equals("display_angle")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("device_ip")).value != null)
            {
                device_ip = actionParamsList.Single(temp => temp.bindingData.Equals("device_ip")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("start_flag")).value != null)
            {
                start_flag = actionParamsList.Single(temp => temp.bindingData.Equals("start_flag")).value.ToString();
            }

            if (string.IsNullOrEmpty(station_cn_name))
            {
                Wrapper.ShowDialog("请填写车站。");
                return false;
            }
            if (string.IsNullOrEmpty(device_id))
            {
                Wrapper.ShowDialog("请填写设备编号。");
                return false;
            }
            else if (device_id.Length < 8)
            {
                Wrapper.ShowDialog("请填写完整的设备编号。");
                return false;
            }
            if (string.IsNullOrEmpty(station_hall_id))
            {
                Wrapper.ShowDialog("请填写站厅编号。");
                return false;
            }
            else if (station_hall_id.Length < 2)
            {
                station_hall_id = "0" + station_hall_id;
            }
            if (string.IsNullOrEmpty(device_group_id))
            {
                Wrapper.ShowDialog("请填写设备组编号。");
                return false;
            }
            if (string.IsNullOrEmpty(device_serial_no))
            {
                Wrapper.ShowDialog("请填写设备序列号。");
                return false;
            }
            if (string.IsNullOrEmpty(device_group_serial_no))
            {
                Wrapper.ShowDialog("请填写设备组序列号。");
                return false;
            }
            if (string.IsNullOrEmpty(honri_index))
            {
                Wrapper.ShowDialog("请填写横轴坐标。");
                return false;
            }
            if (string.IsNullOrEmpty(vertical_index))
            {
                Wrapper.ShowDialog("请填写纵轴坐标。");
                return false;
            }
            if (string.IsNullOrEmpty(display_angle))
            {
                Wrapper.ShowDialog("请填写朝向角度。");
                return false;
            }
            if (string.IsNullOrEmpty(device_ip))
            {
                Wrapper.ShowDialog("请填写设备IP。");
                return false;
            }
            else if (!Utility.Instance.IsValidIP(device_ip))
            {
                Wrapper.ShowDialog("请填写正确格式的IP地址。");
                return false;
            }
            if (string.IsNullOrEmpty(start_flag))
            {
                Wrapper.ShowDialog("请填写设备状态。");
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
            string para_version = "-1";
            Para4042DeviceInfo info = new Para4042DeviceInfo();
            info.para_version = para_version;
            info.line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            string station_id = BuinessRule.GetInstace().GetStationInfoByName(station_cn_name).station_id;
            info.station_id = station_id;
            info.station_map_name = "1";
            info.device_id = device_id;
            info.device_name = BuinessRule.GetInstace().GetBasiDevTypInfo(device_id.Substring(4, 2)).device_name;
            info.device_type = BuinessRule.GetInstace().GetBasiDevTypInfo(device_id.Substring(4, 2)).device_type;
            info.device_sub_type = BuinessRule.GetInstace().GetBasiDevTypInfo(device_id.Substring(4, 2)).device_sub_type;
            info.device_serial_no = device_serial_no;
            info.station_hall_id =  station_hall_id;
            info.device_group_id = device_group_id;
            info.device_group_serial_no = device_group_serial_no;
            info.honri_index = honri_index;
            info.vertical_index = vertical_index;
            info.display_angle = display_angle;
            info.device_ip = device_ip;
            info.start_flag = start_flag;

            //插入新维修工区
            res = DBCommon.Instance.UpdateTable(info, "para_4042_device_info",
                    new KeyValuePair<string, string>("para_version", para_version),
                    new KeyValuePair<string, string>("station_id", station_id),
                    new KeyValuePair<string, string>("device_id", device_id));
            if (res == 1)
            {
                Wrapper.ShowDialog("设备参数信息更新成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Para_4042_Device_Info_Update, "0", "设备参数信息更新成功");
                Util.DataBase.Commit();
                return null;
            }
            else
            {
                Wrapper.ShowDialog("设备参数信息更新失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Para_4042_Device_Info_Update, "1", "设备参数信息更新失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }



        #endregion
    }

}