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

namespace AFC.WS.UI.UIPage.TickStoreManager
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
    public partial class TiketTypeAdded : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();
        public TiketTypeAdded()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            InitLoad();
        }
        private void InitLoad()
        {
            try
            {
                Wrapper.FullComboBox<BasiProductTypeInfo>(this.PhyTypeName, BuinessRule.GetInstace().GetTicketPhyType(), "ticket_phy_type_name", "ticket_phy_type", false, false);
                Wrapper.FullComboBox<BasiProductTypeInfo>(this.ProductTypeName, BuinessRule.GetInstace().GetAllProducType(), "product_type_name_cn", "product_type", false, false);
                Wrapper.FullComboBox<BasiProductTypeInfo>(this.CardIssue, BuinessRule.GetInstace().GetCardIssueId(), "card_issue_id", "card_issue_id", false, false);
                //this.DevType.SelectionChanged += new SelectionChangedEventHandler(cmbDevType_SelectionChanged);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        private void btnAddTicketType_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "TickStoreType", this.TickStoreType.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "txtTickName", this.txtTickName.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "PhyTypeName", this.PhyTypeName.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "PhyTypeId", Wrapper.GetComboBoxUid(PhyTypeName));
            Wrapper.Instance.AddQueryConditionToList(list, "ProductTypeName", this.ProductTypeName.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "ProductTypeId", Wrapper.GetComboBoxUid(ProductTypeName));
            Wrapper.Instance.AddQueryConditionToList(list, "CardIssue", this.CardIssue.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.TickStoreActions.TickStoreAdd();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.TickStoreType.Text = string.Empty;
            this.txtTickName.Text = string.Empty;
            this.PhyTypeName.Text = string.Empty;
            this.ProductTypeName.Text = string.Empty;
            this.CardIssue.Text = string.Empty;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
