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
    /// 数据库表名称：basi_tick_category_info
    /// </summary>
    public class BasiTickCategoryInfo
    {
        
        /// <summary>
        /// COLUMN: TICKET_CATEGORY
        /// </summary>
        private string _ticket_category;
        
        /// <summary>
        /// COLUMN: TICKET_CATEGORY_NAME
        /// </summary>
        private string _ticket_category_name;
        
        /// <summary>
        /// COLUMN: ISSUER_ID
        /// </summary>
        private string _issuer_id;
        
        /// <summary>
        /// COLUMN: TICKET_CATEGORY
        /// </summary>
        public string ticket_category
        {
            get
            {
                return this._ticket_category;
            }
            set
            {
                this._ticket_category = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TICKET_CATEGORY_NAME
        /// </summary>
        public string ticket_category_name
        {
            get
            {
                return this._ticket_category_name;
            }
            set
            {
                this._ticket_category_name = value;
            }
        }
        
        /// <summary>
        /// COLUMN: ISSUER_ID
        /// </summary>
        public string issuer_id
        {
            get
            {
                return this._issuer_id;
            }
            set
            {
                this._issuer_id = value;
            }
        }
    }
}
