using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.ReportManager
{
    /// 报表打印接口
    public interface IReportPrint
    {

        /// <summary>
        /// 报表创建
        /// </summary>
        /// <param name="ReportName">报表名称</param>
        /// <param name="ParamCondition">参数条件</param>
        /// <returns></returns>
        bool ReportCreate(string ReportName, ParamCondition ParamCondition);

        /// <summary>
        /// 报表打印
        /// </summary>
        /// <param name="reportPath">报表路径</param>
        /// <returns></returns>
        int ReportPrint(String reportPath);

        /// <summary>
        /// 打开报表
        /// </summary>
        /// <param name="ReportName">报表名称</param>
        /// <param name="ReportPath">报表路径</param>
        void ReportOpen(string ReportName, string ReportPath);

        /// <summary>
        /// 报表保存
        /// </summary>
        /// <param name="ReportName">报表名称</param>
        /// <param name="SavePath">保存路径</param>
        /// <returns></returns>
        int ReportSave(string ReportName, string SavePath);

        /// <summary>
        /// 自动报表打印
        /// </summary>
        /// <param name="pcItem">要打印的报表列表</param>
        /// <returns></returns>
        bool ReportAutoPrint(List<ParamCondition> pcItem);

        /// <summary>
        /// 自动报表打印
        /// </summary>
        void ReportAutoPrint();

    }
}
