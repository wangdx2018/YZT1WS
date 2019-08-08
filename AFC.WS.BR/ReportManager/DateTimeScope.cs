using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.ReportManager
{
    /// <summary>
    ///  时间范围
    /// </summary>
    public class DateTimeScope
    {

        string reportDateBegin;
        string reportDateEnd;
        string tranDateBegin;
        string tranDateEnd;
        string bissDateBegin;
        string bissDateEnd;
        string runDateBegin;
        string runDateEnd;
        string timeBegin;
        string timeEnd;
        string tranDate;
        string runDate;
        string month;
        string year;

        /// <summary>
        /// 月
        /// </summary>
        public string Month
        {
            get { return month; }
            set { month = value; }
        }
        /// <summary>
        /// 年
        /// </summary>
        public string Year
        {
            get { return year; }
            set { year = value; }
        }

        /// <summary>
        /// 运营日，从数据库中获取得来的。
        /// </summary>
        public string RunDate
        {
            get { return runDate; }
            set { runDate = value; }
        }

        /// <summary>
        /// 交易自然日
        /// </summary>
        public string TranDate
        {
            get { return tranDate; }
            set { tranDate = value; }
        }

        /// <summary>
        /// 交易自然开始日期
        /// </summary>
        public string TranDateBegin
        {
            get { return tranDateBegin; }
            set { tranDateBegin = value; }
        }
        /// <summary>
        /// 开始时间(yyyyMMdd)
        /// </summary>
        public string BissDateBegin
        {
            get { return bissDateBegin; }
            set { bissDateBegin = value; }
        }
        /// <summary>
        /// 运营开始日期(yyyyMMdd)
        /// </summary>
        public string RunDateBegin
        {
            get { return runDateBegin; }
            set { runDateBegin = value; }
        }
        /// <summary>
        /// 运营结束日期(yyyyMMdd)
        /// </summary>
        public string RunDateEnd
        {
            get { return runDateEnd; }
            set { runDateEnd = value; }
        }

        /// <summary>
        /// 运营开始时间(mmss)
        /// </summary>
        public string TimeBegin
        {
            get { return timeBegin; }
            set { timeBegin = value; }
        }

        /// <summary>
        /// 报表开始日期(yyyyMMdd)
        /// </summary>
        public string ReportDateBegin
        {
            get { return reportDateBegin; }
            set { reportDateBegin = value; }
        }
        /// <summary>
        /// 交易自然结束日期
        /// </summary>
        public string TranDateEnd
        {
            get { return tranDateEnd; }
            set { tranDateEnd = value; }
        }
        /// <summary>
        /// 结算日结束日期(yyyyMMdd)
        /// </summary>
        public string BissDateEnd
        {
            get { return bissDateEnd; }
            set { bissDateEnd = value; }
        }
        /// <summary>
        /// 报表结束日期(yyyyMMdd)
        /// </summary>
        public string ReportDateEnd
        {
            get { return reportDateEnd; }
            set { reportDateEnd = value; }
        }
        /// <summary>
        /// 运营结束时间(mmss)
        /// </summary>
        public string TimeEnd
        {
            get { return timeEnd; }
            set { timeEnd = value; }
        }
    }
}
