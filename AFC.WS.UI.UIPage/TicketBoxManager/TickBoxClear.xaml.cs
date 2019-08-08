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
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.ModelView.Actions.CommonActions;
    using AFC.WS.UI.RfidRW;
    using AFC.WS.UI.Common;
    using AFC.WS.ModelView.Actions.TicketBoxManager;
    using AFC.WS.Model.Const;
    using AFC.WS.BR;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.CommonControls;
    using System.Data;

    /// <summary>
    /// TickBoxClear.xaml 的交互逻辑
    /// editor by wangdx 20120717
    /// 修改了lastNum取值，应该从RealNum中获取。
    /// </summary>
    public partial class TickBoxClear : UserControlBase
    {
        private List<QueryCondition> actionParams = new List<QueryCondition>();

        private RfidTicketboxInfo info = null;

        private List<TickBoxOperatorInfo> list = new List<TickBoxOperatorInfo>();

        public TickBoxClear()
        {
            InitializeComponent();
            this.btnClear.Click += new RoutedEventHandler(btnClear_Click);
            this.btnReadRFID.Click += new RoutedEventHandler(btnReadRFID_Click);
            this.btnReset.Click += new RoutedEventHandler(btnReset_Click);
            this.btnRfidConnect.Click += new RoutedEventHandler(btnRfidConnect_Click);
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

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.rfidInfo.ClearRfidInfo();

            this.txtRealNum.Text = string.Empty;
            this.txtRFIDNum.Text = string.Empty;
        }

        private void btnReadRFID_Click(object sender, RoutedEventArgs e)
        {
            RfidReadAsynHandle.StartAsynReadListen(BuinessRule.GetInstace().rfidRw, typeof(RfidTicketboxInfo));
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            if ((this.cmbType.SelectedItem as BasiTickManaTypeInfo) == null)
            {
                MessageDialog.Show("请选择清点库存管理类型", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return;
            }
            if (info == null)
            {
                MessageDialog.Show("请先读取RFID信息", "提示", MessageBoxIcon.Error, MessageBoxButtons.Ok);
                return;
            }
            info.cardIssueId = int.Parse((this.cmbType.SelectedItem as BasiTickManaTypeInfo).tick_mana_type);
            info.operatorId = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            info.lastOpeatorTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            //info.ticketNumber = 0;

            AddQueryConditionData(new QueryCondition { bindingData = "lastNo", value = this.txtRealNum.Text });
            AddQueryConditionData(new QueryCondition { bindingData = "rfidInfo", value = info });
            IAction action = new TickBoxClearAction();
            if (action.CheckValid(actionParams))
            {
               ResultStatus rs= action.DoAction(actionParams);
                if(rs!=null)
                {
                    this.rfidInfo.ClearRfidInfo();
                   
                    BindingToList();
                    this.txtRealNum.Text = string.Empty;
                    this.txtRFIDNum.Text = string.Empty;
                    this.cmbType.SelectedIndex = 0;
                }
            }
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
                ticketBoxStaus = "清点",
                currentNumber = this.txtRealNum.Text,
                operatorId = BuinessRule.GetInstace().brConext.CurrentOperatorId,
                updateDate = DateTime.Now.ToString("yyyy年MM月dd日"),
                updateTime = DateTime.Now.ToString("HH:mm:ss")
            });
            AFC.WS.ModelView.Convertors.TicketOrMoneyBoxIdConvetor convetor = new AFC.WS.ModelView.Convertors.TicketOrMoneyBoxIdConvetor();
            var tempList = from aa in list
                           select new
                           {
                               票箱编码 = convetor.Convert(aa.ticketBoxId, null, null, null).ToString(),

                               操作员编码 = aa.operatorId,

                               票箱操作= aa.ticketBoxStaus,

                               票箱张数 = aa.currentNumber,

                               更新时间 = aa.updateTime,

                               更新日期 = aa.updateDate
                           };

            this.dgTicketBoxInInfo.ItemsSource = tempList.ToList();
        }
        //zg
        public override void UnLoadControls()
        {
            RfidReadAsynHandle.AbortAsynHandle();
            MessageManager.CancelAllSubscribeMessage(RfidReadAsynHandle.Finish_Read_Rfid);
            info = null;
            this.actionParams.Clear();
            this.dgTicketBoxInInfo.ItemsSource = null;
            this.list.Clear();
        }

        public override void InitControls()
        {
            this.rfidInfo.ClearRfidInfo();
            this.txtRealNum.Text = string.Empty;
            this.txtRFIDNum.Text = string.Empty;
            //this.cmbType.ItemsSource = GetACCTickManaTypeInfos();
            //this.cmbType.DisplayMemberPath = "tick_mana_type_name";
            var list = GetACCTickManaTypeInfos();
            this.cmbType.ItemsSource = list;
            this.cmbType.DisplayMemberPath = "tick_mana_type_name";
            if (list != null &&
                list.Count > 0)
            {
                this.cmbType.SelectedIndex = 0;
            }

            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.TickBoxClear, RfidReadAsynHandle.Finish_Read_Rfid, HandleMode.Asyn, true);
        }

        public List<BasiTickManaTypeInfo> GetACCTickManaTypeInfos()
        {
            string cmd = string.Format("select t.* from basi_tick_mana_type_info t inner join tick_storage_info tsi on tsi.tick_mana_type=t.tick_mana_type and (t.card_issue_id='1' or t.card_issue_id='01') and tsi.ticket_status='00' and tsi.in_store_num>=0 and tsi.tick_mana_type='00' and tsi.station_id='{0}'", SysConfig.GetSysConfig().LocalParamsConfig.StationCode);


            return DBCommon.Instance.GetTModelValue<BasiTickManaTypeInfo>(cmd);
        }

        public override void HandleAsynMessageForUI(Message msg)
        {
            if (msg.MessageType == RfidReadAsynHandle.Finish_Read_Rfid)
            {
                info = msg.Content as RfidTicketboxInfo;
                this.rfidInfo.SetTicketBoxRfidInfo(info);
                this.txtRFIDNum.Text = info.ticketNumber.ToString();
                this.txtRealNum.Text = string.Empty;
                RfidReadAsynHandle.AbortAsynHandle();

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
            }
        }

        
    }
}
