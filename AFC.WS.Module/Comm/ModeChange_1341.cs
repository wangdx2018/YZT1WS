using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.Model.Comm
{
    using AFC.BJComm.Data;

   public class ModeChange_1341:AbstractCommBody
    {
       /// <summary>
       /// 模式设备ID
       /// </summary>
       [PackOrder(3),PackInt(4,ByteOrder.Moto)]
       public uint mode_station_id;

       /// <summary>
       /// 模式代码
       /// </summary>
       [PackOrder(4),PackInt(4,ByteOrder.Moto)]
       public uint mode_code;


       public override string ToString()
       {
           return string.Format("模式车站设备ID={0},模式代码={1}", mode_station_id.ToString("x2"), mode_code.ToString("x2"));
           //return base.ToString();
       }
    }
}
