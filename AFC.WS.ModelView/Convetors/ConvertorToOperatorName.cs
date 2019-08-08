using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;
using AFC.WS.BR;

namespace AFC.WS.ModelView.Convetors
{
    public class ConvertorToOperatorName : IConvertor
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
            PrivOperatorInfo info = BuinessRule.GetInstace().operationManager.GetOperatorInfoByOperatorId(value.ToString());
            if (info != null && !string.IsNullOrEmpty(info.operator_id))
            {
                return info.operator_name;
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
