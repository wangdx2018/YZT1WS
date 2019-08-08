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

namespace AFC.WS.UI.UIPage.TicketBoxManager
{
    using AFC.BOM2.UIController;

    using AFC.WS.UI.Common;
    using System.Collections.ObjectModel;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.BR;
    using AFC.WS.Model.Const;
    using AFC.WS.BR.TickMonyBoxManager;
    using AFC.WS.ModelView.Actions.TicketBoxManager;
    using AFC.WS.ModelView.Convertors;
    using System.Data;
    using AFC.WS.BR.TickBoxManager;
    /// <summary>
    /// EmptyTickBoxCallOut.xaml 的交互逻辑
    /// </summary>
    public partial class EmptyTickBoxCallOut : UserControlBase
    {

        private string desStationName;

        public EmptyTickBoxCallOut()
        {
            InitializeComponent();
            this.TickOut.OnOKButtonClicked += new RelactionContol.FunctionCliecked(OnOKButtonClicked);
        }
        public override void InitControls()
        {

            this.TickOut.SetCurrentLabel("在库空票箱列表");
            this.TickOut.SetGroupHeader("票箱调出");
            this.TickOut.SetLeftLabel("调出票箱列表");
            this.TickOut.BindingCurrent(InitAllTickBox());
            this.TickOut.SetCheckBoxContent("打印单据");
            this.TickOut.SetContentVisable(Visibility.Visible);
            string lineId = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;

            List<BasiStationInfo> stationList = BuinessRule.GetInstace().GetAllStationAndLCInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
            if (stationList != null &&
               stationList.Count > 0)
            {
                try
                {
                    stationList.Remove(stationList.Single(temp => temp.station_id == SysConfig.GetSysConfig().LocalParamsConfig.StationCode));
                }
                catch (Exception e)
                {

                }
            }
            Wrapper.FullComboBox(this.comStationID, stationList, "station_cn_name", "station_id", true);
            this.TickOut.BindingLeft(new ObservableCollection<Data>()); 
        }

        private void OnOKButtonClicked(object sender, RelactionEventArgs e)
        {
            desStationName = Wrapper.GetComboBoxText(this.comStationID);
            //打印报表
            if (string.IsNullOrEmpty(desStationName))
            {
                MessageDialog.Show("请选择调出目的车站!", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return;
            }
            if (desStationName == BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name)
            {
                MessageDialog.Show("调出目的车站不能是本站!", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return;
            }

            List<string> tickBoxList = new List<string>();
            for (int i = 0; i < e.left.Count; i++)
            {
                tickBoxList.Add(e.left[i].ID);
            }
            List<QueryCondition> list = new List<QueryCondition>();
            list.Add(new QueryCondition { bindingData = "tickBoxID", value = tickBoxList });

            IAction emptyOut = new EmptyTickBoxCallOutAction();
            if (emptyOut.CheckValid(list))
            {
               ResultStatus result=emptyOut.DoAction(list);
               if (result!=null&&
                   result.resultCode == 0 &&
                   result.resultData.ToString() == "0")
               {
                   if (this.TickOut.GetCheckBoxIsChecked)
                   {
                       Print(e);
                   }
               }
            }
        }

        private void  Print(RelactionEventArgs e)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("ReportTitle", "票箱调出");
            dict.Add("RequestBatchNo", DateTime.Now.ToString("yyyyMMddHHmmss"));
            dict.Add("OperatorID", BuinessRule.GetInstace().brConext.CurrentOperatorId);
            dict.Add("DispatchLocationID", desStationName);
            dict.Add("RequestLocationID", 
                BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name);
            dict.Add("DispatchType", "空票箱调出");
            dict.Add("BoxType", "票箱类型");
            dict.Add("BoxID", "票箱编码");


            DataTable dt = new DataTable("tickBoxCallOut");
        
            dt.Columns.Add("TicketBoxType", typeof(string));
            dt.Columns.Add("TicketBoxId", typeof(string));
            dt.Columns.Add("DispatchWay", typeof(string));

        
            for (int i = 0; i < e.left.Count; i++)
            {
                dt.Rows.Add(GetTickBoxType(e.left[i].ID), e.left[i].Text, 
                    BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name + "-->" +
                   desStationName);
            }


            //AFC.WS.UI.UIPage.CashManager.CrystalRptData cryRptData = new AFC.WS.UI.UIPage.CashManager.CrystalRptData();
            //cryRptData.ShowRptDialog(new AFC.WS.UI.UIPage.TicketBoxManager.CrystalTicketBoxInOutReport(),
            //    dict, dt);
        }


        private string GetTickBoxType(string tickBoxId)
        {
            if (string.IsNullOrEmpty(tickBoxId))
                return "N/A";
            string type = tickBoxId.Substring(2,2);
            if (type == "01")
                return "发票箱";
            if (type == "02")
                return "废票箱";
            if (type == "03")
                return "回收箱";
            return "N/A";
        }

              //cb.Items.Add(CreateComboxItem("94","发票箱"));
              //cb.Items.Add(CreateComboxItem("95", "废票箱"));
              //cb.Items.Add(CreateComboxItem("96", "回收箱"));

        public ObservableCollection<Data> InitAllTickBox()
        {
            List<TickBoxStatusInfo> tickBoxList = BuinessRule.GetInstace().tickMan.GetAllTickOutInfo();
            TicketOrMoneyBoxIdConvetor convertToDecimal = new TicketOrMoneyBoxIdConvetor();
            ObservableCollection<Data> list = new ObservableCollection<Data>();
            if (tickBoxList != null)
            {
                for (int i = 0; i < tickBoxList.Count; i++)
                {
                    TickBoxStatusInfo info = (TickBoxStatusInfo)tickBoxList[i];
                    list.Add(new Data { ID = info.ticket_box_id, Text = convertToDecimal.Convert(info.ticket_box_id,null,null,null).ToString() });
                }
            }
            return list;

        }
    }
}
