using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.ColorSettiing
{
    using AFC.WS.UI.Common;

    public class ParamSynFailedColorSetting : IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            dgr.Background = System.Windows.Media.Brushes.Red;
            dgr.ToolTip = string.Format("该设备参数未同步 服务器版本 {0},设备版本 {1}", dr["local_version"].ToString(), dr["dev_version"].ToString());
        }

        #endregion
    }
}
