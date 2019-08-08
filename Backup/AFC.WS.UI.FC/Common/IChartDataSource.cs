using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// 
    /// 不是用默认数据源，多个DataSeries情况，并且多个SQL，继承此接口。
    /// 
    /// </summary>
    public interface IChartDataSource
    {
        /// <summary>
        /// 当数据源为多个时使用，继承此接口返回X轴坐标，Y轴坐标。
        /// </summary>
        /// <param name="XValue">X轴坐标</param>
        /// <param name="YValue">Y轴坐标</param>
        /// <param name="Interval">X轴间隔数</param>
        void GetDataSource(out List<object> XValue, out List<List<string>> YValue, int Interval);
    }
}
