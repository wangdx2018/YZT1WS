


using System;
using System.Collections.Generic;

using System.Text;
using System.IO.Ports;
using System.Threading;

namespace AFC.WS.UI.RfidRW
{

    using AFC.BOM2.Common;

    /// <summary>
    /// RfidRW 读写RFID操作
    /// 20101015 从CRC32修改成了CRC16
    /// </summary>
    public class RfidRW : IRfidRW
    {
        /// <summary>
        /// 串口信息实体类
        /// </summary>
        private SerialPort sp = new SerialPort();

        /// <summary>
        /// 锁变量
        /// </summary>
        private static object mutex = new object();

        /// <summary>
        /// 当前读写的RFID逻辑块
        /// </summary>
        public string currentReadRfid = string.Empty;

        /// <summary>
        /// 上次读出来票箱的信息
        /// </summary>
        private List<RfidTicketboxInfo> lastReadTicketBoxRfidInfo = new List<RfidTicketboxInfo>();

        /// <summary>
        /// 上次读出的钱箱信息
        /// </summary>
        private List<MoneyBoxRFID> lastReadMoneyBoxRfidInfo = new List<MoneyBoxRFID>();

        private bool status;

        /// <summary>
        /// 判断RFID读写器是否在线
        /// </summary>
        private bool Status
        {
            set
            {
                this.status = value;
            }
            get
            {
                return this.status;
            }
        }

        #region IRfidRW 成员

        public bool Connect(byte pathNumber)
        {
            if (!sp.IsOpen)
            {
                OpenSerialPort();
            }
            if (sp.IsOpen)
            {
                byte[] buffer = PackUnPackCommon.CreateReadOrStatusMessage(RFIDOperatorEnum.Status, 1, pathNumber);
                lock (mutex)
                {
                    int res = SerialOperatorCommon.SendMsg(sp, buffer);
                    if (res != 0)
                    {
                        Status = false;
                        return Status;
                    }
                    byte[] Ack = new byte[2];
                    SerialOperatorCommon.ReadFully(sp, Ack, 0, 2);
                    if (Ack[0] != 0x10 || Ack[1] != 0x06)
                    {
                        Status = false;
                        return Status;
                    }
                    byte[] recive = new byte[28];
                    SerialOperatorCommon.ReadFully(sp, recive, 0, 28);
                    if (recive[2] == 0xA3)
                    {
                        Status = true;
                        return Status;
                    }
                }
            }
            return false;

        }

