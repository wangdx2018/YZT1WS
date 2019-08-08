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

namespace AFC.WS.UI.UIPage.TickStoreManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Common;
    using AFC.WS.ModelView.Actions.TickStoreActions;
    using System.Data;
    using AFC.WS.BR.TickMonyBoxManager;
    using AFC.WS.ModelView.Actions.CommonActions;
   

    /// <summary>
    /// edited by wangdx 20120116 增加了废票的处理
    /// TickCallIn.xaml 的交互逻辑
    /// </summary>
    public partial class TickCallIn : UserControlBase
    {

        /// <summary>
        /// 票卡操作数据集合
        /// </summary>
        private ObservableCollection<TickOperationData> list = new ObservableCollection<TickOperationData>();


        private BasiStationInfo callOutStationInfo = null;


        private string tickStatus;


        public TickCallIn()
        {
            InitializeComponent();
            this.cmbTickStoreType.SelectionChanged += new SelectionChangedEventHandler(cmbTickStoreType_SelectionChanged);
            this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.btnCallOut.Click += new RoutedEventHandler(btnCallOut_Click);

            List<BasiStationInfo> stationList=BuinessRule.GetInstace().GetAllStationAndLCInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
            if (stationList != null &&
               stationList.Count > 0)
            {
                try
                {
                    stationList.Remove(stationList.Single(temp => temp.station_id == SysConfig.GetSysConfig().LocalParamsConfig.StationCode));
                }
                catch (Exception e)
                {
                    
                }
            }

            this.cmbOutStation.ItemsSource = stationList;
            this.cmbOutStation.DisplayMemberPath="station_cn_name";
            this.txtTickNo.Text = string.Empty;
            this.txtReaNum.Text = string.Empty;
            this.txtRealNo.Text = string.Empty;
            this.cmbOutStation.SelectionChanged += new SelectionChangedEventHandler(cmbOutStation_SelectionChanged);
            this.cmbTickStatus.SelectionChanged += new SelectionChangedEventHandler(cmbTickStatus_SelectionChanged);
        }

        private void cmbTickStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.txtTickNo.Text = string.Empty;
            this.txtRealNo.Text = string.Empty;
            this.txtReaNum.Text = string.Empty;
            this.txtRealNo.IsEnabled = true;
            this.btnAdd.IsEnabled = true;

            BasiTickManaTypeInfo info = this.cmbTickStoreType.SelectedItem as BasiTickManaTypeInfo;
            if (info != null)
            {
                string type = info.tick_mana_type;
                if (!string.IsNullOrEmpty(type))
                {
                    tickStatus = (this.cmbTickStatus.SelectedItem as ComboBoxItem).Tag.ToString();
                    TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(type,tickStatus);
                    if (string.IsNullOrEmpty(tickStoreInfo.tick_mana_type))
                        this.txtTickNo.Text = "0";
                    else
                    this.txtTickNo.Text = tickStoreInfo.in_store_num.ToString();
                }
                else
                {
                    this.txtTickNo.Text = "没有该库存类型";
                    this.txtRealNo.IsEnabled = false;
                    this.btnAdd.IsEnabled = false;
                }
            }
        }

        private void cmbOutStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.callOutStationInfo = this.cmbOutStation.SelectedItem as BasiStationInfo;
        }

       

        private void btnCallOut_Click(object sender, RoutedEventArgs e)
        {
            if (callOutStationInfo == null)
            {
                MessageDialog.Show("请选择出库车站", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return;
            }


            DoublePrimissionAction action = new DoublePrimissionAction();
            action.CurrentOperationId = BuinessRule.GetInstace().OperatorId;
            action.subAction = new TickCallInAction();
            List<QueryCondition> listQueryCondition = new List<QueryCondition>();
            listQueryCondition.Add(new QueryCondition { bindingData = "callOutStation", value = callOutStationInfo.station_id });
          
            for (int i = 0; i < list.Count; i++)
            {
                listQueryCondition.Add(new QueryCondition { bindingData = list[i].TickStoreType, value = list[i].TickNum, controlName=list[i].TickStatusName });
            }
        
            if (action.CheckValid(listQueryCondition))
            {
                ResultStatus result = action.DoAction(listQueryCondition);
                if (result == null)
                    return;
                if (result.resultCode == 0 && result.resultData.ToString() == "0")
                {
                    if (cbIsPrint.IsChecked.Value)
                    {
                        this.Print();

                    }
                    this.list.Clear();
                    UpdateTotalCount();

                    BasiTickManaTypeInfo info = this.cmbTickStoreType.SelectedItem as BasiTickManaTypeInfo;
                    string type = info.tick_mana_type;
                    tickStatus = (this.cmbTickStatus.SelectedItem as ComboBoxItem).Tag.ToString();
                    TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(type, tickStatus);
                    if (string.IsNullOrEmpty(tickStoreInfo.tick_mana_type))
                        this.txtTickNo.Text = "0";
                    else
                        this.txtTickNo.Text = tickStoreInfo.in_store_num.ToString();
                    this.txtReaNum.Text = string.Empty;
                    this.txtRealNo.Text = string.Empty;
                }
            }
            //throw new NotImplementedException();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var temp = new ObservableCollection<TickOperationData>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsChecked)
                {
                    temp.Add(list[i]);
                }
            }
            for (int i = 0; i < temp.Count; i++)
            {
                list.Remove(temp[i]);
            }
            UpdateTotalCount();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int inStoreNum;
            int checkOutNum;

            bool res = int.TryParse(this.txtTickNo.Text, out inStoreNum);
            if (!res)
            {
                MessageDialog.Show("票务室库存张数不合法", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            res = int.TryParse(this.txtReaNum.Text, out checkOutNum);
            if (!res)
            {
                MessageDialog.Show("输入张数不合法", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (this.CheckUIValid(inStoreNum, checkOutNum))
            {
                BasiTickManaTypeInfo btmt = this.cmbTickStoreType.SelectedItem as BasiTickManaTypeInfo;
                if (btmt != null)
                {
                    TickOperationData tod = new TickOperationData
                    {
                        IsChecked = false,
                        TickNum = checkOutNum,
                        TickStoreTypeName = btmt.tick_mana_type_name,
                        TickStoreType = btmt.tick_mana_type,
                        OperatorId = BuinessRule.GetInstace().brConext.CurrentOperatorId,
                        UpdateTime = DateTime.Now.ToString("HH:mm:ss"),
                        UpdateDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        TickStatus=this.tickStatus=="00"?"正常":"废票",
                        TickStatusName=this.tickStatus
                     
                    };
                    this.AddTickManaTypeInfoToList(tod);
                    UpdateTotalCount();
                }
            }
        }

        private void UpdateTotalCount()
        {
            int total = 0; 
            if (list.Count == 0)
                this.labTotal.Content = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                total = list[i].TickNum + total;
            }
            this.labTotal.Content = total.ToString();
        }

        private void cmbTickStoreType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            cmbTickStatus_SelectionChanged(sender, e);
            
            //throw new NotImplementedException();
        }

        public override void InitControls()
        {
            List<BasiTickManaTypeInfo> list = BuinessRule.GetInstace().GetBasiTickManaTypeInfo();
            this.cmbTickStoreType.ItemsSource = list;
            this.cmbTickStoreType.DisplayMemberPath = "tick_mana_type_name";
            this.listView1.ItemsSource = this.list;
            UpdateTotalCount();

            tickCallOut.InitControls();
            tickStoreAdjust.InitControls();
        }

        /// <summary>
        /// 检查UI输入是否合法
        /// </summary>
        /// <param name="instoreNum">当前库存张数</param>
        /// <param name="checkNum">调出数量</param>
        /// <returns>成功返回true，否则返回false</returns>
        private bool CheckUIValid(int instoreNum, int checkNum)
        {
            //if (instoreNum < checkNum)
            //{
            //    MessageDialog.Show("领用张数必须小于当前库存张数", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            //    return false;
            //}
            return true;
        }

        public override void UnLoadControls()
        {
            this.list.Clear();
            this.callOutStationInfo = null;
            this.cmbOutStation.SelectedIndex = -1;
            tickCallOut.UnLoadControls();
            tickStoreAdjust.UnLoadControls();
            //base.UnLoadControls();
        }

        /// <summary>
        /// 增加库存关
        /// </summary>
        /// <param name="data"></param>
        private void AddTickManaTypeInfoToList(TickOperationData data)
        {
            if (data == null)
            {
                WriteLog.Log_Error("param is null");
                return;
            }
            if (string.IsNullOrEmpty(data.TickStoreType))
            {
                WriteLog.Log_Error("tick store type is null or empty");
                return;
            }
            try
            {
                int res = list.Count(temp => temp.TickStoreType.Equals(data.TickStoreType)&&
                    temp.TickStatusName.Equals(data.TickStatusName));
                if (res == 1)
                {
                    TickOperationData last = list.Single(temp => temp.TickStoreType.Equals(data.TickStoreType));
          
                    last.TickNum = data.TickNum + last.TickNum;

                }
                else
                    this.list.Add(data);
            }
            catch (Exception ex)
            {

            }

        }

   


        private void  Print()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            //AFC.WS.UI.UIPage.CashManager.CrystalRptData rptData = new AFC.WS.UI.UIPage.CashManager.CrystalRptData();

           string locationStation=BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;

            string callOutStation=this.callOutStationInfo.station_cn_name;

            dict.Add("ReportTitle1", "票卡入库单");
            dict.Add("RequestBatchNo", TickMonyBoxHelp.Instance.GetSequenceNextVal().ToString());
            dict.Add("OperatorID", BuinessRule.GetInstace().brConext.CurrentOperatorId);
            dict.Add("DispatchLocationID",callOutStation);
            dict.Add("RequestLocationID", locationStation);
            dict.Add("DispatchType", "入库");

            DataTable dt = new DataTable();

            dt.Columns.Add("StorageManagerType", typeof(string));
            dt.Columns.Add("TicketStateName", typeof(string));
            dt.Columns.Add("TicketBoxNumber", typeof(string));
            dt.Columns.Add("DispatchWay", typeof(string));


            for (int i = 0; i < list.Count; i++)
            {
                
                dt.Rows.Add(list[i].TickStoreTypeName, 
                    list[i].TickStatus,
                    list[i].TickNum.ToString(), 
                    callOutStation + "——>" + locationStation);
            }
            //rptData.ShowRptDialog(new AFC.WS.UI.UIPage.TickStoreManager.CrystalTickInOutReport(),
            //    dict, dt);
        }

      
    }
}
