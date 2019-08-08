using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

using AFC.WS.UI.Common;
using System.Windows.Forms;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// UI规则文件;
    /// 
    /// 此类主要用于界面上生成多少个控件，如可以生成Label、TextBox、CheckBox、ComboBox等些控件。
    /// 
    /// 如些界面上要生成几行几列、界面的排列方式、Button的样式、Action位置：上下左右、Action对齐方式、是否配置重置按钮等信息。
    /// 
    /// </summary>
    [Description("用于查询的规则文件")]
    public class InteractiveControlRule
    {
        #region --> Property.
        /// <summary>
        /// Top高度
        /// </summary>
        private int _TopSpace = 10;
        /// <summary>
        /// 行高
        /// </summary>
        private int _RowHeight = 26;
        /// <summary>
        /// Action 占窗体的百分比。
        /// </summary>
        private int _ActionHeight =20;
        /// <summary>
        /// Label与控件比例
        /// </summary>
        double _LabWidthPercent = 45;

        /// <summary>
        /// Label与控件比例。
        /// </summary>
        [XmlAttribute(),
        DisplayName("Label与控件比例"),
        Description("Label与TextBox,DateTime,ComboxBox等控件所占比例。"),
        Category("属性设置")]
        public double LabWidthPercent
        {
            get { return _LabWidthPercent; }
            set { _LabWidthPercent = value; }
        }
        /// <summary>
        /// Action 占窗体的百分比。
        /// </summary>
        [XmlAttribute(),
        DisplayName("Action百分比"),
        Description("Action位置所占百分比。"),
        Category("属性设置")]
        public int ActionHeight
        {
            get { return _ActionHeight; }
            set
            {
                if (value <= 100 && value >= 0)
                {
                    _ActionHeight = value;
                }
                else
                {
                    MessageBox.Show("输入的百分比不能为负数或大于100,应该在[0~100]之间。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 行高
        /// </summary>
        [XmlAttribute(),
        DisplayName("行高"),
        Description("行的高度。"),
        Category("属性设置")]
        public int RowHeight
        {
            get { return _RowHeight; }
            set { _RowHeight = value; }
        }

        /// <summary>
        /// Top高度
        /// </summary>
        [XmlAttribute(),
        DisplayName("Top高度"),
        Description("Top高度。"),
        Category("属性设置")]        
        public int TopSpace
        {
            get { return _TopSpace; }
            set { _TopSpace = value; }
        }

        /// <summary>
        /// 列数
        /// </summary>
        private int _ColumnCount;
        /// <summary>
        /// 列数
        /// </summary>
        [Description("列数"),
        DisplayName("列数"),
        XmlAttribute(),
        Category("属性设置")]
        public int ColumnCount
        {
            get { return _ColumnCount; }
            set { _ColumnCount = value; }
        }

        /// <summary>
        /// 排列方式
        /// </summary>
        private LayoutModes _LayoutMode = LayoutModes.Stack;
        /// <summary>
        /// 排列方式
        /// </summary>
        [Description("控件以什么方式排列"),
        DisplayName("排列方式"),
        XmlAttribute(),
        Category("属性设置")]
        public LayoutModes LayoutMode
        {
            get { return _LayoutMode; }
            set { _LayoutMode = value; }
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
        private ActionLocations _ActionLocation = ActionLocations.Left;  //上下左右
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
        private ActionAligns _ActionAlign = ActionAligns.Left;
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
        /// 是否配置重置按钮
        /// </summary>
        private bool _CanRest = true;
        /// <summary>
        /// 是否配置重置按钮
        /// </summary>
        [XmlAttribute(),
        DisplayName("是否配置重置按钮"),
        Description("是否配置重置按钮"),
        Category("属性设置")]
        public bool CanRest
        {
            get { return _CanRest; }
            set { _CanRest = value; }
        }

        /// <summary>
        /// 控件属性集合配置
        /// </summary>
        private List<ControlProperty> _ControlList;
        /// <summary>
        /// 控件属性集合配置
        /// </summary>
        [Description("控件属性"),
        DisplayName("控件集合配置"),
        Category("集合属性设置")]
        public List<ControlProperty> ControlList
        {
            get
            {
                if (_ControlList == null)
                {
                    _ControlList = new List<ControlProperty>();
                }
                return _ControlList;
            }
            set { _ControlList = value; }
        }

        /// <summary>
        /// 操作按钮(Action)集合
        /// </summary>
        private List<ActionProperty> _ActionList;
        /// <summary>
        /// 操作按钮(Action)集合
        /// </summary>
        [Description("动作集合"),
        DisplayName("操作按钮(Action)"),
        Category("集合属性设置")]
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

        #endregion --> Property.
    }
}
