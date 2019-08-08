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
    public partial class TiketTypeNameUpdate : UserControlBase
    {
        private List<QueryCondition> list1 = new List<QueryCondition>();
        string PhyType = "";
        string ProductType = "";
        string CardIssue1 = "";
       
        public TiketTypeNameUpdate()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            List<QueryCondition> list = this.Tag as List<QueryCondition>;
            //Window window = list.Single(temp => temp.bindingData.Equals("window")).value as Window;
            this.TickStoreType.Text = list.Single(temp => temp.bindingData.Equals("tick_mana_type")).value.ToString();
            this.txtTickName.Text = list.Single(temp => temp.bindingData.Equals("tick_mana_type_name")).value.ToString();
            //票卡的物理类型
            PhyType = list.Single(temp => temp.bindingData.Equals("ticket_phy_type")).value.ToString();
            try
            {
                Wrapper.FullComboBox<BasiProductTypeInfo>(this.PhyTypeName, BuinessRule.GetInstace().GetTicketPhyType(), "ticket_phy_type_name", "ticket_phy_type", false, false);
                //this.DevType.SelectionChanged += new SelectionChangedEventHandler(cmbDevType_SelectionChanged);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            Wrapper.ComboBoxSelectedItem(this.PhyTypeName, PhyType);
            //票卡的基本类型（FamilyType）
            ProductType = list.Single(temp => temp.bindingData.Equals("ticket_family_type")).value.ToString();
            try
            {
                Wrapper.FullComboBox<BasiProductTypeInfo>(this.ProductTypeName, BuinessRule.GetInstace().GetAllProducType(), "product_type_name_cn", "product_type", false, false);
                //this.DevType.SelectionChanged += new SelectionChangedEventHandler(cmbDevType_SelectionChanged);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            Wrapper.ComboBoxSelectedItem(this.ProductTypeName, ProductType);
            CardIssue1 = list.Single(temp => temp.bindingData.Equals("card_issue_id")).value.ToString();
            try
            {
                Wrapper.FullComboBox<BasiProductTypeInfo>(this.CardIssue, BuinessRule.GetInstace().GetCardIssueId(), "card_issue_id", "card_issue_id", false, false);
                //this.DevType.SelectionChanged += new SelectionChangedEventHandler(cmbDevType_SelectionChanged);
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            Wrapper.ComboBoxSelectedItem(this.CardIssue, CardIssue1);
        }

        private void btnUpdateTicketType_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list1, "TickStoreType", this.TickStoreType.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "txtTickName", this.txtTickName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "PhyTypeName", this.PhyTypeName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "PhyTypeId", Wrapper.GetComboBoxUid(PhyTypeName));
            Wrapper.Instance.AddQueryConditionToList(list1, "ProductTypeName", this.ProductTypeName.Text);
            Wrapper.Instance.AddQueryConditionToList(list1, "ProductTypeId", Wrapper.GetComboBoxUid(ProductTypeName));
            Wrapper.Instance.AddQueryConditionToList(list1, "CardIssue", this.CardIssue.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.TickStoreActions.TickStoreUpdate();

            //dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list1);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            //this.TickStoreType.Text = string.Empty;
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
