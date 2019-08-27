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
    /// OperatorBasiInfo.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorBasiInfo : UserControlBase
    {
        public OperatorBasiInfo()
        {
            InitializeComponent();
        }

        private string operatorId;

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            this.operatorId = list.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            PrivOperatorInfo operatorInfo = BuinessRule.GetInstace().operationManager.GetOperatorInfoByOperatorId(operatorId);
            InteractiveControlRule icRule = null;

            icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_basicOperatorInfo.xml");

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
                //todo:
            }
        }
    }
}
