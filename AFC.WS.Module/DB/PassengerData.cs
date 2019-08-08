using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.DB
{
    /// <summary>
    /// 客流数据类
    /// </summary>
    public class PassengerData
    {
        string _datetimeSegment;
        decimal _total;
        string _cardIssueName;
        decimal _card_issuer_id;
        string _pass_type_id;
        string _station_cn_name;
        string _station_hall_name;
        string _station_hall_id;
        string _pass_type_name;
        string _station_id;
        /// <summary>
        /// 车站ID
        /// </summary>
        public string Station_id
        {
            get { return _station_id; }
            set { _station_id = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string pass_type_name
        {
            get { return _pass_type_name; }
            set { _pass_type_name = value; }
        }
        /// <summary>
        /// 站厅ID
        /// </summary>
        public string Station_hall_id
        {
            get { return _station_hall_id; }
            set { _station_hall_id = value; }
        }
        /// <summary>
        /// 站厅名称
        /// </summary>
        public string Station_hall_name
        {
            get { return _station_hall_name; }
            set { _station_hall_name = value; }
        }
        /// <summary>
        /// 车站。
        /// </summary>
        public string Station_cn_name
        {
            get { return _station_cn_name; }
            set { _station_cn_name = value; }
        }
        /// <summary>
        /// 客流类型
        /// </summary>
        public string pass_type_id
        {
            get { return _pass_type_id; }
            set { _pass_type_id = value; }
        }
        /// <summary>
        /// 发行商ID
        /// </summary>
        public decimal Card_issuer_id
        {
            get { return _card_issuer_id; }
            set { _card_issuer_id = value; }
        }
        /// <summary>
        /// 发行商名称。
        /// </summary>
        public string CardIssueName
        {
            get { return _cardIssueName; }
            set { _cardIssueName = value; }
        }
        /// <summary>
        /// 总数量。
        /// </summary>
        public decimal Total
        {
            get { return _total; }
            set { _total = value; }
        }
        /// <summary>
        /// 时间段。
        /// </summary>
        public string DatetimeSegment
        {
            get { return _datetimeSegment; }
            set { _datetimeSegment = value; }
        }
    }
}
