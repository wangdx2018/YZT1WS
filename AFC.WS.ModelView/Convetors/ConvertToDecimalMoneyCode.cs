using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using System.Data;

namespace AFC.WS.ModelView.Convertors
{
    public class ConvertToDecimalMoneyCode : IConvertor
    {
        #region IValueConverter 成员
        

        /// <summary>
        /// 钱箱编码序号由十六进制转化十进制
        /// </summary>
        /// <param name="value">序号为十六进制的钱箱编码</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>序号为十进制的钱箱编码</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value == null)
            //    return null;
            //string moneyCode = value.ToString();
            //string partId = null;
            //if (string.IsNullOrEmpty(moneyCode))
            //    return null;

            //if (moneyCode.Length <= 4)
            //    return moneyCode;

            //string moneyNo = moneyCode.Remove(0, 4);

            //if (!string.IsNullOrEmpty(moneyNo))
            //    partId = moneyCode.Substring(0, 4) + System.Convert.ToInt32(moneyNo, 16).ToString("00000");
            //return partId;
            return new TicketOrMoneyBoxIdConvetor().ConvertBack(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
