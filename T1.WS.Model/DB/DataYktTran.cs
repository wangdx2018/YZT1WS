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
    /// 数据库表名称：data_ykt_tran
    /// </summary>
    public class DataYktTran
    {
        
        /// <summary>
        /// COLUMN: LINE_ID
        /// </summary>
        private string _line_id;
        
        /// <summary>
        /// COLUMN: STATION_ID
        /// </summary>
        private string _station_id;
        
        /// <summary>
        /// COLUMN: DEVICE_ID
        /// </summary>
        private string _device_id;
        
        /// <summary>
        /// COLUMN: TRAN_DATE
        /// </summary>
        private string _tran_date;
        
        /// <summary>
        /// COLUMN: TRAN_TIME
        /// </summary>
        private string _tran_time;
        
        /// <summary>
        /// COLUMN: TRAN_TYPE
        /// </summary>
        private string _tran_type;
        
        /// <summary>
        /// COLUMN: TRAN_SUB_TYPE
        /// </summary>
        private string _tran_sub_type;
        
        /// <summary>
        /// COLUMN: UDSN
        /// </summary>
        private string _udsn;
        
        /// <summary>
        /// COLUMN: FILE_SN
        /// </summary>
        private string _file_sn;
        
        /// <summary>
        /// COLUMN: AFC_TYPE
        /// </summary>
        private string _afc_type;
        
        /// <summary>
        /// COLUMN: RUN_DATE
        /// </summary>
        private string _run_date;
        
        /// <summary>
        /// COLUMN: RUN_DATE_TRAN
        /// </summary>
        private string _run_date_tran;
        
        /// <summary>
        /// COLUMN: RECEIVE_DATE
        /// </summary>
        private string _receive_date;
        
        /// <summary>
        /// COLUMN: RECEIVE_TIME
        /// </summary>
        private string _receive_time;
        
        /// <summary>
        /// COLUMN: DEVICE_TYPE
        /// </summary>
        private string _device_type;
        
        /// <summary>
        /// COLUMN: DEVICE_SUB_TYPE
        /// </summary>
        private string _device_sub_type;
        
        /// <summary>
        /// COLUMN: SP_ID
        /// </summary>
        private string _sp_id;
        
        /// <summary>
        /// COLUMN: SPA_ID
        /// </summary>
        private string _spa_id;
        
        /// <summary>
        /// COLUMN: SAMID
        /// </summary>
        private string _samid;
        
        /// <summary>
        /// COLUMN: MODE_CODE
        /// </summary>
        private string _mode_code;
        
        /// <summary>
        /// COLUMN: OPERATOR_ID
        /// </summary>
        private string _operator_id;
        
        /// <summary>
        /// COLUMN: BOM_SHIFT_ID
        /// </summary>
        private string _bom_shift_id;
        
        /// <summary>
        /// COLUMN: TEST_FLAG
        /// </summary>
        private string _test_flag;
        
        /// <summary>
        /// COLUMN: TRAN_VALUE
        /// </summary>
        private string _tran_value;
        
        /// <summary>
        /// COLUMN: REMAIN_VALUE
        /// </summary>
        private string _remain_value;
        
        /// <summary>
        /// COLUMN: ORIGINAL_VALUE
        /// </summary>
        private string _original_value;
        
        /// <summary>
        /// COLUMN: OVERDRAW_VALUE
        /// </summary>
        private string _overdraw_value;
        
        /// <summary>
        /// COLUMN: MONTH_CUMULATE
        /// </summary>
        private string _month_cumulate;
        
        /// <summary>
        /// COLUMN: FOREGIFT
        /// </summary>
        private string _foregift;
        
        /// <summary>
        /// COLUMN: CARD_COST
        /// </summary>
        private string _card_cost;
        
        /// <summary>
        /// COLUMN: FINE_AMOUNT
        /// </summary>
        private string _fine_amount;
        
        /// <summary>
        /// COLUMN: TICKET_COUNTER
        /// </summary>
        private string _ticket_counter;
        
        /// <summary>
        /// COLUMN: PREFERENTIAL
        /// </summary>
        private string _preferential;
        
        /// <summary>
        /// COLUMN: SERVICE_FEE_TYPE
        /// </summary>
        private string _service_fee_type;
        
        /// <summary>
        /// COLUMN: PAYMENT_MEANS
        /// </summary>
        private string _payment_means;
        
        /// <summary>
        /// COLUMN: EXPECT_TRAN_VALUE
        /// </summary>
        private string _expect_tran_value;
        
        /// <summary>
        /// COLUMN: TRANSFER_DISCOUNT
        /// </summary>
        private string _transfer_discount;
        
        /// <summary>
        /// COLUMN: PERSONAL_DISCOUNT
        /// </summary>
        private string _personal_discount;
        
        /// <summary>
        /// COLUMN: PEAK_DISCOUNT
        /// </summary>
        private string _peak_discount;
        
        /// <summary>
        /// COLUMN: REMAINING_RIDES
        /// </summary>
        private string _remaining_rides;
        
        /// <summary>
        /// COLUMN: NUMBER_OF_RIDES
        /// </summary>
        private string _number_of_rides;
        
        /// <summary>
        /// COLUMN: BANK_CARD_ID
        /// </summary>
        private string _bank_card_id;
        
        /// <summary>
        /// COLUMN: BANK_ID
        /// </summary>
        private string _bank_id;
        
        /// <summary>
        /// COLUMN: BANK_TXN_SN
        /// </summary>
        private string _bank_txn_sn;
        
        /// <summary>
        /// COLUMN: ENTRY_STATION_ID
        /// </summary>
        private string _entry_station_id;
        
        /// <summary>
        /// COLUMN: LAST_TRAN_DATE
        /// </summary>
        private string _last_tran_date;
        
        /// <summary>
        /// COLUMN: LAST_TRAN_TIME
        /// </summary>
        private string _last_tran_time;
        
        /// <summary>
        /// COLUMN: CARD_CHIP_TYPE
        /// </summary>
        private string _card_chip_type;
        
        /// <summary>
        /// COLUMN: CARD_SN
        /// </summary>
        private string _card_sn;
        
        /// <summary>
        /// COLUMN: CARD_PHY_ID
        /// </summary>
        private string _card_phy_id;
        
        /// <summary>
        /// COLUMN: ISSUER_ID
        /// </summary>
        private string _issuer_id;
        
        /// <summary>
        /// COLUMN: PRODUCT_CATEGORY
        /// </summary>
        private string _product_category;
        
        /// <summary>
        /// COLUMN: PRODUCT_TYPE
        /// </summary>
        private string _product_type;
        
        /// <summary>
        /// COLUMN: VARIANT_TYPE
        /// </summary>
        private string _variant_type;
        
        /// <summary>
        /// COLUMN: PASSENGER_TYPE
        /// </summary>
        private string _passenger_type;
        
        /// <summary>
        /// COLUMN: PTSN
        /// </summary>
        private string _ptsn;
        
        /// <summary>
        /// COLUMN: RETURN_REASON
        /// </summary>
        private string _return_reason;
        
        /// <summary>
        /// COLUMN: REFUND_REASON
        /// </summary>
        private string _refund_reason;
        
        /// <summary>
        /// COLUMN: REFUND_TYPE
        /// </summary>
        private string _refund_type;
        
        /// <summary>
        /// COLUMN: RECEIPT_NO
        /// </summary>
        private string _receipt_no;
        
        /// <summary>
        /// COLUMN: BLOCK_REASON
        /// </summary>
        private string _block_reason;
        
        /// <summary>
        /// COLUMN: SAMID_TAC
        /// </summary>
        private string _samid_tac;
        
        /// <summary>
        /// COLUMN: YKT_AUTH_SEQ
        /// </summary>
        private string _ykt_auth_seq;
        
        /// <summary>
        /// COLUMN: ONLINE_TNX_SEQ
        /// </summary>
        private string _online_tnx_seq;
        
        /// <summary>
        /// COLUMN: AREA_CODE
        /// </summary>
        private string _area_code;
        
        /// <summary>
        /// COLUMN: AMOUNT_TYPE
        /// </summary>
        private string _amount_type;
        
        /// <summary>
        /// COLUMN: BUS_TIMES
        /// </summary>
        private string _bus_times;
        
        /// <summary>
        /// COLUMN: BUS_TIMES_COUNTER
        /// </summary>
        private string _bus_times_counter;
        
        /// <summary>
        /// COLUMN: METRO_TIMES_COUNTER
        /// </summary>
        private string _metro_times_counter;
        
        /// <summary>
        /// COLUMN: UNION_TIMES_COUNTER
        /// </summary>
        private string _union_times_counter;
        
        /// <summary>
        /// COLUMN: OLD_EXPIRY_DATE
        /// </summary>
        private string _old_expiry_date;
        
        /// <summary>
        /// COLUMN: NEW_EXPIRY_DATE
        /// </summary>
        private string _new_expiry_date;
        
        /// <summary>
        /// COLUMN: UPGRADE_AREA
        /// </summary>
        private string _upgrade_area;
        
        /// <summary>
        /// COLUMN: REASON_CODE
        /// </summary>
        private string _reason_code;
        
        /// <summary>
        /// COLUMN: UPDATE_DATE_TIME
        /// </summary>
        private string _update_date_time;
        
        /// <summary>
        /// COLUMN: SERVICE_FEE
        /// </summary>
        private string _service_fee;
        
        /// <summary>
        /// COLUMN: CARD_APP_MODE
        /// </summary>
        private string _card_app_mode;
        
        /// <summary>
        /// COLUMN: LINE_ID
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
        /// COLUMN: STATION_ID
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
        /// COLUMN: DEVICE_ID
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
        /// COLUMN: TRAN_TIME
        /// </summary>
        public string tran_time
        {
            get
            {
                return this._tran_time;
            }
            set
            {
                this._tran_time = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TRAN_TYPE
        /// </summary>
        public string tran_type
        {
            get
            {
                return this._tran_type;
            }
            set
            {
                this._tran_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TRAN_SUB_TYPE
        /// </summary>
        public string tran_sub_type
        {
            get
            {
                return this._tran_sub_type;
            }
            set
            {
                this._tran_sub_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: UDSN
        /// </summary>
        public string udsn
        {
            get
            {
                return this._udsn;
            }
            set
            {
                this._udsn = value;
            }
        }
        
        /// <summary>
        /// COLUMN: FILE_SN
        /// </summary>
        public string file_sn
        {
            get
            {
                return this._file_sn;
            }
            set
            {
                this._file_sn = value;
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
        /// COLUMN: RUN_DATE
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
        /// COLUMN: RECEIVE_DATE
        /// </summary>
        public string receive_date
        {
            get
            {
                return this._receive_date;
            }
            set
            {
                this._receive_date = value;
            }
        }
        
        /// <summary>
        /// COLUMN: RECEIVE_TIME
        /// </summary>
        public string receive_time
        {
            get
            {
                return this._receive_time;
            }
            set
            {
                this._receive_time = value;
            }
        }
        
        /// <summary>
        /// COLUMN: DEVICE_TYPE
        /// </summary>
        public string device_type
        {
            get
            {
                return this._device_type;
            }
            set
            {
                this._device_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: DEVICE_SUB_TYPE
        /// </summary>
        public string device_sub_type
        {
            get
            {
                return this._device_sub_type;
            }
            set
            {
                this._device_sub_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: SP_ID
        /// </summary>
        public string sp_id
        {
            get
            {
                return this._sp_id;
            }
            set
            {
                this._sp_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: SPA_ID
        /// </summary>
        public string spa_id
        {
            get
            {
                return this._spa_id;
            }
            set
            {
                this._spa_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: SAMID
        /// </summary>
        public string samid
        {
            get
            {
                return this._samid;
            }
            set
            {
                this._samid = value;
            }
        }
        
        /// <summary>
        /// COLUMN: MODE_CODE
        /// </summary>
        public string mode_code
        {
            get
            {
                return this._mode_code;
            }
            set
            {
                this._mode_code = value;
            }
        }
        
        /// <summary>
        /// COLUMN: OPERATOR_ID
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
        /// COLUMN: BOM_SHIFT_ID
        /// </summary>
        public string bom_shift_id
        {
            get
            {
                return this._bom_shift_id;
            }
            set
            {
                this._bom_shift_id = value;
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
        /// COLUMN: TRAN_VALUE
        /// </summary>
        public string tran_value
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
        /// COLUMN: REMAIN_VALUE
        /// </summary>
        public string remain_value
        {
            get
            {
                return this._remain_value;
            }
            set
            {
                this._remain_value = value;
            }
        }
        
        /// <summary>
        /// COLUMN: ORIGINAL_VALUE
        /// </summary>
        public string original_value
        {
            get
            {
                return this._original_value;
            }
            set
            {
                this._original_value = value;
            }
        }
        
        /// <summary>
        /// COLUMN: OVERDRAW_VALUE
        /// </summary>
        public string overdraw_value
        {
            get
            {
                return this._overdraw_value;
            }
            set
            {
                this._overdraw_value = value;
            }
        }
        
        /// <summary>
        /// COLUMN: MONTH_CUMULATE
        /// </summary>
        public string month_cumulate
        {
            get
            {
                return this._month_cumulate;
            }
            set
            {
                this._month_cumulate = value;
            }
        }
        
        /// <summary>
        /// COLUMN: FOREGIFT
        /// </summary>
        public string foregift
        {
            get
            {
                return this._foregift;
            }
            set
            {
                this._foregift = value;
            }
        }
        
        /// <summary>
        /// COLUMN: CARD_COST
        /// </summary>
        public string card_cost
        {
            get
            {
                return this._card_cost;
            }
            set
            {
                this._card_cost = value;
            }
        }
        
        /// <summary>
        /// COLUMN: FINE_AMOUNT
        /// </summary>
        public string fine_amount
        {
            get
            {
                return this._fine_amount;
            }
            set
            {
                this._fine_amount = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TICKET_COUNTER
        /// </summary>
        public string ticket_counter
        {
            get
            {
                return this._ticket_counter;
            }
            set
            {
                this._ticket_counter = value;
            }
        }
        
        /// <summary>
        /// COLUMN: PREFERENTIAL
        /// </summary>
        public string preferential
        {
            get
            {
                return this._preferential;
            }
            set
            {
                this._preferential = value;
            }
        }
        
        /// <summary>
        /// COLUMN: SERVICE_FEE_TYPE
        /// </summary>
        public string service_fee_type
        {
            get
            {
                return this._service_fee_type;
            }
            set
            {
                this._service_fee_type = value;
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
        /// COLUMN: EXPECT_TRAN_VALUE
        /// </summary>
        public string expect_tran_value
        {
            get
            {
                return this._expect_tran_value;
            }
            set
            {
                this._expect_tran_value = value;
            }
        }
        
        /// <summary>
        /// COLUMN: TRANSFER_DISCOUNT
        /// </summary>
        public string transfer_discount
        {
            get
            {
                return this._transfer_discount;
            }
            set
            {
                this._transfer_discount = value;
            }
        }
        
        /// <summary>
        /// COLUMN: PERSONAL_DISCOUNT
        /// </summary>
        public string personal_discount
        {
            get
            {
                return this._personal_discount;
            }
            set
            {
                this._personal_discount = value;
            }
        }
        
        /// <summary>
        /// COLUMN: PEAK_DISCOUNT
        /// </summary>
        public string peak_discount
        {
            get
            {
                return this._peak_discount;
            }
            set
            {
                this._peak_discount = value;
            }
        }
        
        /// <summary>
        /// COLUMN: REMAINING_RIDES
        /// </summary>
        public string remaining_rides
        {
            get
            {
                return this._remaining_rides;
            }
            set
            {
                this._remaining_rides = value;
            }
        }
        
        /// <summary>
        /// COLUMN: NUMBER_OF_RIDES
        /// </summary>
        public string number_of_rides
        {
            get
            {
                return this._number_of_rides;
            }
            set
            {
                this._number_of_rides = value;
            }
        }
        
        /// <summary>
        /// COLUMN: BANK_CARD_ID
        /// </summary>
        public string bank_card_id
        {
            get
            {
                return this._bank_card_id;
            }
            set
            {
                this._bank_card_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: BANK_ID
        /// </summary>
        public string bank_id
        {
            get
            {
                return this._bank_id;
            }
            set
            {
                this._bank_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: BANK_TXN_SN
        /// </summary>
        public string bank_txn_sn
        {
            get
            {
                return this._bank_txn_sn;
            }
            set
            {
                this._bank_txn_sn = value;
            }
        }
        
        /// <summary>
        /// COLUMN: ENTRY_STATION_ID
        /// </summary>
        public string entry_station_id
        {
            get
            {
                return this._entry_station_id;
            }
            set
            {
                this._entry_station_id = value;
            }
        }
        
        /// <summary>
        /// COLUMN: LAST_TRAN_DATE
        /// </summary>
        public string last_tran_date
        {
            get
            {
                return this._last_tran_date;
            }
            set
            {
                this._last_tran_date = value;
            }
        }
        
        /// <summary>
        /// COLUMN: LAST_TRAN_TIME
        /// </summary>
        public string last_tran_time
        {
            get
            {
                return this._last_tran_time;
            }
            set
            {
                this._last_tran_time = value;
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
        /// COLUMN: CARD_SN
        /// </summary>
        public string card_sn
        {
            get
            {
                return this._card_sn;
            }
            set
            {
                this._card_sn = value;
            }
        }
        
        /// <summary>
        /// COLUMN: CARD_PHY_ID
        /// </summary>
        public string card_phy_id
        {
            get
            {
                return this._card_phy_id;
            }
            set
            {
                this._card_phy_id = value;
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
        /// COLUMN: PRODUCT_TYPE
        /// </summary>
        public string product_type
        {
            get
            {
                return this._product_type;
            }
            set
            {
                this._product_type = value;
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
        /// COLUMN: PTSN
        /// </summary>
        public string ptsn
        {
            get
            {
                return this._ptsn;
            }
            set
            {
                this._ptsn = value;
            }
        }
        
        /// <summary>
        /// COLUMN: RETURN_REASON
        /// </summary>
        public string return_reason
        {
            get
            {
                return this._return_reason;
            }
            set
            {
                this._return_reason = value;
            }
        }
        
        /// <summary>
        /// COLUMN: REFUND_REASON
        /// </summary>
        public string refund_reason
        {
            get
            {
                return this._refund_reason;
            }
            set
            {
                this._refund_reason = value;
            }
        }
        
        /// <summary>
        /// COLUMN: REFUND_TYPE
        /// </summary>
        public string refund_type
        {
            get
            {
                return this._refund_type;
            }
            set
            {
                this._refund_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: RECEIPT_NO
        /// </summary>
        public string receipt_no
        {
            get
            {
                return this._receipt_no;
            }
            set
            {
                this._receipt_no = value;
            }
        }
        
        /// <summary>
        /// COLUMN: BLOCK_REASON
        /// </summary>
        public string block_reason
        {
            get
            {
                return this._block_reason;
            }
            set
            {
                this._block_reason = value;
            }
        }
        
        /// <summary>
        /// COLUMN: SAMID_TAC
        /// </summary>
        public string samid_tac
        {
            get
            {
                return this._samid_tac;
            }
            set
            {
                this._samid_tac = value;
            }
        }
        
        /// <summary>
        /// COLUMN: YKT_AUTH_SEQ
        /// </summary>
        public string ykt_auth_seq
        {
            get
            {
                return this._ykt_auth_seq;
            }
            set
            {
                this._ykt_auth_seq = value;
            }
        }
        
        /// <summary>
        /// COLUMN: ONLINE_TNX_SEQ
        /// </summary>
        public string online_tnx_seq
        {
            get
            {
                return this._online_tnx_seq;
            }
            set
            {
                this._online_tnx_seq = value;
            }
        }
        
        /// <summary>
        /// COLUMN: AREA_CODE
        /// </summary>
        public string area_code
        {
            get
            {
                return this._area_code;
            }
            set
            {
                this._area_code = value;
            }
        }
        
        /// <summary>
        /// COLUMN: AMOUNT_TYPE
        /// </summary>
        public string amount_type
        {
            get
            {
                return this._amount_type;
            }
            set
            {
                this._amount_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: BUS_TIMES
        /// </summary>
        public string bus_times
        {
            get
            {
                return this._bus_times;
            }
            set
            {
                this._bus_times = value;
            }
        }
        
        /// <summary>
        /// COLUMN: BUS_TIMES_COUNTER
        /// </summary>
        public string bus_times_counter
        {
            get
            {
                return this._bus_times_counter;
            }
            set
            {
                this._bus_times_counter = value;
            }
        }
        
        /// <summary>
        /// COLUMN: METRO_TIMES_COUNTER
        /// </summary>
        public string metro_times_counter
        {
            get
            {
                return this._metro_times_counter;
            }
            set
            {
                this._metro_times_counter = value;
            }
        }
        
        /// <summary>
        /// COLUMN: UNION_TIMES_COUNTER
        /// </summary>
        public string union_times_counter
        {
            get
            {
                return this._union_times_counter;
            }
            set
            {
                this._union_times_counter = value;
            }
        }
        
        /// <summary>
        /// COLUMN: OLD_EXPIRY_DATE
        /// </summary>
        public string old_expiry_date
        {
            get
            {
                return this._old_expiry_date;
            }
            set
            {
                this._old_expiry_date = value;
            }
        }
        
        /// <summary>
        /// COLUMN: NEW_EXPIRY_DATE
        /// </summary>
        public string new_expiry_date
        {
            get
            {
                return this._new_expiry_date;
            }
            set
            {
                this._new_expiry_date = value;
            }
        }
        
        /// <summary>
        /// COLUMN: UPGRADE_AREA
        /// </summary>
        public string upgrade_area
        {
            get
            {
                return this._upgrade_area;
            }
            set
            {
                this._upgrade_area = value;
            }
        }
        
        /// <summary>
        /// COLUMN: REASON_CODE
        /// </summary>
        public string reason_code
        {
            get
            {
                return this._reason_code;
            }
            set
            {
                this._reason_code = value;
            }
        }
        
        /// <summary>
        /// COLUMN: UPDATE_DATE_TIME
        /// </summary>
        public string update_date_time
        {
            get
            {
                return this._update_date_time;
            }
            set
            {
                this._update_date_time = value;
            }
        }
        
        /// <summary>
        /// COLUMN: SERVICE_FEE
        /// </summary>
        public string service_fee
        {
            get
            {
                return this._service_fee;
            }
            set
            {
                this._service_fee = value;
            }
        }
        
        /// <summary>
        /// COLUMN: CARD_APP_MODE
        /// </summary>
        public string card_app_mode
        {
            get
            {
                return this._card_app_mode;
            }
            set
            {
                this._card_app_mode = value;
            }
        }
    }
}
