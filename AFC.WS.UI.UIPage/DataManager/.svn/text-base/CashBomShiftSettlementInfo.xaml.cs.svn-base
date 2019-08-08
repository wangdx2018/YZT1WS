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
using AFC.BOM2.UIController;
using AFC.WS.Model.Const;
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Common;
using AFC.WS.ModelView.Actions.RunManager;
using AFC.BOM2.MessageDispacher;
using AFC.WS.BR;
using AFC.WS.Model.DB;
using System.Data;
using AFC.WS.ModelView.Actions.CommonActions;
using AFC.WS.ModelView.Actions.DataManager;

namespace AFC.WS.UI.UIPage.DataManager
{
    /// <summary>
    /// SCRunEnd.xaml 的交互逻辑
    /// </summary>
    public partial class CashBomShiftSettlementInfo : UserControlBase
    {
        DateTime RunDate;
        string stationcode;
        private List<QueryCondition> list = new List<QueryCondition>();
        List<CashStorageInfo> listMoney=new List<CashStorageInfo>{};
        List<TickStorageInfo> listTick = new List<TickStorageInfo> { };
        public CashBomShiftSettlementInfo()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            base.InitControls();
            RunDate = DateTime.ParseExact(BuinessRule.GetInstace().rm.GetRunDate(), "yyyy年MM月dd日", null);
            stationcode=SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
            this.GridCashShiftSettlementInfo.ItemsSource = BuinessRule.GetInstace().rm.GetAllMoneyRunDateStatus(stationcode, RunDate.ToString("yyyyMMdd")).DefaultView;
            this.GridTicketShiftSettlementInfo.ItemsSource = BuinessRule.GetInstace().rm.GetAllTikcetRunDateStatus(stationcode,RunDate.ToString("yyyyMMdd")).DefaultView;
            label1.Content = "现金总金额：" + BuinessRule.GetInstace().rm.GetCashTotalStoreValue(stationcode, RunDate.ToString("yyyyMMdd")).ToString().ConvertFenToYuan() + "元";
            label2.Content = "票卡总数量：" + BuinessRule.GetInstace().rm.GetTicketsTotalStoreNum(stationcode, RunDate.ToString("yyyyMMdd")) + "张";
        }

