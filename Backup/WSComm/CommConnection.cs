using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace TJComm
{
    using AFC.BOM2.Common;

	/// <summary>
	/// ������Ϊ���ַ�����˫ͨ�����ӵĽӿڡ�
    /// 
    /// �޸��ˣ�wangdx  ����ǰSend��Receive �ӿڱ�Ϊ��private 20100307
    /// 
    /// �޸��ˣ�wangdx  �޸���ReadingServer�����ķְ�����������MessageFlag���зְ�����
    /// 
    /// �޸��ˣ�wangdx ������ ReceiveTimeout�����������˳�ʱ�����������Ĭ��Ϊ10s��������CommConfig�����á�
    /// 
    /// ���� CommConnection ���캯��������Connect
    /// 
    /// autoReconnect ��Ĭ��Ϊfalse�������Ĭ��Ϊtrue
    /// 
    /// �޸��ˣ���������Connect����ʱ���������¼�
	/// </summary>
	public partial class CommConnection
	{
		#region ����

        /// <summary>
        /// �Ƿ��Զ�����
        /// </summary>
		private bool autoReconnect = true;
		
        /// <summary>
        /// ��װ��Socket����
        /// </summary>
		private CommClient client ;

		/// <summary>
		/// ����ʱ���� ��λ ����
		/// </summary>
		private int _connectInterval=1*1000;

		/// <summary>
		/// ����ӿڱ���
		/// </summary>
		private IServerMessageHandler _serverMessageHandler;

        /// <summary>
        /// ����ʱ��Ĭ��Ϊ10s��
        /// </summary>
        private int _receiveTimeout = 10 * 1000;
	
		/// <summary>
		/// ����ʱ����
		/// </summary>
		public int ConnectInterval
		{
			get { return _connectInterval; }
			set { _connectInterval = value; }
		}

        /// <summary>
        /// queue contains server request.(�첽����)
        /// </summary>
		private Queue<byte[]> requestQueue = new Queue<byte []>(10);

        /// <summary>
        /// queue contains server response.(ͬ������)
        /// </summary>
        private Queue<byte[]> responseQueue = new Queue<byte[]>(20);

        /// <summary>
        /// blocking read package from under socket.��Socket�߳�
        /// </summary>
        private Thread readingThread;

        /// <summary>
        /// blocking read server request from queue.(�����첽�����߳�)
        /// </summary>
        private Thread listenThread;

        // ��̨���ӱ�־λ
        private bool _connectInBackground;
		
	
		/// <summary>
		/// �����Ƿ��ѽ�����
		/// </summary>
		public bool Connected 
		{ 
			get
			{
				return client != null && client.Connected;
			} 
		}

		/// <summary>
		/// "�������Ľӿ�"����
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
		/// ������״̬�����仯ʱ�����¼���
		/// </summary>
		/// <remarks>
		/// �����ӽ���(<see cref="Connect"/>)�������ж�(<see cref="Disconnect"/>)ʱ�����������¼���
		/// </remarks>
		public event EventHandler ConnectionStateChanged;

        #endregion

        #region ���캯��
        /// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="senderServer">�ϴ�ͨ��������ip</param>
		/// <param name="senderPort">�ϴ�ͨ���������˿ں�</param>
		/// <param name="receiverServer">����ͨ��������ip</param>
		/// <param name="receivePort">����ͨ���������˿ں�</param>
		/// <param name="connectInterval">�������Ӽ������λ�����룩</param>
		/// <param name="serverMessageHandler">ʵ��IServerMessageHandler�����ʵ��</param>
		public CommConnection (string senderServer, int senderPort, string receiverServer, int receivePort,
		                       int connectInterval, IServerMessageHandler serverMessageHandler)
		{
			client = new CommClient (senderServer, senderPort);
			ConnectInterval = connectInterval;
			ServerMessageHandler = serverMessageHandler;
		}


      


		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="server">�ϴ�ͨ��/����ͨ��������ip</param>
		/// <param name="senderPoint">�ϴ�ͨ���������˿ں�</param>
		/// <param name="receivePort">����ͨ���������˿ں�</param>
		/// <param name="connectInterval">�������Ӽ������λ�����룩</param>
		/// <param name="serverMessageHandler">ʵ��IServerMessageHandler�����ʵ��</param>
		[Obsolete()]
		public CommConnection (string server, int senderPoint, int receivePort, int connectInterval,
		                       IServerMessageHandler serverMessageHandler) : 
		                       	this (server, senderPoint, server, receivePort, connectInterval, serverMessageHandler)
		{
		}

   
		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="server">�ϴ�ͨ��/����ͨ��������ip</param>
		/// <param name="senderPoint">�ϴ�ͨ���������˿ں�</param>
		/// <param name="receivePort">����ͨ���������˿ں�</param>
		/// <param name="connectInterval">�������Ӽ������λ�����룩</param>
  
		public CommConnection (string server, int senderPoint, int receivePort, int connectInterval)
			: this(server, senderPoint, server, receivePort, connectInterval, null)
		{
		}

      
		/// <summary>
		/// ���캯��
		/// </summary>
		/// <param name="server">�ϴ�ͨ��/����ͨ��������ip</param>
		/// <param name="senderPoint">�ϴ�ͨ���������˿ں�</param>
		/// <param name="connectInterval">�������Ӽ������λ�����룩</param>
        public CommConnection(string server, int senderPoint, int connectInterval)
			: this(server, senderPoint, server, senderPoint, connectInterval, null) 
		{
		}

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="server">�ϴ�ͨ��/����ͨ��������ip</param>
        /// <param name="senderPoint">�ϴ�ͨ���������˿ں�</param>
        /// <param name="connectInterval">�������Ӽ������λ�����룩</param>
        public CommConnection(string server, int senderPoint, int receivePort, int connectInterval,int receiveTimeout)
            : this(server, senderPoint, server, receivePort, connectInterval, null)
        {
        }

 

		#endregion

		/********************************
		 * 
		 *   ����Ϊ�����ṩ�ķ���
		 * 
		/*******************************/

		//*******************����001��ok
		/// <summary>
		/// �Ƚ���Socket���ӣ����������ӽ��
		/// </summary>
		/// <param name="reconnectCount">���ӳ��Դ���</param>
		/// <returns>�������:���ӳɹ�,TIME_OUT:���ӳ�ʱ,UNKNOWN_ERROR����δ֪����<see cref="ResultCode"/></returns>
		/// 
		public int Connect (int reconnectCount)
		{
			return Connect(reconnectCount, _connectInterval);
		}
		
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="reconnectCount">���Ӵ���</param>
        /// <param name="interval">���</param>
        /// <returns>0���ɹ�������ʧ��</returns>
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


		//*******************����002��
		/// <summary>
		/// ������̨�̣߳����ϳ��Խ������ӡ�
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
			//�����߳�Ϊ��̨
			connectInBack.Name = "connectInBackThread";
			connectInBack.IsBackground = true;
			connectInBack.Start ();
			
		}


        ////*******************����003��ok
        ///// <summary>
        ///// �Ͽ����ӡ�
        ///// </summary>
        ///// <remarks>�Ͽ����Ӻ󽫴���<see cref="ConnectionStateChanged"/>�¼���</remarks>
        ///// <param name="waitIfBusy">�������ݴ���ʱ�Ƿ�ǿ�ƹر�</param>
        ///// <returns>ok:�ɹ���UNKNOWN_ERROR:δ֪����</returns>
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


		/*//*******************����004��ok
		/// <summary>
		/// ��ȡ����״̬��
		/// </summary>
		/// <returns>Connecting:��������,Connected:�����ӣ�Disconnected:�����жϣ�Timeout:��ʱ�ж�,Closed:�ѹر�<see cref="CommConnection"/></returns>
		[Obsolete("Replaced with property Connected.")]
		public ConnectionState GetConnectionState (ClientType clientType)
		{
			return client.Connected ? ConnectionState.Connected : ConnectionState.Closed;
			
		}
*/

		//*******************����005��ok
		/// <summary>
		/// ����������������ġ�
		/// </summary>
		/// <remarks>
		/// ֱ����Socketд�����ݣ��������κλ��崦��
		/// </remarks>
		/// <param name="messageBuffer">������</param>
		/// <returns>�������</returns>
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

		//*******************����006��ok
		/// <summary>
		/// ���շ�������Ӧ����
		/// </summary>
		/// <param name="timeout"> ��ʱʱ��(��λ������)��ȡ�������ĳ�ʱʱ��</param>
		/// <param name="retcode">�������</param>
		/// <returns>����������byte[]����</returns>
		private byte[] RecieveResponseFromServer (int timeout, out int retcode)
		{
			return ReadQueue(timeout, out retcode, responseQueue);
		}

		//*******************����007��ok
		/// <summary>
		/// �ȴ�����������
		/// </summary>
		/// <param name="retcode">�������</param>
		/// <returns>���յı���</returns>
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


		//*******************����008��ok
		/// <summary>
		/// �������������Ӧ���ġ�
		/// </summary>
		/// <param name="messageBuffer">��Ӧ����</param>
		/// <returns>�������</returns>
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


		//*******************����009��ok
		/// <summary>
		/// �����̼߳��������������ݡ�
		/// </summary>
		/// <returns>�������</returns>
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
        /// �����첽�����̺߳���
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
								//���д�����
								int retcode1;
								respnoseBuffer = ServerMessageHandler.HandleServerMessage (buffer, out retcode1);
							}
							else
							{
								int retcode2;
								respnoseBuffer = ServerMessageHandler.HandleServerError (ResultCode.NET_PACK_SIZE_ZERO, out retcode2);
							}
							//--->������Ӧ����
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
		/// �����õ��߳�����
		/// </summary>
		private void ReadingServer ()
		{
			try
			{
                while (client.Connected)
                {
                    //--->��ȡ������
                    int retcode;

                    byte[] buffer;
                    try
                    {
                        retcode = client.Receive(out buffer, false); //Receive Ϊ��ʱ
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
                    if (!TJCommMessage.CheckPackageMD5Valid(buffer)) //������MD5��У��
                    {
                         WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"MD5 check error!");
                        continue;
                    }

                    lastSendTime = DateTime.Now; //�����ܵ����ݵ�ʱ��

                    
                    // Judge sessionFlagMap 
                    // extends flag with 0, 1. btye[27]
                    switch ((CommandType)buffer[27])
                    {
                        case CommandType.RESPONSE: //�������ظ���Ӧ��
                        case CommandType.MACK:  //�������ظ���Mack
                            //MessageEnqueue(buffer, requestQueue);
                            HandleResponseMsg(buffer);
                            break;
                        case CommandType.RESQUEST://������������
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