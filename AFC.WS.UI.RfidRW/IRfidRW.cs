using System;
using System.Collections.Generic;

using System.Text;
using System.IO.Ports;

namespace AFC.WS.UI.RfidRW
{
    //--->RFID读写器接口
    /// <summary>
    /// added by wangdx 20100308
    /// RFID读写器接口
    /// </summary>
    public interface IRfidRW
    {

        /// <summary>
        /// 设置串口通信的信息
        /// </summary>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        /// <param name="stopBits">停止位</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="portName">串口名</param>
        void SetSerialPort(int baudRate, Parity parity, StopBits stopBits, int dataBits, string portName);

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns>成功返回0，否则返回-1</returns>
        int CloseSerialPort();
      

        /// <summary>
        /// 判断该RFID设备是否在线
        /// </summary>
        /// <returns>在线返回true，否则返回false</returns>
        bool Connect(byte pathNumber);

        /// <summary>
        /// 得到RFID标签的物理ID
        /// </summary>
        /// <returns>读取0扇区的0块，获得物理ID</returns>
        string GetRFIDPhysicalId(byte pathNumber);

        /// <summary>
        /// 读票箱的RFID信息
        /// </summary>
        /// <param name="res">成功0，否则错误代码</param>
        /// <returns>返回票箱的RFID实体信息</returns>
        RfidTicketboxInfo ReadTicketBoxRFID(byte pathNumber,out int res);

        /// <summary>
        /// 读钱箱的RFID信息
        /// </summary>
        /// <param name="res">成功0，否则-1</param>
        /// <returns>返回钱箱的实体类</returns>
        MoneyBoxRFID ReadMoneyBoxRFID(byte pathNumber,out int res);

        /// <summary>
        /// 写票箱的RFID
        /// </summary>
        /// <param name="ri">票箱的RFID信息</param>
        /// <returns>写成功返回0，否则返回-1</returns>
        int WriteTicketBoxRFID(RfidTicketboxInfo ri,byte pathNumber);

        /// <summary>
        /// 写钱箱的RFID
        /// </summary>
        /// <param name="ri">钱箱的RFID信息</param>
        /// <returns>写成功返回0，否则返回-1</returns>
        int WriteMoneyBoxRFID(MoneyBoxRFID ri,byte pathNumber);

        /// <summary>
        /// 写票箱的RFID信息，不对进行读取，为了初始化函数使用
        /// </summary>
        /// <param name="ri">票箱信息</param>
        /// <param name="pathNumber">通道号</param>
        /// <param name="blockNumber">块标记(A or B)</param>
        /// <returns></returns>
        int WriteTicketBoxRFID(RfidTicketboxInfo ri, byte pathNumber, string blockNumber);

        /// <summary>
        /// 写钱箱RFID信息，为了初始化函数使用
        /// </summary>
        /// <param name="ri">钱箱信息</param>
        /// <param name="pathNumber">通道号</param>
        /// <param name="blockNumber">块号（A or B)</param>
        /// <returns></returns>
        int WriteMoneyBoxRFID(MoneyBoxRFID ri, byte pathNumber, string blockNumber);
     


    }
}
