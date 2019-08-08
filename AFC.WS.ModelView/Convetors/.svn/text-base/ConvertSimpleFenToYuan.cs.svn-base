using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.BR.Data;
using AFC.WS.UI.Common;
using AFC.WS.UI.Config;
using AFC.WS.BR;
using System.Collections;
using System.Globalization;


namespace AFC.WS.ModelView.Convertors
{
   public class ConvertSimpleFenToYuan:  IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    if (value.Equals(DBNull.Value))
                        return "0 ";
                    int fen = System.Convert.ToInt32(value);
                    decimal temp = System.Convert.ToDecimal(fen);
                    decimal result = temp / 100;
                    return result;
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
