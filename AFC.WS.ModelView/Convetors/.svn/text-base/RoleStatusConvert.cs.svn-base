using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Convertors
{
    using AFC.WS.UI.Convertors;
    using AFC.WS.UI.Common;
    //-->added by wangdx 20091109
    /// <summary>
    /// 转换RoleStatus 0已启用  1：已禁用  2：已删除  3：未启用 
    /// </summary>
    public class RoleStatusConvert: IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int res=System.Convert.ToInt32(value, null);
                switch (res)
                {
                    case 0:
                        return "已启用";
                    case 1:
                        return "已禁用";
                    case 2:
                        return "已删除";
                    case 3:
                        return "未启用";
                }
                return null;

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
