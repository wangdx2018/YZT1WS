using System;
using System.Collections.Generic;
using System.Text;

namespace AFC.WS.BR.DataImportExport
{
    /// <summary>
    /// Function :   Read USB Physical Serial Number 
    /// Author:      chengzy
    /// Referencer:  maxc
    /// Time  :      2008-02-01
    /// </summary>
    public class GetUSBPhysicalSN
    {
       public string SerchByDeviceLetter(string diskNO)
       {
           USB.USBDevice device = USB.FindDriveLetter(diskNO);
           return device.InstanceID.Substring(22);
       }
    }
}
