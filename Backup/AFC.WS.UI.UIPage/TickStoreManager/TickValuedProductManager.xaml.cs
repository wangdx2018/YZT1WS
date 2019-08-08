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
using AFC.WS.UI.DataSources;

namespace AFC.WS.UI.UIPage.TickStoreManager
{
    /// <summary>
    /// TickValuedProductManager.xaml 的交互逻辑
    /// </summary>
    public partial class TickValuedProductManager : UserControlBase
    {
        public TickValuedProductManager()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\TickMonyBoxManager\ui_tickValuedProductInfo.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\TickMonyBoxManager\list_tickValuedProductInfo.xml");
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
            DataSourceManager.DisponseDataSource("ds_tick_valued_product_info.xml");
            //base.UnLoadControls();
        }
    }
}
