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
    /// 数据库表名称：dev_status_alarm_cfg
    /// </summary>
    public class DevStatusAlarmCfg
    {
        
        /// <summary>
        /// 设备id
        /// </summary>
        private string _device_id;
        
        /// <summary>
        /// 00:正常服务;01:报警;02:故障;03:通讯中断
        ///
        /// </summary>
        private string _run_status;
        
        /// <summary>
        /// 00：不报警;01：闪烁图标;02：弹出提示;03：声音
        ///
        /// </summary>
        private string _alarm_style;
        
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
        /// 00:正常服务;01:报警;02:故障;03:通讯中断
        ///
        /// </summary>
        public string run_status
        {
            get
            {
                return this._run_status;
            }
            set
            {
                this._run_status = value;
            }
        }
        
        /// <summary>
        /// 00：不报警;01：闪烁图标;02：弹出提示;03：声音
        ///
        /// </summary>
        public string alarm_style
        {
            get
            {
                return this._alarm_style;
            }
            set
            {
                this._alarm_style = value;
            }
        }
    }
}
