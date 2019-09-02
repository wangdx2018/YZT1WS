using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    class UpdateParaStationCfg : IAction
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
            Para0206StationCfgCtrl para0206 = new Para0206StationCfgCtrl();
            para0206.device_id = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
            para0206.device_type = actionParamsList.Single(temp => temp.bindingData.Equals("device_type")).value.ToString();
            para0206.device_name = actionParamsList.Single(temp => temp.bindingData.Equals("device_name")).value.ToString();
            para0206.device_x_pos = actionParamsList.Single(temp => temp.bindingData.Equals("device_x_pos")).value.ToString();
            para0206.device_y_pos = actionParamsList.Single(temp => temp.bindingData.Equals("device_y_pos")).value.ToString();
            para0206.rotate_angle = actionParamsList.Single(temp => temp.bindingData.Equals("rotate_angle")).value.ToString();
            para0206.ip_address = actionParamsList.Single(temp => temp.bindingData.Equals("ip_address")).value.ToString();
            para0206.service_port = actionParamsList.Single(temp => temp.bindingData.Equals("service_port")).value.ToString();
            para0206.para_version = "0000";
           
            List<KeyValuePair<string, string>> tempList = new List<KeyValuePair<string, string>>();
            tempList.Add(new KeyValuePair<string, string>("DEVICE_ID", para0206.device_id));            
            tempList.Add(new KeyValuePair<string, string>("PARA_VERSION", para0206.para_version));
            int res = DBCommon.Instance.UpdateTable(para0206, "para_0206_station_cfg_ctrl", tempList.ToArray());
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
