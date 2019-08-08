using System;
using System.Collections.Generic;

using System.Text;
using AFC.BOM2.Common;

namespace AFC.WS.UI.RfidRW
{

    /// <summary>
    /// modified by wangdx 20130531 修改了CreateMessage函数，不写些静态区域了。
    /// </summary>
    public class PackUnPackCommon
    {


        /// <summary>
        /// 创建读和获取状态的信息
        /// </summary>
        /// <param name="data">RFID枚举，Read Or Status</param>
        /// <param name="blockNumber">需要读取的某个块的编号（系统共64 block 0--63）</param>
        /// <param name="pathNumber">通道号（1，2，3，4）</param>
        /// <returns>返回打包之后的Message</returns>
        public static byte[] CreateReadOrStatusMessage(RFIDOperatorEnum data, byte blockNumber,byte pathNumber)
        {
            if (data == RFIDOperatorEnum.Write)
            {
                WriteLog.Log_Error("operator can't write");
                return null;
            }
            if (pathNumber > 4 || pathNumber < 0)
            {
                WriteLog.Log_Error("set pathNumber error! " + pathNumber.ToString() + "Path number is 1-4");
                return null;
            }
            try
            {
                Header header = new Header();
                header.blockAddress = blockNumber;
                header.pathNumber = pathNumber;
                if (data == RFIDOperatorEnum.Read)
                    header.wrOperation = 0x02;
                if (data == RFIDOperatorEnum.Status)
                    header.wrOperation = 0x03;
                byte[] buffer = PackHeader(header);
                byte[] total = new byte[28];
                Array.Copy(buffer, 0, total, 0, buffer.Length);
                byte sum = GetSumValue(total);
                total[27] = sum;
                return total;
            }
            catch (Exception ex)
            {
                WriteLog.Log_Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 打包头信息
        /// </summary>
        /// <param name="header">RFID头信息</param>
        /// <returns>返回Header打包之后的Byte数组</returns>
        private static byte[] PackHeader(Header header)
        {
            return AFC.BJComm.Data.DataProcessor.PackObject(header);
        }

        /// <summary>
        /// 将业务数据打包
        /// </summary>
        /// <param name="rifdData">业务数据（钱箱RFID或者是票箱RFID）</param>
        /// <returns>返回业务数据的打包之后的Byte[]</returns>
        private static byte[] PackData(object rifdData)
        {
            byte[] buffer = AFC.BJComm.Data.DataProcessor.PackObject(rifdData);
            return buffer;
        }

        /// <summary>
        /// 对包数据进行Sum验证
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns>成功返回sum，否则抛异常</returns>
        private static byte GetSumValue(byte[] buffer)
        {
            if (buffer.Length != 28)
            {
                WriteLog.Log_Error("包格式非法不是【28】个字节，Sum验证错误");
                throw new Exception("package's size error not 28! ");
            }
            int sum = 0xff;
            for (int i = 0; i < buffer.Length - 1; i++)
            {
                sum = sum ^ buffer[i];
            }
            return (byte)sum;
        }

        /// <summary>
        /// 创建写操作Message
        /// </summary>
        /// <param name="blocks">需要的块号</param>
        /// <param name="rfidData">业务层传递过来的对象(钱箱RFID，票箱RFID)</param>
        /// <param name="pathNumber">通道号，RFIDRW共有4个通道号 1，2，3，4</param>
        /// <returns>返回每个28个字节的发送的Block块数组</returns>
        public static List<byte[]> CreateMessage(byte[] blocks, object rfidData,byte pathNumber)
        {
            if (rfidData == null || pathNumber > 0 && pathNumber < 4 && blocks == null)
                return null;
            List<byte[]> listAfter = new List<byte[]>();
            List<byte[]> SplitArray = new List<byte[]>();
            byte[] buffer = AFC.BJComm.Data.DataProcessor.PackObject(rfidData);
            int offset = 0;
            byte[] staticArea = new byte[4];
            Array.Copy(buffer, 0, staticArea, 0, 4);//static Area
            offset += 4;
            SplitArray.Add(staticArea); //modified by wangdx 20130531,目前写数据每次都写静态区，现在不写了。
            for (int i = 1; i < blocks.Length; i++)
            {
                byte[] temp = new byte[16];
                Array.Copy(buffer, offset, temp, 0, temp.Length);
                offset += temp.Length;
                SplitArray.Add(temp);
            }
            for (int i = 0; i < blocks.Length; i++)
            {
                Header header = new Header();
                header.blockAddress = blocks[i];
                header.pathNumber = pathNumber;
                header.wrOperation = 0x01;
                byte[] headerArray = PackHeader(header);
                byte[] total = new byte[28];
                //todo: 0为静态区
                offset = 0;
                Array.Copy(headerArray, 0, total, 0, headerArray.Length);
                offset += headerArray.Length;
                offset += 4;
                Array.Copy(SplitArray[i], 0, total, offset, SplitArray[i].Length);
                byte sum = GetSumValue(total);
                total[27] = sum;
                listAfter.Add(total);
            }
            return listAfter;
        }
    }
}
