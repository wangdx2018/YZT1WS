using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BRTest.TestCase
{
    using TJComm;
    /// <summary>
    /// CommTest 的摘要说明
    /// </summary>
    [TestClass]
    public class CommTest
    {
        public CommTest()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试属性
        //
        // 编写测试时，还可使用以下附加属性:
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        private CommConnection connect = new CommConnection("192.168.1.145", 7021,2000);

        [TestMethod]
        public void TestConnect()
        {
            
            CommConfigs config = new CommConfigs();
            config.isSendHeartBeat = true;
            config.heartBeatMsgSendInterval = 10;
            config.unPackMsgHandleType = " BRTest.TestCase.UnPackHandle,BRTest";
            config.handleMsgType = "BRTest.TestCase.AsynMessageHandle,BRTest";
            //connect.SetupCommConfig(config, 1000, 2000);           
            int res = connect.Connect(3);
            Assert.AreEqual(res, 0);
            //
            // TODO: 在此	添加测试逻辑
            //
        }

        [TestMethod]
        public void TestSendMsg()
        {
            TestConnect();
            //TJCommMessage msg = TJCommMessage.CreateTJMsg(TJCommMessage.BeatHeartMsgType, CommandType.RESQUEST, new object());
            TJCommMessage msg = null;
            int res=connect.SendMsg(msg);
            Assert.AreEqual(0, res);
        }
    }

    public class AsynMessageHandle : IServerMessageHandler
    {
        #region IServerMessageHandler 成员

        public byte[] HandleServerMessage(byte[] requestMessage, out int retcode)
        {
            retcode = 0;
            return null;
           // throw new NotImplementedException();
        }

        public byte[] HandleServerError(int errorCode, out int retcode)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class UnPackHandle : AFC.BJComm.Data.IMutableInstance
    {
        #region IMutableInstance 成员

        public object JudgePackedInstance(object parent, System.IO.MemoryStream s)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
