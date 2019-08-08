using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BRTest
{
    using AFC.WS.UI.Common;
    using AFC.WS.ModelView.Actions.CommonActions;
    
    /// <summary>
    /// UnitTest1 的摘要说明
    /// </summary>
    [TestClass]
    public class CommonActionTest
    {
        public CommonActionTest()
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
        public void TestAction()
        {

            DoublePrimissionAction action = new AFC.WS.ModelView.Actions.CommonActions.DoublePrimissionAction();

            action.subAction = new TestDoublePrimissionAction();

            action.CurrentOperationId = "111111";

            List<QueryCondition> list = new List<QueryCondition>();
            list.Add(new QueryCondition { bindingData = "aa", value = 1, operation = OperationSymbols.Equal });

            if (action.CheckValid(list))
            {
                action.DoAction(list);
            }

        }


       
    }


    public class TestDoublePrimissionAction : IAction,IDoublePrimissionHandler
    {

        #region IAction 成员

        public bool CheckValid(List<QueryCondition> actionParamsList)
        {
            return true;
        }

        public bool CheckPremission(object authInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus DoAction(List<QueryCondition> actionParamsList)
        {
            AFC.WS.UI.CommonControls.MessageDialog.Show("test");
            return new ResultStatus { resultCode = 0, resultData = 0 };
        }

        #endregion

        #region IDoublePrimissionHandler 成员

        public bool HandleDoublePrimission(string operatorId)
        {
            AFC.WS.UI.CommonControls.MessageDialog.Show("handle second operator!!");
            return true;
        }

        #endregion
    }

}
