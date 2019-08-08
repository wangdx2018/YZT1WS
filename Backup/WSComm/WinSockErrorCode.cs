using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TJComm
{
	public static class WinSockErrorCode
	{
		// Return code/value ;
		public const int WSA_INVALID_HANDLE = 6;
		public const int WSA_NOT_ENOUGH_MEMORY = 8;
		public const int WSA_INVALID_PARAMETER = 87;
		public const int WSA_OPERATION_ABORTED = 995;
		public const int WSA_IO_INCOMPLETE = 996;
		public const int WSA_IO_PENDING = 997;
		public const int WSAEINTR = 10004;
		public const int WSAEBADF = 10009;
		public const int WSAEACCES = 10013;
		public const int WSAEFAULT = 10014;
		public const int WSAEINVAL = 10022;
		public const int WSAEMFILE = 10024;
		public const int WSAEWOULDBLOCK = 10035;
		public const int WSAEINPROGRESS = 10036;
		public const int WSAEALREADY = 10037;
		public const int WSAENOTSOCK = 10038;
		public const int WSAEDESTADDRREQ = 10039;
		public const int WSAEMSGSIZE = 10040;
		public const int WSAEPROTOTYPE = 10041;
		public const int WSAENOPROTOOPT = 10042;
		public const int WSAEPROTONOSUPPORT = 10043;
		public const int WSAESOCKTNOSUPPORT = 10044;
		public const int WSAEOPNOTSUPP = 10045;
		public const int WSAEPFNOSUPPORT = 10046;
		public const int WSAEAFNOSUPPORT = 10047;
		public const int WSAEADDRINUSE = 10048;
		public const int WSAEADDRNOTAVAIL = 10049;
		public const int WSAENETDOWN = 10050;
		public const int WSAENETUNREACH = 10051;
		public const int WSAENETRESET = 10052;
		public const int WSAECONNABORTED = 10053;
		public const int WSAECONNRESET = 10054;
		public const int WSAENOBUFS = 10055;
		public const int WSAEISCONN = 10056;
		public const int WSAENOTCONN = 10057;
		public const int WSAESHUTDOWN = 10058;
		public const int WSAETOOMANYREFS = 10059;
		public const int WSAETIMEDOUT = 10060;
		public const int WSAECONNREFUSED = 10061;
		public const int WSAELOOP = 10062;
		public const int WSAENAMETOOLONG = 10063;
		public const int WSAEHOSTDOWN = 10064;
		public const int WSAEHOSTUNREACH = 10065;
		public const int WSAENOTEMPTY = 10066;
		public const int WSAEPROCLIM = 10067;
		public const int WSAEUSERS = 10068;
		public const int WSAEDQUOT = 10069;
		public const int WSAESTALE = 10070;
		public const int WSAEREMOTE = 10071;
		public const int WSASYSNOTREADY = 10091;
		public const int WSAVERNOTSUPPORTED = 10092;
		public const int WSANOTINITIALISED = 10093;
		public const int WSAEDISCON = 10101;
		public const int WSAENOMORE = 10102;
		public const int WSAECANCELLED = 10103;
		public const int WSAEINVALIDPROCTABLE = 10104;
		public const int WSAEINVALIDPROVIDER = 10105;
		public const int WSAEPROVIDERFAILEDINIT = 10106;
		public const int WSASYSCALLFAILURE = 10107;
		public const int WSASERVICE_NOT_FOUND = 10108;
		public const int WSATYPE_NOT_FOUND = 10109;
		public const int WSA_E_NO_MORE = 10110;
		public const int WSA_E_CANCELLED = 10111;
		public const int WSAEREFUSED = 10112;
		public const int WSAHOST_NOT_FOUND = 11001;
		public const int WSATRY_AGAIN = 11002;
		public const int WSANO_RECOVERY = 11003;
		public const int WSANO_DATA = 11004;
		public const int WSA_QOS_RECEIVERS = 11005;
		public const int WSA_QOS_SENDERS = 11006;
		public const int WSA_QOS_NO_SENDERS = 11007;
		public const int WSA_QOS_NO_RECEIVERS = 11008;
		public const int WSA_QOS_REQUEST_CONFIRMED = 11009;
		public const int WSA_QOS_ADMISSION_FAILURE = 11010;
		public const int WSA_QOS_POLICY_FAILURE = 11011;
		public const int WSA_QOS_BAD_STYLE = 11012;
		public const int WSA_QOS_BAD_OBJECT = 11013;
		public const int WSA_QOS_TRAFFIC_CTRL_ERROR = 11014;
		public const int WSA_QOS_GENERIC_ERROR = 11015;
		public const int WSA_QOS_ESERVICETYPE = 11016;
		public const int WSA_QOS_EFLOWSPEC = 11017;
		public const int WSA_QOS_EPROVSPECBUF = 11018;
		public const int WSA_QOS_EFILTERSTYLE = 11019;
		public const int WSA_QOS_EFILTERTYPE = 11020;
		public const int WSA_QOS_EFILTERCOUNT = 11021;
		public const int WSA_QOS_EOBJLENGTH = 11022;
		public const int WSA_QOS_EFLOWCOUNT = 11023;
		public const int WSA_QOS_EUNKOWNPSOBJ = 11024;
		public const int WSA_QOS_EPOLICYOBJ = 11025;
		public const int WSA_QOS_EFLOWDESC = 11026;
		public const int WSA_QOS_EPSFLOWSPEC = 11027;
		public const int WSA_QOS_EPSFILTERSPEC = 11028;
		public const int WSA_QOS_ESDMODEOBJ = 11029;
		public const int WSA_QOS_ESHAPERATEOBJ = 11030;
		public const int WSA_QOS_RESERVED_PETYPE = 11031;
	
		public static string GetErrorCodeName(int retcode)
		{
			Type constType = typeof(WinSockErrorCode);

			FieldInfo[] fields = constType.GetFields();

			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo info = fields[i];
				if (info.FieldType == typeof(int) &&
					retcode.Equals(info.GetValue(null)))
					return info.Name;
			}

			return "WSA_UNKOWN_ERROR";
		}
	}
}
