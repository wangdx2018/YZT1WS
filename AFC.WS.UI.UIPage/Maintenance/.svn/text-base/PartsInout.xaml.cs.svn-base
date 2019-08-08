using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using AFC.BOM2.UIController;
using AFC.WS.BR.Maintenance;
using AFC.BOM2.MessageDispacher;
using Microsoft.Windows.Controls;
using AFC.WS.UI.Common;
using AFC.WS.BR;
using AFC.WS.UI.Config;
using AFC.WS.ModelView.Convetors;
using AFC.WS.ModelView.Convertors;
using AFC.WS.Model.DB;

namespace AFC.WS.UI.UIPage.Maintenance
{
    /// <summary>
    /// PartsInout.xaml 的交互逻辑
    /// </summary>
    public partial class PartsInout : UserControlBase
    {
        //部件状态信息
        public List<DisplayParts> PartsList = new List<DisplayParts>();

        string partsStatus = string.Empty;

        public PartsInout()
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
            this.txtRfidLabel.Text = string.Empty;
            this.PartsList.Clear();
            SetDataGridPartsList(this.dgPartsOutInfo, null);
        }

        public override void HandleAsynMessageForUI(Message msg)
        {
            if (msg.MessageType == RfidRW.RfidReadAsynHandle.Finish_Read_Rfid)
            {
                RfidRW.RfidReadAsynHandle.AbortAsynHandle();
                this.txtRfidLabel.Text = msg.Content as string;
                PartStatusConvert convert = new PartStatusConvert();
                DateTimeConvert  dateConvert = new DateTimeConvert();
                ConvertToTime    timeConvert = new ConvertToTime();
                MatainLablePartStore store = MaintenanceManager.Instance.GetPartStoreById(this.txtRfidLabel.Text);
                if (store != null && !string.IsNullOrEmpty(store.part_id))
                {
                    this.txtLastOperator.Text = store.check_out_operator;
                    this.txtParts.Text = store.part_type_id;
                    this.txtProvider.Text = store.provider_id;
                    this.txtState.Text = convert.Convert(store.status,null,null,null).ToString();
                    this.txtLastOperatorDate.Text = dateConvert.Convert(store.update_date,null,null,null).ToString();
                    this.txtLastOperatorTime.Text = timeConvert.Convert(store.update_time, null, null, null).ToString();
                }
            }
        }

        /// <summary>
        /// 取消注册
        /// </summary>
        public override void CancleSubscribeSynMessage()
        {
            MessageManager.CancelAllSubscribeMessage(RfidRW.RfidReadAsynHandle.Finish_Read_Rfid);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.txtRfidLabel.Text = string.Empty;
            this.txtState.Text = string.Empty;
            this.txtProvider.Text = string.Empty;
            this.txtLastOperatorTime.Text = string.Empty;
            this.txtLastOperatorDate.Text = string.Empty;
            this.txtLastOperator.Text = string.Empty;
            this.txtParts.Text = string.Empty;
            this.txtOperatorID.Text = string.Empty;

        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            //领用，在人
            partsStatus = "01";
            partsInOutPro();
        }

        private void btnIn_Click(object sender, RoutedEventArgs e)
        {
            //归还，在库
            partsStatus = "00";
            partsInOutPro();
        }

        private void btnTuneOut_Click(object sender, RoutedEventArgs e)
        {
            //调出
            partsStatus = "02";
            partsInOutPro();
        }

        private void btnInvalid_Click(object sender, RoutedEventArgs e)
        {
            //做废
            partsStatus = "03";
            partsInOutPro();
        }
        /// <summary>
        /// 填充钱箱明细列表。
        /// </summary>
        /// <param name="dg">DataGrid控件</param>
        /// <param name="mb">钱箱信息类</param>
        public void SetDataGridPartsList(DataGrid dg, DisplayParts mb)
        {

            if (dg == null || mb == null)
            {
                return;
            }
            PartsList.Add(mb);
            List<DisplayParts> temp = new List<DisplayParts>();
            foreach (var v in PartsList)
            {
                DisplayParts part = new DisplayParts();
                part.check_out_operator = v.check_out_operator;
                part.part_id = v.part_id;
                part.status = v.status;
                part.dev_part_cn_name = v.dev_part_cn_name;
                part.mc_dep_name = v.mc_dep_name;


                temp.Add(part);
            }
            dg.ItemsSource = temp.ToArray();
        }


        private void partsInOutPro()
        {
            try
            {
                ResultStatus status = null;
                PartStatusConvert convert = new PartStatusConvert();
                List<QueryCondition> list = new List<QueryCondition>();
                list.Add(new QueryCondition { bindingData = "partsID", value = this.txtRfidLabel.Text.Trim() });
                list.Add(new QueryCondition { bindingData = "operatorID", value = this.txtOperatorID.Text });
                list.Add(new QueryCondition { bindingData = "partsStatus", value = partsStatus });
                IAction action = new AFC.WS.ModelView.Actions.Maintenance.PartsInoutAction();
                if (action.CheckValid(list))
                {

                    status = action.DoAction(list);
                }

                if (status != null && status.resultCode == 0)
                {

                    DisplayParts mb = new DisplayParts();

                    mb.mc_dep_name = this.txtProvider.Text.Trim();
                    mb.dev_part_cn_name = this.txtParts.Text.Trim();
                    mb.status = convert.Convert(partsStatus, null, null, null).ToString();
                    mb.part_id = this.txtRfidLabel.Text.Trim();
                    mb.check_out_operator = this.txtOperatorID.Text.Trim();
                    SetDataGridPartsList(this.dgPartsOutInfo, mb);
                    btnCancel_Click(null, null);
                }
                
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        private void btnReadRFIDInfo_Click(object sender, RoutedEventArgs e)
        {
            RfidRW.RfidReadAsynHandle.StartAsynReadListen(BuinessRule.GetInstace().rfidRw, null);
        }

    }
}
