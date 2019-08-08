using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.DataSources;

namespace AFC.WS.ModelView.Actions.DataManager
{
    public class DelOperContentAction: IAction
    {
        #region IAction 成员
        public bool CheckValid(List<QueryCondition> actionParamsList)
       {
           if (actionParamsList == null || actionParamsList.Count == 0)
           {
               MessageDialog.Show("请选择要删除的备注", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
           string sn = actionParamsList.Single(temp => temp.bindingData.Equals("content_sn")).value.ToString();
           string delSql = string.Format("delete oper_content_log_info t  where t.content_sn ='{0}'", sn);
           try
           {
               int res = 0;
               Util.DataBase.SqlCommand(out res, delSql);
               MessageDialog.Show("删除备注成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
           }
           catch (Exception ex)
           {
               AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
               MessageDialog.Show("删除备注失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
           }
           DataSourceManager.NotfiyDataSourceChange("ds_operContentQuery");

           return null;
        }

        #endregion
    }
}