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
    /// TickCheckOut.xaml 的交互逻辑
    /// 
    /// edited by wangdx 20121107 修改了功能界面的中的删除操作
    /// </summary>
    public partial class TickCheckOut : UserControlBase
    {
        /// <summary>
        /// 票卡操作数据集合
        /// </summary>
        private ObservableCollection<TickOperationData> list = new ObservableCollection<TickOperationData>();

        public TickCheckOut()
        {
            InitializeComponent();
            this.cmbTickStoreType.SelectionChanged += new SelectionChangedEventHandler(cmbTickStoreType_SelectionChanged);
            this.txtOperatorId.KeyUp += new KeyEventHandler(txtOperatorId_KeyUp);
            this.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            this.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            this.btnCallOut.Click += new RoutedEventHandler(btnCallOut_Click);
        }

        void txtOperatorId_KeyUp(object sender, KeyEventArgs e)
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
        }

        private void btnCallOut_Click(object sender, RoutedEventArgs e)
        {
            DoublePrimissionAction dpAction = new DoublePrimissionAction();
            dpAction.CurrentOperationId = BuinessRule.GetInstace().OperatorId;
            IAction action = new AFC.WS.ModelView.Actions.TickStoreActions.TickCheckOutAction();

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
                listQueryCondition.Add(new QueryCondition { bindingData = list[i].TickStoreType, value = list[i].TickNum });
            }
            listQueryCondition.Add(new QueryCondition { bindingData = "operator_id", value = this.txtOperatorId.Text });
            if (dpAction.CheckValid(listQueryCondition))
            {
             
                    //todo:
                    ResultStatus result = dpAction.DoAction(listQueryCondition);
                    if (result!=null&&result.resultCode == 0 && result.resultData.ToString() == "0")
                    {
                        list.Clear();
                        this.txtOperatorId.Text = string.Empty;
                        this.txtRealNo.Text = string.Empty;
                        this.txtTickNo.Text = string.Empty;
                        this.labTotal.Content = string.Empty;
                        this.labTotalTip.Content = string.Empty;
                        this.txtOperatorName.Text = string.Empty;
                        this.cmbTickStoreType.SelectedIndex = -1;
                        UpdateTotalCount();
                    }
                   
                
            }
            //throw new NotImplementedException();
        }

             public List<BasiTickManaTypeInfo> GetACCTickManaTypeInfos()
        {
            //dusj modify begin 增加自定义票种
            //string cmd = string.Format("select t.* from basi_tick_mana_type_info t inner join tick_storage_info tsi on tsi.tick_mana_type=t.tick_mana_type and tsi.ticket_status='00' and tsi.in_store_num>0 and tsi.station_id='{0}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
            string cmd = string.Format("select t.* from basi_tick_mana_type_info t inner join tick_storage_info tsi on tsi.tick_mana_type = t.tick_mana_type and tsi.ticket_status = '00' and tsi.in_store_num > 0 and tsi.station_id='{0}' union all select p.tick_mana_type,p.tick_mana_type_name,p.ticket_phy_type,''ticket_phy_type_name,''ticket_family_type,''ticket_family_type_name,p.card_issue_id from tick_valued_product_info  p inner join tick_storage_info ts on ts.tick_mana_type = p.tick_mana_type and ts.in_store_num > 0  and ts.station_id='{0}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);
           //dusj modify end
            return DBCommon.Instance.GetTModelValue<BasiTickManaTypeInfo>(cmd);
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

            bool res = int.TryParse(this.txtTickNo.Text.Trim(), out inStoreNum);
            if (!res)
            {
                MessageDialog.Show("票务室库存张数不合法", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            res = int.TryParse(this.txtRealNo.Text.Trim(), out checkOutNum);
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
                    TickStorageInfo tickStoreInfo = BuinessRule.GetInstace().GetTickStorageInfoByTickManaType(type);
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
            List<BasiTickManaTypeInfo> list = GetACCTickManaTypeInfos();
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
            if (instoreNum < checkNum)
            {
                MessageDialog.Show("领用张数必须小于当前库存张数", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return false;
            }
            return true;
        }

        public override void UnLoadControls()
        {
            this.list.Clear();
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
                int res = list.Count(temp => temp.TickStoreType.Equals(data.TickStoreType));
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
    }
}
