using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public class Draft4044ParaUpdate
    {
        /// <summary>
        /// 修改para_4044_agm_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4044AgmTickBox(Para4044AgmTickBox para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4044_agm_tick_box", new KeyValuePair<string, string>("para_version", para.para_version));
                if (res != 1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 修改para_4044_agm_tick_rw
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4044AgmTickRw(Para4044AgmTickRw para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4044_agm_tick_rw", new KeyValuePair<string, string>("para_version", para.para_version));
                if (res != 1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }



        /// <summary>
        /// 修改para_4044_alarm_lamp_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4044AlarmLampData(Para4044AlarmLampData para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4044_alarm_lamp_data", new KeyValuePair<string, string>("para_version", para.para_version));
                if (res != 1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 修改para_4044_custom_alarm_lamp
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4044CustomAlarmLamp(Para4044CustomAlarmLamp para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4044_custom_alarm_lamp", new KeyValuePair<string, string>("para_version", para.para_version));
                if (res != 1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 修改para_4044_main_login
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4044MainLogin(Para4044MainLogin para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4044_main_login", new KeyValuePair<string, string>("para_version", para.para_version));
                if (res != 1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 修改para_4044_min_tran_query
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4044MinTranQuery(Para4044MinTranQuery para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4044_min_tran_query", new KeyValuePair<string, string>("para_version", para.para_version));
                if (res != 1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 修改para_4044_pass_control_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4044PassControlData(Para4044PassControlData para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4044_pass_control_data", new KeyValuePair<string, string>("para_version", para.para_version));
                if (res != 1)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }

    }
 }
