//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.4927
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
    /// 数据库表名称：para_4041_user_role
    /// </summary>
    public class Para4041UserRole
    {
        
        /// <summary>
        /// 版本号
        /// </summary>
        private string _para_version;
        
        /// <summary>
        /// COLUMN: USER_ID
        /// </summary>
        private string _user_id;
        
        /// <summary>
        /// 角色id
        /// </summary>
        private string _role_id;
        
        /// <summary>
        /// 版本号
        /// </summary>
        public string para_version
        {
            get
            {
                return this._para_version;
            }
            set
            {
                this._para_version = value;
            }
        }
        
        /// <summary>
        /// COLUMN: USER_ID
        /// </summary>
        public string user_id
        {
            get
            {
                return this._user_id;
            }
            set
            {
                this._user_id = value;
            }
        }
        
        /// <summary>
        /// 角色id
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
    }
}