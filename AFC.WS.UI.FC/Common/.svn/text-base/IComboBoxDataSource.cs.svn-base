using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Windows.Controls;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// ComboBox和其他控件联动时，继承此接口。
    /// 
    /// 选择数据项时，触发选中事件，调用此接口。
    /// </summary>
    public interface IComboBoxDataSource
    {
        /// <summary>
        /// 根据选择内容，绑定其他ComboBox数据。
        /// </summary>
        /// <param name="condition">选中值和隐藏值</param>
        ///<param name="controlName">控件名称</param>
        ///<param name="com">查找控件对象</param>
        void  BindComboBox(Dictionary<string, string> condition,string [] controlName,object com);
    }
}
