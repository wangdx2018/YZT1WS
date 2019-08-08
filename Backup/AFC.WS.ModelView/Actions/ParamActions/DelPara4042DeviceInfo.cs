using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.DataSources;
using AFC.WS.Model.Const;
using AFC.WS.BR;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    public class DelPara4042DeviceInfo : IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要删除的参数", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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

            string station_cn_name = actionParamsList.Single(temp => temp.bindingData.Equals("station_cn_name")).value.ToString();
            string station_id = BuinessRule.GetInstace().GetStationInfoByName(station_cn_name).station_id;
            string device_id = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            string delSql = string.Format("delete para_4042_device_info t  where t.para_version ='-1' and t.station_id='{0}' and t.device_id ='{1}'", station_id, device_id);
           try
           {
               int res = 0;
               Util.DataBase.SqlCommand(out res, delSql);
               MessageDialog.Show("删除参数成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
               BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Para_4042_Device_Info_Delete, "0", "设备参数信息添加成功");
           }
           catch (Exception ex)
           {
               AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
               BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Para_4042_Device_Info_Delete, "1", "设备参数信息添加失败");
           }
           DataSourceManager.NotfiyDataSourceChange("ds_para_4314");

           return null;
        }
        

        #endregion
    }
}
