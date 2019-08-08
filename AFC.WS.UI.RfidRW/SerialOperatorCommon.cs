using System;
using System.Collections.Generic;

using System.Text;
using System.IO.Ports;
using AFC.BOM2.Common;

namespace AFC.WS.UI.RfidRW
{
    /// <summary>
    /// added by wangdx 20100308 
    /// </summary>
    internal class SerialOperatorCommon
    {

        /// <summary>
        /// 向串口中发送指令
        /// </summary>
        /// <param name="sp">串口对象</param>
        /// <param name="buffer">发送的数据</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int SendMsg(System.IO.Ports.SerialPort sp, byte[] buffer)
        {
            try
            {
                if (!sp.IsOpen)
                {

                    sp.Open();
                }
                StringBuilder sb = new StringBuilder();

                if (sp == null || !sp.IsOpen || buffer == null)
                    return -1;
                for (int i = 0; i < buffer.Length; i++)
                {
                    sb.Append(buffer[i].ToString("x2"));
                    sb.Append(" ");
                }
                string cmd = sb.ToString();
                WriteLog.Log_Info("send cmd: "+cmd);
               
                sp.Write(buffer, 0, buffer.Length);
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// 读串口数据到缓冲区
        /// </summary>
        /// <param name="sp">串口</param>
        /// <param name="buffer">读到数据的数组</param>
        /// <param name="offset">起始位置</param>
        /// <param name="expectedLength">需要读取的长度</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int ReadFully(SerialPort sp, byte[] buffer, int offset, int expectedLength)
        {
            int totalLen = 0;
            while (true)
            {
                try
                {
                    if(!sp.IsOpen)
                    {
                       sp.Open();
                    }
                    
                    int readLen = sp.Read(buffer, offset, expectedLength - totalLen);
                    if (readLen <= 0)
                        throw new TimeoutException("Read len < 0:  " + readLen);
                    totalLen += readLen;
                    if (totalLen == expectedLength)
                    {
                        return totalLen;
                    }
                    else
                    {
                        offset += readLen;
                    }
                }
                catch (Exception e)
                {
                    WriteLog.Log_Error("ReadFully error:" + e.Message);
                   // sp.Close();
                    return 0;
                }

            }
        }

    }
}
