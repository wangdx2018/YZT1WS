using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WorkStation.DB;
using System.Data;
using AFC.WS.UI.Common;
using AFC.WS.BR;
using AFC.WS.BR.CommBuiness;

namespace AFC.WS.BR.Primission
{

    using AFC.WS.Model.Const;
    /// <summary>
    /// 
    /// 修订记录：wangdx 20110315 增了了CheckRelaction验证函数，
    /// 操作员所在车站，操作员所能够操作的设备
    /// wangdx 20110329 增加了如果小于15天进行提示 CheckOperatorValid
    /// </summary>
    public class OperatorManager
    {
        #region 操作员相关业务
        //--->操作员LogIn登录
        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="operatorId">操作员编码</param>
        /// <param name="operatorPwd">操作员密码</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int LogIn(string operatorId, string operatorPwd)
        {
            int res = 0;
            try
            {
                /*CommProcess cmp = BRContext.CreateCommProcess;
                res = cmp.LogIn(operatorId, operatorPwd);*/
                return res;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                string value = res == 0 ? "登录成功" : "登录失败";
             //   Log.InsertOperationLogInfo(out res, OperationLogSubType.Operator_LogIn, Log_Level.Low, res.ToString(), value);
            }
            //todo:Look up relactionShip table


        }

        //--->操作员LogOut,发送报文
        /// <summary>
        /// 操作员登出
        /// </summary>
        /// <param name="operatorId">操作员编码</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int LogOut(string operatorId)
        {
            return 0;
             
        }

        /// <summary>
        /// 取得系统设置密码
        /// </summary>
        /// <returns></returns>
        public string GetSysDefaultPassword()
        {
            return "000000";
        }


