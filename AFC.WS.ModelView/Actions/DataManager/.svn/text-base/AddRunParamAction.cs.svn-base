using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.ModelView.Actions.DataManager
{
   public class AddRunParamAction:  IAction
    {
        #region IAction 成员
       string paramCode = string.Empty;
       string paramName = string.Empty;
       string paramValue = string.Empty;
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
             paramCode = actionParamsList.Single(temp => temp.bindingData.Equals("param_code")).value.ToString();
             paramName = actionParamsList.Single(temp => temp.bindingData.Equals("param_name")).value.ToString();
             paramValue = actionParamsList.Single(temp => temp.bindingData.Equals("param_value")).value.ToString();
            if (string.IsNullOrEmpty(paramCode))
            {
                MessageDialog.Show("请输入参数编码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(paramName))
            {
                MessageDialog.Show("请输入参数名称", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
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
            paramInfo.param_code = paramCode;
            paramInfo.param_name = paramName;
            paramInfo.param_value = paramValue;
            int result = DBCommon.Instance.InsertTable(paramInfo, "basi_run_param_info");
            if (result != 1)
            {
                MessageDialog.Show("数据库插入失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            else
            {
                MessageDialog.Show("参数增加成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }

            return null;
        }

        #endregion
    }
}
