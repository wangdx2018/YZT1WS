using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.Cryptography;
using AFC.BJComm.Data;

namespace TJComm
{
    using AFC.BOM2.Common;

	/// <summary>
	/// CommConnection 的扩展类
    /// edit by wangdx 20110907 将存活包发送改成需要等待的接受
    /// 20110913 增加了存活包的超时时间修改为10s
    /// edit by wangdx 20110914修改了定时删除消息的定时任务
	/// </summary>
    public  partial class  CommConnection
    {

        #region [filed]
        /// <summary>
        /// 是否需要发送存活包
        /// </summary>
        public bool _isSendHeartBeatMsg;

        /// <summary>
        /// 发送存活包的时间（单位秒）
        /// </summary>
        public int _sendHeartBeatMsgInterval;

        /// <summary>
        /// 发送的超时时间,默认为10s,单位为毫秒
        /// </summary>
        private int _sendTimeout=10*1000;

        /// <summary>
        /// 定时发送存活包线程
        /// </summary>
        private Thread sendAliveThread = null;
   
        /// <summary>
        /// 解包设置的实体类
        /// </summary>
        public IMutableInstance _instance;

        /// <summary>
        /// 同步消息队列，集合中为同步消息数据
        /// </summary>
        private List<SynMessage> senderMsgCollection = new List<SynMessage>();

        /// <summary>
        /// 初始化时调用该配置信息,必须配置属性
        /// </summary>
        public CommConfigs LoadCommConfigFile()
        {
            try
            {
                CommConfigs config = System.Configuration.ConfigurationManager.GetSection("CommConfigs") as CommConfigs;
                return config;
            }
            catch (Exception ex)
            {
                  WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+ex.Message);
                return null;
            }
            
        }

        /// <summary>
        /// 最后一次发送时间
        /// </summary>
        private DateTime lastSendTime = DateTime.Now;

        /// <summary>
        /// 发送存活包
        /// 启动发送存活包线失程，初始化时调用，当加载配置失败后不启用
        /// </summary>
        private int SendHeartBeatMessage()
        {
            if (!_isSendHeartBeatMsg)
                return 0 ;
            if (DateTime.Now.Subtract(this.lastSendTime).TotalSeconds > this._sendHeartBeatMsgInterval)
            {
                CommHeader header = TJCommMessage.CreateHeader(TJCommMessage.BeatHeartMsgType, TJCommMessage.localDeviceId, TJCommMessage.serverDeviceId, CommandType.RESQUEST);
                BeartHeartData bhd = new BeartHeartData();
                bhd.comPackageData = new CommBodyData(header);
                bhd.selfDefPacketHeaderData = new CommHeaderData(0, 0, 0);
                TJCommMessage tjMsg =TJCommMessage.CreateTJCommMsg(header,bhd);//TJCommMessage.CreateTJCommMsg(0, CommandType.RESQUEST, TJCommMessage.BeatHeartMsgType, new BeartHeartData());
                
                TJCommMessage outTjMsg=TJCommMessage.CreateTJCommMsg(header,bhd);
               return SendMsgAndReceive(tjMsg, out tjMsg);
               
            }
            return 0;
        }

        #endregion

        #region [Function]
        /// <summary>
        /// 监听网络连接中断,网络中断后直接断开连接，在发送存活包线程里
        /// </summary>
        private void MonitorNetRecept()
        {
            Microsoft.VisualBasic.Devices.Computer localMachine = new Microsoft.VisualBasic.Devices.Computer();
            if (!localMachine.Network.IsAvailable)
            {
                this.Disconnect();
            }
        }
               
