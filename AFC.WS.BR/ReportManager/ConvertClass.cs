using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Config;

namespace AFC.WS.BR.ReportManager
{
    public static class ConvertClass
    {
        #region --> 日期、时间格式。
        /// <summary>
        /// 将日期类型转成yyyy-MM-dd格式。
        /// </summary>
        /// <param name="dt">时间</param>
        /// <returns>返回转成yyyy-MM-dd格式的时间</returns>
        public static string ToyyyyMMdd(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static string ToYearMonthDay(this string value)
        {
            string[] buffer = value.Split('-');
            StringBuilder sb = new StringBuilder();
            sb.Append(buffer[0]);
            sb.Append("年");
            sb.Append(buffer[1]);
            sb.Append("月");
            sb.Append(buffer[2]);
            sb.Append("日");
            return sb.ToString();
        }

        /// <summary>
        /// 将非yyyy-MM-dd格式的日期转成yyyy-MM-dd日期格式。
        /// </summary>
        /// <param name="s">字符串时间</param>
        /// <returns>返回转成yyyy-MM-dd格式的时间</returns>
        public static string ToyyyyMMdd(this string s)
        {
            string result = null;
            try
            {
                if (String.IsNullOrEmpty(s))
                {
                    result = s;
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    dt = Convert.ToDateTime(s);
                    result = dt.ToyyyyMMdd();
                }
            }
            catch (Exception ee)
            {
                result = s;
                Wrapper.Instance.ConsoleWriteLine("将[" + s + "]转为日期时出错。", LogFlag.InfoFormat);
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return result;
        }

        /// <summary>
        /// 将101010 转为 10:10:10格式
        /// </summary>
        /// <param name="s">时间字符串</param>
        /// <returns>返回将101010 转为 10:10:10格式字符串</returns>
        public static string FormatToTime(this string s)
        {
            string result = "";
            try
            {
                switch (s.Length)
                {
                    case 4:
                        result = s.Substring(0, 2) + ":" + s.Substring(2);
                        break;
                    case 5:
                        result = s.Substring(0, 1) + ":" + s.Substring(1, 2) + ":" + s.Substring(3);
                        break;
                    case 6:
                        result = s.Substring(0, 2) + ":" + s.Substring(2, 2) + ":" + s.Substring(4);
                        break;
                    default:
                        result = s;
                        break;
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }
            return result;
        }

        /// <summary>
        /// 将yyyyMMddHHmmss转为2010-01-29 12:27
        /// </summary>
        /// <param name="s">yyyyMMddHHmmss时间格式</param>
        /// <returns>返回将yyyyMMddHHmmss转为2010-01-29 12:27字符串</returns>
        public static string FormatToDateTime(this string s)
        {
            try
            {
                DateTime d = DateTime.ParseExact(s, "yyyyMMddHHmmss", null);
                if (d != null)
                {
                    return d.ToString("yyyy-MM-dd HH:mm:ss");
                }
                return s;
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                return s;
            }
        }

        #endregion --> 日期、时间格式。
    }
}
