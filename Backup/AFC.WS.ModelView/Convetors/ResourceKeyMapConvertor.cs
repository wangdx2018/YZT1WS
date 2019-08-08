using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.Convertors;
using AFC.WS.BR;
using AFC.WS.UI.Components;

namespace AFC.WS.ModelView.Convertors
{
   
    public class ResourceKeyMapConvertor : IConvertor
    {
        /// <summary>
        /// 参数名称
        /// </summary>
        [Filter()]
        public string ResourceKeyName
        {
            set;
            get;
        }

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }
            if (string.IsNullOrEmpty(ResourceKeyName))
            {
                return null;
            }
            return UIHelper.GetResouceValueByID("_RKMC_" + ResourceKeyName + "_" + value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}