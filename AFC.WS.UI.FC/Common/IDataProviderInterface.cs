using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.Windows.Controls;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 
    /// 数据联动控件中使用，用户需提供两级数据内容，则需继承此接口实现，
    /// 
    /// 在DataGrid加载时调用。
    /// </summary>
    public interface IDataProviderInterface
    {
        /// <summary>
        /// 第一级DataGrid加载数据内容
        /// </summary>
        /// <returns>DataView</returns>
        DataView LoadListData();

        /// <summary>
        /// 点击DataGrid行数据，加载数据内容
        /// </summary>
        /// <param name="dataRowView">行数据集合</param>
        /// <returns>DataView</returns>
        DataView LoadDetailData(DataRowView dataRowView);

    }
}
