using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public class Draft4045ParaAdd
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

        /// <summary>
        /// 增加para_4044_agm_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddPara4045BomMainLogin(string paraType, string version)
        {
            try
            {
                string cmd = string.Format("select t.* from para_4045_bom_main_login t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4045BomMainLogin info = DBCommon.Instance.GetModelValue<Para4045BomMainLogin>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4045_bom_main_login");
                    if (res != 1)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return -1;
                }


            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 增加para_4045_bom_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddPara4045BomTickBox(string paraType, string version)
        {
            try
            {
                string cmd = string.Format("select t.* from para_4045_bom_tick_box t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4045BomMainLogin info = DBCommon.Instance.GetModelValue<Para4045BomMainLogin>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4045_bom_tick_box");
                    if (res != 1)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return -1;
                }


            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 增加para_4045_min_tran_query
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddPara4045MinTranQuery(string paraType, string version)
        {
            try
            {
                string cmd = string.Format("select t.* from para_4045_min_tran_query t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4045MinTranQuery info = DBCommon.Instance.GetModelValue<Para4045MinTranQuery>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4045_min_tran_query");
                    if (res != 1)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return -1;
                }


            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 增加para_4045_tick_reader_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddPara4045TickReaderData(string paraType, string version)
        {
            try
            {
                string cmd = string.Format("select t.* from para_4045_tick_reader_data t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4045TickReaderData info = DBCommon.Instance.GetModelValue<Para4045TickReaderData>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4045_tick_reader_data");
                    if (res != 1)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return -1;
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

