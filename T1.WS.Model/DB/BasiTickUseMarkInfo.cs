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
    /// 数据库表名称：basi_tick_use_mark_info
    /// </summary>
    public class BasiTickUseMarkInfo
    {
        
        /// <summary>
        /// COLUMN: TICKET_CODE
        /// </summary>
        private string _ticket_code;
        
        /// <summary>
        /// COLUMN: TICKET_DESC_ENG
        /// </summary>
        private string _ticket_desc_eng;
        
        /// <summary>
        /// COLUMN: TICKET_DESC_CHI
        /// </summary>
        private string _ticket_desc_chi;
        
        /// <summary>
        /// COLUMN: TICKET_CODE
        /// </summary>
        public string ticket_code
        {
            get
            {
                return this._ticket_code;
            }
            set
            {
                this._ticket_code = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TICKET_DESC_ENG
        /// </summary>
        public string ticket_desc_eng
        {
            get
            {
                return this._ticket_desc_eng;
            }
            set
            {
                this._ticket_desc_eng = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TICKET_DESC_CHI
        /// </summary>
        public string ticket_desc_chi
        {
            get
            {
                return this._ticket_desc_chi;
            }
            set
            {
                this._ticket_desc_chi = value;
            }
        }
    }
}
