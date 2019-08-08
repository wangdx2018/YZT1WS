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
    public class CloseAlarmMonitor : IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return false;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            string openAlarm = string.Format("update basi_run_param_info t set t.param_value = '01' where t.param_code = '0603'");
            try
            {
                int res = 0;
                Util.DataBase.SqlCommand(out res, openAlarm);
                MessageDialog.Show("设备报警监视结束", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Alarm_Status_CLose, "0", "设置设备报警监视结束成功");
                BR.BuinessRule.GetInstace().rm.StartRunMonitorThread(AsynMessageType.AlarmStatusClose);
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                MessageDialog.Show("设备报警监视结束设置失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Alarm_Status_CLose, "1", "设置设备报警监视结束失败");
            }
            //DataSourceManager.NotfiyDataSourceChange("ds_dev_status_alarm_history");

            return null;
        }

        #endregion
    }
}
