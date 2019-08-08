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
    /// TickStoreAdjust.xaml 的交互逻辑
    /// </summary>
    public partial class TickStoreAdjust : UserControlBase
    {
        private List<QueryCondition> list = new List<QueryCondition>();


        private string tickStatus = string.Empty;

        public TickStoreAdjust()
        {
            InitializeComponent();
            this.cmbTickStoreType.SelectionChanged += new SelectionChangedEventHandler(cmbTickStoreType_SelectionChanged);
            this.cmbTickStatus.SelectionChanged += new SelectionChangedEventHandler(cmbTickStatus_SelectionChanged);

        }

        void cmbTickStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbTickStoreType_SelectionChanged(sender, e);
            //throw new NotImplementedException();
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
                List<BasiTickManaTypeInfo> list = BuinessRule.GetInstace().GetBasiTickManaTypeInfo();
                this.cmbTickStoreType.ItemsSource = list;
                this.cmbTickStoreType.DisplayMemberPath = "tick_mana_type_name";
                this.txtTickNo.Text = string.Empty;
                this.txtRealNo.Text = string.Empty;
                this.txtRemark.Text = string.Empty;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        private void cmbTickStoreType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            BasiTickManaTypeInfo info = this.cmbTickStoreType.SelectedItem as BasiTickManaTypeInfo;
            if (info != null)
            {
                string type = info.tick_mana_type;
                if (!string.IsNullOrEmpty(type))
                {
                    tickStatus = (this.cmbTickStatus.SelectedItem as ComboBoxItem).Tag.ToString();
                    TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(type, tickStatus);
                    if (string.IsNullOrEmpty(tickStoreInfo.tick_mana_type))
                        this.txtTickNo.Text = "0";
                    else
                        this.txtTickNo.Text = tickStoreInfo.in_store_num.ToString();
                }
                //this.txtTickNo.Text = BuinessRule.GetInstace().GetTickStoreInfoByTickManType(tickType).in_store_num.ToString();
                this.txtRealNo.Text = string.Empty;
            }

        }

        private void btnAdjust_Click(object sender, RoutedEventArgs e)
        {
           BasiTickManaTypeInfo btmti= this.cmbTickStoreType.SelectedItem as BasiTickManaTypeInfo;
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            if (btmti == null)
            {
                Wrapper.Instance.AddQueryConditionToList(list, "tickManaType", string.Empty);
            }
            else
            {
                Wrapper.Instance.AddQueryConditionToList(list, "tickManaType", btmti.tick_mana_type);
            }
            Wrapper.Instance.AddQueryConditionToList(list, "tickNo", this.txtTickNo.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "tickReal", this.txtRealNo.Text);
            string value = (this.cmbTickStatus.SelectedItem as ComboBoxItem).Tag.ToString();
            Wrapper.Instance.AddQueryConditionToList(list, "tickStatus", value);
            Wrapper.Instance.AddQueryConditionToList(list, "remark", convertString(this.txtRemark.Text));
            dpaction.subAction = new AFC.WS.ModelView.Actions.TicketBoxManager.TickStoreAdjustAction();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            if (dpaction.CheckValid(list))
            {
               ResultStatus rs= dpaction.DoAction(list);
               if (rs != null)
               {
                   this.txtTickNo.Text = this.txtRealNo.Text;
                   this.txtRealNo.Text = string.Empty;
               }
            }

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.txtRealNo.Text = string.Empty;
            this.txtTickNo.Text = string.Empty;
            this.txtRemark.Text = string.Empty;
            this.cmbTickStoreType.SelectedIndex = -1;

        }

        void txtPutNo_TextChanged(object sender, EventArgs e)
        {

            label1.Content = "剩下可输入字数:" + (50 - this.txtRemark.Text.Length).ToString();
        }

        public string convertString(string value)
        {
            string after = value.Replace("\"", string.Empty).Replace("'", string.Empty);
            return after;
        }
    }
}
