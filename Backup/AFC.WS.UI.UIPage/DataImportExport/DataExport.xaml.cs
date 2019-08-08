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
using AFC.WS.ModelView.Actions.DataImportExport;
using AFC.WS.UI.Common;
using AFC.BOM2.MessageDispacher;
using AFC.WS.BR.DataImportExport;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.UI.UIPage.DataImportExport
{
    /// <summary>
    /// DataExport.xaml 的交互逻辑
    /// </summary>
    public partial class DataExport : UserControlBase,IMessageHandler
    {
        public DataExport()
        {
            InitializeComponent();
        }

        private void cmbExportDataType_DropDownClosed(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.cmbExportDataType.Text))
            {
                MessageDialog.Show("请选择导出数据类型", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            ComboBoxItem cmbItem = this.cmbExportDataType.SelectedItem as ComboBoxItem;
            switch (cmbItem.Tag.ToString())
            {
                case "00":
                case "01":
                    this.gpbData.IsEnabled = true;
                    this.gpbParaSoft.IsEnabled = false;
                    this.InitCheckBox();
                    break;
                case "03":
                    this.gpbParaSoft.IsEnabled = true;
                    this.chbACC.IsEnabled = true;
                    this.chbLCC.IsEnabled = true;
                    this.gpbData.IsEnabled = false;
                    this.InitCheckBox();
                    break;
                case "04":
                    this.gpbParaSoft.IsEnabled = false;
                    this.gpbData.IsEnabled = false;
                    this.InitCheckBox();
                    this.chbCurVer.IsChecked = true;
                    this.chbLCC.IsChecked = true;
                    break;
                default:
                    break;
            }
            this.labExportInfo.Content = "";
            this.labFileName.Content = "";
            this.prcExportBar.Value = 0;
        }

        private void InitGroupBox()
        {
            this.gpbParaSoft.IsEnabled = false;
            this.gpbData.IsEnabled = false;
        }

        public override void InitControls()
        {
            this.InitGroupBox();
            this.InitCheckBox();

            MessageManager.SubscribeMessage(this, MessageSubscribeID.DataExport, MessageType.ExportMessage,
            HandleMode.Syn, true);
            this.prcExportBar.Value = 0;
        }

        public override void UnLoadControls()
        {
            this.InitCheckBox();
            //base.UnLoadControls();
        }

        private void InitCheckBox()
        {
            this.chbACC.IsChecked = false;
            this.chbCurVer.IsChecked = false;
            this.dpStartDate.SetControlValue(null);
            this.chbFail.IsChecked = false;
            this.chbFutVer.IsChecked = false;
            this.chbLCC.IsChecked = false;
            this.dpEndDate.SetControlValue(null);
            this.chbSucc.IsChecked = false;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.cmbExportDataType.Text))
            {
                MessageDialog.Show("请选择导出数据类型", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return;
            }
            string strTag = (this.cmbExportDataType.SelectedItem as ComboBoxItem).Tag.ToString();
            if (string.IsNullOrEmpty(strTag))
                return;
            IAction action = null;
            switch (strTag)
            {
                case "00":
                    action = new TradeDataExportAction();
                    break;
                case "01":
                    action = new BusiDataExportAction();
                    break;
                case "03":
                    action = new ParamExportAction();
                    break;
                case "04":
                    action = new SoftwareExportAction();
                    break;
                default:
                    break;
            }
            List<QueryCondition> list = this.CreateCondition(strTag);
            //list.Add(new QueryCondition { bindingData = "cmbText", value = this.cmbExportDataType.Text });
            if (action.CheckValid(list))
            {
                action.DoAction(list);
            }

        }

        private List<QueryCondition> CreateCondition(string strTag)
        {
            List<QueryCondition> list = new List<QueryCondition>();
            ConditionClass cc = SetCondition(strTag);
            list.Add(new QueryCondition { bindingData = "Condition", value = cc });
            return list;
        }

        private ConditionClass SetCondition(string strTag)
        {
            ConditionClass condition = new ConditionClass();
            ///导出交易数据和业务数据
            if (strTag.Equals("00") || strTag.Equals("01"))
            {
                if ((this.chbSucc.IsChecked == false) && (this.chbFail.IsChecked == false))
                {
                    condition.upResult = "00";
                }
                if ((this.chbSucc.IsChecked == true) && (this.chbFail.IsChecked == true))
                {
                    condition.upResult = "03";
                }
                if ((this.chbSucc.IsChecked == true) && (this.chbFail.IsChecked == false))
                {
                    condition.upResult = "01";
                }
                if ((this.chbSucc.IsChecked == false) && (this.chbFail.IsChecked == true))
                {
                    condition.upResult = "02";
                }

                if (condition.upResult == "00" || this.dpStartDate.GetControlValue() == null || this.dpEndDate.GetControlValue() == null)
                    return null;

                condition.startDate = this.dpStartDate.GetControlValue().ToString().Replace("-", "");
                condition.endDate = this.dpEndDate.GetControlValue().ToString().Replace("-", "");
            }
            else
            {    
                ///参数类型
                if ((this.chbACC.IsChecked == false) && (this.chbLCC.IsChecked == false))
                {
                    condition.paraType = "00";
                }
                if ((this.chbACC.IsChecked == true) && (this.chbLCC.IsChecked == true))
                {
                    condition.paraType = "03";
                }
                if ((this.chbACC.IsChecked == false) && (this.chbLCC.IsChecked == true))
                {
                    condition.paraType = "01";
                }
                if ((this.chbACC.IsChecked == true) && (this.chbLCC.IsChecked == false))
                {
                    condition.paraType = "02";
                }

                ///参数版本
                if ((this.chbCurVer.IsChecked == false) && (this.chbFutVer.IsChecked == false))
                {
                    condition.verType = "00";
                }
                if ((this.chbCurVer.IsChecked == true) && (this.chbFutVer.IsChecked == true))
                {
                    condition.verType = "03";
                }
                if ((this.chbCurVer.IsChecked == true) && (this.chbFutVer.IsChecked == false))
                {
                    condition.verType = "01";
                }
                if ((this.chbCurVer.IsChecked == false) && (this.chbFutVer.IsChecked == true))
                {
                    condition.verType = "02";
                }
                if (condition.paraType == "00" || condition.verType == "00")
                    return null;
            }
            return condition;
        }

        #region IMessageHandler 成员

        void IMessageHandler.HandleAsynMessage(Message msg)
        {
            throw new NotImplementedException();
        }

        void IMessageHandler.HandleSynMessage(Message msg)
        {
            if (msg == null)
                return;
            if (msg.MessageType == MessageType.ExportMessage)
            {
                string percent = msg.Content.ToString();
                string filrName = msg.MessageSource.ToString();
                this.labFileName.Content = "文件名：" + filrName;
                SetTextMessage(Int32.Parse(percent));
                if (percent.Equals("100"))
                {
                    this.labFileName.Content = "数据导出操作完成";
                }
                this.labExportInfo.Content = "百分比：" + percent + "%";
            }
        }

        #endregion

        /// <summary>
        /// 设置进度条
        /// </summary>
        /// <param name="ipos"></param>
        private void SetTextMessage(int ipos)
        {
            //处理消息队列中的所有消息，让进度条走动
            System.Windows.Forms.Application.DoEvents();
            this.prcExportBar.Value = ipos;
        }
    }
}
