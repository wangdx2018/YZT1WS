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
    public class ConvertToBoxType : IConvertor
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
                            case 0301:
                                value = "发票箱";
                                break;
                            case 0302:
                                value = "废票箱";
                                break;
                            case 0303:
                                value = "回收箱";
                                break;
                            case 0311:
                                value = "硬币回收箱";
                                break;
                            case 0321:
                                value = "纸币补充箱";
                                break;
                            case 0322:
                                value = "纸币回收箱";
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
