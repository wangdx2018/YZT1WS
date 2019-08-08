using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 转成ComboBox下拉模式。
    /// 
    /// 将DLL中获取出来的类的名称以ArrayList的形势，
    /// 
    /// 然后将ArrayList存放到PropertyGrid当中去，
    /// 
    /// 这样就实了ComboBox下拉模式了。
    /// 
    /// </summary>
    public abstract class ComboBoxTypeConvert : TypeConverter
    {
        #region --> Property。

        /// <summary>
        /// DLL 里类名集合。
        /// </summary>
        private List<DllClassProperty> _DllClassNameList;
        /// <summary>
        /// DLL 里类名集合。
        /// </summary>
        public List<DllClassProperty> DllClassNameList
        {
            get { return _DllClassNameList; }
            set { _DllClassNameList = value; }
        }

        #endregion --> Property。

        #region --> Conformation Method

        /// <summary>
        /// 构造函数。 
        /// </summary>
        public ComboBoxTypeConvert()
        {
            _DllClassNameList = new List<DllClassProperty>();
            GetConvertList();
            try
            {
                _DllClassNameList = _DllClassNameList.OrderBy(temp => temp.ClassName).ToList();
            }
            catch (Exception ee)
            {
                Utility.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
        }

        #endregion --> Conformation Method

        #region --> Methods。

        /// <summary>
        /// 获取转换列集合。
        /// </summary>
        public abstract void GetConvertList();
        /// <summary>
        /// 使用指定的上下文返回此对象是否支持可以从列表中选取的标准值集。
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <returns>如果应调用 GetStandardValues 来查找对象支持的一组公共值，则为 true；否则，为 false。</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// 当与格式上下文一起提供时，返回此类型转换器设计用于的数据类型的标准值集合。
        /// </summary>
        /// <param name="context">可用来提取有关从中调用此转换器的环境的附加信息。此参数或其属性 (Property) 可以为 nullNothingnullptrnull 引用（在 Visual Basic 中为 Nothing）。</param>
        /// <returns>包含标准有效值集的 TypeConverter..::.StandardValuesCollection；如果数据类型不支持标准值集，则为 nullNothingnullptrnull 引用（在 Visual Basic 中为 Nothing）。</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            string[] strArray = new string[DllClassNameList.Count];
            int i = 0;
            foreach (DllClassProperty dcp in DllClassNameList)
            {
                strArray[i++] = dcp.ClassName;
            }
            return new StandardValuesCollection(strArray);

        }
        /// <summary>
        /// 返回该转换器是否可以使用指定的上下文将给定类型的对象转换为此转换器的类型。
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="sourceType">表示要转换的类型</param>
        /// <returns>如果该转换器能够执行转换，则为 true；否则为 false</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        /// <summary>
        /// 使用指定的上下文和区域性信息将给定的对象转换为此转换器的类型
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="culture">用作当前区域性的 CultureInfo</param>
        /// <param name="value">要转换的 Object。</param>
        /// <returns>表示转换的 value 的 Object</returns>
        public override object ConvertFrom(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                foreach (DllClassProperty dcp in DllClassNameList)
                {
                    if (dcp.ClassName.Equals((value.ToString())))
                    {
                        return dcp.ClassName;
                    }
                }

                return "";
            }
            return base.ConvertFrom(context, culture, value);
        }
        /// <summary>
        /// 使用指定的上下文和区域性信息将给定的值对象转换为指定的类型
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <param name="culture">CultureInfo。如果传递 nullNothingnullptrnull 引用（在 Visual Basic 中为 Nothing），则采用当前区域性</param>
        /// <param name="value">要转换的 Object。</param>
        /// <param name="destinationType">value 参数要转换成的 Type。</param>
        /// <returns>表示转换的 value 的 Object。</returns>
        public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                return "";
            }
            if (destinationType == typeof(string))
            {
                foreach (DllClassProperty dcp in DllClassNameList)
                {
                    if (dcp.ClassName.Equals((value.ToString())))
                    {
                        return dcp.ClassName;
                    }
                }
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        /// <summary>
        /// 使用指定的上下文返回从 GetStandardValues 返回的标准值的集合是否为可能值的独占列表
        /// </summary>
        /// <param name="context">提供格式上下文</param>
        /// <returns>如果从 GetStandardValues 返回的 TypeConverter..::.StandardValuesCollection 是可能值的穷举列表，则为 true；如果还可能有其他值，则为 false。</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        #endregion --> Methods
    }
}
