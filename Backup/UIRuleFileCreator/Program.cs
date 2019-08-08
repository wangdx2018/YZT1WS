using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;

namespace AFC.WS.UI.UIRuleFileCreator
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                string startForm = ConfigurationManager.AppSettings["StartForm"].ToString();
                switch (startForm)
                {
                    case "RuleForm":

                        Application.Run(new RuleForm());

                        break;

                    case "TestForm":

                        Application.Run(new TestForm());
                        break;
                }
            }
            catch { }
        }
    }
}
