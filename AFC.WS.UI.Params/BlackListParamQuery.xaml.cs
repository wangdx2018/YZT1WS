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
    using AFC.WS.UI.Common;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.DataSources;
    using AFC.WS.UI.Components;
    /// <summary>
    /// 黑名单查询
    /// </summary>
    public partial class BlackListParamQuery : UserControlBase
    {
        public BlackListParamQuery()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
           
            this.InitliaizeQueryUI(@".\RuleFiles\Params\ui_para_4011_ykt_blacklist.xml", 
                                                  @".\RuleFiles\Params\list_para_4011_ykt_blackList.xml",
                                                  this.uiyktBlackList,
                                                  this.listyktBlackList);



            this.InitliaizeQueryUI(@".\RuleFiles\Params\ui_para_4012_ypt_full_black_list.xml",
                                                 @".\RuleFiles\Params\list_para_4012_ypt_full_black_list.xml",
                                                this.uIyptFulBlackList,
                                                 this.listyptFulBlackList);



            this.InitliaizeQueryUI(@".\RuleFiles\Params\ui_para_4013_ypt_incre_black_list.xml",
                                                 @".\RuleFiles\Params\list_para_4013_ypt_incre_black_list.xml",
                                                 this.uIyptIncBlackList,
                                                 this.listyptIncBlackList);



            this.InitliaizeQueryUI(@".\RuleFiles\Params\ui_para_4014_ypt_range_blacklist.xml",
                                                 @".\RuleFiles\Params\list_para_4014_ypt_range_blacklist.xml",
                                                 this.uiyptSectionBlackList,
                                                 this.listyptSectionBlackList);



            this.InitliaizeQueryUI(@".\RuleFiles\Params\ui_para_4015_staff_blacklist.xml",
                                                 @".\RuleFiles\Params\list_para_4015_staff_blacklist.xml",
                                                 this.uiStaffBlackList,
                                                 this.listStaffBlackList);
            
            //base.InitControls();
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_para_4011_ykt_blackList");
            DataSourceManager.DisponseDataSource("ds_para_4012_ypt_full_black_list");
            DataSourceManager.DisponseDataSource("ds_para_4013_ypt_incre_black_list");
            DataSourceManager.DisponseDataSource("ds_para_4014_ypt_range_blacklist");
            DataSourceManager.DisponseDataSource("ds_para_4015_staff_blacklist");
        }

        /// <summary>
        /// 初始化UI查询界面
        /// </summary>
        /// <param name="uiFileName">查询规则文件名</param>
        /// <param name="listFileName">列表规则文件名</param>
        /// <param name="ic">交互界面控件</param>
        /// <param name="dlc">列表控件</param>
        private void InitliaizeQueryUI(string uiFileName, string listFileName, InteractiveControl ic, DataListControl dlc)
        {
            InteractiveControlRule icRule = Utility.Instance.GetInteractiveControlObject(uiFileName);
            if (icRule != null)
            {
                ic.Initialize(icRule);
            }
            DataListRule dlr = Utility.Instance.GetDataListObject(listFileName);
            if (dlr != null)
            {
                dlc.Initliaize(dlr);
            }
        }
    }
}
