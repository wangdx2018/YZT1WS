using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AFC.WS.UI.Primission
{
    using AFC.WS.UI.Common;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.UI.Components;
    using AFC.WS.UI.Config;
    using AFC.BOM2.UIController;
    /// <summary>
    /// OperatorStatusInfo.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorStatusInfo : UserControlBase
    {
        public OperatorStatusInfo()
        {
            InitializeComponent();
        }

        private string operatorId;

        public override void InitControls()
        {
                List<QueryCondition> list = this.Tag as List<QueryCondition>;
                this.operatorId = list.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
                PrivOperatorInfo operatorInfo =BuinessRule.GetInstace().operationManager.GetOperatorInfoByOperatorId(operatorId);
                InteractiveControlRule icRule=null;

                icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_operatorStatusDetail.xml");

                if (icRule != null)
                {
                    for (int i = 0; i < icRule.ControlList.Count; i++)
                    {
                        ReBindingInteraciveControlConfig(icRule.ControlList[i], operatorInfo);
                    }
                    InteractiveControl ic = new InteractiveControl();
                    ic.Initialize(icRule);

                    this.Content = ic;
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
                        controlInfo.InitValue = logInConvert.Convert(operatorInfo.login_status, null, null, null).ToString();
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
                        controlInfo.InitValue = password.Convert(operatorInfo.pass_set_mode_flag, null, null, null).ToString();
                        return;
                }
            }
            catch (Exception ex)
            {
                //todo:
            }
        }
    }
}
