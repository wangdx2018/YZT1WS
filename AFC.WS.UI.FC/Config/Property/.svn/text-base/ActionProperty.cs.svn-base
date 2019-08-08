using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using AFC.WS.UI.Common;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 操作按钮属性类;
    /// 
    /// 此类专门为Action提供配置，其实就是为了生成一个Button控件用的，在生成的Button控件的时候，加上一些特殊的属性：
    /// 
    /// 如控件名称、控件上显示的内容、反射用的类名、选择Action各类、还有就是对选择的Action类的显示出来的自定义属性进行填写。
    /// 
    /// 
    /// edited by wangdx 增加了是否为默认按钮的控制。能够IsDefaultButton能够单击回车响应事件。
    /// 
    /// </summary>
    public class ActionProperty
    {
        #region --> Methods.

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return this.Content.ToString();
        }

        /// <summary>
        /// 获取用户自定义Action属性方法
        /// </summary>
        /// <param name="className">类名称</param>
        private void GetUserDefinedActionPropertyMethod(string className)
        {
            try
            {
                DllClassProperty dcp = null;
                List<DllClassProperty> dcpList = Utility.Instance.ClassPropertyActionList;

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
                    ActionTypeName = dcp.FullName + "," + dcp.AssemblyName;
                    targetAction = Activator.CreateInstance(dcp.DllClassType);
                }
            }
            catch (Exception ee)
            {
                //WriteLog.Log_Error(ee);
                Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
            }
        }

        #endregion --> Methods.

        #region --> Property.

        /// <summary>
        /// 控件名称。
        /// </summary>
        private string _ControlName = string.Empty;


        private bool isDeafultButton = false;

        /// <summary>
        /// 控件名称。
        /// </summary>
        [Description("控件名称"),
        XmlAttribute(),
        DisplayName("控件名称"),
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
        /// 控件上显示的内容。
        /// </summary>
        private string _Content = string.Empty;
        /// <summary>
        /// 控件上显示的内容。
        /// </summary>
        [Description("控件上显示的内容"),
        XmlAttribute(),
        DisplayName("控件上显示内容"),
        Category("属性设置")]
        public string Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        [Description("是否为DeafultButton"),
        XmlAttribute(),
        DisplayName("是否为DeafultButton"),
        Category("属性设置")]
        public bool IsDefaultButton
        {
            set { this.isDeafultButton = value; }
            get { return this.isDeafultButton; }
        }

        /// <summary>
        /// 类名。
        /// </summary>
        private string _ActionTypeName;
        /// <summary>
        /// 类名。
        /// </summary>
        [XmlAttribute(),
        DisplayName("类名"),
        Category("属性设置")]
        public string ActionTypeName
        {
            get { return _ActionTypeName; }
            set { _ActionTypeName = value; }
        }

        /// <summary>
        /// 选择Action
        /// </summary>
        private string _ComboBoxAction;
        /// <summary>
        /// 选择Action
        /// </summary>
        [XmlAttribute(),
        Description("自定义属性"),
        DisplayName("选择Action"),
        Category("属性设置"),
        TypeConverter(typeof(TypeConvertPropertyGridAction))]
        public string ComboBoxAction
        {
            get { return _ComboBoxAction; }
            set
            {
                if (String.IsNullOrEmpty(ControlName))
                {
                    MessageBox.Show("请输入控件名称。");
                }
                else
                {
                    _ComboBoxAction = value;

                    GetUserDefinedActionPropertyMethod(_ComboBoxAction);
                }
            }
        }

        /// <summary>
        /// Action自定义属性
        /// </summary>
        Object targetAction;
        /// <summary>
        /// Action自定义属性
        /// </summary>
        [Filter(),
        DisplayName("Action自定义属性"),
        Category("自定义"),
        TypeConverter(typeof(ExpandableObjectConverter))]
        public object TargetAction
        {
            get
            {
                if (targetAction != null)
                {
                    if (Utility.Instance.IsModify)
                    {
                        PropertyHelper.Restore(_PropertyValues, targetAction);
                    }
                    else
                    {
                        PropertyHelper.Save(targetAction, this.GetType().Name, ControlName, ComboBoxAction);
                        _PropertyValues = PropertyHelper.GetPropertyList(this.GetType().Name, ControlName, ComboBoxAction);
                    }

                }
                return targetAction;
            }
            set { targetAction = value; }
        }

        /// <summary>
        ///  键值对集合。
        /// </summary>
        private List<PropertyValue> _PropertyValues = new List<PropertyValue>();
        /// <summary>
        ///  键值对集合。
        /// </summary>
        [DisplayName("键值对"),
        Category("自定义")]
        public List<PropertyValue> PropertyValues
        {
            get { return _PropertyValues; }
            set { _PropertyValues = value; }
        }

        #endregion --> Property.

    }
}
