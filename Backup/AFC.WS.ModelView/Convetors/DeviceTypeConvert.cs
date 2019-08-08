using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Convertors
{
    using AFC.WS.UI.Common;
    using System.Data;

    public class DeviceTypeConvert: IConvertor
    {
        #region IValueConverter 成员
        /// <summary>
        /// 设备类型名称转换器。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if(string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
            int res=0;
           DataSet ds=Util.DataBase.SqlQuery(out res, string.Format("select  t.device_name from basi_dev_type_info t where t.device_type='{0}'", value.ToString()));
           if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
           {
                return ds.Tables[0].Rows[0][0].ToString();
            }
           else
           {
               return null;
           }
 
 
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}
