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

namespace AFC.WS.UI.UIPage.CashManager
{
    using AFC.BOM2.UIController;
    using Visifire.Charts;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.Config;
    using AFC.WS.UI.CommonControls;

    /// <summary>
    /// cashStorePieQuery.xaml 的交互逻辑
    /// </summary>
    public partial class CashStorePieQuery : UserControlBase
    {
        public CashStorePieQuery()
        {
            InitializeComponent();
        }

        string stationId = "";
        /// <summary>
        /// 在库SQL
        /// </summary>
        private readonly string cash_store_info = "select sum(csi.currency_num) from cash_storage_info csi where csi.currency_code='{0}' and csi.station_id='{1}'";

        /// <summary>
        /// 在钱箱SQL
        /// </summary>
        private readonly string cash_box_stroe = "select sum(cbsi.currency_num) from cash_box_status_info cbsi where cbsi.currency_code='{0}' and cbsi.box_position!='01' and cbsi.station_id='{1}'";

        /// <summary>
        /// 在人SQL
        /// </summary>
        private readonly string cash_in_operator_store = "select sum(cioi.cash_in_hand) from cash_in_operator_info cioi where cioi.currency_code='{0}' and cioi.station_id='{1}'";


        public override void InitControls()
        {

            List<BasiMoneyTypeInfo> cashManaList = BuinessRule.GetInstace().GetBasiMoneyTypeInfo();
            this.cmbcashType.ItemsSource = cashManaList;
            this.cmbcashType.DisplayMemberPath = "currency_name";
            this.cmbcashType.SelectionChanged += new SelectionChangedEventHandler(cmbcashType_SelectionChanged);
            try
            {
                Wrapper.FullComboBox<BasiStationInfo>(this.StationName, BuinessRule.GetInstace().GetAllStationInfo(), "station_cn_name", "station_id", false, false);
             }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }

            stationId = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_id;
            Wrapper.ComboBoxSelectedItem(this.StationName, stationId);
            if (this.StationName.Text == "" || this.StationName.Text == null)
            {
                this.StationName.SelectedIndex = 0;
            }
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                this.StationName.IsEnabled = false;
            }
            else
            {
                this.StationName.IsEnabled = true;
            }

            
            //base.InitControls();
        }

        private void cmbcashType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.myChart.Series.Clear();

            BasiMoneyTypeInfo cb = this.cmbcashType.SelectedItem as BasiMoneyTypeInfo;
           if (cb == null)
               return;
           if (this.StationName.Text == "" || this.StationName.Text == null)
           {
               MessageDialog.Show("请选择车站！", "确定", MessageBoxIcon.Information, MessageBoxButtons.Ok);
               return;
           }
           string inStoreCmd = string.Format(this.cash_store_info, cb.currency_code, BuinessRule.GetInstace().GetStationInfoByName(this.StationName.Text).station_id.ToString());
           string inOperatorCmd = string.Format(this.cash_in_operator_store, cb.currency_code, BuinessRule.GetInstace().GetStationInfoByName(this.StationName.Text).station_id.ToString());
           string incashBoxCmd = string.Format(this.cash_box_stroe, cb.currency_code, BuinessRule.GetInstace().GetStationInfoByName(this.StationName.Text).station_id.ToString());

           DataSeries ds = new DataSeries();
           ds.Name = "库存分析";
           ds.RenderAs = RenderAs.Pie;
           ds.XValueType = ChartValueTypes.Auto;
           DataPoint dp = new DataPoint();
           dp.AxisXLabel = "票务室库存";
           dp.YValue =DBCommon.Instance.GetDatatable(inStoreCmd).Rows[0][0].ToString().ConvertNumberStringToUint() ;
           ds.DataPoints.Add(dp);


           DataPoint dp1 = new DataPoint();
           dp1.AxisXLabel = "钱箱库存";
           dp1.YValue = DBCommon.Instance.GetDatatable(incashBoxCmd).Rows[0][0].ToString().ConvertNumberStringToUint();
           ds.DataPoints.Add(dp1);


           DataPoint dp2 = new DataPoint();
           dp2.AxisXLabel = "操作员手中库存";
           dp2.YValue = DBCommon.Instance.GetDatatable(inOperatorCmd).Rows[0][0].ToString().ConvertNumberStringToUint();
           ds.DataPoints.Add(dp2);

           if (string.Equals(dp.YValue.ToString(), "0") && string.Equals(dp1.YValue.ToString(), "0") && string.Equals(dp2.YValue.ToString(), "0"))
           {
               Wrapper.ShowDialog(cb.currency_name.ToString() + "货币类型票务室、钱箱、操作员手中库存均为0。");
           }
           this.myChart.Series.Add(ds);
        }

        public override void UnLoadControls()
        {
            //base.UnLoadControls();
        }


    }
}
