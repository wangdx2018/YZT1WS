using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.ModelView.Convetors
{
   public class PartStatusConvert :IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "";
            string status = value.ToString();
            if (string.IsNullOrEmpty(status))
                return "";
            int inStatus = int.Parse(status);
            switch (inStatus)
            {
                case 0:
                    return "在库";
                case 1:
                    return "在人";
                case 2:
                    return "调出";
                case 3:
                    return "作废";
                default:
                    return "未知状态";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
