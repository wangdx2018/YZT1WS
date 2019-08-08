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
    using AFC.WS.UI.Common;
    using AFC.WS.BR;
    using AFC.BOM2.UIController;
    /// <summary>
    /// RFIDConnectTest.xaml 的交互逻辑
    /// </summary>
    public partial class RFIDConnectTest : UserControlBase
    {
        public RFIDConnectTest()
        {
            InitializeComponent();
            this.btnConnect.Click += new RoutedEventHandler(btnConnect_Click);
            this.btnSave.Click += new RoutedEventHandler(btnSave_Click);
            BindingData();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SysConfig.GetSysConfig().RFIDRWConfig.PortName = this.cmbComName.Text;
                SysConfig.GetSysConfig().RFIDRWConfig.Parity = (System.IO.Ports.Parity)(Enum.Parse(typeof(System.IO.Ports.Parity), this.cmbPartity.Text));
                SysConfig.GetSysConfig().RFIDRWConfig.DataBit = int.Parse(this.cmbDataBit.Text);
                SysConfig.GetSysConfig().RFIDRWConfig.BoundRate = uint.Parse(this.cmbBoundRate.Text);
                SysConfig.GetSysConfig().RFIDRWConfig.PstopBit = (System.IO.Ports.StopBits)(Enum.Parse(typeof(System.IO.Ports.StopBits), this.cmbStopBit.Text));
                int res = SysConfig.GetSysConfig().WrtieSysConfigFile();
                if (res != 0)
                    this.labTip.Content = "保存RFID读写器配置信息失败,请重试!";
                else
                    this.labTip.Content = "保存RFID读写器配置信息成功";
            }

            catch (Exception ex)
            {
             this.labTip.Content="存在不合法的输入项!";//, "提示", AFC.WS.UI.CommonControls.MessageBoxIcon.Error, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                return;
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
           BuinessRule.GetInstace().rfidRw.SetSerialPort((int)SysConfig.GetSysConfig().RFIDRWConfig.BoundRate,
                SysConfig.GetSysConfig().RFIDRWConfig.Parity, SysConfig.GetSysConfig().RFIDRWConfig.PstopBit, SysConfig.GetSysConfig().RFIDRWConfig.DataBit,
              this.cmbComName.Text);
            bool res = BuinessRule.GetInstace().rfidRw.Connect(1);
            if (res)
                labTip.Content = "RFID读写器连接成功!";
            else
                labTip.Content = "RFID读写器连接失败!";
        }

        

        public void BindingData()
        {
            this.cmbBoundRate.Text = SysConfig.GetSysConfig().RFIDRWConfig.BoundRate.ToString();
            this.cmbComName.Text = SysConfig.GetSysConfig().RFIDRWConfig.PortName;
            this.cmbDataBit.Text = SysConfig.GetSysConfig().RFIDRWConfig.DataBit.ToString();
            this.cmbPartity.Text = SysConfig.GetSysConfig().RFIDRWConfig.Parity.ToString();
            this.cmbStopBit.Text = SysConfig.GetSysConfig().RFIDRWConfig.PstopBit.ToString();
            this.labTip.Content = string.Empty;
        }
    }
}
