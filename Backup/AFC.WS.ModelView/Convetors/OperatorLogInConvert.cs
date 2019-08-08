using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Convertors
{
    using AFC.WS.UI.Common;
    
    //--->added by wangdx 20091104
    /// <summary>
    /// 登录状态转换器 00 登录 01 登出
    /// </summary>
    public class OperatorLogInConvert:IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if (string.IsNullOrEmpty(value.ToString()))
                return null;
            if (value.ToString().Equals("00"))
                return "登录";
            if (value.ToString().Equals("01"))
                return "登出";
            return "不确定";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
            //throw new NotImplementedException();
        }

        #endregion
    }


    /// <summary>
    /// 登录登出状态为了强制登录登出使用
    /// </summary>
    public class OperatorForceLogOutConvert : IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if (string.IsNullOrEmpty(value.ToString()))
                return null;
            if (value.ToString().Equals("00"))
                return "登录";
            if (value.ToString().Equals("01"))
                return "登出";
            return "不确定";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
            //throw new NotImplementedException();
        }

        #endregion
    }
}
