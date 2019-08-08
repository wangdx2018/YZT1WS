//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.4927
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
    /// 数据库表名称：tick_box_status_info
    /// </summary>
    public class TickBoxStatusInfo
    {
        
        /// <summary>
        /// 见《01系统编码》
        /// </summary>
        private string _line_id;
        
        /// <summary>
        /// 车站id
        /// </summary>
        private string _station_id;
        
        /// <summary>
        /// 票箱id
        /// </summary>
        private string _ticket_box_id;
        
        /// <summary>
        /// 00：在操作员手上;01：在设备上;02：在库（在票务室）ff：未启用
        /// </summary>
        private string _box_position;
        
        /// <summary>
        /// 库存管理类型
        /// </summary>
        private string _tick_mana_type;
        
        /// <summary>
        /// 票卡当前数量
        /// </summary>
        private decimal _tickets_num;
        
        /// <summary>
        /// 更新日期
        /// </summary>
        private string _update_date;
        
        /// <summary>
        /// 更新时间
        /// </summary>
        private string _update_time;
        
        /// <summary>
        /// 见《01系统编码》
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
        /// 票箱id
        /// </summary>
        public string ticket_box_id
        {
            get
            {
                return this._ticket_box_id;
            }
            set
            {
                this._ticket_box_id = value;
            }
        }
        
        /// <summary>
        /// 00：在操作员手上;01：在设备上;02：在库（在票务室）ff：未启用
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
        /// 库存管理类型
        /// </summary>
        public string tick_mana_type
        {
            get
            {
                return this._tick_mana_type;
            }
            set
            {
                this._tick_mana_type = value;
            }
        }
        
        /// <summary>
        /// 票卡当前数量
        /// </summary>
        public decimal tickets_num
        {
            get
            {
                return this._tickets_num;
            }
            set
            {
                this._tickets_num = value;
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