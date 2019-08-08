using System;
using System.Collections.Generic;
using System.Text;

namespace TJComm
{
    using AFC.BJComm.Data;

    /// <summary>
    /// 设备范围类，在WS中控制命令用到，SLE(BOM)不会用到
    /// 修改人：王冬欣   日期：20100411
    ///  增加了该类
    /// </summary>
   public class DeviceRange
   {
       /// <summary>
       /// 0 all line 
       /// 1:all station 
       /// 2:some deviceId
       /// </summary>
       [PackOrder(1),PackInt(4,ByteOrder.Moto)]
       public uint special_flag;


       /// <summary>
       /// 车站ID
       /// </summary>
       [PackOrder(2), PackInt(4, ByteOrder.Moto)]
       public ushort stationId;

       /// <summary>
       /// 设备范围
       /// </summary>
       [PackOrder(3), PackArray(4,ByteOrder.Moto,4,ByteOrder.Moto)]
       public List<uint> deviceRange = new List<uint>();
        
   }
}
