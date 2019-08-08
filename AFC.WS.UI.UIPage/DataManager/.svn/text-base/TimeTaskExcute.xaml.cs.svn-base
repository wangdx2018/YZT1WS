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

namespace AFC.WS.UI.UIPage.DataManager
{
    /// <summary>
    /// TimeTaskExcute.xaml 的交互逻辑
    /// </summary>
    public partial class TimeTaskExcute : UserControlBase
    {
        public TimeTaskExcute()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\DataManager\ui_timeTaskExcute.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\DataManager\list_timeTaskExcute.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }
            if (dlr != null)
            {
                this.list.SetGridRowColor(new AFC.WS.ModelView.ColorSettiing.TimeTaskExcuteColorSetting());                
                this.list.Initliaize(dlr);
            }
        }


        /// <summary>
        /// 卸载控件
        /// </summary>
        public override void UnLoadControls()
        {
            AFC.WS.UI.DataSources.DataSourceManager.DisponseDataSource("ds_timeTaskExcute");
        }
    }
}
