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

namespace AFC.WS.UI.Primission
{
    /// <summary>
    /// T1RoleMangerment.xaml 的交互逻辑
    /// </summary>
    public partial class T1RoleMangerment : UserControlBase
    {
        public T1RoleMangerment()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Primission\ui_t1_priv_roles.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Primission\dl_t1_priv_roles.xml");
            if (dlr != null)
            {
                //this.list.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.RoleStatusColorSetting());
                this.list.Initliaize(dlr);
            }
        }
        /// <summary>
        /// 重写UserCotrolBase 释放数据源
        /// </summary>
        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_roleInfo");
        }
    }
}
