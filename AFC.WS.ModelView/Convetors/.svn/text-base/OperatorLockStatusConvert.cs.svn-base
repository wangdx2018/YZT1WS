using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Convertors
{
    using AFC.WS.UI.Convertors;
    using AFC.WS.UI.Common;
    //--->操作员锁定状态转换器，added by wangdx 20091102
    /// <summary>
    /// 操作员锁定状态转换器
    /// </summary>
    public class OperatorLockStatusConvert: IConvertor
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
            string data = value.ToString();
            if (data.Equals("00"))
                return "未锁定";
            if (data.Equals("01"))
                return "已锁定";
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
