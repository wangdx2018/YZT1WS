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
    /// 数据库表名称：data_file_deal_info
    /// </summary>
    public class DataFileDealInfo
    {
        
        /// <summary>
        /// 设备编码
        /// </summary>
        private string _device_id;
        
        /// <summary>
        /// COLUMN: FILE_TYPE
        /// </summary>
        private string _file_type;
        
        /// <summary>
        /// COLUMN: FILE_SUB_TYPE
        /// </summary>
        private string _file_sub_type;
        
        /// <summary>
        /// COLUMN: FILE_NAME
        /// </summary>
        private string _file_name;
        
        /// <summary>
        /// COLUMN: FILE_SN
        /// </summary>
        private string _file_sn;
        
        /// <summary>
        /// COLUMN: DEAL_DATE
        /// </summary>
        private string _deal_date;
        
        /// <summary>
        /// COLUMN: DEAL_TIME
        /// </summary>
        private string _deal_time;
        
        /// <summary>
        /// COLUMN: DEAL_RESULT
        /// </summary>
        private string _deal_result;
        
        /// <summary>
        /// COLUMN: RECORD_NUM
        /// </summary>
        private decimal _record_num;
        
        /// <summary>
        /// 1－md5签名错误
        ///2－设备黑名单
        ///3－sam卡黑名单
        ///4－不匹配的交易笔数
        ///5－解码失败
        ///
        /// </summary>
        private decimal _error_no;
        
        /// <summary>
        /// COLUMN: ERROR_CODE
        /// </summary>
        private string _error_code;
        
        /// <summary>
        /// 打包标记：00：已打包；01：未打包
        /// </summary>
        private string _is_pack;
        
        /// <summary>
        /// COLUMN: PACK_DATE
        /// </summary>
        private string _pack_date;
        
        /// <summary>
        /// COLUMN: PACK_TIME
        /// </summary>
        private string _pack_time;
        
        /// <summary>
        /// COLUMN: UP_FILE_SN
        /// </summary>
        private string _up_file_sn;
        
        /// <summary>
        /// 运营日
        /// </summary>
        private string _run_date;
        
        /// <summary>
        /// COLUMN: FILE_SIZE
        /// </summary>
        private decimal _file_size;
        
        /// <summary>
        /// 接收日期
        /// </summary>
        private string _receive_date;
        
        /// <summary>
        /// 接收时间
        /// </summary>
        private string _receive_time;
        
        /// <summary>
        /// 设备编码
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
        /// COLUMN: FILE_TYPE
        /// </summary>
        public string file_type
        {
            get
            {
                return this._file_type;
            }
            set
            {
                this._file_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: FILE_SUB_TYPE
        /// </summary>
        public string file_sub_type
        {
            get
            {
                return this._file_sub_type;
            }
            set
            {
                this._file_sub_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: FILE_NAME
        /// </summary>
        public string file_name
        {
            get
            {
                return this._file_name;
            }
            set
            {
                this._file_name = value;
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
        /// COLUMN: DEAL_DATE
        /// </summary>
        public string deal_date
        {
            get
            {
                return this._deal_date;
            }
            set
            {
                this._deal_date = value;
            }
        }
        
        /// <summary>
        /// COLUMN: DEAL_TIME
        /// </summary>
        public string deal_time
        {
            get
            {
                return this._deal_time;
            }
            set
            {
                this._deal_time = value;
            }
        }
        
        /// <summary>
        /// COLUMN: DEAL_RESULT
        /// </summary>
        public string deal_result
        {
            get
            {
                return this._deal_result;
            }
            set
            {
                this._deal_result = value;
            }
        }
        
        /// <summary>
        /// COLUMN: RECORD_NUM
        /// </summary>
        public decimal record_num
        {
            get
            {
                return this._record_num;
            }
            set
            {
                this._record_num = value;
            }
        }
        
        /// <summary>
        /// 1－md5签名错误
        ///2－设备黑名单
        ///3－sam卡黑名单
        ///4－不匹配的交易笔数
        ///5－解码失败
        ///
        /// </summary>
        public decimal error_no
        {
            get
            {
                return this._error_no;
            }
            set
            {
                this._error_no = value;
            }
        }
        
        /// <summary>
        /// COLUMN: ERROR_CODE
        /// </summary>
        public string error_code
        {
            get
            {
                return this._error_code;
            }
            set
            {
                this._error_code = value;
            }
        }
        
        /// <summary>
        /// 打包标记：00：已打包；01：未打包
        /// </summary>
        public string is_pack
        {
            get
            {
                return this._is_pack;
            }
            set
            {
                this._is_pack = value;
            }
        }
        
        /// <summary>
        /// COLUMN: PACK_DATE
        /// </summary>
        public string pack_date
        {
            get
            {
                return this._pack_date;
            }
            set
            {
                this._pack_date = value;
            }
        }
        
        /// <summary>
        /// COLUMN: PACK_TIME
        /// </summary>
        public string pack_time
        {
            get
            {
                return this._pack_time;
            }
            set
            {
                this._pack_time = value;
            }
        }
        
        /// <summary>
        /// COLUMN: UP_FILE_SN
        /// </summary>
        public string up_file_sn
        {
            get
            {
                return this._up_file_sn;
            }
            set
            {
                this._up_file_sn = value;
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
        /// COLUMN: FILE_SIZE
        /// </summary>
        public decimal file_size
        {
            get
            {
                return this._file_size;
            }
            set
            {
                this._file_size = value;
            }
        }
        
        /// <summary>
        /// 接收日期
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
        /// 接收时间
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
    }
}
