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
using AFC.WS.BR;
using AFC.WS.UI.Common;

namespace AFC.WS.UI.UIPage.CashManager
{
    /// <summary>
    /// DataOperatorSettleQuery.xaml 的交互逻辑
    /// </summary>
    public partial class DataOperatorSettleQuery : UserControlBase
    {
        public DataOperatorSettleQuery()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\CashBoxManager\ui_dataOperatorSettlement.xml");
            if (icRule != null)
            {
                this.icControl.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\CashBoxManager\list_dataOperatorSettlement.xml");
            if (dlr != null)
            {
                this.dataList.Initliaize(dlr);
            }
        }
        /// <summary>
        /// 卸载控件
        /// </summary>
        public override void UnLoadControls()
        {
            AFC.WS.UI.DataSources.DataSourceManager.DisponseDataSource("ds_dataOperatorSettlement");
        }
    }
}
