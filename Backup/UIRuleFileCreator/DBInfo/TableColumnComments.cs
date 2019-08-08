using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.UIRuleFileCreator
{
    /// <summary>
    /// 此类是对数据表 all_col_comments的一个映射关系。
    /// </summary>
    public class TableColumnComments
    {
        /// <summary>
        /// 拥有者
        /// </summary>
        private string _Owner;
        /// <summary>
        /// 拥有者
        /// </summary>
        public string Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }
        /// <summary>
        /// 表名称
        /// </summary>
        private string _TableName;
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        /// <summary>
        /// 表列
        /// </summary>
        private string _ColumnName;
        /// <summary>
        /// 表列
        /// </summary>
        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }
        /// <summary>
        /// 说明
        /// </summary>
        private string _Comments;
        /// <summary>
        /// 说明
        /// </summary>
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
    }
}
