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
using System.IO;

namespace AFC.WS.UI.UIPage.SLEMonitor
{
    using AFC.WS.Model.DB;
    using AFC.WS.UI.Common;
    using AFC.WS.Model.Const;
    using AFC.WS.BR;
    using AFC.WS.BR.SLEMonitorManager;
    using AFC.BOM2.MessageDispacher;
    using System.Data;
    using System.Windows.Media.Animation;
    
    /// <summary>
    /// MonitorImageButton.xaml 的交互逻辑
    /// </summary>
    public partial class MonitorImageButton : UserControl,IMessageHandler
    {
        /// <summary>
        /// 进站
        /// </summary>
        public const string ENTER_GATE = "00";

        /// <summary>
        /// 出站
        /// </summary>
        public const string EXIT_GATE = "01";

        /// <summary>
        /// 双向
        /// </summary>
        public const string DOUBLE_GATE = "02";

        /// <summary>
        /// AG箭头朝向左
        /// </summary>
        public const string DIR_LEFT = "07";

        /// <summary>
        /// AG箭头朝向右
        /// </summary>
        public const string DIR_RIGHT = "03";

        /// <summary>
        /// AG箭头朝向双向
        /// </summary>
        public const string DIR_H_DOUBLE = "02";

        /// <summary>
        /// AG箭头向上
        /// </summary>
        public const string DIR_TOP = "01";

        /// <summary>
        /// AG箭头向下
        /// </summary>
        public const string DIR_DOWN = "05";

        /// <summary>
        /// AG箭头 上下都有
        /// </summary>
        public const string DIR_V_DOUBLE = "06";

        /// <summary>
        /// 状态的宽度
        /// </summary>
        public const double STATUS_IMAGE_WIDTH = 30;

        /// <summary>
        /// 状态的高度
        /// </summary>
        public const double STAUS_IMAGE_HEIGHT = 30;

        /// <summary>
        /// 设备图标
        /// </summary>
        private Image devImage = new Image();

        /// <summary>
        /// 票箱状态
        /// </summary>
        private Image tickBoxStatus = new Image();

        /// <summary>
        /// 票箱状态配置
        /// </summary>
        private ImageCfg tickBoxStatusCfg = new ImageCfg();


        /// <summary>
        /// 设备状态图标
        /// </summary>
        private Image devStatusImage = new Image();

        /// <summary>
        /// 图片配置信息
        /// </summary>
        private ImageCfg devImageCfg = new ImageCfg();

        /// <summary>
        /// 当图片选中后更换的图片
        /// </summary>
        private ImageCfg devImageSelectedCfg = new ImageCfg();

        /// <summary>
        /// 设备状态信息图片配置信息
        /// </summary>
        private ImageCfg devRunStatusCfg = new ImageCfg();

        /// <summary>
        /// 设备基础信息表
        /// </summary>
        private BasiDevInfo basiDevInfo = null;

        /// <summary>
        /// 选择的车站信息
        /// </summary>
        private string stationId = null;

        /// <summary>
        /// 设备运行状态表
        /// </summary>
        private DevRunStatusInfo devRunStatusInfo = null;


        private Storyboard sb = new Storyboard();


        public MonitorImageButton()
        {
            InitializeComponent();
            this.devImage.MouseEnter += new MouseEventHandler(myImage_MouseEnter);
            this.devImage.MouseLeave += new MouseEventHandler(myImage_MouseLeave);
            this.devImage.MouseLeftButtonUp += new MouseButtonEventHandler(myImage_MouseLeftButtonUp);
        }

        private void myImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Message msg = new Message();
            msg.MessageType = SynMessageType.Dev_Image_Selected;
            msg.Content = this.basiDevInfo;
            msg.MessageParam = this.stationId;
            MessageManager.SendMessasge(msg);
        }

        /// <summary>
        /// 设置设备信息的基本信息
        /// </summary>
        /// <param name="basiDevInfo">设备基本配置信息</param>
        public void SetupBasiDevInfo(BasiDevInfo basiDevInfo)
        {
            this.basiDevInfo = basiDevInfo;
        }

        /// <summary>
        /// 设置车站ID
        /// </summary>
        /// <param name="stationId">设置车站ID</param>
        public void SetStationId(string stationId)
        {
            this.stationId = stationId;
        }


