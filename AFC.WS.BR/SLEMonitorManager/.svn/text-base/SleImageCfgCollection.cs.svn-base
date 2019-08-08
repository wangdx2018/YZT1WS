using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AFC.WS.BR.SLEMonitorManager
{
    using AFC.WS.UI.Common;
    using System.IO;

    /// <summary>
    /// added by wangdx 20110603
    /// SLE ImageConfig file
    /// </summary>
    public class SleImageCfgCollection
    {

        /// <summary>
        /// 设备图片的的对应字典
        /// </summary>
        public Dictionary<string, ImageCfg> deviceImageDict = new Dictionary<string, ImageCfg>();

        /// <summary>
        /// 设备运营状态对应字典
        /// </summary>
        public Dictionary<string, ImageCfg> devRunStatusDict = new Dictionary<string, ImageCfg>();

        /// <summary>
        /// 车站运营模式对应字典
        /// </summary>
        public Dictionary<string, ImageCfg> stationRunStatusDict = new Dictionary<string, ImageCfg>();

        /// <summary>
        /// 箱子状态对应关系
        /// </summary>
        public Dictionary<string, ImageCfg> boxStatusDict = new Dictionary<string, ImageCfg>();

        /// <summary>
        /// 车站布局图字典
        /// </summary>
        public Dictionary<string, ImageCfg> sationLayoutDict = new Dictionary<string, ImageCfg>();

        /// <summary>
        /// 选中后的设备图标
        /// </summary>
        public Dictionary<string, ImageCfg> devImageSelectedDict = new Dictionary<string, ImageCfg>();

        /// <summary>
        /// 车站图片布局对象
        /// </summary>
        private XmlDocument doc = null;

        /// <summary>
        /// 得到文档对象
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回XmlDocument对象</returns>
        private XmlDocument GetXmlDoc(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                WriteLog.Log_Error("fileName is null or empty!");
                return null;
            }
            if (!File.Exists(fileName))
            {
                WriteLog.Log_Error("file not exist name=[" + fileName + "]");
                return null;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            return doc;
        }

        /// <summary>
        /// 解析xml文件
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        private List<ImageCfg> ParseXmlNode(string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName))
            {
                WriteLog.Log_Error("nodeName is null or empty");
                return null;
            }

            XmlNodeList element = doc.DocumentElement.GetElementsByTagName(nodeName);
            if (element==null)
            {
                WriteLog.Log_Error("not exist tag name=[" + nodeName + "]");
                return null;
            }

            XmlNodeList list = element[0].ChildNodes;
            if (list == null || list.Count ==0)
            {
                WriteLog.Log_Error("not exist image node or image list's count =0");
                return null;
            }

            List<ImageCfg> collection = new List<ImageCfg>();
            for (int i = 0; i < list.Count; i++)
            {
                ImageCfg cfg = null;
                if (IsContainsAttribute(list[i], "dir"))
                {
                     cfg = new AGImageCfg();
                    cfg.Path = GetAttributeValue(list[i], "path");
                    cfg.Content = GetAttributeValue(list[i], "content");
                    cfg.KeyField = GetAttributeValue(list[i], "keyField");
                    ((AGImageCfg)(cfg)).Dirt=GetAttributeValue(list[i], "dir");
                }
                else
                {
                    cfg = new ImageCfg();
                    cfg.Path = GetAttributeValue(list[i], "path");
                    cfg.Content = GetAttributeValue(list[i], "content");
                    cfg.KeyField = GetAttributeValue(list[i], "keyField");
                }
               
                collection.Add(cfg);

            }
            return collection;
        }

        /// <summary>
        ///判断是存在dir属性
        /// </summary>
        /// <param name="name">属性名称</param>
        /// <param name="node">xml 结点信息</param>
        /// <returns>存在返回true，不存在返回false</returns>
        private bool IsContainsAttribute(XmlNode node,string name)
        {
            try
            {
               string attributeValue= node.Attributes[name].Value;
            }
            catch(Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return false;
            }
          return true;
        }

        /// <summary>
        /// 得到属性是值
        /// </summary>
        /// <param name="attributeName">属性名称</param>
        /// <returns>返回属性值</returns>
        private string GetAttributeValue(XmlNode node, string attributeName)
        {
            try
            {
                if (string.IsNullOrEmpty(attributeName))
                {
                    WriteLog.Log_Error("attribute name is null or empty!");
                    return null;
                }
                if (node == null)
                {
                    WriteLog.Log_Error("xml node is null");
                    return null;
                }
                return node.Attributes[attributeName].Value;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }

        }

        /// <summary>
        ///初始化加载xml配置文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public int init(string fileName)
        {
            if (this.boxStatusDict.Count != 0||
                this.deviceImageDict.Count!=0||
                this.devRunStatusDict.Count!=0||
                this.sationLayoutDict.Count!=0||
                this.stationRunStatusDict.Count!=0)
            {
                WriteLog.Log_Warn("that file name=[" + fileName + "] has been loaded");
                return 0;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                WriteLog.Log_Error("DeviceMonitor image config is null or empty!");
                return -1;
            }
            if (!File.Exists(fileName))
            {
                WriteLog.Log_Error("not exist fileName=[" + fileName + "]");
                return -1;
            }

            doc = GetXmlDoc(fileName);
            if (doc == null)
                return -1;

            int res = InitNodes("Layout", this.sationLayoutDict);
            if (res != 0)
            {
                WriteLog.Log_Error("Init layout node error!");
                return res;
            }
            res = InitNodes("BoxStatus", this.boxStatusDict);
            if (res != 0)
            {
                WriteLog.Log_Error("Init boxStatus error!");
                return -1;
            }
            res = InitNodes("DevImage", this.deviceImageDict);
            if (res != 0)
            {
                WriteLog.Log_Error("Init dev image error!");
                return -1;
            }

            res = InitNodes("DevImageSelected", this.devImageSelectedDict);
            if(res!=0)
            {
                WriteLog.Log_Error("Init dev image selected error!");
                return -1;
            }

            res = InitNodes("StationRunStatus", this.stationRunStatusDict);
            if (res != 0)
            {
                WriteLog.Log_Error("init station run status error!");
                return -1;
            }
            res = InitNodes("DevRunStatus", this.devRunStatusDict);
            if (res != 0)
            {
                WriteLog.Log_Error("init dev run status error!");
                return -1;
            }
            return 0;

        }

        /// <summary>
        /// 解析XmlNode结点
        /// </summary>
        /// <param name="nodeName">结点名称</param>
        /// <param name="dict">字典对象</param>
        /// <returns>成功返回0，否则返回-1</returns>
        private int InitNodes(string nodeName,Dictionary<string,ImageCfg> dict)
        {
            List<ImageCfg> layoutImages = this.ParseXmlNode(nodeName);
            if (layoutImages == null)
            {
                WriteLog.Log_Error("parse xml node name=["+nodeName+"] error!");
                return -1;
            }
            foreach (var temp in layoutImages)
            {
                if (temp.KeyField!=null&&
                    !dict.ContainsKey(temp.KeyField))
                {
                    if (temp is AGImageCfg)
                    {
                        AGImageCfg agCfg = temp as AGImageCfg;
                        agCfg.KeyField = agCfg.KeyField + agCfg.Dirt;
                        dict.Add(agCfg.KeyField, agCfg);
                    }
                    else
                    {
                        dict.Add(temp.KeyField, temp);
                    }
                }
                else
                {
                    WriteLog.Log_Error("Contain key=[" + temp.KeyField + "]");
                }
            }
            return 0;
        }

    }

  

    
}
