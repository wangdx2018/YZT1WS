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

namespace AFC.WS.UI.Primission
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.DataSources;
    /// <summary>
    /// 负责人：王冬欣  最后修改日期:20100121
    /// 查看该操作员有多少个合法的系统功能信息
    /// </summary>
    public partial class OperatorFunctionInfo : UserControlBase
    {
        public OperatorFunctionInfo()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 从BOM2.0 UserControlBase继承而来<see cref="BOM2.0"/>
        /// 功能为初始化WSUI组件<see cref=" WS2.0基础组件"/>
        /// </summary>
        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            string operatorId = list.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Primission\list_operatorFunction.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }
            if (!string.IsNullOrEmpty(operatorId))
            {
                IDataSource dataSource = DataSourceManager.LookupDataSourceByName("ds_operatorFunctions");
                if (dataSource != null)
                {
                    List<string> paraList = new List<string>();
                    paraList.Add(string.Format("poi.operator_id='{0}'", operatorId));
                    this.groupHeader.Header = "操作员:" + operatorId + " 的全部功能列表";
                    dataSource.SetQueryParams(paraList);
                }
            }
        }
        
    }
}
