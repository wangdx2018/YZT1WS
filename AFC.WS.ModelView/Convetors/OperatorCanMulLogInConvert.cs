using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Convertors
{
    using AFC.WS.UI.Common;

    //-->added by wangdx 20091104 
    /// <summary>
    /// 该转换器是为了转换操作员是否能够登录多台设备信息
    /// 是否允许登录多台设备 01 允许登录多台设备 00不允许
    /// </summary>
    public class OperatorCanMulLogInConvert: IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if (string.IsNullOrEmpty(value.ToString()))
                return null;
            if (value.ToString().Equals("01"))
                return "允许";
            else if (value.ToString().Equals("00"))
                return "不允许";
            else
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
