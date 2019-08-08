using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.UI.Config
{
    /// <summary>
    /// 针对SQL语句定义的事件类。
    /// 
    /// 主要是用于判断ＳＱＬ语句是从哪里放到哪里去的。
    /// 
    /// </summary>
    public class SqlSentenceEventArgs : EventArgs
    {
        #region --> Property。

        /// <summary>
        /// 标志。
        /// </summary>
        public EventFlag Flag;
        /// <summary>
        /// 目标对象。
        /// </summary>
        public object Target;
        /// <summary>
        /// SQL语句。
        /// </summary>
        public string SqlSentence;

        #endregion --> Property。

        #region --> Conformation Method
        /// <summary>
        /// 构造方法
        /// </summary>
        public SqlSentenceEventArgs()
            : this(EventFlag.SetToTextBox, null, null)
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="flag">标志</param>
        /// <param name="target">目标对象</param>
        /// <param name="sqlSentence">SQL语句</param>
        public SqlSentenceEventArgs(EventFlag flag, object target, string sqlSentence)
        {
            this.Flag = flag;
            this.Target = target;
            this.SqlSentence = sqlSentence;
        }

        #endregion --> Conformation Method
    }

    /// <summary>
    /// 用于刷新propertyGrid的事件。
    /// 
    /// 主要是用于当选择DataSource里类名时，
    /// 
    /// 在进行切换时每次切换完之后都要对PropertyGrid控件进行刷新。
    /// 
    /// </summary>
    public class RefurbishPropertyGridEventArgs : EventArgs
    {
        #region --> Property。

        /// <summary>
        /// 是否刷新
        /// </summary>
        public bool CanRefurbish;
        /// <summary>
        /// 目标对象
        /// </summary>
        public object TargetObject;

        #endregion --> Property。

        #region --> Conformation Method

        /// <summary>
        /// 用于刷新propertyGrid的事件的构造方法
        /// </summary>
        public RefurbishPropertyGridEventArgs()
            : this(false, null)
        {
        }

        /// <summary>
        /// 用于刷新propertyGrid的事件的构造方法
        /// </summary>
        /// <param name="canRefurbish">是否刷新</param>
        /// <param name="targetObject">目标对象</param>
        public RefurbishPropertyGridEventArgs(bool canRefurbish, object targetObject)
        {
            this.CanRefurbish = canRefurbish;
            this.TargetObject = targetObject;
        }

        #endregion --> Conformation Method
    }
}
