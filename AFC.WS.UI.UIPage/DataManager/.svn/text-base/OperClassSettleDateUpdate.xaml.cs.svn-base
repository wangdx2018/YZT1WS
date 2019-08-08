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
    public partial class OperClassSettleDateUpdate : UserControlBase
    {

        private List<QueryCondition> list = new List<QueryCondition>();
        List<QueryCondition> list1;
        DateTime RunDate;
        string line_id;
        string station_id;
        string operator_id;
        string rundate;
        string oper_date;
        string oper_time;

        public OperClassSettleDateUpdate()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            list1 = this.Tag as List<QueryCondition>;
            line_id = list1.Single(temp => temp.bindingData.Equals("line_id")).value.ToString();
            station_id = list1.Single(temp => temp.bindingData.Equals("station_id")).value.ToString();
            operator_id = list1.Single(temp => temp.bindingData.Equals("operator_id")).value.ToString();
            oper_date = list1.Single(temp => temp.bindingData.Equals("oper_date")).value.ToString();
            oper_time = list1.Single(temp => temp.bindingData.Equals("oper_time")).value.ToString();
            RunDate =DateTime.ParseExact(list1.Single(temp => temp.bindingData.Equals("run_date")).value.ToString(), "yyyyMMdd", null);
            this.balance_date.Content = RunDate.ToString("yyyy年MM月dd日");
            this.tickets_balance.Text = list1.Single(temp => temp.bindingData.Equals("tickets_remain")).value.ToString();
            this.store_cash_total.Text = list1.Single(temp => temp.bindingData.Equals("store_cash_total")).value.ToString();
            this.today_diff_amount.Text = list1.Single(temp => temp.bindingData.Equals("today_diff_amount")).value.ToString();
            this.coin_store_amount.Text = list1.Single(temp => temp.bindingData.Equals("coin_store_amount")).value.ToString();
            this.tvm_income.Text = list1.Single(temp => temp.bindingData.Equals("tvm_income")).value.ToString();
            this.ergency_tickets_income.Text = list1.Single(temp => temp.bindingData.Equals("urgency_tikets_income")).value.ToString();
            this.other_income.Text = list1.Single(temp => temp.bindingData.Equals("others_income")).value.ToString();
            this.bom_income.Text = list1.Single(temp => temp.bindingData.Equals("bom_income")).value.ToString();
            this.group_tickets_income.Text = list1.Single(temp => temp.bindingData.Equals("group_tickets_income")).value.ToString();
            this.balance_income.Text = list1.Single(temp => temp.bindingData.Equals("income_store")).value.ToString();
            this.yesterday_bank_income.Text = list1.Single(temp => temp.bindingData.Equals("yesterday_bank_income")).value.ToString();
            this.account_income.Text = list1.Single(temp => temp.bindingData.Equals("account_income")).value.ToString();
            this.yesterday_bank_store_cash.Text = list1.Single(temp => temp.bindingData.Equals("yesterday_bank_cash_store")).value.ToString();
            this.yesterday_income_amount.Text = list1.Single(temp => temp.bindingData.Equals("yesterday_income_amount")).value.ToString();
            this.today_income_amount.Text = list1.Single(temp => temp.bindingData.Equals("today_income_amount")).value.ToString();
            this.today_subway_income.Text = list1.Single(temp => temp.bindingData.Equals("today_subway_income")).value.ToString();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "line_id", line_id);
            Wrapper.Instance.AddQueryConditionToList(list, "station_id", station_id);
            Wrapper.Instance.AddQueryConditionToList(list, "operator_id", operator_id);
            Wrapper.Instance.AddQueryConditionToList(list, "oper_date", oper_date);
            Wrapper.Instance.AddQueryConditionToList(list, "oper_time", oper_time);
            Wrapper.Instance.AddQueryConditionToList(list, "balance_date", RunDate.ToString("yyyyMMdd"));
            Wrapper.Instance.AddQueryConditionToList(list, "tickets_balance", this.tickets_balance.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "store_cash_total", this.store_cash_total.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "today_diff_amount", this.today_diff_amount.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "coin_store_amount", this.coin_store_amount.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "tvm_income", this.tvm_income.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "ergency_tickets_income", this.ergency_tickets_income.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "other_income", this.other_income.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "bom_income", this.bom_income.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "group_tickets_income", this.group_tickets_income.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "balance_income", this.balance_income.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "yesterday_bank_income", this.yesterday_bank_income.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "account_income", this.account_income.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "yesterday_bank_store_cash", this.yesterday_bank_store_cash.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "yesterday_income_amount", this.yesterday_income_amount.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "today_income_amount", this.today_income_amount.Text);
            Wrapper.Instance.AddQueryConditionToList(list, "today_subway_income", this.today_subway_income.Text);
            dpaction.subAction = new AFC.WS.ModelView.Actions.DataManager.UpdateCashDateSettlementInfoAction();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            //if (dpaction.CheckValid(list))
            //{
            dpaction.DoAction(list);
        }


        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.tickets_balance.Text = list1.Single(temp => temp.bindingData.Equals("tickets_remain")).value.ToString();
            this.store_cash_total.Text = list1.Single(temp => temp.bindingData.Equals("store_cash_total")).value.ToString();
            this.today_diff_amount.Text = list1.Single(temp => temp.bindingData.Equals("today_diff_amount")).value.ToString();
            this.coin_store_amount.Text = list1.Single(temp => temp.bindingData.Equals("coin_store_amount")).value.ToString();
            this.tvm_income.Text = list1.Single(temp => temp.bindingData.Equals("tvm_income")).value.ToString();
            this.ergency_tickets_income.Text = list1.Single(temp => temp.bindingData.Equals("urgency_tikets_income")).value.ToString();
            this.other_income.Text = list1.Single(temp => temp.bindingData.Equals("others_income")).value.ToString();
            this.bom_income.Text = list1.Single(temp => temp.bindingData.Equals("bom_income")).value.ToString();
            this.group_tickets_income.Text = list1.Single(temp => temp.bindingData.Equals("group_tickets_income")).value.ToString();
            this.balance_income.Text = list1.Single(temp => temp.bindingData.Equals("income_store")).value.ToString();
            this.yesterday_bank_income.Text = list1.Single(temp => temp.bindingData.Equals("yesterday_bank_income")).value.ToString();
            this.account_income.Text = list1.Single(temp => temp.bindingData.Equals("account_income")).value.ToString();
            this.yesterday_bank_store_cash.Text = list1.Single(temp => temp.bindingData.Equals("yesterday_bank_cash_store")).value.ToString();
            this.yesterday_income_amount.Text = list1.Single(temp => temp.bindingData.Equals("yesterday_income_amount")).value.ToString();
            this.today_income_amount.Text = list1.Single(temp => temp.bindingData.Equals("today_income_amount")).value.ToString();
            this.today_subway_income.Text = list1.Single(temp => temp.bindingData.Equals("today_subway_income")).value.ToString();
        }


        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
