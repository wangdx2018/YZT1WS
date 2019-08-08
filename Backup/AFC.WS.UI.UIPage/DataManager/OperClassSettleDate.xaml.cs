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
    using Microsoft.Windows.Controls;
    /// <summary>
    /// CashCallIn.xaml 的交互逻辑
    /// </summary>
    public partial class OperClassSettleDate : UserControlBase
    {

        private List<QueryCondition> list = new List<QueryCondition>();
        DateTime RunDate = DateTime.Now;
        string line_id;
        string station_id;
        double cashTotalStore;
        double cashTodayBankTotal;
        double cashBankTotal;
        string cashTotalStores;
        string cashBankTotals;
        string cashTodayBankTotals;
        string bomIncomes;
        string tvmIncomes;
        string date = BuinessRule.GetInstace().rm.GetRunDate();
        DateTime RunDateCurrent = DateTime.Now;
        DateTime selectRunDate = DateTime.Now;
        //结算日期控件
        private Microsoft.Windows.Controls.DatePicker dp = null;

        public OperClassSettleDate()
        {
            //默认为当前运营日,运营日从数据库basi_run_param_info里取到
            InitializeComponent();
            this.balance_date.SetControlValue(date);
            this.balance_date.FormatDateTime = "yyyyMMdd";
            //监听切换不同日期时的消息
            UIElement elementPick = this.balance_date;
            DateTime RunDate = DateTime.ParseExact(BuinessRule.GetInstace().rm.GetRunDate(), "yyyy年MM月dd日", null);
            selectRunDate = RunDate;
            if (elementPick != null && elementPick is DateTimePickerExtend)
            {
                DateTimePickerExtend pickExtend = elementPick as DateTimePickerExtend;
                Grid g = pickExtend.Content as Grid;
                if (g != null)
                {
                    foreach (var a in g.Children)
                    {
                        if (a is Microsoft.Windows.Controls.DatePicker)
                        {
                            dp = a as Microsoft.Windows.Controls.DatePicker;
                            dp.SelectedDate = RunDate;
                            dp.DisplayDate = RunDate;
                            dp.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(balance_date_TextChanged);
                            break;
                        }
                    }
                }
            }

            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName == "SCWS")
            {
                this.today_diff_amount.IsEnabled = false;
                this.tvm_income.IsEnabled = false;
                this.today_income_amount.IsEnabled = false;
                this.yesterday_income_amount.IsEnabled = false;
                this.today_subway_income.IsEnabled = false;
            }  
        }

        public override void InitControls()
        {   //默认为当前运营日,运营日从数据库basi_run_param_info里取到
            string runDates = BuinessRule.GetInstace().rm.GetRunDate();
            this.balance_date.SetControlValue(runDates);
            this.balance_date.FormatDateTime = "yyyyMMdd";
            RunDate = DateTime.ParseExact(runDates, "yyyy年MM月dd日", null);
            //自然日为每月前3天时，可以填写“清算营收”
            DateTime current = DateTime.Now;
            if (current.ToString("dd") == "01" || current.ToString("dd") == "02" || current.ToString("dd") == "03")
            {
                 this.balance_income.IsEnabled = true;
            }
            else
            {
                 this.balance_income.IsEnabled = false;
            }
            //界面初始化时计算运营日结算中的各项值
            line_id = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
            station_id = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            List<QueryCondition> list1 = this.Tag as List<QueryCondition>;
            //如果是SCWS则计算各项值
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName == "SCWS")
            {
                ///CashDateSettlementInfo csi = BuinessRule.GetInstace().rm.GetStationCashDateSettlementInfoValue(station_id, RunDate.ToString("yyyyMMdd"));
                //if (csi != null)
                //{
                //    fullfill_RunDateSettleDate(csi);
                //}
                //else
                //{
                    calc_RunDateSettleDate();
                ///}
            }
            //如果是LCWS则是合计SC上各项值
            else
            {
               // CashDateSettlementInfo csi = BuinessRule.GetInstace().rm.GetStationCashDateSettlementInfoValue(station_id, RunDate.ToString("yyyyMMdd"));
               //if (csi != null)
               //{
               //    fullfill_RunDateSettleDate(csi);
               //}
               //else
               //{
                   CashDateSettlementInfo csilc = BuinessRule.GetInstace().rm.GetCashDateSettlementInfoValue(station_id, RunDate.ToString("yyyyMMdd"));
                   fullfill_RunDateSettleDate(csilc);
               ///}
            }
        }

        //处理运营日结算“确认”按钮摁下后发生的事件
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            string runDate=this.balance_date.GetControlValue().ToString();
            //未能得到运营日时的处理
            if(string.IsNullOrEmpty(runDate))
            {
                MessageDialog.Show("请选择运营日期!","提示",AFC.WS.UI.CommonControls.MessageBoxIcon.Information,AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                return;
            }
            selectRunDate= DateTime.ParseExact(runDate, "yyyyMMdd", null);

            DateTime sysRunDate = DateTime.ParseExact(BuinessRule.GetInstace().rm.GetRunDate(), "yyyy年MM月dd日", null);
            //所选运营日大于系统运营日，或者小于系统运营日一天情况下不能进行运营日结算
            if (selectRunDate.Subtract(sysRunDate).TotalDays > 0 || selectRunDate.Subtract(sysRunDate).TotalDays < -1)
            {
                MessageDialog.Show("所选运营日，无法进行运营日结算!", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                return;
            }
            //当前运营日前一天未做运营日结算，且当前运营日同样未做过运营日结算的情况下，要给昨日未做运营日结算的提示，当前运营日已做过运营日结算的情况下不提示
            if (selectRunDate.Subtract(sysRunDate).TotalDays == 0)
            {
                CashDateSettlementInfo csit = BuinessRule.GetInstace().rm.GetStationCashDateSettlementInfoValue(station_id, sysRunDate.ToString("yyyyMMdd"));
                CashDateSettlementInfo csiy = BuinessRule.GetInstace().rm.GetStationCashDateSettlementInfoValue(station_id, sysRunDate.Subtract(new TimeSpan(24, 0, 0)).ToString("yyyyMMdd"));
                if (csiy == null && csit == null)
                {
                    MessageBoxResult res = MessageDialog.Show("昨日未做运营日结算,请注意,是否要继续做今日运营日结算？", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, AFC.WS.UI.CommonControls.MessageBoxButtons.YesNo);
                    if (res == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            }
        
            DoublePrimissionAction dpaction = new DoublePrimissionAction();
            Wrapper.Instance.AddQueryConditionToList(list, "balance_date", selectRunDate.ToString("yyyyMMdd"));
            Wrapper.Instance.AddQueryConditionToList(list, "tickets_balance", this.tickets_balance.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "today_cash_bank_total", this.today_cash_bank_total.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "today_diff_amount", this.today_diff_amount.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "coin_store_amount", this.coin_store_amount.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "tvm_income", this.tvm_income.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "ergency_tickets_income", this.ergency_tickets_income.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "other_income", this.other_income.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "bom_income", this.bom_income.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "group_tickets_income", this.group_tickets_income.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "balance_income", this.balance_income.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "tomorrow_bank_income", this.tomorrow_bank_income.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "account_income", this.account_income.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "yesterday_income_amount", this.yesterday_income_amount.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "today_income_amount", this.today_income_amount.Text.ConvertYuanToFen());
            Wrapper.Instance.AddQueryConditionToList(list, "today_subway_income", this.today_subway_income.Text.ConvertYuanToFen());
            dpaction.subAction = new AFC.WS.ModelView.Actions.DataManager.AddCashDateSettlementInfoAction();
            dpaction.CurrentOperationId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            dpaction.DoAction(list);
        }
        //TVM营收,应急票营收,其它营收,应急票营收,BOM营收,团体凭证营收会引起今日地铁营收的变化
        //今日地铁营收,今日实际营收的变化会引起今日差异合计的变化
        //昨日累计盘盈,今日差异合计,今日实际营收会引起今日累计盘盈的变化
        private void txtPutNo_TextChanged(object sender, RoutedEventArgs e)
        {
            this.today_subway_income.Text = (this.tvm_income.Text.ToDecimalNumber()
                + this.bom_income.Text.ToDecimalNumber()
                + this.ergency_tickets_income.Text.ToDecimalNumber()
                + this.group_tickets_income.Text.ToDecimalNumber()
                + this.other_income.Text.ToDecimalNumber()
                ).ToString();
            this.today_diff_amount.Text = (this.today_cash_bank_total.Text.ToDecimalNumber()
                - this.today_subway_income.Text.ToDecimalNumber()).ToString();
            this.today_income_amount.Text = (this.yesterday_income_amount.Text.ToDecimalNumber()
                + this.today_diff_amount.Text.ToDecimalNumber()
                - this.balance_income.Text.ToDecimalNumber()).ToString();
                }
        //今日地铁营收,今日实际营收的变化会引起今日差异合计的变化
        //昨日累计盘盈,今日差异合计,今日实际营收会引起今日累计盘盈的变化
        private void todaySubway_TextChanged(object sender, RoutedEventArgs e)
        {
            this.today_diff_amount.Text = (this.today_cash_bank_total.Text.ToDecimalNumber()
                - this.today_subway_income.Text.ToDecimalNumber()).ToString();
            this.today_income_amount.Text = (this.yesterday_income_amount.Text.ToDecimalNumber()
                     + this.today_diff_amount.Text.ToDecimalNumber()
                     - this.balance_income.Text.ToDecimalNumber()).ToString();
        }
        //昨日累计盘盈,今日差异合计,今日实际营收会引起今日累计盘盈的变化
        private void todayDiff_TextChanged(object sender, RoutedEventArgs e)
        {
            this.today_income_amount.Text = (this.yesterday_income_amount.Text.ToDecimalNumber()
                + this.today_diff_amount.Text.ToDecimalNumber()
                - this.balance_income.Text.ToDecimalNumber()).ToString();
        }
        //票务室结余为营收结存和票务室现金和，当营收结存发生变化时会引起票务室结余的变化
        private void ticketsBalance_TextChanged(object sender, RoutedEventArgs e)
        {
            this.tickets_balance.Text = (cashTotalStores.ToDecimalNumber()
                    + this.account_income.Text.ConvertYuanToFen().ToDecimalNumber()).ToString().ConvertFenToYuan();
        }

        //处理“运营日重新选择”后发生的事件
        private void balance_date_TextChanged(object sender, RoutedEventArgs e)
        {
            string runDate = this.balance_date.GetControlValue().ToString();
            //如果获得的“运营日”为空时候的处理
            if (string.IsNullOrEmpty(runDate))
            {
                MessageDialog.Show("请选择运营日期!", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                return;
            }
            DateTime current = DateTime.Now;
            //当自然日为每个月前三天时可以进行清算营收
            if (current.ToString("dd") == "01" || current.ToString("dd") == "02" || current.ToString("dd") == "03")
            {
                this.balance_income.IsEnabled = true;
            }
            else
            {
                this.balance_income.IsEnabled = false;
            }
            //所选运营日大于系统运营日，或者小于系统运营日一天情况下不能进行运营日结算
            RunDate = DateTime.ParseExact(BuinessRule.GetInstace().rm.GetRunDate(), "yyyy年MM月dd日", null);
            selectRunDate = DateTime.ParseExact(runDate, "yyyyMMdd", null);
            if (selectRunDate.Subtract(RunDate).TotalDays > 0 || selectRunDate.Subtract(RunDate).TotalDays < -1)
            {
                button1.IsEnabled = false;
                MessageDialog.Show("选择的运营日不能进行运营日结算!", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
            }
            //所选运营日等于系统运营日，计算运营日结算各项值
            else if (selectRunDate.Subtract(RunDate).TotalDays == 0)
            {
                button1.IsEnabled = true;
                if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName == "SCWS")
                {
                    //CashDateSettlementInfo csi = BuinessRule.GetInstace().rm.GetStationCashDateSettlementInfoValue(station_id, selectRunDate.ToString("yyyyMMdd"));
                    //if (csi != null)
                    //{
                    //    fullfill_RunDateSettleDate(csi);
                    //}
                    //else
                    //{
                        calc_RunDateSettleDate();
                    ///}
                }
                else
                {
                    //CashDateSettlementInfo csi = BuinessRule.GetInstace().rm.GetStationCashDateSettlementInfoValue(station_id, selectRunDate.ToString("yyyyMMdd"));
                    //if (csi != null)
                    //{
                    //    fullfill_RunDateSettleDate(csi);
                    //}
                    //else
                    //{
                        CashDateSettlementInfo csilc = BuinessRule.GetInstace().rm.GetCashDateSettlementInfoValue(station_id, selectRunDate.ToString("yyyyMMdd"));
                        fullfill_RunDateSettleDate(csilc);
                    ///}
                }
            }
            //所选运营日小于系统运营日一天时，计算运营日结算各项值
            else if (selectRunDate.Subtract(RunDate).TotalDays == -1)
            {
                button1.IsEnabled = true;
                if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName == "SCWS")
                {
                    CashDateSettlementInfo csi = BuinessRule.GetInstace().rm.GetStationCashDateSettlementInfoValue(station_id, selectRunDate.ToString("yyyyMMdd"));
                    if (csi != null)
                    {
                        fullfill_RunDateSettleDate(csi);
                    }
                    else
                    {
                        calc_RunDateSettleDate();
                        MessageDialog.Show("所选运营日未做运营日结算!", "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Information, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                    }
                }
                else
                {
                    CashDateSettlementInfo csi = BuinessRule.GetInstace().rm.GetStationCashDateSettlementInfoValue(station_id, selectRunDate.ToString("yyyyMMdd"));
                    if (csi != null)
                    {
                        fullfill_RunDateSettleDate(csi);
                     }
                    else
                    {
                         CashDateSettlementInfo csilc = BuinessRule.GetInstace().rm.GetCashDateSettlementInfoValue(station_id, selectRunDate.ToString("yyyyMMdd"));
                         fullfill_RunDateSettleDate(csilc);
                    }
                }
            }
        }

        //根据从数据库cash_date_settlement_info表中查询到的结算信息，填写各项的值
        //2013.1.4  修改account_income（清算营收）向页面中的balance_income里面赋值，income_store（营收结算）向页面中的account_income里面赋值
        private void fullfill_RunDateSettleDate(CashDateSettlementInfo csi)
        {
            this.tickets_balance.Text = csi.tickets_remain.ToString().ConvertFenToYuan();
            this.today_cash_bank_total.Text = csi.today_cash_bank_total.ToString().ConvertFenToYuan();
            this.today_diff_amount.Text = csi.today_diff_amount.ToString().ConvertFenToYuan();
            this.coin_store_amount.Text = csi.coin_store_amount.ToString().ConvertFenToYuan();
            this.tvm_income.Text = csi.tvm_income.ToString().ConvertFenToYuan();
            this.ergency_tickets_income.Text = csi.urgency_tikets_income.ToString().ConvertFenToYuan();
            this.other_income.Text = csi.others_income.ToString().ConvertFenToYuan();
            this.bom_income.Text = csi.bom_income.ToString().ConvertFenToYuan();
            this.group_tickets_income.Text = csi.group_tickets_income.ToString().ConvertFenToYuan();
            this.balance_income.Text = csi.account_income.ToString().ConvertFenToYuan();
            this.tomorrow_bank_income.Text = csi.tomorrow_bank_income.ToString().ConvertFenToYuan();
            this.account_income.Text = csi.income_store.ToString().ConvertFenToYuan();
            this.yesterday_income_amount.Text = csi.yesterday_income_amount.ToString().ConvertFenToYuan();
            this.today_income_amount.Text = csi.today_income_amount.ToString().ConvertFenToYuan();
            this.today_subway_income.Text = csi.today_subway_income.ToString().ConvertFenToYuan();
        }

        //根据各项的计算公式，逐一进行计算
        private void calc_RunDateSettleDate()
        {
            tvmIncomes = BuinessRule.GetInstace().rm.GetTVMIncomeValue(station_id, selectRunDate.ToString("yyyyMMdd"));
            bomIncomes = BuinessRule.GetInstace().rm.GetBOMIncomeValue(station_id, selectRunDate.ToString("yyyyMMdd"));
            cashTotalStores = BuinessRule.GetInstace().rm.GetCashTotalStoreValue(station_id, selectRunDate.ToString("yyyyMMdd"));
            cashBankTotals = BuinessRule.GetInstace().rm.GetCashBankTotalValue(station_id);
            cashTodayBankTotals = BuinessRule.GetInstace().rm.GetTodayCashBankTotalValue(station_id, selectRunDate.ToString("yyyyMMdd"));
            cashTotalStore = Convert.ToDouble(cashTotalStores);
            cashBankTotal = Convert.ToDouble(cashBankTotals);
            cashTodayBankTotal = Convert.ToDouble(cashTodayBankTotals);
            this.today_cash_bank_total.Text = cashTodayBankTotal.ToString().ConvertFenToYuan();
            this.coin_store_amount.Text = BuinessRule.GetInstace().rm.GetCoinTotalStoreValue(station_id, selectRunDate.ToString("yyyyMMdd")).ConvertFenToYuan();
            this.tvm_income.Text = Convert.ToDouble(tvmIncomes).ToString().ConvertFenToYuan();
            this.ergency_tickets_income.Text = "0";
            this.other_income.Text = "0";
            this.bom_income.Text = Convert.ToDouble(bomIncomes).ToString().ConvertFenToYuan();
            this.group_tickets_income.Text = "0";
            this.balance_income.Text = "0";
            this.today_subway_income.Text = (Convert.ToDouble(tvmIncomes) + Convert.ToDouble(bomIncomes) + Convert.ToDouble(this.ergency_tickets_income.Text.ConvertYuanToFen()) + Convert.ToDouble(this.group_tickets_income.Text.ConvertYuanToFen()) + Convert.ToDouble(this.other_income.Text.ConvertYuanToFen())).ToString().ConvertFenToYuan();
            this.tomorrow_bank_income.Text = (BuinessRule.GetInstace().rm.GetTomorrowIncomeAmount(station_id, selectRunDate.ToString("yyyyMMdd"))).ConvertFenToYuan();
            this.account_income.Text = BuinessRule.GetInstace().rm.GetCashWaitingToBankTotalValue(station_id).ConvertFenToYuan();
            this.yesterday_income_amount.Text = (BuinessRule.GetInstace().rm.GetYesterdayIncomeAmount(station_id, selectRunDate.AddDays(-1).ToString("yyyyMMdd"))).ConvertFenToYuan();
            this.today_diff_amount.Text = (Convert.ToDouble(cashTodayBankTotals) - Convert.ToDouble(this.today_subway_income.Text.ConvertYuanToFen())).ToString().ConvertFenToYuan();
            this.today_income_amount.Text = (Convert.ToDouble(yesterday_income_amount.Text.ConvertYuanToFen()) + Convert.ToDouble(this.today_diff_amount.Text.ConvertYuanToFen()) - Convert.ToDouble(this.balance_income.Text.ConvertYuanToFen())).ToString().ConvertFenToYuan();
            this.tickets_balance.Text = (cashTotalStore + Convert.ToDouble(this.account_income.Text.ConvertYuanToFen())).ToString().ConvertFenToYuan();
        }


        /// <summary>
        /// 重写初始化控件
        /// </summary>
    }
}
