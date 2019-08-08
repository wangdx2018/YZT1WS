using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AFC.WS.ModelView.UIContext
{
    using AFC.BOM2.MessageDispacher;
    using AFC.BOM2.UIController;
    using AFC.WS.Model.Const;
    using AFC.WS.UI.CommonControls;
    using AFC.WS.UI.Common;
    using AFC.WS.Model.Comm;
    using AFC.WS.BR;
    using AFC.WS.Model.DB;

    /// <summary>
    /// 作者：王冬欣 
    /// 日期：20110221
    /// 代码功能：处理主界面的消息，UI切换，登录，登出等。
    /// 修订记录：20120201 修改了自动登出失败后不再提示。
    /// modified by wangdx 20130624 在登出后，关闭线程
    /// </summary>
    internal class UIMessageHandle : IMessageHandler, IControllerMessageHandler
    {
        private delegate void Fun();
        private Window myWindow = null;

        #region IMessageHandler 成员

        /// <summary>
        /// 处理异步消息，如通讯等
        /// </summary>
        /// <param name="msg"></param>
        public void HandleAsynMessage(Message msg)
        {
            this.myWindow.Dispatcher.Invoke(new Fun(() =>
            {
                #region ModeChangeHandle
                if (msg.MessageParam is ModeChangeNotify_1342)
                {
                    ModeChangeNotify_1342 body = msg.MessageParam as ModeChangeNotify_1342;
                    if (body != null)
                    {
                        BasiStationInfo bsi = BuinessRule.GetInstace().GetStationInfoById(body.modeStationId.ToString("X4"));
                         string stationMode = GetModeCodeInfo(body.modeCode);
                       //  MessageDialog.Show("ddd");
                        if (bsi.station_id.Equals(SysConfig.GetSysConfig().LocalParamsConfig.StationCode))
                        {
                            stationMode = GetModeCodeInfo(body.modeCode);
                            Message msg1 = new Message();
                            msg1.MessageType = SynMessageType.Mode_Change;
                            msg1.Content = stationMode;
                            MessageManager.SendMessasge(msg1);
                        }
                        else
                        {
                            MessageDialog.Show(string.Format("{0} 发生 {1} ", bsi.station_cn_name, stationMode),
                                "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                        }
                    
                    }
                    return;
                }
                #endregion

          
            }));
        }

        /// <summary>
        /// 处理同步消息，包括UI的切换，登出，自动登出等
        /// </summary>
        /// <param name="msg">同步消息</param>
        public void HandleSynMessage(Message msg)
        {
            if (msg.MessageType == SynMessageType.SwichPage)
            {
                HandleSwitchUIMessage(msg);
                return;
            }
            if (msg.MessageType == SynMessageType.LogOut)
            {
                HandleLogOutMessage();
                return;
            }

         
        }

        private string GetModeCodeInfo(uint modeCode)
        {
            switch (modeCode)
            {
                case 0:
                    return "正常服务";
                case 1:
                    return "列车故障模式";
                case 4:
                    return "日期免检模式";
                case 8:
                    return "车费免检模式";
                case 16:
                    return "进出站次序免检模式";
                case 32:
                    return "进站免检模式";
                case 64:
                    return "24小时运营模式";
                case 128:
                    return "紧急放行模式";
                case 255:
                    return "关闭服务模式";
                default:
                    return "未知模式";
            }
        }
        
        /// <summary>
        ///设置窗体信息 
        /// </summary>
        /// <param name="window">窗体对象</param>
        public void SetWindow(Window window)
        {
            this.myWindow = window;
        }

        #endregion

        #region IControllerMessageHandler 成员


        /// <summary>
        ///订阅异步消息
        /// </summary>
        public void SubscribeAsynMessage()
        {
            MessageManager.SubscribeMessage(this,
                SynMessageSubscribeId.MainFrame,
                AsynMessageType.CommAsynMsg,
                HandleMode.Asyn,true);
        }

        /// <summary>
        ///  订阅同步消息
        /// </summary>
        public void SubscribeSynMessage()
        {
            MessageManager.SubscribeMessage(this,SynMessageSubscribeId.MainFrame, SynMessageType.SwichPage, HandleMode.Syn, true);//订阅界面切换消息
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.MainFrame, SynMessageType.CreateNavigatorList, HandleMode.Syn, true);//订阅创建菜单导航消息
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.MainFrame, SynMessageType.LogOut, HandleMode.Syn, true);
        }

        #endregion


        /// <summary>
        /// 处理UI的切换消息
        /// </summary>
        /// <param name="msg">同步消息</param>
        private void HandleSwitchUIMessage(Message msg)
        {
            if (msg.MessageType != SynMessageType.SwichPage)
            {
                //todolog here
                return;
            }
            UIController.GetControllerInstance().SwitchFunction(msg.Content.ToString());

            if (msg.Content.ToString() != "SysStartAndCheck")
            {
                this.myWindow.WindowStyle = WindowStyle.None;
                this.myWindow.WindowState = WindowState.Maximized;
                if (this.myWindow.WindowState != WindowState.Maximized)
                {
                    this.myWindow.WindowState = WindowState.Maximized;
                }
               
            }
          
          

        }

        /// <summary>
        /// 创建导航菜单事件
        /// </summary>
        /// <param name="msg">导航菜单消息</param>
        private void HandleCreateNavigatorListMessage(Message msg)
        {

        }

        /// <summary>
        /// 处理登出消息
        /// </summary>
        private void HandleLogOutMessage()
        {
            UIContext.UIOperation.GetInstance().SwitchUI("Login");
            int res = UIOperation.GetInstance().BR.commProcess.LogOut(UIOperation.GetInstance().BR.brConext.CurrentOperatorId.ConvertNumberStringToUint());
            if (res != 0)
            {
                UIOperation.GetInstance().BR.logManager.WriteErrorCode(ErrorLogData.Priv_Send_logOut_Msg_Error);
            }
            UIOperation.GetInstance().BR.brConext.CurrentOperatorId = "00000000";
            AutoLogOutTrigger.GetInstance().StopListen();
            BuinessRule.GetInstace().alarmMonitor.StopAlarmMonitor();
            //send msg for ui navigator change
            MessageManager.SendMessasge(new Message 
            { 
                MessageType = SynMessageType.NavigationSelection, 
                Content = "Welcome" ,
                MessageParam="LogOut"
            });
            //AFC.WS.ModelView.UIContext.AutoLogOutTrigger.GetInstance().StartListen();
           
        }


    }
}
