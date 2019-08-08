using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.WS.Model.DB;
using AFC.WS.UI.BR.Data.PassengerFlow;

namespace AFC.WS.BR.PassengerFlow
{
    /// <summary>
    /// 客流统计事件委托。
    /// </summary>
    /// <param name="sender">存放当前类内容 this</param>
    /// <param name="e">客流统计事件类</param>
    public delegate void PassengerFlowNumberEventHandler(object sender, PassengerFlowNumberEventArgs e);
    /// <summary>
    /// 历史客流查询结果数据事件委托。
    /// </summary>
    /// <param name="sender">存放当前类内容 this</param>
    /// <param name="e">历史客流查询结果事件类</param>
    public delegate void HistoryPassengerFlowQueryResultEventHandler(object sender,HistoryPassengerFlowQueryResultEventArgs e);

    /// <summary>
    /// 历史客流查询结果饼图数据事件委托。
    /// </summary>
    /// <param name="sender">存放当前类内容 this</param>
    /// <param name="e">历史客流查询结果饼图事件类</param>
    public delegate void HistoryPassengerFlowPieEventHandler(object sender,HistoryPassengerFlowPieEventArgs e);

    /// <summary>
    /// 客流统计事件类
    /// </summary>
    public class PassengerFlowNumberEventArgs:EventArgs
    {
        List<PassengerFlowTypeMonitorInfo> _PassengerFlowNumberItem;
        /// <summary>
        /// 客流类型集合:分三段，第段三列
        /// </summary>
        public List<PassengerFlowTypeMonitorInfo> PassengerFlowNumberItem
        {
            get { return _PassengerFlowNumberItem; }
            set { _PassengerFlowNumberItem = value; }
        }

        DateTime _CurrentDateTime;
        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime CurrentDateTime
        {
            get { return _CurrentDateTime; }
            set { _CurrentDateTime = value; }
        }

        string _TotalPage;
        public string TotalPage
        {
            get { return _TotalPage; }
            set { _TotalPage = value; }
        }

        string _CurrentPage;
        public string CurrentPage
        {
            get { return _CurrentPage; }
            set { _CurrentPage = value; }
        }
    }
    
    /// <summary>
    /// 历史客流查询结果事件类
    /// </summary>
    public class HistoryPassengerFlowQueryResultEventArgs : EventArgs
    {
        object _Tag;
        /// <summary>
        /// 用来存放当前类 this
        /// </summary>
        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }
        List<HistoryPassengerFlowData> _PassengerFlowQueryList = new List<HistoryPassengerFlowData>();
        /// <summary>
        /// 客流查询出来的内容。
        /// </summary>
        public List<HistoryPassengerFlowData> PassengerFlowQueryResultList
        {
            get { return _PassengerFlowQueryList; }
            set { _PassengerFlowQueryList = value; }
        }
    }
    
    /// <summary>
    /// 历史客流查询结果饼图事件类。
    /// </summary>
    public class HistoryPassengerFlowPieEventArgs : EventArgs
    {

        PieEnum _PieType = PieEnum.None;
        /// <summary>
        /// 图饼类型枚举
        /// </summary>
        public PieEnum PieType
        {
            get { return _PieType; }
            set { _PieType = value; }
        }

        object _tag;
        /// <summary>
        /// 用来存放当前类 this
        /// </summary>
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        List<HistoryPassengerFlowPieData> _HpList = new List<HistoryPassengerFlowPieData>();
        /// <summary>
        /// 历史客流查询结果集合
        /// </summary>
        public List<HistoryPassengerFlowPieData> HpList
        {
            get { return _HpList; }
            set { _HpList = value; }
        }
    }
    
    /// <summary>
    /// 图饼类型枚举。
    /// </summary>
    public enum PieEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 进站
        /// </summary>
        Entry,
        /// <summary>
        /// 出站
        /// </summary>
        Exit
    }
}
