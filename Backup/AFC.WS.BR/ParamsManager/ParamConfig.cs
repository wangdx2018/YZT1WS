using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.ParamsManager
{
    /// <summary>
    /// 参数配置信息类 对应配置文件 ParamsConfig.xml
    /// added by wangdx  date:20110328
    /// </summary>
    public class ParamConfig
    {

        /// <summary>
        /// 参数类型
        /// </summary>
        public string Type;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 集合信息
        /// </summary>
        public List<ParamItem> ItemList;
    }

    /// <summary>
    /// 显示子项
    /// </summary>
    public class ParamItem
    {
        /// <summary>
        /// 列表项目ID
        /// </summary>
        public string ID;

        /// <summary>
        /// 规则文件名称
        /// </summary>
        public string FileName;

        /// <summary>
        /// 显示方式，Struct，List
        /// </summary>
        public Style Style;

        /// <summary>
        /// 显示描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName;

        /// <summary>
        /// 数据源名称
        /// </summary>
        public string DataSourceName;


        public string ControlType;
    }

    /// <summary>
    /// 显示方式
    /// </summary>
    public enum Style:int
    {
        /// <summary>
        /// 单条显示
        /// </summary>
        Struct=0,

        /// <summary>
        /// 列表显示
        /// </summary>
        List=1
    }
}
