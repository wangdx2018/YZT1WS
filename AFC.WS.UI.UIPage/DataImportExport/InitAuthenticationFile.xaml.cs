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
using System.Windows.Shapes;
using AFC.BOM2.UIController;
using AFC.WS.BR.DataImportExport;
using AFC.WS.UI.BR;

namespace AFC.WS.UI.UIPage.DataImportExport
{
    /// <summary>
    /// InitAuthenticationFile.xaml 的交互逻辑
    /// </summary>
    public partial class InitAuthenticationFile : UserControlBase
    {
        public InitAuthenticationFile()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {
            this.txtPhysicalSN.Text = "";
        }

        private void btuInit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateAuthPhysicalSN validAuthPhysical = new ValidateAuthPhysicalSN();
                GetUSBPhysicalSN gun = new GetUSBPhysicalSN();
                string PathU = validAuthPhysical.getFirstUSB();
                if (PathU == "")
                {
                    AFC.WS.UI.CommonControls.MessageDialog.Show("请正确插入移动硬盘!", "警告", AFC.WS.UI.CommonControls.MessageBoxIcon.Warning, AFC.WS.UI.CommonControls.MessageBoxButtons.Ok);
                    return;
                }

                Boolean valid= validAuthPhysical.writeConf(PathU);
                if (valid)
                {
                    this.txtPhysicalSN.Text = gun.SerchByDeviceLetter(PathU);
                    
                    this.lblResult.Content = "USB初始化成功!";
                }
                else
                {
                    this.lblResult.Content = "USB初始化失败!";
                }
            }
            catch
            {
                this.lblResult.Content = "USB初始化失败!";
            }

        }
    }
}