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
    /// 数据库表名称：basi_fare_table
    /// </summary>
    public class BasiFareTable
    {
        
        /// <summary>
        /// COLUMN: FARE_TABLE_ID
        /// </summary>
        private string _fare_table_id;
        
        /// <summary>
        /// COLUMN: FARE_ZONE
        /// </summary>
        private string _fare_zone;
        
        /// <summary>
        /// COLUMN: FARE
        /// </summary>
        private string _fare;
        
        /// <summary>
        /// COLUMN: FARE_TABLE_ID
        /// </summary>
        public string fare_table_id
        {
            get
            {
                return this._fare_table_id;
            }
            set
            {
                this._fare_table_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: FARE_ZONE
        /// </summary>
        public string fare_zone
        {
            get
            {
                return this._fare_zone;
            }
            set
            {
                this._fare_zone = value;
            }
        }
        
        /// <summary>
        /// COLUMN: FARE
        /// </summary>
        public string fare
        {
            get
            {
                return this._fare;
            }
            set
            {
                this._fare = value;
            }
        }
    }
}