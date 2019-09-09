using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;



namespace TJComm
{
    using AFC.BOM2.Common;

	/// <summary>
	/// �ͻ������ӻ����ࡣʹ��Socketʵ�֡�
    /// 
    /// �޸��ˣ��������������receiveTimeout����
    /// ����˹��캯����20110308
    /// 
    /// �޸��ˣ������� ��Close������������lock��this��ͬ������20111025
	/// </summary>
	internal class CommClient
	{
		#region ����
			
		private Socket _sock = null ;
		

		/// <summary>
		/// �������ӷ������ĵ�ַ
		/// </summary>
		private string _serverAddress;

		/// <summary>
		/// �������ӷ������Ķ˿�
		/// </summary>
		private int _port;

        ///// <summary>
        ///// add by wangdx ��λ ����
        ///// </summary>
        //private int _receiveTimeOut;
		
		/// <summary>
		/// �Ƿ������ӡ�
		/// </summary>
		public bool Connected
		{
			get
			{
				return _sock != null && _sock.Connected;
			}
		}
	
		#endregion 

		/// <summary>
		/// ���캯����
		/// </summary>
		/// <param name="serverAdderss"></param>
		/// <param name="port"></param>
		public CommClient (string serverAdderss, int port)
		{
			_serverAddress = serverAdderss;
			_port = port;

			
		}

        ///// <summary>
        ///// add by wangdx
        ///// </summary>
        ///// <param name="serverAddress">������Ip</param>
        ///// <param name="port">�˿ں�</param>
        ///// <param name="receiveTimeout">��ʱʱ��</param>
        //public CommClient(string serverAddress, int port,int receiveTimeout)
        //{
        //    this._serverAddress = serverAddress;
        //    this._port = port;
        //    //this._receiveTimeOut = receiveTimeout;
        //}

		private void CreateSocket()
		{
			_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_sock.Blocking = true;
        
		}

		/// <summary>
		/// ��������,�����������ֱ�ӷ��ز����ɹ����롣
		/// </summary>
		/// <returns>�������</returns>
		public int Connect ()
		{
			lock(this)
			{
				try
				{

                    WriteLog.Log_Info("[" + System.Threading.Thread.CurrentThread.Name + "]" + "Connect to Server [" + _serverAddress + ":" + _port + "] ...");
					if (_sock != null && _sock.Connected)
					{
						Close();
					}
					
					CreateSocket(); 
				
					_sock.Connect (_serverAddress, _port);



                    WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"Connect to Server [" + _serverAddress + ":" + _port + "] done.");
					
					return 0;
				}
				catch (Exception e)
				{
					return HandleError("Connect to server[" + _serverAddress + ":" + _port + "]", e);
				}
			}
		}

		/// <summary>
		/// ������Ϣ��
		/// </summary>
		/// <param name="message">�����͵���Ϣ����</param>
		/// <returns>�������</returns>
		public int Send (byte[] message)
		{
			//���ͳ�ʱʱ��
			// disable send timeout. 
			/* _stream.WriteTimeout = SendTimeout; */
			lock(this)
			{
				
				if (TestAndReconnect () != ResultCode.OK)
					return ResultCode.NET_BREAK;
				try
				{
					int length = 0;
					LogBuffer ("Send", message);
					while (length < message.Length)
					{
						length += _sock.Send(message, length, message.Length - length , SocketFlags.None);
					}	
					return ResultCode.OK;
				}
				catch (Exception e)
				{
					return HandleError("Send Message" , e);
				}
			}
		
		}

		private int TestAndReconnect()
		{
			if (Connected)
				return 0;
			else 
				return ResultCode.NET_BREAK;
			/*
			try
			{
				_sock.Close();
			}
			catch(Exception)
			{
				// ignore 
				// Make Sure Socket is Closed.
			}
			
			// will reconnect 
			
			log.Debug("Will try to Reconnect.");
			
			return Connect();
			*/
				
		}

		/// <summary>
		/// �������ж�ȡ��Ϣ��������ʱ����
		/// </summary>
		/// <param name="message">���յ�����Ϣ����</param>
		/// <returns>�������</returns>
		public int Receive (out byte[] message)
		{
			return Receive(out message, true);
		}

		/// <summary>
		/// �������ж�ȡ��Ϣ��������ʱ����
		/// </summary>
		/// <param name="message">���յ�����Ϣ����</param>
		/// <returns>�������</returns>
		public int Receive(out byte[] message, bool isLock)
		{
			if (isLock)
			{
				lock (this)
				{
					return DoReceive(out message);
				}
			}
			else
			{
				return DoReceive(out message);
			}
		}

