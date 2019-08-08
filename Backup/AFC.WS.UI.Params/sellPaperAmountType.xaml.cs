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
    /// sellPaperAmountType.xaml 的交互逻辑
    /// </summary>
    public partial class sellPaperAmountType : UserControlBase, ICommonEdit
    {
        private List<CheckBoxExtend> checkList = new List<CheckBoxExtend>();


        private DataTable dt = new DataTable();

        public sellPaperAmountType()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {
            try
            {
                //this.ctrValueAddType.SetVisible();

                DataTable dt = this.Tag as DataTable;
                if (dt!=null)
                {
                    this.SetControlValue(dt.Rows[0][1]);
                    this.dt = dt;
                    
                 }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex);
            }
            //SetControlValue(3);
        }

        #region ICommonEdit 成员
        
        public object GetControlValue()
        {
            int returnValue = 0;
            int[] buffer = new int[7] { 0, 0, 0, 0, 0, 0, 0};
            for (int i = 0; i < 7; i++)
            {
                string checkboxName = "checkBoxType" + i.ToString();
                object checkObject = this.checkPanel.FindName(checkboxName);
                if (checkObject is CheckBoxExtend)
                {
                    CheckBoxExtend selCheckbox = checkObject as CheckBoxExtend;
                    if (selCheckbox.IsChecked == true)
                    {
                        buffer[i] = 1;
                    }
                }
            }
            Array.Reverse(buffer);


            for (int bits = 0; bits < 7; bits++)
            {
                int x = buffer.Length-1-bits;
                returnValue = returnValue + Int32.Parse((buffer[bits] * Math.Pow(2,x)).ToString());              
            }
            return returnValue;
           
        }

        public void Initialize()
        {
            
        }

        public void SetControlValue(object value)
        {
            ushort selCheck;
            bool res = ushort.TryParse(value.ToString(), out selCheck);//todo:Value 为setControlValue的参数
            if (selCheck > 127)
            {
                return;
            }
            if (res) //todo:Convert successful
            {
                byte[] buffer = BitConverter.GetBytes(selCheck);// order is Intel
                Array.Reverse(buffer);// order is moto
                System.Collections.BitArray ba = new System.Collections.BitArray(buffer);
                for (int i = 0; i < 7; i++)
                {

                    if (ba.Get(i+8))//will return bool result 1:true,0:false
                    {
                        string checkboxName = "checkBoxType" + i.ToString();
                        object checkObject = this.checkPanel.FindName(checkboxName);
                        if (checkObject is CheckBoxExtend)
                        {
                            CheckBoxExtend selCheckbox = checkObject as CheckBoxExtend;
                            selCheckbox.IsChecked = true;
                        }
                    }
                }
                //此时需要Set UI Element
            }
            else
            {
                //todo: convert failed 
            }

        }

        public void SetHeader(string headerContent)
        {
            this.groupBoxHeader.Header = headerContent;
        }

        public void SetBtnVisible(Visibility vs)
        {
            this.btnGetValue.Visibility = vs;
        }

        public void SetCheckBoxVisible(Visibility vs)
        {
            this.checkBoxType2.Visibility = vs;
            this.checkBoxType3.Visibility = vs;
            this.checkBoxType4.Visibility = vs;
            this.checkBoxType5.Visibility = vs;
            this.checkBoxType6.Visibility = vs;
        }

        #endregion

        private void btnGetValue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Para4043TvmCashBox data = DBCommon.Instance.GetModelValue<Para4043TvmCashBox>(string.Format("select * from para_4043_tvm_cash_box where para_version='-1'"));

                //if (data != null)
                //{
                //    data.para_version = "-1";

                //    data.sell_paper_amount_type = this.GetControlValue().ToString();
                //}
               int res= 0;

               Util.DataBase.SqlCommand(out res, string.Format("update para_4043_tvm_cash_box set sell_paper_amount_type={0} where para_version=-1",this.GetControlValue().ToString()));


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
