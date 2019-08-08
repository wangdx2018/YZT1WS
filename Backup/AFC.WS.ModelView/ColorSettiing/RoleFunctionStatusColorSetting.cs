using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.BR;

namespace AFC.WS.ModelView.ColorSettiing
{
    using AFC.WS.UI.Common;

    public class FunctionStatusColorSetting: IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string status = dr["function_status"].ToString();

            switch (status)
            {
             
                case "01":
                    dgr.Background = System.Windows.Media.Brushes.BurlyWood;
                    dgr.ToolTip = "已禁用";
                    break;
                case "02":
                    dgr.Background = System.Windows.Media.Brushes.Brown;
                    dgr.ToolTip = "已删除";
                    break;
                case "03":
                    dgr.Background = System.Windows.Media.Brushes.SeaGreen;
                    dgr.ToolTip = "未启用";
                    break;
                default:
                    dgr.Background = System.Windows.Media.Brushes.AliceBlue as System.Windows.Media.Brush;
                    break;

            }
        }

        #endregion
    }


    public class RoleStatusColorSetting:  IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string status = dr["role_status"].ToString();

            switch (status)
            {

                case "01":
                    dgr.Background = System.Windows.Media.Brushes.BurlyWood;
                    dgr.ToolTip = "已禁用";
                    break;
                case "02":
                    dgr.Background = System.Windows.Media.Brushes.Brown;
                    dgr.ToolTip = "已删除";
                    break;
                case "03":
                    dgr.Background = System.Windows.Media.Brushes.SeaGreen;
                    dgr.ToolTip = "未启用";
                    break;
                default:
                    dgr.Background = System.Windows.Media.Brushes.AliceBlue as System.Windows.Media.Brush;
                    break;

            }
        }

        #endregion
    }


    public class OpeClassColorSetting : IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string status = dr["oper_class"].ToString();

            switch (status)
            {

                case "警告":
                    dgr.Background = System.Windows.Media.Brushes.Brown;
                    dgr.ToolTip = "警告";
                    break;
                case "故障":
                    dgr.Background = System.Windows.Media.Brushes.Red;
                    dgr.ToolTip = "故障";
                    break;
                default:
                    dgr.Background = System.Windows.Media.Brushes.AliceBlue as System.Windows.Media.Brush;
                    break;

            }
        }

        #endregion
    }

    public class dataFileColorSetting:  IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string status = dr["status_name"].ToString();

            switch (status)
            {

                case "失败":
                    dgr.Background = System.Windows.Media.Brushes.Red;
                    dgr.ToolTip = "失败";
                    break;
                case "检查失败":
                    dgr.Background = System.Windows.Media.Brushes.Red;
                    dgr.ToolTip = "检查失败";
                    break;
                case "未处理":
                    dgr.Background = System.Windows.Media.Brushes.Red;
                    dgr.ToolTip = "未处理";
                    break;
                default:
                    dgr.Background = System.Windows.Media.Brushes.AliceBlue as System.Windows.Media.Brush;
                    break;

            }
        }

        #endregion
    }

    public class dataUploadRecordsColorSetting : IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string statuslf = dr["local_file_num"].ToString();
            string statussf = dr["sended_file_num"].ToString();

            if (!statuslf.Equals(statussf))
            {
                dgr.Background = System.Windows.Media.Brushes.Red;
                dgr.ToolTip = "存在未上传数据";
            }
        }

        #endregion
    }

    public class TimeTaskExcuteColorSetting:  IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string status = dr["task_is_effect"].ToString();

            switch (status)
            {

                case "未生效":
                    dgr.Background = System.Windows.Media.Brushes.Red;
                    dgr.ToolTip = "未生效";
                    break;
                default:
                    dgr.Background = System.Windows.Media.Brushes.AliceBlue as System.Windows.Media.Brush;
                    break;

            }
        }

        #endregion
    }

    public class FaultStatusColorSetting: IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string status = dr["fault_status"].ToString();

            switch (status)
            {
                case "已上报":
                    dgr.Background = System.Windows.Media.Brushes.BurlyWood;
                    dgr.ToolTip = "已上报";
                    break;
                case "解决中":
                    dgr.Background = System.Windows.Media.Brushes.Brown;
                    dgr.ToolTip = "解决中";
                    break;
                case "已解决":
                    dgr.Background = System.Windows.Media.Brushes.SeaGreen;
                    dgr.ToolTip = "已解决";
                    break;
                default:
                    dgr.Background = System.Windows.Media.Brushes.AliceBlue as System.Windows.Media.Brush;
                    break;
            }
        }
        #endregion
    }

    public class StatusColorSetting :IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string status = dr["fault_status"].ToString();

            switch (status)
            {
                case "创建":
                    dgr.Background = System.Windows.Media.Brushes.BurlyWood;
                    dgr.ToolTip = "创建";
                    break;
                case "指派":
                    dgr.Background = System.Windows.Media.Brushes.Brown;
                    dgr.ToolTip = "指派";
                    break;
                case "关闭":
                    dgr.Background = System.Windows.Media.Brushes.SeaGreen;
                    dgr.ToolTip = "关闭";
                    break;
                default:
                    dgr.Background = System.Windows.Media.Brushes.AliceBlue as System.Windows.Media.Brush;
                    break;
            }
        }
        #endregion
    }

    public class StatusLevelColorSetting : IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string status = dr["status_level"].ToString();

            switch (status)
            {
                case "02":
                    dgr.Background = System.Windows.Media.Brushes.Red;
                    dgr.ToolTip = "故障";
                    break;
                case "01":
                    dgr.Background = System.Windows.Media.Brushes.Yellow;
                    dgr.ToolTip = "警告";
                    break;
                default:
                    dgr.Background = System.Windows.Media.Brushes.AliceBlue as System.Windows.Media.Brush;
                    dgr.ToolTip = "正常";
                    break;
            }
        }
        #endregion
    }

    public class AGUpsStatusColorSetting : IDataGridRowColorSetting
    {
        #region IDataGridRowColorSetting 成员

        public void SetCurrentDataGridRow(Microsoft.Windows.Controls.DataGridRow dgr, System.Data.DataRow dr)
        {
            string status = dr["power_status"].ToString();
            string percent = dr["power_percent"].ToString();
            if (status.Equals("01") && percent.ToInt32() <= BuinessRule.GetInstace().GetRunParamByCode().param_value.ToInt32())
            {
                dgr.Background = System.Windows.Media.Brushes.Red;
                dgr.ToolTip = "可发送关机命令";
            }
           
        }
        #endregion
    }
}
