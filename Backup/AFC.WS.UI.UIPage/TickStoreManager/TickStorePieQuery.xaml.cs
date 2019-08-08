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
    using Visifire.Charts;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.UI.Common;
    using AFC.WS.UI.Config;

    /// <summary>
    /// TickStorePieQuery.xaml 的交互逻辑
    /// </summary>
    public partial class TickStorePieQuery : UserControlBase
    {
        public TickStorePieQuery()
        {
            InitializeComponent();
        }

        string stationId = "";
        /// <summary>
        /// 在库SQL
        /// </summary>
        private readonly string tick_store_info = "select sum(t1.in_store_num) from tick_storage_info t1 where t1.tick_mana_type='{0}' and t1.station_id='{1}'";

        /// <summary>
        /// 在票箱SQL
        /// </summary>
        private readonly string tick_box_stroe = "select sum(tbsi.tickets_num) from tick_box_status_info tbsi where tbsi.tick_mana_type='{0}' and tbsi.station_id='{1}'";

        /// <summary>
        /// 在人SQL
        /// </summary>
        private readonly string tick_in_operator_store = "select sum(tioi.ticket_in_hand) from tick_in_operator_info tioi where tioi.tick_mana_type='{0}' and tioi.station_id='{1}'";


        public override void InitControls()
        {

            List<BasiTickManaTypeInfo> tickManaList = BuinessRule.GetInstace().GetBasiTickManaTypeInfo();
            this.cmbTickType.ItemsSource = tickManaList;
            this.cmbTickType.DisplayMemberPath = "tick_mana_type_name";
            this.cmbTickType.SelectionChanged += new SelectionChangedEventHandler(cmbTickType_SelectionChanged);
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

        private void cmbTickType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.myChart.Series.Clear();

           BasiTickManaTypeInfo cb=this.cmbTickType.SelectedItem as BasiTickManaTypeInfo;
           if (cb == null)
               return;
           string inStoreCmd = string.Format(this.tick_store_info, cb.tick_mana_type, BuinessRule.GetInstace().GetStationInfoByName(this.StationName.Text).station_id.ToString());
           string inOperatorCmd = string.Format(this.tick_in_operator_store, cb.tick_mana_type, BuinessRule.GetInstace().GetStationInfoByName(this.StationName.Text).station_id.ToString());
           string inTickBoxCmd = string.Format(this.tick_box_stroe, cb.tick_mana_type, BuinessRule.GetInstace().GetStationInfoByName(this.StationName.Text).station_id.ToString());

           DataSeries ds = new DataSeries();
           ds.Name = "库存分析";
           ds.RenderAs = RenderAs.Pie;
           ds.XValueType = ChartValueTypes.Auto;
           DataPoint dp = new DataPoint();
           dp.AxisXLabel = "票务室库存";
           dp.YValue =DBCommon.Instance.GetDatatable(inStoreCmd).Rows[0][0].ToString().ConvertNumberStringToUint() ;
           ds.DataPoints.Add(dp);


           DataPoint dp1 = new DataPoint();
           dp1.AxisXLabel = "票箱库存";
           dp1.YValue = DBCommon.Instance.GetDatatable(inTickBoxCmd).Rows[0][0].ToString().ConvertNumberStringToUint();
           ds.DataPoints.Add(dp1);


           DataPoint dp2 = new DataPoint();
           dp2.AxisXLabel = "操作员手中库存";
           dp2.YValue = DBCommon.Instance.GetDatatable(inOperatorCmd).Rows[0][0].ToString().ConvertNumberStringToUint();
           ds.DataPoints.Add(dp2);
           string ll = dp2.YValue.ToString();

           if (string.Equals(dp.YValue.ToString(), "0") && string.Equals(dp1.YValue.ToString(), "0") && string.Equals(dp2.YValue.ToString(), "0"))
           {
               Wrapper.ShowDialog(cb.tick_mana_type_name.ToString()+"票种票务室、票箱、操作员手中库存均为0。");
           }
           this.myChart.Series.Add(ds);
        }

        public override void UnLoadControls()
        {
            //base.UnLoadControls();
        }


    }
}
