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
    /// 数据库表名称：cash_box_status_info
    /// </summary>
    public class CashBoxStatusInfo
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
        /// 钱箱id
        /// </summary>
        private string _money_box_id;
        
        /// <summary>
        /// 01：在库；02：在操作员；03：在设备
        /// </summary>
        private string _box_position;
        
        /// <summary>
        /// 币种代码。当多币种时，填"00"
        /// </summary>
        private string _currency_code;
        
        /// <summary>
        /// 币种数量
        /// </summary>
        private decimal _currency_num;
        
        /// <summary>
        /// 当多币种时此字段写总金额。币种代码填写00
        /// </summary>
        private decimal _total_money_value;
        
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
        /// 钱箱id
        /// </summary>
        public string money_box_id
        {
            get
            {
                return this._money_box_id;
            }
            set
            {
                this._money_box_id = value;
            }
        }
        
        /// <summary>
        /// 01：在库；02：在操作员；03：在设备
        /// </summary>
        public string box_position
        {
            get
            {
                return this._box_position;
            }
            set
            {
                this._box_position = value;
            }
        }
        
        /// <summary>
        /// 币种代码。当多币种时，填"00"
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
        /// 币种数量
        /// </summary>
        public decimal currency_num
        {
            get
            {
                return this._currency_num;
            }
            set
            {
                this._currency_num = value;
            }
        }
        
        /// <summary>
        /// 当多币种时此字段写总金额。币种代码填写00
        /// </summary>
        public decimal total_money_value
        {
            get
            {
                return this._total_money_value;
            }
            set
            {
                this._total_money_value = value;
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
