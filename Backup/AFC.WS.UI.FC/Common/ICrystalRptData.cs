using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 添加人：王冬欣   20110803
    /// 
    /// 所有报表模板全部实现该接口，将数据向报表数据中赋值
    /// 
    /// </summary>
    public interface ICrystalRptData
    {
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="dict">key 模板中名称，value模板中的参数值</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int SetParamsData(Dictionary<string, string> dict);

        /// <summary>
        /// 设置DataTable数据源
        /// </summary>
        /// <param name="dt">数据表对象</param>
        /// <param name="tableName">表名</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        int SetDataTableData(DataTable dt,string tableName);


        
    }



   
}
