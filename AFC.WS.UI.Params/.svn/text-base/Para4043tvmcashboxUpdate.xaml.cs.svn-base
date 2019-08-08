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
using AFC.WS.UI.CommonControls;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.ModelView.Actions.ParamActions;
using AFC.WS.BR;
using System.Data;

namespace AFC.WS.UI.Params
{
    /// <summary>
    /// Para4043tvm_cash_box.xaml 的交互逻辑
    /// </summary>
    public partial class Para4043tvmcashboxUpdate : UserControlBase
    {
        Para4043TvmCashBox para4043 = new Para4043TvmCashBox();

        //List<Para4043TvmCashBox> lt = new List<Para4043TvmCashBox>();
        //DataTable dt = this.Tag as DataTable;

        //List<Para4043TvmCashBox> list = DBCommon.Instance.SetTModelValue<Para4043TvmCashBox>(dt);

        public Para4043tvmcashboxUpdate()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            DataTable dt = this.Tag as DataTable;
            List<Para4043TvmCashBox> list = DBCommon.Instance.SetTModelValue<Para4043TvmCashBox>(dt);
            //this.lt=list;
            //this.para4043 = list[0];  
            txtSellMaxPaperAmount.Text = list[0].sell_max_paper_amount;
            txtSellMaxPaperAcount.Text=list[0].sell_max_paper_acount;
            txtValueAddMaxPaperAmount.Text=list[0].value_add_max_paper_amount;
            txtValueAddMaxPaperTotal.Text=list[0].value_add_max_paper_total;
            txtMaxCoinAmount.Text=list[0].max_coin_amount;
            txtCoinRechargeMaxAmount.Text=list[0].coin_recharge_max_amount;
            txtMaxPaperChargeAmount.Text=list[0].max_paper_charge_amount;
            txtMaxPaperRechareTotal.Text=list[0].max_paper_rechare_total;
            txtTransactionCancelTime.Text=list[0].transaction_cancel_time;
            txtStandbyCutoverTime.Text=list[0].standby_cutover_time;
            txtPaperMoneyBoxNfullAmount.Text=list[0].paper_money_box_nfull_amount;
            txtPaperMoneyBoxFullAmount.Text=list[0].paper_money_box_full_amount;
            txtCoinRecycleNearFullAmount.Text=list[0].coin_recycle_near_full_amount;
            txtCoinRecycleFullAmount.Text=list[0].coin_recycle_full_amount;
            txtCoinRechargeNearEmpty.Text=list[0].coin_recharge_near_empty;
            txtMinTvmTickAmount.Text=list[0].min_tvm_tick_amount;
            txtPaperRechargeNearEmpty.Text = list[0].paper_recharge_near_empty;

           
            this.cmbCashChange.Items.Add("允许");
            this.cmbCashChange.Items.Add("不允许");

            this.cmbSellTick.Items.Add("允许");
            this.cmbSellTick.Items.Add("不允许");

            this.cmbStandby.Items.Add("允许");
            this.cmbStandby.Items.Add("不允许");

            cmbCashChange.Text = list[0].sell_paper_recharge == "1" ? "允许" : "不允许";
            cmbSellTick.Text = list[0].no_coin_sell_allowed == "1" ? "允许" : "不允许";
            cmbStandby.Text = list[0].display_standby_allowed == "1" ? "允许" : "不允许";
                
                           

            //base.InitControls();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
           Para4043TvmCashBox temp= DBCommon.Instance.GetModelValue<Para4043TvmCashBox>(string.Format("select * from para_4043_tvm_cash_box t where t.para_version='-1'"));

           para4043 = temp;

            para4043.sell_max_paper_amount = txtSellMaxPaperAmount.Text;
            para4043.sell_max_paper_acount = txtSellMaxPaperAcount.Text;
            para4043.value_add_max_paper_amount = txtValueAddMaxPaperAmount.Text;
            para4043.value_add_max_paper_total = txtValueAddMaxPaperTotal.Text;
            para4043.max_coin_amount = txtMaxCoinAmount.Text;
            para4043.coin_recharge_max_amount = txtCoinRechargeMaxAmount.Text;
            para4043.max_paper_charge_amount = txtMaxPaperChargeAmount.Text;
            para4043.max_paper_rechare_total = txtMaxPaperRechareTotal.Text;
            para4043.transaction_cancel_time = txtTransactionCancelTime.Text;
            para4043.standby_cutover_time = txtStandbyCutoverTime.Text;
            para4043.paper_money_box_nfull_amount = txtPaperMoneyBoxNfullAmount.Text;
            para4043.paper_money_box_full_amount = txtPaperMoneyBoxFullAmount.Text;
            para4043.coin_recycle_near_full_amount = txtCoinRecycleNearFullAmount.Text;
            para4043.coin_recycle_full_amount = txtCoinRecycleFullAmount.Text;
            para4043.coin_recharge_near_empty = txtCoinRechargeNearEmpty.Text;
            para4043.min_tvm_tick_amount = txtMinTvmTickAmount.Text;
            para4043.paper_recharge_near_empty = txtPaperRechargeNearEmpty.Text;

            para4043.sell_paper_recharge = cmbCashChange.Text == "不允许" ? "0" : "1";
            para4043.no_coin_sell_allowed = cmbSellTick.Text == "不允许" ? "0" : "1";
            para4043.display_standby_allowed = cmbStandby.Text == "不允许" ? "0" : "1";

            int res = 0;
            res = DBCommon.Instance.UpdateTable(para4043, "para_4043_tvm_cash_box",
             new KeyValuePair<string, string>("para_version", "-1"));

            if (res == 1)
            {
                Wrapper.ShowDialog("TVM钱箱参数信息更新成功。");
                //DataSourceManager.NotfiyDataSourceChange("para_4043_tvm_cash_box");
                return ;
            }
            else
            {
                Wrapper.ShowDialog("TVM钱箱参数信息更新失败。");
                return ;
            }
           
            //para4043.DoAction(list);

            

        }
    }
}
