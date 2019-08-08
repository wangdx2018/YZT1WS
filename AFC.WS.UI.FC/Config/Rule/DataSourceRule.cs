using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Windows.Forms;

using AFC.WS.UI.Common;
using System.Reflection;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 数据源规则文件类。
    /// 
    /// 此主要是为了DataList、ChartRule以及Action中提供数据的。
    /// 
    /// 其中主要配置有：数据源名称、反射类名、选择DataSource、DataSource自定义属性的填写。
    /// 
    /// </summary>
    public class DataSourceRule
    {
        #region --> Conformation Method

        /// <summary>
        /// 构造方法
        /// </summary>
        public DataSourceRule()
        {
            //注册事件。
            Utility.Instance.SqlSetMethodEvent += new DelegateSqlSetMethod(SqlGetOrSetEvent);

        }

        #endregion --> Conformation Method

        #region --> Methods.

        /// <summary>
        /// 把TextBox中的Sql语句设置到DataSource上去。
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">针对SQL语句定义的事个事件类。</param>
        void SqlGetOrSetEvent(object sender, SqlSentenceEventArgs e)
        {
            if (e != null)
            {
                if (e.Flag == EventFlag.SetToDataSourceSQL)
                {
                    #region
                    try
                    {
                        if (targetDataSource == null)
                        {
                            return;
                        }
                        Type t = targetDataSource.GetType();
                        PropertyInfo[] piList = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        if (piList == null || piList.Length == 0)
                        {
                            return;
                        }
                        PropertyValueCollection pvcList = new PropertyValueCollection();
                        pvcList.ClassName = this.GetType().Name;
                        pvcList.FlagName = DataSourceTypeName;
                        pvcList.DefineName = ComboBoxDataSource;
                        bool IsExistSQL = false;
                        foreach (PropertyInfo pi in piList)
                        {
                            try
                            {
                                Attribute attr = Attribute.GetCustomAttribute(pi, typeof(FilterAttribute));
                                if (attr != null)
                                {
                                    string pName = pi.Name;
                                    if (pi.Name.ToLower() == "sql")
                                    {
                                        pi.SetValue(targetDataSource, e.SqlSentence, null);
                                        IsExistSQL = true;
                                        break;
                                    }
                                }
                            }
                            catch (Exception pie)
                            {
                                //WriteLog.Log_Error(pie);
                                Utility.Instance.ConsoleWriteLine(pie, LogFlag.Error);
                            }
                        }
                        if (!IsExistSQL)
                        {

                            //WriteLog.Log_Error("当前选择的数据源[ComboBoxDataSource]类中没有[sql]属性");
                            Utility.Instance.ConsoleWriteLine("当前选择的数据源[ComboBoxDataSource]类中没有[sql]属性", LogFlag.Info);
                            MessageBox.Show("当前选择的数据源[ComboBoxDataSource]类中没有[sql]属性。", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            PropertyHelper.Add(pvcList);
                        }
                    }
                    catch (Exception ex)
                    {
                        //WriteLog.Log_Error(ex);
                        Utility.Instance.ConsoleWriteLine(ex, LogFlag.Error);
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 重写ToString()方法.
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            if (String.IsNullOrEmpty(DataSourceName))
            {
                return "";
            }
            else
            {
                return DataSourceName;
            }
        }

        /// <summary>
        /// 获取用户自定义数据源属性
        /// </summary>
        /// <param name="className">类名称</param>
        private void GetUserDefinedDataSourcePropertyMethod(string className)
        {
            try
            {
                DllClassProperty dcp = null;
                List<DllClassProperty> dcpList = Utility.Instance.ClassPropertyDataSourceList;

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
                    DataSourceTypeName = dcp.FullName + "," + dcp.AssemblyName;
                    targetDataSource = Activator.CreateInstance(dcp.DllClassType);
                }
            }
            catch (Exception ee)
            {
                Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
            }
        }

        #endregion --> Methods.

        #region --> Property.

        /// <summary>
        /// 数据源名称。
        /// </summary>
        private string _DataSourceName;
        /// <summary>
        /// 数据源名称。
        /// </summary>
        [XmlAttribute(),
        DisplayName("数据源名称"),
        Description("数据源名称"),
        Category("属性设置")]
        public string DataSourceName
        {
            get { return _DataSourceName; }
            set
            {
                _DataSourceName = value;
                Utility.Instance.JudgeDataSourceIsExist(_DataSourceName);
            }
        }

        /// <summary>
        /// 反射类名
        /// </summary>
        private string _DataSourceTypeName;
        /// <summary>
        /// 反射类名
        /// </summary>
        [XmlAttribute(),
        DisplayName("反射类名"),
        Description("反射类名"),
        Category("属性设置")]
        public string DataSourceTypeName
        {
            get { return _DataSourceTypeName; }
            set { _DataSourceTypeName = value; }
        }

        /// <summary>
        /// 选择DataSource
        /// </summary>
        private string _ComboBoxDataSource;
        /// <summary>
        /// 选择DataSource
        /// </summary>
        [XmlAttribute(),
        Description("自定义属性"),
        DisplayName("选择DataSource"),
        Category("属性设置"),
        TypeConverter(typeof(TypeConvertPropertyGridDataSource))]
        public string ComboBoxDataSource
        {
            get { return _ComboBoxDataSource; }
            set
            {
                if (String.IsNullOrEmpty(DataSourceName))
                {
                    MessageBox.Show("请输入数据源名称。");
                }
                else
                {
                    _ComboBoxDataSource = value;

                    GetUserDefinedDataSourcePropertyMethod(_ComboBoxDataSource);
                    Utility.Instance.RefurbishPropertyGrid(targetDataSource,new RefurbishPropertyGridEventArgs(true,targetDataSource));
                }
            }
        }
        
        /// <summary>
        /// DataSource自定义属性
        /// </summary>
        Object targetDataSource;
        /// <summary>
        /// DataSource自定义属性
        /// </summary>
        [Filter(),
        DisplayName("DataSource自定义属性"),
        Category("自定义"),
        TypeConverter(typeof(ExpandableObjectConverter))]
        public object TargetDataSource
        {
            get
            {
                if (targetDataSource != null)
                {
                    if (Utility.Instance.IsModify)
                    {
                        PropertyHelper.Restore(_PropertyValues, targetDataSource);
                    }
                    else
                    {
                        PropertyHelper.Save(targetDataSource, this.GetType().Name, DataSourceTypeName, ComboBoxDataSource);
                        _PropertyValues = PropertyHelper.GetPropertyList(this.GetType().Name, DataSourceTypeName, ComboBoxDataSource);
                    }
                }
                return targetDataSource;
            }
            set { targetDataSource = value; }
        }

        /// <summary>
        /// 键值对
        /// </summary>
        private List<PropertyValue> _PropertyValues = new List<PropertyValue>();
        /// <summary>
        /// 键值对
        /// </summary>
        [DisplayName("键值对"),
        Description("键值对"),
        Category("自定义")]
        public List<PropertyValue> PropertyValues
        {
            get { return _PropertyValues; }
            set { _PropertyValues = value; }
        }

        #endregion --> Property.

    }
}
