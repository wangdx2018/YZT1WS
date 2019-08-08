using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AFC.WS.BR.DataImportExport
{
    public class ValidateAuthPhysicalSN
    {
        public bool readConf(string diskNum)
        {
            int count = 0;
            //定义移动硬盘的物理序列号
            string diskSerialNum = "";
            //定义认证文件中已经存在的移动硬盘物理序列号
            Boolean validUsb = false;
            byte[] AuthPhysicalSN;

            try
            {
                //获取移动硬盘的物理序列号
                GetUSBPhysicalSN gun = new GetUSBPhysicalSN();
                diskSerialNum = gun.SerchByDeviceLetter(diskNum);

                string path = diskNum + "AuthenticationFile.dat";
                if (!File.Exists(path))
                {
                    validUsb = false;
                }
                else
                {
                    AuthPhysicalSN = File.ReadAllBytes(path);
                    //存储介质的合法性认证等于物理序列号加密码
                    if (BitConverter.ToString(AuthPhysicalSN) == BitConverter.ToString(getAuthPhysicalSN(diskSerialNum)))
                    {
                        validUsb = true;
                    }
                    else
                    {
                        validUsb = false;
                    }
                   
                }
                return validUsb;
            }
            catch
            {
                return false;
            }
        }

        public byte[] getAuthPhysicalSN(string physicalSN)
        {
            byte[] result = Encoding.Default.GetBytes(physicalSN);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return output;
        }

        public string getFirstUSB()
        {
            string deviceUsb = "";
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString() == "Removable")
                {
                    deviceUsb = d.Name;
                    return deviceUsb;
                }
            }
            return deviceUsb;
        }


        public string getPcaRoot()
        {
            string PcaRoot = string.Empty;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType.ToString() == "Removable")
                {
                    FileInfo fileInfo = new FileInfo(d.Name + "\\config.ini");
                    if (fileInfo.Exists)
                    {
                        //FileInfo fileUpdate = new FileInfo(d.Name + "\\UpdatePCA.jar");
                        //if (fileUpdate.Exists)
                        //{
                            PcaRoot = Path.GetPathRoot(d.Name);
                            break;
                        //}
                    }
                }
            }

            return PcaRoot;
        }


        public bool writeConf(string diskNum)
        {
            int count = 0;
            //定义移动硬盘的物理序列号
            string diskSerialNum = "";
            //定义认证文件中已经存在的移动硬盘物理序列号
            Boolean validUsb = false;
            byte[] AuthPhysicalSN;

            try
            {
                //获取移动硬盘的物理序列号
                GetUSBPhysicalSN gun = new GetUSBPhysicalSN();
                diskSerialNum = gun.SerchByDeviceLetter(diskNum);

                string path = diskNum + "AuthenticationFile.dat";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
               
                //物理序列号加密
                AuthPhysicalSN = getAuthPhysicalSN(diskSerialNum);
                FileStream fp = new FileStream(path, FileMode.Create, FileAccess.Write);
                fp.Write(AuthPhysicalSN, 0, 16);
                fp.Close();


                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
