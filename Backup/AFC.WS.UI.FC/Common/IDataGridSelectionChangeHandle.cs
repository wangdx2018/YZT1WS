using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// author:wangdx 
    /// date:20100506
    /// 当列表选择变化时调用该函数
    /// </summary>
    public interface IDataGridSelectionChangeHandle
    {
        /// <summary>
        /// 处理当前的选择列
        /// </summary>
        /// <param name="list">选中行的个数</param>
        void HandleChange(List<QueryCondition> list);
    }
}
