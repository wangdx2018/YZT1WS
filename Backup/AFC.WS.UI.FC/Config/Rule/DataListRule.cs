using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// DataList规则文件类；
    /// 
    /// 主要用于配置DataGridView上的有多少列。
    /// 
    /// 可以配置是否将DataGridView里的数据导出到Excel里去的按钮，是否有翻页功能按钮、是否有刷新按钮、皮肤、
    /// 
    /// 每页显示记录条数、数据选择列配置、数据源名称、Button样式、Action位置：上下左右、Action对齐方式等信息。
    /// 
    /// </summary>
    public class DataListRule
    {
        #region --> Methods

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return this.GetType().Name;
        }

        #endregion -> Methods

        #region --> Property

        /// <summary>
        /// 是否导出Excel
        /// </summary>
        private bool _CanExportExcel = true;
        /// <summary>
        /// 是否导出Excel
        /// </summary>
        [XmlAttribute(),
        DisplayName("是否导出Excel"),
        Description("是否导出Excel"),
        Category("属性设置")]
        public bool CanExportExcel
        {
            get { return _CanExportExcel; }
            set { _CanExportExcel = value; }
        }

        /// <summary>
        /// 是否配置翻页功能
        /// </summary>
        private bool _Paging = true;
        /// <summary>
        /// 是否配置翻页功能
        /// </summary>
        [XmlAttribute(),
        DisplayName("是否配置翻页功能"),
        Category("属性设置")]
        public bool Paging
        {
            get { return _Paging; }
            set { _Paging = value; }
        }

        /// <summary>
        /// 是否配置刷新
        /// </summary>
        private bool _CanRefurbish = true;
        /// <summary>
        /// 是否配置刷新
        /// </summary>
        [XmlAttribute(),
        DisplayName("是否配置刷新"),
        Category("属性设置")]
        public bool CanRefurbish
        {
            get { return _CanRefurbish; }
            set { _CanRefurbish = value; }
        }

        /// <summary>
        /// 皮肤
        /// </summary>
        private string _Style;
        /// <summary>
        /// 皮肤
        /// </summary>
        [XmlAttribute(),
        DisplayName("皮肤"),
        Category("属性设置")]
        public string Style
        {
            get { return _Style; }
            set { _Style = value; }
        }

        /// <summary>
        /// 每页显示记录条数
        /// </summary>
        private int _PageRecordCount = 20;
        /// <summary>
        /// 每页显示记录条数
        /// </summary>
        [XmlAttribute(),
        DisplayName("每页显示记录条数"),
        Description("每页显示记录条数"),
        Category("属性设置")]
        public int PageRecordCount
        {
            get { return _PageRecordCount; }
            set { _PageRecordCount = value; }
        }

        /// <summary>
        /// 数据选择列配置
        /// </summary>
        private SelectionModes _SelectionMode = SelectionModes.None;
        /// <summary>
        /// 数据选择列配置
        /// </summary>
        [XmlAttribute(),
        DisplayName("数据选择列配置"),
        Description("数据选择列配置"),
        Category("属性设置")]
        public SelectionModes SelectionMode
        {
            get { return _SelectionMode; }
            set { _SelectionMode = value; }
        }

        /// <summary>
        /// 数据源名称
        /// </summary>
        private string _DataSourceName;
        /// <summary>
        /// 数据源名称
        /// </summary>
        [XmlAttribute(),
        DisplayName("数据源名称"),
        Description("数据源名称"),
        Category("数据源设置")]
        public string DataSourceName
        {
            get { return _DataSourceName; }
            set { _DataSourceName = value; }
        }

        /// <summary>
        /// Button样式
        /// </summary>
        private string _ButtonStyle;
        /// <summary>
        /// Button样式
        /// </summary>
        [XmlAttribute(),
        DisplayName("Button样式"),
        Description("Button样式"),
        Category("属性设置")]
        public string ButtonStyle
        {
            get { return _ButtonStyle; }
            set { _ButtonStyle = value; }
        }

        /// <summary>
        /// 位置：上下左右
        /// </summary>
        private ActionLocations _ActionLocation = ActionLocations.Bottom;  //上下左右
        /// <summary>
        /// 位置：上下左右
        /// </summary>
        [XmlAttribute(),
        DisplayName("位置"),
        Description("位置"),
        Category("属性设置")]
        public ActionLocations ActionLocation
        {
            get { return _ActionLocation; }
            set { _ActionLocation = value; }
        }

        /// <summary>
        /// 对齐方式
        /// </summary>
        private ActionAligns _ActionAlign = ActionAligns.Middle;
        /// <summary>
        /// 对齐方式
        /// </summary>
        [XmlAttribute(),
        DisplayName("对齐方式"),
        Description("对齐方式"),
        Category("属性设置")]
        public ActionAligns ActionAlign
        {
            get { return _ActionAlign; }
            set { _ActionAlign = value; }
        }

        /// <summary>
        /// 数据列集合
        /// </summary>
        private List<ColumnProperty> _ColumnList;
        /// <summary>
        /// 数据列集合
        /// </summary>
        [Description("数据列集合"),
        DisplayName("列Column设置"),
        CategoryAttribute("集合属性设置")]
        public List<ColumnProperty> ColumnList
        {
            get
            {
                if (_ColumnList == null)
                {
                    _ColumnList = new List<ColumnProperty>();
                }
                return _ColumnList;
            }
            set { _ColumnList = value; }
        }

        /// <summary>
        /// 动作集合
        /// </summary>
        private List<ActionProperty> _ActionList;
        /// <summary>
        /// 动作集合
        /// </summary>
        [Description("动作集合"),
        DisplayName("操作按钮(Action)"),
        CategoryAttribute("集合属性设置")]
        public List<ActionProperty> ActionList
        {
            get
            {
                if (_ActionList == null)
                {
                    _ActionList = new List<ActionProperty>();
                }
                return _ActionList;
            }
            set
            {
                _ActionList = value;
            }
        }

        #endregion --> Property

    }
}
