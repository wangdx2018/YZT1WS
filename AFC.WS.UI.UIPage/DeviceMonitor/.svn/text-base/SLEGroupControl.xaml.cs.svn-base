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
using AFC.WS.UI.Common;
using AFC.WS.UI.CommonControls;
using System.Data;
using System.Collections;
using Microsoft.Windows.Controls;
using Microsoft.Windows.Controls.Primitives;
using AFC.WS.BR.DeviceMonitor;
using AFC.WS.BR;
using AFC.WS.Model.DB;
using TJComm;
using System.Collections.ObjectModel;
using AFC.WS.UI.Config;

namespace AFC.WS.UI.UIPage.DeviceMonitor
{
    /// <summary>
    /// SLEGroupControl.xaml 的交互逻辑
    /// </summary>
    public partial class SLEGroupControl : UserControlBase, ITransParams
    {
        /// <summary>
        /// 单选控件变量
        /// </summary>
        private RiadioButtonExtend radioButton = null;

        private List<QueryCondition> list = new List<QueryCondition>();

        private Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

        private Dictionary<string, string> devTypeDict = new Dictionary<string, string >();
        /// <summary>
        /// 复选控件对象
        /// </summary>
        private CheckBoxExtend checkBox = null;

        /// <summary>
        /// 车站
        /// </summary>
        private string stationCode = null;

        /// <summary>
        /// 站厅名称
        /// </summary>
        private string valueHall = null;

        /// <summary>
        /// 组名称
        /// </summary>
        private string valueGroup = null;

        /// <summary>
        /// 组代码
        /// </summary>
        private string groupId = null;

        /// <summary>
        /// 站厅ID
        /// </summary>
        private string hallId = null;

        /// <summary>
        /// 设备类型代码
        /// </summary>
        private string devTypeCode = null;

        /// <summary>
        /// 设备类型
        /// </summary>
        private string valueDevType = null;


        /// <summary>
        /// 选择要下发的设备类型
        /// </summary>
        private string selDevTypeId = null;

        /// <summary>
        /// 选择要下发的命令类型
        /// </summary>
        private string ctrCommand = null;

        /// <summary>
        /// 选中的的checkbox
        /// </summary>
        private RiadioButtonExtend SelCtrCheckbox = null;


        private string specialflag = null;


        private string SelDeviceID = null;

        /// <summary>
        /// 上一次发送的指令名称
        /// </summary>
        private string selectResult = null;

        /// <summary>
        /// 存放重发命令结果
        /// </summary>
        private List<int[]> listResult = new List<int[]>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public SLEGroupControl()
        {
            try
            {
                InitializeComponent();
                //CreateTemplateColumn();
            }
            catch { }
        }

