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
    /// <summary>
    /// edit by wangdx 20110715 
    /// 
    /// 修改了GetAllNormalFunctionInfos函数中的SQL
    /// </summary>
   public class RoleManager
    {
        //--->增加一个角色
        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="roleInfo">实体类RoleInfo</param>
        /// <returns>增加成功返回0，否则返回-1</returns>
        public int AddRole(string roleId, string roleName)
        {
            PrivRoleInfo ri = new PrivRoleInfo();
            ri.role_id = roleId;
            ri.role_name = roleName;
            ri.role_status = "03";
            ri.update_date = DateTime.Now.ToString("yyyyMMdd");
            ri.update_time = DateTime.Now.ToString("HHmmss");
            ri.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            try
            {
                int res = 0;
                res = DBCommon.Instance.InsertTable(ri, "priv_role_info");
                if (res != 1)
                {
                 //   Util.DataBase.Rollback();
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
                WriteLog.Log_Error(ex.Message);
                return -1;
            }

        }

        //--->增加一个角色
        /// <summary>
        /// 增加一个角色
        /// </summary>
        /// <param name="ri">角色信息实体类</param>
        /// <param name="dbo">数据库访问对象</param>
        /// <returns>成功返回0，否则返回false</returns>
        public  int AddRole(PrivRoleInfo ri)
        {
            try
            {
                int res = 0;
                ri.update_date = DateTime.Now.ToString("yyyyMMdd");
                ri.update_time = DateTime.Now.ToString("HHmmss");
                ri.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                res = DBCommon.Instance.InsertTable(ri, "priv_role_info");
                if (res != 1)
                {
                  //  Util.DataBase.Rollback();
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
                WriteLog.Log_Error(ex.Message);
               // Util.DataBase.Rollback();
                return -1;
            }

        }

        //--->是否存在该角色信息，应该放到业务层中实现
        /// <summary>
        /// 是否存在该该RoleInfo
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>存在该角色Id，返回true，否则返回false</returns>
        public bool IsExistRole(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                WriteLog.Log_Error("roleId is null or empty");
                return false;
            }
            try
            {
                PrivRoleInfo info = GetRoleInfoById(roleId);
                if (info!=null )
                {
                    if (string.IsNullOrEmpty(info.role_id))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return false;
            }
        }

        //--->修改角色状态信息
        /// <summary>
        /// 改变角色状态，包括 启用，禁用，未启用
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="status">角色状态</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int ChangeRoleStatus(string roleId, string status)
        {
            if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(status))
                return -1;
            PrivRoleInfo ri = GetRoleInfoById(roleId);
            if (ri == null)
                return -1;
            ri.update_date = DateTime.Now.ToString("yyyyMMdd");
            ri.update_time = DateTime.Now.ToString("HHmmss");
            ri.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            ri.role_status = Convert.ToInt16(status).ToString("X2");
            try
            {
                int res = 0;
                res = DBCommon.Instance.UpdateTable(ri, "priv_role_info", new KeyValuePair<string, string>("role_id", roleId));
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
                //return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                //Util.DataBase.Rollback();
                return -1;
            }
        }

        //-->修改角色信息
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="ri">角色功能ID</param>
        /// <returns>成功返回0，否则返回-1</returns>
        private int UpdateRoleInfo(PrivRoleInfo ri)
        {
            if (ri == null)
                return -1;

            try
            {
                int res = 0;
                ri.update_date = DateTime.Now.ToString("yyyyMMdd");
                ri.update_time = DateTime.Now.ToString("HHmmss");
                ri.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
                res = DBCommon.Instance.UpdateTable(ri, "priv_role_info", new KeyValuePair<string, string>("role_id", ri.role_id));
                if (res != 1)
                {
                    Util.DataBase.Rollback();
                    return -1;
                }
                else
                {
                    Util.DataBase.Commit();
                    PrimissionManager pm = new PrimissionManager();
                    pm.UpdateChangeParams();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Util.DataBase.Rollback();
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }

        //--->修改角色信息,只能修改角色名称
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="roleName">角色名称</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int UpdateRoleInfo(string roleId, string roleName)
        {
            if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(roleName))
                return -1;
            PrivRoleInfo ri = GetRoleInfoById(roleId);
            if (ri == null)
                return -1;
            ri.role_name = roleName;
            ri.update_date = DateTime.Now.ToString("yyyyMMdd");
            ri.update_time = DateTime.Now.ToString("HHmmss");
            ri.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            try
            {
                int res = 0;
                res = DBCommon.Instance.UpdateTable(ri, "priv_role_info", new KeyValuePair<string, string>("role_id", roleId));
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
                WriteLog.Log_Error(ex.Message);
               // Util.DataBase.Rollback();
                return -1;
            }
        }

        //-->通过RoleId找到Role信息实体
        /// <summary>
        /// 通过RoleId得到角色信息
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>成功RoleInfo，否则null</returns>
        public PrivRoleInfo GetRoleInfoById(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                WriteLog.Log_Error("roleId is null or empty");
                return null;
            }
            PrivFunctionInfo ri = new PrivFunctionInfo();
            try
            {
                string cmd = string.Format("select * from priv_role_info t where t.role_id='{0}'", roleId);
                PrivRoleInfo fi = DBCommon.Instance.GetModelValue<PrivRoleInfo>(cmd);
                return fi;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        public List<PrivFunctionInfo> getFuncByRoleID(string roleId)
        {

            string cmd = string.Format("select * from priv_function_info aa  join "
                + "(select * from priv_role_function_info) bb on aa.function_id=bb.function_id "
                + "where (aa.function_status = '0' or aa.function_status='00') and bb.role_id='{0}' order by aa.function_id", roleId);
            try
            {
                List<PrivFunctionInfo> funcList = DBCommon.Instance.GetTModelValue<PrivFunctionInfo>(cmd);
                return funcList;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return null;
            }

        }


        /// <summary>
        /// 得到该角色所有的功能信息
        /// </summary>
        /// <returns>返回标志位正常的功能信息列表</returns>
        public List<PrivFunctionInfo> GetAllNormalFunctionInfos(string roleId)
        {
           
            int res = 0;
            string cmd = string.Format("select * "
 + "from priv_function_info tf "
 + "left join (select t.*, case "
 + "                    when t.role_id > 0 and t.role_id <=1000 then '21'"
 + "                    when t.role_id >4000 and t.role_id <= 5000 then '06'"
 + "                    when t.role_id >6000 and t.role_id <=7000 then '07'"
 + "                    when t.role_id >8000 and t.role_id <=9000 then '09'"
 + "                    when t.role_id >10000 and t.role_id<=11000 then '0C' end  a"
 + " from priv_role_info t)d on d.a = tf.device_type where d.role_id='{0}'", roleId);
                       
            try
            {
                List<PrivFunctionInfo> funcList = DBCommon.Instance.GetTModelValue<PrivFunctionInfo>(cmd);
                return funcList;
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 删除该角色信息的所有功能
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="functionId">功能ID</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int DeleteRoleToFunctionRelaction(string roleId)
        {
            int res = 0;
            string delSql = string.Format("delete from priv_role_function_info  where role_id='{0}'", roleId);
              try
            {
               Util.DataBase.SqlCommand(out res, delSql);
               if (res == 0)
               {
                   return -1;
               }
               else
               {
                   Util.DataBase.Commit();
                   PrimissionManager pm = new PrimissionManager();
                   pm.UpdateChangeParams();
                   return 0;
               }
            }
            catch (Exception ex)
            {
                AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
                Util.DataBase.Rollback();
                return -1;
            }
        }


       /// <summary>
        /// 增加该角色的功能
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="functionId">功能ID</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int AddRoleToFunctionRelaction(string roleId, string functionId)
        {
            if (string.IsNullOrEmpty(roleId) || string.IsNullOrEmpty(functionId))
                return -1;

            PrivRoleFunctionInfo info = new PrivRoleFunctionInfo();
            info.function_id = functionId;
            info.role_id = roleId;
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            try
            {
                int res = 0;
                res = DBCommon.Instance.InsertTable(info, "priv_role_function_info");
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
                WriteLog.Log_Error(ex.Message);
             //   Util.DataBase.Rollback();
                return -1;
            }
        }

    }
}
