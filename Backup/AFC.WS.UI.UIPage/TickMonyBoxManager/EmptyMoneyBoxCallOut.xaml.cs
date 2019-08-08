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
using System.Collections.ObjectModel;
using AFC.WS.UI.CommonControls;
using AFC.WS.Model.DB;
using AFC.WS.BR.TickMonyBoxManager;
using AFC.WS.ModelView.Convertors;
using AFC.WS.UI.Common;
using AFC.WS.ModelView.Actions.TickMonyBoxManager;
using AFC.WS.BR;
using System.Data;

namespace AFC.WS.UI.UIPage.TickMonyBoxManager
{
    /// <summary>
    /// EmptyMoneyBoxCallOut.xaml 的交互逻辑
    /// </summary>
    public partial class EmptyMoneyBoxCallOut : UserControlBase
    {
        List<string> moneyBoxList = new List<string>();

        public EmptyMoneyBoxCallOut()
        {
            InitializeComponent();
            this.MoneyBoxOut.OnOKButtonClicked += new RelactionContol.FunctionCliecked(OnOKButtonClicked);
        }

        public override void InitControls()
        {

            try
            {
                this.MoneyBoxOut.SetCurrentLabel("在库钱箱列表");
                this.MoneyBoxOut.SetGroupHeader("钱箱列表");
                this.MoneyBoxOut.SetLeftLabel("调出钱箱列表");

                this.MoneyBoxOut.SetCheckBoxContent("打印单据");
                this.MoneyBoxOut.SetContentVisable(Visibility.Visible);
                this.MoneyBoxOut.BindingCurrent(InitAllMoneyBox());
                string lineId = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                string curStationCode = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                List<BasiStationInfo> stationList = BuinessRule.GetInstace().GetAllStationAndLCInfo(lineId);
                stationList.Remove(stationList.Single(temp => temp.station_id.Equals(curStationCode)));
                Wrapper.FullComboBox(this.comStationID, stationList, "station_cn_name", "station_id", true);
                this.MoneyBoxOut.BindingLeft(new ObservableCollection<Data>());
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
            }
        }

        private void OnOKButtonClicked(object sender, RelactionEventArgs e)
        {
            ResultStatus status = null;
            for (int i = 0; i < e.left.Count; i++)
            {
                moneyBoxList.Add(e.left[i].ID);
            }
            List<QueryCondition> list = new List<QueryCondition>();
            list.Add(new QueryCondition { bindingData = "moneyBoxID", value = moneyBoxList });

            IAction emptyOut = new EmptyMoneyBoxOutAction();
            if (emptyOut.CheckValid(list))
            {
                status = emptyOut.DoAction(list);
                if (status != null && status.resultCode == 0)
                {

                    if (this.MoneyBoxOut.GetCheckBoxIsChecked)
                    {
                        //打印报表
                        if (string.IsNullOrEmpty(Wrapper.GetComboBoxText(this.comStationID)))
                        {
                            MessageDialog.Show("请选择调出目的车站!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            return;
                        }
                        AFC.WS.UI.UIPage.CashManager.CrystalRptData rptData = new AFC.WS.UI.UIPage.CashManager.CrystalRptData();
                        Dictionary<string, string> dict = new Dictionary<string, string>();

                        dict.Add("ReportTitle", "钱箱调出");
                        dict.Add("RequestBatchNo", DateTime.Now.ToString("yyyyMMddHHmmss"));
                        dict.Add("OperatorID", BuinessRule.GetInstace().brConext.CurrentOperatorId);
                        dict.Add("RequestLocationID", BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name);
                        dict.Add("DispatchLocationID", Wrapper.GetComboBoxText(this.comStationID));
                        dict.Add("DispatchType", "调出");
                        dict.Add("BoxType", "钱箱类型");
                        dict.Add("BoxID", "钱箱编码");

                        string dispathWay = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name + "----->" + Wrapper.GetComboBoxText(this.comStationID);

                        DataTable dt = new DataTable();

                        dt.Columns.Add("TicketBoxType", typeof(string));
                        dt.Columns.Add("TicketBoxId", typeof(string));
                        dt.Columns.Add("DispatchWay", typeof(string));

                        TicketOrMoneyBoxIdConvetor convertToDecimal = new TicketOrMoneyBoxIdConvetor();


                        for (int i = 0; i < e.left.Count; i++)
                        {
                            int typeCode = e.left[i].ID.Substring(2, 2).ToHexNumberInt32();
                            dt.Rows.Add(new string[] { TickMonyBoxHelp.Instance.GetMoneyBoxType(typeCode), e.left[i].Text.ToString(), dispathWay });
                        }
                        rptData.ShowRptDialog(new AFC.WS.UI.UIPage.TicketBoxManager.CrystalTicketBoxInOutReport(), dict, dt);
                    }
                    this.MoneyBoxOut.BindingLeft(new ObservableCollection<Data>());
                    moneyBoxList.Clear();
                }
            }

        }


        public ObservableCollection<Data> InitAllMoneyBox()
        {
            List<CashBoxStatusInfo> moneyBoxList = TickMonyBoxHelp.Instance.GetAllMoneyOutInfo();
            TicketOrMoneyBoxIdConvetor convertToDecimal = new TicketOrMoneyBoxIdConvetor();
            ObservableCollection<Data> list = new ObservableCollection<Data>();
            if (moneyBoxList != null)
            {
                for (int i = 0; i < moneyBoxList.Count; i++)
                {
                    CashBoxStatusInfo info = (CashBoxStatusInfo)moneyBoxList[i];
                    list.Add(new Data { ID = info.money_box_id, Text = convertToDecimal.Convert(info.money_box_id, null, null, null).ToString() });
                }
            }
            return list;

        }
    }
}
