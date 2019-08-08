using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public class Draft4045ParaDel
    {
        /// <summary>
        /// 删除para_version_info
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelParaVersionInfo4045(string paraType, string version)
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
        /// 删除para_4045_bom_main_login
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4045BomMainLogin(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4045_bom_main_login t where t.para_version='{0}'", version);
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
        /// 删除para_4045_bom_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4045BomTickBox(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4045_bom_tick_box t where t.para_version='{0}'", version);
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
        /// 删除para_4045_min_tran_query
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4045MinTranQuery(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4045_min_tran_query t where t.para_version='{0}'", version);
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
        /// 删除para_4045_tick_reader_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4045TickReaderData(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4045_tick_reader_data t where t.para_version='{0}'", version);
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

