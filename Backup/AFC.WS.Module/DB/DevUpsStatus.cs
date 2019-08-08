using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.DB
{
    public class DevUpsStatus
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

        /// <summary>
        /// COLUMN: POWER_PERCENT
        /// </summary>
        private string _power_percent;

        public string power_percent
        {
            get
            {
                return this._power_percent;
            }
            set
            {
                this._power_percent = value;
            }
        }

        /// <summary>
        /// COLUMN: POWER_STATUS
        /// </summary>
        private string _power_status;

        public string power_status
        {
            get
            {
                return this._power_status;
            }
            set
            {
                this._power_status = value;
            }
        }

        /// <summary>
        /// COLUMN: UPS_STATUS
        /// </summary>
        private string _ups_status;

        public string ups_status
        {
            get
            {
                return this._ups_status;
            }
            set
            {
                this._ups_status = value;
            }
        }

        /// <summary>
        /// COLUMN: IS_OFF
        /// </summary>
        private string _is_off;

        public string is_off
        {
            get
            {
                return this._is_off;
            }
            set
            {
                this._is_off = value;
            }
        }

        /// <summary>
        /// COLUMN: SHUT_DATE
        /// </summary>
        private string _shut_date;

        public string shut_date
        {
            get
            {
                return this._shut_date;
            }
            set
            {
                this._shut_date = value;
            }
        }

        /// <summary>
        /// COLUMN: SHUT_TIME
        /// </summary>
        private string _shut_time;

        public string shut_time
        {
            get
            {
                return this._shut_time;
            }
            set
            {
                this._shut_time = value;
            }
        }

        /// <summary>
        /// COLUMN: OPERATOR_ID
        /// </summary>
        private string _operator_id;

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
        /// COLUMN: UPDATE_DATE
        /// </summary>
        private string _update_date;

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
        /// COLUMN: UPDATE_TIME
        /// </summary>
        private string _update_time;

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
