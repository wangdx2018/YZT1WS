using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace BRTest.TestCase
{
    /// <summary>
    /// MD5Test 的摘要说明
    /// </summary>
    [TestClass]
    public class MD5Test
    {
        public MD5Test()
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
        public void MD5TestD()
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();

            byte[] buffer = new byte[28];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)i;
            }
         byte[] md5 =provider.ComputeHash(buffer, 0, buffer.Length);

         StringBuilder sb = new StringBuilder();

         for (int i = 0; i < md5.Length; i++)
         {
             sb.Append(md5[i].ToString("x2"));
             sb.Append("\t");
         }
         Console.WriteLine(sb.ToString());
         Assert.AreEqual(md5.Length, 16);
        }
    }
}
