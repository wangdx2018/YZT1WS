using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.UI.Config;
using AFC.WS.UI.Components;
using AFC.WS.UI.Common;

namespace AFC.WS.BR.PassengerFlow
{
    /// <summary>
    /// 进站饼图
    /// </summary>
    public class PassengerFlowEntryPie : IChartDataSource
    {
        /// <summary>
        /// 进站饼图构造方法
        /// </summary>
        public PassengerFlowEntryPie()
        {
        }
        /// <summary>
        ///  进站饼图构造方法
        /// </summary>
        /// <param name="entryPie">图表基础控件</param>
        public PassengerFlowEntryPie(ChartControl entryPie)
        {
            entryPie.UserControlClassName = this.GetType().Namespace + "." + this.GetType().Name + "," + "AFC.WS.BR";
            ChartRule cr = Utility.Instance.GetChartRuleObject(@".\RuleFiles\PassengerFlow\PassengerFlowHistoryEntryPie.xml");
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
            entryPie.TitleTime = "";
            entryPie.ButtonLocation = System.Windows.HorizontalAlignment.Center;
            entryPie.DataSourceWay = SqlCount.Multi;
            entryPie.AxisXTicksStyle = "AxisXTicks";
            entryPie.AxisYTicksStyle = "AxisYTicks";
            entryPie.ChartGridXStyle = "ChartGridX";
            entryPie.ChartGridYStyle = "ChartGridY";
            entryPie.DataSeriesStyle = "DataSeries";
            entryPie.RenderType = Visifire.Charts.RenderAs.Pie;
            entryPie.Initialize(cr);
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
            PassengerFlowHelper.HistorySetPassengerFlowEntryPie(out XValue, out YValue, Interval);
        }

        #endregion
    }

}