        void printCrystal()
        {
            AFC.WS.UI.UIPage.DataManager.CrystalCashBomShiftData rptData = new AFC.WS.UI.UIPage.DataManager.CrystalCashBomShiftData();

            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add("ReportName", "班次结算");
            dict.Add("OperatorName", BuinessRule.GetInstace().brConext.CurrentOperatorId);
            dict.Add("StationName", BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name);
            dict.Add("RunDate", RunDate.ToString("yyyy年MM月dd日"));
            dict.Add("LineName", BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name);
            dict.Add("ALL_TOTAL_CURRENCY_VALUE", BuinessRule.GetInstace().rm.GetCashTotalStoreValue(stationcode, RunDate.ToString("yyyyMMdd")).ConvertFenToYuan() + "元");
            dict.Add("ALL_TOTAL_TICKETS_NUM", BuinessRule.GetInstace().rm.GetTicketsTotalStoreNum(stationcode, RunDate.ToString("yyyyMMdd")) + "张");

            DataTable dt = new DataTable();

            dt.Columns.Add("TICK_MANA_TYPE", typeof(string));
            dt.Columns.Add("TICKET_STATUS", typeof(string));
            dt.Columns.Add("TOTAL_NUM", typeof(string));
            dt.Columns.Add("CURRENCY_CODE", typeof(string));
            dt.Columns.Add("TOTAL_CURRENCY_NUM", typeof(string));
            dt.Columns.Add("CURRENCY_TOTAL_VALUE", typeof(string));
            listMoney = BuinessRule.GetInstace().GetAllMoneyStoreInfo(SysConfig.GetSysConfig().LocalParamsConfig.StationCode, RunDate.ToString("yyyyMMdd"));
            listTick = BuinessRule.GetInstace().GetAllTicketsStoreInfo(SysConfig.GetSysConfig().LocalParamsConfig.StationCode, RunDate.ToString("yyyyMMdd"));

            

            if(listMoney.Count>=listTick.Count)
            {
                for (int i = 0; i < listMoney.Count; i++)
                {
                    if (i < listTick.Count)
                    {
                        string ticketName = BuinessRule.GetInstace().GetBasiTickManaTypeInfoById(listTick[i].tick_mana_type.ToString()).tick_mana_type_name;
                        string moneyName = BuinessRule.GetInstace().GetBasiMoneyTypeInfoById(listMoney[i].currency_code.ToString()).currency_name;
                        string moneyValue = BuinessRule.GetInstace().GetBasiMoneyTypeInfoById(listMoney[i].currency_code.ToString()).currency_value;
                        string moneyTotalValue =( Convert.ToInt32(moneyValue) * listMoney[i].currency_num).ToString() + "元";
                        dt.Rows.Add(new string[] { ticketName, listTick[i].ticket_status.ToString(), listTick[i].in_store_num.ToString()+"张", moneyName, listMoney[i].currency_num.ToString(), moneyTotalValue });
                    }
                    else
                    {
                        string moneyName = BuinessRule.GetInstace().GetBasiMoneyTypeInfoById(listMoney[i].currency_code.ToString()).currency_name;
                        string moneyValue = BuinessRule.GetInstace().GetBasiMoneyTypeInfoById(listMoney[i].currency_code.ToString()).currency_value;
                        string moneyTotalValue = (Convert.ToInt32(moneyValue) * listMoney[i].currency_num).ToString() + "元";
                        dt.Rows.Add(new string[] { "", "", "", moneyName, listMoney[i].currency_num.ToString(), moneyTotalValue });
                    }
                }
            }
            else
            {
                for (int i = 0; i < listTick.Count; i++)
                {
                    if (i < listMoney.Count)
                    {
                        string ticketName = BuinessRule.GetInstace().GetBasiTickManaTypeInfoById(listTick[i].tick_mana_type.ToString()).tick_mana_type_name;
                        string moneyName = BuinessRule.GetInstace().GetBasiMoneyTypeInfoById(listMoney[i].currency_code.ToString()).currency_name;
                        string moneyValue = BuinessRule.GetInstace().GetBasiMoneyTypeInfoById(listMoney[i].currency_code.ToString()).currency_value;
                        string moneyTotalValue = (Convert.ToInt32(moneyValue) * listMoney[i].currency_num).ToString() + "元";
                        dt.Rows.Add(new string[] { ticketName, listTick[i].ticket_status.ToString(), listTick[i].in_store_num.ToString() + "张", moneyName, listMoney[i].currency_num.ToString(), moneyTotalValue });
                    }
                    else
                    {
                        string ticketName = BuinessRule.GetInstace().GetBasiTickManaTypeInfoById(listTick[i].tick_mana_type.ToString()).tick_mana_type_name;
                        dt.Rows.Add(new string[] { ticketName, listTick[i].ticket_status.ToString(), listTick[i].in_store_num.ToString() + "张","", "", "" });
                    }
                }
            }


            rptData.ShowRptDialog(new AFC.WS.UI.UIPage.DataManager.CrystalCashBomShiftSettlementReport(),
                dict, dt);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (classSettlement().resultCode==0)
            {
                printCrystal();
            }
        }

        private ResultStatus classSettlement()
        {
            AddClassSettlementInfoAction dpaction = new AddClassSettlementInfoAction();
            Wrapper.Instance.AddQueryConditionToList(list, "tickets_remain", BuinessRule.GetInstace().rm.GetTicketsTotalStoreNum(stationcode, RunDate.ToString("yyyyMMdd")));
            Wrapper.Instance.AddQueryConditionToList(list, "cash_balance", BuinessRule.GetInstace().rm.GetCashTotalStoreValue(stationcode, RunDate.ToString("yyyyMMdd")));
            Wrapper.Instance.AddQueryConditionToList(list, "operator_id", BuinessRule.GetInstace().brConext.CurrentOperatorId);
            Wrapper.Instance.AddQueryConditionToList(list, "run_date", RunDate.ToString("yyyyMMdd"));
            dpaction.CheckValid(list);
            return dpaction.DoAction(list);
        }
    }
}
