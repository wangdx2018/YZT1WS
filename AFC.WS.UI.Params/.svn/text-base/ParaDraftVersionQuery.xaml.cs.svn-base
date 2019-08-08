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


namespace AFC.WS.UI.Params
{
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Components;
    using AFC.WS.UI.DataSources;
    using AFC.WS.UI.CommonControls;
    /// <summary>
    /// ParaDraftVersionQuery.xaml 的交互逻辑
    /// </summary>
    public partial class ParaDraftVersionQuery : UserControlBase
    {
        public ParaDraftVersionQuery()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {

            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Params\ui_param_version.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Params\list_param_version.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }
        }
        /// <summary>
        /// 重写UserCotrolBase 释放数据源
        /// </summary>
        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_para_version_info");
        }


    }
}
