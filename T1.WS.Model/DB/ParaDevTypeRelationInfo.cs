//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.8839
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace T1.WS.Model.DB
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    
    
    /// <summary>
    /// 数据库表名称：para_dev_type_relation_info
    /// </summary>
    public class ParaDevTypeRelationInfo
    {
        
        /// <summary>
        /// COLUMN: PARA_TYPE
        /// </summary>
        private string _para_type;
        
        /// <summary>
        /// COLUMN: DEVICE_TYPE
        /// </summary>
        private string _device_type;
        
        /// <summary>
        /// COLUMN: UPDATE_DATE
        /// </summary>
        private string _update_date;
        
        /// <summary>
        /// COLUMN: UPDATE_TIME
        /// </summary>
        private string _update_time;
        
        /// <summary>
        /// COLUMN: PARA_TYPE
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
        /// COLUMN: DEVICE_TYPE
        /// </summary>
        public string device_type
        {
            get
            {
                return this._device_type;
            }
            set
            {
                this._device_type = value;
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
    }
}
