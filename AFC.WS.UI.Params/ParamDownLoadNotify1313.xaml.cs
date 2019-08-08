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
using AFC.WS.Model.DB;
using AFC.WS.BR;
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using TJComm;
using AFC.WS.Model.Const;
using AFC.WS.Model.Comm;

namespace AFC.WS.UI.Params
{
    /// <summary>
    /// ParamDownLoadNotify1313.xaml 的交互逻辑
    /// </summary>
    public partial class ParamDownLoadNotify1313 : UserControlBase
    {
        private ObservableCollection<Data> devList = new ObservableCollection<Data>();

        private ObservableCollection<SelectData> selectDataCollection = new ObservableCollection<SelectData>();

        private ObservableCollection<SelectPara> selectParaCollection = new ObservableCollection<SelectPara>();


        private string stationId;

        private string devType;
        public ParamDownLoadNotify1313()
        {
            InitializeComponent();
            this.listBox1.ItemsSource = this.devList;
            this.listView1.ItemsSource = this.selectDataCollection;
            this.listView2.ItemsSource = this.selectParaCollection;
        }


        public override void InitControls()
        {

            this.rbSelfDef.IsChecked = true;
            List<BasiStationInfo> stationlist = BuinessRule.GetInstace().GetAllStationInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
            BasiStationInfo info = new BasiStationInfo() { station_cn_name = "全部", station_id = "03FF" };
            stationlist.Insert(0, info);

            this.cmbStationInfo.ItemsSource = stationlist;
            this.cmbStationInfo.DisplayMemberPath = "station_cn_name";
            this.cmbStationInfo.SelectionChanged += new SelectionChangedEventHandler(cmbStationInfo_SelectionChanged);

            List<BasiDevTypeInfo> devTypeList = BuinessRule.GetInstace().GetAllDevciceType();
            BasiDevTypeInfo devTypeInfo = new BasiDevTypeInfo { device_name = "全部" };
            devTypeList.Insert(0, devTypeInfo);
            this.cmbDevType.ItemsSource = devTypeList;

            this.cmbDevType.DisplayMemberPath = "device_name";
            this.cmbDevType.SelectionChanged += new SelectionChangedEventHandler(cmbDevType_SelectionChanged);

            List<notify> notifyList = new List<notify>();
            notify notify1 = new notify { notifyName = "普通同步", notifyID = "0" };
            notify notify2 = new notify { notifyName = "指定同步", notifyID = "1" };
            notifyList.Add(notify1);
            notifyList.Add(notify2);
            this.cmbNotifyType.ItemsSource = notifyList;
            this.cmbNotifyType.DisplayMemberPath = "notifyName";


            List<BasiParaTypeInfo> paraTypeList = BuinessRule.GetInstace().GetAllParaTypeInfo();
            this.cmbParaType.ItemsSource = paraTypeList;
            this.cmbParaType.DisplayMemberPath = "para_desc";
            this.cmbParaType.SelectionChanged += new SelectionChangedEventHandler(cmbParaType_SelectionChanged);


            this.rbLine.Checked += new RoutedEventHandler(CBChecked);
            this.rbStation.Checked += new RoutedEventHandler(CBChecked);
            this.rbSelfDef.Checked += new RoutedEventHandler(CBChecked);
            this.cbAll.Checked += new RoutedEventHandler(CBChecked);
            this.cbAll.Unchecked += new RoutedEventHandler(CBChecked);
            this.cbAllPara.Checked += new RoutedEventHandler(CBChecked);
            this.cbAllPara.Unchecked += new RoutedEventHandler(CBChecked);
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
        public override void InitlizeCompleteDone()
        {
            if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName.Contains("SC"))
            {
                this.rbLine.IsEnabled = false;

                string stationId = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                BasiStationInfo info = (this.cmbStationInfo.ItemsSource as List<BasiStationInfo>).Single(temp => temp.station_id.Equals(stationId));
                int index = (this.cmbStationInfo.ItemsSource as List<BasiStationInfo>).IndexOf(info);
                this.cmbStationInfo.SelectedIndex = index;
                this.cmbStationInfo.IsEnabled = false;
            }
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
                    this.cmbDevType.SelectedIndex = 0;
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
                    this.btnDel.IsEnabled = cb.IsChecked.Value;
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
                if (cbAll.Name.Equals("cbAllPara"))
                {
                    foreach (var temp in this.selectParaCollection)
                    {
                        temp.IsParaChecked = cbAllPara.IsChecked.Value;
                    }
                    cbAllPara.Content = cbAllPara.IsChecked.Value ? "反选" : "全选";
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

        private void cmbParaType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BasiParaTypeInfo paraTypeInfo = this.cmbParaType.SelectedValue as BasiParaTypeInfo;
            if (paraTypeInfo == null)
                return;
            List<ParaVersionInfo> versionList = BuinessRule.GetInstace().GetAllParaVersionInfo(paraTypeInfo.para_type);
            this.cmbVersion.ItemsSource = versionList;
            this.cmbVersion.DisplayMemberPath = "para_version";
            this.cmbVersion.SelectedIndex = 0;

        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            foreach (var temp in this.listBox1.Items)
            {
                Data data = temp as Data;
                if (data.IsChecked && !CheckIsExist(data.ID))
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

        private bool CheckValid()
        {
            if (string.IsNullOrEmpty(this.cmbNotifyType.Text))
            {
                MessageDialog.Show("请选择同步方式!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(this.cmbParaType.Text))
            {
                MessageDialog.Show("请选择要下载的参数类型!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            if (string.IsNullOrEmpty(this.cmbVersion.Text))
            {
                MessageDialog.Show("请选择要下载的参数版本!", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }

            return true;
        }


        private void btnParamsDownLoad_Click(object sender, RoutedEventArgs e)
        {
            if (selectParaCollection.Count < 1)
            {
                MessageDialog.Show("请选择要下载的参数", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            List<ParamsData> paraList = new List<ParamsData>();
            List<TJComm.DeviceRange> list = new List<TJComm.DeviceRange>();



            ILookup<string, SelectPara> lookUpPara = this.selectParaCollection.ToLookup(temp => temp.ParaTypeId);
            foreach (var temp in lookUpPara)
            {
                ParamsData pd = new ParamsData();
                foreach (var dd in temp)
                {
                    pd.paramType = dd.ParaTypeId.ToUShort();
                    pd.paramVersion = dd.ParaVersion.ToUShort();
                    pd.paramSynFlag = dd.ParaSynFlag.ToByte();
                    pd.paramFileName = dd.ParaFileName;
                }
                paraList.Add(pd);
            }

            if (this.rbLine.IsChecked.Value)
            {
                list.Add(new TJComm.DeviceRange { special_flag = 0, stationId = 0, deviceRange = new List<uint>() });
                int res = BuinessRule.GetInstace().commProcess.SpecialParamsDownLoadNotify(paraList, list);
                if (res == 0)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Special_Param_DownLoad_Notify, "0", "参数下载命令发送成功");
                }
                else
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Special_Param_DownLoad_Notify, "1", "参数下载命令发送失败");
                }
                string tip = AFC.WS.ModelView.UIContext.MessageCfg.getMessageContent("1310", res.ToString());
                MessageDialog.Show(tip, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (this.rbStation.IsChecked.Value)
            {
                list.Add(new DeviceRange { special_flag = 1, stationId = this.stationId.ConvertHexStringToUshort(), deviceRange = new List<uint>() });
                int res = BuinessRule.GetInstace().commProcess.SpecialParamsDownLoadNotify(paraList, list);
                if (res == 0)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Special_Param_DownLoad_Notify, "0", "参数下载命令发送成功");
                }
                else
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Special_Param_DownLoad_Notify, "1", "参数下载命令发送失败");
                }
                string tip = AFC.WS.ModelView.UIContext.MessageCfg.getMessageContent("1310", res.ToString());
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

                int res = BuinessRule.GetInstace().commProcess.SpecialParamsDownLoadNotify(paraList, list);
                if (res == 0)
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Special_Param_DownLoad_Notify, "0", "参数下载命令发送成功");
                }
                else
                {
                    BuinessRule.GetInstace().logManager.AddLogInfo(OperationCode.Special_Param_DownLoad_Notify, "1", "参数下载命令发送失败");
                }
                string tip = AFC.WS.ModelView.UIContext.MessageCfg.getMessageContent("1310", res.ToString());
                MessageDialog.Show(tip, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);

            }
        }

        private void btnAddPara_Click(object sender, RoutedEventArgs e)
        {
            if (this.CheckValid())
            {
                SelectPara info = new SelectPara();
                info.ParaTypeId = (this.cmbParaType.SelectedValue as BasiParaTypeInfo).para_type;
                info.ParaTypeName = (this.cmbParaType.SelectedValue as BasiParaTypeInfo).para_desc;
                info.ParaVersion = (this.cmbVersion.SelectedValue as ParaVersionInfo).para_version.ToString();
                info.ParaFileName = (this.cmbVersion.SelectedValue as ParaVersionInfo).para_file_name;
                info.ParaSynFlag = (this.cmbNotifyType.SelectedValue as notify).notifyID;
                info.ParaSynName = (this.cmbNotifyType.SelectedValue as notify).notifyName;
                info.IsParaChecked = false;

                int res = this.selectParaCollection.Count(temp => temp.ParaTypeId.Equals(info.ParaTypeId));
                if (res < 1)
                {
                    selectParaCollection.Add(info);
                }
                
            }
        }

        private void btnDelPara_Click(object sender, RoutedEventArgs e)
        {
            List<SelectPara> tempList = new List<SelectPara>();
            for (int i = 0; i < this.listView2.Items.Count; i++)
            {
                SelectPara sd = this.listView2.Items[i] as SelectPara;
                if (sd.IsParaChecked)
                {
                    tempList.Add(sd);
                }
            }

            for (int i = 0; i < tempList.Count; i++)
            {
                this.selectParaCollection.Remove(tempList[i]);
            }

        }


        public override void UnLoadControls()
        {
            this.selectDataCollection.Clear();
            this.selectParaCollection.Clear();
            this.devList.Clear();
            this.cmbVersion.ItemsSource = null;
            //this.listView1.Items.Clear();
            //this.listView2.Items.Clear();
            //this.listBox1.Items.Clear();
            base.UnLoadControls();
        }



        private class notify
        {
            private string _notifyName;
            private string _notifyID;

            public string notifyName
            {
                get
                {
                    return this._notifyName;
                }
                set
                {
                    this._notifyName = value;
                }
            }

            public string notifyID
            {
                get
                {
                    return this._notifyID;
                }
                set
                {
                    this._notifyID = value;
                }
            }
        }

        internal class SelectPara : System.ComponentModel.INotifyPropertyChanged
        {
            private string paraTypeId;

            public string ParaTypeId
            {
                set
                {
                    this.paraTypeId = value;
                    this.PorperyChangeHandle("ParaTypeId");
                }
                get { return this.paraTypeId;}



            }

            private string paraTypeName;
            public string ParaTypeName
            {
                set
                {
                    this.paraTypeName = value;
                    this.PorperyChangeHandle("ParaTypeName");
                }
                get
                {
                    return this.paraTypeName;
                }
            }

            private bool isParaChecked;

            public bool IsParaChecked
            {
                set
                {
                    this.isParaChecked = value;
                    this.PorperyChangeHandle("IsParaChecked");
                }
                get { return this.isParaChecked; }
            }

            private string paraVersion;

            public string ParaVersion
            {
                set
                {
                    this.paraVersion = value;
                    this.PorperyChangeHandle("ParaVersion");
                }
                get
                {
                    return this.paraVersion;
                }
            }


            private string paraFileName;

            public string ParaFileName
            {
                set
                {
                    this.paraFileName = value;
                    this.PorperyChangeHandle("ParaFileName");
                }
                get { return this.paraFileName; }
            }

            private string paraSynFlag;

            public string ParaSynFlag
            {
                set
                {
                    this.paraSynFlag = value;
                    this.PorperyChangeHandle("ParaSynFlag");
                }
                get { return this.paraSynFlag; }
            }


            private string paraSynName;

            public string ParaSynName
            {
                set
                {
                    this.paraSynName = value;
                    this.PorperyChangeHandle("ParaSynName");
                }
                get { return this.paraSynName; }
            }

            public void PorperyChangeHandle(string name)
            {
                if (this.PropertyChanged != null)
                    this.PropertyChanged.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }

            #region INotifyPropertyChanged 成员

            public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

            #endregion
        }
        #endregion
    }
}
