using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;


namespace AFC.WS.ModelView.UIContext
{
    using AFC.WS.UI.Common;
    using AFC.BOM2.MessageDispacher;
    

    //--->当系统没有操作时候，自动退出到登录界面判断时间然后发送同步消息即可
    /// <summary>
    /// WS无操作自动签退触发器
    /// </summary>
    /// <remarks>
    /// 负责人 王冬欣  最后修改日期：20091021
    /// 无操作自动签出通过调用Windows底层API函数，通过Timer达到配置文件中时间数值发送同步消息（参考BOM2.0基础）
    /// 该类只是定时发送同步消息，需要在具体的消息处理中来做界面的切换操作。使用该类必须在appConfig配置中定义
    /// 【SpaceInterval】配置项：<add key="SpaceInterval" value="200000"/>
    /// </remarks>
    public class AutoLogOutTrigger
    {
        /// <summary>
        /// 时间触发器
        /// </summary>
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        /// <summary>
        /// 时间差值，从配置文件中得到目前在AppConfig中【SpaceInterval】配置中获取
        /// </summary>
        private int spaceInterval;


        private static AutoLogOutTrigger trigger = null;


        public static AutoLogOutTrigger GetInstance()
        {
            if (trigger == null)
                trigger = new AutoLogOutTrigger();
            return trigger;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// 构造函数中挂载了Timer的Tick事件和事件处理函数
        /// </remarks>
        private AutoLogOutTrigger()
        {

            try
            {
                spaceInterval = Convert.ToInt32(ConfigurationManager.AppSettings["SpaceInterval"]);
                this.timer.Tick += new EventHandler(delegate(object sender, EventArgs e)
                {
                    LASTINPUTINFO plii = new LASTINPUTINFO();
                    plii.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(plii);
                    if (GetLastInputInfo(ref plii))
                    {
                        //超过时间自动锁定程序，发送同步消息
                        if (GetTickCount() - plii.dwTime >= this.spaceInterval)
                        {
                            Message msg = new Message();
                            msg.MessageType = AFC.WS.Model.Const.SynMessageType.LogOut;
                            MessageManager.SendMessasge(msg);
                                
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
            }
        }

        /// <summary>
        /// 启动监听（开启Timer的Tick监听）
        /// </summary>
        public void StartListen()
        {
            timer.Start();
        }

        //-->停止监听
        /// <summary>
        /// 停止监听（结束Timer的Tick监听）
        /// </summary>
        public void StopListen()
        {
            timer.Stop();
        }


        //--->可以在配置文件中定义
        /// <summary>
        /// 得到Window系统无操作的时间
        /// </summary>
        /// <returns>返回时间无操作时间单位（毫秒）1S=1000ms</returns>
        [DllImport("Kernel32.dll")]
        private static extern uint GetTickCount();

        //--->获得上次系统中没有操作的状态
        /// <summary>
        /// 获得上次系统输入的状态
        /// </summary>
        /// <param name="plii">上次输入的状态</param>
        /// <returns>正常获取返回true，否则返回false</returns>
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
    }
    //--->上次输入信息
    /// <summary>
    /// 上次输入信息数据结构，参考的WindowsAPI函数说明
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LASTINPUTINFO
    {
        /// <summary>
        /// 内存中的数据长度
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// 时间
        /// </summary>
        public uint dwTime;

    }
}
