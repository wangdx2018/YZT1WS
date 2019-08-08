using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Collections.Specialized;
using System.IO;



namespace AFC.WS.UI.CoreUI
{
    /// <summary>
    /// 物理网络连接图片转换器
    /// </summary>
    public class NetWorkImageConvert : IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return "\\Image\\LocalStatus\\network.gif";
            else
            {
                return "\\Image\\LocalStatus\\network2_off.gif";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// 物理网络连接提示转换器
    /// </summary>
    public class NetWorkToolTipConvert : IValueConverter
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
              if((bool)value)
                return "网络物理连接正常";
            else
                return "网络物理连接异常";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// 数据库连接网络转换器
    /// </summary>
    public class DBConnectionImageConvert : IValueConverter
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if ((bool)value)
            {
                return "\\Image\\LocalStatus\\database.gif";
            }
            else
            {

                return "\\Image\\LocalStatus\\database_off.gif";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// 数据库连接提示转换器
    /// </summary>
    public class DBConnectionToolTipConvert : IValueConverter
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return "数据库连接正常";
            else
                return "数据库连接异常";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }



    public class OnLineImageConvert : IValueConverter
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return "\\Image\\LocalStatus\\server.gif";
            else
                return "\\Image\\LocalStatus\\server_off.gif";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class OnLineToolTipConvert : IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return "服务器连接正常";
            else
                return "服务器连接异常";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }



    public class RFIDRWImageConvert : IValueConverter
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if (string.IsNullOrEmpty(value.ToString()))
                return null;
            int res = 0;
            if (int.TryParse(value.ToString(), out res) && res == 0)
                return "\\Image\\server.gif";
            else
                return "\\Image\\server_off.gif";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class RFIDRWToolTipConvert : IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if (string.IsNullOrEmpty(value.ToString()))
                return null;
            int res = 0;
            if (int.TryParse(value.ToString(), out res) && res == 0)
                return "RFID读写器连接正常";
            else
                return "RFID读写器连接异常";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class ImageConvert : IValueConverter
    {

        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            if (string.IsNullOrEmpty(value.ToString()))
                return null;
            if (value.ToString().Contains("正常"))
            {
                return "afc_3.jpg";
            }
            if (value.ToString().Contains("紧急"))
            {
                return "afc_2.jpg";
            }
            else
                return "afc_1.jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}