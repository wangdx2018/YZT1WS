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
    /// 数据库表名称：priv_operator_location_info
    /// </summary>
    public class PrivOperatorLocationInfo
    {
        
        /// <summary>
        /// 操作员id
        /// </summary>
        private string _operator_id;
        
        /// <summary>
        /// 位置id
        /// </summary>
        private string _location_id;
        
        /// <summary>
        /// 更新日期
        /// </summary>
        private string _update_date;
        
        /// <summary>
        /// 更新时间
        /// </summary>
        private string _update_time;
        
        /// <summary>
        /// 更新操作员id
        /// </summary>
        private string _updating_operator_id;
        
        /// <summary>
        /// 操作员id
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
        
        /// <summary>
        /// 位置id
        /// </summary>
        public string location_id
        {
            get
            {
                return this._location_id;
            }
            set
            {
                this._location_id = value;
            }
        }
        
        /// <summary>
        /// 更新日期
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
        /// 更新时间
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
        /// 更新操作员id
        /// </summary>
        public string updating_operator_id
        {
            get
            {
                return this._updating_operator_id;
            }
            set
            {
                this._updating_operator_id = value;
            }
        }
    }
}
