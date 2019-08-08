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

namespace AFC.WS.UI.UIPage.SLEMonitor
{
    using AFC.BOM2.UIController;
    using AFC.WS.BR;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.Common;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.Const;
    

    /// <summary>
    /// StationTreeViewControl.xaml 的交互逻辑
    /// </summary>
    public partial class StationTreeViewControl : UserControlBase
    {
        public StationTreeViewControl()
        {
            InitializeComponent();
            //InitControls();
        }

        public override void InitControls()
        {
            this.trRoot.Items.Clear();
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("LCWS"))
            {
                this.trRoot.Header=SysConfig.GetSysConfig().LocalParamsConfig.LineCode.ToUInt32().ToString()+"号线";
                string lineId = SysConfig.GetSysConfig().LocalParamsConfig.LineCode;
                List<BasiStationInfo> list = BuinessRule.GetInstace().GetAllStationInfo(lineId);
                List<RunModeStatus> stationRunModeStatus = BuinessRule.GetInstace().GetStationRunModeInfos();

                foreach (var temp in list)
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = temp.station_cn_name;
                    item.DataContext = temp;
                    item.Tag = temp.station_id;
                    item.Header = temp.station_cn_name;
                    item.MouseDoubleClick += new MouseButtonEventHandler(item_MouseDoubleClick);
                    this.trRoot.Items.Add(item);
                }
            }
            else
            {
                BasiStationInfo stationInfo = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
                if (stationInfo == null)
                    return;
                TreeViewItem item = new TreeViewItem();
                item.Header = stationInfo;
                item.DataContext = stationInfo;
                item.Header =stationInfo.station_cn_name;
                item.Tag = stationInfo.station_id; 
                item.MouseDoubleClick += new MouseButtonEventHandler(item_MouseDoubleClick);
                this.trRoot.Items.Add(item);
            }

            (this.trRoot.Items[0] as TreeViewItem).IsSelected = true;
            Message msg = new Message();
            msg.MessageType = SynMessageType.Device_Station_Selected;
            msg.Content =(this.trRoot.Items[0] as TreeViewItem).Tag;
            MessageManager.SendMessasge(msg);
        }



        public Style GetStationRunStyle(string stationId,List<RunModeStatus> list)
        {
            Style style = null;
            try
            {
                string runModeCode = list.Single(temp => temp.station_id.Equals(stationId)).run_mode_code;
             
                switch (runModeCode)
                {
                    case "0":
                        style = this.FindResource("trNoraml") as Style;
                        return style;
                    case "1":
                        style = this.FindResource("trTrainFailed") as Style;
                        return style;
                    case "2":
                    case "4":
                    case "8":
                    case "16":
                    case "32":
                    case "64":
                        style = this.FindResource("trDownLevel") as Style;
                        return style;
                    case "128":
                        style = this.FindResource("trEmergency") as Style;
                        return style;
                    case "255":
                        style = this.FindResource("trUnKonwn") as Style;
                        return style;



                }
                style = this.FindResource("trUnKonwn") as Style;
                return style;
            }
            catch (Exception ex)
            {
                style = this.FindResource("trUnKonwn") as Style;
                return style;
              //  return null; 
            }
        }


        private void item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (sender as TreeViewItem);
            Message msg = new Message();
            msg.MessageType = SynMessageType.Device_Station_Selected;
            msg.Content = item.Tag;
            MessageManager.SendMessasge(msg);
        }

        public override void UnLoadControls()
        {
            base.UnLoadControls();
        }

    }

    public class StationImageSource
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        private string imageRunStatusPath;

        public string ImageRunStatusPath
        {
            get { return imageRunStatusPath; }
            set { imageRunStatusPath = value; }
        }

        /// <summary>
        /// 车站中文名
        /// </summary>
        private string stationCnName;

        public string StationCnName
        {
            get { return stationCnName; }
            set { stationCnName = value; }
        }

        /// <summary>
        /// 车站ID
        /// </summary>
        private string stationId;

        public string StationId
        {
            get { return stationId; }
            set { stationId = value; }
        }

        /// <summary>
        /// 车站运营状态
        /// </summary>
        private string stationRunStatus;

        public string StationRunStatus
        {
            get { return stationRunStatus; }
            set { stationRunStatus = value; }
        }

        /// <summary>
        /// 车站运营状态提示
        /// </summary>
        private string stationRunStatusToolTip;

        public string StationRunStatusToolTip
        {
            get { return stationRunStatusToolTip; }
            set { stationRunStatusToolTip = value; }
        }
    }


 
}
