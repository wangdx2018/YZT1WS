using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using System.Collections;
using System.Globalization;
using AFC.WS.UI.BR.Data;
using AFC.WS.UI.Config;
using AFC.WS.BR;

namespace AFC.WS.ModelView.Convertors
{
    public class ConvertYuanToFen:  IConvertor
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    if (String.IsNullOrEmpty(value.ToString()))
                    {
                        return 0;
                    }
                    else
                    {
                        value = value.ToString().Replace("￥", "");
                        double yuan = System.Convert.ToDouble(value);
                        int i = System.Convert.ToInt32(yuan * 100);
                        return i;

                    }
                }
                else
                {
                    return value;
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
