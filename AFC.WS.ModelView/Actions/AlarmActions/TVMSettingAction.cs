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
    public class TVMSettingAction:IAction
    {


        //private readonly ILog log;

        public TVMSettingAction()
        {
           // log = LogManager.GetLogger("DeviceMonitor");
        }
      

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null ||
                actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择需要修改的状态", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            string status;
            string statusID;
            string statusValue;
            try
            {
                status = actionParamsList.Single(temp => temp.bindingData.Equals("TVM")).value.ToString();
                 statusID=actionParamsList.Single(temp => temp.bindingData.Equals("CSS_STATUS_ID")).value.ToString();
                 statusValue=actionParamsList.Single(temp => temp.bindingData.Equals("CSS_STATUS_VALUE")).value.ToString();
            }
            catch (Exception ex)
            {   //todo: tip operator
                //todo: log here
                return null;
            }


            if (status.Equals("是") || status.Equals("未知"))
            {
                //todo: update 01
                status="01";
            }
            else
            {
                //todo :update 00
                status="00";
            }
            int res = BuinessRule.GetInstace().sleMonitor.UpdateDEVStatus("TVM",statusID, statusValue, status);
            if (res == 0)
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Is_TVM, "0", string.Empty);
                MessageDialog.Show("状态是否应用于TVM设置成功", "提醒", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            else
            {
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Is_TVM, "1", string.Empty);
                //log.Error("cmd" + "[" + cmdMulti + "]" + "Erroe!");
                // Util.DataBase.Rollback();
                MessageDialog.Show("状态是否应用于TVM设置失败", "提醒", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            return new ResultStatus { resultCode = 0, resultData = 0 };

           
            
            //return null;
        }
    }
}
