using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.DataSources
{
    using AFC.WS.UI.Common;
    using System.Data;


    /// <summary>
    /// author：wangdx  
    /// date:2010-04-14
    /// 默认的对象数据源为基础组件提供
    /// 数据源。
    /// date:2010-04-22新增了
    /// edit by wangdx 20100525 将rownumber提前了
    /// 
    /// edit by wangdx 20100608 将增加了临时DataTable用来处理数据
    /// </summary>
    public class ObjectDataSource:IDataSource
    {

        /// <summary>
        /// 数据源名称
        /// </summary>
        private string dataSourceName;

        /// <summary>
        /// 转换器类型
        /// </summary>
        private string objectConvertorType;

        /// <summary>
        /// 数据字典(key对应的字段名称，value对应的中文名称）
        /// </summary>
        public Dictionary<string, string> dict = new Dictionary<string, string>();

        /// <summary>
        /// 数据源对象
        /// </summary>
        public object dataSource = null;

        /// <summary>
        /// 数据列表
        /// </summary>
        public DataTable dt = null;


        /// <summary>
        /// 上次的清空的数据列表
        /// </summary>
        public DataTable lastDataTable = null;

        /// <summary>
        /// 数据源
        /// </summary>
        private Dictionary<string, SortedType> sortedList = new Dictionary<string, SortedType>();

        /// <summary>
        /// 转换器对象
        /// </summary>
        public IObjectConvetor convertor = null;

        /// <summary>
        /// 该属性需要在规则文件中配置，提供对象转换列表。
        /// 列表转对象的接口。
        /// </summary>
        [Filter()]
        public string ObjectConvetorType
        {
            set { this.objectConvertorType = value; }
            get { return this.objectConvertorType; }
        }

        /// <summary>
        /// 数据源的名称,需要在规则文件中进行配置
        /// </summary>
        [Filter()]
        public string DataSourceName
        {
            get
            {
                return dataSourceName;
            }
            set
            {
                dataSourceName = value;
            }
        }


        #region IDataSource 成员

        public void SetQueryParams(List<string> queryCondition)
        {
            //if (dt.Rows.Count == 0)
            //{
            //    WriteLog.Log_Error(this.ToString() + " SetQueryParams error! " + "the datatable row's count  is zero!");  
            //    return;
            //}
            //if (queryCondition == null || queryCondition.Count == 0)
            //{
            //    WriteLog.Log_Error(this.ToString() + "SetQueryParams error!" + "queryCondition is null or empty!");
            //    return;
            //}
           
        }

        /// <summary>
        /// 设置排序参数，然后通知数据源
        /// </summary>
        /// <param name="sortedName">排序字段</param>
        /// <param name="type">排序类型</param>
        public void SetSortParams(string sortedName, SortedType type)
        {
            if (dt==null||dt.Rows.Count==0)
            {
                WriteLog.Log_Error(this.GetType().ToString() + " datatable row's count is zero");
                return;
            }
            if (string.IsNullOrEmpty(sortedName))
            {
                WriteLog.Log_Error("sortName " + sortedName + " is null or empty");
                return;
            }
            this.sortedList.Clear();//CLEAR THE LAST 
            this.sortedList.Add(sortedName, type);

            DataSourceManager.NotfiyDataSourceChange(this.dataSourceName);
        }

        /// <summary>
        /// 取DataTable的每页数据
        /// </summary>
        /// <param name="pageIndex">每页索引</param>
        /// <param name="pageSize">每页的数量</param>
        /// <returns>返回DataTable</returns>
        public System.Data.DataTable FetchPagingData(int pageIndex, int pageSize)
        {
            if (this.dt==null)
                this.dt = GetDataTable();
            //if (this.dt==null||this.dt.Rows.Count==0)
            //    return null;

            if (pageIndex <= 0) //page index error handle,begin from 1
            {
                WriteLog.Log_Error("params 【pageIndex】  is not valid , value is: [" + pageIndex.ToString() + "]");
                return null;
            }

            if (pageSize > dt.Rows.Count) 
            {
                pageSize = dt.Rows.Count;
                WriteLog.Log_Warn("【pageSize】>dt.Rows.Count,ignore,change pageSize=[" + dt.Rows.Count + "]");
            }

            if (dt.Rows.Count - pageSize * (pageIndex - 1) <0)
            {
                WriteLog.Log_Error(string.Format("params set error: dt.Rows.Count - pageSize * (pageIndex - 1)<=0,count={0},pageSize={1},pageIndex={2}",
                    dt.Rows.Count, pageSize, pageIndex));
                return null;
            }

            DataRow[] data = dt.Copy().AsEnumerable().ToArray();
            DataTable pageTable = dt.Copy();
            pageTable.Clear();
            int restCount = dt.Rows.Count - pageSize * (pageIndex - 1);
            DataRow[] pageCollection = new DataRow[restCount>pageSize?pageSize:restCount];//check the fixed array lenth
            Array.Copy(data, pageSize*(pageIndex-1), pageCollection, 0, pageCollection.Length);

            for (int i = 0; i < pageCollection.Length; i++)
            {
                pageTable.Rows.Add(pageCollection[i].ItemArray);
            }
            return pageTable;
        }

        /// <summary>
        /// 通过Object得到DataTable
        /// </summary>
        /// <returns>返回DataTable对象</returns>
        public System.Data.DataTable GetDataTable()
        {
            if (string.IsNullOrEmpty(objectConvertorType))
            {
                WriteLog.Log_Error("must set【objectConvertType】 property first！");
                return null;
            }
            if(this.dataSource==null)
            {
                WriteLog.Log_Error("call 【SetObject(object source)】 function first");
                return null;
            }
            try
            {
                if (dt != null)
                {
                    return CloneDataTable(dt);
                }
             
                IObjectConvetor convert = Activator.CreateInstance(Type.GetType(objectConvertorType)) as IObjectConvetor;
                this.convertor = convert;
                this.dt = convert.ConvertObjectToDataTable(dataSource);
                DataColumn dc = new DataColumn("rowNumber",typeof(string));
                //this.dict = convert.ValueDescriptionDict;
                if (!this.dict.ContainsKey("rowNumber"))
                {
                    this.dict.Add("rowNumber", "rowNumber");
                }
                foreach (var aa in convert.ValueDescriptionDict)
                {
                    if (!this.dict.ContainsKey(aa.Key))
                    {
                        this.dict.Add(aa.Key, aa.Value);
                    }
                }
                dt.Columns.Add(dc); //add row number column
                dc.SetOrdinal(0);  //set ordinal is zero
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i][0] = i.ToString();
                }
                if (dt == null)
                    return null;
                //this.dict.Add("rowNumber", "rowNumber");

                #region no sorted params handle
                if (this.sortedList.Count==0) //no sorted condition 
                    return CloneDataTable(dt);
                #endregion

                #region have some sorted params handle
                /*that code is having some sorted params handle*/
                if (this.sortedList.ToArray()[0].Value == SortedType.SortAscending)
                {
                    var collection = from temp in this.dt.AsEnumerable()
                                     orderby temp.Field<string>(this.sortedList.ToArray()[0].Key)
                                     select temp;
                    dt=new DataTable();
                    this.dt=collection.CopyToDataTable();
                    return CloneDataTable(dt);
                }
              else
                {
                    var collection = from temp in this.dt.AsEnumerable()
                                     orderby temp.Field<string>(sortedList.ToArray()[0].Key) descending
                                     select temp ;
                    dt = new DataTable();
                    this.dt = collection.CopyToDataTable();
                    return CloneDataTable(dt);
                }
                #endregion
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }
        
        /// <summary>
        /// 获得列表中的数量
        /// </summary>
        /// <returns>成功获得</returns>
        public int Count()
        {
            if (dt == null || this.lastDataTable == null)
                //第一次初始化时候
            {
                dt = GetDataTable();
                this.lastDataTable = CloneDataTable(dt);
                return dt.Rows.Count;
            }

            if (dt.Rows.Count != this.lastDataTable.Rows.Count)//dataTable 发生变化时候
            {
                this.lastDataTable = CloneDataTable(dt);
                return dt.Rows.Count;
            }

            if (!this.DataTableCompare(dt, lastDataTable))
            {
                this.lastDataTable = CloneDataTable(dt);
                return dt.Rows.Count;
            }

            if (dataSource is System.Collections.IList) //数据源变化时候，即对象发生了变化
            {
                System.Collections.IList list = dataSource as System.Collections.IList;
                //if (list.Count == dt.Rows.Count)
                //    return dt.Rows.Count;
                //else
                //{
                    dt = null;//重新加载对象数据源
                    dt = GetDataTable();
                    this.lastDataTable = CloneDataTable(dt);
                //}
            }

            if (this.dt.Rows.Count == this.lastDataTable.Rows.Count)
            {
                return dt.Rows.Count;
            }
          

           
            return dt.Rows.Count;


           
        }

        /// <summary>
        /// 设置对象数据源
        /// </summary>
        /// <param name="data">对象数据源</param>
        public void SetObject(object data)
        {
            if (data != null)
                this.dataSource = data;
        }

        public void Dispose()
        {

        }

        public DataTable CloneDataTable(DataTable dt)
        {
            if (dt == null)
                return null;
            DataTable newDataTable = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                newDataTable.Rows.Add(dt.Rows[i].ItemArray);
            }
            return newDataTable;
        }

        #endregion



        private bool DataTableCompare(DataTable nowDataTable, DataTable lastDataTable)
        {
            if (nowDataTable == null || lastDataTable == null)
                return false;
            if (nowDataTable.Rows.Count != lastDataTable.Rows.Count)
                return false;
            if (nowDataTable.Columns.Count != lastDataTable.Columns.Count)
                return false;
            for (int i = 0; i < nowDataTable.Rows.Count; i++)
            {
                for (int j = 0; j < nowDataTable.Columns.Count; j++)
                {
                    if (nowDataTable.Rows[i][j].Equals(lastDataTable.Rows[i][j]))
                        continue;
                    else
                        return false;
                }
            }
            return true;
        }


    }
}
