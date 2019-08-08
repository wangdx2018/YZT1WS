using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BRTest.TestCase
{
    using AFC.BJComm.Data;
    using TJComm;
    using System.IO;
    /// <summary>
    /// PackTest 的摘要说明
    /// </summary>
    [TestClass]
    public class PackTest
    {
        public PackTest()
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

        [TestMethod]
        public void PackCommTest()
        {
           // TJCommMessage msg = TJCommMessage.CreateTJMsg(0x1001, CommandType.RESQUEST, new PackTestData());
            TJCommMessage msg = null;
            byte[] buffer = TJCommMessage.PackTJMsg(msg);

            Assert.AreEqual(buffer.Length, 16 + 28 + 12);
        }

        [TestMethod()]
        public void UnPackCommTest()
        {
           // TJCommMessage msg = TJCommMessage.CreateTJMsg(0x1001, CommandType.RESQUEST, new PackTestData());
            TJCommMessage msg = null;
            byte[] buffer = TJCommMessage.PackTJMsg(msg);

            IMutableInstance data = new MyInstance();
            TJCommMessage msg1 = TJCommMessage.UnPackData(buffer, data);
            Assert.AreNotEqual(null, msg1);

        }

    }


    public class MyInstance : IMutableInstance
    {

        #region IMutableInstance 成员

        public object JudgePackedInstance(object parent, System.IO.MemoryStream s)
        {
            ushort msgType = (parent as TJCommMessage).header.messageType;

           if (msgType == 0x1001)
               return new PackTestData();
           return null;
        }

        #endregion
    }

    

    public class PackTestData
    {
        [PackOrder(1),PackInt(4,ByteOrder.Moto)]
        public int a;

        [PackOrder(2), PackInt(4, ByteOrder.Moto)]
        public int b;

        [PackOrder(3), PackInt(4, ByteOrder.Moto)]
        public int c;
        
        public PackTestData()
        {
            a = 10;
            b = 20;
            c = 12;
        }
    }
}
