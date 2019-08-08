using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using log4net;
using AFC.WS.BR;
using AFC.WS.Model.Const;
using AFC.WS.UI.Components;

namespace AFC.WS.ModelView.Actions.AlarmActions
{
    public class StatusVisibleSettingAction:IAction
    {


        //private readonly ILog log;

        public StatusVisibleSettingAction()
        {
           // log = LogManager.GetLogger("DeviceMonitor");
        }
      

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null ||
                actionParamsList.Count == 0)
            {
                MessageDialog.Show(UIHelper.GetResouceValueByID("DialogMessage_Please_select_the_log_category"), UIHelper.GetResouceValueByID("DialogMessage_Tip"), MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            return true;  //throw new NotImplementedException();
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            string visiable;
            string statusID;
            string statusValue;
            try
            {
                visiable = actionParamsList.Single(temp => temp.bindingData.Equals("status_visiable")).value.ToString();
                 statusID=actionParamsList.Single(temp => temp.bindingData.Equals("CSS_STATUS_ID")).value.ToString();
                 statusValue=actionParamsList.Single(temp => temp.bindingData.Equals("CSS_STATUS_VALUE")).value.ToString();
            }
            catch (Exception ex)
            {    //todo: tip operator
                //todo: log here
                return null;
            }


            if (visiable.Equals("显示") || visiable.Equals("未知"))
            {
                //todo: update 01
                visiable = "01";
            }
            else
            {
                //todo :update 00
                visiable = "00";
            }
            int res = BuinessRule.GetInstace().sleMonitor.UpdateDevStatusVisiable(statusID, statusValue, visiable);
            if (res == 0)
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Is_Visible, "0", string.Empty);
                MessageDialog.Show("监控状态设置成功", "提醒", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            else
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Is_Visible, "1", string.Empty);
                //log.Error("cmd" + "[" + cmdMulti + "]" + "Erroe!");
                // Util.DataBase.Rollback();
                MessageDialog.Show("监控状态设置失败", "提醒", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            return new ResultStatus { resultCode = 0, resultData = 0 };

           
            
            //return null;
        }
    }
}
