using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.TimeSyn
{
    /// <summary>
    /// 系统时间 调用API格式时间
    /// </summary>
    public struct SystemTime
    {
        public ushort wYear;
        public ushort wMonth;
        public ushort wDayOfWeek;
        public ushort wDay;
        public ushort wHour;
        public ushort wMinute;
        public ushort wSecond;
        public ushort wMilliseconds;
    }
}
