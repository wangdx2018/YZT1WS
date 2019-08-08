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
using System.ComponentModel;


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
    /// TickCallOut.xaml 的交互逻辑
    /// </summary>
    public partial class TickCallOut : UserControlBase
    {

        /// <summary>
        /// 票卡操作数据集合
        /// </summary>
        private ObservableCollection<TickOperationData> list = new ObservableCollection<TickOperationData>();


        private BasiStationInfo callOutStationInfo = null;


        private string tickStatus = string.Empty;

        public TickCallOut()
        {
            InitializeComponent();
            this.cmbTickStoreType.SelectionChanged += new SelectionChangedEventHandler(cmbTickStoreType_SelectionChanged);
            this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.btnCallOut.Click += new RoutedEventHandler(btnCallOut_Click);
            this.txtTickNo.Text = string.Empty;
            this.txtReaNum.Text = string.Empty;

            List<BasiStationInfo> stationList = BuinessRule.GetInstace().GetAllStationAndLCInfo(SysConfig.GetSysConfig().LocalParamsConfig.LineCode);
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
            this.cmbOutStation.DisplayMemberPath = "station_cn_name";

            this.cmbOutStation.SelectionChanged += new SelectionChangedEventHandler(cmbOutStation_SelectionChanged);
            this.cmbTickStatus.SelectionChanged += new SelectionChangedEventHandler(cmbTickStatus_SelectionChanged);
        }

        void cmbTickStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.cmbTickStoreType_SelectionChanged(sender, e);
            //throw new NotImplementedException();
        }

        private void cmbOutStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.callOutStationInfo = this.cmbOutStation.SelectedItem as BasiStationInfo;
        }

        private void btnCallOut_Click(object sender, RoutedEventArgs e)
        {
            if (callOutStationInfo == null)
            {
                MessageDialog.Show("请选择入库车站", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return;
            }
            DoublePrimissionAction action = new DoublePrimissionAction();
            action.subAction = new TickCallOutAction();
            action.CurrentOperationId = BuinessRule.GetInstace().OperatorId;
          //  IAction action = new AFC.WS.ModelView.Actions.TickStoreActions.TickCallOutAction();
            List<QueryCondition> listQueryCondition= new List<QueryCondition>();
            listQueryCondition.Add(new QueryCondition { bindingData = "callOutStation", value = callOutStationInfo.station_id });
            for (int i = 0; i < list.Count; i++)
            {
                listQueryCondition.Add(new QueryCondition { bindingData = list[i].TickStoreType, value = list[i].TickNum,controlName=list[i].TickStatusName });
            }
            if (action.CheckValid(listQueryCondition))
            {
                ResultStatus result=action.DoAction(listQueryCondition);
                if (result == null)
                    return;
                if (result.resultCode == 0 && result.resultData.ToString() == "0")
                {
                    //  this.txtRealNo.Text = string.Empty;
                    //this.txtTickNo.Text = string.Empty;
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
                    //this.cmbOutStation.SelectedIndex = -1;
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
                return ;
            }
            res = int.TryParse(this.txtReaNum.Text, out checkOutNum);
            if (!res)
            {
                MessageDialog.Show("输入张数不合法", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }

            if (this.CheckUIValid(inStoreNum, checkOutNum))
            {
                BasiTickManaTypeInfo btmt=this.cmbTickStoreType.SelectedItem as BasiTickManaTypeInfo;
                if (btmt != null)
                {
                   TickOperationData tod= new TickOperationData
                     {
                         IsChecked = false,
                         TickNum = checkOutNum,
                         TickStoreTypeName = btmt.tick_mana_type_name,
                         TickStoreType = btmt.tick_mana_type,
                         OperatorId=BuinessRule.GetInstace().brConext.CurrentOperatorId,
                         UpdateTime = DateTime.Now.ToString("HH:mm:ss"),
                         UpdateDate=DateTime.Now.ToString("yyyy-MM-dd"),
                         TickStatus = this.tickStatus == "00" ? "正常" : "废票",
                         TickStatusName = this.tickStatus
                     };
                   this.AddTickManaTypeInfoToList(tod);
                   UpdateTotalCount();
                }
            }
        }

        private void cmbTickStoreType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.txtTickNo.Text = string.Empty;
            //this.txtRealNo.Text = string.Empty;
            //this.txtRealNo.IsEnabled = true;
            this.btnAdd.IsEnabled = true;

            //ComboBox cb = sender as ComboBox;
            BasiTickManaTypeInfo info = this.cmbTickStoreType.SelectedItem as BasiTickManaTypeInfo;
            if (info != null)
            {
                string type = info.tick_mana_type;
                if (!string.IsNullOrEmpty(type))
                {
                    tickStatus = (this.cmbTickStatus.SelectedItem as ComboBoxItem).Tag.ToString();
                    TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(type, tickStatus);
                    if (string.IsNullOrEmpty(tickStoreInfo.tick_mana_type))
                        this.txtTickNo.Text = "0";
                    else
                        this.txtTickNo.Text = tickStoreInfo.in_store_num.ToString();
                }
                else
                {
                    this.txtTickNo.Text = "没有该库存类型";
                 //   this.txtRealNo.IsEnabled = false;
                    this.btnAdd.IsEnabled = false;
                }

            }
            //throw new NotImplementedException();
        }

        public override void InitControls()
        {
            List<BasiTickManaTypeInfo> list = GetACCTickManaTypeInfos();
            this.cmbTickStoreType.ItemsSource = list;
            this.cmbTickStoreType.DisplayMemberPath = "tick_mana_type_name";
            this.listView1.ItemsSource = this.list;
        }

        public List<BasiTickManaTypeInfo> GetACCTickManaTypeInfos()
        {    
            //dusj modify begin 增加自定义票种
            //string cmd = string.Format("select t.* from basi_tick_mana_type_info t inner join tick_storage_info tsi on tsi.tick_mana_type=t.tick_mana_type and tsi.ticket_status='00' and tsi.in_store_num>0 and tsi.station_id='{0}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            string cmd = string.Format("select t.* from basi_tick_mana_type_info t inner join tick_storage_info tsi on tsi.tick_mana_type = t.tick_mana_type and tsi.ticket_status = '00' and tsi.in_store_num > 0 and tsi.station_id='{0}' union all select p.tick_mana_type,p.tick_mana_type_name,p.ticket_phy_type,''ticket_phy_type_name,''ticket_family_type,''ticket_family_type_name,p.card_issue_id from tick_valued_product_info  p inner join tick_storage_info ts on ts.tick_mana_type = p.tick_mana_type and ts.in_store_num > 0  and ts.station_id='{0}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            //dusj modify end 
            return DBCommon.Instance.GetTModelValue<BasiTickManaTypeInfo>(cmd);
        }


        /// <summary>
        /// 检查UI输入是否合法
        /// </summary>
        /// <param name="instoreNum">当前库存张数</param>
        /// <param name="checkNum">调出数量</param>
        /// <returns>成功返回true，否则返回false</returns>
        private bool CheckUIValid(int instoreNum, int checkNum)
        {
            if (instoreNum < checkNum)
            {
                MessageDialog.Show("出库张数必须小于当前库存张数", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }

        public override void UnLoadControls()
        {
            this.list.Clear();
            this.cmbOutStation.SelectedIndex = -1;
            this.callOutStationInfo = null;
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
                    temp.TickStatus.Equals(data.TickStatus));
                if (res == 1)
                {
                    TickOperationData last = list.Single(temp => temp.TickStoreType.Equals(data.TickStoreType));
                    int number = data.TickNum + last.TickNum;
                   TickStorageInfo info= BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(data.TickStoreType,data.TickStatusName);
                    if (number > info.in_store_num)
                    {
                        MessageDialog.Show("不能继续调出" + BuinessRule.GetInstace().GetBasiTickManaTypeInfoById(info.tick_mana_type).tick_mana_type_name + "，累计调出额不能超过库存", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        return;
                    }
                    last.TickNum = data.TickNum+last.TickNum;
                 
                }
                else
                    this.list.Add(data);
            }
            catch (Exception ex)
            {

            }

        }


        private void UpdateTotalCount()
        {
            int total = 0; ;
            if (list.Count == 0)
                this.labTotal.Content = string.Empty;
            for (int i = 0; i < list.Count; i++)
            {
                total = list[i].TickNum + total;
            }
            this.labTotal.Content = total.ToString();
        }

        private void Print()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            //AFC.WS.UI.UIPage.CashManager.CrystalRptData rptData = new AFC.WS.UI.UIPage.CashManager.CrystalRptData();

            string locationStation = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;

            string callOutStation = this.callOutStationInfo.station_cn_name;

            dict.Add("ReportTitle1", "票卡出库单");
            dict.Add("RequestBatchNo", TickMonyBoxHelp.Instance.GetSequenceNextVal().ToString());
            dict.Add("OperatorID", BuinessRule.GetInstace().brConext.CurrentOperatorId);
            dict.Add("DispatchLocationID", callOutStation);
            dict.Add("RequestLocationID", locationStation);
            dict.Add("DispatchType", "出库");

            DataTable dt = new DataTable();

            dt.Columns.Add("StorageManagerType", typeof(string));
            dt.Columns.Add("TicketStateName", typeof(string));
            dt.Columns.Add("TicketBoxNumber", typeof(string));
            dt.Columns.Add("DispatchWay", typeof(string));


            for (int i = 0; i < list.Count; i++)
            {
                dt.Rows.Add(list[i].TickStoreTypeName, list[i].TickStatus, list[i].TickNum.ToString(), locationStation+"——>"+callOutStation);
            }



            //rptData.ShowRptDialog(new AFC.WS.UI.UIPage.TickStoreManager.CrystalTickInOutReport(),
            //    dict, dt);
        }
    }


    public class TickOperationData:INotifyPropertyChanged
    {

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                PropertyChange("IsChecked");
            }
        }

        private string source;

        public string Source
        {
            set
            {
                this.source = value;
                PropertyChange("Source");
            }
            get { return this.source; }
        }

        private string sourceName;


        public string SourceName
        {
            set
            {
                this.sourceName = value;
                PropertyChange("SourceName");
            }
            get { return this.sourceName; }
        }

        private string tickStatus;

        public string TickStatus
        {
            set
            {
                tickStatus = value;
                PropertyChange("TickStatus");
            }
            get { return this.tickStatus; }
        }

   

        private string tickStoreTypeName;

        public string TickStoreTypeName
        {
            get { return tickStoreTypeName; }
            set
            {
                tickStoreTypeName = value;
                PropertyChange("TickStoreTypeName");
            }
        }

        private string tickStoreType;

        public string TickStoreType
        {
            get { return tickStoreType; }
            set
            {
                tickStoreType = value;
                PropertyChange("TickStoreType");
            }
        }

        private string desStationId;

        public string DesStationId
        {
            get { return desStationId; }
            set
            {
                desStationId = value;
                PropertyChange("DesStationId");
            }
        }

        private string desStationName;


        public string DesStationName
        {
            get { return this.desStationName; }
            set
            {
                this.desStationName = value;
                PropertyChange("DesStationName");
            }
        }

        private string updateDate;

        public string UpdateDate
        {
            get { return updateDate; }
            set 
            { 
                updateDate = value;
                PropertyChange("UpdateDate");
            }
        }

        private string updateTime;

        public string UpdateTime
        {
            get { return updateTime; }
            set
            {
                updateTime = value;
                PropertyChange("UpdateTime");
            }
        }

        private int tickNum;

        public int TickNum
        {
            get { return tickNum; }
            set
            {
                tickNum = value;
                PropertyChange("TickNum");
            }
        }

        private int operation;

        public int Operation
        {
            get { return operation; }
            set
            {
                operation = value;
                PropertyChange("Operation");
            }
        }

        private string operatorId;

        public string OperatorId
        {
            get { return operatorId; }
            set 
            { 
                operatorId = value;
                PropertyChange("OperatorId");
            }
        }

  

        private string tickStatusName;


        public string TickStatusName
        {
            get { return this.tickStatusName; }
            set
            {
                this.tickStatusName = value;
                PropertyChange("TickStatusName");
            }
        }


        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void PropertyChange(string properyTypeName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(properyTypeName));
            }
        }
    }





    //dusj add begin 20121019
 /*   public class TickManaProductData : INotifyPropertyChanged
    {

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                PropertyChange("IsChecked");
            }
        }

        private string tickStoreTypeName;

        public string TickStoreTypeName
        {
            get { return tickStoreTypeName; }
            set
            {
                tickStoreTypeName = value;
                PropertyChange("TickStoreTypeName");
            }
        }

        private string tickStoreType;

        public string TickStoreType
        {
            get { return tickStoreType; }
            set
            {
                tickStoreType = value;
                PropertyChange("TickStoreType");
            }
        }

        private string updateDate;

        public string UpdateDate
        {
            get { return updateDate; }
            set
            {
                updateDate = value;
                PropertyChange("UpdateDate");
            }
        }

        private string updateTime;

        public string UpdateTime
        {
            get { return updateTime; }
            set
            {
                updateTime = value;
                PropertyChange("UpdateTime");
            }
        }

        private int tickNum;

        public int TickNum
        {
            get { return tickNum; }
            set
            {
                tickNum = value;
                PropertyChange("TickNum");
            }
        }

        private int operation;

        public int Operation
        {
            get { return operation; }
            set
            {
                operation = value;
                PropertyChange("Operation");
            }
        }

        private string operatorId;

        public string OperatorId
        {
            get { return operatorId; }
            set
            {
                operatorId = value;
                PropertyChange("OperatorId");
            }
        }
        private decimal  checkInMoney;

        public decimal CheckInMoney
        {
            get { return checkInMoney; }
            set
            {
                checkInMoney = value;
                PropertyChange("CheckInMoney");
            }
        }

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void PropertyChange(string properyTypeName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(properyTypeName));
            }
        }
    }*/
}
