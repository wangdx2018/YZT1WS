using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public  class HandleDraft4045Add
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
            info.para_master_type = ((uint)(AFC.WS.Model.Const.CssFileType_t.CssMT_LcEodMasterControl)).ToString("x2");
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
        public static int AddPara4045BomMainLogin(string paraType)
        {
            try
            {
                
                Para4045BomMainLogin info = new Para4045BomMainLogin();
                info.para_version="-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.login_try_times = "3";
                    info.main_door_unclose_alarm_time = "0";
                    info.no_oper_logout_time = "120";
                    info.pass_input_time = "60";
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
        public static int AddPara4045BomTickBox(string paraType)
        {
            try
            {

                Para4045BomTickBox info = new Para4045BomTickBox();
                 info.para_version = "-1";
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
        public static int AddPara4045MinTranQuery(string paraType)
        {
            try
            {
              
                Para4045MinTranQuery info = new Para4045MinTranQuery();
                 info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.min_query_tran_time = "10";
                    info.min_query_tran_amount = "30";
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
        public static int AddPara4045TickReaderData(string paraType)
        {
            try
            {
               
                Para4045TickReaderData info = new Para4045TickReaderData();
                info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.tick_error_max_amount = "3";
                    info.unfinished_card_wart_time = "5";
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
