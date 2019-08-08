using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.UIRuleFileCreator
{
    /// <summary>
    /// 此类是获取表中字段的属性。
    /// </summary>
    public class TableFieldProperty
    {
        string _TableName;
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        /// <summary>
        /// 列名
        /// </summary>
        private string _ColumnName;
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }
        /// <summary>
        /// 字段类型
        /// </summary>
        private string _DateType;
        /// <summary>
        /// 字段类型
        /// </summary>
        public string DateType
        {
            get { return _DateType; }
            set { _DateType = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        private string _Comments;
        /// <summary>
        /// 备注
        /// </summary>
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }

        /// <summary>
        /// 绑定字段。
        /// </summary>
        private string _BindingData;
        /// <summary>
        /// 绑定字段。
        /// </summary>
        public string BindingField
        {
            get { return _BindingData; }
            set { _BindingData = value; }
        }
    }
}
