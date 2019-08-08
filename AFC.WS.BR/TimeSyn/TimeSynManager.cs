using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.TimeSyn
{
    using AFC.WS.UI.Common;
    using System.Runtime.InteropServices;
    /// <summary>
    /// 时钟管理
    /// </summary>
    public class TimeSynManager
    {
        /// <summary>
        /// 从kernel32.dll引入API：SetLocalTime，设置系统时间
        /// </summary>
        /// <param name="lpSystemTime"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int SetLocalTime(ref SystemTime lpSystemTime);

        /// <summary>
        /// 时钟同步
        /// </summary>
        /// <returns>0:成功，其他:失败</returns>
        public int TimeSyn()
        {
            //time.nist.gov
            NTPClient client;
            try
            {
                string ipAddress = SysConfig.GetSysConfig().CommParamsConfig.ScIPAddress;
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    client = new NTPClient(ipAddress);
                    client.Connect();
                    SystemTime systemTime = DateTimeToSystemTime(client.ReceiveTimestamp);
                    SetLocalTime(ref systemTime);
                    return 0;
                }
                else
                {
                    WriteLog.Log_Info("IP地址为空，请查看是否配置IP地址!");
                    return -1;
                }

            }
            catch (Exception e)
            {
                WriteLog.Log_Error("ERROR: " + e.Message);
                return -1;
            }
        }


        /// <summary>
        /// 转换为系统时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private SystemTime DateTimeToSystemTime(DateTime time)
        {
            SystemTime st = new SystemTime();

            st.wYear = (ushort)time.Year;

            st.wMonth = (ushort)time.Month;

            st.wDayOfWeek = (ushort)time.DayOfWeek;

            st.wDay = (ushort)time.Day;

            st.wHour = (ushort)time.Hour;

            st.wMinute = (ushort)(time.Minute);

            st.wSecond = (ushort)time.Second;

            st.wMilliseconds = (ushort)time.Millisecond;

            return st;

        }
    }
}
