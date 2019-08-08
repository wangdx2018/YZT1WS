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
using AFC.BOM2.MessageDispacher;
using AFC.WS.BR;
using AFC.WS.UI.Config;
using AFC.WS.UI.Common;

namespace AFC.WS.UI.UIPage.Maintenance
{
    /// <summary>
    /// PartsInventoryReg.xaml 的交互逻辑
    /// </summary>
    public partial class PartsInventoryReg : UserControlBase
    {

        private List<QueryCondition> list = new List<QueryCondition>();

        public PartsInventoryReg()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 重写初始化控件
        /// </summary>
        public override void InitControls()
        {
            InitLoad();
            MessageManager.SubscribeMessage(this, "ReadMoneyRfidInfo", RfidRW.RfidReadAsynHandle.Finish_Read_Rfid, HandleMode.Syn, true);
        }
        /// <summary>
        /// 初始化加载方法
        /// </summary>
        private void InitLoad()
        {
            try
            {
                this.txtRfidLabel.Text = string.Empty;
                this.txtOperator.Text = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                //供应商
                Wrapper.FullComboBox(this.cbbSuppliers, BuinessRule.GetInstace().GetBasiProviderInfo(), "mc_dep_name", "provider_id", false);
                //部件ID
                Wrapper.FullComboBox(this.cbbParts, BuinessRule.GetInstace().GetBasiDevTypeInfo(), "dev_part_cn_name", "dev_part_id", false);
               
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }
        public override void HandleAsynMessageForUI(Message msg)
        {
            if (msg.MessageType == RfidRW.RfidReadAsynHandle.Finish_Read_Rfid)
            {
                RfidRW.RfidReadAsynHandle.AbortAsynHandle();
                this.txtRfidLabel.Text = msg.Content as string;
            }
        }

        /// <summary>
        /// 取消注册
        /// </summary>
        public override void CancleSubscribeSynMessage()
        {
            MessageManager.CancelAllSubscribeMessage(RfidRW.RfidReadAsynHandle.Finish_Read_Rfid);
        }

        // <summary>
        /// 获取钱箱的RFID信息。
        /// </summary>
        /// <param name="sender">类</param>
        /// <param name="e">事件类</param>
        private void btnReadRFIDInfo_Click(object sender, RoutedEventArgs e)
        {
            RfidRW.RfidReadAsynHandle.StartAsynReadListen(BuinessRule.GetInstace().rfidRw,null);
        }

        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Wrapper.Instance.AddQueryConditionToList(list, "provider", Wrapper.GetComboBoxUid(this.cbbSuppliers));
                Wrapper.Instance.AddQueryConditionToList(list, "partType", Wrapper.GetComboBoxUid(this.cbbParts));
                Wrapper.Instance.AddQueryConditionToList(list, "partID",this.txtRfidLabel.Text.Trim());
               
                Wrapper.Instance.AddQueryConditionToList(list, "operatorID", this.txtOperator.Text.Trim());


                IAction action = new AFC.WS.ModelView.Actions.Maintenance.PartsInventoryRegAction();
                if (action.CheckValid(list))
                {
                    action.DoAction(list);
                }

                btnCancel_Click(sender, e);
      
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
           
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.txtRfidLabel.Text = string.Empty;
            this.txtOperator.Text = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            this.cbbSuppliers.SelectedIndex = -1;
            this.cbbParts.SelectedIndex = -1;
        }

        public override void UnLoadControls()
        {
            RfidRW.RfidReadAsynHandle.AbortAsynHandle();
        }


    }
}
