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
    using AFC.WS.UI.Common;
    /// <summary>
    /// OperatorPrimissionQuery.xaml 的交互逻辑
    /// </summary>
    public partial class OperatorPrimissionQuery : UserControlBase
    {
        public OperatorPrimissionQuery()
        {
            InitializeComponent();
        }

        private string operatorId;

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            Window window = list.Single(temp => temp.bindingData.Equals("window")).value as Window;
            this.operatorId = list.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            window.Title = string.Format("查看操作员{0}  权限", operatorId);

            SetupUserControl(tabBasiInfo, new OperatorBasiInfo());
            SetupUserControl(tabStatusInfo, new OperatorStatusInfo());
            SetupUserControl(tabRole, new OperatorRoles());
            SetupUserControl(tabSation, new OperatorToWorkLocation());
            SetupUserControl(tabDeviceType, new OperatorToDevType());
            SetupUserControl(tabFunction, new OperatorFunctionInfo());
        }

        private void SetupUserControl(TabItem item, UserControlBase ucb)
        {
            item.Content = ucb;
            ucb.Tag = this.Tag;
            ucb.InitControls();
        }

        
    }
}
