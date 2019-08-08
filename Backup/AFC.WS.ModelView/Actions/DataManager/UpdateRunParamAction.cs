using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;
using AFC.WS.UI.Components;
using AFC.WS.UI.Config;

namespace AFC.WS.ModelView.Actions.DataManager
{
    public class UpdateRunParamAction:  IAction
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
                BasiRunParamInfo runParamInfo = new BasiRunParamInfo();
                runParamInfo.param_code = actionParamsList.Single(temp => temp.bindingData.Equals("param_code")).value.ToString();
                runParamInfo.param_name = actionParamsList.Single(temp => temp.bindingData.Equals("param_name")).value.ToString();
                runParamInfo.param_value = actionParamsList.Single(temp => temp.bindingData.Equals("param_value")).value.ToString();

                InteractiveControl ic = new InteractiveControl();
                InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\DataManager\ui_updateRunParamInfo.xml");
                if (icRule != null)
                {

                    InitRunParamDetails(icRule, runParamInfo);

                    ic.Initialize(icRule);
                }

                ShowDetailsDialog.ShowDetails("选中的参数" + runParamInfo.param_code.ToString(), ic, 400, 500);
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
        /// <param name="icRule">Para4314AutorunTime的实体信息表</param>
        private void InitRunParamDetails(InteractiveControlRule icRule, BasiRunParamInfo runParam)
        {
            if (icRule == null || runParam == null)
                return;
            for (int i = 0; i < icRule.ControlList.Count; i++)
            {
                ReBindingInteraciveControlConfig(icRule.ControlList[i], runParam);
            }
        }
        /// <summary>
        /// 需要将初始数据付给控件信息
        /// </summary>
        /// <param name="controlInfo">控件信息</param>
        /// <param name="operatorInfo">操作员信息</param>
        private void ReBindingInteraciveControlConfig(ControlProperty controlInfo, BasiRunParamInfo runParamInfo)
        {
            try
            {
                if (runParamInfo == null || runParamInfo == null)
                    return;
                switch (controlInfo.BindingField.Split('.')[0].ToUpper())
                {
                    case "PARAM_CODE":
                        controlInfo.InitValue = runParamInfo.param_code;
                        return;
                    case "PARAM_NAME":
                        controlInfo.InitValue = runParamInfo.param_name;
                        return;
                    case "PARAM_VALUE":
                        controlInfo.InitValue = runParamInfo.param_value;
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
