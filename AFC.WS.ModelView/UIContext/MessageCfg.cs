using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.BR;
using System.Xml;
using System.Reflection;
using AFC.WS.UI.Common;

namespace AFC.WS.ModelView.UIContext
{
    public class MessageCfg
    {
        private static List<MessageConfig> messageConfigList = new List<MessageConfig>();

        public static int LoadMessageConfigFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();  
            try
            {
                doc.Load(fileName);
                XmlNodeList xnl = doc.GetElementsByTagName("MessageConfig");
                if (xnl != null && xnl.Count != 0)
                {
                    for (int i = 0; i < xnl.Count; i++)
                    {
                        MessageConfig config = new MessageConfig();
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
                            List<MessageItem> list = new List<MessageItem>();
                            for (int index = 0; index < itemList.Count; index++)
                            {
                                MessageItem item = new MessageItem();
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
                        messageConfigList.Add(config);
                    }
                    AddCommonHandleType();
                }
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }

        }

        public static MessageConfig GetMessageConfig(string messageType)
        {
            try
            {

                return messageConfigList.Single(temp => temp.Type.Equals(messageType));
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("Not found messageType=[" + messageType.ToString() + "]");
                return null;
            }
        }

        public  static string getMessageContent(string messageType, string messageID)
        {

            try
            {
               int value = int.Parse(messageID.ToString());
               if (value < 0)
               {
                   List<MessageItem> list = GetMessageConfig(messageType).ItemList;
                   return list.Single(temp => temp.ID.Equals(value.ToString())).Content;
               }
                messageID = value.ToString("X8");
                return GetMessageConfig(messageType).ItemList.Single(temp => temp.ID.Equals(messageID)).Content;
            }
            catch (Exception ex)
            {
                return "未知错误，错误代码【"+messageID+"】";
            }
        }

        /// <summary>
        /// 将公共头添加到了每个业务处理模块提示中
        /// </summary>
        private static void AddCommonHandleType()
        {
            MessageConfig cfg = GetMessageConfig(CommType);
            if (cfg == null)
                return;
            messageConfigList.Remove(cfg);
            foreach (var temp in messageConfigList)
            {
                temp.ItemList.AddRange(cfg.ItemList);
            }
            
        }

        /// <summary>
        /// 通用的通讯错误处理
        /// </summary>
        public const string CommType = "13FF";

    }
}
