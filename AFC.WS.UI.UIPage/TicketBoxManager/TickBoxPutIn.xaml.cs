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

namespace AFC.WS.UI.UIPage.TicketBoxManager
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.CommonControls;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.Const;
    using AFC.WS.UI.RfidRW;
    using AFC.WS.BR;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.Common;
    using AFC.WS.ModelView.Actions.TicketBoxManager;
    using AFC.WS.Model.DB;

    /// <summary>
    /// TickBoxPutInOut.xaml 的交互逻辑
    /// </summary>
    public partial class TickBoxPutIn : UserControlBase
    {

        private RfidTicketboxInfo info = null;

        private List<QueryCondition> actionParams = new List<QueryCondition>();


        private List<TickBoxOperatorInfo> list = new List<TickBoxOperatorInfo>();

        public TickBoxPutIn()
        {
            InitializeComponent();
            this.btnReset.Click += new RoutedEventHandler(btnReset_Click);
            this.btnReadRFID.Click += new RoutedEventHandler(btnReadRFID_Click);
            this.btnRfidConnect.Click += new RoutedEventHandler(btnRfidConnect_Click);
            this.btnPutIn.Click += new RoutedEventHandler(btnPutIn_Click);
            this.cmbType.SelectionChanged += new SelectionChangedEventHandler(cmbType_SelectionChanged);
         
        }


        

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox)
            {
                ComboBox cb = sender as ComboBox;
                BasiTickManaTypeInfo tickManaType = cb.SelectedItem as BasiTickManaTypeInfo;
                if (tickManaType != null)
                {
                    if (info == null)
                    {
                        return;
                    }
                    this.info.cardIssueId = int.Parse(tickManaType.tick_mana_type,System.Globalization.NumberStyles.HexNumber);
                }
            }
        
        }

        /// <summary>
        /// 增加Action的参数
        /// </summary>
        /// <param name="qc">action的参数</param>
        private void AddQueryConditionData(QueryCondition qc)
        {
            if (qc == null)
            {
                WriteLog.Log_Error("qc is null ");
                return;
            }
            try
            {
                QueryCondition temp = this.actionParams.Single(a => a.bindingData.Equals(qc.bindingData));
                if (temp != null)
                {
                    temp.value = qc.value;
                }
                else
                {
                    this.actionParams.Add(qc);
                }
            }
            catch (Exception ex)
            {
                this.actionParams.Add(qc);
            }
        }

        private void btnPutIn_Click(object sender, RoutedEventArgs e)
        {

            if ((this.cmbType.SelectedItem as BasiTickManaTypeInfo) == null)
            {
                MessageDialog.Show("请选择压入的库存管理类型", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return;
            }
            try
            {
                this.info.cardIssueId = int.Parse((this.cmbType.SelectedItem as BasiTickManaTypeInfo).tick_mana_type);
                info.ticketNumber = Convert.ToUInt16(this.txtTotal.Text);
                info.operatorId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                info.lastOpeatorTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                AddQueryConditionData(new QueryCondition { bindingData = "operatorNo", value = this.txtPutNo.Text });
                AddQueryConditionData(new QueryCondition { bindingData = "lastNo", value = this.txtRFIDNum.Text });
                AddQueryConditionData(new QueryCondition { bindingData = "rfidInfo", value = info });
                IAction action = new TickBoxPutInAction();
                if (action.CheckValid(actionParams))
                {
                    ResultStatus res = action.DoAction(actionParams);
                    if (res == null)
                        return;
                    if (res.resultCode == 0)
                    {
                        this.rfidInfo.ClearRfidInfo();

                        BindingToList();
                        this.txtPutNo.Text = string.Empty;
                        this.txtRFIDNum.Text = string.Empty;
                        this.txtTotal.Text = string.Empty;
                        this.cmbType.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show("存在不合法的输入项", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
        }

        /// <summary>
        /// RFID联机测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRfidConnect_Click(object sender, RoutedEventArgs e)
        {
            ShowWindowAction action = new ShowWindowAction();
            action.Title = "RFID读写器连接测试";
            action.Width = 380;
            action.Height = 250;
            action.IsCheckNULL = false;
            action.ucb = new RFIDConnectTest();
            List<QueryCondition> list = new List<QueryCondition>();
            action.DoAction(list);
        }

        private void btnReadRFID_Click(object sender, RoutedEventArgs e)
        {
            RfidReadAsynHandle.StartAsynReadListen(BuinessRule.GetInstace().rfidRw, typeof(RfidTicketboxInfo));
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.rfidInfo.ClearRfidInfo();
            this.txtPutNo.Text = string.Empty;
            this.txtRFIDNum.Text = string.Empty;
            this.txtTotal.Text = string.Empty;
            this.list.Clear();
        }

        public override void InitControls()
        {
            this.rfidInfo.ClearRfidInfo();
            this.list = new List<TickBoxOperatorInfo>();
         //   this.dgTicketBoxInInfo.ItemsSource = this.list;

            this.txtPutNo.Text = string.Empty;
            this.txtRFIDNum.Text = string.Empty;
            this.txtTotal.Text = string.Empty;
            var list=GetACCTickManaTypeInfos();
            this.cmbType.ItemsSource = list;
            this.cmbType.DisplayMemberPath = "tick_mana_type_name";
            if (list != null &&
                list.Count > 0)
            {
                this.cmbType.SelectedIndex = 0;
            }
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.TickBoxPutIn, RfidReadAsynHandle.Finish_Read_Rfid, HandleMode.Asyn, true);

            tickBoxClear.InitControls();
        }

        public List<BasiTickManaTypeInfo> GetACCTickManaTypeInfos()
        {
            string cmd = string.Format("select t.* from basi_tick_mana_type_info t inner join tick_storage_info tsi on tsi.tick_mana_type=t.tick_mana_type and (t.card_issue_id='1' or t.card_issue_id='01') and tsi.ticket_status='00' and tsi.in_store_num>=0 and tsi.tick_mana_type='00' and tsi.station_id='{0}'",SysConfig.GetSysConfig().LocalParamsConfig.StationCode);

            return DBCommon.Instance.GetTModelValue<BasiTickManaTypeInfo>(cmd);
        }

        public override void HandleAsynMessageForUI(Message msg)
        {
            if (msg.MessageType == RfidReadAsynHandle.Finish_Read_Rfid)
            {
                 info = msg.Content as RfidTicketboxInfo;
                this.rfidInfo.SetTicketBoxRfidInfo(info);
                this.txtRFIDNum.Text = info.ticketNumber.ToString();
                this.txtTotal.Text = info.ticketNumber.ToString();
                this.txtPutNo.Text = string.Empty;
                List<BasiTickManaTypeInfo> list = this.cmbType.ItemsSource as List<BasiTickManaTypeInfo>;
                if (list != null &&
                    list.Count != 0)
                {
                    try
                    {
                        int index = list.IndexOf(list.Single(temp => temp.tick_mana_type == info.cardIssueId.ToString("x2")));
                        this.cmbType.SelectedIndex = index;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                RfidReadAsynHandle.AbortAsynHandle();
            }
        }

        /// <summary>
        /// 输入压票张数文本变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPutNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtPutNo.Text))
                {
                    this.txtTotal.Text = this.txtRFIDNum.Text;
                    return;
                }
                int res = 0;
                bool result = int.TryParse(this.txtPutNo.Text, out res);
                if (!result)
                {
                    MessageDialog.Show("请输入合法的数字", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    this.txtPutNo.Text = string.Empty;
                    return;
                }
                int lastNum = 0;
                result = int.TryParse(this.txtRFIDNum.Text, out lastNum);
                if (!result)
                {
                    MessageDialog.Show("RFID中张数不合法", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                    this.txtPutNo.Text = string.Empty;
                    return;
                }
                this.txtTotal.Text = (res + lastNum).ToString();

            }
            catch (Exception ex)
            {
                MessageDialog.Show("RFID中张数不合法", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                this.txtPutNo.Text = string.Empty;
                return;
            }
        }


        public override void UnLoadControls()
        {
            RfidReadAsynHandle.AbortAsynHandle();
            MessageManager.CancelAllSubscribeMessage(RfidReadAsynHandle.Finish_Read_Rfid);
            info = null;
            actionParams.Clear();
            this.dgTicketBoxInInfo.ItemsSource = null;
            this.list.Clear();
            tickBoxClear.UnLoadControls();
        }

        /// <summary>
        /// 将票箱领用信息绑定到列表中
        /// </summary>
        private void BindingToList()
        {
            // this.dgTicketBoxInInfo.AutoGenerateColumns = false;
            this.list.Insert(0,new TickBoxOperatorInfo
            {
                ticketBoxId = this.info.TicketboxId,
                ticketBoxStaus = "补充",
                currentNumber = this.info.ticketNumber.ToString(),
                operatorId = BuinessRule.GetInstace().brConext.CurrentOperatorId,
                updateDate = DateTime.Now.ToString("yyyy年MM月dd日"),
                updateTime = DateTime.Now.ToString("HH:mm:ss")
            });
            AFC.WS.ModelView.Convertors.TicketOrMoneyBoxIdConvetor convetor = new AFC.WS.ModelView.Convertors.TicketOrMoneyBoxIdConvetor();
            var tempList = from aa in list
                           select new
                           {
                               票箱编码 =convetor.Convert( aa.ticketBoxId,null,null,null).ToString(),

                               操作员编码 = aa.operatorId,

                               票箱操作 = aa.ticketBoxStaus,

                               票箱张数 = aa.currentNumber,

                               更新时间 = aa.updateTime,

                               更新日期 = aa.updateDate
                           };

            this.dgTicketBoxInInfo.ItemsSource = tempList.ToList();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tab = sender as TabControl;
            if (tab != null)
            {
                if((e.AddedItems.Count!=0)&&
                    ((e.AddedItems[0] as TabItem)!=null))
                {
                if (tab.SelectedIndex==0)
                {
                    this.rfidInfo.ClearRfidInfo();
                    //this.dgTicketBoxInInfo.ItemsSource = null;
                    this.txtPutNo.Text = string.Empty;
                    this.txtRFIDNum.Text = string.Empty;
                    this.txtTotal.Text = string.Empty;
                    this.tickBoxClear.UnLoadControls();
                    this.InitControls();
                }
                else
                {
                    this.tickBoxClear.rfidInfo.ClearRfidInfo();
                    this.UnLoadControls();
                    this.tickBoxClear.InitControls();
                    
                    //this.tickBoxClear.dgTicketBoxInInfo.ItemsSource = null;
                }
                }
            }

        }
    }
}
