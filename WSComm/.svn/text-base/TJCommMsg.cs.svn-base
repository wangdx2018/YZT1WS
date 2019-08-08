using System;
using System.Collections.Generic;

using System.Text;
using AFC.BJComm.Data;
using System.Security.Cryptography;
using System.Reflection;

namespace TJComm
{
    using AFC.BOM2.Common;

    public class TJCommMessage
    {
        #region  数据定义
        /// <summary>
        /// 协议号
        /// </summary>
        public const uint PROTOCALVERSON = 0x010100;

        /// <summary>
        /// 存活包消息类型
        /// </summary>
        public const ushort BeatHeartMsgType = 0x1300;

        /// <summary>
        /// 当前的SessionID
        /// </summary>
        public static uint currentSessionId=0;

        /// <summary>
        /// 本机DeviceId
        /// </summary>
        public static uint localDeviceId;

        /// <summary>
        /// 服务器DeviceId
        /// </summary>
        public static uint serverDeviceId;

        /// <summary>
        /// 车站ID
        /// </summary>
        public static ushort stationId;

        /// <summary>
        /// 设置设备的ID
        /// </summary>
        /// <param name="localDeviceId">本机设备ID</param>
        /// <param name="serverDeviceId">服务器设备ID</param>
        public static void SetDevcieId(uint localDeviceId, uint serverDeviceId,ushort stationId)
        {
            TJCommMessage.localDeviceId = localDeviceId;
            TJCommMessage.serverDeviceId = serverDeviceId;
            TJCommMessage.stationId = stationId;
        }

        /// <summary>
        /// 解包对象实现的接口
        /// </summary>
        public static IMutableInstance config = null;

        #endregion

        /// <summary>
        /// 通讯头
        /// </summary>
        [PackOrder(1)]
        [PackStruct(0, ByteOrder.Moto)]
        public CommHeader header=new CommHeader();

        /// <summary>
        ///  打包字段
        /// </summary>
        [PackOrder(2)]
        [PackStruct(0, ByteOrder.Moto)]
        public object packageBody=new object();

        /// <summary>
        /// MD5
        /// </summary>
        [PackOrder(3)]
        [PackArray(0, ByteOrder.Moto, 1, ByteOrder.Moto)]
        public byte[] md5 = new byte[16];

        /// <summary>
        /// 设置Header的初始值,初始化时调用
        /// </summary>
        /// <param name="data">上层传递过来的数据</param>
        public TJCommMessage()
        {          
        }


        /// <summary>
        /// 得到当前的SessionId
        /// </summary>
        public static uint GetCurrentSessionId
        {
            get { return currentSessionId; }
        }
       
        #region Pack or UnPack Function

        /// <summary>
        /// 为天津的数据打包
        /// 只需要填写，messageType，消息类型，其他不用填
        /// </summary>
        /// <returns>返回byte[]数据</returns>
        public static byte[] PackTJMsg(TJCommMessage msg)
        {
            if (msg == null)
                return null;
            byte[] tranData = DataProcessor.PackObject(msg.packageBody); //1.packObject
            msg.header.sessionId = currentSessionId;
            msg.header.packetLength =CommHeader.Head_LEN+16 + tranData.Length-4; //header+md5+commBody-4
            byte[] headerBuf = DataProcessor.PackObject(msg.header);
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] buffer = new byte[CommHeader.Head_LEN + tranData.Length]; //prepare md5 data md5 contains packageLenth
            Array.Copy(headerBuf, 0, buffer, 0, headerBuf.Length);
            Array.Copy(tranData, 0, buffer, headerBuf.Length, tranData.Length);
            msg.md5 = provider.ComputeHash(buffer);//cal md5
            return DataProcessor.PackObject(msg);
       }

        /// <summary>
        /// 公共的解包函数，需要客户程序设置IMutableInstance接口
        /// </summary>
        /// <param name="buffer">Byte[]数组</param>
        /// <param name="instance">实现IMutableInstance接口的对象</param>
        /// <returns>返回解包后的数据</returns>
        public static TJCommMessage UnPackData(byte[] buffer,IMutableInstance instance)
        {

            if (buffer == null)
                return null;
            if (buffer.Length < 16 + CommHeader.Head_LEN)
                return null;
            if (instance == null)
                return null;
            if (!CheckPackageMD5Valid(buffer))
            {
                AFC.BOM2.Common. WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"MD5 Check error!");
                return null;
            }
            try
            {
                TJCommMessage tjMsg = new TJCommMessage();
                byte[] headerBuffer = new byte[CommHeader.Head_LEN];
                Array.Copy(buffer, 0, headerBuffer, 0, CommHeader.Head_LEN);

                object tempHeader = tjMsg.header;
                DataProcessor.UnpackStruct(ref tempHeader,
                    new PackStructAttribute(0,ByteOrder.Moto),
                    new System.IO.MemoryStream(headerBuffer));

                byte[] packBody=new byte[buffer.Length-16-CommHeader.Head_LEN];

                Array.Copy(buffer, CommHeader.Head_LEN, packBody, 0, packBody.Length);
                object convertInstance = instance;
                DataProcessor.UnpackObject(tjMsg, ref convertInstance, packBody);
                tjMsg.packageBody = convertInstance;

                Array.Copy(buffer, buffer.Length - 16, tjMsg.md5, 0, 16);

                return tjMsg;
            }
            catch (Exception ex)
            {
                AFC.BOM2.Common. WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 配置了IMutableInstance接口，可以直接调用该类进行解析
        /// </summary>
        /// <param name="buffer">buffer</param>
        /// <returns>返回解析之后的数据</returns>
        public static TJCommMessage UnPackData(byte[] buffer)
        {
            if (config == null)
            {
                 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"Unpack error please set IMutableInstance first!");
                return null;
            }
            return UnPackData(buffer, config);
        }

