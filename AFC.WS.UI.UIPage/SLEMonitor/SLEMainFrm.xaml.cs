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
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.Const;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    /// <summary>
    /// added by wangdx 20110510
    /// 
    /// </summary>
    public partial class SLEMainFrm : UserControlBase
    {
        public SLEMainFrm()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            this.sleMonitor.InitControls();
            this.sleMonitor.SubscribeAsynMessage();
            this.sleMonitor.SubscribeSynMessage();
            this.tv.InitControls();
            this.tv.SubscribeSynMessage();
            this.tv.SubscribeAsynMessage();
            this.devStatusDetail.InitControls();

            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                this.tv.Visibility = Visibility.Collapsed;
                Message msg = new Message();
                msg.MessageType = SynMessageType.Device_Station_Selected;
                msg.Content = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                MessageManager.SendMessasge(msg);
                this.rootLayout.ColumnDefinitions[0].Width = new GridLength(0, GridUnitType.Pixel);
              
            }
            
          
        }

        public override void UnLoadControls()
        {
            this.sleMonitor.UnLoadControls();
            this.sleMonitor.CancleSubscribeSynMessage();
            this.tv.UnLoadControls();
            this.tv.CancleSubscribeSynMessage();
            this.devStatusDetail.UnLoadControls();
          
        }

        public override void SubscribeSynMessage()
        {
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.DeviceMonitor, SynMessageType.Device_Station_Selected, HandleMode.Syn, true);

          
        }

        public override void HandleSynMessage(Message msg)
        {
            switch (msg.MessageType)
            {
                case SynMessageType.Device_Station_Selected:
                    string stationId = msg.Content.ToString();
                   
                    break;
                default:
                    break;
            }
        }






    }
}
