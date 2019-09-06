using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    class Draft0206Add
    {
        /// <summary>
        /// 增加参数类型
        /// </summary>
        /// <param name="function_id">功能ID</param>
        /// <param name="function_name">功能名字</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddParaVersion(string paraType, string version)
        {
            string cmd = string.Format("select t.* from para_version_info t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
            ParaVersionInfo info = DBCommon.Instance.GetModelValue<ParaVersionInfo>(cmd);
            info.para_version = version;
            info.para_type = paraType;
            info.master_para_type = ((uint)(AFC.WS.Model.Const.CssFileType_t.CssMT_StationCfs)).ToString("x4");
            info.para_version_type = "00";
            info.para_or_soft_flag = "01";

            info.master_para_version = version;
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            int iVersionNo = BuinessRule.GetInstace().paraManager.GetCurrentParamVersionNo(paraType);
            //PRM.0001.9900. 0001
            info.para_file_name = "PRM." + paraType + "." + "0199" + "." + (iVersionNo+1).ToString("D4");
            try
            {
                int res = DBCommon.Instance.InsertTable(info, "para_version_info");
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
                return -1;
            }
        }
    }
}
