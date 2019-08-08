using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Convertors
{
    using AFC.WS.UI.Common;
    using System.Data;

    /// <summary>
    /// author:wangdx
    /// date:20100414
    /// 简单对象转换器
    /// </summary>
    public class SimpleObjectConvertor:IObjectConvetor
    {

        /// <summary>
        /// 数据表对象
        /// </summary>
        private DataTable dt = null;

        /// <summary>
        /// 数据字典
        /// </summary>
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        #region IObjectConvetor 成员

        /// <summary>
        /// 将对象转换成DataTable
        /// </summary>
        /// <param name="data">对象数据源</param>
        /// <returns>返回转换之后的DataTable对象</returns>
        public System.Data.DataTable ConvertObjectToDataTable(object data)
        {
            List<DataColumn> list = ObjectConvertUtil.CreateDataColumn(data.GetType(), out dict);
            if (list == null)
                return null;
            dt = new DataTable();
            for (int i = 0; i < list.Count; i++)
            {
                dt.Columns.Add(list[i]);
            }
            DataRow dr = ObjectConvertUtil.CreateDataRow(data.GetType(), data,list);
            if (dr == null)
                return null;
            dt.Rows.Add(dr.ItemArray);
            return dt;
        }

        public object ConvertDataTableToObject(System.Data.DataTable dt, string type)
        {
            return ObjectConvertUtil.ConvertDataTableToObject(dt, Type.GetType(type));
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
