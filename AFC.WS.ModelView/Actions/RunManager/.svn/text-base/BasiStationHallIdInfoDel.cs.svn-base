using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.DataSources;
using AFC.WS.BR;
using AFC.WS.Model.Const;

namespace AFC.WS.ModelView.Actions.RunManager
{
    public class BasiStationHallIdInfoDel : IAction
    {
        #region IAction 成员
        public bool CheckValid(List<QueryCondition> actionParamsList)
       {
           if (actionParamsList == null || actionParamsList.Count == 0)
           {
               MessageDialog.Show("请选择要删除的车站站厅", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            string StationName = actionParamsList.Single(temp => temp.bindingData.Equals("station_cn_name")).value.ToString();
            string StationId = BuinessRule.GetInstace().GetStationInfoByName(StationName).station_id.ToString();
            string StationHallId = actionParamsList.Single(temp => temp.bindingData.Equals("station_hall_id")).value.ToString();
            string StationHallName = actionParamsList.Single(temp => temp.bindingData.Equals("station_hall_name")).value.ToString();
            string delSql = string.Format("delete basi_station_hall_id_info t where t.station_id ='{0}' and t.station_hall_id ='{1}'", StationId, StationHallId);
           try
           {
               int res = 0;
               Util.DataBase.SqlCommand(out res, delSql);
               MessageDialog.Show("删除车站站厅信息成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
               BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.basi_station_hall_id_info_Delete, "0", "车站站厅删除成功");
           }
           catch (Exception ex)
           {
               AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
               MessageDialog.Show("删除车站站厅信息失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
               BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.basi_station_hall_id_info_Delete, "1", "车站站厅删除失败");
           }
           DataSourceManager.NotfiyDataSourceChange("ds_stationHall");
           return null;
        }

        #endregion
    }
}
