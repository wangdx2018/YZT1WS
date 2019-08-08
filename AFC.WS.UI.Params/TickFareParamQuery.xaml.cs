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

namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
    /// <summary>
    /// TickFareParamQuery.xaml 的交互逻辑
    /// </summary>
    public partial class TickFareParamQuery : UserControlBase
    {
        public TickFareParamQuery()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Params\ui_tickFare.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Params\list_tickFare.xml");
            if (dlr != null)
            {
               // this.list.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.ParamsSwitchResultColorSetting());
                this.list.Initliaize(dlr);
            }
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_tickFare");
        }
    }
}
