using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Convertors
{
    using AFC.WS.UI.Common;
    using System.Collections;
    using System.Data;

    /// <summary>
    /// author:wangdx
    /// date:20100414 am
    /// 列表转换器 将List<T>转成DataTable
    /// 或者将DataTable转成List<T>
    /// </summary>
    public class ListObjectConvertor :IObjectConvetor
    {

        private Dictionary<string, string> dict = new Dictionary<string, string>();

        #region IObjectConvetor 成员

        /// <summary>
        /// 将List泛型 数据转换成DataTable
        /// </summary>
        /// <param name="data">参数对象</param>
        /// <returns>返回DataTable数据</returns>
        public System.Data.DataTable ConvertObjectToDataTable(object data)
        {
            if (data == null)
            {
                WriteLog.Log_Error(this.GetType().ToString() + " ConvertObjectToDataTable params error data=[null]");
                return null;
            }
            if (!(data is IList))
            {
                WriteLog.Log_Error(this.GetType().ToString() + " ConvertObjectToDataTable params error data is not list type!");
                return null;
            }
            List<object> childList = new List<object>();
            IList dataList = data as IList;
            foreach (var temp in dataList)
            {
                childList.Add(temp);
            }
            Type childType=typeof(object);
            if (childList.Count == 0)
            {
                String FullTypeString = data.GetType().FullName;
                FullTypeString = FullTypeString.Remove(0, FullTypeString.IndexOf("[[") + 2);
                string childTypeName=FullTypeString.Substring(0,FullTypeString.LastIndexOf("Version")-2);
                childType =Type.GetType(childTypeName);
            }
            else
            {
                childType = childList[0].GetType();
            }

            if (childType == typeof(string)
                || childType == typeof(int)
                || childType == typeof(uint)
                ||childType==typeof(ushort)
                ||childType==typeof(char)
                ||childType==typeof(byte)
                ||childType==typeof(DateTime)
                ||childType==typeof(short)
                ||childType==typeof(byte[])
               )
            {
                WriteLog.Log_Info("child type is [" + childType.ToString()+"]");
                DataTable simpleTable = new DataTable();
                DataColumn simpledc = new DataColumn();
                object[] description = data.GetType().GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (description != null && description.Length > 0)
                {
                   this.dict.Add(childType.Name, (description[0] as System.ComponentModel.DescriptionAttribute).Description);
                }
                else
                {
                this.dict.Add(childType.Name,childType.Name);//if not config DescriptionAttribute the chinese name is ColumnName
                }
                simpledc.ColumnName =childType.Name;
                simpleTable.Columns.Add(simpledc);

                for (int i = 0; i < childList.Count; i++)
                {
                    if (childList[i] is byte[])
                    {
                        byte[] aa = childList[i] as byte[];
                        StringBuilder sb = new StringBuilder();
                        for (int ii = 0; ii < aa.Length; ii++)
                        {
                            sb.Append(aa[ii].ToString());
                        }
                        simpleTable.Rows.Add(new object[] { sb.ToString() });
                    }
                    else
                    {
                        simpleTable.Rows.Add(new object[] { childList[i] });
                    }
                }
                return simpleTable;
            }
            List<DataColumn> columnList=ObjectConvertUtil.CreateDataColumn(childType, out dict);

            List<DataRow> rowList = new List<DataRow>();

            for (int i = 0; i < childList.Count; i++)
            {
                rowList.Add(ObjectConvertUtil.CreateDataRow(childType,childList[i],columnList));
            }

            DataTable dt = new DataTable();

            for (int i = 0; i < columnList.Count; i++)
            {
                dt.Columns.Add(new DataColumn(columnList[i].ColumnName, typeof(string)));
            }

            for (int i = 0; i < rowList.Count; i++)
            {
                dt.Rows.Add(rowList[i].ItemArray);
            }
            return dt;
        }

        /// <summary>
        /// 将DataTable展成List对象
        /// </summary>
        /// <param name="dt">数据表对象</param>
        /// <param name="type">列表中的对象</param>
        /// <returns>返回转换之后的List对象</returns>
        public object ConvertDataTableToObject(System.Data.DataTable dt, string type)
        {
            List<object> list = new List<object>();
            if (dt.Columns.Contains("rowNumber"))
            {
                dt.Columns.Remove("rowNumber");
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object child = Activator.CreateInstance(Type.GetType(type));
                object data = null;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    System.Reflection.FieldInfo fi = child.GetType().GetField(dt.Columns[j].ColumnName);
                    if (fi.FieldType.FullName == "System.Byte[]")
                    {
                        string value = dt.Rows[i][dt.Columns[j].ColumnName].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            string[] array = value.Split('.');
                            byte[] buffer = new byte[array.Length];
                            for (int k = 0; k < buffer.Length; k++)
                            {
                                buffer[k] = byte.Parse(array[k]);
                            }
                            data = buffer;
                            fi.SetValue(child, data);
                        }
                    }
                    else
                    {
                         data = Util.ParseFieldValue(fi, dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                    }
                    fi.SetValue(child, data);

                }
                list.Add(child);
            }
            if (!dt.Columns.Contains("rowNumber"))
            {
                dt.Columns.Add("rowNumber");
            }
            return list ;
        }

        public Dictionary<string, string> ValueDescriptionDict
        {
            get
            {
                return this.dict;
            }
            set
            {
                this.dict = value;
            }
        }

        #endregion
    }

 
}
