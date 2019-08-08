using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;
using System.Windows.Forms;

namespace AFC.WS.BR.DataImportExport
{
    /// <summary>
    /// 读写INI文件
    /// </summary>
    public class AccessDatFile
    {
        //文件路径
        private string strPath;
        //初期化文件路径
        public AccessDatFile(string strDatFilePath)
        {
            strPath = strDatFilePath;
        }
        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="strSection">节</param>
        /// <param name="strKey">键</param>
        /// <param name="strVal">值</param>
        /// <param name="strFilePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string strSection,
            string strKey, string strVal, string strFilePath);
        /// <summary>
        /// 读INI文件
        /// </summary>
        /// <param name="strSection">节</param>
        /// <param name="strKey">键</param>
        /// <param name="strDef"></param>
        /// <param name="retVal"></param>
        /// <param name="intSize"></param>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string strSection,
            string strKey, string strDef, StringBuilder retVal, int intSize, string strFilePath);


        [DllImport("kernel32", EntryPoint = "GetPrivateProfileSectionNames")]
        private static extern int GetPrivateProfileSectionNames(byte[] lpszReturnBuffer, int intSize, string strFilePath);

        /// <summary>
        /// 生成dat文件。
        /// </summary>
        /// <param name="strSection">节</param>
        /// <param name="strKey">键</param>
        /// <param name="strValue">键值</param>
        public void DatWriteValue(string strSection, string strKey, string strValue)
        {
            try
            {
                WritePrivateProfileString(strSection, strKey, strValue, strPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 读取Dat文件。
        /// </summary>
        /// <param name="strSection">节</param>
        /// <param name="strKey">键</param>
        /// <returns></returns>
        public string DatReadValue(string strSection, string strKey)
        {
            StringBuilder objStringBuilder = new StringBuilder(255);
            try
            {
                int i = GetPrivateProfileString(strSection, strKey, "无法读取对应数值！", objStringBuilder, 255, strPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objStringBuilder.ToString();
        }

        public string[] GetSectionsName()
        {
            byte[] objBuffer = new byte[500];
            string[] returnStr;
            try
             {
                 int i = GetPrivateProfileSectionNames(objBuffer, 500, strPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
             string[] objStr= Encoding.GetEncoding("GB2312").GetString(objBuffer).Split('\0');
             
             string preValue = "1";
             int secLength = 0;
             for (int i = 0; i < objStr.Length; i++)
             {
                 if (objStr[i] == "")
                 {
                     if (preValue == "")
                     {
                         secLength = i-1;
                         break;
                     }
                 }
                 preValue = objStr[i];
             }
             returnStr = new string[secLength];
             Array.Copy(objStr, returnStr, secLength);
             return returnStr;
        }

        /// <summary>
        /// 数字前补零
        /// </summary>
        /// <param name="num">需要转换的数字</param>
        /// <returns>前补零后的字串</returns>
        public string intConverString(int num)
        {
            string str = Convert.ToString(num);
            int len = 4 - str.Length;
            if (len > 0)
            {
                for (int i = 0; i < len; i++)
                {
                    str = "0" + str;
                }
            }
            return str;
        }

        public string[] getDevName(string devCount)
        {
            int intDevCount = int.Parse(devCount);
            string[] devName = new string[intDevCount];
            for (int i = 0; i < intDevCount; i++)
            {
                devName[i] = DatReadValue("FILE", "DeviceID" + (i + 1));
            }
            return devName;
        }
        public Hashtable getHashDevName(int devCount)
        {
            Hashtable hashDev = new Hashtable();
            for (int i = 0; i < devCount; i++)
            {
                hashDev.Add("DeviceID" + (i + 1),DatReadValue("FILE", "DeviceID" + (i + 1)));
            }
            return hashDev;
        }

    }
}
