using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.DeviceMonitor
{

    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.SLEMonitorManager;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;
    /// <summary>
    /// added by wangdx 20110708
    /// 报警设置
    /// 
    /// </summary>
    public class AlarmSettingAction:IAction
    {

        private string alarmLevel;

        private List<string> alarmStyle = new List<string>();

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择报警级别和报警方式", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return false;
            }
            try
            {
                alarmLevel = actionParamsList.Single(temp => temp.bindingData.Equals("alarmType")).value.ToString();
                this.alarmStyle = actionParamsList.Single(temp => temp.bindingData.Equals("alarmStyle")).value as List<string>;

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.Show("请选择报警级别和报警方式", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                WriteLog.Log_Error(ex.Message);
                return false;
            }
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return false;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            ErrorAlarm ea = new ErrorAlarm();
           int res= ea.UpdateAlarmStyle(alarmLevel, alarmStyle);
           if (res == 0)
           {
               MessageDialog.Show("报警方式设置成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
               BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Alarm_Style, "0", "报警方式设置成功");
               return new ResultStatus { resultCode = 0, resultData = 0 };
           }
           else
           {
               MessageDialog.Show("报警方式设置失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
               BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Alarm_Style, "1", "报警方式设置失败");
               return null;
           }
        }

        #endregion
    }
}
