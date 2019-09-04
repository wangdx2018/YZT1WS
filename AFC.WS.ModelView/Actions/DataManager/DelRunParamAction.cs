using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.DataSources;

namespace AFC.WS.ModelView.Actions.DataManager
{
   public class DelRunParamAction  :IAction
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
           string paramCode = actionParamsList.Single(temp => temp.bindingData.Equals("param_code")).value.ToString();
           string delSql = string.Format("delete from basi_run_param_info   where param_code ='{0}' ", paramCode);
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
           DataSourceManager.NotfiyDataSourceChange("ds_runParamInfo");

           return null;
        }

        #endregion
    }
}
