using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Convertors
{
    using AFC.WS.UI.Common;
    using System.Data;
    using System.Collections;
    using System.Reflection;

    public class ComplexDataConvetor:IObjectConvetor
    {
        #region IObjectConvetor 成员


        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public System.Data.DataTable ConvertObjectToDataTable(object data)
        
        {
            List<DataColumn> list = AFC.WS.UI.Common.ComplexObjectConvertUtil.GetDataList(data.GetType());
            
            if (list == null || list.Count == 0)
            {
                WriteLog.Log_Error("Create dataColumn error: type=[" + data.GetType().ToString() + "]");
                return null;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (!dict.ContainsKey(list[i].ColumnName))
                {
                    dict.Add(list[i].ColumnName, list[i].ColumnName);
                }
                else
                {
                    dict.Add(list[i].ColumnName + i.ToString(), list[i].ColumnName + i.ToString());
                    list[i].ColumnName = list[i].ColumnName + i.ToString();
                }
            }

           List<object[]> rowList=ComplexObjectConvertUtil.CreateRowList(data);

           DataTable dt = new DataTable();

           for (int i = 0; i < list.Count; i++)
           {
               dt.Columns.Add(list[i]);
           }

           if (rowList == null || rowList.Count == 0)
               return dt;

          
           for (int i = 0; i < rowList.Count; i++)
           {
               dt.Rows.Add(rowList[i]);
           }

           return dt;
    
        }

        public virtual object ConvertDataTableToObject(System.Data.DataTable dt, string type)
        {
            return null;
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
