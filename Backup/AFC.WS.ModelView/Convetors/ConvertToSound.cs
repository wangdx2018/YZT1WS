using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Convetors
{
    using AFC.WS.UI.Convertors;
    using AFC.WS.UI.Common;
    using System.Collections;

    public class ConvertToSound:  IConvertor
    {
        #region IValueConverter 成员

        /// <summary>
        /// 将所定状态编码转换成相应文本
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ushort data = 0;

            SoundData sd = new SoundData();

            bool res = ushort.TryParse(value.ToString(), out data);
            if (!res)
            {
                WriteLog.Log_Error("AG sound config set error,not convert to ushort type!");
                return null;
            }

            byte[] valueBuffer = BitConverter.GetBytes(data);

            if (valueBuffer[1] != 0)
            {
                sd.times = valueBuffer[1];
            }

            BitArray bitArray = new BitArray(new byte[1] { valueBuffer[0] });

            byte lowsum = 0;
            for (int i = 0; i < 4; i++)
            {
                lowsum += (byte)(Math.Pow(2, i) * (bitArray.Get(i) ? 1:  0));
            }

            byte highSum = 0;

            for (int i = 4; i < 8; i++)
            {
                highSum += (byte)(Math.Pow(2, i) * (bitArray.Get(i) ? 1:  0));
            }

            if (highSum == 0)
            {
                sd.IsLong = true;
            }
            if (highSum == 16)
            {
                sd.IsLong = false;
            }

            sd.voice = lowsum;

            return sd.ToString();
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion

    }


    internal class SoundData
    {
        public byte voice;

        public byte times;

        public bool IsLong;


        public override string ToString()
        {
            return string.Format("{0}，声音次数为 {1},音量大小为 {2}", IsLong ? "长音" : "短音", times.ToString(), voice.ToString());
            //return base.ToString();
        }
    }

    
}
