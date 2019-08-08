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
    class UpdateOperContentAction: IAction
    {
        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择要编辑的备注", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
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
                OperContentLogInfo logInfo = new OperContentLogInfo();
                logInfo.line_id = actionParamsList.Single(temp => temp.bindingData.Equals("line_name")).value.ToString();
                logInfo.station_id = actionParamsList.Single(temp => temp.bindingData.Equals("station_cn_name")).value.ToString();
                logInfo.project_name = actionParamsList.Single(temp => temp.bindingData.Equals("project_name")).value.ToString();
                logInfo.operator_id = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
                logInfo.content = actionParamsList.Single(temp => temp.bindingData.Equals("content")).value.ToString();
                logInfo.content_sn = actionParamsList.Single(temp => temp.bindingData.Equals("content_sn")).value.ToString().ToInt32();


                InteractiveControl ic = new InteractiveControl();
                InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\DataManager\ui_updateOperContent.xml");
                if (icRule != null)
                {

                    InitContentDetails(icRule, logInfo);

                    ic.Initialize(icRule);
             
                }
               
                ShowDetailsDialog.ShowDetails("选中的备注" + logInfo.content_sn.ToString(), ic, 400, 500);
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
        private void InitContentDetails(InteractiveControlRule icRule, OperContentLogInfo logInfo)
        {
            if (icRule == null || logInfo == null)
                return;
            for (int i = 0; i < icRule.ControlList.Count; i++)
            {
                ReBindingInteraciveControlConfig(icRule.ControlList[i], logInfo);
            }
        }
        /// <summary>
        /// 需要将初始数据付给控件信息
        /// </summary>
        /// <param name="controlInfo">控件信息</param>
        /// <param name="operatorInfo">操作员信息</param>
        private void ReBindingInteraciveControlConfig(ControlProperty controlInfo, OperContentLogInfo logInfo)
        {
            try
            {
                if (controlInfo == null || logInfo == null)
                    return;
                switch (controlInfo.BindingField.Split('.')[0].ToUpper())
                {
                    case "LINE_ID":
                        controlInfo.InitValue = logInfo.line_id;
                        return;
                    case "STATION_ID":
                        controlInfo.InitValue = logInfo.station_id;
                        return;
                    case "PROJECT_NAME":
                        controlInfo.InitValue = logInfo.project_name;
                        return;
                    case "CONTENT":
                        controlInfo.InitValue = logInfo.content;
                        return;
                    case "OPERATOR_ID":
                        controlInfo.InitValue = logInfo.operator_id;
                        return;
                    case "CONTENT_SN":
                        controlInfo.InitValue = logInfo.content_sn.ToString();
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
