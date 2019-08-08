using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using System.Data;

namespace AFC.WS.ModelView.Convertors
{
   public class StationCovert: IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
            int res = 0;
            DataSet ds = Util.DataBase.SqlQuery(out res, string.Format("select t.station_cn_name from basi_station_info t where t.station_id='{0}'", value.ToString()));
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