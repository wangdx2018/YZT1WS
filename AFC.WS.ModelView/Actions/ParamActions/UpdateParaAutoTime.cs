using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.ModelView.Actions.ParamActions
{
   public class UpdateParaAutoTime:  IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null && actionParamsList.Count == 0)
                return false;
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            return true;
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            Para4314AutorunTime para4314 = new Para4314AutorunTime();
            para4314.device_id = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            para4314.control_code = actionParamsList.Single(temp => temp.bindingData.Equals("control_code")).value.ToString();
            para4314.para_version = "-1";
            string time = actionParamsList.Single(temp => temp.bindingData.Equals("action_time")).value.ToString();
            string[] arrTime = time.Split(':');
            int intTime = arrTime[0].Trim().ToInt32() * 3600 + arrTime[1].Trim().ToInt32() * 60 + arrTime[2].Trim().ToInt32();
            para4314.action_time = intTime;
            List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
            tempList.Add(new KeyValuePair<string, string>("DEVICE_ID", para4314.device_id));
            tempList.Add(new KeyValuePair<string, string>("CONTROL_CODE", para4314.control_code));
            int res = DBCommon.Instance.UpdateTable(para4314, "para_4314_autorun_time", tempList.ToArray());
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
        }

        #endregion
    }
}