        /// <summary>
        /// 发送控制命令
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateRadioButton() == true)
                {
                    SendMessageToServer();
                }
                       
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("发送控制命令异常" + ex.ToString());
            }
        }


        /// <summary>
        /// 发送报文
        /// </summary>
        /// <returns></returns>
        public void SendMessageToServer()
        {
            try
            {
                Wrapper.Instance.AddQueryConditionToList(list,"deviceRange", specialflag);
                Wrapper.Instance.AddQueryConditionToList(list,"commandType", ctrCommand);
                Wrapper.Instance.AddQueryConditionToList(list,"stationID", stationCode);
                Wrapper.Instance.AddQueryConditionToList(list,"deviceID", SelDeviceID);
                Wrapper.Instance.AddQueryConditionToList(list,"hallID", hallId);
                Wrapper.Instance.AddQueryConditionToList(list,"groupID", groupId);
                Wrapper.Instance.AddQueryConditionToList(list,"devTypeID", selDevTypeId);
                Wrapper.Instance.AddQueryConditionToList(list,"reSendDict", dict);
                Wrapper.Instance.AddQueryConditionToList(list,"devTypeDict", devTypeDict);


                IAction action = new AFC.WS.ModelView.Actions.DeviceMonitor.SLEGroupControlAction();
                if (action.CheckValid(list))
                {
                    action.DoAction(list);
                }
                dict = null;
                devTypeDict = null;
                //IEnumerable drv = dgCommandResult.Items.SourceCollection;
                //ObservableCollection<CommandResultInfo> cmdResult = SLEGroupControlManager.Instance.GetSendCommandResultInfo(drv,1, false);
                //this.dgCommandResult.DataContext = new TeamModel { CmdResults = cmdResult };

     
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        /// <summary>
        /// 验证设备范围是否被设置
        /// </summary>
        /// <returns>true:成功设置，false:失败</returns>
        private bool ValidateRadioButton()
        {
            try
            {
                if (true != this.AllDevice.IsChecked && true != this.OneDevice.IsChecked && true != this.stationHallAndGroupAndType.IsChecked && true!= this.AllLine.IsChecked)
                {
                    MessageDialog.Show("此指令发送需要选择范围，请选择范围！", "确定", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    return false;
                }
                else
                {
                    if (true == this.OneDevice.IsChecked)
                    {
                        if (null == this.dgDevInfo.SelectedItem)
                        {
                            MessageDialog.Show("请选择单个设备！", "确定", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
            return false;
        }


        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void UserControlBase_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                //initControl();
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }

        }

        /// <summary>
        /// 设置车站站厅
        /// </summary>
        private IEnumerable<string> SetStationHallInfo(string stationCode)
        {
            IEnumerable<string> query = null;
            if (listBoxHall != null)
            {

                try
                {
                    //listBoxHall.Items.Clear();
                    if (BuinessRule.GetInstace().GetBasiStationHall(stationCode) != null)
                    {

                        query = from item in BuinessRule.GetInstace().GetBasiStationHall(stationCode)
                                orderby item.station_hall_id
                                select item.station_hall_name;
                    }
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    return null;
                }

            }
            return query;
        }

        /// <summary>
        /// 设置工作群组
        /// </summary>

        private IEnumerable<string> SetDevGroup(string stationCode)
        {
            IEnumerable<string> query = null;
            if (listBoxGroup != null)
            {
                
                try
                {
                    //listBoxGroup.Items.Clear();
                    if (BuinessRule.GetInstace().GetBasiHallGroup(stationCode) != null)
                    {
                        query = from item in BuinessRule.GetInstace().GetBasiHallGroup(stationCode)
                                orderby item.hall_group_id
                                select item.hall_group_name;
                    }
         
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    return null;
                }
            }
            return query;
        }


        /// <summary>
        /// 设置设备类型
        /// </summary>

        private IEnumerable<string> SetDevTypeInfo(string stationCode)
        {
            IEnumerable<string> query = null;
            if (listBoxDevType != null)
            {
                
                try
                {
                    //listBoxDevType.Items.Clear();
                    if (BuinessRule.GetInstace().GetAllDevciceType() != null)
                    {

                       query = from item in  BuinessRule.GetInstace().GetAllDevciceType()
                        where (item.device_type == "01" || item.device_type == "02" || item.device_type == "06" || item.device_type == "04")

                        orderby item.device_type
                        select item.device_name;
                           
                    }
                    
                }
                catch (Exception ee)
                {
                    Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    return null;
                }
            }
            return query;
        }

        /// <summary>
        /// 设置车站信息
        /// </summary>

        private void SetComBoxStation(string lineId)
        {
            Wrapper.FullComboBox(this.comboxStation, BuinessRule.GetInstace().GetAllStationInfo(lineId), "station_cn_name", "station_id", true);
        }


        /// <summary>
        /// RadioButton选中事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void RiadioButtonExtend_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is RiadioButtonExtend)
                {
                    radioButton = sender as RiadioButtonExtend;
                    ctrCommand = radioButton.Uid;
                    SelCtrCheckbox = radioButton;
                    checkBox = null;

                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("RiadioButtonExtend_Checked异常" + ex.ToString());
            }
        }

        /// <summary>
        /// 选择设备范围复选框触发
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void checkBoxStationHall_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (true != stationHallAndGroupAndType.IsChecked)
                {
                    stationHallAndGroupAndType.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("checkBoxStationHall_Checked" + ex.ToString());
            }
        }

        /// <summary>
        /// 选择全部设备或单个设备时发生。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void AllDevice_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
               
                if (sender is RiadioButtonExtend)
                {
                    RiadioButtonExtend devRangeRadio = sender as RiadioButtonExtend;

                    specialflag = devRangeRadio.Uid;
                    if ("LCWS".Equals(SysConfig.GetSysConfig().LocalParamsConfig.SystemName))
                    {
                        if (this.comboxStation.SelectedItem != null)
                        {
                            stationCode = Wrapper.GetComboBoxUid(this.comboxStation).ToString();
                        }

                    }
                    else
                    {
                        stationCode = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                    }

                    if (OneDevice.IsChecked == false)
                    {
                        this.dgDevInfo.ItemsSource = null;
                        this.dgDevInfo.ItemsSource = SLEGroupControlManager.Instance.GetDevInfo(null, null, null, stationCode);
                      
                    }
                    if (stationHallAndGroupAndType.IsChecked == false)
                    {
                        this.checkBoxStationHall.IsChecked = false;

                        this.checkBoxworkGroup.IsChecked = false;

                        this.checkBoxdevType.IsChecked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("AllDevice_Checked函数异常:" + ex.ToString());
            }
        }

        #region ITransParams 成员
        /// <summary>
        /// 设置参数下载控制命令，当点击参数或软件下发时到此界面，
        /// 
        /// 并且参数下发打开为选中状态。
        /// </summary>
        /// <param name="paramsData"></param>
        /*public void SetParams(object paramsData)
        {
            try
            {
                if (paramsData != null)
                {
                    if (paramsData.ToString() == "1")
                    {
                        this.XPSoftWareSend.IsExpanded = true;

                        XPSoftWareSend.Header = "软件版本下发";

                        softWareSend.Content = "软件下发";

                        softWareSend.IsChecked = false;
                    }
                    if (paramsData.ToString() == "2")
                    {
                        this.XPSoftWareSend.IsExpanded = true;

                        XPSoftWareSend.Header = "参数版本下发";

                        softWareSend.Content = "参数下发";

                        softWareSend.IsChecked = false;
                    }
                    if (paramsData.ToString() == "3")
                    {
                        this.XPSoftWareSend.IsExpanded = false;

                        XPSoftWareSend.Header = "参数版本下发";

                        softWareSend.Content = "参数下发";

                        softWareSend.IsChecked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }*/

        #endregion


        /// <summary>
        /// 选择ListBox中内容时，设备信息表数据发生变化。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void listBoxHall_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox listbox = sender as ListBox;
                specialflag = listbox.Uid;

                //if (this.stationHallAndGroupAndType.IsChecked==false)
                //{
                //    this.stationHallAndGroupAndType.IsChecked = true;                
                //}
                if ("listBoxHall" == listbox.Name)
                {
                    if (!string.IsNullOrEmpty(listbox.SelectedValue.ToString()))
                    {
                        hallId = BuinessRule.GetInstace().GetBasiStationHall(stationCode).Where(p => p.station_hall_name == listbox.SelectedValue.ToString()).GetTContext<BasiStationHallIdInfo>().station_hall_id;
                    }
                   
                }
                if ("listBoxGroup" == listbox.Name)
                {
                    if(!string.IsNullOrEmpty(listbox.SelectedValue.ToString())){
                        groupId = BuinessRule.GetInstace().GetBasiHallGroup(stationCode).Where(p => p.hall_group_name == listbox.SelectedValue.ToString() && p.station_hall_id == hallId).GetTContext<BasiHallGroupIdInfo>().hall_group_id;
                    }
                 
                }
                if ("listBoxDevType" == listbox.Name)
                {
                    if (!string.IsNullOrEmpty(listbox.SelectedValue.ToString()))
                    {

                        devTypeCode = BuinessRule.GetInstace().GetAllDevciceType().Where(p => p.device_name == listbox.SelectedValue.ToString()).GetTContext<BasiDevTypeInfo>().device_type;
                        selDevTypeId = devTypeCode;
                    }
                   
                }

                this.dgDevInfo.ItemsSource=SLEGroupControlManager.Instance.GetDevInfo(hallId, groupId, devTypeCode, stationCode);
                
          
         
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("群组控制选择更新异常:" + ex.ToString());
            }
        }

        /// <summary>
        /// 重置命令，取消所有选择。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                valueHall = null;

                valueGroup = null;

                groupId = null;

                hallId = null;

                selDevTypeId = null;


                AllDevice.IsChecked = false;

                stationHallAndGroupAndType.IsChecked = false;

                checkBoxStationHall.IsChecked = false;

                checkBoxworkGroup.IsChecked = false;

                checkBoxdevType.IsChecked = false;

                BarControlCommand.AllowMultipleExpands = false;

                OneDevice.IsChecked = false;

                this.dgDevInfo.SelectedItem = null;

                initControl();

                SelCtrCheckbox.IsChecked = false;
               

                //dgCommandResult.DataContext = new TeamModel { CmdResults = null };
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("btnSet_Click事件异常:" + ex.ToString());
            }
        }


        /// <summary>
        /// 单个设备重发命令
        /// </summary>
        /// <param name="commandResult">控制命令结果</param>
        /// <param name="commandName">控制命令名称</param>
        private void ReSendCommand(string commandName)
        {
            try
            {
                int[] result = new int[2];

                int isAcross = 0;

                //重发控制命令
                specialflag = "4";
                //发命令
                SendMessageToServer();
                
                //取得反馈
                if (isAcross != 0)
                {
                    listResult.Add(result);
                }

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }


        /// <summary>
        /// 车站选择，LCWS中使用。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboxStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (sender is ComboBox)
                {
                    ComboBoxExtend commbox = sender as ComboBoxExtend;
                    this.hallId = null;
                    this.groupId = null;
                    this.devTypeCode = null;

                    if ("LCWS".Equals(SysConfig.GetSysConfig().LocalParamsConfig.SystemName))
                    {
                        if (commbox.SelectedItem != null)
                        {
                            stationCode = Wrapper.GetComboBoxUid(this.comboxStation).ToString();
                        }

                    }
                    OneDevice.IsEnabled = true;
                    this.listBoxHall.ItemsSource = SetStationHallInfo(stationCode);
                    this.listBoxGroup.ItemsSource =SetDevGroup(stationCode);
                    this.listBoxDevType.ItemsSource = SetDevTypeInfo(stationCode);

                    this.dgDevInfo.ItemsSource = SLEGroupControlManager.Instance.GetDevInfo(null, null, null, stationCode);
                }
                //initControl();
                 
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        /// <summary>
        /// DataGrid行选中事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgDevInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {   
                if (this.dgDevInfo.SelectedItem != null)
                {
                    this.OneDevice.IsChecked = true;

                    string devName = Wrapper.Instance.GetObjectValue(this.dgDevInfo.SelectedItem, "设备类型").ToString();

                    BasiDevTypeInfo devTypeInfo = BuinessRule.GetInstace().GetAllDevciceType().Where(p => p.device_name == devName).GetTContext<BasiDevTypeInfo>();

                    selDevTypeId = devTypeInfo.device_type;
                    SelDeviceID = Wrapper.Instance.GetObjectValue(this.dgDevInfo.SelectedItem, "设备编码").ToString();
                }
                
                DataGrid dataGrid = sender as DataGrid;

                if (e.AddedItems != null && e.AddedItems.Count > 0)
                {
                    int index = dataGrid.SelectedIndex;


                    SelectDevice(index, dataGrid);
                }

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        /// <summary>
        /// 当选中当前行时，CheckBox为选中状态。
        /// </summary>
        /// <param name="index">选中行索引</param>
        /// <param name="dataGrid">选中控件</param>
        private void SelectDevice(int index, DataGrid dataGrid)
        {
            try
            {
                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(dataGrid.Items[i]);
                    if (row != null)
                    {
                        DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);
                      
                        DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(0) as DataGridCell;
                        if (i != index)
                        {
                            if (cell != null)
                            {
                                if ((cell.Content as System.Windows.Controls.CheckBox).IsChecked == true)
                                    (cell.Content as System.Windows.Controls.CheckBox).IsChecked = false;

                            }
                        }
                        else
                        {
                            (cell.Content as System.Windows.Controls.CheckBox).IsChecked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
        }

        /// <summary>
        /// 查找控件
        /// </summary>
        /// <typeparam name="T">行对象</typeparam>
        /// <param name="parent">父类</param>
        /// <returns>返回</returns>
        private static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null) child = GetVisualChild<T>(v);
                if (child != null) break;
            }
            return child;
        }

        /// <summary>
        /// 获取行对象
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public IEnumerable<Microsoft.Windows.Controls.DataGridRow> GetDataGridRows(Microsoft.Windows.Controls.DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as Microsoft.Windows.Controls.DataGridRow;
                if (null != row) yield return row;
            }
        }


        private void initControl()
        {
            comboxStation.Visibility = Visibility.Collapsed;
           if ("LCWS" == SysConfig.GetSysConfig().LocalParamsConfig.SystemName)
            {
                comboxStation.Visibility = Visibility.Visible;
                AllLine.IsEnabled= true;
                OneDevice.IsEnabled = true;
                SetComBoxStation(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
                stationCode = Wrapper.GetComboBoxUid(this.comboxStation).ToString();
                
            }
            BarControlCommand.AllowMultipleExpands = false;

            if ("SCWS".Equals(SysConfig.GetSysConfig().LocalParamsConfig.SystemName))
            {
                stationCode = SysConfig.GetSysConfig().LocalParamsConfig.StationCode;
                AllLine.IsEnabled = false;
                OneDevice.IsEnabled = true;
                this.listBoxHall.ItemsSource = SetStationHallInfo(stationCode);
                this.listBoxGroup.ItemsSource = SetDevGroup(stationCode);
                this.listBoxDevType.ItemsSource= SetDevTypeInfo(stationCode);

                this.dgDevInfo.ItemsSource = SLEGroupControlManager.Instance.GetDevInfo(null, null, null, stationCode);
            }
            //初始化变量
            selDevTypeId =null;
            ctrCommand   = null;
            SelCtrCheckbox = null;
            specialflag=null;
            this.AllDevice.IsChecked = true;
            //this.ElecClose.IsChecked = true;
        }

        private void AllLine_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                this.checkBoxStationHall.IsChecked = false;

                this.checkBoxworkGroup.IsChecked = false;

                this.checkBoxdevType.IsChecked = false;

                this.stationCode = null;

                if (sender is RiadioButtonExtend)
                {
                    RiadioButtonExtend devRangeRadio = sender as RiadioButtonExtend;
                    specialflag = devRangeRadio.Uid;

                    if (OneDevice.IsChecked == false)
                    {
                        WriteLog.Log_Info(" begin time=" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                        this.dgDevInfo.ItemsSource = SLEGroupControlManager.Instance.GetDevInfo(null, null, null,null);
                        WriteLog.Log_Info(" end time=" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("AllDevice_Checked函数异常:" + ex.ToString());
            }
        }

        public override void UnLoadControls()
        {

            this.AllLine.IsChecked = false;
            this.AllDevice.IsChecked = false;
            this.comboxStation.SelectedIndex = -1;
            this.stationHallAndGroupAndType.IsChecked = false;
            this.OneDevice.IsChecked = false;
        }


        public override void InitControls()
        {
            this.initControl();
            //base.InitControls();
        }


        #region ITransParams 成员

        public void SetParams(object paramsData)
        {
            
        }

        #endregion
    }
}
