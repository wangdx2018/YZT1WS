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

namespace AFC.WS.UI.Params
{
    /// <summary>
    /// valueAddAmountType.xaml 的交互逻辑
    /// </summary>
    public partial class valueAddAmountType : UserControlBase, ICommonEdit
    {
        public valueAddAmountType()
        {
            InitializeComponent();
        }
        public override void InitControls()
        {
            //SetControlValue(3);
        }

        #region ICommonEdit 成员

        public object GetControlValue()
        {
            int returnValue = 0;
            int[] buffer = new int[2] { 0, 0};
            for (int i = 0; i < 2; i++)
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


            for (int bits = 0; bits < 2; bits++)
            {
                int x = buffer.Length - 1 - bits;
                returnValue = returnValue + Int32.Parse((buffer[bits] * Math.Pow(2, x)).ToString());
            }
            return returnValue;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void SetControlValue(object value)
        {
            ushort selCheck;
            bool res = ushort.TryParse(value.ToString(), out selCheck);//todo:Value 为setControlValue的参数
            if (res) //todo:Convert successful
            {
                byte[] buffer = BitConverter.GetBytes(selCheck);// order is Intel
                Array.Reverse(buffer);// order is moto
                System.Collections.BitArray ba = new System.Collections.BitArray(buffer);
                for (int i = 0; i < 2; i++)
                {

                    if (ba.Get(i + 8))//will return bool result 1:true,0:false
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

        #endregion

        private void btnGetValue_Click(object sender, RoutedEventArgs e)
        {
            string returnValue = GetControlValue().ToString();
            MessageDialog.Show(returnValue, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
        }
    }
}
