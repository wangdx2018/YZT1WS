using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public class Draft4043ParaAdd
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
            info.master_para_type = AFC.WS.Model.Const.CssFileType_t.CssMT_LcEodMasterControl.ToString("x2");
        
         
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
        /// 增加para_4043_maintain_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int addPara4043MaintainData(string paraType, string version)
        {
            try
            {

                string cmd = string.Format("select t.* from para_4043_maintain_data t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4043MaintainData info = DBCommon.Instance.GetModelValue<Para4043MaintainData>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4043_maintain_data");
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
        /// 增加para_4043_min_query_tran_amoun
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int addPara4043MinQueryTranAmoun(string paraType, string version)
        {
            try
            {

                string cmd = string.Format("select t.* from para_4043_min_query_tran_amoun t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4043MinQueryTranAmoun info = DBCommon.Instance.GetModelValue<Para4043MinQueryTranAmoun>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4043_min_query_tran_amoun");
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
        /// 增加para_4043_tvm_cash_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int addPara4043TvmCashBox(string paraType, string version)
        {
            try
            {

                string cmd = string.Format("select t.* from para_4043_tvm_cash_box t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4043TvmCashBox info = DBCommon.Instance.GetModelValue<Para4043TvmCashBox>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4043_tvm_cash_box");
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
        /// 增加para_4043_tvm_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int addPara4043TvmTickBox(string paraType, string version)
        {
            try
            {

                string cmd = string.Format("select t.* from para_4043_tvm_tick_box t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4043TvmTickBox info = DBCommon.Instance.GetModelValue<Para4043TvmTickBox>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4043_tvm_tick_box");
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
        /// 增加para_4043_tvm_tick_read
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int addPara4043TvmTickRead(string paraType, string version)
        {
            try
            {

                string cmd = string.Format("select t.* from para_4043_tvm_tick_read t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4043TvmTickRead info = DBCommon.Instance.GetModelValue<Para4043TvmTickRead>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4043_tvm_tick_read");
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



