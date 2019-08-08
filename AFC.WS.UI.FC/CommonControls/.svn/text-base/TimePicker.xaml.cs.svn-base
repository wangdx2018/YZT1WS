#region [       Copyright (C), 2010,  中软AFC, Token Shen.     ]
/************************************************************
  FileName: TimePicker.xaml
  
  Author: 沈克涛    
 
  Version :  1.0   
 
  Date:20100207
 
  Description: 设置时间   
 
  Function List:  
 
    1. SetStyle  // ---> 设置样式
 
  History: 
 
      <author>   <time>      <version >     <desc>
 
      沈克涛    2010/02/07     1.0         增加代码说明
 * 
 *    /// edit by wangdx 20110627
    /// 
    /// 修改了GetControlValue的Text的取值，变成了十进制2位显示。
 * ***********************************************************/
#endregion

#region [       Using namespaces       ]
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
using System.ComponentModel;
using AFC.WS.UI.Common;
using Microsoft.Windows.Controls;
using System.Windows.Controls.Primitives;
#endregion

namespace AFC.WS.UI.CommonControls
{

    [TemplatePart(Name = TimePicker.ElementHourTextBox, Type = typeof(TextBox))]

    [TemplatePart(Name = TimePicker.ElementMinuteTextBox, Type = typeof(TextBox))]

    [TemplatePart(Name = TimePicker.ElementSecondTextBox, Type = typeof(TextBox))]

    [TemplatePart(Name = TimePicker.ElementIncrementButton, Type = typeof(RepeatButton))]

    [TemplatePart(Name = TimePicker.ElementDecrementButton, Type = typeof(RepeatButton))]

    public partial class TimePicker : UserControl, ICommonEdit
    {

        #region [       Constants       ]

        private const string ElementHourTextBox = "PART_HourTextBox";

        private const string ElementMinuteTextBox = "PART_MinuteTextBox";

        private const string ElementSecondTextBox = "PART_SecondTextBox";

        private const string ElementIncrementButton = "PART_IncrementButton";

        private const string ElementDecrementButton = "PART_DecrementButton";

        private static TimeSpan MinValidTime = new TimeSpan(0, 0, 0);

        private static TimeSpan MaxValidTime = new TimeSpan(23, 59, 59);

        private TextBox selectedTextBox = null;

        #endregion

