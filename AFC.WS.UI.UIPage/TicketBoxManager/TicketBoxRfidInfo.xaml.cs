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
    using AFC.WS.UI.BR;
    using AFC.WS.BR;
    /// <summary>
    ///负责人：王冬欣  最后修改日期：20091205
    ///
    /// 票箱RFID信息
    /// </summary>
    public partial class TicketBoxRfidInfo : UserControl
    {
        public TicketBoxRfidInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置票箱的RFID信息
        /// </summary>
        /// <param name="info">票箱RFID信息</param>
        public void SetTicketBoxRfidInfo(AFC.WS.UI.RfidRW.RfidTicketboxInfo info)
        {
            if (info == null)
                return;
            try
            {
                 AFC.WS.ModelView.Convertors.TicketOrMoneyBoxIdConvetor convetor=new AFC.WS.ModelView.Convertors.TicketOrMoneyBoxIdConvetor();
                this.labTickBoxIdValue.Content =convetor.Convert (info.ticketboxId,null,null,null);//todo:need convert
                this.labTickBoxStationValue.Content =BuinessRule.GetInstace().GetStationInfoById(info.stationCode).station_cn_name; //todo: need convert
                this.labTickBoxTypeValue.Content = GetTickType(info.ticketboxId);//todo: need convert
                this.labUpdateTimeValue.Content = info.LastOpeatorTime;
                this.labStoreTypeValue.Content =GetTickStoreType(info.CardIssueId);//todo:need convert
                this.labSetupStatusValue.Content =GetOperatorStatus(info.operatorTicketboxStatus);//todo need convert
                this.labCurrentNumValue.Content = info.ticketNumber;
                this.labSetupLocationValue.Content = GetTickBoxSetupLocation(info.setupLoaction);//todo:need convert
                this.labLocationValue.Content =GetLocationStatus(info.ticketboxLoactionStatus);//todo need convert
                this.labDeviceIdValue.Content = info.deviceId;
            }
            catch (Exception ex)
            {
                //todo:
            }

        }


        private string GetTickType(string tickBoxId)
        {
            if (string.IsNullOrEmpty(tickBoxId))
                return string.Empty;
            string tickType = tickBoxId.Substring(2, 2);
            if (tickType == "01")
                return "发票箱";
            if (tickType == "03")
                return "回收箱";
            else
                return "废票箱";
        }


        private string GetTickStoreType(int tickStoreTypeId)
        {
          
          AFC.WS.Model.DB.BasiTickManaTypeInfo  value= BuinessRule.GetInstace().GetBasiTickManaTypeInfoById(tickStoreTypeId.ToString("x2"));
          if (value != null && !string.IsNullOrEmpty(value.tick_mana_type))
              return value.tick_mana_type_name;
          else
              return "N/A";
        }


        private string GetTickBoxSetupLocation(byte value)
        {
            if (value == 0xff)
            { return "N/A位置"; }
            else
            return "票箱" + value.ToString() + "位置";
        }


        private string GetOperatorStatus(byte value)
        {
            if (value == 1)
                return "正常安装";
            if (value == 2)
                return "非法安装";
            if (value == 3)
                return "正常卸下";
            if (value == 4)
                return "非法卸下";
            return "N/A状态";
        }


        private string GetLocationStatus(byte value)
        {
            if (value == 1)
                return "在库";
            if (value == 2)
                return "在操作员手中";
            if (value == 3)
                return "在设备";
            if (value == 4)
                return "调出";
            return "N/A";
        }

        /// <summary>
        ///清空票箱信息
        /// </summary>
        public void ClearRfidInfo()
        {
            this.labCurrentNumValue.Content = string.Empty;
            this.labDeviceIdValue.Content = string.Empty;
            this.labLocationValue.Content = string.Empty;
            this.labSetupStatusValue.Content = string.Empty;
            this.labTickBoxStationValue.Content = string.Empty;
            this.labUpdateTimeValue.Content = string.Empty;
            this.labCurrentNumValue.Content = string.Empty;
            this.labTickBoxIdValue.Content = string.Empty;
            this.labStoreTypeValue.Content = string.Empty;
            this.labTickBoxTypeValue.Content = string.Empty;
            this.labSetupLocationValue.Content = string.Empty;
        }
    }
}
