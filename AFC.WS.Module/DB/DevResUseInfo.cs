//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.4963
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
    /// 数据库表名称：dev_res_use_info
    /// </summary>
    public class DevResUseInfo
    {
        
        /// <summary>
        /// 设备id
        /// </summary>
        private string _device_id;
        
        /// <summary>
        /// 磁盘空间总容量
        /// </summary>
        private decimal _total_disk_volume;
        
        /// <summary>
        /// 当前磁盘使用量
        /// </summary>
        private decimal _used_disk_volume;
        
        /// <summary>
        /// 当前数据库文件容量
        /// </summary>
        private decimal _db_file_volume;
        
        /// <summary>
        /// 内存总容量
        /// </summary>
        private decimal _mem_volume;
        
        /// <summary>
        /// cpu总容量
        /// </summary>
        private decimal _cpu_volume;

        /// <summary>
        /// 硬盘状况
        /// </summary>
        private decimal _db_status;
        
        /// <summary>
        /// 更新日期
        /// </summary>
        private string _update_date;
        
        /// <summary>
        /// 更新时间
        /// </summary>
        private string _update_time;
        
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
        /// 磁盘空间总容量
        /// </summary>
        public decimal total_disk_volume
        {
            get
            {
                return this._total_disk_volume;
            }
            set
            {
                this._total_disk_volume = value;
            }
        }
        
        /// <summary>
        /// 当前磁盘使用量
        /// </summary>
        public decimal used_disk_volume
        {
            get
            {
                return this._used_disk_volume;
            }
            set
            {
                this._used_disk_volume = value;
            }
        }
        
        /// <summary>
        /// 当前数据库文件容量
        /// </summary>
        public decimal db_file_volume
        {
            get
            {
                return this._db_file_volume;
            }
            set
            {
                this._db_file_volume = value;
            }
        }
        
        /// <summary>
        /// 内存总容量
        /// </summary>
        public decimal mem_volume
        {
            get
            {
                return this._mem_volume;
            }
            set
            {
                this._mem_volume = value;
            }
        }
        
        /// <summary>
        /// cpu总容量
        /// </summary>
        public decimal cpu_volume
        {
            get
            {
                return this._cpu_volume;
            }
            set
            {
                this._cpu_volume = value;
            }
        }

        /// <summary>
        /// 硬盘状况
        /// </summary>
        public decimal db_status
        {
            get
            {
                return this._db_status;
            }
            set
            {
                this._db_status = value;
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
