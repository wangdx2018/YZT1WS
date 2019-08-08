using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFC.WS.BR.Primission
{
    //--->加密，解密算法
    /// <summary>
    /// </summary>
    public  static class PassWordEncryptDecrypt
    {
        //-->加密密码
        /// <summary>
        /// 加密密码，返回加密之后的密码

        /// </summary>
        /// <param name="currentPwd">加密之前的密码</param>
        /// <returns>返回加密之后的密码</returns>
        public static string EncrptPassWord(string currentPwd)
        {
            if (string.IsNullOrEmpty(currentPwd))
                return null;
            string cipheretxt = null;
            encrypt(currentPwd, ref cipheretxt);
            return cipheretxt;
        }

        //-->解密密码
        /// <summary>
        /// 解密密码，返回解密之后的密码
        /// </summary>
        /// <param name="currentPwd">当前密码</param>
        /// <returns>返回解密之后的密码</returns>
        public static string DecryptPassWord(string currentPwd)
        {
            if (string.IsNullOrEmpty(currentPwd))
                return null;

            string plaintxt = null;
            decrypt(currentPwd, ref plaintxt);

            return plaintxt;
        }

        /// <summary>
        /// 加密算法
        /// </summary>
        /// <param name="plaintext">输入值</param>
        /// <param name="ciphertext">输出值</param>
        private static void encrypt(string plaintext, ref string ciphertext)
        {
            ciphertext = plaintext;
            //if (string.IsNullOrEmpty(plaintext))
            //    return;
            //char[] plainTxt = plaintext.ToCharArray();
            //int len = plaintext.Length;
            //char[] cipherTxt = new char[len];
            //int i;
            //for (i = 0; i < plainTxt.Length; i++)
            //{
            //    cipherTxt[i] =(char) (((plainTxt[i] - '0') + i) % 10 + '0');
            //}
            //ciphertext = new String(cipherTxt);
        }

        /// <summary>
        /// 解密算法
        /// </summary>
        /// <param name="ciphertext">解密字符串</param>
        /// <param name="plaintext">返回字符串</param>
        private static void decrypt(string ciphertext, ref string plaintext)
        {
            plaintext = ciphertext;
            //if (string.IsNullOrEmpty(ciphertext))
            //    return;

            //char[] cipherTxt = ciphertext.ToCharArray();

            //char[] plainTxt = new char[cipherTxt.Length];
            //int i;
            //for (i = 0; i < cipherTxt.Length; i++)
            //{
            //    if (cipherTxt[i] < (i + 0x30))
            //    {
            //        plainTxt[i] = (char)(10 + cipherTxt[i] - i);
            //    }
            //    else
            //    {
            //        plainTxt[i] = (char)(cipherTxt[i] - i);
            //    }
            //}
            //plaintext = new string(plainTxt);
        }

    }
}
