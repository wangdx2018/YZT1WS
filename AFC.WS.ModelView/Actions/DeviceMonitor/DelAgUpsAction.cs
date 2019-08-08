using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.BR;
using AFC.WS.Model.Const;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.DataSources;

namespace AFC.WS.ModelView.Actions.DeviceMonitor
{
    public class DelAgUpsAction : IAction
    {
        #region IAction 成员
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要删除信息", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            string UpsId = actionParamsList.Single(temp => temp.bindingData.Equals("t.ups_id")).value.ToString();
            string deviceId = actionParamsList.Single(temp => temp.bindingData.Equals("t.device_id")).value.ToString();
            string delSql = string.Format("delete dev_ups_status t  where t.ups_id ='{0}'", UpsId);
            string delSqlmap = string.Format("delete dev_ups_map t  where t.device_id ='{0}'", deviceId);
            try
            {
                int res = 0;
                Util.DataBase.SqlCommand(out res, delSql);
                Util.DataBase.SqlCommand(out res, delSqlmap);
                MessageDialog.Show("删除信息成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.dev_ups_status_delete, "0", "删除信息成功");
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                MessageDialog.Show("删除信息失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.dev_ups_status_delete, "1", "删除信息失败");
            }
            DataSourceManager.NotfiyDataSourceChange("ds_AgUpsStatusQuery");

            return null;
        }

        #endregion
    }
}
