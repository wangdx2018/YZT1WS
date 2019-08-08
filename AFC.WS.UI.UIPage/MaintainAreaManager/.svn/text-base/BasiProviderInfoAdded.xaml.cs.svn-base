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

namespace AFC.WS.UI.UIPage.MaintainAreaManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.BR;
    using AFC.WS.UI.Config;
    using AFC.WS.Model.DB;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.Common;
    /// <summary>
    /// TiketTypeAdded.xaml 的交互逻辑
    /// </summary>
    public partial class BasiProviderInfoAdded : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();

        public BasiProviderInfoAdded()
        {
            InitializeComponent();
        }      

        private void btnAddProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "ProviderID", this.ProviderID.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "ProviderName", this.ProviderName.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "ProviderAddress", this.ProviderAddress.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "ProviderContector", this.ProviderContector.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "ProviderPhone", this.ProviderPhone.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.MaintainAreaManager.BasiProviderInfoAdd();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.ProviderID.Text = string.Empty;
            this.ProviderName.Text = string.Empty;
            this.ProviderAddress.Text = string.Empty;
            this.ProviderContector.Text = string.Empty;
            this.ProviderPhone.Text = string.Empty;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
