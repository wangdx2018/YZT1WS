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
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR.Primission;
    using AFC.WS.Model.Const;
    /// <summary>
    /// 增加操作员界面，采用WS基础组件
    /// </summary>
    public partial class OperatorAdded : UserControlBase
    {
        public OperatorAdded()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 是否是手工设置密码还是系统设置密码
        /// </summary>
        private List<RadioButton> radioButtonList = new List<RadioButton>();

        public override void InitControls()
        {
            List<AFC.WS.UI.Common.QueryCondition> list = this.Tag as List<AFC.WS.UI.Common.QueryCondition>;

            System.Windows.Window window = list.Single(temp => temp.bindingData.Equals("window")).value as System.Windows.Window;

            window.Title = "新增操作人员";
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_addOperator.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
                InitData();
            }
        }

        private void InitData()
        {
            OperationCode.passwordSetMode = "";
            RadioButton rbSystem = this.ic.GetCommonControlByName("rad_system") as RadioButton;
            if (rbSystem != null)
            {
                radioButtonList.Add(rbSystem);
                rbSystem.Click += new RoutedEventHandler(RadioButtonClick);
            }

            RadioButton rbHand = this.ic.GetCommonControlByName("rad_hand") as RadioButton;
            if (rbHand != null)
            {
                rbHand.IsChecked = true;
                radioButtonList.Add(rbHand);
                rbHand.Click += new RoutedEventHandler(RadioButtonClick);
            }
        }

        private void RadioButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                RadioButton rb = sender as RadioButton;
                if (rb.Name.Equals("rad_system"))
                {
                    PasswordExtend txtPwd = this.ic.GetCommonControlByName("btn_current_password") as PasswordExtend;
                    if (txtPwd != null)
                    {
                        OperatorManager pm = new OperatorManager();
                        txtPwd.SetControlValue(pm.GetSysDefaultPassword());
                        OperationCode.passwordSetMode = "00";
                        txtPwd.IsEnabled = false;
                    }
                }
                else
                {
                    PasswordExtend txtPwd = this.ic.GetCommonControlByName("btn_current_password") as PasswordExtend;
                    if (txtPwd != null)
                    {

                        txtPwd.SetControlValue(string.Empty);
                        OperationCode.passwordSetMode = "01";
                        txtPwd.IsEnabled = true;
                    }
                }
            }
        }

    

        public override void UnLoadControls()
        {
            //base.UnLoadControls();
        }
    }
}
