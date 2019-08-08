using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.UIRuleFileCreator
{
    /// <summary>
    /// 用于给ComboBox赋值用。
    /// </summary>
    public class ItemCom
    {
        #region --> Methods

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion --> Methods

        #region --> Property

        /// <summary>
        /// 名称
        /// </summary>
        private string _Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        /// <summary>
        /// value值
        /// </summary>
        private string _Value;
        /// <summary>
        /// value值
        /// </summary>
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        /// <summary>
        /// 对象
        /// </summary>
        private object _Tag;
        /// <summary>
        /// 对象
        /// </summary>
        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }

        #endregion --> Property.

        #region --> Conformation Methods

        /// <summary>
        /// 构造方法 
        /// </summary>
        public ItemCom()
        {
        }
        /// <summary>
        /// 构造方法 
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">value值</param>
        public ItemCom(string name, string value)
            : this(name, value, null)
        {
        }
        /// <summary>
        /// 构造方法 
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="tag">对象</param>
        public ItemCom(string name, object tag)
            : this(name, null, tag)
        {

        }
        /// <summary>
        /// 构造方法 
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">value值</param>
        /// <param name="tag">对象</param>
        public ItemCom(string name, string value, object tag)            
        {
            this.Name = name;
            this.Value = value;
            this.Tag = tag;
        }

        #endregion --> Conformation Methods
    }
}
