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
    /// 数据库表名称：basi_oper_code_info
    /// </summary>
    public class BasiOperCodeInfo
    {
        
        /// <summary>
        /// COLUMN: OPER_CODE
        /// </summary>
        private string _oper_code;
        
        /// <summary>
        /// 操作类型：1：发布
        ///2：接收
        ///2：生效
        ///3：淘汰
        ///
        /// </summary>
        private string _oper_type;
        
        /// <summary>
        /// COLUMN: OPER_CLASS
        /// </summary>
        private string _oper_class;
        
        /// <summary>
        /// 设备类型
        /// </summary>
        private string _device_type;
        
        /// <summary>
        /// COLUMN: OPER_CONTENT
        /// </summary>
        private string _oper_content;
        
        /// <summary>
        /// COLUMN: OPER_CODE
        /// </summary>
        public string oper_code
        {
            get
            {
                return this._oper_code;
            }
            set
            {
                this._oper_code = value;
            }
        }
        
        /// <summary>
        /// 操作类型：1：发布
        ///2：接收
        ///2：生效
        ///3：淘汰
        ///
        /// </summary>
        public string oper_type
        {
            get
            {
                return this._oper_type;
            }
            set
            {
                this._oper_type = value;
            }
        }
        
        /// <summary>
        /// COLUMN: OPER_CLASS
        /// </summary>
        public string oper_class
        {
            get
            {
                return this._oper_class;
            }
            set
            {
                this._oper_class = value;
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
        /// COLUMN: OPER_CONTENT
        /// </summary>
        public string oper_content
        {
            get
            {
                return this._oper_content;
            }
            set
            {
                this._oper_content = value;
            }
        }
    }
}
