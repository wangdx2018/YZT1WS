//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.3053
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
    /// 数据库表名称：basi_report_sub_type_info
    /// </summary>
    public class BasiReportSubTypeInfo
    {
        
        /// <summary>
        /// 报表子类型id
        ///
        /// </summary>
        private decimal _report_sub_type_id;
        
        /// <summary>
        /// COLUMN: REPORT_SUB_TYPE_NAME
        /// </summary>
        private string _report_sub_type_name;
        
        /// <summary>
        /// 报表类型id
        ///
        /// </summary>
        private decimal _report_type_id;
        
        /// <summary>
        /// 报表子类型id
        ///
        /// </summary>
        public decimal report_sub_type_id
        {
            get
            {
                return this._report_sub_type_id;
            }
            set
            {
                this._report_sub_type_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: REPORT_SUB_TYPE_NAME
        /// </summary>
        public string report_sub_type_name
        {
            get
            {
                return this._report_sub_type_name;
            }
            set
            {
                this._report_sub_type_name = value;
            }
        }
        
        /// <summary>
        /// 报表类型id
        ///
        /// </summary>
        public decimal report_type_id
        {
            get
            {
                return this._report_type_id;
            }
            set
            {
                this._report_type_id = value;
            }
        }
    }
}
