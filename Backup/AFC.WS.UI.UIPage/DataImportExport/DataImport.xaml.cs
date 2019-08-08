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
using AFC.WS.ModelView.Actions.DataImportExport;
using AFC.BOM2.MessageDispacher;
using AFC.WS.BR.DataImportExport;
using AFC.WS.UI.CommonControls;

namespace AFC.WS.UI.UIPage.DataImportExport
{
    /// <summary>
    /// DataImport.xaml 的交互逻辑
    /// </summary>
    public partial class DataImport : UserControlBase,IMessageHandler
    {
        public DataImport()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            MessageManager.SubscribeMessage(this, MessageSubscribeID.DataImport, MessageType.ImportMessage, 
                HandleMode.Syn, true);
            this.prcImportBar.Value = 0;
            //base.InitControls();
        }

        public override void UnLoadControls()
        {
            //base.UnLoadControls();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.cmbImportDataType.Text))
            {
                MessageDialog.Show("请选择导入数据类型", "提示", MessageBoxIcon.Information,MessageBoxButtons.Ok);
                return;
            }
            string strTag = (this.cmbImportDataType.SelectedItem as ComboBoxItem).Tag.ToString();
            if (string.IsNullOrEmpty(strTag))
                return;
            IAction action = null;
            switch (strTag)
            {
                case "00":
                    action = new TradeDataImportAction();
                    break;
                case "01":
                    action = new BusiDataImportAction();
                    break;
                case "03":
                    action = new ParamImportAction();
                    break;
                case "04":
                    action = new SoftwareImportAction();
                    break;
                default:
                    break;
            }
            List<QueryCondition> list = new List<QueryCondition>();
            list.Add(new QueryCondition { bindingData = "cmbText", value = this.cmbImportDataType.Text });
            if (action.CheckValid(list))
            {
                action.DoAction(list);
            }
            
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
            if (msg.MessageType == MessageType.ImportMessage)
            {
                string percent=msg.Content.ToString();
                string filrName=msg.MessageSource.ToString();
                this.labFileName.Content = "文件名：" + filrName;
                SetTextMessage(Int32.Parse(percent));
                if (percent.Equals("100"))
                {
                    this.labFileName.Content = "数据导入操作完成";
                }
                this.labImportInfo.Content = "百分比：" + percent + "%";
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
            this.prcImportBar.Value = ipos;
        }

        private void cmbImportDataType_DropDownClosed(object sender, EventArgs e)
        {
            this.labFileName.Content = "";
            this.labImportInfo.Content = "";
            this.prcImportBar.Value = 0;
        }
    }
}
