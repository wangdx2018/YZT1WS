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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AFC.WS.UI.CommonControls
{
    /// <summary>
    /// NumericUpDownExtend.xaml 的交互逻辑
    /// </summary>
    public partial class NumericUpDownExtend : UserControl
    {
        private const decimal MinValue = 0, MaxValue = 2147483647;
        /// <summary>
        /// 增加或减少触发此委托
        /// </summary>
        /// <param name="result"></param>
        public delegate void  NumericUpDownSelectDelegate(ref int result);

        /// <summary>
        /// 增加或减少触发此事件
        /// </summary>
        public  event NumericUpDownSelectDelegate OnSelectEvent;

        /// <summary>
        /// 
        /// </summary>
        public NumericUpDownExtend()
        {
            InitializeComponent();
            UpdateTextBlock();
        }

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(decimal), typeof(NumericUpDownExtend),
            new FrameworkPropertyMetadata(MinValue, new PropertyChangedCallback(OnValueChanged)));
        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDownExtend control = obj as NumericUpDownExtend;
            control.UpdateTextBlock();

            RoutedPropertyChangedEventArgs<decimal> e = new RoutedPropertyChangedEventArgs<decimal>(
                (decimal)args.OldValue, (decimal)args.NewValue, ValueChangedEvent);
            control.OnValueChanged(e);
        }

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventArgs<decimal>),
            typeof(NumericUpDownExtend));

        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        private void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> e)
        {
            RaiseEvent(e);
        }

        private void UpdateTextBlock()
        {
            valueText.Text = Value.ToString();
        }

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            int result = 1;
            if (OnSelectEvent != null)
                OnSelectEvent(ref result);
            if (result == 1)
            {
                if (Value < MaxValue)
                {
                    Value++;
                    UpdateTextBlock();
                }
            }
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            //int result = 1;
            //if (OnSelectEvent != null)
            //    OnSelectEvent(ref result);
            //if (result == 1)
            //{
                if (Value > MinValue)
                {
                    Value--;
                    UpdateTextBlock();
                }
            //}
        }
    }
}
