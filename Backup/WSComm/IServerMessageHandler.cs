using System;
using System.Collections.Generic;
using System.Text;

namespace TJComm
{
	/// <summary>
	/// 报文处理接口
	/// </summary>
	public interface IServerMessageHandler
	{
		/// <summary>
		///  处理接受到的报文，返回响应报文。
		/// </summary>
		/// <param name="requestMessage">要处理的报文</param>
		/// <param name="retcode">操作结果</param>
		/// <returns>响应报文</returns>
		byte[] HandleServerMessage (byte[] requestMessage, out int retcode);

		/// <summary>
		///  处理报文错误，通常会断开连接，并终止该线程。
		/// </summary>
		/// <param name="errorCode">错误代码0:没有错误，1，2，3....代表错误</param>
		/// <param name="retcode">操作结果</param>
		/// <returns></returns>
		byte[] HandleServerError (int errorCode, out int retcode);
	}
}