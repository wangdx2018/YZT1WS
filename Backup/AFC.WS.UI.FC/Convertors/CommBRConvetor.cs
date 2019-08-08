using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.UI.Components;

namespace AFC.WS.UI.FC.Convertors
{
    /// <summary>
    /// added by wangdx  20120110
    ///通用业务处理转换器
    /// </summary>
    public class CommBRConvetor : IConvertor
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                WriteLog.Log_Error(this.GetType().ToString() + " convert is error!");
                return string.Empty;
            }
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return string.Empty;
            }
            if (this.hashTable.ContainsKey(value.ToString()))
            {
                return
                    UIHelper.GetBindingFormatValue(this.hashTable[value.ToString()]);
            }
            if (string.IsNullOrEmpty(this.UnDefined))
                return string.Empty;
            return UIHelper.GetBindingFormatValue(unDefined);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 存放key,value的键值对数据，能够根据键值对
        /// key对应的值，
        /// value对应的资源字段的值，或者是文本
        /// </summary>
        private Dictionary<string, string> hashTable = new Dictionary<string, string>();

       
        /// <summary>
        /// HashTable数据字段
        /// </summary>
        public Dictionary<string, string> HashTable
        {
            get { return hashTable; }
            set { hashTable = value; }
        }

        private string unDefined;

        /// <summary>
        /// 显示未定义的显示，可以是文本也可以是资源ID
        /// </summary>
        public string UnDefined
        {
            set { this.unDefined = value; }
            get { return this.unDefined; }
        }

    



    }
}
