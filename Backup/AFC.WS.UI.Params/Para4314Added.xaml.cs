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
using AFC.BOM2.UIController;
using AFC.WS.UI.Config;

namespace AFC.WS.UI.Params
{
    /// <summary>
    /// Para4314Added.xaml 的交互逻辑
    /// </summary>
    public partial class Para4314Added : UserControlBase
    {
        public Para4314Added()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {
            List<AFC.WS.UI.Common.QueryCondition> list = this.Tag as List<AFC.WS.UI.Common.QueryCondition>;

            System.Windows.Window window = list.Single(temp => temp.bindingData.Equals("window")).value as System.Windows.Window;

            window.Title = "新增4314参数";
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Params\ui_addPara4314.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
        }

    }
}
