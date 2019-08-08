using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public class HandleDraft4043Add
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
        /// 增加para_4043_maintain_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int addPara4043MaintainData(string paraType)
        {
            try
            {


                Para4043MaintainData info = new Para4043MaintainData();
                info.para_version = "-1";

                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.pass_enter_time = "60";
                    info.login_try_time = "3";
                    info.no_oper_logout_time = "120";
                    info.main_door_unclose_alrm_time = "60";
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
        public static int addPara4043MinQueryTranAmoun(string paraType)
        {
            try
            {


                Para4043MinQueryTranAmoun info = new Para4043MinQueryTranAmoun();
                info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.min_query_tran_amount ="30";
                    info.min_query_tran_time = "10";
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
        public static int addPara4043TvmCashBox(string paraType)
        {
            try
            {


                Para4043TvmCashBox info = new Para4043TvmCashBox();
                info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.sell_paper_amount_type = "0";
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
        public static int addPara4043TvmTickBox(string paraType)
        {
            try
            {


                Para4043TvmTickBox info = new Para4043TvmTickBox();
                info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
            
                    info.tick_box_empty_amount = "0";
                    info.tick_box_near_empty_amount = "0";
                    
              
              
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
        public static int addPara4043TvmTickRead(string paraType)
        {
            try
            {


                Para4043TvmTickRead info = new Para4043TvmTickRead();
                info.para_version = "-1";
                if (info != null && !string.IsNullOrEmpty(info.para_version))
                {
                    info.para_version = "-1";
                    info.tick_error_max_num = "3";
                    info.unfinishied_retry_times="0";
                    info.unfinished_card_wait_time="5";

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

