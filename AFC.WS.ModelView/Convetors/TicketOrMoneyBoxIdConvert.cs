using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.ModelView.Convertors
{
    /// <summary>
    /// 钱票箱ID转换器
    /// edit by wangdx  将票箱编码变成了大写数字
    /// </summary>
    public class TicketOrMoneyBoxIdConvetor : IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                //if(value==null || String.IsNullOrEmpty( value.ToString()))
                //    return value;
                //string boxId = value.ToString();
                //if (boxId.Length != 8)
                //{
                //    return value;
                //}

                //ushort dd = 0;
                //string secNumber = boxId.Substring(4, 4);//取出后四位
                //string data = secNumber.Substring(2, 2) + secNumber.Substring(0, 2);//转入字节序
                //bool res = ushort.TryParse(data, System.Globalization.NumberStyles.HexNumber, null, out dd);
                //if (!res)
                //    return boxId;
                //string bb = dd.ToString("d5");
                //return (boxId.Substring(0, 4) + bb).ToLower();
                return value.ToString();

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return value.ToString();
                //if (value == null)
                //    return null;
                //if (string.IsNullOrEmpty(value.ToString()))
                //    return value;
                //if (value.ToString().Length != 9)
                //    return value;
                //string sec = value.ToString().Substring(4, 5);
                //ushort aa = 0;
                //bool res = ushort.TryParse(sec, out aa);
                //if (!res)
                //    return value.ToString();
                //byte[] buffer = BitConverter.GetBytes(aa);
                //StringBuilder sb = new StringBuilder();
                //for (int i = 0; i < buffer.Length; i++)
                //{
                //    sb.Append(buffer[i].ToString("X2"));
                //}
                //return (value.ToString().Substring(0, 4) + sb.ToString());
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("ConvertBack异常" + ex.ToString());
            }
            return null;
        }

        #endregion
    }
}
