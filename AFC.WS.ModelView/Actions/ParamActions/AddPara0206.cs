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
    using AFC.WS.BR.ParamsManager;
    class AddPara0206: IAction
    {

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            string deviceID = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            string deviceType = actionParamsList.Single(temp => temp.bindingData.Equals("device_type")).value.ToString();
            string deviceName = actionParamsList.Single(temp => temp.bindingData.Equals("device_name")).value.ToString();
            string deviceXpos = actionParamsList.Single(temp => temp.bindingData.Equals("device_x_pos")).value.ToString();
            string deviceYpos = actionParamsList.Single(temp => temp.bindingData.Equals("device_y_pos")).value.ToString();
            string rotateAngle = actionParamsList.Single(temp => temp.bindingData.Equals("rotate_angle")).value.ToString();
            string ipAddress = actionParamsList.Single(temp => temp.bindingData.Equals("ip_address")).value.ToString();
            string servicePort = actionParamsList.Single(temp => temp.bindingData.Equals("service_port")).value.ToString();
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
            if (deviceType.Length < 2)
            {
                MessageDialog.Show("请输入二位设备类型", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            Para0206StationCfgCtrl StaionCfg = BuinessRule.GetInstace().GetPara0206StationCfgCtrl(deviceID);
            if (!string.IsNullOrEmpty(StaionCfg.device_id))
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
            Para0206StationCfgCtrl para0206 = new Para0206StationCfgCtrl();
            para0206.device_id = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            para0206.device_name = actionParamsList.Single(temp => temp.bindingData.Equals("device_name")).value.ToString();
            para0206.device_type = actionParamsList.Single(temp => temp.bindingData.Equals("device_type")).value.ToString();
            para0206.device_x_pos = actionParamsList.Single(temp => temp.bindingData.Equals("device_x_pos")).value.ToString();
            para0206.device_y_pos = actionParamsList.Single(temp => temp.bindingData.Equals("device_y_pos")).value.ToString();
            para0206.ip_address = actionParamsList.Single(temp => temp.bindingData.Equals("ip_address")).value.ToString();
            para0206.rotate_angle = actionParamsList.Single(temp => temp.bindingData.Equals("rotate_angle")).value.ToString();
            para0206.service_port = actionParamsList.Single(temp => temp.bindingData.Equals("service_port")).value.ToString();
            ParaManager pa = BuinessRule.GetInstace().paraManager;
            para0206.para_version =  "0000";
            int result = DBCommon.Instance.InsertTable(para0206, "para_0206_station_cfg_ctrl");
            if (result != 1)
            {
                MessageDialog.Show("数据库插入失败", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }
            else
            {
                MessageDialog.Show("参数增加成功", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }

            return null;
        }

        #endregion
    }
}
