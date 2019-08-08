using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.ModelView.Convertors
{
    public class ConvertToActionTime  :IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int time = value.ToString().ToInt32();
            int hour = time/3600;
            int minute = (time % 3600) / 60;
            int Second = time - hour * 3600 - minute * 60;

            string strTime = hour.ToString().PadLeft(2, '0') + "" + minute.ToString().PadLeft(2, '0') + "" + Second.ToString().PadLeft(2, '0');
            return strTime;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
