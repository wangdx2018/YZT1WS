using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;

namespace AFC.WS.ModelView.Actions.DataManager
{
   public class UpdateRunParamInfo: IAction
    {
        #region IAction 成员

       string paramValue = string.Empty;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            paramValue = actionParamsList.Single(temp => temp.bindingData.Equals("param_value")).value.ToString();
            if (string.IsNullOrEmpty(paramValue))
            {
                MessageDialog.Show("请输入参数值", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            BasiRunParamInfo paramInfo = new BasiRunParamInfo();
            paramInfo.param_code = actionParamsList.Single(temp => temp.bindingData.Equals("param_code")).value.ToString();
            paramInfo.param_name = actionParamsList.Single(temp => temp.bindingData.Equals("param_name")).value.ToString();
            paramInfo.param_value = paramValue;
            int res = DBCommon.Instance.UpdateTable(paramInfo, "basi_run_param_info", new KeyValuePair<string, string>("PARAM_CODE", paramInfo.param_code));
            if (res != 1)
            {
                MessageDialog.Show("数据库更新失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
            }
            else
            {
                MessageDialog.Show("参数更新成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
            }
            return null;
        }

        #endregion
    }
}
