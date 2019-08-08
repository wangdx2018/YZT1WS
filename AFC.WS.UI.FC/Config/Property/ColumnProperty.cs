using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;
using AFC.WS.UI.Common;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 列属性类。
    /// 
    /// 配置DataGridView中列表头的名称、列的绑定字段、列的宽度等。
    /// 
    /// 如果配置的列当中还有对时间进行转换的话，可以配置将此列转成相应的格式的格式。
    /// 
    /// 如在配置DataGridView列中当中有一列为时间，想将时间的格式转换成"YYYY-MM-DD"格式的话
    /// 
    /// 先要选择“选择Convertor”中一个日期转换类，然后再在下面写上"YYYY-MM-DD"就可以转成你想要的格式了。
    /// </summary>
    public class ColumnProperty
    {
        #region --> Methods

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return HeaderName == null ? this.GetType().Name : HeaderName;
        }

        /// <summary>
        /// 获取用户自定义Convertor属性方法
        /// </summary>
        /// <param name="className">类名称</param>
        private void GetUserDefinedConvertorPropertyMethod(string className)
        {
            try
            {
                DllClassProperty dcp = null;
                List<DllClassProperty> dcpList = Utility.Instance.ClassPropertyConvertorList;

                foreach (DllClassProperty obj in dcpList)
                {
                    if (obj.ClassName == className)
                    {
                        dcp = obj;
                        break;
                    }
                }
                if (null != dcp)
                {
                    ConvertoTypeName = dcp.FullName + "," + dcp.AssemblyName;
                    targetConvertor = Activator.CreateInstance(dcp.DllClassType);
                }
            }
            catch (Exception ee)
            {
                //WriteLog.Log_Error(ee);
                Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
            }
        }

        #endregion --> Methods

        #region --> Property.


        bool _IsVisbility = true;

        /// <summary>
        /// 是否显示此列
        /// </summary>
        [Description("是否显示此列"),
        DisplayName("是否显示此列"),
        XmlAttribute(),
        Category("属性设置")]
        public bool IsVisbility
        {
            get { return _IsVisbility; }
            set { _IsVisbility = value; }
        }


        /// <summary>
        /// 表头名称
        /// </summary>
        private string _HeaderName;


        /// <summary>
        /// 表头名称
        /// </summary>
        [Description("表头名称"),
        DisplayName("表头名称"),
        XmlAttribute(),
        Category("属性设置")]
        public string HeaderName
        {
            get { return _HeaderName; }
            set { _HeaderName = value; }
        }

        /// <summary>
        /// 要绑定字段
        /// </summary>
        private string _BindingField;
        /// <summary>
        /// 要绑定字段
        /// </summary>
        [Description("要绑定字段"),
        DisplayName("要绑定字段"),
        XmlAttribute(),
        Category("属性设置")]
        public string BindingField
        {
            get { return _BindingField; }
            set { _BindingField = value; }
        }

        /// <summary>
        /// 格式化
        /// </summary>
        private string _ConvertoTypeName;
        /// <summary>
        /// 格式化
        /// </summary>
        [Description("类名"),
        DisplayName("类名"),
        XmlAttribute(),
        Category("属性设置")]
        public string ConvertoTypeName
        {
            get { return _ConvertoTypeName; }
            set { _ConvertoTypeName = value; }
        }

        /// <summary>
        /// 列宽
        /// </summary>
        private int _Width = 100;
        /// <summary>
        /// 列宽
        /// </summary>
        [DisplayName("列宽"),
        XmlAttribute(),
        Category("属性设置")]
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        /// <summary>
        /// 选择Convertor
        /// </summary>
        private string _ComboBoxConvertor;
        /// <summary>
        /// 选择Convertor
        /// </summary>
        [XmlAttribute(),
        Description("自定义属性"),
        DisplayName("选择Convertor"),
        Category("属性设置"),
        TypeConverter(typeof(TypeConvertPropertyGridConvertor))]
        public string ComboBoxConvertor
        {
            get { return _ComboBoxConvertor; }
            set
            {

                if (String.IsNullOrEmpty(BindingField))
                {
                    MessageBox.Show("请输入绑定字段。");
                }
                else
                {
                    _ComboBoxConvertor = value;

                    GetUserDefinedConvertorPropertyMethod(_ComboBoxConvertor);
                }
            }
        }

        /// <summary>
        /// Convertor自定义属性
        /// </summary>
        Object targetConvertor;
        /// <summary>
        /// Convertor自定义属性
        /// </summary>
        [Filter(),
        DisplayName("Convertor自定义属性"),
        Category("自定义"),
        TypeConverter(typeof(ExpandableObjectConverter))]
        public object TargetConvertor
        {
            get
            {
                if (targetConvertor != null)
                {
                    if (Utility.Instance.IsModify)
                    {
                        PropertyHelper.Restore(_PropertyValues, targetConvertor);
                    }
                    else
                    {
                        PropertyHelper.Save(targetConvertor, this.GetType().Name, BindingField, ComboBoxConvertor);
                        _PropertyValues = PropertyHelper.GetPropertyList(this.GetType().Name, BindingField, ComboBoxConvertor);
                    }
                }
                return targetConvertor;
            }
            set { targetConvertor = value; }
        }
        /// <summary>
        /// 键值对集合。
        /// </summary>
        private List<PropertyValue> _PropertyValues = new List<PropertyValue>();
        /// <summary>
        /// 键值对集合。
        /// </summary>
        [DisplayName("键值对"),
        Category("自定义")]
        public List<PropertyValue> PropertyValues
        {
            get { return _PropertyValues; }
            set { _PropertyValues = value; }
        }

        #endregion --> Property.
    }
}
