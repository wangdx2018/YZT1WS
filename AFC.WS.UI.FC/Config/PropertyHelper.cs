using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Reflection;
using AFC.WS.UI.Common;
using System.Drawing;
using System.Windows.Forms;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 属性帮助类；
    /// 
    /// 主要用于对键值对集合类的操作还有就是保存自定义属性值，
    /// 
    /// 及恢复序列化中的自定义属性的值。
    /// 
    /// </summary>
    public class PropertyHelper
    {

        #region -->PropertyValueCollection 操作

        /// <summary>
        /// PropertyValueCollection类集合。
        /// </summary>
        public static List<PropertyValueCollection> PropertyValueItem = new List<PropertyValueCollection>();
        
        /// <summary>
        /// 添加PropertyValueCollection类。
        /// </summary>
        /// <param name="pvc"> 键值对集合类.</param>
        public static void Add(PropertyValueCollection pvc)
        {
            if (IsExist(pvc))
            {
                Remove(pvc);
            }
            PropertyValueItem.Add(pvc);
        }
        
        /// <summary>
        /// 判断PropertyValueCollection类是否存在。
        /// </summary>
        /// <param name="pvc"> 键值对集合类.</param>
        /// <returns>true:成功；false:失败</returns>
        private static bool IsExist(PropertyValueCollection pvc)
        {
            bool result = false;

            foreach (PropertyValueCollection obj in PropertyValueItem)
            {
                if (obj.ClassName == pvc.ClassName &&
                    obj.DefineName == pvc.DefineName &&
                    obj.FlagName == pvc.FlagName)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 移除PropertyValueCollection类对象。
        /// </summary>
        /// <param name="pvc"> 键值对集合类.</param>
        private static void Remove(PropertyValueCollection pvc)
        {
            if (pvc == null)
            {
                return;
            }
            PropertyValueCollection temp = null; ;

            foreach (PropertyValueCollection obj in PropertyValueItem)
            {
                if (obj.ClassName == pvc.ClassName &&
                    obj.DefineName == pvc.DefineName &&
                    obj.FlagName == pvc.FlagName)
                {
                    temp = obj;
                    break;
                }
            }
            if (temp != null)
            {
                PropertyValueItem.Remove(temp);
            }
        }
        
        /// <summary>
        /// 获取指定的键值对。
        /// </summary>
        /// <param name="ClassName">类名：ActionProperty、ColumnProperty、ControlProperty、DataSourceRule。</param>
        /// <param name="FlagName">控件名称、绑定字段、数据源名称</param>
        /// <param name="DefineName">自定义名称：Action名称、DataSource名称、Controls名称、Converter名称</param>
        /// <returns>键值对集合</returns>
        public static List<PropertyValue> GetPropertyList(string ClassName, string FlagName, string DefineName)
        {
            List<PropertyValue> pvList = new List<PropertyValue>();

            foreach (PropertyValueCollection pvc in PropertyValueItem)
            {
                if (pvc.FlagName == FlagName &&
                    pvc.DefineName == DefineName &&
                    pvc.ClassName == ClassName)
                {
                    pvList = pvc.PropertyList;
                    break;
                }
            }
              
            return pvList;
        }

        #endregion -->PropertyValueCollection 操作

        #region -->保存和恢复值操作。

        /// <summary>
        /// 恢复值操作
        /// </summary>
        /// <param name="PropertyValues">键值对集合</param>
        /// <param name="targetObj">目标类对象</param>
        public static void Restore(List<PropertyValue> PropertyValues, object targetObj)
        {
            if (targetObj != null)
            {
                Type t = targetObj.GetType();
                PropertyInfo[] piList = t.GetProperties();
                FieldInfo[] fiList = t.GetFields();
                foreach (PropertyInfo pi in piList)
                {
                    Attribute att = Attribute.GetCustomAttribute(pi,typeof(FilterAttribute));

                    if (att != null)
                    {
                        foreach (PropertyValue pv in PropertyValues)
                        {
                            if (pi.Name == pv.Key)
                            {
                                try
                                {
                                    #region -->
                                    object temp = null;
                                    if (pi.PropertyType.BaseType.Name != "Enum")
                                    {
                                        switch (pi.PropertyType.Name)
                                        {                                                
                                           case "Byte":

                                                temp = Convert.ToByte(pv.Value);

                                                break;

                                            case "Char":

                                                temp = Convert.ToChar(pv.Value);

                                                break;

                                            case "DateTime":

                                                temp = Convert.ToDateTime(pv.Value);

                                                break;

                                            case "Decimal":

                                                temp = Convert.ToDecimal(pv.Value);

                                                break;

                                            case "Double":

                                                temp = Convert.ToDouble(pv.Value);

                                                break;

                                            case "Int16": 
                                                
                                                temp = Convert.ToInt16(pv.Value);

                                                break;

                                            case "Int32":

                                                temp = Convert.ToInt32(pv.Value);

                                                break;

                                            case "Int64":

                                                temp = Convert.ToInt64(pv.Value);

                                                break;

                                            case "SByte":

                                                temp = Convert.ToSByte(pv.Value);

                                                break;

                                            case "Single":
                                                temp = Convert.ToSingle(pv.Value);

                                                break;

                                            case "UInt16":

                                                temp = Convert.ToUInt16(pv.Value);

                                                break;

                                            case "UInt32":

                                                temp = Convert.ToUInt32(pv.Value);

                                                break;

                                            case "UInt64":
                                                
                                                temp = Convert.ToUInt64(pv.Value);

                                                break;

                                            default:

                                                temp = pv.Value;

                                                break;

                                        }
                                    #endregion -->
                                        pi.SetValue(targetObj, temp, null);
                                        GetTargetDataSourceSqlSentence(pv.Key, pv.Value,targetObj);
                                    }
                                    else
                                    {
                                        Type aaaa = pi.PropertyType.ReflectedType;
                                    }
                                    break;
                                }
                                catch (Exception ee)
                                {
                                    Utility.Instance.ConsoleWriteLine(ee, LogFlag.Error);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="targetObj">目标对象</param>
        /// <param name="ClassName">类名：ActionProperty、ColumnProperty、ControlProperty、DataSourceRule。</param>
        /// <param name="FlagName">控件名称、绑定字段、数据源名称</param>
        /// <param name="DefineName">自定义名称：Action名称、DataSource名称、Controls名称、Converter名称</param>
        public static void Save(object targetObj, string ClassName, string FlagName, string DefineName)
        {
            try
            {
                if (targetObj == null)
                {
                    return;
                }
                Type t = targetObj.GetType();
                PropertyInfo[] piList = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                if (piList == null || piList.Length == 0)
                {
                    return;
                }
                PropertyValueCollection pvcList = new PropertyValueCollection();
                pvcList.ClassName = ClassName;
                pvcList.DefineName = DefineName;
                pvcList.FlagName = FlagName;

                foreach (PropertyInfo pi in piList)
                {
                    try
                    {
                        Attribute attr = Attribute.GetCustomAttribute(pi, typeof(FilterAttribute));
                        if (attr != null)
                        {
                            PropertyValue fp = new PropertyValue();
                            string pName = pi.Name;
                            fp.Key = pName;
                            object pValue = pi.GetValue(targetObj, null);
                            if (pValue != null)
                            {
                                //获取数据源里的查询语句。
                                GetTargetDataSourceSqlSentence(pName, pValue,targetObj);
                                #region --> 
                                //if (pValue is IList<string>)
                                //{
                                //    List<string> temp = pValue as List<string>;
                                //    if (temp != null)
                                //    {
                                //        System.Text.StringBuilder sb = new StringBuilder();
                                //        for (int i = 0; i < temp.Count; i++)
                                //        {
                                //            sb.Append(temp[i]);
                                //            sb.Append(",");
                                //        }
                                //        fp.Value = sb.ToString();
                                //    }
                                //}
                                //else
                                //{
                                //    fp.Value = pValue.ToString();
                                //}
                                //Add(fp);
                                #endregion -->
                                fp.Value = pValue.ToString();
                            }
                            pvcList.Add(fp);
                        }
                    }
                    catch (Exception pie)
                    {
                        Utility.Instance.ConsoleWriteLine(pie, LogFlag.Error);
                    }
                }
                Add(pvcList);
            }
            catch (Exception e)
            {
                Utility.Instance.ConsoleWriteLine(e, LogFlag.Error);
            }
        }
  
        /// <summary>
        /// 获取数据源里的查询语句。
        /// </summary>
        /// <param name="pName">数据源里的自定义属性字段</param>
        /// <param name="pValue">自定义属性的值</param>
        /// <param name="targetObj">目标对象</param>
        private static void GetTargetDataSourceSqlSentence(string pName,object pValue,object targetObj)
        {
            if (pName.ToLower() == "sql" ||
                pName.ToLower() == "groupparams" ||
                pName.ToLower() == "havingparams" ||
                pName.ToLower() == "whereparams" ||
                pName.ToLower() == "orderbyparams"
                )
            {
                if (pValue != null)
                {
                    Utility.Instance.SqlSetMethod(pName,
                        new SqlSentenceEventArgs(EventFlag.SetToTextBox, targetObj,
                            pName.ToLower() == "sql" ? pValue.ToString() : ""));
                }
            }
            else if (pName.ToLower() == "contentdatepicker")
            {
                if (pValue != null)
                {
                    Utility.Instance.InitValueContent(pValue.ToString());
                }
            }
        }

        #endregion 
        

        #region --> Set Property Value.

        /// <summary>
        /// 设备属性是否可以显示。
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="visible">true:可以显示;false:不可以显示</param>
        public static void SetPropertyVisibility(object obj, string propertyName, bool visible)
        {
            Type type = typeof(BrowsableAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;
            FieldInfo fld = type.GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic);
            fld.SetValue(attrs[type], visible);
        }
        /// <summary>
        /// 设备属性是否为可读。
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="readOnly">true:不可以修改；false:为可以修改</param>
        public static void SetPropertyReadOnly(object obj, string propertyName, bool readOnly)
        {
            Type type = typeof(System.ComponentModel.ReadOnlyAttribute);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(obj);
            AttributeCollection attrs = props[propertyName].Attributes;
            FieldInfo fld = type.GetField("isReadOnly", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance);
            fld.SetValue(attrs[type], readOnly);
        }

        #endregion --> Set Property Value。
    }
}
