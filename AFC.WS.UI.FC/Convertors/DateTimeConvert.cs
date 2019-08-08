using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using System.Windows.Data;
using System.Windows.Converters;
using System.Windows;

//定义所有的Convert
namespace AFC.WS.UI.Convertors
{
    /// <summary>
    /// 日期时间格式，继承IConvertor接口。
    /// </summary>
    public class DateTimeConvert:IConvertor
    {
        /// <summary>
        /// 日期时间格式
        /// </summary>
        [Filter()]
        public string DateTimeFormat
        {
            set;
            get;
        }


        #region IValueConverter 成员
        // 摘要:
        //     转换值。
        //
        // 参数:
        //   value:
        //     绑定源生成的值。
        //
        //   targetType:
        //     绑定目标属性的类型。
        //
        //   parameter:
        //     要使用的转换器参数。
        //
        //   culture:
        //     要用在转换器中的区域性。
        //
        // 返回结果:
        //     转换后的值。如果该方法返回 null，则使用有效的 null 值。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return "";
                }
                if (String.IsNullOrEmpty(value.ToString()))
                {
                    Config.Utility.Instance.ConsoleWriteLine("value error:value is [" + value + "]。", Config.LogFlag.InfoFormat);
                    return "";
                }

                if (value.ToString().Length < 6)
                {
                    Config.Utility.Instance.ConsoleWriteLine("value format error:value is [" + value + "]。", Config.LogFlag.InfoFormat);
                    return "";
                }
                string dateFormat = value.ToString().Substring(0, 4) + "-" + value.ToString().Substring(4, 2) + "-" + value.ToString().Substring(6, 2);

                return dateFormat;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("Convert error:params[" + this.DateTimeFormat + "]!!" + ex.Message);
                return null;
            }
        }
        //
        // 摘要:
        //     转换值。
        //
        // 参数:
        //   value:
        //     绑定目标生成的值。
        //
        //   targetType:
        //     要转换到的类型。
        //
        //   parameter:
        //     要使用的转换器参数。
        //
        //   culture:
        //     要用在转换器中的区域性。
        //
        // 返回结果:
        //     转换后的值。如果该方法返回 null，则使用有效的 null 值。
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //throw new NotImplementedException();
            return null;
        }

        #endregion
    }

  
}
