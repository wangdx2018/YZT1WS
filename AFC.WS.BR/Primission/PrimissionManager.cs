using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.Common;
using AFC.WorkStation.DB;


namespace AFC.WS.BR.Primission
{
    /// <summary>
    /// 定义一些关系类的操作
    /// wangdx 20110411 
    /// 增加了修改权限后，在run_params_info 表中设置某个字段
    /// 
    /// wangdx 20110622修改权限后参数UpdateParamInfo变成了1.
    /// </summary>
    public partial class PrimissionManager
    {
        /// <summary>
        /// 通过操作员Id和车站ID得到角色列表
        /// </summary>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="stationId">车站ID</param>
        /// <returns>返回角色列表</returns>
        public List<PrivRoleInfo> GetRoleInfoByOperatorId(string operatorId)
        {
            if (string.IsNullOrEmpty(operatorId))
            {
                WriteLog.Log_Error("operatorId or stationId is null or empty!");
                return null;
            }
            try
            {

                string cmd = string.Format("select aa.* from priv_role_info aa left join priv_operator_role_info bb on aa.role_id=bb.role_id where bb.operator_id='{0}'", operatorId);
                List<PrivRoleInfo> roleList = DBCommon.Instance.GetTModelValue<PrivRoleInfo>(cmd);
                return roleList;
            }
             catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        public List<PrivRoleInfo> GetAllNormalRoleInfos()
        {
            try
            {

                string cmd = string.Format("select * from priv_role_info aa where aa.role_status='00'");
                List<PrivRoleInfo> roleList = DBCommon.Instance.GetTModelValue<PrivRoleInfo>(cmd);
                return roleList;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }

        }

        /// <summary>
        /// 添加操作员和角色关系
        /// </summary>
        /// <param name="operatorId">操作员编码</param>
        /// <param name="roleId">角色Id</param>
        /// <param name="stationId">车站ID</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int AddOperatorToRoleRelaction(string operatorId, string roleId)
        {
            PrivOperatorRoleInfo info = new PrivOperatorRoleInfo();
            info.operator_id= operatorId;
            info.role_id = roleId;
            info.update_date = DateTime.Now.ToString("yyyyMMdd");
            info.update_time = DateTime.Now.ToString("HHmmss");
            info.upd_operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            try
            {
                int res = 0;
                res = DBCommon.Instance.InsertTable(info, "priv_operator_role_info");
                if (res != 1)
                {
                    Util.DataBase.Rollback();
                    return -1;
                }
                else
                {
                    Util.DataBase.Commit();
                    UpdateChangeParams();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                Util.DataBase.Rollback();
                return -1;
            }
        }


        /// <summary>
        /// 删除该操作人员的所有角色
        /// </summary>
        /// <param name="operatorId">操作人员ID</para>
        /// <returns>成功返回0，否则返回-1</returns>
        public int DeleteOperatorToRoleRelaction(string operatorId)
        {
            int res = 0;
            string delSql = string.Format("delete priv_operator_role_info t where t.operator_id='{0}'", operatorId);
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
                    UpdateChangeParams();
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
        /// 为操作员分配车站
        /// </summary>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="currentStationList">车站代码信息列表</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int AdjustOperatorStationPrimission(string operatorId, List<string> currentStationList)
        {
            //001:delete all stationInfo
            //002:add list to opeatorId
           //List<BasiStationInfo> list= BuinessRule.GetInstace().operationManager.GetOperatorStationInfos(operatorId);

           //List<string> strList = list.Select(temp => temp.station_id).ToList();

           Util.DataBase.BeginTransaction();
            
            int res=0;
           try
           {
           
               string cmd = string.Format("delete from priv_operator_location_info t where t.operator_id='{0}'", operatorId);
               Util.DataBase.SqlCommand(out res, cmd);
               if (res != 0)
                   throw new Exception("分配车站失败！");
               for (int i = 0; i < currentStationList.Count; i++)
               {
                   PrivOperatorLocationInfo info = new PrivOperatorLocationInfo();
                   info.location_id = currentStationList[i];
                   info.operator_id = operatorId;
                   info.updating_operator_id = BuinessRule.GetInstace().OperatorId;
                   info.update_date = DateTime.Now.ToString("yyyyMMdd");
                   info.update_time = DateTime.Now.ToString("HHmmss");
                  res=DBCommon.Instance.InsertTable<PrivOperatorLocationInfo>(info, "priv_operator_location_info");
                   if(res!=1)
                       throw new Exception("  分配车站失败！");
               }
               Util.DataBase.Commit();
               UpdateChangeParams();
               return 0;
           }
           catch (Exception ex)
           {
               WriteLog.Log_Error(ex.Message);
               Util.DataBase.Rollback();
               return 0;
           }
           finally
           {
            //   return 0;
           }
        }

        /// <summary>
        /// 为工区分配车站
        /// </summary>
        /// <param name="maintainareaid">工区ID</param>
        /// <param name="currentStationList">车站代码信息列表</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int AdjustMaintainAreaStationPrimission(string maintainareaid, List<string> currentStationList)
        {
            //001:delete all stationInfo
            //002:add list to opeatorId
            //List<BasiStationInfo> list= BuinessRule.GetInstace().operationManager.GetOperatorStationInfos(operatorId);

            //List<string> strList = list.Select(temp => temp.station_id).ToList();

            Util.DataBase.BeginTransaction();

            int res = 0;
            try
            {

                string cmd = string.Format("delete from maintain_area_station_info t where t.maintain_area_id='{0}'", maintainareaid);
                Util.DataBase.SqlCommand(out res, cmd);
                if (res != 0)
                    throw new Exception("分配车站失败！");
                for (int i = 0; i < currentStationList.Count; i++)
                {
                    MaintainAreaStationInfo info = new MaintainAreaStationInfo();
                    info.station_id = currentStationList[i];
                    info.maintain_area_id = maintainareaid;
                    info.update_operator = BuinessRule.GetInstace().OperatorId;
                    info.update_date = DateTime.Now.ToString("yyyyMMdd");
                    info.update_time = DateTime.Now.ToString("HHmmss");
                    res = DBCommon.Instance.InsertTable<MaintainAreaStationInfo>(info, "maintain_area_station_info");
                    if (res != 1)
                        throw new Exception("  分配车站失败！");
                }
                Util.DataBase.Commit();
                UpdateChangeParams();
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                Util.DataBase.Rollback();
                return 0;
            }
            finally
            {
                //   return 0;
            }
        }


        /// <summary>
        /// 为操作员分配设备类型
        /// </summary>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="currentDevTypeList">当前的设备类型</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int AdjustOperatorDevTypePrimission(string operatorId, List<String> currentDevTypeList)
        {
            //List<BasiDevTypeInfo> list = BuinessRule.GetInstace().GetOperatorDevType(operatorId);

            int res = 0;

            Util.DataBase.BeginTransaction();

            try
            {
               Util.DataBase.SqlCommand(out res,string.Format("delete from priv_oper_device_type_info where user_id='{0}'",operatorId));
               if (res != 0)
                   throw new Exception("调整设备类型失败");
               for (int i = 0; i < currentDevTypeList.Count; i++)
               {
                   PrivOperDeviceTypeInfo info = new PrivOperDeviceTypeInfo();
                   info.user_id = operatorId;
                   info.dev_type = currentDevTypeList[i];
                  res= DBCommon.Instance.InsertTable<PrivOperDeviceTypeInfo>(info, "priv_oper_device_type_info");
                  if (res != 1)
                      throw new Exception("分配设备类型失败");
               }
               Util.DataBase.Commit();
               UpdateChangeParams();
               return 0;
            }
            catch (Exception ex)
            {
                Util.DataBase.Rollback();
                return -1;
            }
            finally
            {
                //todo log here
            }
        }

        /// <summary>
        /// 修改basi_run_params_info 表的数据
        /// </summary>
        /// <returns></returns>
        public int UpdateChangeParams()
        {
            string cmd = "update basi_run_param_info t set t.param_value='1' where t.param_code='0101'";
            int res=0;
            Util.DataBase.SqlCommand(out res, cmd);
            res=UpdateParamVersion();
            return res;
        }


        /// <summary>
        /// 更新参数版本信息
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        private int UpdateParamVersion()
        {
            int res = 0;
            ParaVersionInfo paraVerInfo =GetPrimissionDraftParamInfo();
            if (paraVerInfo == null ||
                string.IsNullOrEmpty(paraVerInfo.para_type))
            {
                paraVerInfo.master_para_version ="-1";
                paraVerInfo.para_version = "-1";
                paraVerInfo.occur_date = DateTime.Now.ToString("yyyyMMdd");
                paraVerInfo.occur_time = DateTime.Now.ToString("HHmmss");
                paraVerInfo.active_date = paraVerInfo.occur_date;
                paraVerInfo.active_time = paraVerInfo.occur_time;
                paraVerInfo.update_date = paraVerInfo.occur_date;
                paraVerInfo.update_time = paraVerInfo.occur_time;
                paraVerInfo.para_type = "0203";
                paraVerInfo.master_para_type = "0203";
               res= DBCommon.Instance.InsertTable<ParaVersionInfo>(paraVerInfo, "para_version_info");
               return res == 1 ? 0 : -1;
            }
            else
            {
                paraVerInfo.occur_date = DateTime.Now.ToString("yyyyMMdd");
                paraVerInfo.occur_time = DateTime.Now.ToString("HHmmss");
                paraVerInfo.active_date = paraVerInfo.occur_date;
                paraVerInfo.active_time = paraVerInfo.occur_time;
                paraVerInfo.update_date = paraVerInfo.occur_date;
                paraVerInfo.update_time = paraVerInfo.occur_time;
                res=DBCommon.Instance.UpdateTable<ParaVersionInfo>(paraVerInfo, "para_version_info",
                    new KeyValuePair<string, string>("para_type", "4041"),
                    new KeyValuePair<string, string>("para_version", "-1"),
                    new KeyValuePair<string, string>("para_master_type", "4300"));
                return res == 1 ? 0 : -1;
            }
        }

        private ParaVersionInfo GetPrimissionDraftParamInfo()
        {
            string cmd = string.Format("select * from para_version_info t where t.para_version='-1' and t.para_type='4041'");
            return DBCommon.Instance.GetModelValue<ParaVersionInfo>(cmd);
        }
    

    }
}
