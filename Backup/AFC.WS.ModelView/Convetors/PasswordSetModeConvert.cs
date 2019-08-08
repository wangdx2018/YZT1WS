using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Convertors
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.Convertors;

    public class PasswordSetModeConvert:  IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;
            string data = value.ToString();
          
            if (data.Equals("00"))
                return "系统设置";
            if (data.Equals("01"))
                return "人为设置";
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
