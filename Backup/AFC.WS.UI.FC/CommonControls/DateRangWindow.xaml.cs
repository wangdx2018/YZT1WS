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
    /// DateRangWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DateRangWindow:Window
    {

        public double pointX;
        public double pointY;
        public DateRangWindow()
        {
            InitializeComponent();

        }

        private void myWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (sender as Window).DragMove();
            this.Top = pointY;
            this.Left = pointX;
        }

        private void allDay_Click(object sender, RoutedEventArgs e)
        {
            DateRangSelect.value = this.LabelAllDay.Content.ToString();
            Close();
        }

        private void oneDay_Click(object sender, RoutedEventArgs e)
        {
            DateRangSelect.value = this.LabelOneDay.Content.ToString();
            Close();
        }

        private void weekDay_Click(object sender, RoutedEventArgs e)
        {
            DateRangSelect.value = this.LabelWeekDay.Content.ToString();
            Close();
        }

        private void mothDay_Click(object sender, RoutedEventArgs e)
        {
            DateRangSelect.value = this.LabelMothDay.Content.ToString();
            Close();
        }

        private void yearDay_Click(object sender, RoutedEventArgs e)
        {

            DateRangSelect.value = this.LabelYearDay.Content.ToString();
            Close();
        }

    }

    public class DateRangSelect
    {
        public static string value;
    }
}
