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

namespace TJWS
{
    using AFC.WS.ModelView.UIContext;
    using log4net.Config;
    using System.Globalization;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Components;
    using System.Configuration;
    using AFC.WS.BR;
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            BuinessRule.GetInstace().brConext.JudgeCustomApplicationIsStart();
            UIOperation operation = UIOperation.GetInstance();
            operation.InitliaizeUIOpeation(this);
            operation.SwitchUI("SysStartAndCheck");
            //Button btn = new Button();
            //btn.IsEnabled
        }
        
    }
}
