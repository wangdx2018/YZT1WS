using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.DataSources;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    public class DelPara4044AlarmLamp:IAction
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
            string cardIssuerId = actionParamsList.Single(temp => temp.bindingData.Equals("card_issue_id")).value.ToString();

            string cardIssue = "ACC";
            string delSql;
            if (cardIssuerId.ToUpper().Equals(cardIssue))
            {
                 delSql = string.Format("delete para_4044_custom_alarm_lamp t  where t.card_issuer_id ='{0}' and t.para_version ='-1'", "01");
            }
            else
            {
                 delSql = string.Format("delete para_4044_custom_alarm_lamp t  where t.card_issuer_id ='{0}' and t.para_version ='-1'", "99");
            }
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
            DataSourceManager.NotfiyDataSourceChange("ds_para_4044_custom_alarm_lamp");

            return null;
        }


        #endregion


    }
}
