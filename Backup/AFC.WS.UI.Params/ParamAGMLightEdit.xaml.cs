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

namespace AFC.WS.UI.Params
{
    using AFC.BOM2.UIController;
    using AFC.WS.UI.Common;
    using System.Collections;
    using AFC.WS.UI.CommonControls;
    /// <summary>
    /// ParamAGMLightEdit.xaml 的交互逻辑
    /// </summary>
    public partial class ParamAGMLightEdit : UserControlBase,ICommonEdit
    {

        private ushort lightValue;


        

        public ParamAGMLightEdit()
        {
            InitializeComponent();
        }

        public void SetHeader(string headerContent)
        {
            this.groupHeader.Header = headerContent;
        }

        public void SetRadioButtonLightGroup(string redGroupName,string greenGroupName)
        {
            // Light Group
            this.radGreenNo.GroupName = greenGroupName;
            this.radGreenYes.GroupName = greenGroupName;

            this.radRedNo.GroupName = redGroupName;
            this.radRedYes.GroupName = redGroupName;
        }




        #region ICommonEdit 成员

        //Update the database
        public object GetControlValue()
        {
            if (string.IsNullOrEmpty(this.txtFlashNum.Text))
            {
                MessageDialog.Show("闪烁次数不能为空！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
               //todo:show error
            }
            if (this.txtFlashNum.Text.ToInt32() > 255)
            {
                MessageDialog.Show("闪烁次数不能超过255！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
            }
            
            byte[] lightInfo=new byte[2];

           

            byte swapValue=0;

            if (this.radRedYes.IsChecked.Value)
            {
                swapValue += 1;
            }
            if (this.radGreenYes.IsChecked.Value)
            {
                swapValue += 2;
            }

            if (this.radRedNo.IsChecked.Value &&
                this.radGreenNo.IsChecked.Value)
            {
                this.txtFlashNum.Text = "0";
            }

            lightInfo[1] = txtFlashNum.Text.ToByte();
            lightInfo[0] = swapValue;
            this.lightValue = BitConverter.ToUInt16(lightInfo, 0);
            return (object)this.lightValue;

            //todo: 1.prepare first byte according light swaple count
            //todo: 2.prepare 2 byte according light config
            //todo: call BitConvert.GetUint16
            //todo: this.lightValue=BitConvert.GetUint16
            //byte[] ad =new byte[2];

            //ad[0]=2;
            //ad[1]=1;
            //this.lightValue=BitConverter.ToUInt16(ad, 0);
            //return (object)this.lightValue;
        }

        public void Initialize()
        {
            
            throw new NotImplementedException();
        }

        //Get the light information from database
        public void SetControlValue(object value)
        {
            if (value == null)
            {
                return;
             }
                
            if (string.IsNullOrEmpty(value.ToString()))
            {
                this.radRedNo.IsChecked = true;
                this.radGreenNo.IsChecked = true;
                this.txtFlashNum.Text = "0";
                return;
            }
            ushort data=0;
            bool res = ushort.TryParse(value.ToString(), out data);
            if (!res)
            {
                WriteLog.Log_Error("AG light config set error,not convert to ushort type!");
                return;
            }

            byte[] valueBuffer = BitConverter.GetBytes(data);

            
                this.txtFlashNum.Text = valueBuffer[1].ToString();
           

            BitArray bitArray = new BitArray(new byte[1] { valueBuffer[0] });

            this.radRedYes.IsChecked = bitArray.Get(0);
            this.radRedNo.IsChecked = !bitArray.Get(0);

            this.radGreenYes.IsChecked = bitArray.Get(1);
            this.radGreenNo.IsChecked = !bitArray.Get(1);


          
        }

        #endregion

        private void UserControlBase_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void radRedNo_Checked(object sender, RoutedEventArgs e)
        {
            if (radGreenNo.IsChecked.Value && radRedNo.IsChecked.Value)
            {
                txtFlashNum.Text = "0";
            }
        }

        private void radGreenNo_Checked(object sender, RoutedEventArgs e)
        {
            if (radGreenNo.IsChecked.Value && radRedNo.IsChecked.Value)
            {
                txtFlashNum.Text = "0";
            }
        }
    }
}
