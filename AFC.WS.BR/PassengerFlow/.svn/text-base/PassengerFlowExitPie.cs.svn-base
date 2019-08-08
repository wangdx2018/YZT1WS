using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Config;
using AFC.WS.UI.Components;
using System.Windows;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.PassengerFlow
{
    /// <summary>
    /// 出站饼图类
    /// </summary>
    public class PassengerFlowExitPie : IChartDataSource
    {
        /// <summary>
        /// 出站饼图构造方法
        /// </summary>
        public PassengerFlowExitPie()
        {
        }
        /// <summary>
        /// 进站饼图构造方法
        /// </summary>
        /// <param name="exitPie">图表基础控件</param>
        public PassengerFlowExitPie(ChartControl exitPie)
        {
            exitPie.UserControlClassName = this.GetType().Namespace + "." + this.GetType().Name + "," + "AFC.WS.BR";
            ChartRule cr = Utility.Instance.GetChartRuleObject(@".\RuleFiles\PassengerFlow\PassengerFlowHistoryExitPie.xml");
            try
            {
                if (SysConfig.GetSysConfig().LocalParamsConfig.SystemName == "SCWS")
                {
                    cr.Title = BuinessRule.GetInstace().GetStationInfoById(SysConfig.GetSysConfig().LocalParamsConfig.StationCode).station_cn_name;
                }
                else
                {
                    cr.Title = BuinessRule.GetInstace().GetLineInfoById(SysConfig.GetSysConfig().LocalParamsConfig.LineCode).line_name;
                }
            }
            catch (Exception ee)
            {
                Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
            }

            exitPie.TitleTime = "";
            exitPie.ButtonLocation = HorizontalAlignment.Center;
            exitPie.DataSourceWay = SqlCount.Multi;
            exitPie.AxisXTicksStyle = "AxisXTicks";
            exitPie.AxisYTicksStyle = "AxisYTicks";
            exitPie.ChartGridXStyle = "ChartGridX";
            exitPie.ChartGridYStyle = "ChartGridY";
            exitPie.DataSeriesStyle = "DataSeries";
            exitPie.RenderType = Visifire.Charts.RenderAs.Pie;
            exitPie.Initialize(cr);
        }

        #region IChartDataSource 成员

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="XValue">X轴坐标值</param>
        /// <param name="YValue">Y轴坐标值</param>
        /// <param name="Interval">时间间隔</param>
        public void GetDataSource(out List<object> XValue, out List<List<string>> YValue, int Interval)
        {
            PassengerFlowHelper.HistorySetPassengerFlowExitPie(out XValue, out YValue, Interval);
        }

        #endregion
    }
}
