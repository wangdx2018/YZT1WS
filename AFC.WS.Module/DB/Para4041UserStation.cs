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
    /// 数据库表名称：para_4041_user_station
    /// </summary>
    public class Para4041UserStation
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
        /// 车站id
        /// </summary>
        private string _station_id;
        
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
        /// 车站id
        /// </summary>
        public string station_id
        {
            get
            {
                return this._station_id;
            }
            set
            {
                this._station_id = value;
            }
        }
    }
}
