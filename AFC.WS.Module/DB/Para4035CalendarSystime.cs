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
    /// 数据库表名称：para_4035_calendar_systime
    /// </summary>
    public class Para4035CalendarSystime
    {
        
        /// <summary>
        /// 版本号
        /// </summary>
        private string _para_version;
        
        /// <summary>
        /// COLUMN: TIME_ZONE_DIFF_VALUE
        /// </summary>
        private string _time_zone_diff_value;
        
        /// <summary>
        /// COLUMN: SYSTEM_START_TIME
        /// </summary>
        private decimal _system_start_time;
        
        /// <summary>
        /// COLUMN: SYSTEM_END_TIME
        /// </summary>
        private decimal _system_end_time;
        
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
        /// COLUMN: TIME_ZONE_DIFF_VALUE
        /// </summary>
        public string time_zone_diff_value
        {
            get
            {
                return this._time_zone_diff_value;
            }
            set
            {
                this._time_zone_diff_value = value;
            }
        }
        
        /// <summary>
        /// COLUMN: SYSTEM_START_TIME
        /// </summary>
        public decimal system_start_time
        {
            get
            {
                return this._system_start_time;
            }
            set
            {
                this._system_start_time = value;
            }
        }
        
        /// <summary>
        /// COLUMN: SYSTEM_END_TIME
        /// </summary>
        public decimal system_end_time
        {
            get
            {
                return this._system_end_time;
            }
            set
            {
                this._system_end_time = value;
            }
        }
    }
}
