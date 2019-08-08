using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 图表控件和列表控件的刷新功能接口 
    /// </summary>
    public interface IDataSourceClient
    {
        /// <summary>
        /// 数据源变化的时候的处理程序
        /// </summary>
        void HandleDataSourceChange();

        /// <summary>
        /// 数据源销毁时的处理
        /// </summary>
        void HandleDataSourceDispose();
    }
}