		private int DoReceive(out byte[] message)
		{
			message = null;
				
			// test for reconnect.
			if (TestAndReconnect() != ResultCode.OK)
				return ResultCode.NET_BREAK;
				
				
			try
			{
				//���������
				byte[] lenBuffer = new byte[2];

				//��ȡǰ4���ֽڣ���ȡ�����ȡ�
                
		int readLen = _sock.Receive(lenBuffer, 0, 2, SocketFlags.None);
				
				if (readLen == 0)
				{

                     WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Can't read data from socket.");

                    WriteLog.Log_Debug("SocketOptions: NoDelay=" + _sock.NoDelay +
							"; Blocking=" + _sock.Blocking +
							"; LingerState=" + _sock.LingerState +
							"; ReceiveTimeout=" + _sock.ReceiveTimeout +
							"; SendTimeout=" + _sock.SendTimeout +
							"; Connected=" + _sock.Connected + 
							".");
					
					return ResultCode.NET_PACK_SIZE_ZERO;
				}
				if (readLen != 2)
				{
				   WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Expect a 4 bytes length, but only " + readLen + " bytes!");	
					return ResultCode.NET_PACK_SIZE_WRONG;
				}
                //Array.Reverse(lenBuffer);
                ////lenBufferΪbig-endian����
                int length = BitConverter.ToInt16(lenBuffer, 0);


                WriteLog.Log_Debug("Read 2 bytes message length: " + length);

				if (length <= 2)
					return ResultCode.NET_PACK_SIZE_WRONG;
				
				//���溬�����ֶεİ�
				message = new Byte[length+2];
				
				// recovery original byte order. 
                //Array.Reverse(lenBuffer);
				
				lenBuffer.CopyTo (message, 0);

				readLen = 0;
				do
				{
					readLen += _sock.Receive (message, readLen + 2, length- readLen , SocketFlags.None);
				} while (readLen != length);

				
				if (readLen != length)
				{
					// never go  here.
					return ResultCode.NET_ERROR;
				}

				LogBuffer("Recieve", message);
				return ResultCode.OK;
			}
			catch (Exception e)
			{
				return HandleError("Read Message", e);
			}
		}

		/// <summary>
		/// �ر����ӡ�
		/// </summary>
		/// <returns></returns>
		public int Close ()
		{
            lock (this)
            {
                try
                {

                    if (_sock != null && _sock.Connected)
                    {
                        _sock.Close();
                    }


                    WriteLog.Log_Error("[" + System.Threading.Thread.CurrentThread.Name + "]" + "Disconnect Server [" + _serverAddress + ":" + _port + "].");
                    return ResultCode.OK;

                }
                catch (Exception e)
                {
                    return HandleError("Close connection", e);
                }
                finally
                {
                    _sock = null;
                }
            }
        
		}
		
		private void LogBuffer (string type, byte [] buffer)
		{
			
			
			if (buffer == null)
			{
				 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"LogBuffer: buffer is NULL.");
				return;
			}
			StringBuilder b = new StringBuilder (buffer.Length * 4);
			
			b.Append (type + " Message Size: " + buffer.Length + "\r\n");
			b.Append ("            00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F\r\n");
			
			int lineSize = 16;
			for (int i = 0; i < buffer.Length; i += lineSize)
			{

				b.Append (i.ToString ("X10"));
				b.Append (": ");

				int count = i + lineSize < buffer.Length ? lineSize : buffer.Length - i ;
				for (int j = 0; j < lineSize ; j ++)
					if (j < count)
						b.Append(buffer[i + j].ToString("X2") + " ");
					else
						b.Append ("   ");

				for (int j = 0; j < lineSize; j++)
					if (j < count)
					{
						if (char.IsLetterOrDigit((char)buffer[i + j]))
							b.Append((char)buffer[i + j]);
						else
							b.Append ('*');
					}
					else
						b.Append("-");


				b.Append("\r\n");
			}

            WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+b.ToString());
		}
		
		
		private int HandleError (string action, Exception e)
		{
            WriteLog.Log_Info("["+System.Threading.Thread.CurrentThread.Name+"]"+"SocketOptions: NoDelay=" + _sock.NoDelay +
					"; Blocking=" + _sock.Blocking +
					"; LingerState=" + _sock.LingerState +
					"; ReceiveTimeout=" + _sock.ReceiveTimeout +
					"; SendTimeout=" + _sock.SendTimeout +
					"; Connected=" + _sock.Connected +
					".");
			
			if (e is SocketException)
			{
				
				SocketException se = (SocketException)e;

				if (se.ErrorCode == (int)SocketError.TimedOut)
				{

                     WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+action + " timeout[" + _sock.ReceiveTimeout + "ms], ErrorCode: " + se.ErrorCode + ".");
                     WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+e.Message);
				}
				else
				{

                     WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+action + " Socket Error [" + se.ErrorCode + ": " + 
						          WinSockErrorCode.GetErrorCodeName(se.ErrorCode) + "]."+ e.Message);
				}

				return ResultCode.NET_ERROR;
			}
			else
			{

                 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+action + " Error."+e.Message);
				return ResultCode.NET_ERROR;
			}

		}
	}
}