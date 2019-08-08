using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace TJComm
{
    using AFC.BOM2.Common;

	/// <summary>
	/// 本类作为保持服务器双通道连接的接口。
    /// 
    /// 修改人：wangdx  将以前Send，Receive 接口变为了private 20100307
    /// 
    /// 修改人：wangdx  修改了ReadingServer函数的分包处理，用天津的MessageFlag进行分包处理
    /// 
    /// 修改人：wangdx 增加了 ReceiveTimeout参数，配置了超时，如果不配置默认为10s。可以在CommConfig中配置。
    /// 
    /// 新增 CommConnection 构造函数增加了Connect
    /// 
    /// autoReconnect 从默认为false，变成了默认为true
    /// 
    /// 修改人：王冬欣，Connect联机时，不调用事件
	/// </summary>
	public partial class CommConnection
	{
		#region 属性

        /// <summary>
        /// 是否自动重练
        /// </summary>
		private bool autoReconnect = true;
		
        /// <summary>
        /// 封装的Socket对象
        /// </summary>
		private CommClient client ;

		/// <summary>
		/// 连接时间间隔 单位 毫秒
		/// </summary>
		private int _connectInterval=1*1000;

		/// <summary>
		/// 处理接口变量
		/// </summary>
		private IServerMessageHandler _serverMessageHandler;

        /// <summary>
        /// 重练时间默认为10s。
        /// </summary>
        private int _receiveTimeout = 10 * 1000;
	
		/// <summary>
		/// 连接时间间隔
		/// </summary>
		public int ConnectInterval
		{
			get { return _connectInterval; }
			set { _connectInterval = value; }
		}

        /// <summary>
        /// queue contains server request.(异步队列)
        /// </summary>
		private Queue<byte[]> requestQueue = new Queue<byte []>(10);

        /// <summary>
        /// queue contains server response.(同步队列)
        /// </summary>
        private Queue<byte[]> responseQueue = new Queue<byte[]>(20);

        /// <summary>
        /// blocking read package from under socket.读Socket线程
        /// </summary>
        private Thread readingThread;

        /// <summary>
        /// blocking read server request from queue.(监听异步队列线程)
        /// </summary>
        private Thread listenThread;

        // 后台连接标志位
        private bool _connectInBackground;
		
	
		/// <summary>
		/// 连接是否已建立。
		/// </summary>
		public bool Connected 
		{ 
			get
			{
				return client != null && client.Connected;
			} 
		}

		/// <summary>
		/// "处理方法的接口"变量
		/// </summary>
		public IServerMessageHandler ServerMessageHandler
		{
			get { return _serverMessageHandler; }
			set { _serverMessageHandler = value; }
		}

		public bool AutoReconnect
		{
			set { autoReconnect = value; }
		}

		/// <summary>
		/// 当连接状态发生变化时触发事件。
		/// </summary>
		/// <remarks>
		/// 当连接建立(<see cref="Connect"/>)或连接中断(<see cref="Disconnect"/>)时，将触发该事件。
		/// </remarks>
		public event EventHandler ConnectionStateChanged;

        #endregion

        #region 构造函数
        /// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="senderServer">上传通道服务器ip</param>
		/// <param name="senderPort">上传通道服务器端口号</param>
		/// <param name="receiverServer">下行通道服务器ip</param>
		/// <param name="receivePort">下行通道服务器端口号</param>
		/// <param name="connectInterval">尝试连接间隔（单位：毫秒）</param>
		/// <param name="serverMessageHandler">实现IServerMessageHandler的类的实例</param>
		public CommConnection (string senderServer, int senderPort, string receiverServer, int receivePort,
		                       int connectInterval, IServerMessageHandler serverMessageHandler)
		{
			client = new CommClient (senderServer, senderPort);
			ConnectInterval = connectInterval;
			ServerMessageHandler = serverMessageHandler;
		}


      


		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="server">上传通道/下行通道服务器ip</param>
		/// <param name="senderPoint">上传通道服务器端口号</param>
		/// <param name="receivePort">下行通道服务器端口号</param>
		/// <param name="connectInterval">尝试连接间隔（单位：毫秒）</param>
		/// <param name="serverMessageHandler">实现IServerMessageHandler的类的实例</param>
		[Obsolete()]
		public CommConnection (string server, int senderPoint, int receivePort, int connectInterval,
		                       IServerMessageHandler serverMessageHandler) : 
		                       	this (server, senderPoint, server, receivePort, connectInterval, serverMessageHandler)
		{
		}

   
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="server">上传通道/下行通道服务器ip</param>
		/// <param name="senderPoint">上传通道服务器端口号</param>
		/// <param name="receivePort">下行通道服务器端口号</param>
		/// <param name="connectInterval">尝试连接间隔（单位：毫秒）</param>
  
		public CommConnection (string server, int senderPoint, int receivePort, int connectInterval)
			: this(server, senderPoint, server, receivePort, connectInterval, null)
		{
		}

      
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="server">上传通道/下行通道服务器ip</param>
		/// <param name="senderPoint">上传通道服务器端口号</param>
		/// <param name="connectInterval">尝试连接间隔（单位：毫秒）</param>
        public CommConnection(string server, int senderPoint, int connectInterval)
			: this(server, senderPoint, server, senderPoint, connectInterval, null) 
		{
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="server">上传通道/下行通道服务器ip</param>
        /// <param name="senderPoint">上传通道服务器端口号</param>
        /// <param name="connectInterval">尝试连接间隔（单位：毫秒）</param>
        public CommConnection(string server, int senderPoint, int receivePort, int connectInterval,int receiveTimeout)
            : this(server, senderPoint, server, receivePort, connectInterval, null)
        {
        }

 

		#endregion

		/********************************
		 * 
		 *   以下为对外提供的方法
		 * 
		/*******************************/

		//*******************方法001：ok
		/// <summary>
		/// 先建立Socket连接，并返回连接结果
		/// </summary>
		/// <param name="reconnectCount">连接尝试次数</param>
		/// <returns>操作结果:连接成功,TIME_OUT:连接超时,UNKNOWN_ERROR发生未知错误<see cref="ResultCode"/></returns>
		/// 
		public int Connect (int reconnectCount)
		{
			return Connect(reconnectCount, _connectInterval);
		}
		
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="reconnectCount">连接次数</param>
        /// <param name="interval">间隔</param>
        /// <returns>0，成功，其他失败</returns>
		private int Connect (int reconnectCount, int interval)
		{
			try
			{
				int count = 0;

				while (reconnectCount == 0 || count++ < reconnectCount)
				{
				
						WriteLog.Log_Info ("Connect to Server(" + count + ") ...");
					
					// Connect Sender port.
					if (client.Connect() == ResultCode.OK)
					{
						// start Listen Thread ;
						readingThread = StartThread(readingThread, ReadingServer, "CommServerMessageThread");
						listenThread = StartThread(listenThread, RequestListen, "CommListenThread");
                        sendAliveThread=StartThread(this.sendAliveThread, this.SendAliveMsgThreadFunc, "SendAliveMsgThread");
						// fire Event

                        if (ConnectionStateChanged != null)
                        {
                            new Thread(delegate()
                                        {
                                            try
                                            {
                                                // start new thread handle. 
                                                ConnectionStateChanged(this, new EventArgs());
                                            }
                                            catch (Exception e)
                                            {
                                                WriteLog.Log_Error("[" + System.Threading.Thread.CurrentThread.Name + "]" + "Fire event error." + e.Message);
                                            }
                                        }).Start();
                        }
								
						
						return ResultCode.OK ;
					}

					Thread.Sleep (interval);
				}
			}
			catch (Exception e)
			{
				
				 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Connect server error." +e.Message);
				
			}
			
			// Start Background Connecting .
			if (autoReconnect)
				ConnectInBackground();
			return ResultCode.NET_BREAK;
		}


		//*******************方法002：
		/// <summary>
		/// 启动后台线程，不断尝试建立连接。
		/// </summary>    
		public void ConnectInBackground ()
		{
			if (_connectInBackground)
			{
				 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Background connecting thread already started.");
				return;
			}
			Thread connectInBack = new Thread (delegate()
			                                   	{
													_connectInBackground = true;
			                                   		int bgInterval = ConnectInterval ;
													WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"Start Background connecting thread ...");
			                                   		
													Thread.Sleep(bgInterval);
			                                   		
			                                   		Connect(0, bgInterval);
                                                    WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"Background connecting thread complete.");
			                                   		_connectInBackground = false;
			                                   	});
			//设置线程为后台
			connectInBack.Name = "connectInBackThread";
			connectInBack.IsBackground = true;
			connectInBack.Start ();
			
		}


        ////*******************方法003：ok
        ///// <summary>
        ///// 断开连接。
        ///// </summary>
        ///// <remarks>断开连接后将触发<see cref="ConnectionStateChanged"/>事件。</remarks>
        ///// <param name="waitIfBusy">在有数据传输时是否强制关闭</param>
        ///// <returns>ok:成功，UNKNOWN_ERROR:未知错误</returns>
        //public int Disconnect (bool waitIfBusy)
        //{
        //    try
        //    {
        //        client.Close();
        //    }
        //    catch (Exception e)
        //    {

        //         WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Close Sender client error."+e.Message);
        //    }
			
        //    WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"Socket has been closed. Wait for Reading Thread...");
        //    if (Thread.CurrentThread != readingThread)
        //        readingThread.Join();

        //    WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"Reading Thread runing state: " + (readingThread != null && readingThread.IsAlive));
			
        //    // Disconnect0(); 
        //    /*
        //    // disconnect in new thread.
        //    Thread th = new Thread(delegate()
        //                {
        //                    Disconnect0();
        //                }) ;
        //    th.Name = "CommDisconnectingThread";
        //    th.Start();
			
        //    th.Join();
        //    */
        //    return ResultCode.OK;
        //}

		public void Disconnect()
		{
          
                try
                {
                    
                    client.Close();

                }
                catch (Exception e)
                {

                    WriteLog.Log_Error("Disconnect Sender client error." + e.Message);
                }
                // shutdown thread
                ShutdownThread(readingThread);
                ShutdownThread(listenThread);
                ShutdownThread(sendAliveThread);

                // try to notify all thread blocked on queue.
                try
                {
                    lock (requestQueue)
                    {
                        Monitor.Pulse(requestQueue);
                    }

                    lock (responseQueue)
                    {
                        Monitor.Pulse(responseQueue);
                    }
                }
                catch (Exception e)
                {
                    WriteLog.Log_Error("[" + System.Threading.Thread.CurrentThread.Name + "]" + "notify blocking queue thread error, ignored." + e.Message);
                }
                // fire Event

                if (ConnectionStateChanged != null)
                    new Thread(delegate()
                                {
                                    try
                                    {
                                        ConnectionStateChanged(this, new EventArgs());
                                    }
                                    catch (Exception e)
                                    {
                                        WriteLog.Log_Error("[" + System.Threading.Thread.CurrentThread.Name + "]" + "Fire event error." + e.Message);
                                    }
                                }).Start();

                if (autoReconnect)
                    ConnectInBackground();
            
		}

		private void ShutdownThread(Thread thread)
		{

			if (thread != null && 
			    thread != Thread.CurrentThread && 
			    thread.IsAlive)
			{
				try
				{
					thread.Abort();
					
					// Make sure this thread be terminated.
					thread.Join();
				}
				catch (Exception e)
				{
					// ignore 
					WriteLog.Log_Debug("Shutdown thread error." +e.Message);
				}
			}
			
		}


		/*//*******************方法004：ok
		/// <summary>
		/// 获取连接状态。
		/// </summary>
		/// <returns>Connecting:正在连接,Connected:已连接，Disconnected:连接中断，Timeout:超时中断,Closed:已关闭<see cref="CommConnection"/></returns>
		[Obsolete("Replaced with property Connected.")]
		public ConnectionState GetConnectionState (ClientType clientType)
		{
			return client.Connected ? ConnectionState.Connected : ConnectionState.Closed;
			
		}
*/

		//*******************方法005：ok
		/// <summary>
		/// 向服务器发送请求报文。
		/// </summary>
		/// <remarks>
		/// 直接向Socket写入数据，不进行任何缓冲处理。
		/// </remarks>
		/// <param name="messageBuffer">请求报文</param>
		/// <returns>操作结果</returns>
		private int SendRequestToServer (byte[] messageBuffer)
		{
			lock (responseQueue)
			{
				try
				{
					return client.Send(messageBuffer);
				}
				catch (Exception e)
				{
				
						WriteLog.Log_Error ("Send Request Error." + e.Message);
				
					return ResultCode.NET_WRITE_ERROR;
				}
				finally
				{
					// clear response queue.
					if (responseQueue.Count > 0)
						WriteLog.Log_Debug("Clear response queue before recieve new response, " + 
						          responseQueue.Count + " message(s) removed.");
					
					responseQueue.Clear(); 
				}
			}
		}

		//*******************方法006：ok
		/// <summary>
		/// 接收服务器响应报文
		/// </summary>
		/// <param name="timeout"> 超时时间(单位：毫秒)读取网络流的超时时间</param>
		/// <param name="retcode">操作结果</param>
		/// <returns>服务器返回byte[]数组</returns>
		private byte[] RecieveResponseFromServer (int timeout, out int retcode)
		{
			return ReadQueue(timeout, out retcode, responseQueue);
		}

		//*******************方法007：ok
		/// <summary>
		/// 等待服务器请求。
		/// </summary>
		/// <param name="retcode">操作结果</param>
		/// <returns>接收的报文</returns>
		private byte[] WaitServerRequest (out int retcode)
		{
			return ReadQueue(Timeout.Infinite, out retcode, requestQueue);
		}

		// read package from synchronized queue.
		private byte[] ReadQueue(int timeout, out int retCode, Queue<byte[]> queue)
		{
			retCode = ResultCode.OK;
			byte[] buffer = null;

			try
			{
				// make sure lock queue.
				lock(queue)
				{
					// BugFixed: Change from requestQueue to queue(BUG).
					if (queue.Count == 0) 
					{
						// release lock and wait.
						Monitor.Wait(queue, timeout);
						
						// waked up by Monitor.Pulse.
						if (queue.Count == 0)
						{
							retCode = ResultCode.NET_READ_TIMEOUT;
							
							 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Read Server request Time out[Empty Queue].");

							return null;
						}
						else
							return queue.Dequeue();
					
					}
					else
						return queue.Dequeue();
				}
			}
			catch (Exception e)
			{
				// here maybe a InterruptedException. ignored,
				
					WriteLog.Log_Error ("Read Server request error."+e.Message);
				retCode = ResultCode.NET_READ_ERROR;
			}
			return buffer;
		}


		//*******************方法008：ok
		/// <summary>
		/// 向服务器发送响应报文。
		/// </summary>
		/// <param name="messageBuffer">响应报文</param>
		/// <returns>操作结果</returns>
		private int SendResponseToServer (byte[] messageBuffer)
		{
			try
			{
				return client.Send(messageBuffer);
			}
			catch (Exception e)
			{
			
					 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Send Response to Server error."+e.Message);
				return ResultCode.NET_READ_ERROR;
			}
		}


		//*******************方法009：ok
		/// <summary>
		/// 启动线程监听服务器的数据。
		/// </summary>
		/// <returns>操作结果</returns>
		private Thread StartThread(Thread thread, ThreadStart start, string threadName)
		{

			if (thread != null)
			{
				try
				{
					thread.Abort();
				}
				catch
				{
				}
			}

			thread = new Thread(new ThreadStart(start));
			thread.Name = threadName;
			thread.IsBackground = true;
			thread.Start();
			
			return thread;
		}

        /// <summary>
        /// 监听异步队列线程函数
        /// </summary>
		private void RequestListen()
		{
			if (ServerMessageHandler == null)
			{
				 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"ServerMessageHandler Not set, listenThread will not Startup."); 
				return;
			}

			try
			{
				while (client.Connected)
				{
					try
					{
						int retcode;
						byte [] buffer = WaitServerRequest(out retcode);
					
						if (retcode == ResultCode.OK)
						{
							byte[] respnoseBuffer;
							if (buffer.Length > 0)
							{
								//运行处理函数
								int retcode1;
								respnoseBuffer = ServerMessageHandler.HandleServerMessage (buffer, out retcode1);
							}
							else
							{
								int retcode2;
								respnoseBuffer = ServerMessageHandler.HandleServerError (ResultCode.NET_PACK_SIZE_ZERO, out retcode2);
							}
							//--->发送响应报文
							if (respnoseBuffer != null && respnoseBuffer.Length > 0)
								SendResponseToServer (respnoseBuffer);
						}
						else
						{
							Thread.Sleep(60*1000) ;
						}
					}
					catch (Exception e)
					{
						// ignore
						 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Unknown Error" + e.Message);
					}
				}
			}
			catch (Exception e)
			{
				 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Unexpected Error in RequestListen ()"+e.Message);
			}
			
		
		}


		/// <summary>
		/// 被启用的线程内容
		/// </summary>
		private void ReadingServer ()
		{
			try
			{
                while (client.Connected)
                {
                    //--->读取网络流
                    int retcode;

                    byte[] buffer;
                    try
                    {
                        retcode = client.Receive(out buffer, false); //Receive 为超时
                    }
                    catch (ThreadInterruptedException e)
                    {

                        WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"ReadingServer interrupted. " + e.Message);
                        if (!client.Connected)
                        {
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }


                    if (retcode == ResultCode.NET_PACK_SIZE_ZERO)
                    {
                         WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Connection breaked, can not read from Server, will close socket.");
                        // never go here.
                        // Disconnect(true);
                        return;
                        //return;
                    }

                    // ignore error
                    if (retcode != ResultCode.OK)
                        continue;


                    if (buffer.Length < 10)
                    {
                         WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Recieve Message ignored, size too short: [" + buffer.Length + "].");
                        continue;
                    }
                    if (!TJCommMessage.CheckPackageMD5Valid(buffer)) //增加了MD5的校验
                    {
                         WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"MD5 check error!");
                        continue;
                    }

                    lastSendTime = DateTime.Now; //最后接受的数据的时间

                    
                    // Judge sessionFlagMap 
                    // extends flag with 0, 1. btye[27]
                    switch ((CommandType)buffer[27])
                    {
                        case CommandType.RESPONSE: //服务器回复的应答
                        case CommandType.MACK:  //服务器回复的Mack
                            //MessageEnqueue(buffer, requestQueue);
                            HandleResponseMsg(buffer);
                            break;
                        case CommandType.RESQUEST://服务器的请求
                            //HandleResponseMsg(buffer);
                            MessageEnqueue(buffer,requestQueue);
                            break;
                        default:
                            // unknown
                             WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Recieve Message ignored, transaction code error: [" + Encoding.Default.GetString(buffer, 8, 4) + "].");
                            continue;
                    }
                }
			}
			catch (Exception e)
			{
				 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Unexpected Error in ReadingServer ()." +e.Message);
			}
			finally
			{
				// try to close socket and fire event
				try
				{
					Disconnect();
				}
				catch (Exception e)
				{
					 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Error During Disconnect." +e.Message);
				}
			}
			
		}

		// Enqueue and wake up waiting thread.
		private void MessageEnqueue(byte[] buffer, Queue<byte[]> queue)
		{
			
				WriteLog.Log_Debug("Put buffer(size=" + buffer.Length + ") into " + 
				          (queue == requestQueue ? "request" : "response") + " queue.");
			
			lock (queue)
			{
				queue.Enqueue(buffer);
				// important.
				Monitor.PulseAll(queue);
			}
		}
	}
}