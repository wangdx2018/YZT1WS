//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行库版本:2.0.50727.3615
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace AFC.WS.UI.BR.Data
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    
    
    /// <summary>
    /// 数据库表名称：cash_box_replace_info
    /// </summary>
    public class CashBoxReplaceInfo
    {
        
        /// <summary>
        /// 本次操作的实际金额
        /// </summary>
        private decimal _total_money_count;
        
        /// <summary>
        /// 线路id
        /// </summary>
        private string _line_id;
        
        /// <summary>
        /// 车站id
        /// </summary>
        private string _station_id;
        
        /// <summary>
        /// 设备id
        /// </summary>
        private string _device_id;
        
        /// <summary>
        /// 更换流水号
        /// </summary>
        private string _replace_sn;
        
        /// <summary>
        /// 01：安装 ；02：卸下 ；03：清点 ；04：压钱；05：领用 ；06：归还 ；07：rfid初始化；08：钱箱登记
        ///
        /// </summary>
        private string _replace_type;
        
        /// <summary>
        /// 钱箱id
        /// </summary>
        private string _money_box_id;
        
        /// <summary>
        /// 01：正常安装；02：非法安装；03：正常卸下；04：非法卸下；
        /// </summary>
        private string _install_status;
        
        /// <summary>
        /// 在设备上的安装位置。在设备内按照从上到下从左到右的顺序排序编码，从01开始
        /// </summary>
        private string _position_in_dev;
        
        /// <summary>
        /// 币种代码
        /// </summary>
        private string _currency_code;
        
        /// <summary>
        /// 币种当前数量
        /// </summary>
        private decimal _currency_num;
        
        /// <summary>
        /// 操作员id
        /// </summary>
        private string _operator_id;
        
        /// <summary>
        /// 更换操作的日期。从业务数据中的“公共部分”获得
        /// </summary>
        private string _occur_date;
        
        /// <summary>
        /// 更换操作的时间。从业务数据的“公共部分”获得
        /// </summary>
        private string _occur_time;
        
        /// <summary>
        /// 本地处理日期
        /// </summary>
        private string _update_date;
        
        /// <summary>
        /// 本地处理时间
        /// </summary>
        private string _update_time;
        
        /// <summary>
        /// COLUMN: BEFORE_OPERATION_MONEY
        /// </summary>
        private decimal _before_operation_money;
        
        /// <summary>
        /// 本次操作的实际金额
        /// </summary>
        public decimal total_money_count
        {
            get
            {
                return this._total_money_count;
            }
            set
            {
                this._total_money_count = value;
            }
        }
        
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
        /// 设备id
        /// </summary>
        public string device_id
        {
            get
            {
                return this._device_id;
            }
            set
            {
                this._device_id = value;
            }
        }
        
        /// <summary>
        /// 更换流水号
        /// </summary>
        public string replace_sn
        {
            get
            {
                return this._replace_sn;
            }
            set
            {
                this._replace_sn = value;
            }
        }
        
        /// <summary>
        /// 01：安装 ；02：卸下 ；03：清点 ；04：压钱；05：领用 ；06：归还 ；07：rfid初始化；08：钱箱登记
        ///
        /// </summary>
        public string replace_type
        {
            get
            {
                return this._replace_type;
            }
            set
            {
                this._replace_type = value;
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
        /// 01：正常安装；02：非法安装；03：正常卸下；04：非法卸下；
        /// </summary>
        public string install_status
        {
            get
            {
                return this._install_status;
            }
            set
            {
                this._install_status = value;
            }
        }
        
        /// <summary>
        /// 在设备上的安装位置。在设备内按照从上到下从左到右的顺序排序编码，从01开始
        /// </summary>
        public string position_in_dev
        {
            get
            {
                return this._position_in_dev;
            }
            set
            {
                this._position_in_dev = value;
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
        /// 币种当前数量
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
        /// 更换操作的日期。从业务数据中的“公共部分”获得
        /// </summary>
        public string occur_date
        {
            get
            {
                return this._occur_date;
            }
            set
            {
                this._occur_date = value;
            }
        }
        
        /// <summary>
        /// 更换操作的时间。从业务数据的“公共部分”获得
        /// </summary>
        public string occur_time
        {
            get
            {
                return this._occur_time;
            }
            set
            {
                this._occur_time = value;
            }
        }
        
        /// <summary>
        /// 本地处理日期
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
        /// 本地处理时间
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
        
        /// <summary>
        /// COLUMN: BEFORE_OPERATION_MONEY
        /// </summary>
        public decimal before_operation_money
        {
            get
            {
                return this._before_operation_money;
            }
            set
            {
                this._before_operation_money = value;
            }
        }
    }
}
