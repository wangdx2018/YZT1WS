using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace AFC.WS.ModelView.Convetors
{
    using AFC.WS.UI.Common;
    /// <summary>
    /// added by wangdx 20110415
    /// 参数使用范围转换器
    /// </summary>
    public class ParamsUseRange : IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            uint aa =0;
            StringBuilder sb = new StringBuilder();
            aa=value.ToString().ConvertHexStringToUint();
        
                byte[] buffer = BitConverter.GetBytes(aa);
             //   Array.Reverse(buffer);
                BitArray bi = new BitArray(buffer);

                bool flag = false;

                for (int i = 0; i < 11; i++)
                {
                    if (bi.Get(i))
                        flag = true;
                }
                if (!flag)
                {
                    return "未指定";
                }

                if (bi.Get(0))
                { 
                    sb.Append("服务器");
                }
                if (bi.Get(1))
                {
                    sb.Append(",");
                    sb.Append("工作站");
                }
                if (bi.Get(2))
                {
                    sb.Append(",");
                    sb.Append("网络设备");
                }
                if (bi.Get(3))
                {
                    sb.Append(",");
                    sb.Append("UPS");
                }
                if (bi.Get(4))
                {
                    sb.Append(",");
                    sb.Append("E/S");
                }
                if (bi.Get(5))
                {
                    sb.Append(",");
                    sb.Append("AGM");
                }
                if (bi.Get(6))
                {
                    sb.Append(",");
                    sb.Append("BOM");
                }
                if (bi.Get(7))
                {
                    sb.Append(",");
                    sb.Append("TVM");
                }
                if (bi.Get(8))
                {
                    sb.Append(",");
                    sb.Append("AVM");
                }
                if (bi.Get(9))
                {
                    sb.Append(",");
                    sb.Append("TCM");
                }
                if (bi.Get(10))
                {
                    sb.Append(",");
                    sb.Append("PCA");
                }
                return sb.ToString();
            
            return value;

            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
           // throw new NotImplementedException();
        }

        #endregion
    }
}