        /// <summary>
        /// 创建CommHeader
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="opearatorId">操作员ID</param>
        /// <param name="stationId">车站ID</param>
        /// <param name="localDeviceId">本机设备ID</param>
        /// <param name="serverDeviceId">服务器的设备ID</param>
        /// <param name="commType">指令类型</param>
        /// <returns>返回CommonHeader对象</returns>
        public static CommHeader CreateHeader(ushort messageType,
            uint localDeviceId, 
            uint serverDeviceId, 
            CommandType commType)
        {
            CommHeader header = new CommHeader();
            header.messageType = messageType;
            header.senderId = localDeviceId;
            header.receiveId = serverDeviceId;
            header.protocalVersion = PROTOCALVERSON;
            header.sessionFlagMap = commType;
            header.sessionId = TJCommMessage.currentSessionId;
            return header;
        }

        /// <summary>
        /// 创建通讯头
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="commType">命令类型</param>
        /// <returns>返回通讯头对象</returns>
        public static CommHeader CreateHeader(ushort messageType, CommandType commType)
        {
            return CreateHeader(messageType, TJCommMessage.localDeviceId, TJCommMessage.serverDeviceId, commType);
        }

        /// <summary>
        /// 创建TJCommMsg
        /// </summary>
        /// <param name="header">通讯头</param>
        /// <param name="packBody">交易字段</param>
        /// <returns>返回TJCommMsg（不带MD5校验，打包PackTJMsg()后产生MD5）</returns>
        public static TJCommMessage CreateTJCommMsg(CommHeader header, object packBody)
        {
            TJCommMessage msg = new TJCommMessage();
            msg.header = header;
            msg.packageBody = packBody;
            return msg;
        }

    
        /// <summary>
        /// 检查buffer的MD5校验
        /// </summary>
        /// <param name="buffer">二进制的数组</param>
        /// <returns>MD5验证成功返回true，否则返回false</returns>
        public static bool CheckPackageMD5Valid(byte[] buffer)
        {
            if (buffer == null)
            {
                 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"CheckPackageMD5 Error function params error,buffer is null ");
                return false;
            }
            if (buffer.Length <= 16 + 28) //16:md5 lenth  40:header lenth
            {
                 WriteLog.Log_Error("["+System.Threading.Thread.CurrentThread.Name+"]"+"CheckPackageMD5 Error function params error,buffer's lenth <16+28. lenth is  [" + buffer.Length + "  ].");
                return false;
            }
            byte[] countData = new byte[buffer.Length - 16];
            Array.Copy(buffer, 0, countData, 0, countData.Length);
            byte[] lastMD5 = new byte[16];
            Array.Copy(buffer, countData.Length, lastMD5, 0, 16);
            string lastMD5Str = PrintBufferData(lastMD5);
            WriteLog.Log_Info("receive Md5 is [" + lastMD5Str+"]");
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] currentMd5 = provider.ComputeHash(countData, 0, countData.Length);
            string currentMd5Str = PrintBufferData(currentMd5);
            WriteLog.Log_Info("local cal md5 is [" + currentMd5Str + "]");
            return CheckBufferEqual(lastMD5, currentMd5);
        }

        /// <summary>
        /// 检查两个byte数组是否相同
        /// </summary>
        /// <param name="buffer1">byte[] buffer1</param>
        /// <param name="buffer2">byte[] buffer2</param>
        /// <returns>相等返回true，否则返回false</returns>
        private static bool CheckBufferEqual(byte[] buffer1, byte[] buffer2)
        {
            if (buffer1 == null || buffer1.Length == 0)
                return false;
            if (buffer2 == null || buffer2.Length == 0)
                return false;
            if (buffer1.Length != buffer2.Length)
                return false;
            for (int i = 0; i < buffer1.Length; i++)
            {
                if (buffer1[i] != buffer2[i])
                    return false;
            }
            return true;
        }

        #endregion


        public override string ToString()
        {
            string value = Environment.NewLine+ ObjectToString(this);
            return value;
            //return base.ToString();
        }

        private static String PrintBufferData(byte[] buffer)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                sb.Append(buffer[i].ToString("x2"));
                sb.Append(" ");
            }
            return sb.ToString();
        }


        private static String ObjectToString(Object data)
        {

            StringBuilder sb = new StringBuilder();
         

            if (data == null)
                return "params error!";


            if (data is ValueType)
            {
                return string.Empty;
            }
            FieldInfo[] fieldCollection = data.GetType().GetFields();

            List<FieldInfo> fieldList = new List<FieldInfo>();
            foreach (var temp in fieldCollection)
            {
                if (temp.GetCustomAttributes(typeof(PackOrderAttribute), true).Length != 0)
                {
                    fieldList.Add(temp);
                }
            }

            foreach (var temp in fieldList)
            {
                object value = temp.GetValue(data);

                #region base data
                if (temp.FieldType.Name == "Byte" ||
                    temp.FieldType.Name == "UInt16" ||
                    temp.FieldType.Name == "UInt32" ||
                    temp.FieldType.Name == "UInt64" ||
                    temp.FieldType.Name == "String"||
                    temp.FieldType.IsEnum)
                {
                    if (temp.FieldType.Name == "String" ||
                        temp.FieldType.IsEnum)
                    {
                        sb.Append(string.Format("fieldName=[{0}],type=[{2}],value=[{1}]", temp.Name, temp.GetValue(data).ToString(), temp.FieldType.ToString()));
                    }
                    else
                    {
                        sb.Append(string.Format("fieldName=[{0}],type=[{2}],value=[{1}]", temp.Name, 
                            uint.Parse(temp.GetValue(data).ToString()).ToString("x2"), 
                            temp.FieldType.ToString()));
                    }
                    sb.Append(Environment.NewLine);
                    continue;
                }
                #endregion

                #region base data
                if (temp.FieldType.Name == "Int16" ||
                    temp.FieldType.Name == "Int32" ||
                    temp.FieldType.Name == "Int64"
                    )
                {
                    sb.Append(string.Format("fieldName=[{0}],type=[{2}],value=[{1}]", temp.Name, 
                    System.Int64.Parse(temp.GetValue(data).ToString()).ToString("x2"), 
                    temp.FieldType.ToString()));
                    sb.Append(Environment.NewLine);
                    continue;
                }
                #endregion

                #region byte[] data
                if (temp.FieldType.Name == "Byte[]")
                {

                    StringBuilder bufTemp = new StringBuilder();
                    byte[] buffer = temp.GetValue(data) as byte[];
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        bufTemp.Append(buffer[i].ToString());
                    }
                    sb.Append(string.Format("fieldName=[{0}],type=[{2}],value=[{1}]", temp.Name, bufTemp.ToString(), temp.FieldType.ToString()));
                    sb.Append(Environment.NewLine);

                   
                   
                    sb.Append(Environment.NewLine);
                    continue;
                }
                #endregion


                # region list data
                if (value is System.Collections.IList)
                {
                    
                    System.Collections.IList list = value as System.Collections.IList;
                    sb.Append(string.Format("fieldName=[{0}],type=[{1}] \r\n ",
                        temp.Name,
                        temp.FieldType.ToString()));
                    if (list.Count == 0)
                    {
                        sb.Append("list count is 0!");
                        sb.Append(Environment.NewLine);
                        continue;
                    }
                    for (int ii = 0; ii < list.Count; ii++)
                    {

                        sb.Append(Environment.NewLine);
                        sb.Append(string.Format("item[{0}]\r\n", ii.ToString()));
                        sb.Append("**************" + list[ii].ToString() + " value list*************");
                        sb.Append(Environment.NewLine);
                        sb.Append(ObjectToString(list[ii]));
                        sb.Append("**************" + list[ii].ToString() + " end value list*******************");
                        sb.Append(Environment.NewLine);
                    }
                    sb.Append(Environment.NewLine);
                    continue;
                }
                #endregion

                #region class data
                if (temp.FieldType.IsClass &&
                    temp.FieldType.Name != "String" &&
                    temp.FieldType.Name != "Byte[]")
                {
                  
                    sb.Append(string.Format("fieldName=[{0}],type=[{1}]  ",
                        temp.Name,
                        temp.FieldType.ToString()));
                    sb.Append(Environment.NewLine);
                    sb.Append("**************" + temp.Name + " value list*************");
                    sb.Append(Environment.NewLine);
                    sb.Append(ObjectToString(value));
                    sb.Append("**************" + temp.Name + " end value list*******************");
                    sb.Append(Environment.NewLine);
                    continue;
                }
                #endregion



            }

            return sb.ToString();
        }


    }

}
