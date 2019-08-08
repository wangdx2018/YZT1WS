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

namespace AFC.WS.UI.UIPage.TickStoreManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.Model.DB;
    using AFC.WS.BR;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Common;
    using AFC.WS.ModelView.Actions.TickStoreActions;
    using System.Collections.ObjectModel;
    using AFC.WS.ModelView.Actions.CommonActions;
    /// <summary>
    /// TickCheckIn.xaml 的交互逻辑
    /// 
    /// edited by wangdx 20121009 在操作员归还时，增加了各个票种的废票处理
    /// edited by wangdx 20121107 在操作员废票归还时，增加了废票来源。废票盒，BOM操作员
    /// edited by wangdx 20121214 将废票盒名称修改为回收盒，将票卡记录放置到正常票卡库存中。
    /// </summary>
    public partial class TickCheckIn : UserControlBase
    {
        /// <summary>
        /// 票卡操作数据集合
        /// </summary>
        private ObservableCollection<TickOperationData> list = new ObservableCollection<TickOperationData>();

        /// <summary>
        /// 票卡状态 00：正常  01:废票
        /// </summary>
        private string tickStatus;

        /// <summary>
        /// 废票来源 00：BOM，01：废票盒
        /// </summary>
        private string wasteSource="00";

        public TickCheckIn()
        {
            tickStatus = "00";//正常票卡
            InitializeComponent();
            this.cmbTickStoreType.SelectionChanged += new SelectionChangedEventHandler(cmbTickStoreType_SelectionChanged);
            this.cmbTickStatus.SelectionChanged += new SelectionChangedEventHandler(cmbTickStatus_SelectionChanged);
            this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.btnCallOut.Click += new RoutedEventHandler(btnCallOut_Click);
            this.txtOperatorId.KeyUp += new KeyEventHandler(txtOperatorId_KeyUp);
            this.rdBom.Checked += new RoutedEventHandler(rdBom_Checked);
            this.rdWasteBox.Checked += new RoutedEventHandler(rdBom_Checked);
            this.cmbTickStatus.SelectedIndex =0 ;
            this.wasteTicketSource.Visibility = Visibility.Visible;
            this.rdBom.IsChecked = true;
        }

        void rdBom_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rd = sender as RadioButton;
            if (rd.Name == "rdBom")
            {
                this.wasteSource = "00";
            }
            else
            {
                this.wasteSource = "01";
            }
        }

        private void cmbTickStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.tickStatus = (e.AddedItems[0] as ComboBoxItem).Tag.ToString();
            if (this.tickStatus == "00")
            {
                this.wasteTicketSource.Visibility = Visibility.Visible;
              
                
            }
            else
            {
                this.wasteTicketSource.Visibility = Visibility.Collapsed;
            }
            this.rdBom.IsChecked = true;
            this.cmbTickStoreType_SelectionChanged(this.cmbTickStoreType, e);
            //throw new NotImplementedException();
        }

        private void txtOperatorId_KeyUp(object sender, KeyEventArgs e)
        {
            string operatorCode = this.txtOperatorId.Text.Trim();
            if (operatorCode.Length == 8)
            {
                PrivOperatorInfo operatorInfo = BuinessRule.GetInstace().operationManager.GetOperatorInfoByOperatorId(operatorCode);
                if (operatorInfo == null || string.IsNullOrEmpty(operatorInfo.operator_id))
                {
                    MessageDialog.Show("请输入正确的操作员", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
                else
                {
                    this.txtOperatorName.Text = operatorInfo.operator_name;
                }
            }
            //throw new NotImplementedException();
        }

        private void btnCallOut_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpAction = new DoublePrimissionAction();
            dpAction.CurrentOperationId = BuinessRule.GetInstace().OperatorId;
            IAction action = new AFC.WS.ModelView.Actions.TickStoreActions.TickCheckInAction();
            dpAction.subAction = action;
            List<QueryCondition> listQueryCondition = new List<QueryCondition>();
            if (string.IsNullOrEmpty(this.txtOperatorId.Text))
            {
                MessageDialog.Show("请输入操作员编码", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            if (list.Count == 0)
            {
                MessageDialog.Show("请添加票卡信息！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            for (int i = 0; i < list.Count; i++)
            {
                ResultStatus res = new ResultStatus();
                res.resultCode=list[i].TickNum;
                res.resultData=list[i].TickStatus;
                listQueryCondition.Add(new QueryCondition { bindingData = list[i].TickStoreType,value =res, controlName=list[i].Source});
              //  listQueryCondition.Add(new QueryCondition { bindingData = list[i].TickStatus,value = list[i].TickStatus});
            }
            listQueryCondition.Add(new QueryCondition { bindingData = "operator_id", value = this.txtOperatorId.Text });
            
            if (dpAction.CheckValid(listQueryCondition))
            {
               ResultStatus result= dpAction.DoAction(listQueryCondition);
               if (result!=null&&result.resultCode == 0 && result.resultData.ToString() == "0")
               {
                   list.Clear();
                   this.txtOperatorId.Text = string.Empty;
                   this.txtOperatorName.Text = string.Empty;
                   this.cmbTickStoreType.SelectedIndex = -1;
                   UpdateTotalCount();
                   this.txtTickNo.Text = string.Empty;
               }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var temp = new List<TickOperationData>();
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
            res = int.TryParse(this.txtRealNo.Text, out checkOutNum);
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
                        TickStatus=this.tickStatus,
                        TickStatusName=this.tickStatus.Equals("00")?"正常票":"废票",
                        Source=this.wasteSource,
                        SourceName=this.wasteSource.Equals("00")?"BOM操作员":"回收盒"
                    };
                    this.AddTickManaTypeInfoToList(tod);
                    UpdateTotalCount();
                    this.txtRealNo.Text = string.Empty;
                }
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

        private void cmbTickStoreType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.txtTickNo.Text = string.Empty;
            this.txtRealNo.Text = string.Empty;
            this.txtRealNo.IsEnabled = true;
            this.btnAdd.IsEnabled = true;

            ComboBox cb = sender as ComboBox;
            BasiTickManaTypeInfo info = cb.SelectedItem as BasiTickManaTypeInfo;
            if (info != null)
            {
                string type = info.tick_mana_type;
                if (!string.IsNullOrEmpty(type))
                {
                    TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(type,this.tickStatus);
                    this.txtTickNo.Text = tickStoreInfo.in_store_num.ToString();
                }
                else
                {
                    this.txtTickNo.Text = "没有该库存类型";
                    this.txtRealNo.IsEnabled = false;
                    this.btnAdd.IsEnabled = false;
                }

            }
            //throw new NotImplementedException();
        }

        public override void InitControls()
        {
            List<BasiTickManaTypeInfo> list =BuinessRule.GetInstace().GetBasiTickManaTypeInfo(false);
            this.cmbTickStoreType.ItemsSource = list;
            this.cmbTickStoreType.DisplayMemberPath = "tick_mana_type_name";
            this.listView1.ItemsSource = this.list;
            UpdateTotalCount();
            this.txtOperatorId.Text = string.Empty;
            this.txtOperatorName.Text = string.Empty;
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
            //base.UnLoadControls();
        }

        public List<BasiTickManaTypeInfo> GetACCTickManaTypeInfos()
        {
            string cmd = "select t.* from basi_tick_mana_type_info t inner join tick_storage_info tsi on tsi.tick_mana_type=t.tick_mana_type and (t.card_issue_id='1' or t.card_issue_id='01') and tsi.ticket_status='00' and tsi.total_num>=0";

            return DBCommon.Instance.GetTModelValue<BasiTickManaTypeInfo>(cmd);
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
                    temp.TickStatus.Equals(data.TickStatus)&&temp.Source.Equals(data.Source));
                if (res == 1)
                {
                    TickOperationData last = list.Single(temp => temp.TickStoreType.Equals(data.TickStoreType)&&temp.Source.Equals(data.Source));

                    last.TickNum = data.TickNum + last.TickNum;

                }
                else
                    this.list.Add(data);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
