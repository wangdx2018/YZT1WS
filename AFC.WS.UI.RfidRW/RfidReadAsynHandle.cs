using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AFC.WS.UI.RfidRW
{
    using AFC.BOM2.MessageDispacher;
    using AFC.BOM2.Common;
    /// <summary>
    /// RFID异步读取数据的通用类,为了进入界面即将启动读取RFID信息的线程
    ///  读取之后发送一步消息给UI订阅者
    ///  
    /// added by :wangdx  date:20101111
    /// 
    /// edit by wangdx 20110706  增加了读取物理标签的线程处理
    /// </summary>
    public class RfidReadAsynHandle
    {

        /// <summary>
        /// 已经完成读取RFID信息
        /// </summary>
        public const string Finish_Read_Rfid = "Finish_Read_Rfid";

        private static Thread readRFidThread = null;


        private static Thread CreateNewThread(IRfidRW rfidHandle,Type t)
        {
            var thread = new Thread(new ThreadStart(() =>
                                                        {
                                                            int res = 0;
                                                            Message msg = new Message();
                                                            msg.MessageType = Finish_Read_Rfid;
                                                            while (true)
                                                            {
                                                                Thread.Sleep(500); // 为了避免线程死锁


                                                                if (t == null)
                                                                {
                                                                    string rfidPhysicalLabel =
                                                                        rfidHandle.GetRFIDPhysicalId(1);
                                                                    if (!string.IsNullOrEmpty(rfidPhysicalLabel))
                                                                    {
                                                                        msg.MessageParam = res;
                                                                        msg.Content = rfidPhysicalLabel;
                                                                        MessageManager.SendMessasge(msg);
                                                                    }
                                                                    continue;
                                                                }
                                                                if (t == typeof (RfidTicketboxInfo))
                                                                {
                                                                    RfidTicketboxInfo info =
                                                                        rfidHandle.ReadTicketBoxRFID(1, out res);
                                                                    if (info == null)
                                                                    {
                                                                        continue;
                                                                    }
                                                                    if (res != 0 &&
                                                                        string.IsNullOrEmpty(info.ticketboxId))
                                                                    {
                                                                        continue;
                                                                    }
                                                                    if (info.ticketboxId.Substring(2, 2) == "01" ||
                                                                        info.ticketboxId.Substring(2, 2) == "02" ||
                                                                        info.ticketboxId.Substring(2, 2) == "03")
                                                                    {
                                                                        msg.MessageParam = res;
                                                                        msg.Content = info;
                                                                        MessageManager.SendMessasge(msg);
                                                                    }
                                                                    continue;
                                                                }

                                                                if (t == typeof (MoneyBoxRFID))
                                                                {
                                                                    MoneyBoxRFID info = rfidHandle.ReadMoneyBoxRFID(1,
                                                                                                                    out
                                                                                                                        res);
                                                                    if (info == null)
                                                                    {
                                                                        continue;
                                                                    }

                                                                    if (res != 0 ||
                                                                        string.IsNullOrEmpty(info.moneyBoxId))
                                                                    {
                                                                        continue;
                                                                    }
                                                                    if (info.moneyBoxId.Substring(2, 2) == "11" ||
                                                                        info.moneyBoxId.Substring(2, 2) == "21" ||
                                                                        info.moneyBoxId.Substring(2, 2) == "22")
                                                                    {
                                                                        msg.MessageParam = res;
                                                                        msg.Content = info;
                                                                        MessageManager.SendMessasge(msg);
                                                                    }
                                                                    continue;
                                                                }
                                                            }
                                                        }));
            return thread;
        }

        private static IRfidRW rfidHandle = null;

        /// <summary>
        /// 异步读取RFID信息，读取成功之后发送异步消息
        /// </summary>
        /// <param name="rfidHandle">RFID操作信息</param>
        /// <param name="t">票箱RFID，钱箱RFID实体,传递NULL返回物理标签ID</param>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int StartAsynReadListen(IRfidRW rfidHandle, Type t)
        {
            if (rfidHandle == null)
            {
                WriteLog.Log_Error("rifdhandle is null params error! function name=[StartAsynReadListen(IRfidRW rfidHandle, Type t)],rifdHandle=[null]");
                return -1;
            }
           RfidReadAsynHandle.rfidHandle = rfidHandle;
          
            if (readRFidThread != null)
            {
                try
                {
                    rfidHandle.CloseSerialPort();
                    readRFidThread.Abort();
                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {
             
                }
            }
            readRFidThread = CreateNewThread(rfidHandle, t);
            readRFidThread.Name = "ReadAsynRfidInfo";
            readRFidThread.IsBackground = true;
            readRFidThread.Start(); 
            return 0;
        }

        /// <summary>
        /// 停止线程
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        public static int AbortAsynHandle()
        {
            if (readRFidThread == null)
            {
                WriteLog.Log_Error("plase call readRFID info first!");
                return -1;
            }
            try
            {
                if(rfidHandle!=null)
                {
                    rfidHandle.CloseSerialPort();
                }
             
                readRFidThread.Abort();
              
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return -1;
            }
            return 0;
        }

    }
}
