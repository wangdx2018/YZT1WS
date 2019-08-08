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
    using AFC.WS.Model.DB;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    using AFC.WS.BR.SLEMonitorManager;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.UIPage.DeviceMonitor;
    using AFC.WS.UI.DataSources;
    /// <summary>
    /// DevStatusDetail.xaml 的交互逻辑
    /// </summary>
    public partial class DevStatusDetail : UserControlBase
    {

        private BasiDevInfo info = new BasiDevInfo();

        /// <summary>
        /// 得到该状态ID，状态值的中文名称
        /// </summary>
        private DevStatus devStatus = new DevStatus();


        public DevStatusDetail()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
      
        }

        

       public override void InitControls()
        {
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.SLEImageButton, SynMessageType.Dev_Image_Selected,
                HandleMode.Syn, true);
            this.tabMoneyBox.IsEnabled = false;
            this.tabHopper.IsEnabled = false;
            this.tabTicketBox.IsEnabled = false;
            this.baseInfo.IsEnabled = false;
            this.DevSet.IsEnabled = false;
            this.labTrouble.IsEnabled = false;
            this.tabDevResUseInfo.IsEnabled = false;
            base.InitControls();
        }

        public override void UnLoadControls()
        {
            MessageManager.CancelAllSubscribeMessage(SynMessageType.Dev_Image_Selected);
            //base.UnLoadControls();
        }

        /// <summary>
        ///设置哪个Tab的Enable是否为true或者是false
        /// </summary>
        /// <param name="info">设备信息表</param>
        private void SetVaildControl(BasiDevInfo info)
        {
            this.info = info;
            this.TableControl.SelectedIndex = 0;
            this.baseInfo.IsEnabled = true;
            this.DevSet.IsEnabled = true;
            this.labTrouble.IsEnabled = true;
            this.tabMoneyBox.IsEnabled = true;
            this.tabHopper.IsEnabled = true;
            this.tabTicketBox.IsEnabled = true;
          //  this.tabDevResUseInfo.IsEnabled = true;
            if (info == null)
            {
                WriteLog.Log_Error("info is error!");
                return;
            }
            switch (info.device_type)
            {
                case DevType.DEV_AGM:
                    this.baseInfo.Visibility = Visibility.Visible;
                    this.tabMoneyBox.Visibility =Visibility.Collapsed;
                    this.tabHopper.Visibility = Visibility.Collapsed;
                    this.tabTicketBox.Visibility=Visibility.Visible;
                    this.TableControl.Visibility=Visibility.Visible;
                    this.DevSet.Visibility = Visibility.Visible;
                 //   this.tabDevResUseInfo.Visibility = Visibility.Visible;
                    break;
                case DevType.DEV_BOM:
                    this.baseInfo.Visibility = Visibility.Visible;
                    this.tabMoneyBox.Visibility = Visibility.Collapsed;
                    this.tabHopper.Visibility = Visibility.Collapsed;
                    this.tabTicketBox.Visibility = Visibility.Collapsed;
                    this.TableControl.Visibility = Visibility.Visible;
                    this.DevSet.Visibility = Visibility.Visible;
                   // this.tabDevResUseInfo.Visibility = Visibility.Visible;
                    break;
                case DevType.DEV_TVM:
                    this.baseInfo.Visibility = Visibility.Visible;
                    this.tabMoneyBox.Visibility = Visibility.Visible;
                    this.tabTicketBox.Visibility = Visibility.Visible;
                    this.TableControl.Visibility = Visibility.Visible;
                    this.DevSet.Visibility = Visibility.Visible;
                    this.tabHopper.Visibility = Visibility.Visible;
                 //   this.tabDevResUseInfo.Visibility = Visibility.Visible;
                    break;
                default:
                    this.baseInfo.Visibility = Visibility.Visible;
                    this.tabMoneyBox.Visibility = Visibility.Collapsed;
                    this.tabTicketBox.Visibility = Visibility.Collapsed;
                    this.TableControl.Visibility = Visibility.Visible;
                    this.DevSet.Visibility = Visibility.Collapsed;
                    this.tabHopper.Visibility = Visibility.Collapsed;
                //    this.tabDevResUseInfo.Visibility = Visibility.Visible;
                    break;
            }
            return;
        }

        /// <summary>
        /// 设置设备的基本信息
        /// </summary>
        /// <param name="info">设备信息</param>
        private void SetBasiDevInfo(BasiDevInfo info)
        {
            this.txtDeviceId.Text = info.device_id;
            this.txtDevType.Text = BuinessRule.GetInstace().GetDevTypeInfoById(info.device_type).device_name;
            this.txtIpAddress.Text = info.device_ip;
            this.txtGroupId.Text = BuinessRule.GetInstace().GetBasiHallGroupById(info.station_id,
                info.station_hall_id, info.hall_group_id).hall_group_name;
            this.txtGroupInterId.Text = info.group_serial_no;
            this.txtHall.Text = BuinessRule.GetInstace().GetBasiStationHallById(info.station_id, info.station_hall_id).station_hall_name;
        }

        /// <summary>
        /// 设置设备的资源信息
        /// </summary>
        /// <param name="info">设备资源信息</param>
        /// 
        
        private void SetDevResUseInfo(BasiDevInfo info)
        {
            DevResUseInfo info1 = BuinessRule.GetInstace().GetDevResUseInfo(info.device_id.ToString());
            this.totalDiskVolume.Text = info1.total_disk_volume.ToString()+"K";
            this.usedDiskVolume.Text = info1.used_disk_volume.ToString() + "K";
            this.dbFileVolume.Text = info1.db_file_volume.ToString() + "K";
            this.memVolume.Text = info1.mem_volume.ToString() + "%";
            this.cpuVolume.Text = info1.cpu_volume.ToString() + "%";
            if (info1.db_status != null)
            {
                if (info1.db_status.ToString().Equals("0"))
                {
                    this.dbStatus.Text = "正常";
                }
                else if (info1.db_status.ToString().Equals("1"))
                {
                    this.dbStatus.Text = "故障";
                }
            }
            else
            {
                this.dbStatus.Text = "N/A";
            }
            if (info1.update_date!=null)
            {
                this.updateDateTime.Text = info1.update_date.Substring(0, 4) + "/" + info1.update_date.Substring(4, 2) + "/" + info1.update_date.Substring(6, 2) + " " + info1.update_time.Substring(0, 2) + ":" + info1.update_time.Substring(2, 2) + ":" + info1.update_time.Substring(4, 2);
            }
            else
            {
                this.updateDateTime.Text = "0000/00/00" + " " + "00:00:00";
            }
         }

        /// <summary>
        /// 得到某类设备的工作模式
        /// </summary>
        /// <param name="info">设备基础信息</param>
        private void SetWorkModeInfo(BasiDevInfo info)
        {
            DevRunStatusDetail drs = null; 
            
            switch (info.device_type)
            {
                case DevType.DEV_AGM:
                    this.txtWork.IsEnabled = true;
                    drs = devStatus.GetDevRunStatusDetail(info.station_id,
                        info.device_id,
                        CSSStatusIDInfo.AG_DEV_TYPE);
                    break;
                case DevType.DEV_BOM:
                    this.txtWork.IsEnabled = true;
                    drs= devStatus.GetDevRunStatusDetail(info.station_id,
                        info.device_id,
                        CSSStatusIDInfo.BOM_WORK_MODE);
                    break;
                case DevType.DEV_TVM:
                    this.txtWork.IsEnabled = true;
                    drs = devStatus.GetDevRunStatusDetail(info.station_id,
                     info.device_id,
                     CSSStatusIDInfo.TVM_WORK_MODE);
                    break;
                default:
                    this.txtWork.IsEnabled = false;
                    this.txtWork.Text = string.Empty;
                    break;
            }
            if ((drs == null || drs.status_id == null))
                this.txtWork.Text = "未获取到工作模式";
            else
                this.txtWork.Text = devStatus.GetStatusValueCNName(devStatus.GetBasiStatusIdInfoByCssId(drs.status_id, drs.status_value));
            //由于没有定义EQM的工作模式，天津业主提出EQM工作模式修改为“正常”，2013年6月13日按天津业主及马晓春要求做出此处修改。
            if (info.device_type == DevType.DEV_TCM)
            {
                this.txtWork.IsEnabled = true;
                this.txtWork.Text = "正常";
            }

        }

        /// <summary>
        /// 设置服务模式
        /// </summary>
        /// <param name="info">服务模式</param>
        private void SetServerModeInfo(BasiDevInfo info)
        {
            DevRunStatusDetail dtt = null;    //工作模式
            DevRunStatusDetail dpt = null;    //支付模式
            DevRunStatusDetail dct = null;    //找零模式
            switch(info.device_type)
            {
                case DevType.DEV_AGM:
                    this.txtService.IsEnabled = true;
                    dtt = devStatus.GetDevRunStatusDetail(info.station_id,
                    info.device_id,
                    CSSStatusIDInfo.SERVER_MODE);
                    break;
                case DevType.DEV_TCM:
                    this.txtService.IsEnabled = true;
                    dtt = devStatus.GetDevRunStatusDetail(info.station_id,
                    info.device_id,
                    CSSStatusIDInfo.SERVER_MODE);
                    break;
                case DevType.DEV_TVM:
                    this.txtService.IsEnabled=true;
                    dtt = devStatus.GetDevRunStatusDetail(info.station_id,
                    info.device_id,
                    CSSStatusIDInfo.TVM_WORK_MODE);
                    dpt = devStatus.GetDevRunStatusDetail(info.station_id,
                    info.device_id,
                    CSSStatusIDInfo.TVM_PAMMENT_METHOD);
                    dct = devStatus.GetDevRunStatusDetail(info.station_id,
                    info.device_id,
                    CSSStatusIDInfo.TVM_CHANGE_MODE);
                    break;
                default:
                    this.txtService.IsEnabled = false;
                    this.txtService.Text = string.Empty;
                    return;
            }
            if (dtt == null || dtt.status_id == null)
                this.txtService.Text = "未获取到服务模式";
            else if (info.device_type.Equals(DevType.DEV_TVM))
                this.txtService.Text = devStatus.GeTvmWorkModeName(dtt.status_value, dpt.status_value, dct.status_value);
            else
                this.txtService.Text = devStatus.GeAGEQMServerModeName(dtt.status_id, dtt.status_value);
            //天津业主提出EQM服务模式修改为“正常”，2013年6月13日按天津业主及马晓春要求做出此处修改。
            if (info.device_type == DevType.DEV_TCM)
            {
                this.txtService.IsEnabled = true;
                this.txtService.Text = "正常";
            }
        }

        /// <summary>
        /// 设置运行模式
        /// </summary>
        /// <param name="info">设备基础信息</param>
        private void SetRunStatusInfo(BasiDevInfo info)
        {
          List<DevRunStatusInfo> list=BuinessRule.GetInstace().GetAllDevRunStatusInfo(info.station_id);
          if (list != null && list.Count > 0)
          {
              try
              {
                  DevRunStatusInfo runInfo = list.Single(temp => temp.device_id.Equals(info.device_id));
                  switch (runInfo.run_status)
                  {
                      case "00":
                          this.txtRunState.Text= "正常服务";
                          break;
                      case "01":
                          this.txtRunState.Text = "报警";
                          break;
                      case "02":
                          this.txtRunState.Text = "故障";
                          break;
                      case "03":
                          this.txtRunState.Text = "网络中断";
                          break;
                      default:
                          this.txtRunState.Text = "N/A状态";
                          break;
                  }
              }
              catch (Exception ex)
              {
                  WriteLog.Log_Error(ex.Message);
              }
              
          }
        }

        /// <summary>
        /// 处理图标选中的同步消息
        /// </summary>
        /// <param name="msg"></param>
        public override void HandleSynMessage(Message msg)
        {
            if (msg.MessageType == SynMessageType.Dev_Image_Selected)
            {
                BasiDevInfo devInfo = msg.Content as BasiDevInfo;
                SetVaildControl(devInfo);
                SetBasiDevInfo(devInfo);
                SetWorkModeInfo(devInfo);
                SetServerModeInfo(devInfo);
                SetRunStatusInfo(devInfo);
                SetTickBoxInfo(devInfo);
                SetMoneyBoxInfo(devInfo);
                SetControlCmd(devInfo);
                SetHopperInfo(devInfo);
                //SetDevResUseInfo(devInfo);
                //this.sleControl.SetStationInfo(msg.MessageParam.ToString());
            }
            //base.HandleSynMessage(msg);
        }

        /// <summary>
        /// 设置票箱状态
        /// </summary>
        /// <param name="info"></param>
        private void SetTickBoxInfo(BasiDevInfo info)
        {

           // this.tickBoxManager = new BoxManager(info, 0);
            this.tickBoxManager.Init(info,0);
        }

    

        /// <summary>
        /// 设置钱箱状态
        /// </summary>
        /// <param name="info"></param>
        private void SetMoneyBoxInfo(BasiDevInfo info)
        {
            if (info.device_type == DevType.DEV_TVM)
            {
               // this.cashBoxManager = new BoxManager(info, 1);
                this.cashBoxManager.Init(info, 1);
            }
        }

        /// <summary>
        /// 2013年5月7日应天津业主孙飞要求加入Hopper数量状态及数量
        /// </summary>
        /// <param name="info"></param>
        private void SetHopperInfo(BasiDevInfo info)
        {
            if (info.device_type == DevType.DEV_TVM)
            {
                // this.cashBoxManager = new BoxManager(info, 1);
                this.HopperManager.Init(info, 2);
            }
        }

        /// <summary>
        /// 设置控制用户控件
        /// </summary>
        /// <param name="info"></param>
        private void SetControlCmd(BasiDevInfo info)
        {
            this.sleControl.SetBasiDevInfo(info);
        }

        #region ui envents
        private void labTrouble_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowWindowAction action = new ShowWindowAction();
            action.Title = "当前设备的所有运行状态";
            action.ucb = new DevRunStatusQuery();
            action.Width = 800;
            action.Height = 600;
         
            List<QueryCondition> list = new List<QueryCondition>();
            list.Add(new QueryCondition { bindingData = "device_id", operation = OperationSymbols.Equal, value = info.device_id });
            action.DoAction(list);
           
       
        }

  
        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonReSet_Click(object sender, RoutedEventArgs e)
        {

        }

    
     
        #endregion









    }
}