        /// <summary>
        /// 得到服务模式
        /// </summary>
        /// <returns>服务模式的值</returns>
        private string GetServerMode()
        {
            string cmd=string.Format("select t.status_value from dev_run_status_info t where t.device_id='{0}' and t.status_id='{1}'", basiDevInfo.device_id,CSSStatusIDInfo.SERVER_MODE);
            DataTable dt=DBCommon.Instance.GetDatatable(cmd);
            if (dt.Rows.Count == 0)
                return string.Empty;
            else
                return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// 得到连结状态
        /// 2013年5月22日根据天津业主要求在设备监控界面中增加设备连接状态的监控
        /// </summary>
        /// <returns>服务模式的值</returns>
        private string GetConnectStatus()
        {
            string cmd = string.Format("select t.status_value from dev_run_status_info t where t.device_id='{0}' and t.status_id='{1}'", basiDevInfo.device_id, CSSStatusIDInfo.ON_LINE_STATUS);
            DataTable dt = DBCommon.Instance.GetDatatable(cmd);
            if (dt.Rows.Count == 0)
                return string.Empty;
            else
                return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// 得到通道模式
        /// </summary>
        /// <returns>返回通道模式的数据</returns>
        private string GetDoorServer()
        {
            string cmd = string.Format("select t.status_value from dev_run_status_info t where t.device_id='{0}' and t.status_id='{1}'", basiDevInfo.device_id,CSSStatusIDInfo.AG_DEV_TYPE);
            DataTable dt = DBCommon.Instance.GetDatatable(cmd);
            if (dt.Rows.Count == 0)
                return string.Empty;
            else
                return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// 鼠标离开图形时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myImage_MouseLeave(object sender, MouseEventArgs e)
        {
            SetImageSource(this.devImage, this.GetImagePath(devImageCfg.KeyField, 0));
        }

        /// <summary>
        /// 当鼠标移动到图形中时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myImage_MouseEnter(object sender, MouseEventArgs e)
        {
            SetImageSource(this.devImage,
                this.GetImagePath(this.devImageSelectedCfg.KeyField, 1),
                this.devImage.Width+20,
                this.devImage.Height+20
           );
        }

        /// <summary>
        /// 设置设备整体运行状态
        /// </summary>
        /// <param name="info">设备运营状态</param>
        public void SetDevRunStatus(DevRunStatusInfo info)
        {
            this.devRunStatusInfo = info;
        }

        /// <summary>
        /// 设置设备基础信息
        /// </summary>
        /// <param name="info">设备基础信息</param>
        public void SetDevInfo(BasiDevInfo info)
        {
            this.basiDevInfo = info;
        }

        public void Init()
        {
            //2013年5月22日根据马晓春要求“通讯中断”或“关机”时，应用一个单独的图标来显示，因此传入的参数增加一个连结状态。
            if (this.basiDevInfo.device_type != DevType.DEV_AGM)
            {
                LoadDevImage(this.basiDevInfo.device_type,
                    GetServerMode(),GetConnectStatus());
            }
            else
            {
                LoadDevImage(this.basiDevInfo.device_type,
                 GetServerMode(), GetDoorServer(), GetConnectStatus());
            }
        }

      

        /// <summary>
        /// 加载设备图片
        /// </summary>
        /// <param name="devType">设备类型</param>
        /// <param name="runStatus">运营状态</param>
        /// <param name="agDir">如果是AG需要设置箭头方向
        /// <see cref="AGDIR,AFC.WS.Model.Const.AGDIR"/>
        /// </param>
        /// <returns>成功返回0，否则返回-1</returns>
        private int LoadDevImage(string devType, string runStatus,params string[] agDir)
        {
            try
            {
                if (devType != DevType.DEV_AGM)
                {
                    this.devImageCfg = BuinessRule.GetInstace().sleMonitor.
                        imageCollection.
                        deviceImageDict.Single(temp => temp.Key.Equals(devType)).Value;

                    this.devImageSelectedCfg = BuinessRule.GetInstace().sleMonitor.
                        imageCollection.
                        devImageSelectedDict.Single(temp => temp.Key.Equals(devType)).Value;
                }
                else
                {
                    this.devImageCfg = BuinessRule.GetInstace().sleMonitor.
                        imageCollection.
                        deviceImageDict.Single(temp => temp.Key.Equals(devType +CalAGImage(agDir[0]))).Value;

                    this.devImageSelectedCfg = BuinessRule.GetInstace().sleMonitor.
                        imageCollection.
                        devImageSelectedDict.Single(temp => temp.Key.Equals(devType +CalAGImage(agDir[0]))).Value;
                }

                SetImageSource(this.devImage, this.GetImagePath(devImageCfg.KeyField, 0));//add to grid
                this.devImage.ToolTip = this.basiDevInfo.device_id;

                #region 设置服务模式
                //2013年3月21日与马晓春讨论确定只保留“正常”、“警告”、“暂停”、“紧急”，其它归入“暂定”
                if (!string.IsNullOrEmpty(runStatus))
                {
                   switch (runStatus)
                   {
                        case "03":
                            runStatus = "02";
                            break;
                        case "04":
                            runStatus = "02";
                            break;
                        case "05":
                            runStatus = "02";
                            break;
                        case "06":
                            runStatus = "02";
                            break;
                        default:
                            break;
                    }
                }

                if (devType != DevType.DEV_AGM)
                {
                    if(agDir[0]=="00")
                        runStatus = "03";
                }
                else
                {
                    if (agDir[1] == "00")
                        runStatus = "03";
                }

                if (!string.IsNullOrEmpty(runStatus) &&
                    (runStatus != "00"&& runStatus!="01")) //todo: if dev status is normal ,then not load status image
                {
                    this.devRunStatusCfg = BuinessRule.GetInstace().sleMonitor.
                        imageCollection.
                        devRunStatusDict.Single(temp => temp.Key.Equals(runStatus)).Value; //get image cfg
                    this.devImage.ToolTip = this.basiDevInfo.device_id+" " +devRunStatusCfg.Content;
                    SetImageSource(this.devStatusImage,this.GetImagePath(this.devRunStatusCfg.KeyField, 2),
                        STATUS_IMAGE_WIDTH,
                        STAUS_IMAGE_HEIGHT);

                    sb = this.FindResource("imageShine") as Storyboard;
                    if (sb != null)
                    {
                        sb.RepeatBehavior = RepeatBehavior.Forever;
                        Storyboard.SetTarget(sb, this.devStatusImage);
                        Storyboard.SetTargetProperty(this.devStatusImage, new PropertyPath("Visibility"));
                        sb.Begin();
                    }
                    else
                    {
                        try
                        {
                            sb.Stop();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else
                {
                    if (this.rootLayout.Children.Contains(this.devStatusImage))
                    {
                        this.rootLayout.Children.Remove(this.devStatusImage);
                    }
                }
                #endregion

                 
                #region 设置票箱将空，将满状态

                string value = this.CheckDeviceTickBoxStatus(basiDevInfo.device_id);
                if (!value.Equals("00"))
                {
                    this.tickBoxStatusCfg = BuinessRule.GetInstace().sleMonitor.imageCollection.boxStatusDict.Single(temp => temp.Key.Equals(value)).Value;
                    SetImageSource(this.tickBoxStatus, this.GetImagePath(this.tickBoxStatusCfg.KeyField, 3),
                      STATUS_IMAGE_WIDTH,
                      STAUS_IMAGE_HEIGHT);
                    this.devImage.ToolTip = this.devImage.ToolTip.ToString() + " 票箱状态:" + GetTickStoreCNName(this.tickBoxStatusCfg.KeyField);


                    Storyboard sb1 = this.FindResource("imageShine") as Storyboard;
                    if (sb1 != null)
                    {
                        sb.RepeatBehavior = RepeatBehavior.Forever;
                        Storyboard.SetTarget(sb1, this.tickBoxStatus);
                        Storyboard.SetTargetProperty(this.tickBoxStatus, new PropertyPath("Visibility"));
                        sb1.Begin();
                    }
                    else
                    {
                        try
                        {
                            sb1.Stop();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else
                {
                    if (this.rootLayout.Children.Contains(this.tickBoxStatus))
                    {
                        this.rootLayout.Children.Remove(this.tickBoxStatus);
                    }
                }


                #endregion

                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }

        private string GetTickStoreCNName(string status)
        {
            switch (status)
            {
                case "01":
                    return "已空";
                case "02":
                    return "将空";
                case "03":
                    return "已满";
                case "04":
                    return "将满";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 通过程序计算得到AG到底需要加载哪个Image
        /// </summary>
        /// <param name="doorStatus">AG门状态，00：进站，01 出站  02：双向</param>
        /// <param name="dir">箭头朝向 01:左，02：右   03：左右双向   04：上  05：下  06：上下双向</param>
        private string CalAGImage(string doorStatus)
        {
            if (string.IsNullOrEmpty(doorStatus))
                return string.Empty;

            if (basiDevInfo.device_sub_type == ENTER_GATE ||
                basiDevInfo.device_sub_type == EXIT_GATE)   //单向
            {
                if (!basiDevInfo.device_sub_type.Equals(doorStatus)) //如果sub_type和doorStatus不相同
                {
                    WriteLog.Log_Error(string.Format("AG's door status error:sub_type='{0}',and doorStatus='{1}", basiDevInfo.device_sub_type,
                        doorStatus));
                    return string.Empty;
                }
                if (basiDevInfo.device_sub_type.Equals(doorStatus))//说明这个设备就是单向AG
                {
                    return basiDevInfo.device_direction;
                }
            }

            if (basiDevInfo.device_sub_type.Equals(DOUBLE_GATE))//双向，如果设备类型和设备不匹配则是双向的AG，设置的状态变成了单进或者单出
            {
                return GetCurrentDoubleAGDir(basiDevInfo.device_sub_type, doorStatus);
            }

            return string.Empty;
        }

        /// <summary>
        /// 图片配置的方向
        /// </summary>
        /// <param name="dir">图片配置的方向</param>
        /// <returns>返回当前的Image图片</returns>
        public string GetCurrentDoubleAGDir(string devSubType,string agDoorStatus)
        {
            if (devSubType != DOUBLE_GATE)//不是双向的，说明该设备就不是双向AG
            {
                return basiDevInfo.device_direction;
            }
            if (devSubType == DOUBLE_GATE &&   //双向轧机，但是设置成了单向
                agDoorStatus != DOUBLE_GATE)
            {
                if (agDoorStatus == ENTER_GATE)
                    return basiDevInfo.device_direction;
                if (agDoorStatus == EXIT_GATE)
                {
                    switch (basiDevInfo.device_direction)
                    {
                        case DIR_LEFT:
                            return DIR_RIGHT;
                        case DIR_RIGHT:
                            return DIR_LEFT;
                        case DIR_TOP:
                            return DIR_DOWN;
                        case DIR_DOWN:
                            return DIR_TOP;
                        default:
                            return basiDevInfo.device_direction;
                    }
                }
            }

            if (agDoorStatus.Equals(DOUBLE_GATE) &&      //双向轧机
                devSubType.Equals(DOUBLE_GATE))
            {
                switch (basiDevInfo.device_direction)
                {
                    case DIR_TOP:
                    case DIR_DOWN:
                    case DIR_V_DOUBLE:
                 
                        return DIR_V_DOUBLE;
                    case DIR_LEFT:
                    case DIR_RIGHT:
                    case DIR_H_DOUBLE:
               
                        return DIR_H_DOUBLE;
                    default:
                        return null;
                }
            }

            return basiDevInfo.device_direction;
        }

        


        /// <summary>
        /// 车站选择后更换图片
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="path"></param>
        private void SetImageSource(Image image, string path,params double[] size)
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
               if (size.Length == 0)
               {
                   image.Width = imageSize.Width;
                   image.Height = imageSize.Height;
               }
               if (size.Length == 2)
               {
                   image.Width = size[0];
                   image.Height = size[1];
               }
                image.Source = genBmpImage;
                if (!this.rootLayout.Children.Contains(image))
                {
                    this.rootLayout.Children.Add(image);
                    if (this.rootLayout.Children.Count == 1)
                    {
                        image.HorizontalAlignment = HorizontalAlignment.Left;
                        image.VerticalAlignment = VerticalAlignment.Center;
                    }
                    if (this.rootLayout.Children.Count == 2)
                    {
                        image.HorizontalAlignment = HorizontalAlignment.Center;
                        image.VerticalAlignment = VerticalAlignment.Center;
                    }

                    if (this.rootLayout.Children.Count == 3)
                    {
                        image.HorizontalAlignment = HorizontalAlignment.Right;
                        image.VerticalAlignment = VerticalAlignment.Center;
                    }

                }
            }
        }

        /// <summary>
        /// 创建Image图片的路径
        /// </summary>
        /// <param name="key">关键字段ID</param>
        /// <param name="flag">0:devImage 1:devSelectedImage 2:devStatusImage</param>
        /// <returns>返回图片的Image的路径</returns>
        private string GetImagePath(string key,int flag)
        {
            if (flag == 0)
            {
                return BuinessRule.GetInstace().sleMonitor.imageCollection.deviceImageDict[key].Path;
            }
            else if (flag == 1)
            {
                return BuinessRule.GetInstace().sleMonitor.imageCollection.devImageSelectedDict[key].Path;
            }
            else if (flag == 2)
            {
                return BuinessRule.GetInstace().sleMonitor.imageCollection.devRunStatusDict[key].Path;
            }
            else
            {
                return BuinessRule.GetInstace().sleMonitor.imageCollection.boxStatusDict[key].Path;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devciceId">设备ID</param>
        /// <param name="storeStatus">01：将空 02：已空，03：将满，04：已满</param>
        /// <returns></returns>
        private string CheckDeviceTickBoxStatus(string deviceId)
        {
        
                List<TickBoxInDevInfo> list = BuinessRule.GetInstace().GetTickInDevInfosByDeviceId(deviceId);
                if (list != null && list.Count > 0)
                {
                    if (basiDevInfo.device_type == DevType.DEV_AGM)
                    {
                        List<TickBoxInDevInfo> AgTickBoxList = list.Where(temp => temp.position_in_dev == "07" || temp.position_in_dev == "08").ToList();

                        if (AgTickBoxList != null && AgTickBoxList.Count > 0)
                        {
                            List<string> agList = new List<string>();

                            for (int i = 0; i < AgTickBoxList.Count; i++)
                            {
                                if (AgTickBoxList[i].tick_store_status == "03" || AgTickBoxList[i].tick_store_status == "04")
                                    agList.Add(AgTickBoxList[i].tick_store_status);
                            }
                            //if (agList.Contains("03"))  //有将满，返回将满
                            //    return "03";
                            //if (agList.Contains("04")) //无将满，返回已满
                            //    return "04";
                            if (agList.Count == 2)
                            {
                                if ((agList[0] == "03" && agList[1] == "03") || (agList[0] == "03" && agList[1] == "04") || (agList[0] == "04" && agList[1] == "03"))  //两个票箱均安装情况下有将满，返回将满
                                    return "03";
                                if (agList[0] == "04" && agList[1] == "04") //两个票箱均安装情况下无将满，返回已满
                                    return "04";
                            }
                            else if (agList.Count == 1 && AgTickBoxList.Count == 1)
                            {
                                if (agList.Contains("03"))  //一个票箱安装情况下有将满，返回将满
                                    return "03";
                                if (agList.Contains("04")) //一个票箱安装情况下无将满，返回已满
                                    return "04";                           
                            }
                        }
                    }

                    if (deviceId.Substring(4, 2) == DevType.DEV_TVM)
                    {
                        List<DevRunStatusDetail> list1 = BuinessRule.GetInstace().GetHopperStatusByDeviceId(deviceId);
                        if (list1 != null && list1.Count > 0)
                        {
                                    if (list1.Count == 2)
                                    {
                                        if ((list1[0].status_value == "01" && list1[1].status_value == "01") || (list1[0].status_value == "01" && list1[1].status_value == "02") || (list1[0].status_value == "02" && list1[1].status_value == "01"))  
                                            return "01";
                                        if (list1[0].status_value == "02" && list1[1].status_value == "02")
                                            return "02";
                                    }
                                    else if (list1.Count == 1)
                                    {
                                        if (list1[0].status_value == "01") 
                                            return "01";
                                        if (list1[0].status_value == "02") 
                                            return "02";
                                    }
                                }
                            }


                    //2013年5月13日应天津业主孙飞的要求,TVM票箱将空将满的状态以hopper的状态为准，之前是以票箱的状态为准
                    //if (basiDevInfo.device_type == DevType.DEV_TVM)
                    //{
                    //    List<TickBoxInDevInfo> tvmTickBoxList = list.Where(temp =>
                    //        temp.position_in_dev == "01" ||
                    //        temp.position_in_dev == "02" ||
                    //        temp.position_in_dev == "03" ||
                    //        temp.position_in_dev == "04").ToList();

                    //    if (tvmTickBoxList != null && tvmTickBoxList.Count > 0)
                    //    {
                    //        List<string> tvmStatus = new List<string>();
                    //        for (int i = 0; i < tvmTickBoxList.Count; i++)
                    //        {

                    //            if (tvmTickBoxList[i].tick_store_status == "01" ||
                    //                tvmTickBoxList[i].tick_store_status == "02" ||
                    //                tvmTickBoxList[i].tick_store_status == "03" ||
                    //                tvmTickBoxList[i].tick_store_status == "04"
                    //                )
                    //            {
                    //                tvmStatus.Add(tvmTickBoxList[i].tick_store_status);
                    //            }
                    //        }
                    //        if (tvmStatus.Contains("01"))
                    //            return "01";
                    //        if (tvmStatus.Contains("03"))
                    //            return "03";
                    //        if (tvmStatus.Contains("02"))
                    //            return "02";
                    //        if (tvmStatus.Contains("04"))
                    //            return "04";
                    //    }
                    //}
                }
                return "00";
            
        }

       



        #region IMessageHandler 成员

        public void HandleAsynMessage(Message msg)
        {
        
        }

        public void HandleSynMessage(Message msg)
        {
            
        }

        #endregion
    }
}
