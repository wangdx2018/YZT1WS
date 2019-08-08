using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Components;
using AFC.WS.UI.Config;
using AFC.WS.Model.DB;
using AFC.WS.BR;
using AFC.WS.ModelView.Convertors;
using AFC.WS.ModelView.Convetors;

namespace AFC.WS.ModelView.Actions.ParamActions
{
   public class UpdatePara4314Action:  IAction
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
                Para4314AutorunTime para4314 = new Para4314AutorunTime();
                para4314.device_id = actionParamsList.Single(temp => temp.bindingData.Equals("device_id")).value.ToString();
                para4314.control_code = actionParamsList.Single(temp => temp.bindingData.Equals("control_code")).value.ToString();
                para4314.para_version = actionParamsList.Single(temp => temp.bindingData.Equals("action_time")).value.ToString();

                InteractiveControl ic = new InteractiveControl();
                InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Params\ui_updatePara4314.xml");
                if (icRule != null)
                {
                   
                    InitAutorunTimeDetails(icRule, para4314);
                    
                    ic.Initialize(icRule);
                }

                ShowDetailsDialog.ShowDetails("选中的设备" + para4314.device_id.ToString(), ic, 400, 500);
                //BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Operator_Action, "0", "操作员修改成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            catch (Exception ex)
            {
                //todo
                //BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Operator_Action, "1", "操作员修改失败");
                return null;
            }
        }

        /// <summary>
        /// 将某个表中的具体的信息显示到交互界面上
        /// </summary>
        /// <param name="icRule">Para4314AutorunTime的实体信息表</param>
        private void InitAutorunTimeDetails(InteractiveControlRule icRule, Para4314AutorunTime autoRunTime)
        {
            if (icRule == null || autoRunTime == null)
                return;
            for (int i = 0; i < icRule.ControlList.Count; i++)
            {
                ReBindingInteraciveControlConfig(icRule.ControlList[i], autoRunTime);
            }
        }


        /// <summary>
        /// 需要将初始数据付给控件信息
        /// </summary>
        /// <param name="controlInfo">控件信息</param>
        /// <param name="operatorInfo">操作员信息</param>
        private void ReBindingInteraciveControlConfig(ControlProperty controlInfo, Para4314AutorunTime autoRunTime)
        {
            try
            {

                ConvertToActionTime timeCovert = new ConvertToActionTime();
                CovertToCtrlCode ctrlCovert = new CovertToCtrlCode();
                if (controlInfo == null || autoRunTime == null)
                    return;
                switch (controlInfo.BindingField.Split('.')[0].ToUpper())
                {
                    case "DEVICE_ID":
                        controlInfo.InitValue = autoRunTime.device_id;
                        return;
                    case "CONTROL_CODE":
                        controlInfo.InitValue = ctrlCovert.Convert(autoRunTime.control_code,null,null,null).ToString();
                        return;
                    case "PARA_VERSION":
                        controlInfo.InitValue = "-1";
                        return;
                    case "ACTION_TIME":
                        controlInfo.InitValue = timeCovert.Convert(autoRunTime.para_version, null, null, null).ToString();
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
