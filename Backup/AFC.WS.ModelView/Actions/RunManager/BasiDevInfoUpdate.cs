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

namespace AFC.WS.ModelView.Actions.RunManager
{
    public class BasiDevInfoUpdate:  IAction
    {
        #region IAction 成员

        string LineName = string.Empty;
        string StationName = string.Empty;
        string DeviceId = string.Empty;
        string StationHallId = string.Empty;
        string StationHallName = string.Empty;
        string HallGroupId = string.Empty;
        string HallGroupName = string.Empty;
        string HallGroupSerialNo = string.Empty;
        string stationId = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要更新的设备站厅组别信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("LineName")).value != null)
            {
                LineName = actionParamsList.Single(temp => temp.bindingData.Equals("LineName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("StationName")).value != null)
            {
                StationName = actionParamsList.Single(temp => temp.bindingData.Equals("StationName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("DeviceId")).value != null)
            {
                DeviceId = actionParamsList.Single(temp => temp.bindingData.Equals("DeviceId")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("StationHallId")).value != null)
            {
                StationHallId = actionParamsList.Single(temp => temp.bindingData.Equals("StationHallId")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("StationHallName")).value != null)
            {
                StationHallName = actionParamsList.Single(temp => temp.bindingData.Equals("StationHallName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("HallGroupName")).value != null)
            {
                HallGroupName = actionParamsList.Single(temp => temp.bindingData.Equals("HallGroupName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("HallGroupSerialNo")).value != null)
            {
                HallGroupSerialNo = actionParamsList.Single(temp => temp.bindingData.Equals("HallGroupSerialNo")).value.ToString();
            }
            if (string.IsNullOrEmpty(LineName))
            {
                Wrapper.ShowDialog("请填写线路。");
                return false;
            }
            if (string.IsNullOrEmpty(StationName))
            {
                Wrapper.ShowDialog("请填写车站。");
                return false;
            }
            if (string.IsNullOrEmpty(DeviceId))
            {
                Wrapper.ShowDialog("请填写设备编码。");
                return false;
            }
            if (string.IsNullOrEmpty(StationHallId))
            {
                Wrapper.ShowDialog("请填写车站站厅编码。");
                return false;
            }
            if (string.IsNullOrEmpty(StationHallName))
            {
                Wrapper.ShowDialog("请填写车站站厅名称。");
                return false;
            }
            if (string.IsNullOrEmpty(HallGroupName))
            {
                Wrapper.ShowDialog("请填写车站站厅组别名称。");
                return false;
            }
            if (string.IsNullOrEmpty(HallGroupSerialNo))
            {
                Wrapper.ShowDialog("请填写设备组内序号。");
                return false;
            }
            stationId = BuinessRule.GetInstace().GetStationInfoByName(StationName).station_id.ToString();
            HallGroupId = BuinessRule.GetInstace().GetBasiHallGroupByName(stationId, StationHallId, HallGroupName).hall_group_id.ToString();
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
            BasiDevInfo info = new BasiDevInfo();
            info = BuinessRule.GetInstace().GetBasiDevInfoById(DeviceId);
            info.station_hall_id = StationHallId;
            info.hall_group_id = HallGroupId;
            info.group_serial_no = HallGroupSerialNo;

            //设备站厅组别序号更新
            res = DBCommon.Instance.UpdateTable(info, "basi_dev_info", new KeyValuePair<string, string>("device_id", DeviceId));
            if (res == 1)
            {
                Wrapper.ShowDialog("设备站厅组别序号更新成功。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.basi_dev_info_sn_Update, "0", "设备站厅组别序号更新成功");
                Util.DataBase.Commit();
                return null;
            }
            else
            {
                Wrapper.ShowDialog("设备站厅组别序号更新失败。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.basi_dev_info_sn_Update, "1", "设备站厅组别序号更新失败");
                Util.DataBase.Rollback();
                return null;
            }
            return null;
        }

        #endregion
    }

}