using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.ReportManager
{
  
    /// <summary>
    /// 报表打印事件委托。
    /// </summary>
    /// <param name="sender">默认填写this</param>
    /// <param name="e">报表打印事件类</param>
    public delegate void ReportPrintEventHandle(object sender, ReportPrintEventArgs e);

    /// <summary>
    /// 报表打印事件类。
    /// </summary>
    public class ReportPrintEventArgs : EventArgs
    {
        ReportStatus _Status;
        /// <summary>
        /// 报表状态。
        /// </summary>
        public ReportStatus Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        Object _source;
        String _messageContent;
        DateTime _updateDate;
        bool _isComplete = false;
        bool _IsBatchPrintComplete = false;

        /// <summary>
        /// 源
        /// </summary>
        public Object Source
        {
            get { return _source; }
            set { _source = value; }
        }
        /// <summary>
        /// 信息内容。
        /// </summary>
        public String MessageContent
        {
            get { return _messageContent; }
            set { _messageContent = value; }
        }
        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTime UpdateDate
        {
            get { return _updateDate; }
            set { _updateDate = value; }
        }
        /// <summary>
        /// 批量打印是否完成。
        /// </summary>
        public bool IsBatchPrintComplete
        {
            get { return _IsBatchPrintComplete; }
            set { _IsBatchPrintComplete = value; }
        }
        /// <summary>
        /// 是否完成。
        /// </summary>
        public bool IsComplete
        {
            get { return _isComplete; }
            set { _isComplete = value; }
        }
        public ReportPrintEventArgs()
        {
            this._isComplete = false;
            this._messageContent = null;
            this._source = null;
            this._IsBatchPrintComplete = false;
            this._Status = ReportStatus.None;
        }

        /// <summary>
        /// 报表打印事件构造函数。
        /// </summary>
        /// <param name="source">谁发起的</param>
        /// <param name="messageContent">信息内容</param>
        public ReportPrintEventArgs(Object source, String messageContent)
        {
            this._source = source;
            this._messageContent = messageContent;
            this._isComplete = false;
        }

        /// <summary>
        /// 报表打印事件构造函数。
        /// </summary>
        /// <param name="source">谁发起的</param>
        /// <param name="messageContent">信息内容</param>
        /// <param name="isComplete">是否完成</param>
        public ReportPrintEventArgs(Object source, String messageContent, bool isComplete)
            : this(source, messageContent)
        {
            this._isComplete = isComplete;
        }
    }
}
