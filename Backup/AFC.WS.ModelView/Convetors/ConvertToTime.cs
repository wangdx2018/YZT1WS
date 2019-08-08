using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using System.Windows.Data;
using AFC.WS.UI.Config;

namespace AFC.WS.ModelView.Convertors
{
    public class ConvertToTime:IConvertor
    {
        #region IValueConverter 成员
        // 摘要
        //     转换值。
        //
        // 参数
        //   value
        //     绑定源生成的值。
        //
        //   targetType
        //     绑定目标属性的类型。
        //
        //   parameter
        //     要使用的转换器参数。
        //
        //   culture
        //     要用在转换器中的区域性。
        //
        // 返回结果
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
            if (value == null)
            {
                return null;
            }
            string time = value.ToString();
            if (String.IsNullOrEmpty(time))
            {
                return null;
            }
            string result = "";
            try
            {
                switch (time.Length)
                {
                    case 4:
                        result = time.Substring(0, 2) + ":" + time.Substring(2);
                        break;
                    case 5:
                        result = time.Substring(0, 1) + ":" + time.Substring(1, 2) + ":" + time.Substring(3);
                        break;
                    case 6:
                        result = time.Substring(0, 2) + ":" + time.Substring(2, 2) + ":" + time.Substring(4);
                        break;
                    default:
                        result = time;
                        break;
                }
            }
            catch (Exception ee)
            {
               // Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return result;
        }
        //
        // 摘要
        //     转换值。
        //
        // 参数
        //   value
        //     绑定目标生成的值。
        //
        //   targetType
        //     要转换到的类型。
        //
        //   parameter
        //     要使用的转换器参数。
        //
        //   culture
        //     要用在转换器中的区域性。
        //
        // 返回结果
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
