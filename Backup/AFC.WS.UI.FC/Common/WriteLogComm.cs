using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;

namespace AFC.WS.UI.Common
{
    /// <summary>
    /// ��¼������־
    /// </summary>
    public class WriteLogComm
    {
        private IntPtr logHandle = new IntPtr(0);

        public WriteLogComm(IntPtr point, bool isLogEnable)
        {
            this.logHandle = point;
           this. isLogEnable = isLogEnable;
        }

        // ---> ��־ģ�����ʹ��
        /// <summary>
        /// ��־ģ�����ʹ��
        /// </summary>
        private   bool isLogEnable = false;

        // ---> ��¼debug�������־
        /// <summary>
        /// ��¼debug�������־
        /// </summary>
        /// <param name="message">��־�ı�</param>
        public   void Log_Debug(string message)
        {
            try
            {
                if (isLogEnable)
                    WriteLogApi.Log_Debug(logHandle, " [" + Thread.CurrentThread.Name + "]" + message);
            }
            catch
            {
            }
        }

        // ---> ��¼����ʽ��debug�������־
        /// <summary>
        /// ��¼����ʽ��debug�������־
        /// </summary>
        /// <param name="LogCode">��־��,ʹ��ǰ������־�����������Ӧ����</param>
        /// <param name="LogSubCode">��־����,ĿǰΪ�����ֶ�,Ĭ��Ϊ0</param>
        /// <param name="message">��־�ı�</param>
        public   void Log_DebugFormat(string LogCode, string LogSubCode, string message)
        {
            try
            {
                if (isLogEnable)
                    WriteLogApi.Log_DebugFormat(logHandle, LogCode, LogSubCode, " [" + Thread.CurrentThread.Name + "]" + message);
            }
            catch
            {
            }
        }

        // ---> ��¼info�������־
        /// <summary>
        /// ��¼info�������־
        /// </summary>
        /// <param name="message">��־�ı�</param>
        public   void Log_Info(string message)
        {
            try
            {
                if (isLogEnable)
                    WriteLogApi.Log_Info(logHandle, " [" + Thread.CurrentThread.Name + "]" + message);
            }
            catch (Exception ex)
            { }
        }

        // ---> ��¼����ʽ��info�������־
        /// <summary>
        /// ��¼����ʽ��info�������־
        /// </summary>
        /// <param name="LogCode">��־��,ʹ��ǰ������־�����������Ӧ����</param>
        /// <param name="LogSubCode">��־����,ĿǰΪ�����ֶ�,Ĭ��Ϊ0</param>
        /// <param name="message">��־�ı�</param>
        public   void Log_InfoFormat(string LogCode, string LogSubCode, string message)
        {
            try
            {
                if (isLogEnable)
                    WriteLogApi.Log_InfoFormat(logHandle, LogCode, LogSubCode, " [" + Thread.CurrentThread.Name + "]" + message);
            }
            catch
            {
            }
        }

        // ---> ��¼warn�������־
        /// <summary>
        /// ��¼warn�������־
        /// </summary>
        /// <param name="message">��־�ı�</param>
        public   void Log_Warn(string message)
        {
            try
            {
                if (isLogEnable)
                    WriteLogApi.Log_Warn(logHandle, " [" + Thread.CurrentThread.Name + "]" + message);
            }
            catch
            { }
        }

        // ---> ��¼����ʽ��warn�������־
        /// <summary>
        /// ��¼����ʽ��warn�������־
        /// </summary>
        /// <param name="LogCode">��־��,ʹ��ǰ������־�����������Ӧ����</param>
        /// <param name="LogSubCode">��־����,ĿǰΪ�����ֶ�,Ĭ��Ϊ0</param>
        /// <param name="message">��־�ı�</param>
        public   void Log_WarnFormat(string LogCode, string LogSubCode, string message)
        {
            try
            {
                if (isLogEnable)
                    WriteLogApi.Log_WarnFormat(logHandle, LogCode, LogSubCode, " [" + Thread.CurrentThread.Name + "]" + message);
            }
            catch { }
        }

