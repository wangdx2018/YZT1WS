using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Convertors
{
    using AFC.WS.UI.Common;
    using AFC.WS.UI.Convertors;

    //-->added by wangdx 20091103 
    /// <summary>
    /// 操作员状态转换器
    /// </summary>
    public class OperatorStatusConvert:  IConvertor
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return value;
            string status = value.ToString();
            if (string.IsNullOrEmpty(status))
                return null;
            int inStatus = int.Parse(status);
            switch (inStatus)
            {
                case 0:
                    return "正常";
                case 1:
                    return "密码终止";
                case 2:
                    return "停用";
                case 3:
                    return "锁定";
                case 4:
                    return "强制修改密码";
                case 5:
                    return "未启用";
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
