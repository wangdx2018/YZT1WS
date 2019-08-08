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
using System.Collections.ObjectModel;


namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using AFC.WS.BR;
    using AFC.WS.Model;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.CommonControls;
    using TJComm;
    using AFC.WS.UI.Common;
    using AFC.WS.Model.Const;
    

    /// <summary>
    /// 参数下载通知
    /// added by wangdx  20110412
    /// </summary>
    public partial class ParamDownLoadNotify : UserControlBase
    {
        private ObservableCollection<Data> devList = new ObservableCollection<Data>();

        private ObservableCollection<SelectData> selectDataCollection = new ObservableCollection<SelectData>();

        private string stationId;

        private string devType;

        public ParamDownLoadNotify()
        {
            InitializeComponent();
            this.listBox1.ItemsSource = this.devList;
            this.listView1.ItemsSource = this.selectDataCollection;
        }

        public override void InitControls()
        {
           List<BasiStationInfo> stationlist = BuinessRule.GetInstace().GetAllStationInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
           BasiStationInfo info = new BasiStationInfo() { station_cn_name = "全部", station_id="03FF" };
           stationlist.Insert(0, info);

          this.cmbStationInfo.ItemsSource = stationlist;
          this.cmbStationInfo.DisplayMemberPath = "station_cn_name";
          this.cmbStationInfo.SelectionChanged += new SelectionChangedEventHandler(cmbStationInfo_SelectionChanged);
           
          List<BasiDevTypeInfo> devTypeList= BuinessRule.GetInstace().GetAllDevciceType();
          BasiDevTypeInfo devTypeInfo = new BasiDevTypeInfo { device_name = "全部" };
          devTypeList.Insert(0, devTypeInfo);
          this.cmbDevType.ItemsSource = devTypeList;

          this.cmbDevType.DisplayMemberPath = "device_name";
          this.cmbDevType.SelectionChanged += new SelectionChangedEventHandler(cmbDevType_SelectionChanged);

          this.rbLine.Checked += new RoutedEventHandler(CBChecked);
          this.rbStation.Checked += new RoutedEventHandler(CBChecked);
          this.rbSelfDef.Checked += new RoutedEventHandler(CBChecked);
          this.cbAll.Checked += new RoutedEventHandler(CBChecked);
          this.cbAll.Unchecked += new RoutedEventHandler(CBChecked);

          specialParamDownLoadNotify.InitControls();
        }

        public override void UnLoadControls()
        {
            //this.selectDataCollection.Clear();
            //this.devList.Clear();
            //this.listView1.Items.Clear();
            //this.listBox1.Items.Clear();
            //base.UnLoadControls();
            specialParamDownLoadNotify.UnLoadControls();
        }

        public override void InitlizeCompleteDone()
        {
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                this.rbLine.IsEnabled = false;
               
                string stationId = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                BasiStationInfo info = (this.cmbStationInfo.ItemsSource as List<BasiStationInfo>).Single(temp => temp.station_id.Equals(stationId));
                 int index= (this.cmbStationInfo.ItemsSource as List<BasiStationInfo>).IndexOf(info);
                 this.cmbStationInfo.SelectedIndex = index;
                 this.cmbStationInfo.IsEnabled = false;
            }
            specialParamDownLoadNotify.InitlizeCompleteDone();
            //base.InitlizeCompleteDone();
        }
        
        #region Event Handle
        private void CBChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton)
            {
                RadioButton cb = sender as RadioButton;
                if (cb.Name.Equals("rbLine"))
                {
                    this.cmbStationInfo.SelectedIndex = 0;
                    this.cmbStationInfo.IsEnabled = !cb.IsChecked.Value;
                    this.cmbDevType.IsEnabled = !cb.IsChecked.Value;
                    this.listBox1.IsEnabled = !cb.IsChecked.Value;
                    this.listView1.IsEnabled = !cb.IsChecked.Value;
                    this.btnAdd.IsEnabled = !cb.IsChecked.Value;
                    this.btnDel.IsEnabled = !cb.IsChecked.Value;
                    this.devList.Clear();
                    this.selectDataCollection.Clear();
                }

                if (cb.Name.Equals("rbStation"))
                {
                    this.cmbDevType.IsEnabled = !cb.IsChecked.Value;
                    this.cmbDevType.SelectedIndex = 0;
                    this.listView1.IsEnabled = !cb.IsChecked.Value;
                    this.listBox1.IsEnabled = !cb.IsChecked.Value;
                    this.cmbDevType.SelectedIndex=0;
                    if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("LC"))
                    {
                        this.cmbStationInfo.IsEnabled = cb.IsChecked.Value;
                    }
                    this.devList.Clear();
                    this.selectDataCollection.Clear();
                }

                if (cb.Name.Equals("rbSelfDef"))
                {
                   
                    if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("LC"))
                    {
                        this.cmbStationInfo.SelectedIndex = 0;
                        this.cmbStationInfo.IsEnabled = cb.IsChecked.Value;
                    }
                  //  this.cmbStationInfo.IsEnabled = cb.IsChecked.Value;
                    this.cmbDevType.IsEnabled = cb.IsChecked.Value;
                    this.listBox1.IsEnabled = cb.IsChecked.Value;
                    this.listView1.IsEnabled = cb.IsChecked.Value;
                    this.btnAdd.IsEnabled = cb.IsChecked.Value;
                    this.btnDel.IsEnabled =cb.IsChecked.Value;
                    this.devList.Clear();
                    this.selectDataCollection.Clear();
                }

              
              
            }
            if (sender is CheckBox)
            {
                CheckBox cbAll = sender as CheckBox;
                if (cbAll.Name.Equals("cbAll"))
                {
                    foreach (var temp in this.selectDataCollection)
                    {
                        temp.IsChecked = cbAll.IsChecked.Value;
                    }
                    cbAll.Content = cbAll.IsChecked.Value ? "反选" : "全选";
                }
            }

            //}
            //throw new NotImplementedException();
        }

        private void cmbDevType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BasiStationInfo stationInfo = cmbStationInfo.SelectedValue as BasiStationInfo;
            if (stationInfo == null)
                return;

            BasiDevTypeInfo basiDevTypeInfo = cmbDevType.SelectedValue as BasiDevTypeInfo;
            if (basiDevTypeInfo == null)
                return;
            stationId = stationInfo.station_id;
            devType = basiDevTypeInfo.device_type;
            List<BasiDevInfo> list = null;
            if (basiDevTypeInfo.device_name != "全部")
            {
               list = BuinessRule.GetInstace().GetBasiDevInfo(stationId, devType);
            }
            else
            {
                list = BuinessRule.GetInstace().GetBasiDevInfo(stationId);
            }
            if (list == null)
            {
                devList.Clear();
                return;
            }
            devList.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                Data data = new Data();
                data.ID = list[i].device_id;
                data.Text = list[i].device_id;
                data.IsChecked = false;
                devList.Add(data);
            }
            
            //throw new NotImplementedException();
        }

        private void cmbStationInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BasiStationInfo stationInfo = cmbStationInfo.SelectedValue as BasiStationInfo;
            this.cmbDevType.SelectedIndex = 0;
            if (stationInfo == null)
                return;
            stationId = stationInfo.station_id;
            List<BasiDevInfo> list = BuinessRule.GetInstace().GetBasiDevInfo(stationId);
            devList.Clear();
            if (list == null)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                Data data = new Data();
                data.ID = list[i].device_id;
                data.Text = list[i].device_id;
                data.IsChecked = false;
                devList.Add(data);
            }
          
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            foreach (var temp in this.listBox1.Items)
            {
                Data data = temp as Data;
                if (data.IsChecked&&!CheckIsExist(data.ID))
                {
                      data.IsChecked = false;

                      SelectData sd = new SelectData
                      {
                          DeviceId = data.ID,
                          StationId = data.ID.Substring(0, 4).ToString(),
                          IsChecked = false,
                          StationName = BuinessRule.GetInstace().GetStationInfoById(data.ID.Substring(0, 4).ToString()).station_cn_name,
                          DevTypeName = BuinessRule.GetInstace().GetBasiDevTypInfo(data.ID.Substring(4, 2)).device_name
                      };
                      this.selectDataCollection.Add(sd);
                }
            }
        }

        private bool CheckIsExist(string deviceId)
        {
            try
            {
                this.selectDataCollection.Single(temp => temp.DeviceId.Equals(deviceId));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            List<SelectData> tempList = new List<SelectData>();
            for (int i = 0; i < this.listView1.Items.Count; i++)
            {
                SelectData sd = this.listView1.Items[i] as SelectData;
                if (sd.IsChecked)
                {
                    tempList.Add(sd);
                }
            }
            
            for (int i = 0; i < tempList.Count; i++)
            {
                this.selectDataCollection.Remove(tempList[i]);
            }
        }

        private void btnParamsDownLoad_Click(object sender, RoutedEventArgs e)
        {
            if (!(this.rbLine.IsChecked.Value ||
                this.rbSelfDef.IsChecked.Value ||
                this.rbStation.IsChecked.Value))
            {
                MessageDialog.Show("请选择设备范围信息!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            ushort paramType =0x4001;
            List<TJComm.DeviceRange> list=new List<TJComm.DeviceRange>();

            if(this.rbLine.IsChecked.Value)
            {
                list.Add(new TJComm.DeviceRange{ special_flag=0, stationId=0, deviceRange=new List<uint>()});
               int res= BuinessRule.GetInstace().commProcess.ParamsDownloadNotify(paramType, list);
               if (res == 0)
               {
                   BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Param_DownLoad_Notify, "0", "参数下载命令发送成功");
               }
               else 
               {
                   BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Param_DownLoad_Notify, "1", "参数下载命令发送失败");
               }
               string tip = AFC.WS.ModelView.UIContext.MessageCfg.getMessageContent("1309", res.ToString());
               MessageDialog.Show(tip, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (this.rbStation.IsChecked.Value)
            {
                list.Add(new DeviceRange { special_flag = 1, stationId = this.stationId.ConvertHexStringToUshort(), deviceRange = new List<uint>() });
                int res = BuinessRule.GetInstace().commProcess.ParamsDownloadNotify(paramType, list);
                if (res == 0)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Param_DownLoad_Notify, "0", "参数下载命令发送成功");
                }
                else
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Param_DownLoad_Notify, "1", "参数下载命令发送失败");
                }
                string tip = AFC.WS.ModelView.UIContext.MessageCfg.getMessageContent("1309", res.ToString());
                MessageDialog.Show(tip, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (this.rbSelfDef.IsChecked.Value)
            { 
                    ILookup<string, SelectData> lookUpData = this.selectDataCollection.ToLookup(temp => temp.StationId);
                    foreach (var temp in lookUpData)
                    {
                        DeviceRange dr = new DeviceRange();
                        dr.special_flag = 2;
                        foreach (var dd in temp)
                        {
                            dr.stationId = dd.StationId.ConvertHexStringToUshort();
                            dr.deviceRange.Add(dd.DeviceId.ConvertHexStringToUint());
                        }
                        list.Add(dr);
                    }
                  
                int res=BuinessRule.GetInstace().commProcess.ParamsDownloadNotify(paramType, list);
                if (res == 0)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Param_DownLoad_Notify, "0", "参数下载命令发送成功");
                }
                else
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Param_DownLoad_Notify, "1", "参数下载命令发送失败");
                }
                string tip = AFC.WS.ModelView.UIContext.MessageCfg.getMessageContent("1309", res.ToString());
                MessageDialog.Show(tip, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                
            }
        }

        #endregion

    }

      internal class SelectData:System.ComponentModel.INotifyPropertyChanged
    {
        private string stationId;

        public string StationId
        {
            set
            {
                this.stationId = value;
                this.PorperyChangeHandle("StationId");
            }
            get { return this.stationId; }
          
            
      
        }

        private string deviceId;
        public string DeviceId
        {
            set
            {
                this.deviceId = value;
                this.PorperyChangeHandle("DeviceId");
            }
            get
            {
                return this.deviceId;
            }
        }

        private bool isChecked;

        public bool IsChecked
        {
            set
            {
                this.isChecked = value;
                this.PorperyChangeHandle("IsChecked");
            }
            get { return isChecked; }
        }

      private string devTypName;

      public string DevTypeName
      {
          set
          {
              this.devTypName = value;
              this.PorperyChangeHandle("DevTypeName");
          }
          get
          {
              return this.devTypName;
          }
      }


      private string stationName;

      public string StationName
      {
          set
          {
              this.stationName = value;
              this.PorperyChangeHandle("StationName");
          }
          get { return this.stationName; }
      }

        public void PorperyChangeHandle(string name)
        {
            if(this.PropertyChanged!=null)
                this.PropertyChanged.Invoke(this,new System.ComponentModel.PropertyChangedEventArgs(name));
        }

        #region INotifyPropertyChanged 成员

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
};
