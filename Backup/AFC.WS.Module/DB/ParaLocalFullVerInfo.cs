//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行库版本:2.0.50727.3615
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
    /// 数据库表名称：para_local_full_ver_info
    /// </summary>
    public class ParaLocalFullVerInfo
    {
        
        /// <summary>
        /// 参数类型
        /// </summary>
        private string _para_type;
        
        /// <summary>
        /// 版本类型：0：现在版本
        ///1：将来版本
        ///3：历史版本
        ///
        /// </summary>
        private string _edition_type;
        
      
        /// <summary>
        /// 版本号
        /// </summary>
        private string _para_version;
        
        
        /// <summary>
        /// 参数子类型
        /// </summary>
        private string _para_sub_type;
        
        /// <summary>
        /// 全路径名称
        /// </summary>
        private string _para_file_name;
        
        
        /// <summary>
        /// 生效日期
        /// </summary>
        private string _active_date;
        
        /// <summary>
        /// COLUMN: ACTIVE_TIME
        /// </summary>
        private string _active_time;
        
        
        /// <summary>
        /// 参数类型
        /// </summary>
        public string para_type
        {
            get
            {
                return this._para_type;
            }
            set
            {
                this._para_type = value;
            }
        }
        
        /// <summary>
        /// 版本类型：0：现在版本
        ///1：将来版本
        ///3：历史版本
        ///
        /// </summary>
        public string edition_type
        {
            get
            {
                return this._edition_type;
            }
            set
            {
                this._edition_type = value;
            }
        }
        
        
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
        /// 参数子类型
        /// </summary>
        public string para_sub_type
        {
            get
            {
                return this._para_sub_type;
            }
            set
            {
                this._para_sub_type = value;
            }
        }
        
        /// <summary>
        /// 全路径名称
        /// </summary>
        public string para_file_name
        {
            get
            {
                return this._para_file_name;
            }
            set
            {
                this._para_file_name = value;
            }
        }
       
        
        /// <summary>
        /// 生效日期
        /// </summary>
        public string active_date
        {
            get
            {
                return this._active_date;
            }
            set
            {
                this._active_date = value;
            }
        }
        
        /// <summary>
        /// COLUMN: ACTIVE_TIME
        /// </summary>
        public string active_time
        {
            get
            {
                return this._active_time;
            }
            set
            {
                this._active_time = value;
            }
        }
        
    }
}
