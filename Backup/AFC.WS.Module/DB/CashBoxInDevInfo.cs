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
    /// 数据库表名称：cash_box_in_dev_info
    /// </summary>
    public class CashBoxInDevInfo
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
        /// 设备id
        /// </summary>
        private string _device_id;
        
        /// <summary>
        /// 在设备上的安装位置。在设备内按照从上到下从左到右的顺序排序编码，从01开始
        /// </summary>
        private string _position_in_dev;
        
        /// <summary>
        /// 钱箱id
        /// </summary>
        private string _money_box_id;
        
        /// <summary>
        /// 01：正常安装；02：非法安装；03：正常卸下；04：非法卸下；
        /// </summary>
        private string _install_status;
        
        /// <summary>
        /// 00：正常；01：将空；02：已空；03：将满；04：已满
        /// </summary>
        private string _currency_store_status;
        
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
        /// 00：正常；01：将空；02：已空；03：将满；04：已满
        /// </summary>
        public string currency_store_status
        {
            get
            {
                return this._currency_store_status;
            }
            set
            {
                this._currency_store_status = value;
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
