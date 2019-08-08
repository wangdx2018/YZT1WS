using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.DataSources;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    public class DelPara4314Action:  IAction
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
           string  deviceId = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
           string controlCode = actionParamsList.Single(temp => temp.bindingData.Equals("control_code")).value.ToString();
           string delSql = string.Format("delete para_4314_autorun_time t  where t.device_id ='{0}' and t.control_code='{1}' and t.para_version ='-1'", deviceId, controlCode);
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
           DataSourceManager.NotfiyDataSourceChange("ds_para_4314");

           return null;
        }
        

        #endregion
    }
}