        /// <summary>
        /// 同步报文发送指令
        /// </summary>
        /// <param name="sendData">发送数据</param>
        /// <param name="receiveData">接收数据</param>
        /// <param name="timeout">超时时间，如果不填写按照10s发送</param>
        /// <returns>成功返回0，否则返回错误代码</returns>
        public int SendMsgAndReceive(TJCommMessage sendData, out TJCommMessage receiveData,params int[] timeout)
        {
            if (sendData == null)
            {
                  WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"input params error ! function [SendMsgAndReceive(TJCommMessage sendData, out TJCommMessage receiveData)] sendData is null");
                receiveData = null;
                return -1;
            }
            SynMessage msg = new SynMessage();//创建一个同步变量
            msg.sendMsg = sendData;
            msg.sendTime = DateTime.Now;
            lock (senderMsgCollection) //将消息放入List
            {
                int res = client.Send(TJCommMessage.PackTJMsg(msg.sendMsg));
                if (res != 0)
                {
                    WriteLog.Log_Error("[" + System.Threading.Thread.CurrentThread.Name + "]" + "sender package type=[" + sendData.header.messageType.ToString("x2") + "] failed!");
                    receiveData = null;
                    return -1; //send error
                }
                WriteLog.Log_Info("[" + System.Threading.Thread.CurrentThread.Name + "]" + "has send data:" + sendData.ToString());

                TJCommMessage.currentSessionId++;
                this.lastSendTime = DateTime.Now;
                senderMsgCollection.Add(msg);
                WriteLog.Log_Info("msg has put the synQueuee type=[" + msg.sendMsg.header.messageType.ToString("x2") + "]");
                Monitor.PulseAll(senderMsgCollection);//释放资源
            }
            lock (msg) //锁住该对象
            {
                if (timeout == null || timeout.Length == 0)
                {
                    Monitor.Wait(msg, this._sendTimeout);
                }
                else
                {
                    Monitor.Wait(msg, timeout[0]);
                }
            }

