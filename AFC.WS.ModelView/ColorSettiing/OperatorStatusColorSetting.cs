using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.ModelView.ColorSettiing
{
    using AFC.WS.UI.Common;
    

    public class OperatorStatusColorSetting: IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string validEndDate = dr["validity_date_end"].ToString();
            try
            {
                DateTime dt = DateTime.ParseExact(validEndDate, "yyyyMMdd", null);
                if (DateTime.Now.Subtract(dt).TotalDays >= 0)
                {
                    dgr.Background = System.Windows.Media.Brushes.AliceBlue;
                    dgr.ToolTip = "操作员已过期";
                    return;
                }
                if (dr["operator_status"].ToString() == "02")
                {
                    dgr.Background = System.Windows.Media.Brushes.BurlyWood;
                    dgr.ToolTip = "操作员密码已终止";
                    return;
                }
                if (dr["operator_status"].ToString() == "05")
                {
                    dgr.Background = System.Windows.Media.Brushes.SeaGreen;
                    dgr.ToolTip = "未启用";
                    return;
                }
                if (dr["operator_status"].ToString() == "04")
                {
                    dgr.Background = System.Windows.Media.Brushes.DarkCyan;
                    dgr.ToolTip = "操作员已锁定";
                    return;
                }
                if (dr["operator_status"].ToString() == "06")
                {
                    dgr.Background = System.Windows.Media.Brushes.Brown;
                    dgr.ToolTip = "操作员已删除";
                    return;
                }
                else
                {
                    dgr.Background = System.Windows.Media.Brushes.AliceBlue as System.Windows.Media.Brush;
                }
                   
                
              
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