        public string GetRFIDPhysicalId(byte pathNumber)
        {
            byte[] productId = GetItemBlock(0, 16, pathNumber);
            byte[] productData = new byte[4];
            if (productId == null)
                return "FFFFFFFF";
            Array.Copy(productId, 0, productData, 0, 4);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < productData.Length; i++)
            {
                sb.Append(productData[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public RfidTicketboxInfo ReadTicketBoxRFID(byte pathNumber, out int res)
        {
            List<RfidTicketboxInfo> list = ReadRFID<RfidTicketboxInfo>(BlockParams.ticketboxRfidA, BlockParams.ticketboxRfidB, BlockParams.StaticArea, pathNumber, out res);
            if (list != null && list.Count == 2)
            {
                if (res == OperatorResult.Successful)
                {
                    this.lastReadTicketBoxRfidInfo = list;
                    string blockNumber = ChooseBlock(list[0].blockOpeatorFlag, list[1].blockOpeatorFlag);
                    this.currentReadRfid = blockNumber;
                    if (string.IsNullOrEmpty(blockNumber))
                    {
                        res = OperatorResult.Not_Yet_Read_Card;
                        //todo:log here
                        return null;
                    }
                    res = 0;
                    this.lastReadTicketBoxRfidInfo = list;
                    return blockNumber.Equals("a") ? list[0].Clone() as RfidTicketboxInfo : list[1].Clone() as RfidTicketboxInfo;
                }
                if (res == OperatorResult.Connect_Failed)
                {
                    res = OperatorResult.Connect_Failed;
                    currentReadRfid = string.Empty;
                    return null;
                }

                if (res == OperatorResult.ReadDesBlockError)
                {
                    currentReadRfid = string.Empty;
                    res = OperatorResult.ReadDesBlockError;
                    WriteLog.Log_Error("A and B not any all valid");
                    return null;
                }
                if (res == OperatorResult.CRC_Data_Error)
                {
                    currentReadRfid = string.Empty;
                    res = OperatorResult.CRC_Data_Error;
                    WriteLog.Log_Error("A and B not any all valid");
                    return null;
                }
                if (res == OperatorResult.Block_A_B_NotValid)
                {
                    currentReadRfid = string.Empty;
                    res = OperatorResult.Block_A_B_NotValid;
                    WriteLog.Log_Error("A and B not any all valid");
                    return null;
                }
                if (res == OperatorResult.Block_A_NOtValid)
                {
                    currentReadRfid = "b";
                    return list[1].Clone() as RfidTicketboxInfo;//返回B
                }
                if (res == OperatorResult.Block_B_NotValid)
                {
                    currentReadRfid = "a";
                    return list[0].Clone() as RfidTicketboxInfo;//返回A
                }
            }
            return null;
        }

        public MoneyBoxRFID ReadMoneyBoxRFID(byte pathNumber, out int res)
        {
            List<MoneyBoxRFID> list = ReadRFID<MoneyBoxRFID>(BlockParams.moneyBoxRifdA, BlockParams.moneyBoxRfidB, BlockParams.StaticArea, pathNumber, out res);//该函数会检查合法性

            if (list != null && list.Count == 2)
            {
                this.lastReadMoneyBoxRfidInfo = list;
                if (res == OperatorResult.Successful)
                {
                    string blockNumber = ChooseBlock(list[0].blockOperatorFlag, list[1].blockOperatorFlag);
                    this.currentReadRfid = blockNumber;
                    if (string.IsNullOrEmpty(blockNumber))
                    {
                        res = OperatorResult.Not_Yet_Read_Card;//块A，块B非法
                        return null;
                    }
                    else
                    {
                       // res = OperatorResult.Successful;
                        currentReadRfid = blockNumber;
                        return currentReadRfid == "a" ? list[0].Clone() as MoneyBoxRFID : list[1].Clone() as MoneyBoxRFID;
                    }
                }
                if (res == OperatorResult.Connect_Failed)
                {
                    res = OperatorResult.Connect_Failed;
                    currentReadRfid = string.Empty;
                    return null;
                }

                if (res == OperatorResult.ReadDesBlockError)
                {
                    currentReadRfid = string.Empty;
                    res = OperatorResult.ReadDesBlockError;
                    WriteLog.Log_Error("A and B not any all valid");
                    return null;
                }
                if (res == OperatorResult.CRC_Data_Error)
                {
                    currentReadRfid = string.Empty;
                    res = OperatorResult.CRC_Data_Error;
                    WriteLog.Log_Error("A and B not any all valid");
                    return null;
                }
                if (res == OperatorResult.Block_A_B_NotValid)
                {
                    currentReadRfid = string.Empty;
                    res = OperatorResult.CRC_Data_Error;
                    WriteLog.Log_Error("A and B not any all valid");
                    return null;
                }
                if (res == OperatorResult.Block_A_NOtValid)
                {
                    currentReadRfid = "b";
                    return list[1].Clone() as MoneyBoxRFID;//返回B
                }
                if (res == OperatorResult.Block_B_NotValid)
                {
                    currentReadRfid = "a";
                    return list[0].Clone() as MoneyBoxRFID;//返回A
                }
            }
            return null;
        }

        public int WriteTicketBoxRFID(RfidTicketboxInfo ri, byte pathNumber)
        {
            if (string.IsNullOrEmpty(currentReadRfid))
                return OperatorResult.Not_Yet_Read_Card;
            int blockNumber = 0;

            if (currentReadRfid == "a")//read a,write b
            {
                WriteLog.Log_Info("last read ticketboxrifd A block,then It will be writing ticketboxrfid B block area");
                blockNumber = (lastReadTicketBoxRfidInfo[1].blockOpeatorFlag == 0 ? 1 : 0);
                this.lastReadTicketBoxRfidInfo[1] = ri;//set write b block 
                lastReadTicketBoxRfidInfo[1].blockOpeatorFlag = blockNumber;
                lastReadTicketBoxRfidInfo[1].checkField = this.GetCrcData<RfidTicketboxInfo>(lastReadTicketBoxRfidInfo[1]);
                WriteLog.Log_Info(" after write the block operator flag A=[" + lastReadTicketBoxRfidInfo[0].blockOpeatorFlag.ToString() + "],B=[" + lastReadTicketBoxRfidInfo[1].blockOpeatorFlag.ToString() + "]");
               return  WriteMessage<RfidTicketboxInfo>(this.lastReadTicketBoxRfidInfo[1], BlockParams.StaticArea, BlockParams.ticketboxRfidB, pathNumber);
            }
            if (currentReadRfid == "b") //read b write a
            {
                WriteLog.Log_Info("last read ticketboxrifd B block,then It will be writing ticketboxrfid A block area");
                blockNumber = (lastReadTicketBoxRfidInfo[0].blockOpeatorFlag == 0 ? 1 : 0);
                this.lastReadTicketBoxRfidInfo[0] = ri;//set write b block 
                lastReadTicketBoxRfidInfo[0].blockOpeatorFlag = blockNumber;
                lastReadTicketBoxRfidInfo[0].checkField = this.GetCrcData<RfidTicketboxInfo>(lastReadTicketBoxRfidInfo[0]);
                WriteLog.Log_Info(" after write the block operator flag A=[" + lastReadTicketBoxRfidInfo[0].blockOpeatorFlag.ToString() + "],B=[" + lastReadTicketBoxRfidInfo[1].blockOpeatorFlag.ToString() + "]");
                return WriteMessage<RfidTicketboxInfo>(this.lastReadTicketBoxRfidInfo[0], BlockParams.StaticArea, BlockParams.ticketboxRfidA, pathNumber);

            }

            return -1;
            


           
        }

        public int WriteMoneyBoxRFID(MoneyBoxRFID ri, byte pathNumber)
        {
            if (string.IsNullOrEmpty(currentReadRfid))
                return OperatorResult.Not_Yet_Read_Card;
            int blockNumber = 0;
            if (currentReadRfid == "a")//read a,write b
            {
                WriteLog.Log_Info("last read moneyboxrifd A block,then It will be writing ticketboxrfid B block area");
               //set write b block 
                
                blockNumber = (lastReadMoneyBoxRfidInfo[1].blockOperatorFlag == 0 ? 1 : 0);
                this.lastReadMoneyBoxRfidInfo[1] = ri;
                this.lastReadMoneyBoxRfidInfo[1].blockOperatorFlag = blockNumber;
                this.lastReadMoneyBoxRfidInfo[1].checkArea = this.GetCrcData<MoneyBoxRFID>(this.lastReadMoneyBoxRfidInfo[1]);
                WriteLog.Log_Info(" after write the block operator flag A=[" + lastReadMoneyBoxRfidInfo[0].blockOperatorFlag.ToString() + ",B=[" + lastReadMoneyBoxRfidInfo[1].blockOperatorFlag.ToString());
                return WriteMessage<MoneyBoxRFID>(this.lastReadMoneyBoxRfidInfo[1], BlockParams.StaticArea, BlockParams.moneyBoxRfidB, pathNumber);
            }
            if (currentReadRfid == "b") //read b write a
            {
                WriteLog.Log_Info("last read moneyboxrifd B block,then It will be writing ticketboxrfid A block area");
               
                blockNumber = (lastReadMoneyBoxRfidInfo[0].blockOperatorFlag == 0 ? 1 : 0);
                this.lastReadMoneyBoxRfidInfo[0] = ri;
                this.lastReadMoneyBoxRfidInfo[0].blockOperatorFlag = blockNumber;
                this.lastReadMoneyBoxRfidInfo[0].checkArea = this.GetCrcData<MoneyBoxRFID>(this.lastReadMoneyBoxRfidInfo[0]);
                WriteLog.Log_Info(" after write the block operator flag A=[" + lastReadMoneyBoxRfidInfo[0].blockOperatorFlag.ToString() + ",B=[" + lastReadMoneyBoxRfidInfo[1].blockOperatorFlag.ToString());
                return WriteMessage<MoneyBoxRFID>(this.lastReadMoneyBoxRfidInfo[0], BlockParams.StaticArea, BlockParams.moneyBoxRifdA, pathNumber);

            }
            return -1;
        }

        public void SetSerialPort(int baudRate, System.IO.Ports.Parity parity, System.IO.Ports.StopBits stopBits, int dataBits, string portName)
        {
            try
            {
            if (sp.IsOpen)
                sp.Close();
            sp.BaudRate = baudRate;
            sp.Parity = parity;
            sp.StopBits = stopBits;
            sp.PortName = portName;
            sp.DataBits = dataBits;
            sp.ReadTimeout = 2000;
            WriteLog.Log_Info("com name is:" + portName + "\r\n");
            WriteLog.Log_Info("baudRate is:" + baudRate.ToString() + "\r\n");
            WriteLog.Log_Info("parity is:" + parity.ToString() + "\r\n");
            WriteLog.Log_Info("stopBits is :" + stopBits.ToString() + "\r\n");
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
            }
        }




        #endregion

        /// <summary>
        /// 得到CRC数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ri"></param>
        /// <returns></returns>
        private ushort GetCrcData<T>(T ri)
        {
            byte[] buffer = AFC.BJComm.Data.DataProcessor.PackObject(ri);
            if (buffer == null)
            {
                WriteLog.Log_Error("Unpack object error" + ri.GetType().ToString());
                return ushort.MaxValue;
            }
            byte[] crcData = null;
            if (ri is RfidTicketboxInfo)
            {
                crcData = new byte[34];//money box will calcaulate 30 byte,remove the money box id
            }
            if (ri is MoneyBoxRFID)
            {
                crcData = new byte[30];//money box will calcaulate 30 byte,remove the money box id
            }
            Array.Copy(buffer, 4, crcData, 0, crcData.Length);
            ushort crc = this.CaulatorCRC(crcData);

            WriteLog.Log_Info(ri.ToString() + "    type=[" + ri.GetType().ToString() + "]," + "crc=[" + crc.ToString() + "]");

            return crc;
        }

        /// <summary>
        /// 获得某块的数据
        /// </summary>
        /// <param name="blockNumber">读取物理结构的块号</param>
        /// <param name="dataSize">所需的块的长度</param>
        /// <returns>成功返回需要的数据，否则返回null</returns>
        private byte[] GetItemBlock(byte blockNumber, int dataSize, byte pathNumber)
        {
            try
            {
                if (!Status)
                {
                    WriteLog.Log_Error("Connect_Failed");
                    return null;
                }
                /// 读取静态区 TicketBoxID 从块1中读取
                byte[] buffer = PackUnPackCommon.CreateReadOrStatusMessage(RFIDOperatorEnum.Read, blockNumber, pathNumber);
                lock (mutex)
                {
                    int res = SerialOperatorCommon.SendMsg(sp, buffer);
                    if (res != 0)
                        return null;
                    byte[] Ack = new byte[2];
                    SerialOperatorCommon.ReadFully(sp, Ack, 0, 2);
                    if (Ack[0] != 0x10 || Ack[1] != 0x06)
                        return null;
                    byte[] recive = new byte[28];
                    SerialOperatorCommon.ReadFully(sp, recive, 0, 28);//从第4个字节开始是数据
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < recive.Length; i++)
                    {
                        sb.Append(recive[i].ToString("x2"));
                        sb.Append(" ");
                    }
                    string cmd = sb.ToString();
                    WriteLog.Log_Info("receive data: " + cmd);
                    if (recive[20] == 0 && recive[22] == 0) //读取时检验位 第1，和3为必须为0，数据为才合法
                    {
                        byte[] data = new byte[dataSize];
                        Array.Copy(recive, 4, data, 0, dataSize);
                        return data;
                    }
                    else
                    {
                        WriteLog.Log_Error("读取时检验位 第1，和3为错误");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                WriteLog.Log_Error("OperatorResult.ReadDesBlockError");
                return null;
            }
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns>成功返回0，否则返回错误代码</returns>
        private int OpenSerialPort()
        {
            try
            {
                sp.Open();
                return 0;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error("Open port [" + sp.PortName + "]");
                WriteLog.Log_Error(ex.Message);
                return OperatorResult.SerialPortNotOpen;
            }
        }

        /// <summary>
        /// 读RFID数据
        /// </summary>
        /// <typeparam name="T">钱箱或票箱</typeparam>
        /// <param name="blockA">块A</param>
        /// <param name="blockB">块B</param>
        /// <param name="staticArea">静态区</param>
        /// <param name="res">读成功返回0，否则返回错误代码</param>
        /// <returns>返回A块和B块，索引0为A块，1为B块</returns>
        private List<T> ReadRFID<T>(byte[] blockA, byte[] blockB, byte staticArea, byte pathNumber, out  int res) where T : class, new()
        {
            if (!Status)
            {
                res = OperatorResult.Connect_Failed;
                return null;
            }
            try
            {
                res = 0;
                T a = new T();
                T b = new T();
                List<T> list = new List<T>();

                byte[] staticItem = GetItemBlock(staticArea, 4, pathNumber);
                List<Byte[]> blockArray = new List<byte[]>();
                for (int i = 0; i < blockA.Length; i++)
                {
                    byte[] temp = GetItemBlock(blockA[i], 16, pathNumber);
                    if (temp != null)
                    {
                        blockArray.Add(temp);
                    }
                    else
                    {
                        res = OperatorResult.ReadDesBlockError;
                        return null;
                    }
                }

                List<Byte[]> blockBarray = new List<byte[]>();
                for (int i = 0; i < blockB.Length; i++)
                {
                    byte[] temp = GetItemBlock(blockB[i], 16, pathNumber);
                    if (temp != null)
                    {
                        blockBarray.Add(temp);
                    }
                    else
                    {
                        res = OperatorResult.ReadDesBlockError;
                        return null;
                    }
                }
                byte[] totalA = GetTotalItems(staticItem, blockArray);
                if (totalA == null)
                {
                    return null;
                }
                byte[] totalB = GetTotalItems(staticItem, blockBarray);
                if (totalB == null)
                {
                    return null;
                }
                object A = a as object;
                object B = b as object;
                AFC.BJComm.Data.DataProcessor.UnpackObject(ref A, totalA);
                AFC.BJComm.Data.DataProcessor.UnpackObject(ref B, totalB);
                list.Add(A as T);
                list.Add(B as T);

                bool checkA = CheckCRC(A, totalA);
                bool checkB = CheckCRC(B, totalB);
                if (!checkB)
                {
                    res = OperatorResult.Block_B_NotValid;
                    WriteLog.Log_Error("CRC 验证块B非法");
                }
                if (!checkA)
                {
                    res = OperatorResult.Block_A_NOtValid;
                    WriteLog.Log_Error("CRC 验证块A非法");
                }
                if (!checkA && !checkB)
                {
                    res = OperatorResult.CRC_Data_Error;
                    WriteLog.Log_Error("CRC 验证 块A，块B都非法");
                }
                return list;
            }
            catch (Exception ex)
            {
                res = -1;
                return null;
            }

        }

        private bool CheckCRC(object rfidData, byte[] rfidBuffer)
        {
            //return true;
            if (rfidData is RfidTicketboxInfo)
            {
                byte[] buffer = new byte[34];
                StringBuilder sb = new StringBuilder();
             
                Array.Copy(rfidBuffer, 4, buffer, 0, buffer.Length);
                RfidTicketboxInfo info = rfidData as RfidTicketboxInfo;
                for (int i = 0; i < buffer.Length; i++)
                {
                    sb.Append(buffer[i].ToString("x2"));
                    sb.Append(" ");
                }
                WriteLog.Log_Info("crcbuffer data is: " + sb.ToString());
                ushort crc = this.CaulatorCRC(buffer);
                return crc == info.checkField;
            }
            if (rfidData is MoneyBoxRFID)
            {
                byte[] buffer = new byte[30];
                Array.Copy(rfidBuffer, 4, buffer, 0, buffer.Length);
                MoneyBoxRFID info = rfidData as MoneyBoxRFID;
                ushort crc = this.CaulatorCRC(buffer);
                return crc == info.checkArea;
            }
            return false;
        }

        /// <summary>
        /// 得到读到的总的数据
        /// </summary>
        /// <param name="staticArea">静态区</param>
        /// <param name="blockAreas">逻辑块A或B</param>
        /// <returns>返回读取的RFID打包之后的数据</returns>
        private byte[] GetTotalItems(byte[] staticArea, List<byte[]> blockAreas)
        {
            if (staticArea == null)
            {
                return null;
            }
            int count = staticArea.Length;
            for (int i = 0; i < blockAreas.Count; i++)
            {
                count += blockAreas[i].Length;
            }
            byte[] totalBuffer = new byte[count];
            int offset = 0;
            Array.Copy(staticArea, 0, totalBuffer, offset, staticArea.Length);
            offset += staticArea.Length;
            for (int i = 0; i < blockAreas.Count; i++)
            {
                Array.Copy(blockAreas[i], 0, totalBuffer, offset, blockAreas[i].Length);
                offset += blockAreas[i].Length;
            }
            return totalBuffer;
        }

        /// <summary>
        /// 发送信息到串口，并作ACK和应答数据处理
        /// </summary>
        /// <param name="buffer">数据字节</param>
        /// <returns>成功返回0，否则返回-1</returns>
        private int WriteMessage(byte[] buffer)
        {
            lock (mutex)
            {
                int res = SerialOperatorCommon.SendMsg(sp, buffer);
                byte[] Ack = new byte[2];
                SerialOperatorCommon.ReadFully(sp, Ack, 0, 2);
                if (Ack[0] != 0x10 || Ack[1] != 0x06)
                    return -1;
                byte[] recive = new byte[28];
                SerialOperatorCommon.ReadFully(sp, recive, 0, 28);//从第8个字节开始是数据
                return HandleError(recive);
            }
        }

        /// <summary>
        /// 处理写数据是否成功
        /// </summary>
        /// <param name="buffer">数据字段</param>
        /// <returns>根据错误代码返回数据</returns>
        private int HandleError(byte[] buffer)
        {
            if (buffer == null)
                return -1;
            if (buffer[20] != 0)
            {
                return OperatorResult.ReadDesBlockError;
            }
            if (buffer[21] != 0)
            {
                return OperatorResult.WriteDesBlockError;
            }
            if (buffer[22] != 0)
            {
                return OperatorResult.PathSwitchParamError;
            }
            if (buffer[23] != 0)
            {
                return OperatorResult.WriteIDError;
            }
            if (buffer[24] != 0)
            {
                return OperatorResult.NotValidOperatorId;
            }
            if (buffer[25] != 0)
            {
                return OperatorResult.WriteDesBlockParamNotValid;
            }
            if (buffer[26] != 0)
            {
                return OperatorResult.SelectDesError;
            }
            return 0;

        }

        /// <summary>
        /// 写RFID数据
        /// </summary>
        /// <typeparam name="T">钱箱或者票箱RFID类型</typeparam>
        /// <param name="a">钱箱或者票箱RFID对象</param>
        /// <param name="saticArea">静态区</param>
        /// <param name="blocks">逻辑块A,B的块号</param>
        /// <param name="flag">flag为true，写静态区，默认不写静态区</param>
        /// <returns>成功返回0，否则返回-1</returns>
        private int WriteMessage<T>(T a, byte saticArea, byte[] blocks, byte pathNumber,params bool[] flag) where T : class, new()
        {
            if (!Status)
                return OperatorResult.Connect_Failed;
            object rifdData = a as Object;
            byte[] array = null;
            //if (flag.Length > 0
            //    && flag[0])
            //{
                 array = new byte[1 + blocks.Length];
                array[0] = saticArea;
                Array.Copy(blocks, 0, array, 1, blocks.Length);
            //}
            //else
            //{
            //     array = new byte[ blocks.Length];
            //     Array.Copy(blocks, 0, array,0, blocks.Length);
            //}

            List<byte[]> packData = PackUnPackCommon.CreateMessage(array, rifdData, pathNumber);
            int res = 0;

            int index = (flag.Length > 0 && flag[0]) ? 0: 1;
            for (int i = index; i < packData.Count; i++)
            {
              
                res = WriteMessage(packData[i]);
                if (res != 0)
                    return OperatorResult.WriteDesBlockError;
            }
            currentReadRfid = string.Empty;
            return 0;
        }

        /// <summary>
        /// 块A操作标记	块B操作标记	操作说明
        /// 0x00	              0x00	读B块,写A块
        ///0x00	              0x01	读A块,写B块
        ///0x01	              0x01	读B块,写A块
        ///0x01	              0x00	读A块,写B块
        /// </summary>
        /// <param name="a">块A的操作标志</param>
        /// <param name="b">块B的操作标志</param>
        /// <returns>读块A返回a，读块B返回b</returns>
        private string ChooseBlock(int a, int b)
        {
            if (a == 0x00 && b == 0x00)
            {
                WriteLog.Log_Info("Write A,Read B");
                return "b";
            }
            if (a == 0x00 && b == 0x01)
            {
                WriteLog.Log_Info("Write B,Read A");
                return "a";
            }
            if (a == 0x01 && b == 0x01)
            {
                WriteLog.Log_Info("Write A,Read B");
                return "b";
            }
            if (a == 0x01 && b == 0x00)
            {
                WriteLog.Log_Info("Write B,Read A");
                return "a";
            }
            return null;
        }

        /// <summary>
        /// 计算buffer的CRC校验
        /// </summary>
        /// <param name="buffer">需要验证的CRC数据</param>
        /// <returns></returns>
        private ushort CaulatorCRC(byte[] buffer)
        {
            return this.CaulatorCRC(0xffff, buffer);
        }

        /// <summary>
        /// 得到CRC16校验码
        /// </summary>
        /// <param name="initValue">初始值（目前为0xfff）</param>
        /// <param name="buffer">需要验证的数组</param>
        /// <returns>返回CRC16校验码</returns>
        private ushort CaulatorCRC(ushort initValue, byte[] buffer)
        {
            if (initValue != 0xffff)
            {
                WriteLog.Log_Error("initValue error initValue=[" + initValue.ToString() + "]");
                return ushort.MaxValue;
            }
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < buffer.Length; j++)
            {
                sb.Append(buffer[j].ToString("x2"));
                sb.Append(" ");
            }
            WriteLog.Log_Info("Crc Data= " + sb.ToString());
            if (buffer == null || buffer.Length == 0)
            {
                WriteLog.Log_Error("buffer is null or empty!");
                return ushort.MaxValue;
            }

            ushort[] crc16_tab = new ushort[256] {
                                                        0x0000, 0x1189, 0x2312, 0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
                                                        0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
                                                        0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
                                                        0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
                                                        0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
                                                        0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
                                                        0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
                                                        0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
                                                        0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
                                                        0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
                                                        0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
                                                        0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
                                                        0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
                                                        0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
                                                        0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
                                                        0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
                                                        0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
                                                        0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
                                                        0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
                                                        0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
                                                        0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
                                                        0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
                                                        0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
                                                        0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
                                                        0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
                                                        0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
                                                        0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
                                                        0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
                                                        0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
                                                        0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
                                                        0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
                                                        0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78
                                                        };

            ushort crc16 = initValue;
            for (int i = 0; i < buffer.Length; i++)
            {
                crc16 = (ushort)((crc16 >> 8) ^ crc16_tab[(crc16 ^ buffer[i]) & 0xFF]);
            }
            return crc16;
        }

        /// <summary>
        /// 检查比较CRC的值
        /// </summary>
        /// <param name="buffer">需要的CRC参数</param>
        /// <param name="crc">验证数据字典</param>
        /// <returns>验证成功返回true，否则返回false</returns>
        private bool CheckCRC(byte[] buffer, ushort crc)
        {
            ushort lastCrc = CaulatorCRC(buffer);
            return lastCrc == crc;
        }


        #region IRfidRW 成员


        public int CloseSerialPort()
        {
            if (sp.IsOpen)
            {
                try
                {
                    sp.Close();
                    return 0;
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex.Message);
                    return -1;
                }
            }
            return -1;
        }

        #endregion

        #region IRfidRW 成员


        public int WriteTicketBoxRFID(RfidTicketboxInfo ri, byte pathNumber, string blockNumber)
        {
            ri.checkField = this.GetCrcData<RfidTicketboxInfo>(ri);
            if (blockNumber == "A")
                return WriteMessage<RfidTicketboxInfo>(ri, BlockParams.StaticArea, BlockParams.ticketboxRfidA, pathNumber,true);
            else
                return WriteMessage<RfidTicketboxInfo>(ri, BlockParams.StaticArea, BlockParams.ticketboxRfidB, pathNumber,true);
        }

        public int WriteMoneyBoxRFID(MoneyBoxRFID ri, byte pathNumber, string blockNumber)
        {
            ri.checkArea = this.GetCrcData<MoneyBoxRFID>(ri);
            if(blockNumber=="A")
            return WriteMessage<MoneyBoxRFID>(ri, BlockParams.StaticArea, BlockParams.moneyBoxRifdA, pathNumber,true);
            else
             return WriteMessage<MoneyBoxRFID>(ri, BlockParams.StaticArea, BlockParams.moneyBoxRfidB, pathNumber,true);
        }

        #endregion
    }
}


