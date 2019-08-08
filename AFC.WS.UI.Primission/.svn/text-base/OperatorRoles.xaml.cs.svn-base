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
    /// OperatorRoles.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorRoles : UserControlBase
    {
        public OperatorRoles()
        {
            InitializeComponent();
        }

        private string operatorId;

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            if (list != null)
            {
                this.operatorId = list.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            }
            AFC.WS.UI.Config.DataListRule dlr = Utility.Instance.GetDataListObject(@".\RuleFiles\Primission\list_OperatorroleInfo.xml");
            this.list.Initliaize(dlr);
            IDataSource dataSource = DataSourceManager.LookupDataSourceByName("ds_operatorRoleInfo");
            if (dataSource != null)
            {
                List<string> listQuery= new List<string>();
                listQuery.Add(string.Format("t.operator_id='{0}'", operatorId));
                dataSource.SetQueryParams(listQuery);
            }
           
            
        }

        public override void UnLoadControls()
        {
            DataSourceManager.DisponseDataSource("ds_operatorRoleInfo");
            //base.UnLoadControls();
        }

       
    }
}
