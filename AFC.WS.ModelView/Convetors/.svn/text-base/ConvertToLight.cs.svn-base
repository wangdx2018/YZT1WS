using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Convetors
{
    using AFC.WS.UI.Convertors;
    using AFC.WS.UI.Common;
    using System.Collections;

    public class ConvertToLight:IConvertor
    {
        #region IValueConverter 成员

        /// <summary>
        /// 灯信息装换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //byte[] lightInfo = new byte[2];
            //string data = value.ToString();

            //lightInfo = System.Text.Encoding.Default.GetBytes(data);

            //if (lightInfo[0].ToString()!="0"&&lightInfo[1].ToString()=="1")
            //    return "红灯亮："+lightInfo[0].ToString()+"次";
            //if (lightInfo[0].ToString() != "0" && lightInfo[1].ToString() == "2")
            //    return "绿灯亮：" + lightInfo[0].ToString() + "次";
            //if (lightInfo[0].ToString() != "0" && lightInfo[1].ToString() == "3")
            //    return "红、绿灯都亮共计：" + lightInfo[0].ToString() + "次";
            //if (string.IsNullOrEmpty(lightInfo[0].ToString()))
            //    return "红绿灯都不亮";
            //return value.ToString();

            ushort data = 0;
            LightData ld = new LightData();
            
            bool res = ushort.TryParse(value.ToString(), out data);
            if (!res)
            {
                WriteLog.Log_Error("AG light config set error,not convert to ushort type!");
                return null;
            }

            byte[] valueBuffer = BitConverter.GetBytes(data);


            ld.times = valueBuffer[1];


            BitArray bitArray = new BitArray(new byte[1] { valueBuffer[0] });
            ld.redLight = bitArray.Get(0);
            //!ld.redLight = !bitArray.Get(0);

            ld.greenLight = bitArray.Get(1);
            //!ld.greenLight = !bitArray.Get(1);

            return ld.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion

    }

    internal class LightData
    {
        public byte times;

        public bool redLight;

        public bool greenLight;


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (redLight && greenLight)
            {
                sb.Append("红绿灯都亮");

            }
            if (redLight && !greenLight)
            {
                sb.Append("红灯亮");
            }

            if (!redLight && greenLight)
            {
                sb.Append("绿灯亮");
            }

            if (!redLight && !greenLight)
            {
                sb.Append("无亮灯");

            }
            sb.Append(string.Format("，闪烁次数为 {0}", times.ToString()));

            return sb.ToString();
        }
    }
}
