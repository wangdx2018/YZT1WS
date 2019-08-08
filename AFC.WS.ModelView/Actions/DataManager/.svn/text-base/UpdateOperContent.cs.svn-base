using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;

namespace AFC.WS.ModelView.Actions.DataManager
{
    public class UpdateOperContent : IAction
    {
        #region IAction 成员

       string projectName = string.Empty;
       string content     = string.Empty;

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            projectName = actionParamsList.Single(temp => temp.bindingData.Equals("project_name")).value.ToString();
            content = actionParamsList.Single(temp => temp.bindingData.Equals("content")).value.ToString();
            if (string.IsNullOrEmpty(projectName))
            {
                MessageDialog.Show("请输入项目名称", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
              if (string.IsNullOrEmpty(content))
            {
                MessageDialog.Show("请输入备注内容", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            OperContentLogInfo logInfo = new OperContentLogInfo();
            logInfo.line_id = actionParamsList.Single(temp => temp.bindingData.Equals("line_id")).value.ToString();
            logInfo.station_id = actionParamsList.Single(temp => temp.bindingData.Equals("station_id")).value.ToString();
            logInfo.operator_id = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            logInfo.content_sn = actionParamsList.Single(temp => temp.bindingData.Equals("content_sn")).value.ToString().ToInt32(); ;
            logInfo.project_name =projectName;
            logInfo.content = content;
            logInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
            logInfo.update_time = DateTime.Now.ToString("HHmmss");
            List<KeyValuePair<string, string>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add( new KeyValuePair<string, string>("CONTENT_SN",logInfo.content_sn.ToString()));
            paraList.Add( new KeyValuePair<string, string>("LINE_ID",logInfo.line_id));
            paraList.Add( new KeyValuePair<string, string>("STATION_ID",logInfo.station_id));

            int res = DBCommon.Instance.UpdateTable(logInfo, "oper_content_log_info", paraList.ToArray());
            if (res != 1)
            {
                MessageDialog.Show("数据库更新失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
            }
            else
            {
                MessageDialog.Show("备注更新成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
            }
            return null;
        }

        #endregion
    }
}