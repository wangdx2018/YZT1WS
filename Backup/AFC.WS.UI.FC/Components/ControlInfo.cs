using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Config;
using AFC.WS.UI.Common;
using System.Windows;
using System.Windows.Controls;


namespace AFC.WS.UI.Components
{


    /// <summary>
    /// 该类中定义了增加了Label之后的最终控件
    /// 包括控件实例和控件布局（是否占据一行）
    /// </summary>
    internal class FinalControl
    {
        /// <summary>
        /// 控件实例
        /// </summary>
        public UIElement element = null;

        /// <summary>
        /// 是否占据1行
        /// </summary>
        public bool isSingleRow = false;

        /// <summary>
        /// 如果是占有多为占有的行数
        /// </summary>
        public int occupyRowCount = 0;

        /// <summary>
        /// 控件名字
        /// </summary>
        public string name;

        /// <summary>
        /// 控件的Label字段
        /// </summary>
        public Label label = null;

        /// <summary>
        /// 控件的属性
        /// </summary>
        public ControlProperty property;
    }

}
