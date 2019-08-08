using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using AFC.WS.UI.Common;
using System.Drawing.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 键值对类；
    /// 
    /// 主要用来存放用户自定义属性的属性名称及属性值的。
    /// 
    /// </summary>
    public class PropertyValue
    {
        #region --> Method

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return Key != null ? Key : this.GetType().Name;
        }

        #endregion --> Method

        #region --> Property

        /// <summary>
        /// 值。
        /// </summary>
        private string _Value;
        /// <summary>
        /// 值。
        /// </summary>
        [XmlAttribute(),
        Category("属性设置")]
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        /// <summary>
        /// 属性名称。
        /// </summary>
        private string _Key;
        /// <summary>
        /// 属性名称。
        /// </summary>
        [XmlAttribute(),
        Category("属性设置")]
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }

        #endregion --> Property.
    }

    /// <summary>
    /// 键值对集合类；
    /// 
    /// 这里主要是存放哪个控件类下面自定义控件属性当中，自定义属性的键值对集合的。
    /// 
    /// </summary>
    public class PropertyValueCollection
    {
        #region --> Property

        /// <summary>
        /// 类名：ActionProperty、ColumnProperty、ControlProperty、DataSourceRule。
        /// </summary>
        private string _ClassName;
        /// <summary>
        /// 类名：ActionProperty、ColumnProperty、ControlProperty、DataSourceRule。
        /// </summary>
        public string ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }
        /// <summary>
        /// 控件名称、绑定字段、数据源名称
        /// </summary>
        private string _FlagName;
        /// <summary>
        /// 控件名称、绑定字段、数据源名称
        /// </summary>
        public string FlagName
        {
            get { return _FlagName; }
            set { _FlagName = value; }
        }

        /// <summary>
        /// 自定义名称：Action名称、DataSource名称、Controls名称、Converter名称
        /// </summary>
        private string _DefineName;
        /// <summary>
        /// 自定义名称：Action名称、DataSource名称、Controls名称、Converter名称
        /// </summary>
        public string DefineName
        {
            get { return _DefineName; }
            set { _DefineName = value; }
        }

        /// <summary>
        /// 自定义键值对集合
        /// </summary>
        private List<PropertyValue> _PropertyList = new List<PropertyValue>();
        /// <summary>
        /// 自定义键值对集合
        /// </summary>
        public List<PropertyValue> PropertyList
        {
            get { return _PropertyList; }
            set { _PropertyList = value; }
        }

        #endregion --> Property.

        #region --> Methods.

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="fv">键值对类</param>
        public void Add(PropertyValue fv)
        {
            if (IsExist(fv))
            {
                Remove(fv);
            }
            PropertyList.Add(fv);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="fv">键值对类</param>
        /// <returns>true 存在,false 不存在</returns>
        private  bool IsExist(PropertyValue fv)
        {
            bool result = false;

            foreach (PropertyValue obj in PropertyList)
            {
                if (obj.Key == fv.Key)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="fp">键值对类</param>
        private  void Remove(PropertyValue fp)
        {
            PropertyValue cp = null;
            foreach (PropertyValue obj in PropertyList)
            {
                if ((obj.Key == fp.Key))// &&                    (obj.TargetName == fp.TargetName))
                {
                    cp = obj;
                    break;
                }
            }
            if (cp != null)
            {
                PropertyList.Remove(cp);
            }
        }

        #endregion --> Methods.
    }
}
