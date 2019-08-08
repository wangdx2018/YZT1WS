using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 所有控件都实现该接口
    /// </summary>
    public interface ICommonEdit
    {
        /// <summary>
        /// 初始化控件。
        /// </summary>
        void Initialize();
        /// <summary>
        /// 得到控件中的数据
        /// </summary>
        /// <returns>返回该控件的数值</returns>
        object GetControlValue();

        /// <summary>
        /// 设置控件中的数据
        /// </summary>
        /// <param name="value">将控件中的数据设置到控件中</param>
        void SetControlValue(object value);
    }
}
