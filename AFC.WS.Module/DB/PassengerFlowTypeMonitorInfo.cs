using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.DB
{
    /// <summary>
    /// 客流类型:分三段，第段三列。
    /// </summary>
    public class PassengerFlowTypeMonitorInfo
    {
        string _PassengerFlowTotal;
        string _PassengerFlowTotal1;
        string _PassengerFlowTotal2;

        /// <summary>
        /// 客流总数。
        /// </summary>
        public string PassengerFlowTotal2
        {
            get { return _PassengerFlowTotal2; }
            set { _PassengerFlowTotal2 = value; }
        }

        /// <summary>
        /// 客流总数。
        /// </summary>
        public string PassengerFlowTotal1
        {
            get { return _PassengerFlowTotal1; }
            set { _PassengerFlowTotal1 = value; }
        }

        string _CurrentPagePassengerFlowNumber;
        string _CurrentPagePassengerFlowNumber1;
        string _CurrentPagePassengerFlowNumber2;

        /// <summary>
        /// 当前页面的客流数量。
        /// </summary>
        public string CurrentPagePassengerFlowNumber2
        {
            get { return _CurrentPagePassengerFlowNumber2; }
            set { _CurrentPagePassengerFlowNumber2 = value; }
        }

        /// <summary>
        /// 当前页面的客流数量。
        /// </summary>
        public string CurrentPagePassengerFlowNumber1
        {
            get { return _CurrentPagePassengerFlowNumber1; }
            set { _CurrentPagePassengerFlowNumber1 = value; }
        }

        string _PassengerFlowTypeName;
        string _PassengerFlowTypeName1;
        string _PassengerFlowTypeName2;

        /// <summary>
        /// 客流类型名称。
        /// </summary>
        public string PassengerFlowTypeName2
        {
            get { return _PassengerFlowTypeName2; }
            set { _PassengerFlowTypeName2 = value; }
        }

        /// <summary>
        /// 客流类型名称。
        /// </summary>
        public string PassengerFlowTypeName1
        {
            get { return _PassengerFlowTypeName1; }
            set { _PassengerFlowTypeName1 = value; }
        }


        /// <summary>
        /// 卡发行商ID：一卡通、一票通。
        /// </summary>
        string _CardIssueName;
        string _CardIssueName1;
        string _CardIssueName2;

        /// <summary>
        /// 卡发行商名称：一卡通、一票通。
        /// </summary>
        public string CardIssueName2
        {
            get { return _CardIssueName2; }
            set { _CardIssueName2 = value; }
        }

        /// <summary>
        /// 卡发行商名称：一卡通、一票通。
        /// </summary>
        public string CardIssueName1
        {
            get { return _CardIssueName1; }
            set { _CardIssueName1 = value; }
        }

        /// <summary>
        /// 卡发行商名称：一卡通、一票通。
        /// </summary>
        public string CardIssueName
        {
            get { return _CardIssueName; }
            set { _CardIssueName = value; }
        }

        /// <summary>
        /// 客流类型名称。
        /// </summary>
        public string PassengerFlowTypeName
        {
            get { return _PassengerFlowTypeName; }
            set { _PassengerFlowTypeName = value; }
        }

        /// <summary>
        /// 客流总数。
        /// </summary>
        public string PassengerFlowTotal
        {
            get { return _PassengerFlowTotal; }
            set { _PassengerFlowTotal = value; }
        }

        /// <summary>
        /// 当前页面的客流数量。
        /// </summary>
        public string CurrentPagePassengerFlowNumber
        {
            get { return _CurrentPagePassengerFlowNumber; }
            set { _CurrentPagePassengerFlowNumber = value; }
        }
    }
}
