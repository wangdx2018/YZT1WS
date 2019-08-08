using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WorkStation.DB;
using System.Data;
using AFC.WS.UI.Common;
using AFC.WS.BR;
using System.Reflection;
using AFC.WS.BR.CommBuiness;
using System.Xml;

namespace AFC.WS.BR.ParamsManager
{
    public class ParaManager
    {
        /// <summary>
        /// 获取当前正式版参数信息
        /// </summary>
        string SQL_PARA_VERSION_SYN_INFO = "select t.para_type,t.edition_type,t.para_version,t.para_sub_type,t.para_file_name,t.active_date,t.active_time from para_local_full_ver_info t where t.edition_type='0' and to_date(t.active_date,'yyyy-MM-dd') <= sysdate ";
        /// <summary>
        /// 通过paraType找到相应的草稿版
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <returns>参数版本实体类</returns>
        public ParaVersionInfo GetParaVersionInfo(string paraType)
        {
            try
            {

                string cmd = string.Format("select t.* from para_version_info t where t.para_version='-1' and t.para_type='{0}'", paraType);
                ParaVersionInfo fi = DBCommon.Instance.GetModelValue<ParaVersionInfo>(cmd);
                return fi;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 通过paraType找到相应的版本
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <returns>参数版本实体类</returns>
        public ParaVersionInfo GetCurrentParaVersionInfo(string paraType)
        {
            try
            {

                string cmd = string.Format("select t.* from para_version_info t where t.para_version!=-1 and t.para_type='{0}'", paraType);
                ParaVersionInfo fi = DBCommon.Instance.GetModelValue<ParaVersionInfo>(cmd);
                return fi;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 通过参数类型来判断数据库中是否存在该功能信息对象
        /// </summary>
        /// <param name="functionId">参数类型</param>
        /// <returns>存在返回true，否则返回false</returns>
        public bool IsExistParaInfo(string paraType)
        {
            if (string.IsNullOrEmpty(paraType))
            {
                WriteLog.Log_Error("paraType is null or empty");
                return false;
            }

            try
            {
                ParaVersionInfo info = GetParaVersionInfo(paraType);
                if (info != null)
                {
                    if (string.IsNullOrEmpty(info.para_type))
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
        /// 通过参数类型和版本来判断数据库中是否存在该功能信息对象
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">参数版本</param>
        /// <returns>存在返回true，否则返回false</returns>
        public ParaVersionInfo GetParaByVersion(string paraType, string version)
        {
            try
            {

                string cmd = string.Format("select t.* from para_version_info t where t.para_type= '{0}' and t.para_version='{1}'", paraType, version);
                ParaVersionInfo fi = DBCommon.Instance.GetModelValue<ParaVersionInfo>(cmd);
                return fi;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// 增加4043参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int add4043DraftPara(ParaVersionInfo info)
        {
            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new Draft4043ParaAdd().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static| BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[2] { paraType, paraVersion }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;

        }


        /// <summary>
        /// 手动增加4043参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int addHandle4043DraftPara(ParaVersionInfo info)
        {
            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new HandleDraft4043Add().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static| BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[1] { paraType }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;

        }



        /// <summary>
        /// 手动增加4044参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int addHandle4044DraftPara(ParaVersionInfo info)
        {
            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new HandleDraft4044Add().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[1] { paraType }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;

        }



        /// <summary>
        /// 手动增加4045参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int addHandle4045DraftPara(ParaVersionInfo info)
        {
            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new HandleDraft4045Add().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[1] { paraType }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;

        }
        /// <summary>
        ///  增加4044参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int add4044DraftPara(ParaVersionInfo info)
        {
            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new Draft4044ParaAdd().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static| BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[2] { paraType, paraVersion }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;

        }


        /// <summary>
        ///  增加4045参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int add4045DraftPara(ParaVersionInfo info)
        {
            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new Draft4045ParaAdd().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static| BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[2] { paraType, paraVersion }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;

        }


        public int add4042DraftPara(ParaVersionInfo info)
        {
            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new Draft4042Add().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[2] { paraType, paraVersion }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;
        }

        public int add4314DraftPara(ParaVersionInfo info)
        {
            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new Draft4314Add().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[2] { paraType, paraVersion }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;
        }


        /// <summary>
        /// 删除4043参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int del4043DraftPara(ParaVersionInfo info)
        {

            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new Draft4043ParaDel().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static| BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[2] { paraType, paraVersion }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;
        }


        /// <summary>
        /// 删除4044参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int del4044DraftPara(ParaVersionInfo info)
        {

            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new Draft4044ParaDel().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static| BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[2] { paraType, paraVersion }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;
        }


        /// <summary>
        /// 删除4045参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int del4045DraftPara(ParaVersionInfo info)
        {

            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new Draft4045ParaDel().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static| BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[2] { paraType, paraVersion }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;
        }


        /// <summary>
        /// 删除4314参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int del4314DraftPara(ParaVersionInfo info)
        {

            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new Draft4314ParaDel().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[2] { paraType, paraVersion }).ToString();
                if (result != "0")
                {
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;
        }

        /// <summary>
        /// 删除4042参数类型草稿版
        /// </summary>
        /// <param name="info">参数版本实体类</param>
        /// <returns></returns>
        public int del4042DraftPara(ParaVersionInfo info)
        {

            string paraType = info.para_type;
            string paraVersion = info.para_version.ToString();
            Util.DataBase.BeginTransaction();
            Type tp = new Draft4042ParaDel().GetType();
            MethodInfo[] methodInfo = tp.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            for (int i = 0; i < methodInfo.Length; i++)
            {
                string result = methodInfo[i].Invoke(null, new string[2] { paraType, paraVersion }).ToString();
                if (result != "0")
                {
                    Util.DataBase.Rollback();
                    return -1;
                }

            }
            Util.DataBase.Commit();
            return 0;
        }

        /// <summary>
        /// 修改参数表
        /// </summary>
        /// <param name="paraType">参数类型</param>
        /// <param name="version">版本号</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int updateParaVersion<T>(T para,string tableName,string paraVersion) where T : class, new()
        {
            try
            {
                if (para == null)
                {
                    return -1;
                }
                int res = 0;
                res = DBCommon.Instance.UpdateTable(para, tableName, new KeyValuePair<string, string>("para_version", paraVersion));
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
        /// 更新参数类型草稿版
        /// </summary>
        /// <param name="para">参数版本实体类</param>
        /// <returns></returns>
        public int updateDraftPara(string tableName, object para)
        {
            try
            {
              

                int res = 0;
                string paraVersion = "";

                switch (tableName)
                {
                    case "para_4043_maintain_data":
                        Para4043MaintainData data = para as Para4043MaintainData;
                        paraVersion = data.para_version;
                        res = updateParaVersion(data,tableName,paraVersion);
                        break;
                    case "para_4043_min_query_tran_amoun":
                        Para4043MinQueryTranAmoun Amoun = para as Para4043MinQueryTranAmoun;
                        paraVersion = Amoun.para_version;
                        res = updateParaVersion(Amoun, tableName, paraVersion);
                        break;
                    case "para_4043_tvm_cash_box":
                        Para4043TvmCashBox Box = para as Para4043TvmCashBox;
                        paraVersion = Box.para_version;
                        res = updateParaVersion(Box, tableName, paraVersion);
                        break;
                    case "para_4043_tvm_tick_box":
                        Para4043TvmTickBox TickBox = para as Para4043TvmTickBox;
                        paraVersion = TickBox.para_version;
                        res = updateParaVersion(TickBox, tableName, paraVersion);
                        break;
                    case "para_4043_tvm_tick_read":
                        Para4043TvmTickRead TickRead = para as Para4043TvmTickRead;
                        paraVersion = TickRead.para_version;
                        res = updateParaVersion(TickRead, tableName, paraVersion);
                        break;
                    case "para_4044_agm_tick_box":
                        Para4044AgmTickBox AgmTickBox = para as Para4044AgmTickBox;
                        paraVersion = AgmTickBox.para_version;
                        res = updateParaVersion(AgmTickBox, tableName, paraVersion);
                        break;
                    case "para_4044_agm_tick_rw":
                        Para4044AgmTickRw TickRw = para as Para4044AgmTickRw;
                        paraVersion = TickRw.para_version;
                        res = updateParaVersion(TickRw, tableName, paraVersion);
                        break;
                    case "para_4044_alarm_lamp_data":
                        Para4044AlarmLampData LampData = para as Para4044AlarmLampData;
                        paraVersion = LampData.para_version;
                        res = updateParaVersion(LampData, tableName, paraVersion);
                        break;
                    case "para_4044_custom_alarm_lamp":
                        Para4044CustomAlarmLamp AlarmLamp = para as Para4044CustomAlarmLamp;
                        paraVersion = AlarmLamp.para_version;
                        res = updateParaVersion(AlarmLamp, tableName, paraVersion);
                        break;
                    case "para_4044_main_login":
                        Para4044MainLogin Login = para as Para4044MainLogin;
                        paraVersion = Login.para_version;
                        res = updateParaVersion(Login, tableName, paraVersion);
                        break;
                    case "para_4044_min_tran_query":
                        Para4044MinTranQuery Query = para as Para4044MinTranQuery;
                        paraVersion = Query.para_version;
                        res = updateParaVersion(Query, tableName, paraVersion);
                        break;
                    case "para_4044_pass_control_data":
                        Para4044PassControlData ControlData = para as Para4044PassControlData;
                        paraVersion = ControlData.para_version;
                        res = updateParaVersion(ControlData, tableName, paraVersion);
                        break;

                    case "para_4045_bom_main_login":
                        Para4045BomMainLogin MainLogin = para as Para4045BomMainLogin;
                        paraVersion = MainLogin.para_version;
                        res = updateParaVersion(MainLogin, tableName, paraVersion);
                        break;
                    case "para_4045_bom_tick_box":
                        Para4045BomTickBox Box4045 = para as Para4045BomTickBox;
                        paraVersion = Box4045.para_version;
                        res = updateParaVersion(Box4045, tableName, paraVersion);
                        break;
                    case "para_4045_min_tran_query":
                        Para4045MinTranQuery Query4045 = para as Para4045MinTranQuery;
                        paraVersion = Query4045.para_version;
                        res = updateParaVersion(Query4045, tableName, paraVersion);
                        break;
                    case "para_4045_tick_reader_data":
                        Para4045TickReaderData ReaderData4045 = para as Para4045TickReaderData;
                        paraVersion = ReaderData4045.para_version;
                        res = updateParaVersion(ReaderData4045, tableName, paraVersion);
                        break;
                    default:
                        break;
                }

                if (res != 0)
                {
                    return -1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }



        private List<ParamConfig> paramConfigList = new List<ParamConfig>();

        public int LoadParamsConfigFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            try
            {
                XmlNodeList xnl = doc.GetElementsByTagName("ParamConfig");//get all paramConfig
                if (xnl != null && xnl.Count != 0)
                {
                    for (int i = 0; i < xnl.Count; i++)
                    {
                        ParamConfig config = new ParamConfig();
                        Type t = config.GetType();
                        FieldInfo[] fArray = t.GetFields();
                        for (int j = 0; j < fArray.Length; j++)
                        {
                            if (fArray[j].Name != "ItemList")
                            {
                                fArray[j].SetValue(config, xnl[i].Attributes[fArray[j].Name].Value);
                            }
                        } //set parents data

                        XmlNodeList itemList = xnl[i].ChildNodes;
                        if (itemList != null && itemList.Count != 0)
                        {
                            List<ParamItem> list = new List<ParamItem>();
                            for (int index = 0; index < itemList.Count; index++)
                            {
                                ParamItem item = new ParamItem();
                                Type t1 = item.GetType();
                                FieldInfo[] ifArray = t1.GetFields();
                                for (int jj = 0; jj < ifArray.Length; jj++)
                                {
                                    ifArray[jj].SetValue(item, Util.ParseFieldValue(ifArray[jj], itemList[index].Attributes[ifArray[jj].Name].Value));
                                }
                                list.Add(item);
                            }
                            config.ItemList = list;
                        }
                        this.paramConfigList.Add(config);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }

        }

        public ParamConfig GetParamConfig(string paramType)
        {

            return paramConfigList.Single(temp => temp.Type.Equals(paramType));
        }

        /// <summary>
        /// 获取当前正式参数版本信息
        /// </summary>
        /// <returns>List<ParaVersionSynInfo></returns>
       public List<ParaLocalFullVerInfo> GetParaVersionSynInfo(out DataSet ds)
        {
            try
            {
                //string Programs = "";
                string inStr = "";
                string[] paraTypeArr=new string[]{};
                if ("LCWS" == SysConfig.GetSysConfig().LocalParamsConfig.SystemName)
                    inStr = "4311";
                if ("SCWS" == SysConfig.GetSysConfig().LocalParamsConfig.SystemName)
                    inStr = "4310";
                string ftpParaType = System.Configuration.ConfigurationSettings.AppSettings["FTPDownLoadParamType"].ToString();
              
                if(!string.IsNullOrEmpty(ftpParaType))
                {
                    paraTypeArr = ftpParaType.Split(',');
                }
                int retCode = 0;
                for (int i = 0; i < paraTypeArr.Length; i++)
                {
                    //if (i == 0) //edit by wangdx
                    //{
                    //    inStr = inStr + paraTypeArr[i];
                    //}
                    //else
                    //{
                    //    inStr = inStr + "," + paraTypeArr[i];
                    //}
                    inStr = inStr + "," + paraTypeArr[i];
                }
                SQL_PARA_VERSION_SYN_INFO = SQL_PARA_VERSION_SYN_INFO + "and t.para_type in(" + inStr + ")";
                DataSet dataset = Util.DataBase.SqlQuery(out retCode, SQL_PARA_VERSION_SYN_INFO);
                ds = dataset;

                List<ParaLocalFullVerInfo> paraVersionSynInfoItem = DBCommon.Instance.SetTModelValue<ParaLocalFullVerInfo>(ds.Tables[0]);
                if (paraVersionSynInfoItem != null)
                    return paraVersionSynInfoItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }


       public int GetCurrentParamVersionNo(string paramType)
       {
           if (string.IsNullOrEmpty(paramType))
           {
               WriteLog.Log_Error("paramType is null or empty!");
               return -2;
           }
           string cmd = string.Format("select t.para_type, max(t.para_version) from para_version_info t where t.para_type='{0}' and t.para_version<>'-1' group by t.para_type order by t.para_type", paramType);

         DataTable dt=DBCommon.Instance.GetDatatable(cmd);
         try
         {
             return Convert.ToInt32(dt.Rows[0][1]);
         }
         catch (Exception ex)
         {
             return -1;
         }
       }


        /// <summary>
        /// 更新最大版本号
        /// </summary>
        /// <param name="parmType">参数类型</param>
        /// <param name="currentNo">当前版本号</param>
        /// <returns>更新成功返回true，否则返回false</returns>
       public bool UpdateMaxParamVersionNo(string paramType,uint currentNo)
       {
           //todo:get max and query

           uint maxNo = (uint)this.GetCurrentParamVersionNo(paramType);
           if (maxNo == uint.MaxValue)
           {
               ParaVersionInfo pvi = new ParaVersionInfo();
               pvi.para_type = paramType;
               pvi.para_master_type = "4300";
               pvi.para_sub_type = "0";
               pvi.para_version = "-1";
               pvi.update_date = DateTime.Now.ToString("yyyyMMdd");
               pvi.update_time = DateTime.Now.ToString("HHmmss");
               pvi.active_date = DateTime.Now.ToString("yyyyMMdd");
               pvi.active_time = DateTime.Now.ToString("HHmmss");
               pvi.master_para_version = "-1";
               pvi.para_version = (currentNo-1).ToString();//更新为版本号小与1的数据
               pvi.occur_time = pvi.update_time;
               pvi.occur_date = pvi.update_date;
               pvi.para_file_name = string.Empty;
               return DBCommon.Instance.InsertTable<ParaVersionInfo>(pvi, "para_version_info") > 0;
             
           }
           string cmd = string.Format("update para_version_info t set t.para_version='{0}' where t.para_type='{1}' and t.para_version='{2}'",
              (currentNo - 1).ToString("d4"), paramType, maxNo.ToString("d4"));
           int res=0;
           return AFC.WS.UI.Common.Util.DataBase.SqlCommand(out res, cmd) > 0;
        
       }


       public bool IsExistNewVersion(out string verNo, out string effectDate)
       {
           verNo = string.Empty;
           effectDate = string.Empty;
           ParaLocalFullVerInfo versionInfo = BuinessRule.GetInstace().GetFutureVersionPara();
           if (versionInfo != null && !string.IsNullOrEmpty(versionInfo.para_version))
           {
               verNo = versionInfo.para_version;
               effectDate = versionInfo.active_date;
               return true;
           }
           else
           {
               verNo = string.Empty;
               effectDate = string.Empty;
               return false;
           }
       }
        /// <summary>
        /// 删除参数版本主表
        /// </summary>
        /// <param name="paraType"></param>
        /// <param name="version"></param>
        /// <returns></returns>
       public int DelParaVersionInfo(string paraType, string version)
       {
           int res = 0;
           string delSql = string.Format("delete para_version_info t where  t.para_type ='{0}' and  t.para_version='{1}'", paraType, version);
           try
           {
               Util.DataBase.SqlCommand(out res, delSql);
               return res;
           }
           catch (Exception ex)
           {
               AFC.WS.UI.Common.WriteLog.Log_Error(ex.Message);
               return -1;
           }
       }

       public int AddParamsData<T>(string versionNo, string tableName) where T : class, new()
       {
           if (string.IsNullOrEmpty(versionNo) ||
               string.IsNullOrEmpty(tableName)
              )
           {
               WriteLog.Log_Error("AddParamsData() params error!");
               return -1;
           }

           string queryCmd = string.Format("select * from {0} where para_version='{1}'", tableName, versionNo);

           List<T> list = DBCommon.Instance.GetTModelValue<T>(queryCmd);

           if (list == null ||
               list.Count == 0)
           {
               return -1;
           }

           foreach (var temp in list)
           {
               Type t=temp.GetType();//.GetField("para_version");
               PropertyInfo pi = t.GetProperty("para_version");
               pi.SetValue(temp, "-1", null);
           }

           foreach (var temp in list)
           {
               int res = DBCommon.Instance.InsertTable<T>(temp, tableName);
               if (res != 1)
                   return res;
               else
                   continue;

           }
           return 0;
       }


       /// <summary>
       /// 根据现有版本生成草稿版
       /// </summary>
       /// <param name="paraType">参数类型</param>
       /// <param name="paraFileName">参数名</param>
       /// <param name="paraMasterType">主控版本类型</param>
       /// <returns></returns>
       public int InsertParaVersion(string paraType, string paraFileName, string paraMasterType)
       {
          
           ParaVersionInfo paraVersionInfo = new ParaVersionInfo();
           paraVersionInfo.active_date = DateTime.Now.ToString("yyyyMMdd");
           paraVersionInfo.active_time = DateTime.Now.ToString("HHmmss");
           paraVersionInfo.master_para_version = string.Empty;
           paraVersionInfo.occur_date = DateTime.Now.ToString("yyyyMMdd");
           paraVersionInfo.occur_time = DateTime.Now.ToString("HHmmss");
           paraVersionInfo.para_file_name = paraFileName;
           paraVersionInfo.para_master_type = paraMasterType;
           paraVersionInfo.para_sub_type = string.Empty;
           paraVersionInfo.para_type = paraType;
           paraVersionInfo.para_version = "-1";
           paraVersionInfo.update_date = DateTime.Now.ToString("yyyyMMdd");
           paraVersionInfo.update_time = DateTime.Now.ToString("HHmmss");

           if (IsExistParaInfo(paraType))
           {
               int delResult = DelParaVersionInfo(paraType, "-1");
               int res = 0;
               switch (paraType)
               {
                   case "4043":
                       res = BuinessRule.GetInstace().paraManager.del4043DraftPara(paraVersionInfo);
                       break;
                   case "4044":
                       res = BuinessRule.GetInstace().paraManager.del4044DraftPara(paraVersionInfo);
                       break;
                   case "4045":
                       res = BuinessRule.GetInstace().paraManager.del4045DraftPara(paraVersionInfo);
                       break;
                   case "4042":
                       res = BuinessRule.GetInstace().paraManager.del4042DraftPara(paraVersionInfo);
                       break;
                   case "4314":
                       res = BuinessRule.GetInstace().paraManager.del4314DraftPara(paraVersionInfo);
                       break;
                   default:
                       break;
               }

               if (delResult != 0 || res!=0)
               {
                   return -1;
               }
           }
           return DBCommon.Instance.InsertTable<ParaVersionInfo>(paraVersionInfo, "para_version_info") >0 ? 0:-1;
             
       }
         

    }



}