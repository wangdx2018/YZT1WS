using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.DB
{
    /// <summary>
    /// 数据库表名称：dev_ups_map
    /// </summary>
    public class DevUpsMap
    {
        /// <summary>
        /// COLUMN: UPS_ID
        /// </summary>
        private string _ups_id;

        public string ups_id
        {
            get
            {
                return this._ups_id;
            }
            set
            {
                this._ups_id = value;
            }
        }

        /// <summary>
        /// COLUMN: DEVICE_ID
        /// </summary>
        private string _device_id;

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

    }
}
