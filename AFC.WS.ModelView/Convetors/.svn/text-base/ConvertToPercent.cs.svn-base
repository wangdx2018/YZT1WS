using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.ModelView.Convetors
{
    public class ConvertToPercent : IConvertor
    {
        #region IValueConverter 成员

        private string strPercent;
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value!=null)
            {
                 strPercent = value + "%";
            }
            else
            {
                return "0%";
            }
            return strPercent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
