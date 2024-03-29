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
    /// 数据库表名称：data_pass_flow_info
    /// </summary>
    public class DataPassFlowInfo
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
        /// 设备id
        /// </summary>
        private string _device_id;
        
        /// <summary>
        /// COLUMN: AFC_TYPE
        /// </summary>
        private string _afc_type;
        
        /// <summary>
        /// COLUMN: TRAN_DATE
        /// </summary>
        private string _tran_date;
        
        /// <summary>
        /// COLUMN: TRAN_TIME_MIN
        /// </summary>
        private string _tran_time_min;
        
        /// <summary>
        /// 运营日期
        /// </summary>
        private string _run_date;
        
        /// <summary>
        /// COLUMN: RUN_DATE_TRAN
        /// </summary>
        private string _run_date_tran;
        
        /// <summary>
        /// COLUMN: ISSUER_ID
        /// </summary>
        private string _issuer_id;
        
        /// <summary>
        /// COLUMN: CARD_CHIP_TYPE
        /// </summary>
        private string _card_chip_type;
        
        /// <summary>
        /// COLUMN: TICK_CARD_TYPE
        /// </summary>
        private string _tick_card_type;
        
        /// <summary>
        /// COLUMN: PRODUCT_CATEGORY
        /// </summary>
        private string _product_category;
        
        /// <summary>
        /// COLUMN: VARIANT_TYPE
        /// </summary>
        private string _variant_type;
        
        /// <summary>
        /// COLUMN: PASSENGER_TYPE
        /// </summary>
        private string _passenger_type;
        
        /// <summary>
        /// COLUMN: TEST_FLAG
        /// </summary>
        private string _test_flag;
        
        /// <summary>
        /// COLUMN: PAYMENT_MEANS
        /// </summary>
        private string _payment_means;
        
        /// <summary>
        /// COLUMN: TRAN_VALUE
        /// </summary>
        private decimal _tran_value;
        
        /// <summary>
        /// COLUMN: PASSENGER_NUM
        /// </summary>
        private decimal _passenger_num;
        
        /// <summary>
        /// COLUMN: FOREGIFT_SUM
        /// </summary>
        private decimal _foregift_sum;
        
        /// <summary>
        /// COLUMN: CARD_COST_SUM
        /// </summary>
        private decimal _card_cost_sum;
        
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
        /// COLUMN: AFC_TYPE
        /// </summary>
        public string afc_type
        {
            get
            {
                return this._afc_type;
            }
            set
            {
                this._afc_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TRAN_DATE
        /// </summary>
        public string tran_date
        {
            get
            {
                return this._tran_date;
            }
            set
            {
                this._tran_date = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TRAN_TIME_MIN
        /// </summary>
        public string tran_time_min
        {
            get
            {
                return this._tran_time_min;
            }
            set
            {
                this._tran_time_min = value;
            }
        }
        
        /// <summary>
        /// 运营日期
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
        /// COLUMN: RUN_DATE_TRAN
        /// </summary>
        public string run_date_tran
        {
            get
            {
                return this._run_date_tran;
            }
            set
            {
                this._run_date_tran = value;
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
        
        /// <summary>
        /// COLUMN: CARD_CHIP_TYPE
        /// </summary>
        public string card_chip_type
        {
            get
            {
                return this._card_chip_type;
            }
            set
            {
                this._card_chip_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TICK_CARD_TYPE
        /// </summary>
        public string tick_card_type
        {
            get
            {
                return this._tick_card_type;
            }
            set
            {
                this._tick_card_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: PRODUCT_CATEGORY
        /// </summary>
        public string product_category
        {
            get
            {
                return this._product_category;
            }
            set
            {
                this._product_category = value;
            }
        }
        
        /// <summary>
        /// COLUMN: VARIANT_TYPE
        /// </summary>
        public string variant_type
        {
            get
            {
                return this._variant_type;
            }
            set
            {
                this._variant_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: PASSENGER_TYPE
        /// </summary>
        public string passenger_type
        {
            get
            {
                return this._passenger_type;
            }
            set
            {
                this._passenger_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TEST_FLAG
        /// </summary>
        public string test_flag
        {
            get
            {
                return this._test_flag;
            }
            set
            {
                this._test_flag = value;
            }
        }
        
        /// <summary>
        /// COLUMN: PAYMENT_MEANS
        /// </summary>
        public string payment_means
        {
            get
            {
                return this._payment_means;
            }
            set
            {
                this._payment_means = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TRAN_VALUE
        /// </summary>
        public decimal tran_value
        {
            get
            {
                return this._tran_value;
            }
            set
            {
                this._tran_value = value;
            }
        }
        
        /// <summary>
        /// COLUMN: PASSENGER_NUM
        /// </summary>
        public decimal passenger_num
        {
            get
            {
                return this._passenger_num;
            }
            set
            {
                this._passenger_num = value;
            }
        }
        
        /// <summary>
        /// COLUMN: FOREGIFT_SUM
        /// </summary>
        public decimal foregift_sum
        {
            get
            {
                return this._foregift_sum;
            }
            set
            {
                this._foregift_sum = value;
            }
        }
        
        /// <summary>
        /// COLUMN: CARD_COST_SUM
        /// </summary>
        public decimal card_cost_sum
        {
            get
            {
                return this._card_cost_sum;
            }
            set
            {
                this._card_cost_sum = value;
            }
        }
    }
}
