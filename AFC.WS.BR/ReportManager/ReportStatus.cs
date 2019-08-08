using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.ReportManager
{
    /// <summary>
    /// 报表状态。
    /// </summary>
    public enum ReportStatus
    {
        None = 0,
        Success = 1,
        CreatingReport = 20,
        CreateReportError = -21,

        PrintingReport = 30,
        PrintingReportError = -31,
        PrintReportSuccess = 32,
        PrintReportFailed = -33,

        UpLoadingReport = 40,
        UpLoadingReportError = -41,
        UpLoadReportSuccess = 42,
        UpLoadReportFailed = -43

    }
}