        // ---> ��¼error�������־
        /// <summary>
        /// ��¼error�������־
        /// </summary>
        /// <param name="message">��־�ı�</param>
        public   void Log_Error(string message)
        {
            try
            {
                if (isLogEnable)
                    WriteLogApi.Log_Error(logHandle, " [" + Thread.CurrentThread.Name + "]" + message);
            }
            catch (Exception ex)
            {

            }
        }

        // ---> ��¼error�������־
        /// <summary>
        /// ��¼error�������־
        /// </summary>
        /// <param name="ex"></param>
        public   void Log_Error(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Message: ").Append(ex.Message).Append("\n StackTrace: ").Append(ex.StackTrace).Append("\n Source: ").Append(ex.Source).Append("\n InnerException: ").Append(ex.InnerException);
            Log_Error(sb.ToString());

        }

        // ---> ��¼����ʽ��error�������־
        /// <summary>
        /// ��¼����ʽ��error�������־
        /// </summary>
        /// <param name="LogCode">��־��,ʹ��ǰ������־�����������Ӧ����</param>
        /// <param name="LogSubCode">��־����,ĿǰΪ�����ֶ�,Ĭ��Ϊ0</param>
        /// <param name="message">��־�ı�</param>
        public   void Log_ErrorFormat(string LogCode, string LogSubCode, string message)
        {
            try
            {
                if (isLogEnable)
                    WriteLogApi.Log_ErrorFormat(logHandle, LogCode, LogSubCode, " [" + Thread.CurrentThread.Name + "]" + message);
            }
            catch { }
        }

        // ---> ��¼fatal�������־
        /// <summary>
        /// ��¼fatal�������־
        /// </summary>
        /// <param name="message">��־�ı�</param>
        public   void Log_Fatal(string message)
        {
            try
            {
                if (isLogEnable)
                    WriteLogApi.Log_Fatal(logHandle, " [" + Thread.CurrentThread.Name + "]" + message);
            }
            catch
            { }
        }

        // ---> ��¼����ʽ��fatal�������־
        /// <summary>
        /// ��¼����ʽ��fatal�������־
        /// </summary>
        /// <param name="LogCode">��־��,ʹ��ǰ������־�����������Ӧ����</param>
        /// <param name="LogSubCode">��־����,ĿǰΪ�����ֶ�,Ĭ��Ϊ0</param>
        /// <param name="message">��־�ı�</param>
        public   void Log_FatalFormat(string LogCode, string LogSubCode, string message)
        {
            try
            {
                if (isLogEnable)
                    WriteLogApi.Log_FatalFormat(logHandle, LogCode, LogSubCode, " [" + Thread.CurrentThread.Name + "]" + message);
            }
            catch { }
        }

        //public   void LogBuffer(string type, byte[] buffer, NewWriteLog logInstance)
        //{
        //    //if (!log.IsDebugEnabled)
        //    //    return;


        //    if (buffer == null)
        //    {
        //        logInstance.Log_Error("LogBuffer: buffer is NULL.");
        //        return;
        //    }
        //    StringBuilder b = new StringBuilder(buffer.Length * 4);

        //    b.Append(type + " Message Size: " + buffer.Length + "\r\n");
        //    b.Append("            00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F\r\n");

        //    int lineSize = 16;
        //    for (int i = 0; i < buffer.Length; i += lineSize)
        //    {

        //        b.Append(i.ToString("X10"));
        //        b.Append(": ");

        //        int count = i + lineSize < buffer.Length ? lineSize : buffer.Length - i;
        //        for (int j = 0; j < lineSize; j++)
        //            if (j < count)
        //                b.Append(buffer[i + j].ToString("X2") + " ");
        //            else
        //                b.Append("   ");

        //        for (int j = 0; j < lineSize; j++)
        //            if (j < count)
        //            {
        //                if (char.IsLetterOrDigit((char)buffer[i + j]))
        //                    b.Append((char)buffer[i + j]);
        //                else
        //                    b.Append('*');
        //            }
        //            else
        //                b.Append("-");


        //        b.Append("\r\n");
        //    }

        //    logInstance.Log_Info(b.ToString());
        //}

    }
}
