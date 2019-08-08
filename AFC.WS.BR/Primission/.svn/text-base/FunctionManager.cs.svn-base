using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WorkStation.DB;
using System.Data;
using AFC.WS.UI.Common;
using AFC.WS.BR;

namespace AFC.WS.BR.Primission
{
    public class FunctionManager
    {
        /// <summary>
        /// 通过FunctionId找到相应的Function实体类对象
        /// </summary>
        /// <param name="functionId">系统功能ID</param>
        /// <returns>功能信息实体类</returns>
        public PrivFunctionInfo GetFunctionInfoById(string functionId)
        {
            try
            {
               
                string cmd = string.Format("select * from priv_function_info where function_id='{0}'", functionId);
                PrivFunctionInfo fi = DBCommon.Instance.GetModelValue<PrivFunctionInfo>(cmd);
                return fi;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

            /// <summary>
        /// 通过功能ID来判断数据库中是否存在该功能信息对象
        /// </summary>
        /// <param name="functionId">系统功能ID</param>
        /// <returns>存在返回true，否则返回false</returns>
        public bool IsExistFunction(string functionId)
        {
            if (string.IsNullOrEmpty(functionId))
            {
                WriteLog.Log_Error("roleId is null or empty");
                return false;
            }
  
            try
            {
                PrivFunctionInfo info = GetFunctionInfoById(functionId);
                if (info != null)
                {
                    if (string.IsNullOrEmpty(info.function_id))
                    {
                        return false;
                    }
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 更新系统功能信息
        /// </summary>
        /// <param name="functionId">功能信息ID</param>
        /// <param name="functionName">功能名称</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int UpdateFunctionInfo(string functionId, string functionName)
        {
            if (string.IsNullOrEmpty(functionId) || string.IsNullOrEmpty(functionName))
                return -1;
            PrivFunctionInfo fi = GetFunctionInfoById(functionId);
            if (fi == null)
                return -1;
            fi.update_date = DateTime.Now.ToString("yyyyMMdd");
            fi.update_time = DateTime.Now.ToString("HHmmss");
            fi.updating_operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            fi.function_name = functionName;

            int res = 0;
            try
            {
                res = DBCommon.Instance.UpdateTable(fi, "priv_function_info", new KeyValuePair<string, string>("function_id", functionId));
                if (res != 1)
                {
                   // Util.DataBase.Rollback();
                    return -1;
                }
                else
                {
                    PrimissionManager pm = new PrimissionManager();
                    pm.UpdateChangeParams();
                
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //Util.DataBase.Rollback();
                return -1;
            }

        }
        /// <summary>
        /// 增加功能信息函数
        /// </summary>
        /// <param name="function_id">功能ID</param>
        /// <param name="function_name">功能名字</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int AddFunction(string function_id, string function_name)
        {
            if (string.IsNullOrEmpty(function_id) || string.IsNullOrEmpty(function_name))
                return -1;
            PrivFunctionInfo fi = new PrivFunctionInfo();
            fi.function_id = function_id;
            fi.device_type = fi.function_id.Substring(0, 2);
            fi.function_category = fi.function_id.Substring(2, 4);
            fi.function_sub_category = fi.function_id.Substring(4, 2);
            fi.function_name = function_name;
            fi.update_date = DateTime.Now.ToString("yyyyMMdd");
            fi.update_time = DateTime.Now.ToString("HHmmss");
            fi.updating_operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            fi.function_status = "03";
            int res = 0;
            try
            {
                res = DBCommon.Instance.InsertTable(fi, "priv_function_info");
                if (res != 1)
                {
                   // Util.DataBase.Rollback();
                    return -1;
                }
                else
                {
                    PrimissionManager pm = new PrimissionManager();
                    pm.UpdateChangeParams();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                //Util.DataBase.Rollback();
                return -1;
            }
        }

        /// <summary>
        ///更改功能状态 如删除，禁用，启用等
        /// </summary>
        /// <param name="function_id">功能ID</param>
        /// <param name="status">状态编码 正常：0  禁用：1  已删除：2  未启用：3</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int ChangeFunctionStatus(string function_id, string status)
        {
            if (string.IsNullOrEmpty(function_id))
                return -1;
            PrivFunctionInfo fi = GetFunctionInfoById(function_id);
            if (fi == null)
                return -1;
            fi.function_status = Convert.ToInt16(status).ToString("X2");
            fi.update_date = DateTime.Now.ToString("yyyyMMdd");
            fi.update_time = DateTime.Now.ToString("HHmmss");
            fi.updating_operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            try
            {
                int res = 0;
                res = DBCommon.Instance.UpdateTable(fi, "priv_function_info", new KeyValuePair<string, string>("function_id", function_id));
                if (res != 1)
                {
                   // Util.DataBase.Rollback();
                    return -1;
                }
                PrimissionManager pm = new PrimissionManager();
                pm.UpdateChangeParams();
                return 0;
            }
            catch (Exception ex)
            {
                //Util.DataBase.Rollback();
                WriteLog.Log_Error(ex.Message);
                return -1;
            }

        }

    }
}
