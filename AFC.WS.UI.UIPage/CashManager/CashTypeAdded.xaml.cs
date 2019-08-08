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
    public partial class CashTypeAdded : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();
        public CashTypeAdded()
        {
            InitializeComponent();
        }


        private void btnAddCashetType_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "CashStoreType", this.CashStoreType.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "CashName", this.txtCashName.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "txtIndex", this.txtIndex.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.MoneyStoreActions.CashStoreAdd();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.CashStoreType.Text = string.Empty;
            this.txtCashName.Text = string.Empty;
            this.txtIndex.Text = string.Empty;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
