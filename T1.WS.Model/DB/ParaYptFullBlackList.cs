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
    /// 数据库表名称：para_ypt_full_black_list
    /// </summary>
    public class ParaYptFullBlackList
    {
        
        /// <summary>
        /// COLUMN: PARA_VERSION
        /// </summary>
        private string _para_version;
        
        /// <summary>
        /// COLUMN: PHYSICAL_CARD_NO
        /// </summary>
        private string _physical_card_no;
        
        /// <summary>
        /// COLUMN: CONTROL_MODE
        /// </summary>
        private string _control_mode;
        
        /// <summary>
        /// COLUMN: PARA_VERSION
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
        /// COLUMN: PHYSICAL_CARD_NO
        /// </summary>
        public string physical_card_no
        {
            get
            {
                return this._physical_card_no;
            }
            set
            {
                this._physical_card_no = value;
            }
        }
        
        /// <summary>
        /// COLUMN: CONTROL_MODE
        /// </summary>
        public string control_mode
        {
            get
            {
                return this._control_mode;
            }
            set
            {
                this._control_mode = value;
            }
        }
    }
}
