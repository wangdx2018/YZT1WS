using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace AFC.WS.UI.RfidRW
{
    /// <summary>
    /// author:wangdx  date :20100609
    /// 
    /// RFID读写器底层驱动API
    /// </summary>
    internal class RFIDCommon
    {
        /// <summary>
        /// RFID读写器初始化，相当于连接指令
        /// </summary>
        /// <param name="portNo">串口号</param>
        /// <returns></returns>
        [DllImport(@".\DLL\serial_dll.dll", EntryPoint = "RFInitComm")]
        public static extern int RFInitComm(int portNo);

        /// <summary>
        /// 关闭RFID读写器，相当于关闭栓串口
        /// </summary>
        /// <param name="handle">读写器句柄</param>
        /// <returns>大于等于0成功，失败返回小于0的数据</returns>
        [DllImport(@".\DLL\serial_dll.dll", EntryPoint = "RFCloseComm")]
        public static extern int RFCloseComm(int handle);

        /// <summary>
        /// 读取RFID信息
        /// </summary>
        /// <param name="handle">读写器句柄</param>
        /// <param name="block">块号，1 静态区，2 逻辑块A 3 逻辑块B  4 物理块号</param>
        /// <param name="data">读到的数据</param>
        /// <param name="lenth">数据长度</param>
        /// <returns>返回读取数据的长度，失败返回小于0的数据</returns>
        [DllImport(@".\DLL\serial_dll.dll", EntryPoint = "RFReadData", CharSet = CharSet.Ansi)]
        public static extern int RFReadData(int handle, int block, byte[] data, int lenth);

        /// <summary>
        /// 写入RDID数据
        /// </summary>
        /// <param name="handle">RFID句柄</param>
        /// <param name="block">1 静态区 2 块A，3 块B</param>
        /// <param name="data">需要写入的数据</param>
        /// <param name="lenth">写入的数据长度</param>
        /// <returns>返回写入数据的长度，失败返回小于0的数据</returns>
        [DllImport(@".\DLL\serial_dll.dll", EntryPoint = "RFWriteData", CharSet = CharSet.Ansi)]
        public static extern int RFWriteData(int handle, int block, byte[] data, int lenth);
    }
}
