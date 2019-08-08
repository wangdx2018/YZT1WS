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

namespace AFC.WS.UI.Params
{
    /// <summary>
    /// Para4314Query.xaml 的交互逻辑
    /// </summary>
    public partial class Para4314Query : UserControlBase
    {
        public Para4314Query()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Params\list_para_4314.xml");
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
            DataSourceManager.DisponseDataSource("ds_para_4314");
        }
    }
}
