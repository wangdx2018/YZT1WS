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
    /// 数据库表名称：para_4042_device_info
    /// </summary>
    public class Para4042DeviceInfo
    {
        
        /// <summary>
        /// 版本号
        /// </summary>
        private string _para_version;
        
        /// <summary>
        /// 见《01系统编码》
        /// </summary>
        private string _line_id;
        
        /// <summary>
        /// 车站id
        /// </summary>
        private string _station_id;
        
        /// <summary>
        /// png格式
        /// </summary>
        private string _station_map_name;
        
        /// <summary>
        /// 设备id
        /// </summary>
        private string _device_id;
        
        /// <summary>
        /// 设备名称
        /// </summary>
        private string _device_name;
        
        /// <summary>
        /// 设备类型
        /// </summary>
        private string _device_type;
        
        /// <summary>
        /// 设备子类型
        /// </summary>
        private string _device_sub_type;
        
        /// <summary>
        /// 车站内设备的逻辑序号
        /// </summary>
        private string _device_serial_no;
        
        /// <summary>
        /// 设备所属站厅
        /// </summary>
        private string _station_hall_id;
        
        /// <summary>
        /// 设备所属组
        /// </summary>
        private string _device_group_id;
        
        /// <summary>
        /// 设备在组内的序号
        /// </summary>
        private string _device_group_serial_no;
        
        /// <summary>
        /// COLUMN: HONRI_INDEX
        /// </summary>
        private string _honri_index;
        
        /// <summary>
        /// COLUMN: VERTICAL_INDEX
        /// </summary>
        private string _vertical_index;
        
        /// <summary>
        /// COLUMN: DISPLAY_ANGLE
        /// </summary>
        private string _display_angle;
        
        /// <summary>
        /// ip的4个字节以u32_t从高位到低位表示
        /// </summary>
        private string _device_ip;
        
        /// <summary>
        /// 1：在用；2：停用；3：移除
        /// </summary>
        private string _start_flag;
        
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
        /// png格式
        /// </summary>
        public string station_map_name
        {
            get
            {
                return this._station_map_name;
            }
            set
            {
                this._station_map_name = value;
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
        /// 设备名称
        /// </summary>
        public string device_name
        {
            get
            {
                return this._device_name;
            }
            set
            {
                this._device_name = value;
            }
        }
        
        /// <summary>
        /// 设备类型
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
        /// 设备子类型
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
        /// 车站内设备的逻辑序号
        /// </summary>
        public string device_serial_no
        {
            get
            {
                return this._device_serial_no;
            }
            set
            {
                this._device_serial_no = value;
            }
        }
        
        /// <summary>
        /// 设备所属站厅
        /// </summary>
        public string station_hall_id
        {
            get
            {
                return this._station_hall_id;
            }
            set
            {
                this._station_hall_id = value;
            }
        }
        
        /// <summary>
        /// 设备所属组
        /// </summary>
        public string device_group_id
        {
            get
            {
                return this._device_group_id;
            }
            set
            {
                this._device_group_id = value;
            }
        }
        
        /// <summary>
        /// 设备在组内的序号
        /// </summary>
        public string device_group_serial_no
        {
            get
            {
                return this._device_group_serial_no;
            }
            set
            {
                this._device_group_serial_no = value;
            }
        }
        
        /// <summary>
        /// COLUMN: HONRI_INDEX
        /// </summary>
        public string honri_index
        {
            get
            {
                return this._honri_index;
            }
            set
            {
                this._honri_index = value;
            }
        }
        
        /// <summary>
        /// COLUMN: VERTICAL_INDEX
        /// </summary>
        public string vertical_index
        {
            get
            {
                return this._vertical_index;
            }
            set
            {
                this._vertical_index = value;
            }
        }
        
        /// <summary>
        /// COLUMN: DISPLAY_ANGLE
        /// </summary>
        public string display_angle
        {
            get
            {
                return this._display_angle;
            }
            set
            {
                this._display_angle = value;
            }
        }
        
        /// <summary>
        /// ip的4个字节以u32_t从高位到低位表示
        /// </summary>
        public string device_ip
        {
            get
            {
                return this._device_ip;
            }
            set
            {
                this._device_ip = value;
            }
        }
        
        /// <summary>
        /// 1：在用；2：停用；3：移除
        /// </summary>
        public string start_flag
        {
            get
            {
                return this._start_flag;
            }
            set
            {
                this._start_flag = value;
            }
        }
    }
}