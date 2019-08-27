
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Actions.PrimissionActions
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.Config;
   
    using AFC.WS.UI.Components;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model.DB;
    
    //--->操作员基本信息查询，操作员状态信息查询调用的Action
    /// <summary>
    /// </summary>
    /// <remarks>
    /// WS2.0基础组件中调用的此Action处理类，能够查询操作员的基本信息
    /// </remarks>
    public class BasicOperatorInfoQueryAction:IAction
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
            int count = actionParamsList.Count<QueryCondition>(temp => temp.bindingData.Equals("operator_id"));
            if (count > 1)
            {
                MessageDialog.Show("您只能选择一个操作员", "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok) ;
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
                QueryCondition qc=actionParamsList.Single(temp => temp.bindingData.Equals("operator_id"));
                OperatorManager pm = new OperatorManager();
                if (qc != null)
                {
                    string operatorId = qc.value.ToString();

                    PrivOperatorInfo operatorInfo = pm.GetOperatorInfoByOperatorId(operatorId);
                    if (operatorInfo != null)
                    {
                        InteractiveControlRule icRule=null;
                        if (this.Status == QueryStatus.BASIC_INFO)
                        {
                            icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_basicOperatorInfo.xml");
                        }
                        else
                        {
                            icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_operatorStatusDetail.xml");
                        }
                        if (icRule != null)
                        {
                            for (int i = 0; i < icRule.ControlList.Count; i++)
                            {
                                ReBindingInteraciveControlConfig(icRule.ControlList[i], operatorInfo);
                            }
                            InteractiveControl ic = new InteractiveControl();
                            ic.Initialize(icRule);
                            double width = Status == QueryStatus.BASIC_INFO ? 400:  800;
                            string tip=Status==QueryStatus.BASIC_INFO?"操作员基本信息":"操作员状态信息";
                            ShowDetailsDialog.ShowDetails(tip, ic, 300, width);
                        }
                    }
                    
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
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

                AFC.WS.ModelView.Convertors.OperatorCanMulLogInConvert canmulLogInConvert = new AFC.WS.ModelView.Convertors.OperatorCanMulLogInConvert();

                AFC.WS.ModelView.Convertors.OperatorStatusConvert currentStatusConvert = new AFC.WS.ModelView.Convertors.OperatorStatusConvert();

                AFC.WS.ModelView.Convertors.OperatorLogInConvert logInConvert = new AFC.WS.ModelView.Convertors.OperatorLogInConvert();

                AFC.WS.ModelView.Convertors.OperatorLockStatusConvert lockStatus = new AFC.WS.ModelView.Convertors.OperatorLockStatusConvert();
                AFC.WS.ModelView.Convertors.PasswordSetModeConvert password = new AFC.WS.ModelView.Convertors.PasswordSetModeConvert();

                if (controlInfo == null || operatorInfo == null)
                    return;
                switch (controlInfo.BindingField.ToUpper())
                {
                    case "COMPANY_ID":
                        AFC.WS.ModelView.Convertors.StationCovert convertLocation = new AFC.WS.ModelView.Convertors.StationCovert();
                        controlInfo.InitValue = convertLocation.Convert(operatorInfo.company_id, null, null, null).ToString();
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
                    case "UPD_OPERATOR_ID":
                        controlInfo.InitValue = operatorInfo.upd_operator_id;
                        return;
                    case "PASSWORD":
                        controlInfo.InitValue = operatorInfo.password;
                        return;
                    case "VALIDITY_DATE_START":
                        controlInfo.InitValue = convert.Convert(operatorInfo.validity_date_start, null, null, null).ToString();
                        return;
                    case "VALIDITY_DATE_END":
                        controlInfo.InitValue = convert.Convert(operatorInfo.validity_date_end, null, null, null).ToString();
                        return;
                    case "VALIDITY_DATE_FIRST_LOGIN":
                        controlInfo.InitValue = convert.Convert(operatorInfo.validity_date_first_login, null, null, null).ToString();
                        return;
                    case "PWD_INVALIDITY_DATE":
                        controlInfo.InitValue = convert.Convert(operatorInfo.pwd_invalidity_date, null, null, null).ToString();
                        return;
                    case "IS_MULTI_LOGIN":
                        controlInfo.InitValue = canmulLogInConvert.Convert(operatorInfo.is_multi_login, null, null, null).ToString();
                        return;
                    case "OPERATOR_STATUS":
                        controlInfo.InitValue = currentStatusConvert.Convert(operatorInfo.operator_status, null, null, null).ToString();
                        return;
                    //case "LOGIN_STATUS":
                    //    controlInfo.InitValue = logInConvert.Convert(operatorInfo.login_status, null, null, null).ToString();
                    //    return;
                    //case "LOCK_STATUS":
                    //    controlInfo.InitValue = lockStatus.Convert(operatorInfo.lock_status, null, null, null).ToString();
                    //    return;
                    case "OPERATOR_NAME":
                        controlInfo.InitValue = operatorInfo.operator_name;
                        return;
                    case "CONTACT_INFO1":
                        controlInfo.InitValue = operatorInfo.contact_info1;
                        return;
                    case "CONTACT_INFO2":
                        controlInfo.InitValue = operatorInfo.contact_info2;
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
                    case "PASSWORD_HIS1":
                        controlInfo.InitValue = operatorInfo.password_his1;
                        return;
                    case "PASSWORD_HIS2":
                        controlInfo.InitValue = operatorInfo.password_his2;
                        return;
                    case "PWD_SET_MODE":
                        controlInfo.InitValue = password.Convert(operatorInfo.pwd_set_mode, null, null, null).ToString();
                        return;
                }
            }
            catch (Exception ex)
            {
                //todo
            }
        }

        [Filter()]
        public QueryStatus Status
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// 枚举状态信息
    /// </summary>
    public enum QueryStatus  :byte
    {
        /// <summary>
        /// 基础信息
        /// </summary>
        BASIC_INFO=0,

        /// <summary>
        /// 状态信息
        /// </summary>
        STATUS_INFO=1
    }
}
