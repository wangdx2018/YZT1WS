using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.ParamsManager
{
    public class Draft4043ParaUpdate
    {
        /// <summary>
        /// 修改para_4043_maintain_data
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4043MaintainData(Para4043MaintainData maintainData)
        {
            try
            {
                if (maintainData == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(maintainData, "para_4043_maintain_data", new KeyValuePair<string, string>("para_version", maintainData.para_version));
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
        /// 修改para_4043_min_query_tran_amoun
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4043MinQueryTranAmoun(Para4043MinQueryTranAmoun para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4043_min_query_tran_amoun", new KeyValuePair<string, string>("para_version", para.para_version));
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
        /// 修改para_4043_tvm_cash_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4043TvmCashBox(Para4043TvmCashBox para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4043_tvm_cash_box", new KeyValuePair<string, string>("para_version", para.para_version));
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
        /// 修改para_4043_tvm_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4043TvmTickBox(Para4043TvmTickBox para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4043_tvm_tick_box", new KeyValuePair<string, string>("para_version", para.para_version));
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
        /// 修改para_4043_tvm_tick_box
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updatePara4043TvmTickRead(Para4043TvmTickRead para)
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, "para_4043_tvm_tick_read", new KeyValuePair<string, string>("para_version", para.para_version));
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
