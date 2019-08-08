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
    /// 数据库表名称：para_4032_station_info
    /// </summary>
    public class Para4032StationInfo
    {
        
        /// <summary>
        /// 版本号
        /// </summary>
        private string _para_version;
        
        /// <summary>
        /// 车站id
        /// </summary>
        private string _station_id;
        
        /// <summary>
        /// COLUMN: STATION_CH_NAME
        /// </summary>
        private string _station_ch_name;
        
        /// <summary>
        /// COLUMN: STATION_EN_NAME
        /// </summary>
        private string _station_en_name;
        
        /// <summary>
        /// 见《01系统编码》
        /// </summary>
        private string _line_id;
        
        /// <summary>
        /// 0－不启用
        ///1－启用
        ///
        /// </summary>
        private string _is_used_flag;
        
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
        
        /// <summary>
        /// COLUMN: STATION_CH_NAME
        /// </summary>
        public string station_ch_name
        {
            get
            {
                return this._station_ch_name;
            }
            set
            {
                this._station_ch_name = value;
            }
        }
        
        /// <summary>
        /// COLUMN: STATION_EN_NAME
        /// </summary>
        public string station_en_name
        {
            get
            {
                return this._station_en_name;
            }
            set
            {
                this._station_en_name = value;
            }
        }
        
        /// <summary>
        /// 见《01系统编码》
        /// </summary>
        public string line_id
        {
            get
            {
                return this._line_id;
            }
            set
            {
                this._line_id = value;
            }
        }
        
        /// <summary>
        /// 0－不启用
        ///1－启用
        ///
        /// </summary>
        public string is_used_flag
        {
            get
            {
                return this._is_used_flag;
            }
            set
            {
                this._is_used_flag = value;
            }
        }
    }
}