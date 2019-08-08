using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.Convertors
{
    using AFC.WS.UI.Common;
    using AFC.WorkStation.DB;
  
    /// <summary>
    /// added by wangdx 20091216 
    /// 操作员所在公司转换器
    /// </summary>
    public class OperatorCompanyConvert: IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
          if(value==null)
              return null;
            if(string.IsNullOrEmpty(value.ToString()))
                return null;
            switch(value.ToString())
            {
                case "1":
                    return "运营一公司";
                case "2":
                    return "运营二公司";
                case "3":
                    return "通号公司";
                case "4":
                    return "营销公司";
                default:
                    return null;
            }
            
         
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 所在的线路
        /// </summary>
        [Filter()]
        public string LineId
        {
            get;
            set;
        }

        #endregion
    }
}
