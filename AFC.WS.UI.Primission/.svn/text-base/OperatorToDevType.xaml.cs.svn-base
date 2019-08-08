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
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using AFC.WS.UI.DataSources;

namespace AFC.WS.UI.Primission
{
    /// <summary>
    /// OperatorToDevType.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorToDevType : UserControlBase
    {
        private string operatorId;
        public OperatorToDevType()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            operatorId = list.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Primission\list_operatorDevInfo.xml");
            if (dlr != null)
            {
                this.list.Initliaize(dlr);
            }

            if (!string.IsNullOrEmpty(operatorId))
            {
                IDataSource dataSource = DataSourceManager.LookupDataSourceByName("ds_operatorDevInfo");
                if (dataSource != null)
                {
                    List<string> paraList = new List<string>();
                    paraList.Add(string.Format("p.operator_id='{0}'", operatorId));
                    this.groupHeader.Header = "操作员:" + operatorId + " 的全部可用设备列表";
                    dataSource.SetQueryParams(paraList);
                }
            }
        }
    }
}
