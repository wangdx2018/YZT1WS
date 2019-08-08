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
    using System.IO;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.UI.Common;
    /// <summary>
    /// 设备监控的监视界面
    /// 
    /// 根据车站的变化更改布局图
    /// </summary>
    public partial class SLEDeviceMonitorControl : UserControlBase
    {

        /// <summary>
        /// 车站的布局图
        /// </summary>
        private Image myImage = new Image();


        private string currentStationId;

        public SLEDeviceMonitorControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 订阅同步消息，车站选择
        /// 和设备监控
        /// 车站选择，切换车站的Image图片。
        /// 设备监控是定时任务来更改图片的状态。
        /// </summary>
        public override void SubscribeSynMessage()
        {
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.DeviceMonitor, SynMessageType.Device_Station_Selected, HandleMode.Syn, true);
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.DeviceMonitor, AsynMessageType.DeviceMonitor, HandleMode.Asyn, true);
        }


        public override void InitControls()
        {
            //base.InitControls();
            if (!this.myCanvas.Children.Contains(this.myImage))
            {
                this.myCanvas.Children.Add(this.myImage);
            }
            BuinessRule.GetInstace().sleMonitor.imageCollection.init(@".\Config\MonitorImageCfg.xml");
        }


        public override void UnLoadControls()
        {
            BuinessRule.GetInstace().sleMonitor.ShutdownMonitorDevice();
            MessageManager.CancelAllSubscribeMessage(SynMessageType.Device_Station_Selected);
            MessageManager.CancelAllSubscribeMessage(AsynMessageType.DeviceMonitor);
            this.myCanvas.Children.Remove(this.myImage);
           
        }

        /// <summary>
        /// 消息处理，处理
        /// </summary>
        /// <param name="msg">TreeView发送过来的车站选择消息</param>
        public override void HandleSynMessage(Message msg)
        {
            switch (msg.MessageType)
            {
                case SynMessageType.Device_Station_Selected:
                    this.HandleStationSwitch(msg);
                    BuinessRule.GetInstace().sleMonitor.ShutdownMonitorDevice();
                    BuinessRule.GetInstace().sleMonitor.StartMonitorDevice(msg.Content.ToString());
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 定时刷新设备状态表信息
        /// </summary>
        /// <param name="msg">消息体</param>
        public override void HandleAsynMessageForUI(Message msg)
        {
            if (msg.MessageType == AsynMessageType.DeviceMonitor)
            {
                SetupAllDevice(currentStationId);
            }
        }

        /// <summary>
        /// 处理车站选择变化通知
        /// </summary>
        /// <param name="msg">同步消息</param>
        private void HandleStationSwitch(Message msg)
        {
            this.myCanvas.Children.Clear();
            this.myCanvas.Children.Add(this.myImage);

            //todo: load layout image
            currentStationId = msg.Content.ToString();
            string imageFilePath = CreateImagePath(currentStationId);
            if (string.IsNullOrEmpty(imageFilePath))
                return;
            this.SetImageSource(this.myImage, imageFilePath);//todo:Change Image here
            this.myCanvas.Width = this.myImage.Width;
            this.myCanvas.Height = this.myImage.Height;
         //   this.myCanvas.Children.Add(this.myImage);
            SetupAllDevice(currentStationId);
        }

        /// <summary>
        /// 车站选择后更换图片
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="path"></param>
        private void SetImageSource(Image image, string path)
        {
            string dir = Environment.CurrentDirectory;
            string imageAddress = dir + "\\" + path;

            if (File.Exists(imageAddress))
            {
                image.Stretch = Stretch.Uniform;
                BitmapImage genBmpImage = new BitmapImage();
                genBmpImage.BeginInit();
                genBmpImage.UriSource = new Uri(imageAddress);
                genBmpImage.EndInit();
                System.Drawing.Image imageSize = System.Drawing.Image.FromFile(imageAddress);
                image.Width = imageSize.Width;
                image.Height = imageSize.Height;
                image.Source = genBmpImage;
            }
        }

        /// <summary>
        /// 创建Image图片的路径
        /// </summary>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回图片的Image的路径</returns>
        private string CreateImagePath(string stationId)
        {
            try
            {
                return BuinessRule.GetInstace().sleMonitor.imageCollection.sationLayoutDict[stationId].Path;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        ///  将图形添加到Layout布局中
        /// </summary>
        /// <param name="uc">用户控件</param>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        private void SetupControl(UserControl uc, double x, double y)
        {
            if (x == 0 && y == 0)
                return;
            bool flag = false;
            int index=0;
            for (int i = 0; i < this.myCanvas.Children.Count; i++)
            {
                if (Canvas.GetLeft(this.myCanvas.Children[i]) == x &&
                    Canvas.GetTop(this.myCanvas.Children[i]) == y)
                {
                    flag = true;
                    index=i;
                }
            }
            if (flag)
            {
                this.myCanvas.Children.RemoveAt(index);
            }
         
                this.myCanvas.Children.Add(uc);
                Canvas.SetLeft(uc, x);
                Canvas.SetTop(uc, y);
            
        }

      /// <summary>
      /// 将各个设备的的图片添加到Layout布局图中
      /// </summary>
      /// <param name="stationId">车站ID</param>
        private void SetupAllDevice(string stationId)
        {
            List<BasiDevInfo> list = BuinessRule.GetInstace().GetBasiDevInfo(stationId);
            List<DevRunStatusInfo> devRunStatusList=BuinessRule.GetInstace().GetAllDevRunStatusInfo(stationId);
            if (list == null || list.Count == 0)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                MonitorImageButton btn = new MonitorImageButton();
                btn.SetDevInfo(list[i]);
               
                try
                {
                    DevRunStatusInfo info = devRunStatusList.Single(temp => temp.device_id.Equals(list[i].device_id));
                    if (info != null)
                    {
                        btn.SetDevRunStatus(info);
                    }
                    btn.Init();
                    SetupControl(btn, (double)list[i].honri_rela_distance, (double)list[i].verti_rela_distance);
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex.Message);
                }
            }
        }
    }
}