            receiveData = msg.receiveMsg;
            if (receiveData == null)
            {
                  WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"time out messageType=[" + sendData.header.messageType.ToString("x2") + "] no receive data");
                return -2; //receive error
            }
            WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"receive msg is :" + receiveData.ToString());
            lock (senderMsgCollection) //reomove msg 
            {
                WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"will remove the syn message queue ,msgType=[" + msg.sendMsg.header.messageType.ToString("x2") + "], current queue lenth is " + senderMsgCollection.Count.ToString());
                senderMsgCollection.Remove(msg);
            }
            return 0;
        }

        /// <summary>
        /// 发送请求，不判断应答
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        public int SendMsg(TJCommMessage sendData)
        {
            if (sendData == null)
            {
                  WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"input params error ! function [SendMsg(TJCommMessage sendData)] sendData is null");
                return -1;
            }
            try
            {
                WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"will send msg type=["+sendData.header.messageType.ToString("x2")+"]" + sendData.ToString());
                int res= client.Send(TJCommMessage.PackTJMsg(sendData));
                if (res == 0)
                {
                    TJCommMessage.currentSessionId++;
                    this.lastSendTime = DateTime.Now;

                }
                else
                {
                      WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"send msg failed! msgType=[" + sendData.header.messageType.ToString("x2") + "]");
                }
                return res;
            }
            catch(Exception ex)
            {
                  WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 处理同步消息和应答消息
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private int HandleResponseMsg(byte[] buffer)
        {
            TJCommMessage receiveMsg = TJCommMessage.UnPackData(buffer);
            if (receiveMsg == null)
                return -1;
            bool hasFound = false;
            lock (senderMsgCollection)
            {
                for (int i = 0; i < senderMsgCollection.Count; i++)
                {
                    if (hasFound) //已经发现了匹配的消息
                        break;
                    if (receiveMsg.header.messageType == senderMsgCollection[i].sendMsg.header.messageType &&   //检查分发消息是否需要删除
                      receiveMsg.header.sessionId == senderMsgCollection[i].sendMsg.header.sessionId)
                    {
                        lock (senderMsgCollection[i])
                        {
                            senderMsgCollection[i].receiveMsg = receiveMsg;
                            hasFound = true;
                            Monitor.PulseAll(senderMsgCollection[i]);
                            break;
                        }
                    }
                }
                if (!hasFound) //没有发现匹配的消息，丢掉该消息
                {
                      WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"that msg can't match in synrequest queue,ingore that msg,msgType=["+receiveMsg.header.messageType.ToString("x2")+"] sessionId is "+receiveMsg.header.sessionId.ToString()+"sendQ len="+senderMsgCollection.Count.ToString());
                    return 0;
                }

            }
            return 0;
        }

        /// <summary>
        /// 删除超时的同步消息队列
        /// </summary>
        private void RemoveTimeOutSynMsg()
        {
            lock (senderMsgCollection)
            {
                for (int i = 0; i < senderMsgCollection.Count; i++)
                {

                    if (DateTime.Now.Subtract(senderMsgCollection[i].sendTime).TotalSeconds >= this._sendTimeout/1000) //清除过期的消
                    {
                        WriteLog.Log_Info("[" + System.Threading.Thread.CurrentThread.Name + "]" + "Remove timeout msg:" + senderMsgCollection[i].sendMsg.ToString() + "dateTime is" + senderMsgCollection[i].sendTime.ToString("yyyyMMddHHmmss"));
                        senderMsgCollection.Remove(senderMsgCollection[i]);
          
                    }
                }
            }
        }

        /// <summary>
        /// 发送存活包的线程函数
        /// </summary>
        private void SendAliveMsgThreadFunc()
        {
            while (client.Connected)
            {
                Thread.Sleep(2000);
                MonitorNetRecept();
                RemoveTimeOutSynMsg();
                int res=SendHeartBeatMessage();
                if (res != 0)
                {
                    this.Disconnect();
                }
            }
        }

        /// <summary>
        /// 必须需要调用的接口，CommConfigs可以通过appConfig配置，
        /// 也可以实例化调用。
        /// </summary>
        /// <param name="config">CommConfig对象</param>
        /// <param name="senderId">发送方的设备ID</param>
        /// <param name="receiveId">接收方的设备ID</param>
        /// <returns>初始化成功返回true，否则返回false</returns>
        public bool SetupCommConfig(CommConfigs config)
        {
          
            if (string.IsNullOrEmpty(config.handleMsgType))
            {
                if (ServerMessageHandler == null)
                {
                      WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Initliaize error! [asynMessageHandle] not set!");
                    return false;
                }
                WriteLog.Log_Warn("Initliaize failed !but asynMessageHandle has set first!");
            }

            if (string.IsNullOrEmpty(config.unPackMsgHandleType))
            {
                  WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"["+System.Threading.Thread.CurrentThread.Name+"]"+"Initliaize error ![iMutableInstanceType] not set!");
                return false;
            }

           
            this._isSendHeartBeatMsg = config.isSendHeartBeat;
            this._sendHeartBeatMsgInterval = config.heartBeatMsgSendInterval;
            try
            {
                this._instance = Activator.CreateInstance(Type.GetType(config.unPackMsgHandleType)) as IMutableInstance;
                TJCommMessage.config = this._instance;
            }
            catch (Exception ex)
            {
                  WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Create IMutableInstance type error:[" + config.unPackMsgHandleType + "]");
                  WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+ex.Message);
                return false;
            }
            try
            {
                this.ServerMessageHandler = Activator.CreateInstance(Type.GetType(config.handleMsgType)) as IServerMessageHandler;
            }

            catch (Exception ex)
            {
                  WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Create IServerMessageHandler type error:[" + config.handleMsgType + "]");
                  WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+ex.Message);
                return false;
            }

            WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"initliaize commConfig data successfully");
            WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+config.ToString());
            return true;         
        }

      

        #endregion

        /*
         * 将AbstractCommData设置为object，设置接口
         * 
         * 1.send msg and add msg to list
         * 
         * 2.lock msg 
         * 
         * 3. reading socket thread match message type，found the msg,and pluse all the request.
         * 
         * 4.不调用DoReceive接口，同步消息不会入队。清除队列中的消息，返回给上层程序。
         * 
         * 5.壳子的封装添加一个时间戳。判断时间戳是否相同。
         * 
         * 6.将村活宝现成放在读Socket线程里。
         * 
         * 7.将重练现成在Socket中启动。
         * 
         * 8.连接状态变化事件，将last和current发送到Event中
         * 
         * */




       
    }
}
