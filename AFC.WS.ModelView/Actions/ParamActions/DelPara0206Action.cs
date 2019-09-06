using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.DataSources;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    class DelPara0206Action: IAction
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
            string deviceId = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            string delSql = string.Format("delete from para_0206_station_cfg_ctrl   where device_id ='{0}'and para_version ='0000'", deviceId);
            try
            {
                int res = 0;
                Util.DataBase.SqlCommand(out res, delSql);
                MessageDialog.Show("删除参数成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                MessageDialog.Show("删除参数失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            DataSourceManager.NotfiyDataSourceChange("ds_para_0206_station_cfg_ctrl");

            return null;
        }


        #endregion
    }
}
