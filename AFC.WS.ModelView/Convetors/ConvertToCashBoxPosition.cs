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
    public class ConvertToCashBoxPosition : IConvertor
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
                        switch (System.Convert.ToInt32(value.ToString()))
                        {
                            case 01:
                                value = "纸币回收";
                                break;
                            case 02:
                                value = "纸币补充";
                                break;
                            case 03:
                                value = "硬币回收";
                                break;
                            default:
                                value = "未知";
                                break;
                        }
                        return value;
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
