using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
  public class HandleDraft4044Add
    {
      /// <summary>
        /// 增加参数类型
        /// </summary>
        /// <param name="function_id">功能ID</param>
        /// <param name="function_name">功能名字</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddParaVersion(string paraType)
        {

            ParaVersionInfo info = new ParaVersionInfo();
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

        /// <summary>
        /// 增加para_4044_agm_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AddPara4044AgmTickBox(string paraType)
        {
            try
            {
               
                Para4044AgmTickBox info = new Para4044AgmTickBox();
                info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.disuse_tick_full_amount = "0";
                    info.disuse_tick_near_full_amount = "0";
                    info.tick_box_empty_amount = "0";
                    info.tick_box_near_empty_amount = "0";
                
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
        public static int AddPara4044AgmTickRw(string paraType)
        {
            try
            {

                Para4044AgmTickRw info = new Para4044AgmTickRw();
                info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.tick_error_max_amount = "3";
                    info.unfinished_card_wart_time = "5";
                   
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
        public static int AddPara4044AlarmLampData(string paraType)
        {
            try
            {

                Para4044AlarmLampData info = new Para4044AlarmLampData();
                info.para_version = "-1";
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
        //public static int AddPara4044CustomAlarmLamp(string paraType)
        //{
        //    try
        //    {

        //        Para4044CustomAlarmLamp info = new Para4044CustomAlarmLamp();
        //        info.para_version="-1";
        //        if (info != null && !string.IsNullOrEmpty(info.para_version))
        //        {
        //            info.para_version = "-1";
        //            info.card_issuer_id = "01";
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
        public static int AddPara4044MainLogin(string paraType)
        {
            try
            {
                
                Para4044MainLogin info = new Para4044MainLogin();
                info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.pass_input_time = "60";
                    info.login_try_times = "3";
                    info.no_oper_logout_time = "120";
                    info.main_door_unclose_alarm_time = "60";
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
        public static int AddPara4044MinTranQuery(string paraType)
        {
            try
            {
                
                Para4044MinTranQuery info = new Para4044MinTranQuery();
                info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.min_query_tran_time = "10";
                    info.min_query_tran_amount = "30";
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
        public static int AddPara4044PassControlData(string paraType)
        {
            try
            {
               
                Para4044PassControlData info = new Para4044PassControlData();
                info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.enter_delay_time = "350";
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
