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
    /// 数据库表名称：basi_cash_box_type_info
    /// </summary>
    public class BasiCashBoxTypeInfo
    {
        
        /// <summary>
        /// COLUMN: BOX_LOCATION_TYPE
        /// </summary>
        private string _box_location_type;
        
        /// <summary>
        /// COLUMN: CASH_BOX_TYPE_NAME
        /// </summary>
        private string _cash_box_type_name;
        
        /// <summary>
        /// COLUMN: REMARK
        /// </summary>
        private string _remark;
        
        /// <summary>
        /// COLUMN: BOX_LOCATION_TYPE
        /// </summary>
        public string box_location_type
        {
            get
            {
                return this._box_location_type;
            }
            set
            {
                this._box_location_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: CASH_BOX_TYPE_NAME
        /// </summary>
        public string cash_box_type_name
        {
            get
            {
                return this._cash_box_type_name;
            }
            set
            {
                this._cash_box_type_name = value;
            }
        }
        
        /// <summary>
        /// COLUMN: REMARK
        /// </summary>
        public string remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                this._remark = value;
            }
        }
    }
}