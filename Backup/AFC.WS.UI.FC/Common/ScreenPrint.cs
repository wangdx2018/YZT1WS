using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

namespace AFC.WS.UI.Common
{

    using AFC.WS.UI.CommonControls;
        /// <summary>
        /// 屏幕打印类。
        /// </summary>
        public class ScreenPrint
        {
            static ScreenPrint _Instance;
            /// <summary>
            /// 创建屏幕打印
            /// </summary>
            public static ScreenPrint Instance
            {
                get
                {
                    if (null == _Instance)
                    {
                        _Instance = new ScreenPrint();
                    }
                    return _Instance;
                }
            }
            /// <summary>
            /// 截屏所存放图片。
            /// </summary>
            private Image myImage;
            /// <summary>
            /// 打印预览窗体
            /// </summary>
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            /// <summary>
            /// 文档打印控件。
            /// </summary>
            PrintDocument pd = new PrintDocument();
            /// <summary>
            /// 开始截屏
            /// </summary>
            public void BeginScreenshots()
            {
                if (PrinterSettings.InstalledPrinters.Count <= 0)
                {
                    MessageDialog.Show("没有安装打印机","提示",MessageBoxIcon.Error,MessageBoxButtons.Ok);
                    return;
                }
                else
                {
                    try
                    {
                        //获得当前屏幕的分辨率
                        Screen scr = Screen.PrimaryScreen;
                        Rectangle rc = scr.Bounds;
                        int iWidth = rc.Width;
                        int iHeight = rc.Height;
                        //创建一个和屏幕一样大的Bitmap
                        myImage = new Bitmap(iWidth, iHeight);
                        //从一个继承自Image类的对象中创建Graphics对象
                        Graphics g = Graphics.FromImage(myImage);
                        //抓屏并拷贝到myimage里
                        g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(iWidth, iHeight));
                        PrintDocument printDoc = new PrintDocument();
                        printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
                        printDoc.DefaultPageSettings.Landscape = true;
                        ppd.Document = printDoc;
                        ppd.ShowIcon = false;
                        ppd.ShowDialog();
                    }
                    catch (Exception ee)
                    {
                        ppd.Close();
                        MessageDialog.Show(ee.Message,"提示",MessageBoxIcon.Error,MessageBoxButtons.Ok);
                       
                    }
                }//End if;
            }

            void printDoc_PrintPage(object sender, PrintPageEventArgs e)
            {
                try
                {
                    //打印纸的高度。
                    int height = pd.PrinterSettings.DefaultPageSettings.PaperSize.Height;
                    //打印纸的宽度。
                    int width = pd.PrinterSettings.DefaultPageSettings.PaperSize.Width;
                    //图片大小。
                    e.Graphics.DrawImage(myImage, 0, 0, height, width);
                }
                catch (Exception ee)
                {
                    MessageDialog.Show(ee.Message, "提示", MessageBoxIcon.Information, MessageBoxButtons.Ok);
                   // Wrapper.Instance.ConsoleWriteLine(ee, LogFlag.ErrorFormat);
                    return;
                }//End try;
            }
        }
    
}
