//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.9145
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AFC.WS.Model.DB
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    
    
    /// <summary>
    /// 数据库表名称：priv_role_function_info
    /// </summary>
    public class PrivRoleFunctionInfo
    {
        
        /// <summary>
        /// COLUMN: ROLE_ID
        /// </summary>
        private string _role_id;
        
        /// <summary>
        /// COLUMN: FUNCTION_ID
        /// </summary>
        private string _function_id;
        
        /// <summary>
        /// COLUMN: UPDATE_DATE
        /// </summary>
        private string _update_date;
        
        /// <summary>
        /// COLUMN: UPDATE_TIME
        /// </summary>
        private string _update_time;
        
        /// <summary>
        /// COLUMN: OPERATOR_ID
        /// </summary>
        private string _operator_id;
        
        /// <summary>
        /// COLUMN: ROLE_ID
        /// </summary>
        public string role_id
        {
            get
            {
                return this._role_id;
            }
            set
            {
                this._role_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: FUNCTION_ID
        /// </summary>
        public string function_id
        {
            get
            {
                return this._function_id;
            }
            set
            {
                this._function_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: UPDATE_DATE
        /// </summary>
        public string update_date
        {
            get
            {
                return this._update_date;
            }
            set
            {
                this._update_date = value;
            }
        }
        
        /// <summary>
        /// COLUMN: UPDATE_TIME
        /// </summary>
        public string update_time
        {
            get
            {
                return this._update_time;
            }
            set
            {
                this._update_time = value;
            }
        }
        
        /// <summary>
        /// COLUMN: OPERATOR_ID
        /// </summary>
        public string operator_id
        {
            get
            {
                return this._operator_id;
            }
            set
            {
                this._operator_id = value;
            }
        }
    }
}
