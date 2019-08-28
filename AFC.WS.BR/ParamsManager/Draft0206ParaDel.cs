using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    class Draft0206ParaDel
    {

        /// <summary>
        /// 删除para_version_info
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int DelParaVersionInfo0206(string paraType, string version)
        {
            int res = 0;
            string delSql = string.Format("delete from para_version_info where  para_type ='{0}' and  para_version='{1}'", paraType, version);
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


        public static int DelDevInfoConfig(string paraType, string version)
        {
            //int res = 0;
            //string delSql = string.Format("delete from para_0206_station_cfg_ctrl t where t.para_version='00000'", paraType, version);
            //try
            //{
            //    Util.DataBase.SqlCommand(out res, delSql);
            //    return res;
            //}
            //catch (Exception ex)
            //{
            //    AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
            //    return -1;
            //}
            return 0;
        }
    }
}
