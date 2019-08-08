using System;
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
using System.Windows.Shapes;

namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// ShowDetailsDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ShowDetailsDialog : Window
    {
        public ShowDetailsDialog()
        {
            InitializeComponent();
           
        }

        public static void ShowDetails(string text, UIElement control)
        {
            if (string.IsNullOrEmpty(text) || control == null)
                return;
            ShowDetailsDialog sd = new ShowDetailsDialog();
            sd.Title = text;
            sd.rootLayout.Children.Add(control);
            sd.ShowDialog();
        }

        public static void ShowDetails(string text, UIElement control, double height, double width)
        {
            if (string.IsNullOrEmpty(text) || control == null)
                return;
            ShowDetailsDialog sd = new ShowDetailsDialog();
           // sd.WindowStyle = WindowStyle.ToolWindow;
            sd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
            sd.Title = text;
            sd.Width = width;
            sd.Height = height;
            sd.MaxHeight = height;
            sd.MaxWidth = width;
            sd.rootLayout.Children.Add(control);
            sd.ShowDialog();

        }
    }
}