        //-->数据库中是否存在当前操作员
        /// <summary>
        ///数据库中是否存在该操作员，该密码为加密之前的密码，从UI层中得到的密码
        /// </summary>
        /// <param name="code">操作员编码</param>
        /// <param name="currentPwd">当前密码</param>
        /// <returns>成功返回true,否则返回false</returns>
        public bool IsExistCurrentOperator(string code)
        {
            PrivOperatorInfo operatorInfo = GetOperatorInfoByOperatorId(code);
            if (operatorInfo != null)
            {
                if (string.IsNullOrEmpty(operatorInfo.operator_id))
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


        public bool CheckPassWord(string operatorId, string password)
        {
            PrivOperatorInfo operatorInfo = GetOperatorInfoByOperatorId(operatorId);
            if (operatorInfo != null)
            {
                if (string.IsNullOrEmpty(operatorInfo.operator_id))
                {
                    return false;
                }
                else
                {
                    if (password == operatorInfo.current_password)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        //--->通过operatorId得到操作员的基本信息
        /// <summary>
        /// 通过operatorId得到操作员的信息
        /// </summary>
        /// <param name="operatorId">操作员编码</param>
        /// <returns>操作员实体对象</returns>
        public PrivOperatorInfo GetOperatorInfoByOperatorId(string operatorId)
        {
            try
            {
                string cmd = string.Format("select * from priv_operator_info where operator_id='{0}'", operatorId);
                PrivOperatorInfo pi = DBCommon.Instance.GetModelValue<PrivOperatorInfo>(cmd);
                return pi;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //-->新增操作员
        /// <summary>
        /// 业务层增加操作员接口
        /// </summary>
        /// <param name="operatorInfo">操作员历史信息实体对象</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int AddNewOperator(PrivOperatorInfo operatorInfo)
        {
            if (operatorInfo == null)
                return -1;
            int res = 0;

            try
            {
                res = DBCommon.Instance.InsertTable(operatorInfo, "priv_operator_info");
                if (res != 1)
                {
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
                return -1;
            }
        }

        //-->修改操作员信息
        /// <summary>
        /// 更新操作员信息
        /// </summary>
        /// <param name="operatorInfo">操作员信息实体对象</param>
        /// <param name="dbo">数据库访问基类</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int UpdateOperatorInfo(PrivOperatorInfo operatorInfo)
        {
            if (operatorInfo == null)
                return -1;
            if (string.IsNullOrEmpty(operatorInfo.operator_id))
            {
                return -1;
            }
            if (operatorInfo.history_password_one == null)
                operatorInfo.history_password_one = operatorInfo.current_password;
            if (operatorInfo.history_password_two == null)
                operatorInfo.history_password_two = operatorInfo.current_password;
            operatorInfo.update_time = DateTime.Now.ToString("HHmmss");
            operatorInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
            operatorInfo.updating_operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            try
            {
                int res = 0;
                res = DBCommon.Instance.UpdateTable(operatorInfo, "priv_operator_info", new KeyValuePair<string, string>("operator_id", operatorInfo.operator_id));
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


        //-->设置操作员状态,-->更新操作员状态，如启用，禁用，密码终止
        /// <summary>
        /// 设置操作员状态
        /// </summary>
        /// <param name="status">状态代码</param>
        /// <param name="opeatorCode">操作员编码</param>
        /// <returns>成功返回0，否则返回-1</returns>
      public int SetOperatorStatus(string status, string operatorId)
        {

            if (string.IsNullOrEmpty(operatorId) || string.IsNullOrEmpty(status))
                return -1;
            PrivOperatorInfo pi = GetOperatorInfoByOperatorId(operatorId);
            if (pi == null)
                return -1;
            if (string.IsNullOrEmpty(pi.operator_id))
            {
                return -1;
            }
            pi.update_date = DateTime.Now.ToString("yyyyMMdd");
            pi.update_time = DateTime.Now.ToString("HHmmss");
            pi.updating_operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
            pi.validity_status = status;
            if (status == AFC.WS.Model.Const.OperatorStatus.ForceChangePwd)
            {
                pi.validity_status = "00";
                pi.history_password_two = pi.history_password_one;
                pi.history_password_one = pi.current_password;
                pi.current_password = "111111";
                pi.new_password = "111111";
            }

            int res = 0;
            try
            {
                res = DBCommon.Instance.UpdateTable(pi, "priv_operator_info", new KeyValuePair<string, string>("operator_id", operatorId));
                if (res != 1)
                {
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
                return -1;
            }
        }


      //-->设置操作员状态,-->更新操作员状态，如启用，禁用，密码终止
      /// <summary>
      /// 设置操作员状态
      /// </summary>
      /// <param name="status">状态代码</param>
      /// <param name="opeatorCode">操作员编码</param>
      /// <returns>成功返回0，否则返回-1</returns>
      public int SetOperatorLock(string lockStatus, string operatorId)
      {

          if (string.IsNullOrEmpty(operatorId) || string.IsNullOrEmpty(lockStatus))
              return -1;
          PrivOperatorInfo pi = GetOperatorInfoByOperatorId(operatorId);
          if (pi == null)
              return -1;
          if (string.IsNullOrEmpty(pi.operator_id))
          {
              return -1;
          }
          pi.update_date = DateTime.Now.ToString("yyyyMMdd");
          pi.update_time = DateTime.Now.ToString("HHmmss");
          pi.updating_operator_id = BuinessRule.GetInstace().brConext.CurrentOperatorId;
          pi.lock_status = lockStatus;

          int res = 0;
          try
          {
              res = DBCommon.Instance.UpdateTable(pi, "priv_operator_info", new KeyValuePair<string, string>("operator_id", operatorId));
              if (res != 1)
              {
                  Util.DataBase.Rollback();
                  return -1;
              }
              else
              {
                  Util.DataBase.Commit();
                  PrimissionManager pm = new PrimissionManager();
                  //BuinessRule.GetInstace().logManager.AddLogInfo(Oper
                  pm.UpdateChangeParams();
                  return 0;
              }
          }
          catch (Exception ex)
          {
              Util.DataBase.Rollback();
              return -1;
          }
      }


        #endregion



        /// <summary>
        ///操作员车站的合法性检查
        /// </summary>
        /// <param name="operatorId">操作员ID</param>
        /// <param name="sationId">车站ID</param>
        /// <returns>有权限返回true，否则返回false</returns>
      public bool CheckStationPrimission(string operatorId, string stationId)
      {
          string cmd=string.Format("select * from priv_operator_location_info t where t.operator_id='{0}' and t.location_id='{1}'",operatorId,stationId);
          PrivOperatorLocationInfo info = DBCommon.Instance.GetModelValue<PrivOperatorLocationInfo>(cmd);
          return info.operator_id != null;
      }

     /// <summary>
     /// 检查操作员是否有该设备的访问权限
     /// </summary>
     /// <param name="operatorId">操作员ID</param>
     /// <param name="devcieType">设备类型</param>
     /// <returns>能够访问该设备类型返回true，否则返回false</returns>
      public bool CheckCanAccessDeviceType(string operatorId, string devcieType)
      {
          if (string.IsNullOrEmpty(operatorId) && string.IsNullOrEmpty(devcieType))
          {
              WriteLog.Log_Error("function params error!");
              return false;
          }
          PrivOperDeviceTypeInfo info = DBCommon.Instance.GetModelValue<PrivOperDeviceTypeInfo>(string.Format("select * from priv_oper_device_type_info t where t.user_id='{0}' and t.dev_type='{1}'", operatorId, devcieType));
          return info.user_id != null;
      }

      /// <summary>
      /// 检查操作员状态
      /// 操作员有效期是否已过期
      /// </summary>
      /// <param name="operatorId">操作员ID</param>
      /// <returns>成功返回0，否则返回错误代码</returns>
      public int CheckOperatorValid(string operatorId,string pwd)
      {
          PrivOperatorInfo info = GetOperatorInfoByOperatorId(operatorId);
          if (info.operator_id==null)
              return -1;// not exist
          //if (info.validity_status != "00")
          //{
          //    return -2;//operator status not valid
          //}

          //if (string.IsNullOrEmpty(info.validity_end_date))
          //{
          //    return -8;
          //}

          //DateTime pwdEndDate = DateTime.ParseExact(info.validity_end_date, "yyyyMMdd", null);

          //if (DateTime.Now.Subtract(pwdEndDate).TotalDays >= 0)
          //{
          //    return -8; //操作员密码已到期
          //}

          //DateTime dtStart = DateTime.ParseExact(info.validity_start_date, "yyyyMMdd", null);

          //if (DateTime.Now.Subtract(dtStart).TotalDays < 0)
          //{
          //    return -9; //操作员未到启用时间
          //}


    
              
          //DateTime dtLast = DateTime.ParseExact(info.validity_end_date, "yyyyMMdd", null);
          //if (DateTime.Now.Subtract(dtLast).TotalDays >= 0)
          //{
          //    return -3;//操作员有效期已到达
          //}


       

          //if (!this.CheckStationPrimission(operatorId, SysConfig.GetSysConfig().LocalParamsConfig.StationCode))
          //{
          //    return -4;//不能在改车站登录
          //}
          //if (!this.CheckCanAccessDeviceType(operatorId, SysConfig.GetSysConfig().LocalParamsConfig.DeviceType))
          //{
          //    return -5;//不能操作该设
          //}
          //if (info.lock_status == "01")
          //{
          //    return -6; //操作员已锁定
          //}

          

          //if (!PassWordEncryptDecrypt.EncrptPassWord(pwd).Equals(info.current_password))
          //{
          //    return -7;//操作员密码错误
          //}

          //if (DateTime.Now.Subtract(dtLast).TotalDays >= -15) //密码即将过期
          //{
          //    return 1;
          //}
          
          return 0;
          
      }

      /// <summary>
      /// 获得Operator的所有车站信息
      /// </summary>
      /// <param name="operatorId">操作员ID</param>
      /// <returns>返回该操作员的所有车站信息</returns>
      public List<BasiStationInfo> GetOperatorStationInfos(string operatorId)
      {
          List<BasiStationInfo> list = DBCommon.Instance.GetTModelValue<BasiStationInfo>(string.Format("select * from basi_station_info t right join (select poli.location_id from priv_operator_info poi left join priv_operator_location_info poli on poi.operator_id =poli.operator_id where poi.operator_id = '{0}') t1 on t1.location_id =t.station_id where t.line_id='{1}'", operatorId,SysConfig.GetSysConfig().LocalParamsConfig.LineCode));
          return list;
      }



      /// <summary>
      /// 获得该操作员的权限信息列表
      /// </summary>
      /// <param name="operatorId">操作员编码</param>
      /// <returns>通过操作员编码找到操作员的权限信息列表</returns>
      public List<string> GetCurrentOperatorFunctionList(string operatorId)
      {
          if (string.IsNullOrEmpty(operatorId))
              return null;
          string cmd = string.Format("select distinct pfi.function_id"+
  " from priv_function_info pfi"+
  "  inner join"+
 "  (select prfi.function_id"+
  "  from priv_role_function_info prfi"+
 "   left join (select pri.*"+
    "            from priv_role_info pri"+
     "           left join priv_operator_role_info pori on pori.role_id ="+
          "                                                pri.role_id"+
         "                                             and pri.role_status = '00'"+
       "        where pori.operator_id = '{0}') t on t.role_id ="+
       "                                                  prfi.role_id) tt"+
"  on pfi.function_id = tt.function_id and pfi.function_status='00'"+
"  where pfi.device_type = '12'", operatorId);
        
          DataTable ds = DBCommon.Instance.GetDatatable(cmd);
       
          if (ds == null)
              return null;
          List<string> list = new List<string>();
          for (int i = 0; i < ds.Rows.Count; i++)
          {
              list.Add(ds.Rows[i]["function_id"].ToString().ToLower());
          }
        
          return list;
      }

    }
}
