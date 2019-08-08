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
using AFC.WS.UI.DataSources;

namespace AFC.WS.ModelView.Actions.DeviceMonitor
{
    public class AlarmStatusAction : IAction
    {
        #region IAction 成员
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择需要确认的设备故障", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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

            var deviceId = actionParamsList.Where(temp => temp.bindingData.Equals("device_id")).ToList();
            var statusId = actionParamsList.Where(temp => temp.bindingData.Equals("status_id")).ToList();
            var statusValue = actionParamsList.Where(temp => temp.bindingData.Equals("status_value")).ToList();
            var updateDate = actionParamsList.Where(temp => temp.bindingData.Equals("update_date")).ToList();
            var updateTime = actionParamsList.Where(temp => temp.bindingData.Equals("update_time")).ToList();
            List<DevStatusAlarmHistory> deviceCurrentAlarmNoConfirm = new List<DevStatusAlarmHistory>();
            for (int i = 0; i < deviceId.Count(); i++)
            {
                DevStatusAlarmHistory ds = new DevStatusAlarmHistory();
                ds.device_id = deviceId[i].value.ToString();
                ds.status_id = statusId[i].value.ToString();
                ds.status_value = statusValue[i].value.ToString();
                ds.update_date = updateDate[i].value.ToString();
                ds.update_time = updateTime[i].value.ToString();
                deviceCurrentAlarmNoConfirm.Add(ds);
            }
           Util.DataBase.BeginTransaction();
           for (int i = 0; i < deviceCurrentAlarmNoConfirm.Count(); i++)
                {
                    DevStatusAlarmHistory dataInfo =
                        DBCommon.Instance.GetModelValue<DevStatusAlarmHistory>(
                            string.Format("select t.* from dev_status_alarm_history t where t.device_id='{0}' "+
              " and t.status_id='{1}' and t.status_value='{2}' and t.update_date='{3}' and t.update_time='{4}' ", 
              deviceCurrentAlarmNoConfirm[i].device_id,
              deviceCurrentAlarmNoConfirm[i].status_id,
              deviceCurrentAlarmNoConfirm[i].status_value,
              deviceCurrentAlarmNoConfirm[i].update_date,
              deviceCurrentAlarmNoConfirm[i].update_time));
                    if (dataInfo != null)
                    {
                        //状态变成未处理
                        dataInfo.is_confirm = "00";
                        dataInfo.confirm_oper = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                        dataInfo.confirm_date = DateTime.Now.ToString("yyyymmdd");
                        dataInfo.confirm_time = DateTime.Now.ToString("HHmmss");
                        List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
                        tempList.Add(new KeyValuePair<string, string>("device_id", dataInfo.device_id));
                        tempList.Add(new KeyValuePair<string, string>("status_id", dataInfo.status_id));
                        tempList.Add(new KeyValuePair<string, string>("status_value", dataInfo.status_value));
                        tempList.Add(new KeyValuePair<string, string>("update_date", dataInfo.update_date));
                        tempList.Add(new KeyValuePair<string, string>("update_time", dataInfo.update_time));
                        int result = DBCommon.Instance.UpdateTable(dataInfo, "dev_status_alarm_history", tempList.ToArray());
                        if (result < 1)
                        {
                            MessageDialog.Show("处理失败", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information,
                                               AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Dev_Alarm_Confirm, "1", "设备报警状态确认失败");
                            Util.DataBase.Rollback();
                            return null;
                        }
                        else                      
                        {
                            MessageDialog.Show("确认完成", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information,
                                                AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                            BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Dev_Alarm_Confirm, "0", "设备报警状态确认成功");
                            Util.DataBase.Commit();
                        }
                    }
                }

                DataSourceManager.NotfiyDataSourceChange("ds_dev_current_status_alarm");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            
        }
        #endregion
    }
}
