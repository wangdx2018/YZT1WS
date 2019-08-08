using System;
using System.Collections.Generic;
using System.Linq;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Components;
    using AFC.WS.UI.Config;
    using AFC.WS.Model.DB;
    using AFC.WS.BR.Primission;
    using AFC.WS.ModelView.Convertors;
    using System.Windows;
    using AFC.WS.Model.Const;
    using AFC.WS.BR;
    ///<summary>
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，更新操作员信息
    /// </remarks>
    public class UpdateOperatorAction: IAction
    {
        #region IAction 成员
        /// <summary>
        /// 检查UI传递过来的数据是否合法
        /// </summary>
        /// <param name="actionParamsList">传递来的数据</param>
        /// <returns>合法返回true，否则返回false</returns>
        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            if (actionParamsList == null || actionParamsList.Count == 0)
            {
                MessageDialog.Show("请选择操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 检查该操作员是否有此权限
        /// </summary>
        /// <param name="authInfo">操作员权限信息</param>
        /// <returns>有权限返回true，否则返回false</returns>
        public bool CheckPremission(object authInfo)
        {
            return true;
        }
        /// <summary>
        /// 检查合法后执行Action
        /// </summary>
        /// <param name="actionParamsList">从UI层中传递过来的数据</param>
        /// <returns>成功返回ResultStatus对象，否则返回null</returns>
        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            try
            {
                QueryCondition qc = actionParamsList.Single(temp => temp.bindingData.Equals("operator_id"));
                string operatorId = qc.value.ToString();
                OperatorManager operatorManager = new OperatorManager();
                PrivOperatorInfo operatorInfo = operatorManager.GetOperatorInfoByOperatorId(operatorId);
                InteractiveControl ic = new InteractiveControl();
                InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_updateOperatorInfo.xml");
                if (icRule != null)
                {
                    if (operatorInfo != null)
                    {
                        InitOperatorDetails(icRule, operatorInfo);
                    }
                    ic.Initialize(icRule);
                }
          
                ShowDetailsDialog.ShowDetails("选中的操作员编码" + operatorInfo.operator_id.ToString(), ic, 500, 600);
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Operator_Action, "0", "操作员修改成功");
                return new ResultStatus { resultCode = 0, resultData = 0 };
            }
            catch (Exception ex)
            {
                //todo
                BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Update_Operator_Action, "1", "操作员修改失败");
                return null;
            }
        }

        /// <summary>
        /// 将某个表中的具体的日志信息显示到交互界面上
        /// </summary>
        /// <param name="logInfo">Log的实体信息表</param>
        private void InitOperatorDetails(InteractiveControlRule icRule, PrivOperatorInfo operatorDetails)
        {
            if (icRule == null || operatorDetails == null)
                return;
            for (int i = 0; i < icRule.ControlList.Count; i++)
            {
                ReBindingInteraciveControlConfig(icRule.ControlList[i], operatorDetails);
            }
        }


        /// <summary>
        /// 需要将初始数据付给控件信息
        /// </summary>
        /// <param name="controlInfo">控件信息</param>
        /// <param name="operatorInfo">操作员信息</param>
        private void ReBindingInteraciveControlConfig(ControlProperty controlInfo, PrivOperatorInfo operatorInfo)
        {
            try
            {
                AFC.WS.UI.Convertors.DateTimeConvert convert = new AFC.WS.UI.Convertors.DateTimeConvert();
                convert.DateTimeFormat = "yyyy年MM月dd日";
               OperatorCanMulLogInConvert canmulLogInConvert = new OperatorCanMulLogInConvert();

                OperatorStatusConvert currentStatusConvert = new OperatorStatusConvert();

                OperatorLogInConvert logInConvert = new OperatorLogInConvert();

                OperatorLockStatusConvert lockStatus = new OperatorLockStatusConvert();
                PasswordSetModeConvert passwordSet = new PasswordSetModeConvert();

                if (controlInfo == null || operatorInfo == null)
                    return;
                switch (controlInfo.BindingField.Split('.')[1].ToUpper())
                {
                    case "COMPANY_NAME":
                        AFC.WS.ModelView.Convertors.StationCovert convertLocation = new AFC.WS.ModelView.Convertors.StationCovert();
                        controlInfo.InitValue = convertLocation.Convert(operatorInfo.company_name, null, null, null).ToString();
                        return;
                    case "OPERATOR_ID":
                        controlInfo.InitValue = operatorInfo.operator_id;
                        return;
                    case "UPDATE_DATE":
                        controlInfo.InitValue = convert.Convert(operatorInfo.update_date, null, null, null).ToString();
                        return;
                    case "UPDATE_TIME":
                        controlInfo.InitValue = operatorInfo.update_time;
                        return;
                    case "UPDATING_OPERATOR_ID":
                        controlInfo.InitValue = operatorInfo.updating_operator_id;
                        return;
                    case "CURRENT_PASSWORD":
                        controlInfo.InitValue = operatorInfo.current_password;
                        return;
                    case "VALIDITY_START_DATE":
                        controlInfo.InitValue = convert.Convert(operatorInfo.validity_start_date, null, null, null).ToString();
                        return;
                    case "VALIDITY_END_DATE":
                        controlInfo.InitValue = convert.Convert(operatorInfo.validity_end_date, null, null, null).ToString();
                        return;
                    case "VALIDITY_FIRST_LOGIN_DATE":
                        controlInfo.InitValue = convert.Convert(operatorInfo.validity_first_login_date, null, null, null).ToString();
                        return;
                    case "PASSWORD_INVALIDITY_DATE":
                        controlInfo.InitValue = convert.Convert(operatorInfo.password_invalidity_date, null, null, null).ToString();
                        return;
                    case "IS_MULTYLY_LOGIN":
                        controlInfo.InitValue = canmulLogInConvert.Convert(operatorInfo.is_multyly_login, null, null, null).ToString();
                        return;
                    case "VALIDITY_STATUS":
                        controlInfo.InitValue = currentStatusConvert.Convert(operatorInfo.validity_status, null, null, null).ToString();
                        return;
                    case "LOGIN_STATUS":
                        controlInfo.InitValue = logInConvert.Convert(operatorInfo.lock_status, null, null, null).ToString();
                        return;
                    case "LOCK_STATUS":
                        controlInfo.InitValue = lockStatus.Convert(operatorInfo.lock_status, null, null, null).ToString();
                        return;
                    case "OPERATOR_NAME":
                        controlInfo.InitValue = operatorInfo.operator_name;
                        return;
                    case "CONTACT_INFO_ONE":
                        controlInfo.InitValue = operatorInfo.contact_info_one;
                        return;
                    case "CONTACT_INFO_TWO":
                        controlInfo.InitValue = operatorInfo.contact_info_two;
                        return;
                    case "CONTACT_ADDRESS":
                        controlInfo.InitValue = operatorInfo.contact_address;
                        return;
                    case "EMAIL_ADDRESS":
                        controlInfo.InitValue = operatorInfo.email_address;
                        return;
                    case "OPERATOR_DISPLAY_ID":
                        controlInfo.InitValue = operatorInfo.operator_display_id;
                        return;
                    case "HISTORY_PASSWORD_ONE":
                        controlInfo.InitValue = operatorInfo.history_password_one;
                        return;
                    case "HISTORY_PASSWORD_TWO":
                        controlInfo.InitValue = operatorInfo.history_password_two;
                        return;
                    case "PASS_SET_MODE_FLAG":
                        controlInfo.InitValue = passwordSet.Convert(operatorInfo.pass_set_mode_flag,null,null,null).ToString();
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
