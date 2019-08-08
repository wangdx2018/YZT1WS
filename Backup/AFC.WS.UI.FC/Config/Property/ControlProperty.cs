using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

using AFC.WS.UI.Common;
using System.Reflection;
using System.IO;
using System.Xml;

/****************************************************************************************
 * 
 * 修 改 人：LiaoHaiBing
 * 修改时间：2010-05-04 AM
 * 修改原因：针对一些值要进行，如在界面上输入的设备编码、钱箱编码、票箱编码九位，可是数据库
 *           里存放的却是8位，设备编码、钱箱编码、票箱编码最后三位由十六进表示的，所在要将
 *           进行转换，要不能UI输入的ID是九位，查询不出结果出来。
 * 
 * **************************************************************************************/

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 控件属性类。
    /// 
    /// 对用户控件属性进行配置，如配置控件的样式、控件的操作符（'=' '＞=' '＜=' '＞' '＜' '＜＞' ）、控件的初始值、
    /// 
    /// 控件名称、控件绑定字段、是否占一行、占用行数、类名(用于反射创建用的)、控件显示内容、选择控件类型(CheckBox、ComboBox、Label、RiadioButton、TextBox)等。
    /// 
    /// </summary>
    public class ControlProperty
    {
        #region --> Conformation Method

        /// <summary>
        /// 控件属性类构造方法
        /// </summary>
        public ControlProperty()
        {
            Utility.Instance.InitValueEvent += new DelegateInitValue(InitValueEvent);
        }

        #endregion -> Conformation Method

        #region --> Methods

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return ControlName == null ? this.GetType().Name : ControlName.ToString();
        }

        /// <summary>
        /// 获取用户自定义控件事件方法
        /// </summary>
        /// <param name="className">类名称</param>
        private void GetUserDefinedControlPropertyMethod(string className)
        {
            if (!Utility.Instance.IsModify)
            {
                try
                {
                    DllClassProperty dcp = null;
                    List<DllClassProperty> dcpList = Utility.Instance.ClassPropertyControlsList;
                    foreach (DllClassProperty obj in dcpList)
                    {
                        if (obj.ClassName == className)
                        {
                            dcp = obj;
                            break;
                        }
                    }
                    if (null != dcp)
                    {
                        ControlTypeName = dcp.FullName + "," + dcp.AssemblyName;
                        controlObject = Activator.CreateInstance(dcp.DllClassType);
                        CreateForm();
                    }
                }
                catch (Exception ee)
                {
                    //WriteLog.Log_Error(ee);
                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                }
            }
        }
        /// <summary>
        /// 赋值操作.
        /// </summary>
        /// <param name="content">内容</param>
        void InitValueEvent(string content)
        {
            _InitValue = content;
        }

        /// <summary>
        /// 判断行数据是否正确。
        /// </summary>
        /// <param name="value">行数</param>
        /// <returns>true:正确；false:错误。</returns>
        private bool JudgRowCountIsRight(int value)
        {
            bool result = false;

            if (IsOccupancyRow)
            {
                if (value < 2 || value > 100)
                {
                    result = false;
                    MessageBox.Show("您输入的行数过大或过小，行数的范围应在 2~100 之间。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = false;
                //MessageBox.Show("[是否占一行]值“False”，所以不能输入行数值。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return result;
        }
        
        #region -->创建弹出窗体
        /// <summary>
        /// 定义一个窗体
        /// </summary>
        Form controlForm;
        /// <summary>
        /// 定义一个PropertyGrid控件.
        /// </summary>
        PropertyGrid pg;
        /// <summary>
        /// 控件定义属性对象
        /// </summary>
        object controlObject;
        /// <summary>
        /// 创建窗体，并在窗体上添加一个PropertyGrid控件。
        /// </summary>
        private void CreateForm()
        {
            try
            {
                int Width = 300;
                int Height = 500;
                controlForm = new Form();
                pg = new PropertyGrid();

                controlForm.Size = new Size(Width, Height);
                controlForm.ControlBox = false;
                controlForm.StartPosition = FormStartPosition.CenterScreen;
                controlForm.MaximumSize = new Size(Width, Height);
                controlForm.MinimumSize = new Size(Width, Height);
                controlForm.Text = _ComboBoxControl + "属性";
                pg.Dock = DockStyle.Top;
                pg.Size = new Size(Width, Height - 100);

                Button btSave = new Button();
                btSave.Name = "btnSave";
                btSave.Text = "确定";
                btSave.Location = new Point(60, Height - 80);
                btSave.Click += new EventHandler(btSave_Click);
                Button btClose = new Button();
                btClose.Name = "btnClose";
                btClose.Text = "取消";
                btClose.Location = new Point(170, Height - 80);
                btClose.Click += new EventHandler(btClose_Click);
                pg.BrowsableAttributes = new AttributeCollection(new Attribute[] { new FilterAttribute() });

                try
                {
                    //Type co = controlObject.GetType();
                    List<PropertyValue> pvList = PropertyHelper.GetPropertyList(this.GetType().Name, ControlName, ComboBoxControl);
                    if (pvList != null && pvList.Count > 0)
                    {
                        PropertyHelper.Restore(pvList, controlObject);
                    }
                }
                catch { }

                pg.SelectedObject = controlObject;
                controlForm.Controls.Add(pg);
                controlForm.Controls.Add(btSave);
                controlForm.Controls.Add(btClose);
                controlForm.CancelButton = btClose;
                controlForm.ShowDialog();
            }
            catch (Exception ee)
            {
                // WriteLog.Log_Error(ee);
                Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
            }
        }
        /// <summary>
        /// 关闭窗体事件.
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        void btClose_Click(object sender, EventArgs e)
        {
            if (controlForm != null)
            {
                controlForm.Dispose();
                controlForm.Close();
            }
        }
        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        void btSave_Click(object sender, EventArgs e)
        {
            if (controlObject != null)
            {
                controlObject = pg.SelectedObject;

                PropertyHelper.Save(controlObject, this.GetType().Name, ControlName, ComboBoxControl);
                _PropertyValues = PropertyHelper.GetPropertyList(this.GetType().Name, ControlName, ComboBoxControl);

                controlForm.Dispose();
                controlForm.Close();
            }
        }

        #endregion -->创建弹出窗体

        #endregion --> Mehtods

        #region --> Property
        /// <summary>
        /// 像素大小
        /// </summary>
        private int _Space = 5;
        /// <summary>
        /// 像素大小。
        /// </summary>
        [XmlAttribute(),
        DisplayName("像素大小"),
        Description("像素大小"),
        Category("属性设置")]
        public int Space
        {
            get { return _Space; }
            set { _Space = value; }
        }

        /// <summary>
        /// 操作符类型
        /// </summary>
        private OperationSymbols _Symbols = OperationSymbols.Equal;
        /// <summary>
        /// 操作符类型，默认为等于'='
        /// </summary>
        [DisplayName("操作符"),
        Description("操作符"),
        XmlAttribute(),
        Category("属性设置")]
        public OperationSymbols Symbols
        {
            get { return _Symbols; }
            set { _Symbols = value; }
        }

        /// <summary>
        /// 初始值。
        /// </summary>
        private string _InitValue;
        /// <summary>
        /// 初始值。
        /// </summary>
        [DisplayName("初始值"),
        Description("初始值"),
        XmlAttribute(),
        Category("属性设置")]
        public string InitValue
        {
            get { return _InitValue; }
            set { _InitValue = value; }
        }

        /// <summary>
        /// 控件名称
        /// </summary>
        private string _ControlName;
        /// <summary>
        /// 控件名称
        /// </summary>
        [Description("控件名称"),
        DisplayName("控件名称"),
        XmlAttribute(),
        Category("属性设置")]
        public string ControlName
        {
            get { return _ControlName; }
            set
            {
                //-->控件名称只能是下划线及字母开头。
                if (Utility.Instance.JudegPropertyNameIsRight(value))
                {

                    _ControlName = value;
                }
                else
                {
                    MessageBox.Show("控件名称必须是以下划线及字母开头。", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        /// <summary>
        /// 要绑定字段
        /// </summary>
        private string _BindingField;
        /// <summary>
        /// 要绑定字段
        /// </summary>
        [Description("要绑定字段"),
        DisplayName("绑定字段"),
        XmlAttribute(),
        Category("属性设置")]
        public string BindingField
        {
            get { return _BindingField; }
            set { _BindingField = value; }
        }

        /// <summary>
        /// 是否单独一行
        /// </summary>
        private bool _IsOccupancyRow = false;
        /// <summary>
        /// 是否单独一行
        /// </summary>
        [DisplayName("是否占一行"),
        XmlAttribute(),
        Category("属性设置")]
        public bool IsOccupancyRow
        {
            get { return _IsOccupancyRow; }
            set {
                _IsOccupancyRow = value;
                PropertyHelper.SetPropertyVisibility(this, "RowsCount", value);
            }
        }

        /// <summary>
        /// 占用行数
        /// </summary>
        private int _RowsCount = 2;
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false),
         DisplayName("占用行数"),
         XmlAttribute(),
         Category("属性设置")]
        public int RowsCount
        {
            get { return _RowsCount; }
            set
            {
                if (JudgRowCountIsRight(value))
                {
                    _RowsCount = value;
                }
            }
        }

        /// <summary>
        /// 样式
        /// </summary>
        private string _Style;
        /// <summary>
        /// 样式
        /// </summary>
        [Description("样式"),
        DisplayName("样式"),
        XmlAttribute(),
        Category("属性设置")]
        public string Style
        {
            get { return _Style; }
            set { _Style = value; }
        }

        /// <summary>
        /// 类名
        /// </summary>
        private string _ControlTypeName;
        /// <summary>
        /// 类名
        /// </summary>
        [XmlAttribute(),
        DisplayName("类名"),
        Category("属性设置")]
        public string ControlTypeName
        {
            get { return _ControlTypeName; }
            set { _ControlTypeName = value; }
        }

        /// <summary>
        /// 控件显示内容
        /// </summary>
        private string _Lable;
        /// <summary>
        /// 控件显示内容
        /// </summary>
        [XmlAttribute(),
        DisplayName("控件显示内容"),
        Category("属性设置")]
        public string Lable
        {
            get { return _Lable; }
            set { _Lable = value; }
        }

        /// <summary>
        /// 选择Control
        /// </summary>
        private string _ComboBoxControl;
        /// <summary>
        /// 选择Control
        /// </summary>
        [XmlAttribute(),
        Description("自定义控件属性"),
        DisplayName("选择Control"),
        Category("属性设置"),
        TypeConverter(typeof(TypeConvertPropertyGridControls))]
        public string ComboBoxControl
        {
            get { return _ComboBoxControl; }
            set
            {
                if (String.IsNullOrEmpty(ControlName))
                {
                    MessageBox.Show("请输入控件名称。");
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        _ComboBoxControl = value;
                    }
                    GetUserDefinedControlPropertyMethod(_ComboBoxControl);
                }
            }
        }

        /// <summary>
        /// 键值对集合。
        /// </summary>
        private List<PropertyValue> _PropertyValues = new List<PropertyValue>();
        /// <summary>
        /// 键值对集合。
        /// </summary>
        [Category("自定义"),
        DisplayName("键值对")]
        public List<PropertyValue> PropertyValues
        {
            get
            {
                if (_PropertyValues !=null)
                {
                    foreach (PropertyValue pv in _PropertyValues)
                    {
                        //if (pv.Key.ToLower() == "contentdatepicker")
                        //{
                        //    _InitValue = pv.Value;
                        //    break;
                        //}
                    }
                }
                PropertyValueCollection pvcList = new PropertyValueCollection();
                pvcList.ClassName = this.GetType().Name;
                pvcList.DefineName = ComboBoxControl;
                pvcList.FlagName = ControlName;
                pvcList.PropertyList = _PropertyValues;
                PropertyHelper.Add(pvcList);
                return _PropertyValues;
            }
            set
            {
                _PropertyValues = value;
            }
        }

        #endregion --> Property

        /// <summary>
        /// 格式化
        /// </summary>
        private string _ConvertoTypeName;
        /// <summary>
        /// 选择Convertor
        /// </summary>
        private string _ComboBoxConvertor;
        /// <summary>
        /// Convertor自定义属性
        /// </summary>
        Object targetConvertor;
        /// <summary>
        /// 键值对集合。
        /// </summary>
        private List<PropertyValue> _TextConvertor = new List<PropertyValue>();


        /// <summary>
        /// 格式化
        /// </summary>
        [Description("转换器类名"),
        DisplayName("转换器类名"),
        XmlAttribute(),
        Category("属性设置")]
        public string ConvertoTypeName
        {
            get { return _ConvertoTypeName; }
            set { _ConvertoTypeName = value; }
        }
        /// <summary>
        /// 选择Convertor
        /// </summary>
        [XmlAttribute(),
        Description("自定义属性"),
        DisplayName("选择Convertor"),
        Category("属性设置"),
        TypeConverter(typeof(TypeConvertPropertyGridConvertor))]
        public string ComboBoxConvertor
        {
            get { return _ComboBoxConvertor; }
            set
            {
                if (String.IsNullOrEmpty(BindingField))
                {
                    MessageBox.Show("请输入绑定字段。");
                }
                else
                {
                    _ComboBoxConvertor = value;
                    GetUserDefinedConvertorPropertyMethod(_ComboBoxConvertor);
                }
            }
        }



        private string toolTip;

        [Description("提示"),
     DisplayName("ToolTip提示"),
     XmlAttribute(),
     Category("提示")]
        public string ToolTip
        {
            set { this.toolTip = value; }
            get { return this.toolTip; }
        }

        /// <summary>
        /// 获取用户自定义Convertor属性方法
        /// </summary>
        /// <param name="className">类名称</param>
        private void GetUserDefinedConvertorPropertyMethod(string className)
        {
            try
            {
                DllClassProperty dcp = null;
                List<DllClassProperty> dcpList = Utility.Instance.ClassPropertyConvertorList;
                foreach (DllClassProperty obj in dcpList)
                {
                    if (obj.ClassName == className)
                    {
                        dcp = obj;
                        break;
                    }
                }
                if (null != dcp)
                {
                    ConvertoTypeName = dcp.FullName + "," + dcp.AssemblyName;
                    targetConvertor = Activator.CreateInstance(dcp.DllClassType);
                }
            }
            catch (Exception ee)
            {
                Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
            }
        }
        /// <summary>
        /// Convertor自定义属性
        /// </summary>
        [Filter(),
        DisplayName("Convertor自定义属性"),
        Category("自定义"),
        TypeConverter(typeof(ExpandableObjectConverter))]
        public object TargetConvertor
        {
            get
            {
                if (targetConvertor != null)
                {
                    if (Utility.Instance.IsModify)
                    {
                        PropertyHelper.Restore(_TextConvertor, targetConvertor);
                    }
                    else
                    {
                        PropertyHelper.Save(targetConvertor, this.GetType().Name, BindingField, ComboBoxConvertor);
                        _TextConvertor = PropertyHelper.GetPropertyList(this.GetType().Name, BindingField, ComboBoxConvertor);
                    }
                }
                return targetConvertor;
            }
            set { targetConvertor = value; }
        }
        /// <summary>
        /// 键值对集合。
        /// </summary>
        [DisplayName("内容转换键值对"),
        Category("自定义")]
        public List<PropertyValue> TextConvertor
        {
            get { return _TextConvertor; }
            set { _TextConvertor = value; }
        }
    }
}