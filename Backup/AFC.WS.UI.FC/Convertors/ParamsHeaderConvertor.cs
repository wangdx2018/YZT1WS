using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Convertors
{
    using AFC.WS.UI.Common;
    using System.Data;
    using System.Reflection;
    using System.Collections;
    using AFC.BJComm.Data;
    /// <summary>
    /// author:wangdx
    /// date:2010_04_16
    /// 参数头信息转换器 不对偏移量进行解析，需要上层程序
    /// 对其进行赋值计算
    /// </summary>
    public class ParamsHeaderConvertor:IObjectConvetor
    {

        private Dictionary<string, string> dict = new Dictionary<string, string>();

        #region IObjectConvetor 成员

        public System.Data.DataTable ConvertObjectToDataTable(object data)
        {
            bool res=ObjectConvertUtil.CheckFiledOrderAttribute(data.GetType());
            if(res)
            {
               List<FieldInfo> list=ObjectConvertUtil.GetAfterOrderByFiledInfoList(data.GetType().GetFields());

                 List<FieldInfo> listFilter=new List<FieldInfo>();
               for (int i = 0; i < list.Count; i++)
               {
                   if (list[i].GetCustomAttributes(typeof(PackArrayAttribute), false).Length == 0
                       &&list[i].GetCustomAttributes(typeof(PackBytesAttribute),false).Length==0
                      )
                   {
                       listFilter.Add(list[i]);
                   }
               }
               List<DataColumn> columnList = ObjectConvertUtil.CreateDataColumn(listFilter, out this.dict);
               DataTable dt = new DataTable();
               DataRow dr = ObjectConvertUtil.CreateDataRow(data.GetType(), data, columnList);
               for (int i = 0; i < columnList.Count; i++)
               {
                   dt.Columns.Add(new DataColumn(columnList[i].ColumnName, typeof(string)));
               }
               dt.Rows.Add(dr.ItemArray);

               return dt;
            }
            return null;
        }

        public object ConvertDataTableToObject(System.Data.DataTable dt, string type)
        {
            if (dt.Columns.Contains("rowNumber"))
            {
                dt.Columns.Remove("rowNumber");
            }
            object header = Activator.CreateInstance(Type.GetType(type));
            for (int i = 0; i < dt.Columns.Count; i++)
            {
             FieldInfo fi = header.GetType().GetField(dt.Columns[i].ColumnName);
             object data=Util.ParseFieldValue(fi, dt.Rows[0][dt.Columns[i].ColumnName].ToString());
             fi.SetValue(header, data);
            }
            return header;
        }

        public Dictionary<string, string> ValueDescriptionDict
        {
            get
            {
                return dict;
            }
            set
            {
                dict = value;
            }
        }


      

      

        #endregion
    }
}
