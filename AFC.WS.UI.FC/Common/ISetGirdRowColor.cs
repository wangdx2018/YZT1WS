using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Windows.Controls;
using System.Data;

namespace AFC.WS.UI.Common
{
   /// <summary>
   /// 设置列表某列的颜色
   /// </summary>
    public interface IDataGridRowColorSetting
    {
         void SetCurrentDataGridRow(DataGridRow dgr, DataRow dr);
    }
}
