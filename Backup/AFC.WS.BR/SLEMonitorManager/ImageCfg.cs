using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;


namespace AFC.WS.BR.SLEMonitorManager
{
    using AFC.WS.UI.Common;
    

    /// <summary>
    /// 图片配置
    /// </summary>
    public class ImageCfg
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        protected string path;

        /// <summary>
        /// 图片路径属性
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        
        /// <summary>
        /// 内容
        /// </summary>
        protected string content;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content
        {
            set { this.content = value; }
            get { return this.content; }
        }

        /// <summary>
        /// 关键字段
        /// </summary>
        protected string keyField;

        /// <summary>
        /// 关键字段
        /// </summary>
        public string KeyField
        {
            set { this.keyField = value; }
            get { return this.keyField; }
        }

    

        public ImageCfg()
        {
         //   this.doc = GetXmlDoc(@".\Config\MonitorImageCfg.xml");
        }

     
    }
}
