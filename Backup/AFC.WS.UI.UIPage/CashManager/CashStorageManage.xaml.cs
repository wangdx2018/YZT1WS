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

namespace AFC.WS.UI.UIPage.CashManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
   
    public partial class CashStorageManage : UserControlBase
    {
        public CashStorageManage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 从BOM2.0 UserControlBase继承而来<see cref="BOM2.0"/>
        /// 功能为初始化WSUI组件<see cref=" WS2.0基础组件"/>
        /// </summary>
        public override void InitControls()
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(@".\RuleFiles\Mode\ui_basi_money_type_info.xml");
            if (icRule != null)
            {
                this.ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Mode\dl_basi_money_type_info.xml");
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
            DataSourceManager.DisponseDataSource("ds_basi_money_type_info.xml");
            //base.UnLoadControls();
        }
    }
}
