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
    using AFC.WS.BR;
    /// <summary>
    /// OperatorChangePwd.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorChangePwd : UserControlBase
    {
        public OperatorChangePwd()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_operatorChangePwd.xml");
            if (icRule != null)
            {
                try
                {
                    ControlProperty cp = icRule.ControlList.Single(temp => temp.ControlName.Equals("txtOperatorCode"));
                    if (cp != null)
                    {
                        cp.InitValue = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                        this.operatorPwdChanged.Initialize(icRule);
                    }
                }
                catch (Exception ex)
                {
                    AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                }
            }
        }

    }
}
