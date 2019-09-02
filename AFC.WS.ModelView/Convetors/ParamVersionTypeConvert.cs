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

namespace AFC.WS.ModelView.Convetors
{
    public class ParamVersionTypeConvert : IConvertor
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
                            case 00:
                                value = "草稿版";
                                break;
                            case 01:
                                value = "当前版";
                                break;
                            case 02:
                                value = "将来版";
                                break;
                            case 03:
                                value = "历史版";
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
            return null;
        }

        #endregion
    }
}
