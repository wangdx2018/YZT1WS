using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public class Draft4044ParaAdd
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
        public static int AddPara4044AgmTickBox(string paraType, string version)
        {
            try
            {
                string cmd = string.Format("select t.* from para_4044_agm_tick_box t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4044AgmTickBox info = DBCommon.Instance.GetModelValue<Para4044AgmTickBox>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4044_agm_tick_box");
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
        /// 增加para_4044_agm_tick_rw
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddPara4044AgmTickRw(string paraType, string version)
        {
            try
            {
                string cmd = string.Format("select t.* from para_4044_agm_tick_rw t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4044AgmTickRw info = DBCommon.Instance.GetModelValue<Para4044AgmTickRw>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4044_agm_tick_rw");
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
        /// 增加para_4044_alarm_lamp_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddPara4044AlarmLampData(string paraType, string version)
        {
            try
            {
                string cmd = string.Format("select t.* from para_4044_alarm_lamp_data t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4044AlarmLampData info = DBCommon.Instance.GetModelValue<Para4044AlarmLampData>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4044_alarm_lamp_data");
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

        ///// <summary>
        ///// 增加para_4044_custom_alarm_lamp
        ///// </summary>
        ///// <param name="paraType">参数类型</param>
        ///// <param name="version">版本号</param>
        ///// <returns>成功返回0，否则返回-1</returns>
        //public static int AddPara4044CustomAlarmLamp(string paraType, string version)
        //{
        //    try
        //    {
        //        string cmd = string.Format("select t.* from para_4044_custom_alarm_lamp t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
        //        Para4044CustomAlarmLamp info = DBCommon.Instance.GetModelValue<Para4044CustomAlarmLamp>(cmd);
        //        if (info != null && !string.IsNullOrEmpty(info.para_version))
        //        {
        //            info.para_version = "-1";
        //            info.card_issuer_id = "1"; //edited by wangdx 20111222
        //            int res = DBCommon.Instance.InsertTable(info, "para_4044_custom_alarm_lamp");
        //            if (res != 1)
        //            {
        //                return -1;
        //            }
        //            else
        //            {
        //                return 0;
        //            }

        //        }
        //        else
        //        {
        //            return -1;
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.Log_Error(ex.Message);
        //        return -1;
        //    }
        //}

        /// <summary>
        /// 增加para_4044_main_login
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddPara4044MainLogin(string paraType, string version)
        {
            try
            {
                string cmd = string.Format("select t.* from para_4044_main_login t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4044MainLogin info = DBCommon.Instance.GetModelValue<Para4044MainLogin>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4044_main_login");
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
        /// 增加para_4044_min_tran_query
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddPara4044MinTranQuery(string paraType, string version)
        {
            try
            {
                string cmd = string.Format("select t.* from para_4044_min_tran_query t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4044MinTranQuery info = DBCommon.Instance.GetModelValue<Para4044MinTranQuery>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4044_min_tran_query");
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
        /// 增加para_4044_pass_control_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddPara4044PassControlData(string paraType, string version)
        {
            try
            {
                string cmd = string.Format("select t.* from para_4044_pass_control_data t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                Para4044PassControlData info = DBCommon.Instance.GetModelValue<Para4044PassControlData>(cmd);
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    int res = DBCommon.Instance.InsertTable(info, "para_4044_pass_control_data");
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

