﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Common;
using AFC.WS.Model.DB;

namespace AFC.WS.BR.ParamsManager
{

    /// <summary>
    /// added by wangdx 20111017
    /// 
    /// 添加车站设备信息参数
    /// </summary>
    public class Draft4042Add
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
            info.para_version = "-1";
           info.master_para_type = ((uint)(AFC.WS.Model.Const.CssFileType_t.CssMT_LcEodMasterControl)).ToString("x2");
            info.para_type = paraType;
            info.master_para_version = "-1";
         
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
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
