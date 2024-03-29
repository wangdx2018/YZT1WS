﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestComm
{
    using TJComm;
    using AFC.WS.Model.Const;
    using AFC.WS.Model.Comm;
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window1_Loaded);
        }

        //CommConnection con = new CommConnection("192.168.1.145", 7021, 30);

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            CommConfigs config = this.con.LoadCommConfigFile();


            if (config != null)
            {
                bool res = con.SetupCommConfig(config);
                if (res)
                    con.ConnectionStateChanged += new EventHandler(con_ConnectionStateChanged);
            }
        }
        

        void con_ConnectionStateChanged(object sender, EventArgs e)
        {
            CommConnection con = sender as CommConnection;
            if (con.Connected)
                MessageBox.Show("Connect done");
            else
                MessageBox.Show("Socket DisConnet");
        }



        private CommConnection con = new CommConnection("192.168.1.145", 7021,30);

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           int res= con.Connect(3);
           if (res == 0)
               MessageBox.Show("connect successfully");
           else
               MessageBox.Show("failed");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Log_In_Out, 0, 0, CommandType.RESPONSE);

            OperatorLogInOut_1301 inOut = new OperatorLogInOut_1301();
            inOut.loginType = 0;
            inOut.password = "11111111";
            inOut.commBody = new CommBodyData(header);
            inOut.headerData = new CommHeaderData(0, 0, 0);

            TJCommMessage msg = TJCommMessage.CreateTJCommMsg(header, inOut);

            TJCommMessage outMsg = null;

            con.SendMsgAndReceive(msg, out outMsg);

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Log_In_Out, 0, 0, CommandType.RESQUEST);

            OperatorLogInOut_1301 inOut = new OperatorLogInOut_1301();
            inOut.loginType = 0;
            inOut.password = "2";
            inOut.commBody = new CommBodyData(header);
            inOut.headerData = new CommHeaderData(0, 0, 0);

            TJCommMessage msg = TJCommMessage.CreateTJCommMsg(header, inOut);

          //TJCommMessage outMsg = null;

            con.SendMsg(msg);

        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            //CommHeader header = TJCommMessage.CreateHeader(CommMsgType.Unspecified, 11111111, 301, 12345678, 87654321, CommandType.RESQUEST);

            //TestMsgContent data = new TestMsgContent();
            //data.info1.loginType = 0;
            //data.info1.password = "111111";
            //data.info2.unlockedOperatorId = 11111111;
            
            //TJCommMessage msg = TJCommMessage.CreateTJCommMsg(header, data);

            //con.SendMsg(msg);

        }
    }
}
