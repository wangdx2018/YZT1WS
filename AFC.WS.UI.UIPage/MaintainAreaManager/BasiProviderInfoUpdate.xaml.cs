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
    public partial class BasiProviderInfoUpdate : UserControlBase
    {
        private List<QueryCondition> list1 = new List<QueryCondition>();

        public BasiProviderInfoUpdate()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            //Window window = list.Single(temp => temp.bindingData.Equals("window")).value as Window;
            this.ProviderID.Text = list.Single(temp => temp.bindingData.Equals("provider_id")).value.ToString();
            this.ProviderName.Text = list.Single(temp => temp.bindingData.Equals("mc_dep_name")).value.ToString();
            this.ProviderAddress.Text = list.Single(temp => temp.bindingData.Equals("provider_address")).value.ToString();
            this.ProviderContector.Text = list.Single(temp => temp.bindingData.Equals("contract_person")).value.ToString();
            this.ProviderPhone.Text = list.Single(temp => temp.bindingData.Equals("phone")).value.ToString();
            
        }

        private void btnUpdateProvider_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list1, "ProviderID", this.ProviderID.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "ProviderName", this.ProviderName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "ProviderAddress", this.ProviderAddress.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "ProviderContector", this.ProviderContector.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "ProviderPhone", this.ProviderPhone.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.MaintainAreaManager.BasiProviderInfoUpdate();

            //dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list1);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
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
