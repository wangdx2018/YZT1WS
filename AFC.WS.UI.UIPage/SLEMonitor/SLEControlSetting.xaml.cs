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
using AFC.WS.Model.DB;

namespace AFC.WS.UI.UIPage.SLEMonitor
{
    using AFC.WS.Model.Const;
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    using AFC.WS.BR.SLEMonitorManager;
    using AFC.WS.UI.CommonControls;
    using TJComm;
    /// <summary>
    /// SLEControlSetting.xaml 的交互逻辑
    /// </summary>
    public partial class SLEControlSetting : UserControl
    {
        /// <summary>
        /// 当前控制代码
        /// </summary>
        private string currentSelected = null;

        /// <summary>
        /// 基本设备信息
        /// </summary>
        private BasiDevInfo basiDevInfo = null;

        

        public SLEControlSetting()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
         
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.currentSelected))
            {
                MessageDialog.Show("请选择控制命令", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return;
            }
            if (string.IsNullOrEmpty(this.cmdRange.Text))
            {
                MessageDialog.Show("请选择控制命令范围", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return;
            }
            else
            {
                byte controlType = this.currentSelected.Substring(0, 2).ToHexNumberByte();
                string devRange = (this.cmdRange.SelectedItem as ComboBoxItem).Tag.ToString();
                List<DeviceRange> list = this.CreateDeviceRanage(devRange);
                int res = BuinessRule.GetInstace().commProcess.ControlCmd(controlType, this.currentSelected.ToHexNumberUShort(), list);
                if (res == 0)
                {
                    MessageDialog.Show("发送控制命令成功！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    this.Reset();
                    return;
                }
                else
                {
                    MessageDialog.Show("发送控制命令失败！", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                    this.Reset();
                    return;
                }
                
            }
        }

        private List<DeviceRange> CreateDeviceRanage(string selected)
        {
            List<DeviceRange> list = new List<DeviceRange>();
            DeviceRange dr = new DeviceRange();
            dr.stationId =basiDevInfo.station_id.ToHexNumberUShort();
            dr.special_flag = 2;
            dr.deviceRange = new List<uint>();

            if (selected.Equals("0"))//应用到该元素
            {
                dr.deviceRange.Add(this.basiDevInfo.device_id.ConvertHexStringToUint());
             
            }
            if (selected.Equals("1"))//应用到本站厅
            {
                dr.deviceRange.AddRange(this.GetDeviceIdCollectionByStationIdAndHallId(basiDevInfo.station_id, basiDevInfo.station_hall_id));
            }
            if (selected.Equals("2"))//应用到本站同类设备
            {
                dr.deviceRange.AddRange(this.GetDeviceIdCollectionByStationId(basiDevInfo.station_id));
            }
               list.Add(dr);
               return list;
          
        }

        private List<uint> GetDeviceIdCollectionByStationId(string stationId)
        {
            string cmd = string.Format("select * from basi_dev_info t where t.station_id='{0}' and t.device_type='{1}'", stationId,basiDevInfo.device_type);
            List<BasiDevInfo> list=DBCommon.Instance.GetTModelValue<BasiDevInfo>(cmd);
            if (list == null || list.Count == 0)
                return null;
            else
                return list.Select(temp => temp.device_id.ConvertHexStringToUint()).ToList();
        }

        private List<uint> GetDeviceIdCollectionByStationIdAndHallId(string stationId, string hallId)
        {
            string cmd = string.Format("select * from basi_dev_info t where t.station_id='{0}' and t.station_hall_id='{1}' and t.device_type='{2}'", stationId, hallId,basiDevInfo.device_type);
            List<BasiDevInfo> list = DBCommon.Instance.GetTModelValue<BasiDevInfo>(cmd);
            if (list == null || list.Count == 0)
                return null;
            else
                return list.Select(temp => temp.device_id.ConvertHexStringToUint()).ToList();
        }


        private void btuCancle_Click(object sender, RoutedEventArgs e)
        {
            this.Reset();
            this.currentSelected = null;
         
        }

        /// <summary>
        /// 设置设备的基本信息
        /// </summary>
        /// <param name="devInfo">设备基本信息</param>
        /// <returns>设置设备基本信息</returns>
        public void SetBasiDevInfo(BasiDevInfo devInfo)
        {
            this.basiDevInfo = devInfo;
            FillRange();
            if (devInfo.device_type == DevType.DEV_BOM)
            {
                
                this.tvmRunCtlCommand.Visibility = Visibility.Collapsed;
                this.bomRunCtlCommand.Visibility = Visibility.Visible;
                this.agRunCtlCommand.Visibility = Visibility.Collapsed;
                this.powerControl.Visibility = Visibility.Collapsed;
                this.devRunStatus.Visibility = Visibility.Visible;
                return;

            }
            if (devInfo.device_type == DevType.DEV_AGM)
            {
                this.tvmRunCtlCommand.Visibility = Visibility.Collapsed;
                this.bomRunCtlCommand.Visibility = Visibility.Collapsed;
                this.agRunCtlCommand.Visibility = Visibility.Visible;
                this.powerControl.Visibility = Visibility.Visible;
                this.devRunStatus.Visibility = Visibility.Visible;
                return;
            }

            if (devInfo.device_type == DevType.DEV_TVM)
            {
                this.tvmRunCtlCommand.Visibility = Visibility.Visible;
                this.bomRunCtlCommand.Visibility = Visibility.Collapsed;
                this.agRunCtlCommand.Visibility = Visibility.Collapsed;
                this.powerControl.Visibility = Visibility.Visible;
                this.devRunStatus.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                this.tvmRunCtlCommand.Visibility = Visibility.Collapsed;
                this.bomRunCtlCommand.Visibility = Visibility.Collapsed;
                this.agRunCtlCommand.Visibility = Visibility.Collapsed;
                this.bomRunCtlCommand.Visibility = Visibility.Collapsed;
                this.agRunCtlCommand.Visibility = Visibility.Collapsed;
                return;
            }


            
        }

      

        /// <summary>
        /// 填充应用范围
        /// </summary>
        private void FillRange()
        {
            this.cmdRange.Items.Clear();
            ComboBoxItem item = new ComboBoxItem();
            item.Content = "应用到本设备";
            item.Tag = "0";

            ComboBoxItem item1 = new ComboBoxItem();
            item1.Content = "应用到本站厅同类设备";
            item1.Tag = "1";

            ComboBoxItem item2 = new ComboBoxItem();
            item2.Content = "应用到本站同类设备";
            item2.Tag = "2";

            ComboBoxItem item3 = new ComboBoxItem();
            item3.Content = "应用到线路同类设备";
            item3.Tag = "3";
            this.cmdRange.Items.Add(item);
            this.cmdRange.Items.Add(item1);
            this.cmdRange.Items.Add(item2);

            this.cmdRange.SelectedIndex = 0;

            //if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("LCWS"))
            //{
            //    this.cmdRange.Items.Add(item3);
            //}
         

        }

        /// <summary>
        /// 重置，将选中的信息重置
        /// </summary>
        private void Reset()
        {
            this.groupBoxControl.GetVisuals().OfType<RiadioButtonExtend>().ToList().ForEach(temp => { temp.IsChecked = false; });
            this.groupBoxControl.GetVisuals().OfType<ComboBox>().ToList().ForEach(temp => { temp.Text = string.Empty; });
        }

        private void RiadioButtonExtend_Checked(object sender, RoutedEventArgs e)
        {
            RiadioButtonExtend rb = sender as RiadioButtonExtend;
            this.currentSelected = rb.Uid;  
        }
    }
}
