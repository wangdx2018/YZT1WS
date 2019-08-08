using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AFC.BJComm.Data;

namespace AFC.WS.Model.Comm
{
    /// <summary>
    /// 报警消息数据结构的定义
    /// </summary>
    public class AlarmMessage
    {
        [PackOrder(1)]
        /// <summary>
        /// 报警ID
        /// </summary>
        public string alarmId;

        [PackOrder(2)]
        /// <summary>
        /// 报警值
        /// </summary>
        public string alarmValue;

        [PackOrder(3)]
        /// <summary>
        /// 报警内容
        /// </summary>
        public string alarmContent;

        [PackOrder(4)]
        /// <summary>
        /// 消息来源
        /// </summary>
        public string messageSource;

        [PackOrder(5)]
        /// <summary>
        /// 处理页面名称
        /// </summary>
        public string HandleMessagePageName;

        [PackOrder(6)]
        /// <summary>
        /// 日期
        /// </summary>
        public string date;

        [PackOrder(7)]
        /// <summary>
        /// 时间
        /// </summary>
        public string time;


        [PackOrder(8)]
        /// <summary>
        /// 页面参数
        /// </summary>
        public object pageParams;

        public override string ToString()
        {
            return string.Format("alarmId={0},alarmValue={1},alarmContent={2},messageSource={3},handeMessagePageName={4},date={5},time={6}",
                this.alarmId, this.alarmValue, this.alarmContent, this.messageSource, this.HandleMessagePageName, this.date, this.time);
            // return base.ToString();
        }
    }
}
