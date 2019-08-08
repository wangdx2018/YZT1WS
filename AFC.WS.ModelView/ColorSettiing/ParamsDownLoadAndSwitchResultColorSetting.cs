﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.ModelView.ColorSettiing
{
    public class ParamsSwitchResultColorSetting:  IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            if (dr["switch_result"].ToString() == "成功")
            {
                dgr.ToolTip = "参数切换成功";

            }
            else
            {
                dgr.ToolTip = "参数切换失败";
                dgr.Background = System.Windows.Media.Brushes.Red;
            }
        }

        #endregion
    }


    public class ParamDownLoadResultColorSetting:  IDataGridRowColorSetting
    {

        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            if (dr["down_result"].ToString() == "成功")
            {
                dgr.ToolTip = "参数下载成功";

            }
            else
            {
                dgr.ToolTip = "参数下载失败";
                dgr.Background = System.Windows.Media.Brushes.Red;
            }
        }

        #endregion
    }

}
