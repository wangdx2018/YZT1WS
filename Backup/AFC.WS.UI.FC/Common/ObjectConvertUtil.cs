using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Common
{
    using System.Reflection;
    using System.Data;
    using AFC.BJComm.Data;
    using AFC.BJComm;
    using AFC.WS.UI.Common;
    using System.Collections;
   
    /// <summary>
    /// author：wangdx
    /// date:20100414
    /// 该类封装了对象数组转换器通用处理
    /// </summary>
    internal class ObjectConvertUtil
    {

        /// <summary>
        /// 创建数据列
        /// </summary>
        /// <param name="type">数据类型</param>
        /// <param name="valueDescriptionDict">字典描述</param>
        /// <returns>返回DataColumn集合</returns>
        public static List<DataColumn> CreateDataColumn(Type type,  out Dictionary<string, string> valueDescriptionDict)
        {
            valueDescriptionDict = new Dictionary<string, string>();
            bool res = CheckFiledOrderAttribute(type); //check wheather config PackOrderAttribute
            if (!res)
            {
                valueDescriptionDict = null;
                return null;
            }
            List<FieldInfo> list = GetAfterOrderByFiledInfoList(type.GetFields());//get after order by FiledList
            return CreateDataColumn(list, out valueDescriptionDict);
           
        }

        /// <summary>
        /// 创建列
        /// </summary>
        /// <param name="list">字段信息列表</param>
        /// <param name="valueDescriptionDict">字典</param>
        /// <returns>返回DataColumn列表</returns>
        public static List<DataColumn> CreateDataColumn(List<FieldInfo> list, out Dictionary<string, string> valueDescriptionDict)
        {
            valueDescriptionDict = new Dictionary<string, string>();
            List<DataColumn> dataColumnList = new List<DataColumn>();
            for (int i = 0; i < list.Count; i++)
            {
                DataColumn dc = new DataColumn();
                object[] description = list[i].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (description != null && description.Length > 0)
                {
                    valueDescriptionDict.Add(list[i].Name, (description[0] as System.ComponentModel.DescriptionAttribute).Description);
                }
                else
                {
                    valueDescriptionDict.Add(list[i].Name, list[i].Name);//if not config DescriptionAttribute the chinese name is ColumnName
                }
                dc.ColumnName = list[i].Name;
                dataColumnList.Add(dc);
                //valueDescriptionDict.Add(list[i].Name, dc.ColumnName);
            }
            return dataColumnList;
        }

        /// <summary>
        /// 创建DataRow
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="resource">对象资源</param>
        /// <returns>返回DataRow集合</returns>
        public static DataRow CreateDataRow(Type type, object resource, List<DataColumn> dataColumnList)
        {
            if (!CheckFiledOrderAttribute(type)) //check weather config the order attribute
                return null;
            List<FieldInfo> list = GetAfterOrderByFiledInfoList(type.GetFields());//order by packOrderAttribute order params
           // return null;
            DataTable dt = new DataTable();
            for (int i = 0; i < dataColumnList.Count; i++)
            {
                dt.Columns.Add(new DataColumn(dataColumnList[i].ColumnName,typeof(string)));
            }
            DataRow dr = dt.NewRow();

            for (int i = 0; i < dataColumnList.Count; i++)
            {
                object temp=type.GetField(dataColumnList[i].ColumnName).GetValue(resource);
                object[] attributes=type.GetField(dataColumnList[i].ColumnName).GetCustomAttributes(typeof(PackBytesAttribute),true);
                if(attributes!=null&&attributes.Length>0&&(attributes[0] as PackBytesAttribute).byteSize>1)
                {
                    byte[] array = (byte[])temp;
                    StringBuilder sb = new StringBuilder();
                    for (int ii = 0; ii < array.Length; ii++)
                    {
                        sb.Append(array[ii].ToString("d2"));
                        if (ii < array.Length - 1)
                        {
                            sb.Append(".");
                        }
                    }
                    dr[i] = sb.ToString();
                }
                else
                    dr[i] = temp;
            }
            return dr;
        }

        /// <summary>
        /// 检查是否是所有字段都配置了PackOrder 特性
        /// </summary>
        /// <param name="t">Type t</param>
        /// <returns>有返回true，否则返回false</returns>
        public static bool CheckFiledOrderAttribute(Type t)
        {
            FieldInfo[] fieldCollection=t.GetFields();
            for (int i = 0; i < fieldCollection.Length; i++)
            {
                object[] temp = fieldCollection[i].GetCustomAttributes(typeof(PackOrderAttribute), false);
                if (temp == null || temp.Length == 0)
                {
                    WriteLog.Log_Error(t.GetType().ToString() + " " + fieldCollection[i].Name + " not set PackOrderAttribute!");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 得到packOrderAttribute 排序之后的FiledInfo集合
        /// </summary>
        /// <param name="list">字段排序集合</param>
        /// <returns>返回排序之后的集合</returns>
        public static List<FieldInfo> GetAfterOrderByFiledInfoList(FieldInfo[] list)
        {
            var collection = from temp in list
                             orderby (temp.GetCustomAttributes(typeof(PackOrderAttribute), false)[0] as PackOrderAttribute).order
                             select temp;
            return collection.ToList();

        }

        /// <summary>
        /// 将数据转换成Object对象，参数t不是IEnumable接口类型
        /// 不能为IList，Array等。（为简单的数据类型，不支持复杂嵌套类型）
        /// </summary>
        /// <param name="dt">数据表DataTable</param>
        /// <param name="t">需要转换的对象类型</param>
        /// <returns>返回转换之后的object对象</returns>
        public static object ConvertDataTableToObject(DataTable dt,Type t)
        {
            if(dt==null||t==null)
            {
                WriteLog.Log_Error("params error ! dt="+(dt==null?"dt==null":"valid")+"type="+t==null?"t==null":"valid");
                return null;
            }
            if (dt.Columns.Contains("rowNumber"))
            {
                dt.Columns.Remove("rowNumber");
            }

            object convertObject = Activator.CreateInstance(t);
            if (convertObject == null)
            {
                WriteLog.Log_Error("Create object error! type=[" + t.GetType().ToString() + "]");
                return null;
            }
            if (convertObject is IEnumerable || convertObject is IList)
            {
                WriteLog.Log_Error("object type errror! type is IEnumerable or IList interface ! " + " type=" + "[" + t.GetType().ToString() + "]");
                return null;
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                FieldInfo fi = convertObject.GetType().GetField(dt.Columns[i].ColumnName);
                if (fi == null)
                {
                    WriteLog.Log_Error("object type=[" + convertObject.ToString() + "] does not have " + dt.Columns[i].ColumnName + " field");
                    return null;
                }
                if (fi.FieldType.FullName == "System.Byte[]")
                {
                    string[] array = dt.Rows[0][dt.Columns[i].ColumnName].ToString().Split('.');
                    if (array != null && array.Length > 0)
                    {
                        byte[] buffer = new byte[array.Length];
                        //object data = Util.ParseFieldValue(fi, dt.Rows[0][dt.Columns[i].ColumnName].ToString());
                        fi.SetValue(convertObject, buffer);
                    }
                }
                else
                {

                    object data = Util.ParseFieldValue(fi, dt.Rows[0][dt.Columns[i].ColumnName].ToString());
                    fi.SetValue(convertObject, data);
                }
                //fi.SetValue(convertObject,dt.Rows[0][dt.Columns[i].ColumnName]);
            }
            return convertObject;


        }





    }


    public static class ComplexObjectConvertUtil
    {
        public static List<DataColumn> GetDataList(Type t)
        {
            List<DataColumn> list = new List<DataColumn>();

            //todo:order by Packorder first 

            if (t==typeof(uint)||
                t==typeof(string)||
                t==typeof(int)||
                t==typeof(byte)||
                t==typeof(char)
                )
            {
                list.Add(new DataColumn(t.Name));
             
                return list;
            }

            if(t.FullName.Contains("System.Collections.Generic.List`1"))
            {
                String FullTypeString = t.FullName;
                FullTypeString = FullTypeString.Remove(0, FullTypeString.IndexOf("[[") + 2);
                string childTypeName = FullTypeString.Substring(0, FullTypeString.LastIndexOf("Version") - 2);
                list.AddRange(GetDataList(Type.GetType(childTypeName)));
            }

            foreach (var temp in t.GetFields())
            {
                if (temp.FieldType.FullName.Contains("System.Collections.Generic.List`1"))
                {
                    Type child = GetListChildFulType(temp);
                    list.AddRange(GetDataList(child));
                }

                if (!temp.FieldType.FullName.Contains("System.Collections.Generic.List`1") &&
                    temp.FieldType.IsClass &&
                    temp.FieldType != typeof(string))
                {

                    foreach (var dd in temp.FieldType.GetFields())
                    {
                        if (dd.FieldType == typeof(int) ||
                            dd.FieldType == typeof(uint) ||
                            dd.FieldType == typeof(string) ||
                            dd.FieldType == typeof(Enum) ||
                            dd.FieldType == typeof(byte) ||
                            dd.FieldType == typeof(ushort) ||
                            dd.FieldType == typeof(short))
                        {
                            list.Add(new DataColumn(dd.Name));
                            object[] description = dd.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                         
                        }
                        else
                        {
                            list.AddRange(GetDataList(dd.FieldType));

                        }
                    }
                }

                if (temp.FieldType == typeof(int)
                    || temp.FieldType == typeof(uint)
                    || temp.FieldType == typeof(string)
                    || temp.FieldType == typeof(Enum)
                    || temp.FieldType == typeof(byte)
                    || temp.FieldType == typeof(ushort)
                    || temp.FieldType == typeof(short))
                {
                    list.Add(new DataColumn(temp.Name));
                
                }
            }

            return list;
        }


        public static List<object[]> CreateRowList(object data)
        {
            bool isExistList = false;

            List<object[]> rowCollection = new List<object[]>();

            List<object> list = new List<object>();

            List<object[]> tempRowCollection = new List<object[]>();//for mul row collection temp saved

            if (data.GetType() == typeof(string) ||
                data.GetType() == typeof(int) ||
                data.GetType() == typeof(uint) ||
                data.GetType() == typeof(short) ||
                data.GetType() == typeof(ushort) ||
                data.GetType() == typeof(byte))
            {
                list.Add(data);
                tempRowCollection.Add(list.ToArray());
                return tempRowCollection;
            }
            if (data.GetType().FullName.Contains("System.Collections.Generic.List`1"))
            {
                foreach (var aa in (data as IList))
                {
                    tempRowCollection.AddRange(CreateRowList(aa));
                }
                return tempRowCollection;
            }

            foreach (var temp in data.GetType().GetFields())
            {
                //todo: single type
                #region simple type handle
                if (temp.FieldType == typeof(uint) ||
                    temp.FieldType == typeof(ushort) ||
                    temp.FieldType == typeof(string) ||
                    temp.FieldType == typeof(byte) ||
                    temp.FieldType == typeof(ushort) ||
                    temp.FieldType == typeof(int) ||
                    temp.FieldType == typeof(Enum))
                {
                    if (tempRowCollection.Count == 0) //chck if the first time
                    {
                        list.Add(temp.GetValue(data));
                    }
                    else //add other data here
                    {
                        for (int i = 0; i < tempRowCollection.Count; i++)
                        {
                            List<object> aab = new List<object>();
                            aab.AddRange(tempRowCollection[i].ToList());
                            aab.Add(temp.GetValue(data));
                            tempRowCollection[i] = aab.ToArray();
                        }
                    }
                }
                #endregion

                #region struct data handle
                //todo: struct data
                if (!temp.FieldType.FullName.Contains("System.Collections.Generic.List`1") &&
                   temp.FieldType.IsClass &&
                   temp.FieldType != typeof(string))
                {
                    //todo:order by fileds
                    foreach (var child in temp.FieldType.GetFields())
                    {
                        if (child.FieldType == typeof(uint) ||
                         child.FieldType == typeof(ushort) ||
                         child.FieldType == typeof(string) ||
                         child.FieldType == typeof(byte) ||
                         child.FieldType == typeof(ushort) ||
                         child.FieldType == typeof(int) ||
                         child.FieldType == typeof(Enum))
                        {
                            if (tempRowCollection.Count == 0) //chck if the first time
                            {
                                list.Add(child.GetValue(temp.GetValue(data)));
                            }
                            else //add other data here
                            {
                                for (int i = 0; i < tempRowCollection.Count; i++)
                                {
                                    List<object> objectList = tempRowCollection[i].ToList();
                                    objectList.Add(child.GetValue(temp.GetValue(data)));
                                    tempRowCollection[i] = objectList.ToArray();
                                }
                            }
                        }
                        else
                        {
                            if (tempRowCollection.Count == 0)
                            {
                                tempRowCollection.Add(list.ToArray());
                            }
                            object[][] beforeData = tempRowCollection.ToArray();
                            tempRowCollection.Clear();
                            List<object[]> childrenRowCollection = CreateRowList(child.GetValue(temp.GetValue(data)));//get all children data
                            for (int i = 0; i < beforeData.Length; i++)
                            {
                                for (int j = 0; j < childrenRowCollection.Count; j++)
                                {
                                    List<object> childItem = beforeData[i].ToList();
                                    childItem.AddRange(childrenRowCollection[j].ToList());
                                    tempRowCollection.Add(childItem.ToArray());
                                }
                            }
                        }
                    }
                }
                #endregion

                #region list data handle
                //todo:list data
                if (temp.FieldType.FullName.Contains("System.Collections.Generic.List`1"))
                {
                    IList childCollection = temp.GetValue(data) as IList;
                    if (tempRowCollection.Count == 0)
                    {
                        tempRowCollection.Add(list.ToArray());
                    }
                    if (childCollection.Count == 0)
                        continue;

                    object[][] beforeData = tempRowCollection.ToArray();
                    tempRowCollection.Clear();

                    foreach (var child in childCollection)
                    {
                        #region Simple type handler
                        if (child.GetType() == typeof(uint) ||
                             child.GetType() == typeof(ushort) ||
                             child.GetType() == typeof(string) ||
                             child.GetType() == typeof(byte) ||
                             child.GetType() == typeof(ushort) ||
                             child.GetType() == typeof(int) ||
                             child.GetType() == typeof(Enum))
                        {
                            for (int i = 0; i < beforeData.Length; i++)
                            {
                                List<object> itemList = new List<object>();
                                itemList = beforeData[i].ToList();
                                itemList.Add(child);
                                tempRowCollection.Add(itemList.ToArray());
                            }
                        }
                        #endregion

                        else
                        {
                            List<object[]> childItems = CreateRowList(child);
                            for (int i = 0; i < beforeData.Length; i++)
                            {
                                for (int j = 0; j < childItems.Count; j++)
                                {
                                    List<object> itemList = new List<object>();
                                    itemList = beforeData[i].ToList();
                                    itemList.AddRange(childItems[j]);
                                    tempRowCollection.Add(itemList.ToArray());
                                }
                            }
                        }
                    }
                }
                #endregion

            }

            if (tempRowCollection.Count == 0)
                tempRowCollection.Add(list.ToArray());
            return tempRowCollection;
        }


        private static Type GetListChildFulType(FieldInfo fi)
        {
            String FullTypeString = fi.FieldType.FullName;
            FullTypeString = FullTypeString.Remove(0, FullTypeString.IndexOf("[[") + 2);
            string childTypeName = FullTypeString.Substring(0, FullTypeString.LastIndexOf("Version") - 2);
            return Type.GetType(childTypeName);
        }
    }
}
