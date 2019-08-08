using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.ModelView.Convertors
{
    public class ConvertToHexDevCode  :IConvertor
    {
        #region IValueConverter 成员

        /// <summary>
        /// 设备编码序号由十六进制转化十进制
        /// </summary>
        /// <param name="value">序号为十六进制的设备编码</param>
        /// <param name="targetType">可以为null</param>
        /// <param name="parameter">可以为null</param>
        /// <param name="culture">可以为null</param>
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
            int devNo = -1;
            try
            {
                devNo = Int32.Parse(devCode.Remove(0, 6));
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("GetHexDevCode函数转换异常：" + ex.ToString());
            }

            if (devNo != -1)
                deviceId = devCode.Substring(0, 6) + devNo.ToString("x2");
            return deviceId;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
