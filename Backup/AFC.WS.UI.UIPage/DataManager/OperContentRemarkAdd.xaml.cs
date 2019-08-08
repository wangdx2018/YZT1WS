using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace AFC.WS.UI.UIPage.DataManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.CommonControls;
    using System.Collections.ObjectModel;
    using AFC.WS.ModelView.Actions.CommonActions;
    using System.Data;
    using AFC.WS.BR.TickMonyBoxManager;
    using System.Windows.Forms;
    /// <summary>
    /// CashCallIn.xaml 的交互逻辑
    /// </summary>
    public partial class OperContentRemarkAdd : UserControlBase
    {

        private List<QueryCondition> list = new List<QueryCondition>();
        public OperContentRemarkAdd()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "project_name", this.project_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "operator_id", this.operator_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "content", convertString(this.content.Text));
            dpaction.subAction = new AFC.WS.ModelView.Actions.DataManager.AddOperContentAction();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }


        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.project_name.Text = string.Empty;
            this.operator_id.Text = string.Empty;
            this.content.Text = string.Empty;
        }

        void txtPutNo_TextChanged(object sender, EventArgs e)
        {

            labTotalTip.Content = "剩下可输入字数:" + (100 - this.content.Text.Length).ToString();
        }

        public string convertString(string value)
        {
            string after = value.Replace("\"", string.Empty).Replace("'", string.Empty);
            return after;
        }

        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
