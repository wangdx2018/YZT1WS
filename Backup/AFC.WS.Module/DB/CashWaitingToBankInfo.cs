//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行库版本:2.0.50727.3615
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
    /// 数据库表名称：cash_waiting_to_bank_info
    /// </summary>
    public class CashWaitingToBankInfo
    {
        
        /// <summary>
        /// 线路id
        /// </summary>
        private string _line_id;
        
        /// <summary>
        /// 车站id
        /// </summary>
        private string _station_id;
        
        /// <summary>
        /// 操作员id
        /// </summary>
        private string _operator_id;
        
        /// <summary>
        /// 运营日
        /// </summary>
        private string _run_date;
        
        /// <summary>
        /// 币种代码
        /// </summary>
        private string _currency_code;
        
        /// <summary>
        /// 总金额
        /// </summary>
        private decimal _total_value;
        
        /// <summary>
        /// 更新日期
        /// </summary>
        private string _update_date;
        
        /// <summary>
        /// 更新时间
        /// </summary>
        private string _update_time;
        
        /// <summary>
        /// 线路id
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
        /// 操作员id
        /// </summary>
        public string operator_id
        {
            get
            {
                return this._operator_id;
            }
            set
            {
                this._operator_id = value;
            }
        }
        
        /// <summary>
        /// 运营日
        /// </summary>
        public string run_date
        {
            get
            {
                return this._run_date;
            }
            set
            {
                this._run_date = value;
            }
        }
        
        /// <summary>
        /// 币种代码
        /// </summary>
        public string currency_code
        {
            get
            {
                return this._currency_code;
            }
            set
            {
                this._currency_code = value;
            }
        }
        
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal total_value
        {
            get
            {
                return this._total_value;
            }
            set
            {
                this._total_value = value;
            }
        }
        
        /// <summary>
        /// 更新日期
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
        /// 更新时间
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
