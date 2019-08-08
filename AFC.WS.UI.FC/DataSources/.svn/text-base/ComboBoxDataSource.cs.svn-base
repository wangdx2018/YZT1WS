using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AFC.WorkStation.DB;
using AFC.WS.UI.Common;

namespace AFC.WS.UI.DataSources
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ComboBoxDataSource
    {

        /// <summary>
        /// ComboBox和其他控件联动时，继承此接口。
        /// 
        /// 选择数据项时，触发选中事件，调用此接口。
        /// </summary>
        public static DataTable GetBindData(string sql)
        {
            int retCode;
            DataSet dset = Util.DataBase.SqlQuery(out retCode, sql);
            if (retCode != 0)
                return null;
            return dset.Tables[0];
        }
    }
}
