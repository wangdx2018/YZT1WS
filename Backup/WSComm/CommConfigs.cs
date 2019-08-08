using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Reflection;


namespace TJComm
{
    using AFC.BJComm.Data;
    using AFC.BOM2.Common;
    /// <summary>
    /// 作者：wangdx
    /// 日期：20110302
    ///描述： 通讯配置信息对象
    ///<?xml version="1.0" encoding="utf-8">
    ///<configuration>
    ///  <configSections>
    ///    <section name="CommConfigs" type="TJComm.CommConfigSection,TJComm"/>
    ///  </configSections>
    ///  <CommConfigs>
    ///    <CommConfig name="SendHeartBeatMsgInterval" value="30"/>
    ///    <CommConfig name="IsSendHeartBeat" value="true"/>
    ///    <CommConfig name="SessionIdStartIndex" value="1"/>
    ///    <CommConfig name="SendTimeOut" value="3000"/>
    ///  </CommConfigs>
    ///</configuration>
    /// </summary>
    public class CommConfigs
    {
        public CommConfigs()
        {
        }   

        /// <summary>
        /// 发送存活包的周期，时间为秒
        /// </summary>
        public int heartBeatMsgSendInterval;
      
        /// <summary>
        /// 是否发送存活包
        /// </summary>
        public bool isSendHeartBeat;
       
        /// <summary>
        /// 解析包配置的信息，需要配置
        /// </summary>
        public string unPackMsgHandleType;

        /// <summary>
        ///处理下行数据的消息处理函数
        /// </summary>
        public string handleMsgType;
     
        public override string ToString()
        {
            FieldInfo[] fiArray = this.GetType().GetFields();
            StringBuilder sb = new StringBuilder();
            sb.Append("Config set is:\r\n");
            for (int i = 0; i < fiArray.Length; i++)
            {
                sb.Append(string.Format("[{0}]=[{1}]\r\n", fiArray[i].Name, fiArray[i].GetValue(this).ToString()));
            }
            return sb.ToString();
            //return base.ToString();
        }
    }


  /// <summary>
  /// 通讯的configSection对象
  /// </summary>
    internal class CommConfigSection:IConfigurationSectionHandler
    {

        #region IConfigurationSectionHandler 成员

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            CommConfigs config = new CommConfigs();
            if (section == null)
            {
                 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"no config sction part! set [CommonConfigs] section first!");
                return null;
            }
            XmlNodeList xnl = section.SelectNodes("CommConfig");
            if (xnl == null || xnl.Count == 0)
            {
                 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"[CommConfig] set is empty!,set CommConfig first!");
                return null;
            }
            foreach (XmlNode node in xnl)
            {
                FieldInfo[] fiArray = config.GetType().GetFields();
                for (int i = 0; i < fiArray.Length; i++)
                {
                    if (fiArray[i].Name == node.Attributes["name"].Value)
                    {
                        try
                        {
                                fiArray[i].SetValue(config, ParseFieldValue(fiArray[i], node.Attributes["value"].Value));
                        }
                        catch (Exception ex)
                        {
                             WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Set Filed name=[" + fiArray[i].Name + "] error!" + ex.Message);
                            return null;
                        }
                    }
                }
            }
            return config;
        }

        private  object ParseFieldValue(FieldInfo fi, string value)
        {
            try
            {
                if (fi.FieldType.IsEnum)
                {
                    return Enum.Parse(fi.FieldType, value);
                }

                if (fi.FieldType.Name == "String")
                {
                    return value;
                }

                if (fi.FieldType.Name == "Double")
                {
                    double res = 0;
                    if (double.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Single")
                {
                    float res = 0;
                    if (float.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Char")
                {
                    char res = '0';
                    if (char.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Byte")
                {
                    byte res = 0;
                    if (byte.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "UInt16")
                {
                    UInt16 res = 0;
                    if (UInt16.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Int16")
                {
                    Int16 res = 0;
                    if (Int16.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Int32")
                {
                    int res = 0;
                    if (int.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "UInt32")
                {
                    uint res = 0;
                    if (uint.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Uint64")
                {
                    UInt64 res = 0;
                    if (UInt64.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }

                if (fi.FieldType.Name == "Int64")
                {
                    Int64 res = 0;
                    if (Int64.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }
                if (fi.FieldType.Name == "Boolean")
                {
                    bool res = false;
                    if (bool.TryParse(value, out res))
                        return res;
                    throw new Exception("FieldName=[" + fi.Name + "], type=[" + fi.FieldType + "],value=[" + value + "]");
                }
                throw new Exception("system can't Handle that type [" + fi.FieldType.Name + "]");
            }
            catch (Exception ex)
            {
                 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+ex.Message);
                return null;
            }
        }
        #endregion
    }
}
