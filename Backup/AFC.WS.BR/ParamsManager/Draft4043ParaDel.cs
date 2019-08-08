using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public class Draft4043ParaDel
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
        /// 删除para_4043_maintain_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4043MaintainData(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4043_maintain_data t where t.para_version='{0}'", version);
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
        /// 删除para_4043_min_query_tran_amoun
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4043MinQueryTranAmoun(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4043_min_query_tran_amoun t where t.para_version='{0}'", version);
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
        /// 删除para_4043_tvm_cash_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4043TvmCashBox(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4043_tvm_cash_box t where t.para_version='{0}'", version);
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
        /// 删除para_4043_tvm_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4043TvmTickBox(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4043_tvm_tick_box t where t.para_version='{0}'", version);
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
        /// 删除para_4043_tvm_tick_read
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelPara4043TvmTickRead(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete para_4043_tvm_tick_read t where t.para_version='{0}'", version);
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
