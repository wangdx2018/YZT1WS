using System;
using System.Collections.Generic;

using System.Text;
using AFC.BOM2.Common;

namespace AFC.WS.UI.RfidRW
{
    /// <summary>
    /// RFID读写器模拟器程序，在没有外设时模拟RFID信息
    /// </summary>
    public class RFIDSimulator:IRfidRW
    {

        #region IRfidRW 成员

        public void SetSerialPort(int baudRate, System.IO.Ports.Parity parity, System.IO.Ports.StopBits stopBits, int dataBits, string portName)
        {
            WriteLog.Log_Info("SetSerialPort is called");
        }

        public bool Connect(byte pathNumber)
        {
            WriteLog.Log_Info("Connect(byte pathNumber) called");
            return true;
        }

        public string GetRFIDPhysicalId(byte pathNumber)
        {
            WriteLog.Log_Info("Connect(byte pathNumber) called");
            return "abcdef11";
        }

        public RfidTicketboxInfo ReadTicketBoxRFID(byte pathNumber, out int res)
        {
            res = 0;
            WriteLog.Log_Info("RfidTicketboxInfo ReadTicketBoxRFID(byte pathNumber, out int res) called");
            RfidTicketboxInfo info = new RfidTicketboxInfo();
            info.operatorId = "111111";
            info.ticketNumber = 100;
            info.lastOpeatorTime = "20101203123211";
            info.stationCode = "9821";
            info.deviceId = "02092101";
            info.cardPhysicalType = 1;
            info.prevAddValueCard = 1;
            info.setupLoaction = 1;
            info.ticketboxId = "98210101";
            info.ticketboxLoactionStatus = 1;
            info.cardIssueId = 1;
            info.cardPhysicalType = 2;
            info.ticketProductType = 1;
            info.extendProductId = 1;
            return info;
        }

        public MoneyBoxRFID ReadMoneyBoxRFID(byte pathNumber, out int res)
        {
            res = 0;
            WriteLog.Log_Info("ReadMoneyBoxRFID(byte pathNumber, out int res) called");
            return new MoneyBoxRFID();
        }

        public int WriteTicketBoxRFID(RfidTicketboxInfo ri, byte pathNumber)
        {
            WriteLog.Log_Info("public int WriteTicketBoxRFID(RfidTicketboxInfo ri, byte pathNumber) called");
            return 0;
        }

        public int WriteMoneyBoxRFID(MoneyBoxRFID ri, byte pathNumber)
        {
            WriteLog.Log_Info("WriteMoneyBoxRFID(MoneyBoxRFID ri, byte pathNumber) called");
            return 0;
        }

        #endregion

        #region IRfidRW 成员


        public int CloseSerialPort()
        {
            WriteLog.Log_Info("Close serial port successfully");
            return 0;
        }

        #endregion

        #region IRfidRW 成员


        public int WriteTicketBoxRFID(RfidTicketboxInfo ri, byte pathNumber, string blockNumber)
        {
            return 0;
        }

        public int WriteMoneyBoxRFID(MoneyBoxRFID ri, byte pathNumber, string blockNumber)
        {
            return 0;
        }

        #endregion
    }
}