        #region [       Staic Constructor       ]
        static TimePicker()
        {

            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimePicker), new FrameworkPropertyMetadata(typeof(TimePicker)));

        }
        #endregion

        #region [        Constructor       ]
        public TimePicker()
        {

            SelectedTime = DateTime.Now.TimeOfDay;

        }



        #endregion Ctor

        #region [       Data       ]


        private TextBox hourTextBox;

        private TextBox minuteTextBox;

        private TextBox secondTextBox;

        private RepeatButton incrementButton;

        private RepeatButton decrementButton;

        #endregion Data

        #region [       Public Properties       ]
        /// <summary>
        /// 获得TimePicker值
        /// </summary>
        [
        Description("获得TimePicker控件值。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("TimePicker"),
        Filter()
        ]
        public string Text
        {
            get
            {
                TimeSpan timeSpan = SelectedTime;
                string dataTime = timeSpan.Hours.ToString("d2") + ":" + timeSpan.Minutes.ToString("d2") + ":" + timeSpan.Seconds.ToString("d2");
                return dataTime;
            }
            set
            {
                DateTime dateTime = new DateTime();
                try
                {
                    dateTime = Convert.ToDateTime(value);
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Info("错误的日期格式!" + ex.ToString());
                }
                if (dateTime != null)
                {
                    TimeSpan ts = new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
                    SelectedTime = ts;
                }
            }
        }

        #region SelectedTime
   

        public TimeSpan SelectedTime
        {

            get { return (TimeSpan)GetValue(SelectedTimeProperty); }

            set { SetValue(SelectedTimeProperty, value); }

        }



        public static readonly DependencyProperty SelectedTimeProperty =

            DependencyProperty.Register(

                "SelectedTime",

                typeof(TimeSpan),

                typeof(TimePicker),

                new FrameworkPropertyMetadata(TimePicker.MinValidTime, new PropertyChangedCallback(TimePicker.OnSelectedTimeChanged), new CoerceValueCallback(TimePicker.CoerceSelectedTime)));



        #endregion SelectedTime



        #region MinTime



        public TimeSpan MinTime
        {

            get { return (TimeSpan)GetValue(MinTimeProperty); }

            set { SetValue(MinTimeProperty, value); }

        }



        public static readonly DependencyProperty MinTimeProperty =

            DependencyProperty.Register(

                "MinTime",

                typeof(TimeSpan),

                typeof(TimePicker),

                new FrameworkPropertyMetadata(TimePicker.MinValidTime, new PropertyChangedCallback(TimePicker.OnMinTimeChanged)),

                new ValidateValueCallback(TimePicker.IsValidTime));



        #endregion MinTime



        #region MaxTime



        public TimeSpan MaxTime
        {

            get { return (TimeSpan)GetValue(MaxTimeProperty); }

            set { SetValue(MaxTimeProperty, value); }

        }



        public static readonly DependencyProperty MaxTimeProperty =

            DependencyProperty.Register(

                "MaxTime",

                typeof(TimeSpan),

                typeof(TimePicker),

                new FrameworkPropertyMetadata(TimePicker.MaxValidTime, new PropertyChangedCallback(TimePicker.OnMaxTimeChanged), new CoerceValueCallback(TimePicker.CoerceMaxTime)),

                new ValidateValueCallback(TimePicker.IsValidTime));



        #endregion MaxTime



        #endregion Public Properties

        #region [       SelectedTimeChangedEvent       ]
        public event RoutedPropertyChangedEventHandler<TimeSpan> SelectedTimeChanged
        {

            add { base.AddHandler(SelectedTimeChangedEvent, value); }

            remove { base.RemoveHandler(SelectedTimeChangedEvent, value); }

        }

        public static readonly RoutedEvent SelectedTimeChangedEvent =

             EventManager.RegisterRoutedEvent(

                 "SelectedTimeChanged",

                 RoutingStrategy.Bubble,

                 typeof(RoutedPropertyChangedEventHandler<TimeSpan>),

                 typeof(TimePicker));



        #endregion SelectedTimeChangedEvent

        #region [       Public Methods       ]


        public override void OnApplyTemplate()
        {

            base.OnApplyTemplate();



            hourTextBox = GetTemplateChild(ElementHourTextBox) as TextBox;

            if (hourTextBox != null)
            {

                hourTextBox.IsReadOnly = true;

                hourTextBox.GotFocus += SelectTextBox;
            }



            minuteTextBox = GetTemplateChild(ElementMinuteTextBox) as TextBox;

            if (minuteTextBox != null)
            {

                minuteTextBox.IsReadOnly = true;

                minuteTextBox.GotFocus += SelectTextBox;

            }



            secondTextBox = GetTemplateChild(ElementSecondTextBox) as TextBox;

            if (secondTextBox != null)
            {

                secondTextBox.IsReadOnly = true;

                secondTextBox.GotFocus += SelectTextBox;

            }



            incrementButton = GetTemplateChild(ElementIncrementButton) as RepeatButton;

            if (incrementButton != null)
            {

                incrementButton.Click += IncrementTime;

            }



            decrementButton = GetTemplateChild(ElementDecrementButton) as RepeatButton;

            if (decrementButton != null)
            {

                decrementButton.Click += DecrementTime;

            }

        }


        #endregion Public Methods

        #region [       Private Methods       ]

        protected virtual void OnSelectedTimeChanged(TimeSpan oldSelectedTime, TimeSpan newSelectedTime)
        {

            RoutedPropertyChangedEventArgs<TimeSpan> e = new RoutedPropertyChangedEventArgs<TimeSpan>(oldSelectedTime, newSelectedTime);

            e.RoutedEvent = SelectedTimeChangedEvent;

            base.RaiseEvent(e);

        }

        private static object CoerceSelectedTime(DependencyObject d, object value)
        {

            TimePicker timePicker = (TimePicker)d;

            TimeSpan minimum = timePicker.MinTime;

            if ((TimeSpan)value < minimum)
            {

                return minimum;

            }


            TimeSpan maximum = timePicker.MaxTime;

            if ((TimeSpan)value > maximum)
            {

                return maximum;

            }


            return value;

        }

        private static object CoerceMaxTime(DependencyObject d, object value)
        {

            TimePicker timePicker = (TimePicker)d;

            TimeSpan minimum = timePicker.MinTime;

            if ((TimeSpan)value < minimum)
            {

                return minimum;

            }

            return value;

        }

        private static bool IsValidTime(object value)
        {

            TimeSpan time = (TimeSpan)value;

            return MinValidTime <= time && time <= MaxValidTime;

        }

        private static void OnSelectedTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            TimePicker element = (TimePicker)d;

            element.OnSelectedTimeChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue);

        }

        protected virtual void OnMinTimeChanged(TimeSpan oldMinTime, TimeSpan newMinTime)
        {

        }

        private static void OnMinTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            TimePicker element = (TimePicker)d;

            element.CoerceValue(MaxTimeProperty);

            element.CoerceValue(SelectedTimeProperty);

            element.OnMinTimeChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue);

        }

        protected virtual void OnMaxTimeChanged(TimeSpan oldMaxTime, TimeSpan newMaxTime)
        {

        }

        private static void OnMaxTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            TimePicker element = (TimePicker)d;

            element.CoerceValue(SelectedTimeProperty);

            element.OnMaxTimeChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue);

        }


        private void SelectTextBox(object sender, RoutedEventArgs e)
        {

            selectedTextBox = sender as TextBox;
            // selectedTextBox.MouseLeftButtonDown += new MouseButtonEventHandler(selectedTextBox_MouseLeftButtonDown);

        }


        private void IncrementTime(object sender, RoutedEventArgs e)
        {

            IncrementDecrementTime(1);

        }



        private void DecrementTime(object sender, RoutedEventArgs e)
        {

            IncrementDecrementTime(-1);

        }



        private void IncrementDecrementTime(int step)
        {

            if (selectedTextBox == null)
            {

                selectedTextBox = hourTextBox;

            }



            TimeSpan time;


            if (selectedTextBox == hourTextBox)
            {

                time = SelectedTime.Add(new TimeSpan(step, 0, 0));

            }

            else if (selectedTextBox == minuteTextBox)
            {

                time = SelectedTime.Add(new TimeSpan(0, step, 0));

            }

            else
            {

                time = SelectedTime.Add(new TimeSpan(0, 0, step));

            }



            SelectedTime = time;

        }



        #endregion

        #region [       ICommonEdit 成员       ]
        public void Initialize()
        {
          
        }

        public object GetControlValue()
        {
            return this.Text;
        }

        public void SetControlValue(object value)
        {
            this.Text = value.ToString();
        }

        #endregion
    }

}
