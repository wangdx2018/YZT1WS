using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.ModelView.Convertors
{
    public class ConvertToDecimalDevCode:  IConvertor
    {
        #region IValueConverter 成员

        /// <summary>
        /// 设备编码序号由十六进制转化十进制
        /// </summary>
        /// <param name="value">序号为十六进制的设备编码</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>序号为十进制的设备编码</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            string devCode = value.ToString();

            string deviceId = null;
            if (string.IsNullOrEmpty(devCode))
                return null;

            if (devCode.Length <= 6)
                return devCode;

            string devNo = devCode.Remove(0, 6);

            if (!string.IsNullOrEmpty(devNo))
                deviceId = devCode.Substring(0, 6) + System.Convert.ToInt32(devNo, 16).ToString("000");
            return deviceId;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //throw new NotImplementedException();
            return null;
        }

        #endregion
    }
}
