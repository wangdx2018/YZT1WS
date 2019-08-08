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
    public partial class OperContentRemarkUpdate : UserControlBase
    {

        private List<QueryCondition> list = new List<QueryCondition>();
        public OperContentRemarkUpdate()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            List<QueryCondition> list1 = this.Tag as List<QueryCondition>;
            this.content_sn.Text = list1.Single(temp => temp.bindingData.Equals("content_sn")).value.ToString();
            this.line_id.Text = list1.Single(temp => temp.bindingData.Equals("line_name")).value.ToString();
            this.station_id.Text = list1.Single(temp => temp.bindingData.Equals("station_cn_name")).value.ToString();
            this.project_name.Text = list1.Single(temp => temp.bindingData.Equals("project_name")).value.ToString();
            this.operator_id.Text = list1.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            this.content.Text = list1.Single(temp => temp.bindingData.Equals("content")).value.ToString();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "content_sn", this.content_sn.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "line_id", BuinessRule.GetInstace().GetLineInfoByName(this.line_id.Text).line_id.ToString());
            Wrapper.Instance.AddQueryConditionToList(list, "station_id", BuinessRule.GetInstace().GetStationInfoByName(this.station_id.Text).station_id.ToString());
            Wrapper.Instance.AddQueryConditionToList(list, "project_name", this.project_name.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "operator_id", this.operator_id.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "content", convertString(this.content.Text));
            dpaction.subAction = new AFC.WS.ModelView.Actions.DataManager.UpdateOperContent();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }


        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.content_sn.Text = string.Empty;
            this.line_id.Text = string.Empty;
            this.station_id.Text = string.Empty;
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
