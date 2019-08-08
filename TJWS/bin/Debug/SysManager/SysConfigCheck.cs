using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AFC.WS.UI.Common;
using AFC.WorkStation.DB;

namespace AFC.WS.UI.BR.Data.SysManager
{
    public  class SysConfigCheck
    {
        public int validDevice(string connstring, string DeviceCode, string LocalIPaddress)
        {
            //1代表查询有数；0代表查询无；-1代表数据库连接失败
            int isExist = -1;
            DBO dbo =null;
            try
            {
                //获取消息中文名称
                string SQL_DeviceInfo = "select * from  basi_dev_info t where t.device_id='" + DeviceCode + "' and t.device_ip ='" + LocalIPaddress + "'";
                
                DataTable dt = null;
                int retCode = 0;
                dbo = new DBO(connstring);
                if (dbo != null)
                {
                    DataSet ds = dbo.SqlQuery(out retCode, SQL_DeviceInfo);
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            isExist = 1;
                        }
                        else
                        {
                            isExist = 0;
                        }

                    }
                    else
                    {
                        if (retCode == -206)
                        {
                            isExist = -1;
                        }
                        else
                        {
                            isExist = 0;
                        }
                       
                    }
                }
                return isExist;

            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
                return -1;
            }
            finally
            {
                if (dbo != null)
                {
                    dbo.SqlClose();
                }
            }

        }

        public string getDBIP(string connstring)
        {
            string ipStr = "";
            DBO dbo = null;
            string  SQL_GetIP = "select utl_inaddr.get_host_address as ipaddress from dual";
            try
            {
                DataTable dt = null;
                int retCode = 0;
                dbo = new DBO(connstring);
                if (dbo != null)
                {
                    DataSet ds = dbo.SqlQuery(out retCode, SQL_GetIP);
                     if (ds.Tables.Count > 0)
                     {
                         dt = ds.Tables[0];
                         if (dt != null && dt.Rows.Count > 0)
                         {
                             ipStr = dt.Rows[0]["ipaddress"].ToString();
                         }
                     }
                }

                return ipStr;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.ToString());
                return ipStr;
            }

            finally
            {
                if (dbo != null)
                {
                    dbo.SqlClose();
                }
            }

        }
    }
}
