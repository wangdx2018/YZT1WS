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

    /// <summary>
    /// 作者：王冬欣 
    /// 日期：20110223
    /// 代码功能：登录用户控件。
    /// 修订记录：
    /// </summary>
    public partial class Login : UserControlBase
    {
        public Login()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_Login.xml");
            if (icRule != null)
            {
                try
                {
                    this.operatorLogIn.Initialize(icRule);
                    //UIElement element = this.operatorLogIn.GetCommonControlByName("btnLogIn");
                    //if (element != null)
                    //{
                    //    Button btn = (element as Button);
                    //    if (btn != null)
                    //    {
                    //        btn.IsDefault = true;
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                }
            }
        }
    }
}
