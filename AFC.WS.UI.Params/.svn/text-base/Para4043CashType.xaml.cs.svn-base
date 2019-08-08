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
using AFC.WS.UI.CommonControls;
using System.Collections;
using System.Data;

namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using System.Data;
    using AFC.WS.UI.Common;
    using AFC.WS.Model.DB;
    using AFC.WS.UI.CommonControls;
    /// <summary>
    /// Para4043CashType.xaml 的交互逻辑
    /// </summary>
    public partial class Para4043CashType : UserControlBase
    {
        Para4043TvmCashBox para4043 = new Para4043TvmCashBox();
        
        public Para4043CashType()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
          
            this.ctrSellPaperType.SetHeader("售票接收的纸币种类");
            this.ctrValueAddType.SetHeader("充值接收的纸币种类");

            this.ctrSellPaperType.SetBtnVisible(Visibility.Collapsed);
            this.ctrValueAddType.SetBtnVisible(Visibility.Collapsed);

           // this.ctrValueAddType.SetCheckBoxVisible(Visibility.Hidden);

            DataTable dt = this.Tag as DataTable;
            List<Para4043TvmCashBox> list = DBCommon.Instance.SetTModelValue<Para4043TvmCashBox>(dt);
            this.ctrSellPaperType.SetControlValue(list[0].sell_paper_amount_type);
            this.ctrValueAddType.SetControlValue(list[0].value_add_amount_type);
                        
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int res = 0;

                Util.DataBase.SqlCommand(out res, string.Format("update para_4043_tvm_cash_box set sell_paper_amount_type={0} where para_version='-1'", this.ctrSellPaperType.GetControlValue().ToString()));
                Util.DataBase.SqlCommand(out res, string.Format("update para_4043_tvm_cash_box set value_add_amount_type={0} where para_version='-1'", this.ctrValueAddType.GetControlValue().ToString()));


                if (res == 0)
                {
                    MessageDialog.Show("更新成功！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }
                else
                {
                    MessageDialog.Show("更新失败！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                }

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
                MessageDialog.Show("更新失败！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
            }

        }



        
    }
}
