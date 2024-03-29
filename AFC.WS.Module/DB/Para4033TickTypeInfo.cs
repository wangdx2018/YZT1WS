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
    /// 数据库表名称：para_4033_tick_type_info
    /// </summary>
    public class Para4033TickTypeInfo
    {
        
        /// <summary>
        /// 版本号
        /// </summary>
        private string _para_version;
        
        /// <summary>
        /// COLUMN: TICK_TYPE_ID
        /// </summary>
        private string _tick_type_id;
        
        /// <summary>
        /// COLUMN: TICKET_FAMILY_ID
        /// </summary>
        private decimal _ticket_family_id;
        
        /// <summary>
        /// COLUMN: TICK_TYPE_CH_NAME
        /// </summary>
        private string _tick_type_ch_name;
        
        /// <summary>
        /// COLUMN: TICK_TYPE_EN_NAME
        /// </summary>
        private string _tick_type_en_name;
        
        /// <summary>
        /// 0：否；1：是
        /// </summary>
        private string _rec_name_store_value_tick_flag;
        
        /// <summary>
        /// 0：否；1：是
        /// </summary>
        private string _souvenir_flag;
        
        /// <summary>
        /// 0：红色
        ///1：黄色
        ///2：绿色
        ///
        /// </summary>
        private string _light_display_id;
        
        /// <summary>
        /// 0：否；1：是
        /// </summary>
        private string _is_check_blacklist;
        
        /// <summary>
        /// 只对单程票有用
        ///0－否1－是（只允许售卖站进站）
        ///
        /// </summary>
        private string _is_check_sell_station;
        
        /// <summary>
        /// COLUMN: IS_RECHARGEABLE
        /// </summary>
        private string _is_rechargeable;
        
        /// <summary>
        /// COLUMN: STORE_TICK_MAX_REMAIN_VALUE
        /// </summary>
        private decimal _store_tick_max_remain_value;
        
        /// <summary>
        /// COLUMN: MIN_ENTER_STATION_MONEY
        /// </summary>
        private decimal _min_enter_station_money;
        
        /// <summary>
        /// COLUMN: MIN_EXIT_STATION_MONEY
        /// </summary>
        private decimal _min_exit_station_money;
        
        /// <summary>
        /// COLUMN: TICK_SELL_DEPOSIT
        /// </summary>
        private decimal _tick_sell_deposit;
        
        /// <summary>
        /// COLUMN: IS_TICK_RETURN
        /// </summary>
        private string _is_tick_return;
        
        /// <summary>
        /// COLUMN: MAX_INS_TICK_RETURN_MONEY
        /// </summary>
        private decimal _max_ins_tick_return_money;
        
        /// <summary>
        /// COLUMN: DEPOSIT_FEE
        /// </summary>
        private decimal _deposit_fee;
        
        /// <summary>
        /// 限制计时票
        /// </summary>
        private string _max_travel_times;
        
        /// <summary>
        /// COLUMN: MAX_SINGLE_TRAVEL_MILE
        /// </summary>
        private decimal _max_single_travel_mile;
        
        /// <summary>
        /// COLUMN: MAX_DAY_TRAVEL_TIME
        /// </summary>
        private string _max_day_travel_time;
        
        /// <summary>
        /// COLUMN: TICK_VALIDATE_MODE
        /// </summary>
        private decimal _tick_validate_mode;
        
        /// <summary>
        /// COLUMN: TIME_LIMIT_RANGE
        /// </summary>
        private decimal _time_limit_range;
        
        /// <summary>
        /// COLUMN: FIXED_EFFECTIVE_DEADLINE
        /// </summary>
        private string _fixed_effective_deadline;
        
        /// <summary>
        /// COLUMN: IS_PROLONG_ALLOWED
        /// </summary>
        private string _is_prolong_allowed;
        
        /// <summary>
        /// COLUMN: DEFAULT_PROLONG_DAYS
        /// </summary>
        private decimal _default_prolong_days;
        
        /// <summary>
        /// COLUMN: IS_LAST_RIDE_DISCOUNT_ALLOW
        /// </summary>
        private string _is_last_ride_discount_allow;
        
        /// <summary>
        /// COLUMN: REENTER_STATION_MAX_TIME
        /// </summary>
        private string _reenter_station_max_time;
        
        /// <summary>
        /// COLUMN: UPDATE_MONEY
        /// </summary>
        private decimal _update_money;
        
        /// <summary>
        /// COLUMN: MAX_STAY_TIME
        /// </summary>
        private string _max_stay_time;
        
        /// <summary>
        /// COLUMN: TIMEOUT_REFUND_MONEY
        /// </summary>
        private decimal _timeout_refund_money;
        
        /// <summary>
        /// COLUMN: EXCEED_RIDE_REFUND_MONEY
        /// </summary>
        private decimal _exceed_ride_refund_money;
        
        /// <summary>
        /// COLUMN: RIDE_TIME_OUT_REFUND_MONEY
        /// </summary>
        private decimal _ride_time_out_refund_money;
        
        /// <summary>
        /// COLUMN: ENTER_EXIT_MISMATCH_MONEY
        /// </summary>
        private decimal _enter_exit_mismatch_money;
        
        /// <summary>
        /// COLUMN: RE_REFUND_MONEY_OTHER_STATION
        /// </summary>
        private decimal _re_refund_money_other_station;
        
        /// <summary>
        /// 0：否；1：是
        /// </summary>
        private string _audi_record_flag;
        
        /// <summary>
        /// 字节位表示法，置0禁用，置1启用。
        ///bit0：联乘优惠
        ///bit1：累积优惠
        ///bit2：折扣优惠
        ///bit3~bit7：保留
        ///
        /// </summary>
        private string _discount_mode;
        
        /// <summary>
        /// 不用，填0
        /// </summary>
        private string _multi_trip_discount_code;
        
        /// <summary>
        /// 不用，填0
        /// </summary>
        private string _point_discount_code;
        
        /// <summary>
        /// 百分比数，使用时除100
        /// </summary>
        private string _tianji_discount_rate;
        
        /// <summary>
        /// 百分比数，使用时除100
        /// </summary>
        private string _jinbin_discount_rate;
        
        /// <summary>
        /// 0：否；1：是
        /// </summary>
        private string _use_flag;
        
        /// <summary>
        /// 版本号
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
        /// COLUMN: TICK_TYPE_ID
        /// </summary>
        public string tick_type_id
        {
            get
            {
                return this._tick_type_id;
            }
            set
            {
                this._tick_type_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TICKET_FAMILY_ID
        /// </summary>
        public decimal ticket_family_id
        {
            get
            {
                return this._ticket_family_id;
            }
            set
            {
                this._ticket_family_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TICK_TYPE_CH_NAME
        /// </summary>
        public string tick_type_ch_name
        {
            get
            {
                return this._tick_type_ch_name;
            }
            set
            {
                this._tick_type_ch_name = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TICK_TYPE_EN_NAME
        /// </summary>
        public string tick_type_en_name
        {
            get
            {
                return this._tick_type_en_name;
            }
            set
            {
                this._tick_type_en_name = value;
            }
        }
        
        /// <summary>
        /// 0：否；1：是
        /// </summary>
        public string rec_name_store_value_tick_flag
        {
            get
            {
                return this._rec_name_store_value_tick_flag;
            }
            set
            {
                this._rec_name_store_value_tick_flag = value;
            }
        }
        
        /// <summary>
        /// 0：否；1：是
        /// </summary>
        public string souvenir_flag
        {
            get
            {
                return this._souvenir_flag;
            }
            set
            {
                this._souvenir_flag = value;
            }
        }
        
        /// <summary>
        /// 0：红色
        ///1：黄色
        ///2：绿色
        ///
        /// </summary>
        public string light_display_id
        {
            get
            {
                return this._light_display_id;
            }
            set
            {
                this._light_display_id = value;
            }
        }
        
        /// <summary>
        /// 0：否；1：是
        /// </summary>
        public string is_check_blacklist
        {
            get
            {
                return this._is_check_blacklist;
            }
            set
            {
                this._is_check_blacklist = value;
            }
        }
        
        /// <summary>
        /// 只对单程票有用
        ///0－否1－是（只允许售卖站进站）
        ///
        /// </summary>
        public string is_check_sell_station
        {
            get
            {
                return this._is_check_sell_station;
            }
            set
            {
                this._is_check_sell_station = value;
            }
        }
        
        /// <summary>
        /// COLUMN: IS_RECHARGEABLE
        /// </summary>
        public string is_rechargeable
        {
            get
            {
                return this._is_rechargeable;
            }
            set
            {
                this._is_rechargeable = value;
            }
        }
        
        /// <summary>
        /// COLUMN: STORE_TICK_MAX_REMAIN_VALUE
        /// </summary>
        public decimal store_tick_max_remain_value
        {
            get
            {
                return this._store_tick_max_remain_value;
            }
            set
            {
                this._store_tick_max_remain_value = value;
            }
        }
        
        /// <summary>
        /// COLUMN: MIN_ENTER_STATION_MONEY
        /// </summary>
        public decimal min_enter_station_money
        {
            get
            {
                return this._min_enter_station_money;
            }
            set
            {
                this._min_enter_station_money = value;
            }
        }
        
        /// <summary>
        /// COLUMN: MIN_EXIT_STATION_MONEY
        /// </summary>
        public decimal min_exit_station_money
        {
            get
            {
                return this._min_exit_station_money;
            }
            set
            {
                this._min_exit_station_money = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TICK_SELL_DEPOSIT
        /// </summary>
        public decimal tick_sell_deposit
        {
            get
            {
                return this._tick_sell_deposit;
            }
            set
            {
                this._tick_sell_deposit = value;
            }
        }
        
        /// <summary>
        /// COLUMN: IS_TICK_RETURN
        /// </summary>
        public string is_tick_return
        {
            get
            {
                return this._is_tick_return;
            }
            set
            {
                this._is_tick_return = value;
            }
        }
        
        /// <summary>
        /// COLUMN: MAX_INS_TICK_RETURN_MONEY
        /// </summary>
        public decimal max_ins_tick_return_money
        {
            get
            {
                return this._max_ins_tick_return_money;
            }
            set
            {
                this._max_ins_tick_return_money = value;
            }
        }
        
        /// <summary>
        /// COLUMN: DEPOSIT_FEE
        /// </summary>
        public decimal deposit_fee
        {
            get
            {
                return this._deposit_fee;
            }
            set
            {
                this._deposit_fee = value;
            }
        }
        
        /// <summary>
        /// 限制计时票
        /// </summary>
        public string max_travel_times
        {
            get
            {
                return this._max_travel_times;
            }
            set
            {
                this._max_travel_times = value;
            }
        }
        
        /// <summary>
        /// COLUMN: MAX_SINGLE_TRAVEL_MILE
        /// </summary>
        public decimal max_single_travel_mile
        {
            get
            {
                return this._max_single_travel_mile;
            }
            set
            {
                this._max_single_travel_mile = value;
            }
        }
        
        /// <summary>
        /// COLUMN: MAX_DAY_TRAVEL_TIME
        /// </summary>
        public string max_day_travel_time
        {
            get
            {
                return this._max_day_travel_time;
            }
            set
            {
                this._max_day_travel_time = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TICK_VALIDATE_MODE
        /// </summary>
        public decimal tick_validate_mode
        {
            get
            {
                return this._tick_validate_mode;
            }
            set
            {
                this._tick_validate_mode = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TIME_LIMIT_RANGE
        /// </summary>
        public decimal time_limit_range
        {
            get
            {
                return this._time_limit_range;
            }
            set
            {
                this._time_limit_range = value;
            }
        }
        
        /// <summary>
        /// COLUMN: FIXED_EFFECTIVE_DEADLINE
        /// </summary>
        public string fixed_effective_deadline
        {
            get
            {
                return this._fixed_effective_deadline;
            }
            set
            {
                this._fixed_effective_deadline = value;
            }
        }
        
        /// <summary>
        /// COLUMN: IS_PROLONG_ALLOWED
        /// </summary>
        public string is_prolong_allowed
        {
            get
            {
                return this._is_prolong_allowed;
            }
            set
            {
                this._is_prolong_allowed = value;
            }
        }
        
        /// <summary>
        /// COLUMN: DEFAULT_PROLONG_DAYS
        /// </summary>
        public decimal default_prolong_days
        {
            get
            {
                return this._default_prolong_days;
            }
            set
            {
                this._default_prolong_days = value;
            }
        }
        
        /// <summary>
        /// COLUMN: IS_LAST_RIDE_DISCOUNT_ALLOW
        /// </summary>
        public string is_last_ride_discount_allow
        {
            get
            {
                return this._is_last_ride_discount_allow;
            }
            set
            {
                this._is_last_ride_discount_allow = value;
            }
        }
        
        /// <summary>
        /// COLUMN: REENTER_STATION_MAX_TIME
        /// </summary>
        public string reenter_station_max_time
        {
            get
            {
                return this._reenter_station_max_time;
            }
            set
            {
                this._reenter_station_max_time = value;
            }
        }
        
        /// <summary>
        /// COLUMN: UPDATE_MONEY
        /// </summary>
        public decimal update_money
        {
            get
            {
                return this._update_money;
            }
            set
            {
                this._update_money = value;
            }
        }
        
        /// <summary>
        /// COLUMN: MAX_STAY_TIME
        /// </summary>
        public string max_stay_time
        {
            get
            {
                return this._max_stay_time;
            }
            set
            {
                this._max_stay_time = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TIMEOUT_REFUND_MONEY
        /// </summary>
        public decimal timeout_refund_money
        {
            get
            {
                return this._timeout_refund_money;
            }
            set
            {
                this._timeout_refund_money = value;
            }
        }
        
        /// <summary>
        /// COLUMN: EXCEED_RIDE_REFUND_MONEY
        /// </summary>
        public decimal exceed_ride_refund_money
        {
            get
            {
                return this._exceed_ride_refund_money;
            }
            set
            {
                this._exceed_ride_refund_money = value;
            }
        }
        
        /// <summary>
        /// COLUMN: RIDE_TIME_OUT_REFUND_MONEY
        /// </summary>
        public decimal ride_time_out_refund_money
        {
            get
            {
                return this._ride_time_out_refund_money;
            }
            set
            {
                this._ride_time_out_refund_money = value;
            }
        }
        
        /// <summary>
        /// COLUMN: ENTER_EXIT_MISMATCH_MONEY
        /// </summary>
        public decimal enter_exit_mismatch_money
        {
            get
            {
                return this._enter_exit_mismatch_money;
            }
            set
            {
                this._enter_exit_mismatch_money = value;
            }
        }
        
        /// <summary>
        /// COLUMN: RE_REFUND_MONEY_OTHER_STATION
        /// </summary>
        public decimal re_refund_money_other_station
        {
            get
            {
                return this._re_refund_money_other_station;
            }
            set
            {
                this._re_refund_money_other_station = value;
            }
        }
        
        /// <summary>
        /// 0：否；1：是
        /// </summary>
        public string audi_record_flag
        {
            get
            {
                return this._audi_record_flag;
            }
            set
            {
                this._audi_record_flag = value;
            }
        }
        
        /// <summary>
        /// 字节位表示法，置0禁用，置1启用。
        ///bit0：联乘优惠
        ///bit1：累积优惠
        ///bit2：折扣优惠
        ///bit3~bit7：保留
        ///
        /// </summary>
        public string discount_mode
        {
            get
            {
                return this._discount_mode;
            }
            set
            {
                this._discount_mode = value;
            }
        }
        
        /// <summary>
        /// 不用，填0
        /// </summary>
        public string multi_trip_discount_code
        {
            get
            {
                return this._multi_trip_discount_code;
            }
            set
            {
                this._multi_trip_discount_code = value;
            }
        }
        
        /// <summary>
        /// 不用，填0
        /// </summary>
        public string point_discount_code
        {
            get
            {
                return this._point_discount_code;
            }
            set
            {
                this._point_discount_code = value;
            }
        }
        
        /// <summary>
        /// 百分比数，使用时除100
        /// </summary>
        public string tianji_discount_rate
        {
            get
            {
                return this._tianji_discount_rate;
            }
            set
            {
                this._tianji_discount_rate = value;
            }
        }
        
        /// <summary>
        /// 百分比数，使用时除100
        /// </summary>
        public string jinbin_discount_rate
        {
            get
            {
                return this._jinbin_discount_rate;
            }
            set
            {
                this._jinbin_discount_rate = value;
            }
        }
        
        /// <summary>
        /// 0：否；1：是
        /// </summary>
        public string use_flag
        {
            get
            {
                return this._use_flag;
            }
            set
            {
                this._use_flag = value;
            }
        }
    }
}
