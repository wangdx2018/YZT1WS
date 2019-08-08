using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public class Draft4044ParaDel
    {
        
        /// <summary>
        /// 删除para_version_info
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelParaVersionInfo(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_version_info t where  t.para_type ='{0}' and  t.para_version='{1}'", paraType, version);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                return res;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }





        /// <summary>
        /// 删除para_4044_agm_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4044AgmTickBox(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4044_agm_tick_box t where t.para_version='{0}'", version);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                return res;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 删除para_4044_agm_tick_rw
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4044AgmTickRw(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4044_agm_tick_rw t where t.para_version='{0}'", version);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                return res;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }



        /// <summary>
        /// 删除para_4044_alarm_lamp_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4044AlarmLampData(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4044_alarm_lamp_data t where t.para_version='{0}'", version);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                return res;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }




        /// <summary>
        /// 删除para_4044_custom_alarm_lamp
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4044CustomAlarmLamp(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4044_custom_alarm_lamp t where t.para_version='{0}'", version);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                return res;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }



        /// <summary>
        /// 删除para_4044_main_login
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4044MainLogin(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4044_main_login t where t.para_version='{0}'", version);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                return res;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }



        /// <summary>
        /// 删除para_4044_min_tran_query
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4044MinTranQuery(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4044_min_tran_query t where t.para_version='{0}'", version);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                return res;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }



        /// <summary>
        /// 删除para_4044_pass_control_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4044PassControlData(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4044_pass_control_data t where t.para_version='{0}'", version);
            try
            {
                Util.DataBase.SqlCommand(out res, delSql);
                return res;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }
    }
}

