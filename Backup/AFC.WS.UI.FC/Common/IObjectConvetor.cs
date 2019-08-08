using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 数据源对象的接口。可以将Object转成DataTable
    /// 将DataTable转成Object
    /// </summary>
    public interface IObjectConvetor
    {
        /// <summary>
        /// 将Object转成DataTable
        /// </summary>
        /// <param name="data">内存中的对象</param>
        /// <returns>返回DataTable的实例</returns>
        DataTable ConvertObjectToDataTable(object data);

        /// <summary>
        /// 将DataTable转成Object
        /// </summary>
        /// <param name="dt">数据表</param>
        /// <param name="type">需要转成的类型</param>
        /// <returns>返回转换之后的object</returns>
        object ConvertDataTableToObject(DataTable dt, string type);

        /// <summary>
        /// 获得字段名和列名描述的字典
        /// </summary>
        Dictionary<string, string> ValueDescriptionDict
        {
            set;
            get;
        }
    }
}
