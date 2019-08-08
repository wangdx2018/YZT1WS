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
    public class BasiHallGroupIdInfoAdd : IAction
    {
        #region IAction 成员

        string LineName = string.Empty;
        string StationName = string.Empty;
        string StationHallId = string.Empty;
        string StationHallName = string.Empty;
        string HallGroupId = string.Empty;
        string HallGroupName = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList.Single(temp => temp.bindingData.Equals("LineName")).value != null)
            {
                LineName = actionParamsList.Single(temp => temp.bindingData.Equals("LineName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("StationName")).value != null)
            {
                StationName = actionParamsList.Single(temp => temp.bindingData.Equals("StationName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("StationHallId")).value != null)
            {
                StationHallId = actionParamsList.Single(temp => temp.bindingData.Equals("StationHallId")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("StationHallName")).value != null)
            {
                StationHallName = actionParamsList.Single(temp => temp.bindingData.Equals("StationHallName")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("HallGroupId")).value != null)
            {
                HallGroupId = actionParamsList.Single(temp => temp.bindingData.Equals("HallGroupId")).value.ToString();
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("HallGroupName")).value != null)
            {
                HallGroupName = actionParamsList.Single(temp => temp.bindingData.Equals("HallGroupName")).value.ToString();
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
            if (string.IsNullOrEmpty(HallGroupId))
            {
                Wrapper.ShowDialog("请填写车站站厅组别编码。");
                return false;
            }
            if (string.IsNullOrEmpty(HallGroupName))
            {
                Wrapper.ShowDialog("请填写车站站厅组别名称。");
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
            BasiHallGroupIdInfo info = new BasiHallGroupIdInfo();
            info.station_id = BuinessRule.GetInstace().GetStationInfoByName(StationName).station_id.ToString();
            info.station_hall_id = StationHallId;
            if (string.IsNullOrEmpty(BuinessRule.GetInstace().GetBasiHallGroupById(info.station_id, StationHallId, HallGroupId).hall_group_id))
            {
                info.hall_group_id = HallGroupId;
                info.hall_group_name = HallGroupName;

                //插入新车站站厅组别
                res = DBCommon.Instance.InsertTable(info, "basi_hall_group_id_info");
                if (res == 1)
                {
                    Wrapper.ShowDialog("车站站厅组别增加成功。");
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.basi_hall_group_id_info_Add, "0", "车站站厅组别增加成功");
                    Util.DataBase.Commit();
                    return null;
                }
                else
                {
                    Wrapper.ShowDialog("车站站厅组别增加失败。");
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.basi_hall_group_id_info_Add, "1", "车站站厅组别增加失败");
                    Util.DataBase.Rollback();
                    return null;
                }
            }
            else
            {
                Wrapper.ShowDialog("新增车站站厅组别已存在。");
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.basi_hall_group_id_info_Add, "1", "车站站厅组别增加失败");
                return null;
            }
        }

        #endregion
    }

}