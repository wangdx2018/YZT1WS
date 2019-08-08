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
    public partial class BussinessDetailInfoQuery : UserControlBase
    {
        string stationId;
        string runDateTran;
        string tranValue;
        string todayCashBankTotal;
        private List<QueryCondition> list;
        List<CashStorageInfo> listMoney=new List<CashStorageInfo>{};
        List<TickStorageInfo> listTick = new List<TickStorageInfo> {};
        public BussinessDetailInfoQuery()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            base.InitControls();
            list = this.Tag as List<QueryCondition>;
            stationId = list.Single(temp => temp.bindingData.Equals("station_id")).value.ToString();
            runDateTran = list.Single(temp => temp.bindingData.Equals("run_date_tran")).value.ToString();
            tranValue = list.Single(temp => temp.bindingData.Equals("tran_value")).value.ToString();
            todayCashBankTotal = list.Single(temp => temp.bindingData.Equals("today_cash_bank_total")).value.ToString();
            this.GridCashShiftSettlementInfo.ItemsSource = BuinessRule.GetInstace().rm.GetBOMRunDateBussDetail(stationId, runDateTran).DefaultView;
            this.GridTicketShiftSettlementInfo.ItemsSource = BuinessRule.GetInstace().rm.GetTVMRunDateBussDetail(stationId, runDateTran).DefaultView;
            label1.Content = "现金总金额：" + tranValue.ConvertFenToYuan() + "元";
            label2.Content = "待解行现金总金额：" + todayCashBankTotal.ConvertFenToYuan() + "元";
        }
    }
}
