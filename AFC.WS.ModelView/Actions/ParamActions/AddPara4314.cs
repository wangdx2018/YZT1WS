using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.BR;
using AFC.WS.Model.DB;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    public class AddPara4314:  IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            string controlCode = string.Empty;
            string deviceID = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            string runTime = actionParamsList.Single(temp => temp.bindingData.Equals("action_time")).value.ToString();
            if (string.IsNullOrEmpty(deviceID))
            {
                MessageDialog.Show("请输入设备编码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (deviceID.Length < 8)
            {
                MessageDialog.Show("请输入八位设备编码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(BuinessRule.GetInstace().GetBasiDevInfoById(deviceID).device_id))
            {
                MessageDialog.Show("请输入正确的设备编码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (actionParamsList.Single(temp => temp.bindingData.Equals("control_code")).value == null)
            {
                MessageDialog.Show("请选择控制代码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            else
            {
                controlCode = actionParamsList.Single(temp => temp.bindingData.Equals("control_code")).value.ToString();
            }
            Para4314AutorunTime autoRunTime = BuinessRule.GetInstace().GetPara4314AutorunTime(deviceID, controlCode);
            if (!string.IsNullOrEmpty(autoRunTime.device_id))
            {
                MessageDialog.Show("此设备已存在草稿版", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            Para4314AutorunTime para4314 = new Para4314AutorunTime();
            para4314.device_id = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            para4314.control_code = actionParamsList.Single(temp => temp.bindingData.Equals("control_code")).value.ToString();
            para4314.para_version = "-1";
            string time = actionParamsList.Single(temp => temp.bindingData.Equals("action_time")).value.ToString();
            string[] arrTime = time.Split(':');
            int intTime = arrTime[0].Trim().ToInt32() * 3600 + arrTime[1].Trim().ToInt32() * 60 + arrTime[2].Trim().ToInt32();
            para4314.action_time = intTime;
            int result = DBCommon.Instance.InsertTable(para4314, "para_4314_autorun_time");
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
