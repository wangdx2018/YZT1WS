using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace TJComm
{
    public abstract class ResultCode
    {
        /// <summary>
        /// 成功 = 0。
        /// </summary>
        public const int OK = 0;

        /* System Error < 0 */

        public const int NET_ERROR = -100;

        public const int NET_BREAK = -101;

        public const int NET_READ_ERROR = -102;

        public const int NET_READ_TIMEOUT = -103;

        public const int NET_WRITE_ERROR = -104;


        public const int NET_PACK_SIZE_WRONG = -105;

        public const int NET_PACK_SIZE_ZERO = -106;

        /* Database Error here, -200 */

        public const int DB_IS_CLOSED = -201;
        public const int DB_CONNECT_ERROR = -202;
        public const int DB_CLOSE_ERROR = -203;
        public const int DB_ERROR = -200;
        public const int DB_SQL_ERROR = -204;
        public const int DB_SQL_EXEC_ERROR = -206;
        public const int DB_EMPTY_RESAULT = -210;




        /* Business Error > 100 */


        public const int XDR_PACK_FIELD_ERROR = -310;
        public const int XDR_UNPACK_FIELD_ERROR = -309;


        public const int XDR_NO_FIELD_DEF = -311;

        public const int XDR_BUF_SIZE_ERROR = -312;

        public const int XDR_TXN_CODE_ERROR = -313;

        public const int XDR_DATA_TYPE_ERROR = -302;


        public const int XDR_MESSAGE_SIZE_WRONG = -303;

        public const int XDR_CHECK_SUM_ERROR = -305;

        //FTP Error, -400
        public const int FTP_URI_FORMAT_ERROR = -401;

        public const int FTP_NET_ERROR = -410;

        public const int FTP_IO_ERROR = -411;


        public static string GetErrorCodeName(int retcode)
        {
            Type constType = typeof(ResultCode);

            FieldInfo[] fields = constType.GetFields();

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo info = fields[i];
                if (info.FieldType == typeof(int) &&
                    retcode.Equals(info.GetValue(null)))
                    return info.Name;
            }

            return "RESULT_CODE_UNKOWN_ERROR";
        }
    }
}
