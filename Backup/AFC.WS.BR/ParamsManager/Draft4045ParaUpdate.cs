using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public class Draft4045ParaUpdate
    {
        
        /// <summary>
        /// 修改para_4045_bom_main_login
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4045BomMainLogin(Para4045BomMainLogin para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4045_bom_main_login", new KeyValuePair<string, string>("para_version", para.para_version));
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
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 修改para_4045_bom_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4045BomTickBox(Para4045BomTickBox para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4045_bom_tick_box", new KeyValuePair<string, string>("para_version", para.para_version));
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
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 修改para_4045_min_tran_query
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4045MinTranQuery(Para4045MinTranQuery para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4045_min_tran_query", new KeyValuePair<string, string>("para_version", para.para_version));
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
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }


        /// <summary>
        /// 修改para_4045_tick_reader_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4045TickReaderData(Para4045TickReaderData para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4045_tick_reader_data", new KeyValuePair<string, string>("para_version", para.para_version));
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
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }



    }
 }
