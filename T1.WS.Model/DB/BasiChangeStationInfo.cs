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
    /// 数据库表名称：basi_change_station_info
    /// </summary>
    public class BasiChangeStationInfo
    {
        
        /// <summary>
        /// COLUMN: STATION_A_ID
        /// </summary>
        private string _station_a_id;
        
        /// <summary>
        /// COLUMN: STATION_B_ID
        /// </summary>
        private string _station_b_id;
        
        /// <summary>
        /// COLUMN: STATION_A_ID
        /// </summary>
        public string station_a_id
        {
            get
            {
                return this._station_a_id;
            }
            set
            {
                this._station_a_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: STATION_B_ID
        /// </summary>
        public string station_b_id
        {
            get
            {
                return this._station_b_id;
            }
            set
            {
                this._station_b_id = value;
            }
        }
    }
}