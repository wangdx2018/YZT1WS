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
    using AFC.WS.BR;
    using AFC.WS.UI.Config;
    using AFC.WS.Model.DB;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.Common;
    /// <summary>
    /// TiketTypeAdded.xaml 的交互逻辑
    /// </summary>
    public partial class CashTypeNameUpdate : UserControlBase
    {
        private List<QueryCondition> list1 = new List<QueryCondition>();
       
        public CashTypeNameUpdate()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            //Window window = list.Single(temp => temp.bindingData.Equals("window")).value as Window;
            this.CashStoreType.Text = list.Single(temp => temp.bindingData.Equals("currency_code")).value.ToString();
            this.txtCashName.Text = list.Single(temp => temp.bindingData.Equals("currency_name")).value.ToString();
            this.txtIndex.Text = list.Single(temp => temp.bindingData.Equals("currency_value")).value.ToString();
        }

        private void btnUpdateCashetType_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list1, "CashStoreType", this.CashStoreType.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "CashName", this.txtCashName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "txtIndex", this.txtIndex.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.MoneyStoreActions.CashStoreUpdate();

            //dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list1);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            //this.CashStoreType.Text = string.Empty;
            this.txtCashName.Text = string.Empty;
            this.txtIndex.Text = string.Empty;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
