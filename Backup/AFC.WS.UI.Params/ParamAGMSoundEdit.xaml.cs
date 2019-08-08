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
    /// ParamAGMSoundEdit.xaml 的交互逻辑
    /// </summary>
    public partial class ParamAGMSoundEdit : UserControlBase, ICommonEdit
    {
        private ushort soundValue;

        public ParamAGMSoundEdit()
        {
            InitializeComponent();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public object GetControlValue()
        {
            if (string.IsNullOrEmpty(this.txtSoundNum.Text) && string.IsNullOrEmpty(this.txtVolume.Text))
            {
                MessageDialog.Show("声音次数/音量大小不能为空！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
                //todo:show error
            }
            if (this.txtSoundNum.Text.ToInt32() > 255)
            {
                MessageDialog.Show("声音次数不能超过255！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);

                return null;
            }
            if (this.txtVolume.Text.ToInt32()>15)
            {
                MessageDialog.Show("音量大小不能超过15！", "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                return null;
            }


            byte[] voiceInfo = new byte[2];
            
            byte voice = 0;
            if (this.radShortSound.IsChecked.Value)
            {
                voice += 16;
            }
            if (this.radLongSound.IsChecked.Value)
            {
                voice += 0;
            }

            byte volume = 0;
            volume = txtVolume.Text.ToByte();

            voiceInfo[1] = txtSoundNum.Text.ToByte();
            voiceInfo[0] = Convert.ToByte((int)voice + (int)volume);

            this.soundValue = BitConverter.ToUInt16(voiceInfo, 0);
            return (object)this.soundValue;
 
        }

        public void SetHeader(string header)
        {
            this.groupHeader.Header = header;
        }

        public void SetRadButtonVoiceGroup(string voice)
        {
            this.radLongSound.GroupName = voice;
            this.radShortSound.GroupName = voice;
        }

        public void SetControlValue(object value)
        {
            if (value == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(value.ToString()))
            {
                this.radLongSound.IsChecked = true;
                txtSoundNum.Text = "0";
                txtVolume.Text = "0";
                return;
                
 
            }
                

            ushort data = 0;
            bool res = ushort.TryParse(value.ToString(), out data);
            if (!res)
            {
                WriteLog.Log_Error("AG sound config set error,not convert to ushort type!");
                return;
            }

            byte[] valueBuffer = BitConverter.GetBytes(data);

            if (valueBuffer[1] != 0)
            {
                this.txtSoundNum.Text = valueBuffer[1].ToString();
            }

            BitArray bitArray = new BitArray(new byte[1] { valueBuffer[0] });




            //this.radLongSound.IsChecked = bitArray.Get(0);
            //this.radShortSound.IsChecked = bitArray.Get(1);

            byte lowsum = 0;
            for(int i=0;i<4;i++)
            {
                lowsum += (byte)(Math.Pow(2, i) * (bitArray.Get(i)? 1:0));
            }

            byte highSum = 0;

            for (int i = 4; i < 8; i++)
            {
                highSum += (byte)(Math.Pow(2, i) * (bitArray.Get(i) ? 1 : 0));
            }

            if (highSum == 0)
            {
                this.radLongSound.IsChecked = true;
            }
            if (highSum == 16)
            {
                this.radShortSound.IsChecked = true;
            }

            txtVolume.Text = lowsum.ToString();
            //BitArray bitlow = new BitArray(new byte[1] { lowsum });
            //BitArray bithigh = new BitArray(new byte[1] { highSum });

            //this.radLongSound.IsChecked = bitlow.Get(0);

            //todo: set ui lowsum,highSum
            //todo: set sound 大小
        }

        private void UserControlBase_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
