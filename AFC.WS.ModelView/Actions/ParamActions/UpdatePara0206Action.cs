using AFC.WS.BR;
using AFC.WS.Model.DB;
using AFC.WS.ModelView.Convertors;
using AFC.WS.ModelView.Convetors;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Components;
using AFC.WS.UI.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.ParamActions
{
    class UpdatePara0206Action : IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要编辑的参数", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
            try
            {
                Para0206StationCfgCtrl para0206 = new Para0206StationCfgCtrl();
                para0206.device_id = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
                para0206.device_name = actionParamsList.Single(temp => temp.bindingData.Equals("device_name")).value.ToString();
                para0206.rotate_angle = actionParamsList.Single(temp => temp.bindingData.Equals("rotate_angle")).value.ToString();
                para0206.device_y_pos = actionParamsList.Single(temp => temp.bindingData.Equals("device_y_pos")).value.ToString();
                para0206.device_type = actionParamsList.Single(temp => temp.bindingData.Equals("device_type")).value.ToString();
                para0206.device_x_pos = actionParamsList.Single(temp => temp.bindingData.Equals("device_x_pos")).value.ToString();
                para0206.ip_address = actionParamsList.Single(temp => temp.bindingData.Equals("ip_address")).value.ToString();
                para0206.service_port = actionParamsList.Single(temp => temp.bindingData.Equals("service_port")).value.ToString();

                InteractiveControl ic = new InteractiveControl();
                InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Params\ui_updatePara0206.xml");
                if (icRule != null)
                {

                    InitAutorunTimeDetails(icRule, para0206);

                    ic.Initialize(icRule);
                }

                ShowDetailsDialog.ShowDetails("选中的设备" + para0206.device_id.ToString(), ic, 600, 650);
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 将某个表中的具体的信息显示到交互界面上
        /// </summary>
        /// <param name="icRule">Para0206StationCfgCtrl的实体信息表</param>
        private void InitAutorunTimeDetails(InteractiveControlRule icRule, Para0206StationCfgCtrl StationCfgTime)
        {
            if (icRule == null || StationCfgTime == null)
                return;
            for (int i = 0; i < icRule.ControlList.Count; i++)
            {
                ReBindingInteraciveControlConfig(icRule.ControlList[i], StationCfgTime);
            }
        }


        /// <summary>
        /// 需要将初始数据付给控件信息
        /// </summary>
        /// <param name="controlInfo">控件信息</param>
        /// <param name="operatorInfo">操作员信息</param>
        private void ReBindingInteraciveControlConfig(ControlProperty controlInfo, Para0206StationCfgCtrl StationCfgTime)
        {
            try
            {

                ConvertToActionTime timeCovert = new ConvertToActionTime();
                CovertToCtrlCode ctrlCovert = new CovertToCtrlCode();
                if (controlInfo == null || StationCfgTime == null)
                    return;
                switch (controlInfo.BindingField.Split('.')[0].ToUpper())
                {
                    case "DEVICE_ID":
                        controlInfo.InitValue = StationCfgTime.device_id;
                        return;
                    case "DEVICE_NAME":
                        controlInfo.InitValue = StationCfgTime.device_name;
                        return;
                    case "DEVICE_TYPE":
                        controlInfo.InitValue = StationCfgTime.device_type;
                        return;
                    case "DEVICE_X_POS":
                        controlInfo.InitValue = StationCfgTime.device_x_pos;
                        return;
                    case "DEVICE_Y_POS":
                        controlInfo.InitValue = StationCfgTime.device_y_pos;
                        return;
                    case "IP_ADDRESS":
                        controlInfo.InitValue = StationCfgTime.ip_address;
                        return;
                    case "ROTATE_ANGLE":
                        controlInfo.InitValue = StationCfgTime.rotate_angle;
                        return;
                    case "SERVICE_PORT":
                        controlInfo.InitValue = StationCfgTime.service_port;
                        return;
                }
            }
            catch (Exception ex)
            {
                //todo
            }
        }


        #endregion
    }
}
