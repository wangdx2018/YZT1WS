using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 该特性类继承Attribute，为了筛选自定义属性 
    /// </summary>
    public class FilterAttribute:Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsConfigable = true;
        /// <summary>
        /// 根据HashCode过滤属性。
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            //return base.GetHashCode();
            return base.GetHashCode() + 29 * IsConfigable.GetHashCode();
        }
        /// <summary>
        /// 重写Equals方法
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>TRUE:成功，False：失败</returns>
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            FilterAttribute configAttribute = obj as FilterAttribute;
            if (configAttribute == null) return false;
            if (!base.Equals(obj)) return false;
            if (!Equals(IsConfigable, configAttribute.IsConfigable)) return false;
            return true;
        }
    }
}
