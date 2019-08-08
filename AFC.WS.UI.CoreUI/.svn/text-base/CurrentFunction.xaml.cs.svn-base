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
using System.Windows.Markup;
using System.Xml;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Animation;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;
using AFC.WS.UI.Common;

#endregion


namespace AFC.WS.UI.CoreUI
{
    using AFC.BOM2.UIController;
    using AFC.BOM2.MessageDispacher;
    using AFC.WS.Model.Const;
    using AFC.WS.ModelView;
    
    

    /// <summary>
    /// CurrentFunction.xaml 的交互逻辑
    /// </summary>
    public partial class CurrentFunction : UserControlBase
    {
        #region [       Declaration       ]
        private StackPanel functionPanel = null;

        /// <summary>
        /// 选中的菜单
        /// </summary>
        private Label selectedLabel=null;
        #endregion

        #region [       Constructor       ]
        /// <summary>
        /// 构造函数
        /// </summary>
        public CurrentFunction()
        {
            InitializeComponent();
            
        }
      
        #endregion

        #region [       Private Methods       ]

        //--->添加第一级菜单
        /// <summary>
        /// 增加主功能按钮
        /// </summary>
        private void AddMainFunctionButton(Dictionary<Function,List<Function>> dict)
        {
            List<Function> list = (from temp in dict
                                   select temp.Key).ToList(); //get main navigaoor

            functionPanel = new StackPanel();
            if (list != null)
            {
                this.stackPanel.Children.Clear();
                foreach (Function funcItem in list)
                {
                    if (!String.IsNullOrEmpty(funcItem.functionId.ToString()))
                    {
                        Expander funcButton = new Expander();
                        funcButton.Expanded += new RoutedEventHandler(funcButton_Click);
                        funcButton.FontSize = 13;
                        funcButton.Width = 210;
                        funcButton.HorizontalAlignment = HorizontalAlignment.Left;
                        funcButton.Name = funcItem.buinessControlId.ToString();
                        funcButton.Header = funcItem.text;
                        funcButton.Tag = dict[funcItem];
                        funcButton.IsEnabled = true;
                        funcButton.TabIndex = (int)(funcItem.functionId);

                        funcButton.Content = AddChildFunction(funcButton);

                        functionPanel.Children.Add(funcButton);
                    }
                }
                this.stackPanel.Children.Add(functionPanel);
            }
            else
            {
                //MessageBox.Show("加载XML文件失败！");
            }
        }
        
        //--->添加第二级菜单
        /// <summary>
        /// 增加子功能。
        /// </summary>
        /// <param name="expander"></param>
        /// <returns></returns>
        private StackPanel AddChildFunction(Expander expander)
        {
            double panelHeight = 0;
            Expander detailFunction = expander;

            StackPanel stackPanel = new StackPanel();

            List<Function> item = detailFunction.Tag as List<Function>;
           
            foreach (Function functionItem in item)
            {
                try
                {
                    Label text = new Label();
                    text.Name = functionItem.buinessControlId;
                    text.Tag = functionItem;
                    text.Content = "     " + functionItem.text;
                    text.Style = SetStyle("LabelStyle");
                    text.Height = 25;
                    text.Width = detailFunction.Width - 10;
                    text.MouseLeftButtonUp += new MouseButtonEventHandler(txtMouseLeftButtonDown);
                    text.HorizontalContentAlignment = HorizontalAlignment.Left;
                    text.FontSize = 13;
                    panelHeight = panelHeight + text.Height;

                    stackPanel.Children.Add(text);
                }
                catch (Exception ex)
                {
                    WriteLog.Log_Error(ex);
                }
              //  stackPanel.Children.Add(new Separator());
            }

            stackPanel.Height = panelHeight;

            return stackPanel;
         
        }
  

        //--->菜单双击事件(发送同步消息给菜单)
        /// <summary>
        /// 双击子功能时出发。
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void txtMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            //--->选中颜色改变
            Label label = sender as Label;
            if (selectedLabel != null)
            {
                selectedLabel.Foreground = System.Windows.Media.Brushes.White as Brush;
            }
            selectedLabel = label;
            label.SetResourceReference(Label.ForegroundProperty, "MenuSelected");

            //-->发送切换界面消息
            Function labelTag = label.Tag as Function;
            if (labelTag != null)
            {
                Message msgSwitchPage = new Message();
                msgSwitchPage.MessageType = SynMessageType.SwichPage;
                msgSwitchPage.Content = labelTag.buinessControlId;
                if (!string.IsNullOrEmpty(labelTag.paramsData))
                {
                    msgSwitchPage.MessageParam = labelTag.paramsData;
                }
                msgSwitchPage.MessageSource = labelTag.functionId.ToString();//
                MessageManager.SendMessasge(msgSwitchPage);

                //--->发送菜单导航消息
                Message msg = new Message();
                msg.MessageType = SynMessageType.NavigationSelection;
                msg.Content = labelTag.functionId;
                MessageManager.SendMessasge(msg);
            }
        }

        //-->主菜单的单击（展开）
        /// <summary>
        /// 点击主功能按钮后加载子功能列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void funcButton_Click(object sender, RoutedEventArgs e)
        {
            Expander ex = sender as Expander;

            foreach (Expander expander in functionPanel.Children)
            {
                if (expander.Name != ex.Name)
                {
                    if (expander.IsExpanded == true)
                    {
                        expander.IsExpanded = false;
                    }
                }
            }
        }

        //--->设置样式
        /// <summary>
        /// 设置样式
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>样式</returns>
        private Style SetStyle(string key)
        {
            try
            {
                object obj = this.FindResource(key);
                return obj != null ? obj as Style : null;
            }
            catch
            {
                return null;
            }
        }

        //--->界面加载显示界面信息//todo:进行权限的认证，计算结果，得到菜单
        /// <summary>
        /// 控件加载事件
        ///  //todo:进行权限的认证，计算结果，得到菜单
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           //todo:进行权限的认证，计算结果，得到菜单
            
            ((Storyboard)this.Resources["storyBoard"]).Begin(this);
        }

        #endregion


        /// <summary>
        /// 订阅消息，订阅登录之后能够根据操作员编码生成菜单列表
        /// </summary>
        public override void SubscribeSynMessage()
        {
            MessageManager.SubscribeMessage(this,SynMessageSubscribeId.Naviagtor, SynMessageType.CreateNavigatorList, HandleMode.Syn, false);
            MessageManager.SubscribeMessage(this, SynMessageSubscribeId.Naviagtor, SynMessageType.SwichPage, HandleMode.Syn, false);
        }

        /// <summary>
        /// 处理同步消息,根据功能的编号生成菜单选项
        /// </summary>
        /// <param name="msg"></param>
        public override void HandleSynMessage(Message msg)
        {
            if (SynMessageType.CreateNavigatorList == msg.MessageType)
            {
                Dictionary<Function,List<Function>> dict=FunctionDataCollection.GetFunctionDataCollection().GetParentFunctionCollection(msg.Content as List<string>);
                AddMainFunctionButton(dict);
            }
            if (msg.MessageType == SynMessageType.SwichPage)
            {
                var item = msg.MessageParam as Function;
                if (item == null)
                {
                    return;
                }
                var parentFunctionInfo =
                    FunctionDataCollection.GetFunctionDataCollection().GetFunctionById(item.parentId);

                if (item.parentId != 0 && item.functionId != 0)
                {
                    foreach (Expander expander in functionPanel.Children)
                    {
                        if (expander.Name.Equals(parentFunctionInfo.buinessControlId))
                        {
                            //展开导航子菜单
                            expander.IsExpanded = true;
                            DependencyObject dependencyObject = LogicalTreeHelper.FindLogicalNode(expander,
                                                                                                  item.buinessControlId);
                            if (dependencyObject != null)
                            {

                                var label = dependencyObject as Label;
                                if (label != null)
                                {
                                    if (selectedLabel != null)
                                    {
                                        selectedLabel.Foreground = Brushes.White as Brush;
                                    }
                                    selectedLabel = label;
                                    label.SetResourceReference(ForegroundProperty, "MenuSelected");
                                }
                            }
                        }

                    }

                }

            }

        }
    }

    public class AnimationDecorator : Decorator
    {
        static AnimationDecorator()
        {
            //Sets the default value of the 'ClipToBounds' dependency property to 'true'
            ClipToBoundsProperty.OverrideMetadata(
                typeof(AnimationDecorator), new FrameworkPropertyMetadata(true));

            //Sets the default value of the 'Opacity' dependency property to '0.0'
            OpacityProperty.OverrideMetadata(
                typeof(AnimationDecorator), new FrameworkPropertyMetadata(0.0));

            //Sets the default value of the 'Focusable' dependency property to 'false'
            FocusableProperty.OverrideMetadata(
                typeof(AnimationDecorator), new FrameworkPropertyMetadata(false));
        }

        public AnimationDecorator()
            : base()
        {
            ClipToBounds = true;
        }



        public bool OpacityAnimation
        {
            get { return (bool)GetValue(OpacityAnimationProperty); }
            set { SetValue(OpacityAnimationProperty, value); }
        }

        public static readonly DependencyProperty OpacityAnimationProperty =
            DependencyProperty.Register("OpacityAnimation",
            typeof(bool),
            typeof(AnimationDecorator),
            new UIPropertyMetadata(true));



        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register("IsExpanded",
            typeof(bool),
            typeof(AnimationDecorator),
            new PropertyMetadata(true, IsExpandedChanged));

        public Double ExpandOrCollapseDuration
        {
            get { return (Double)GetValue(AnimationDecorator.ExpandOrCollapseDurationProperty); }
            set { SetValue(AnimationDecorator.ExpandOrCollapseDurationProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ExpandOrCollapseDuration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpandOrCollapseDurationProperty =
            DependencyProperty.Register("ExpandOrCollapseDuration",
            typeof(Double), typeof(AnimationDecorator),
            new FrameworkPropertyMetadata(1000.0));
        private Storyboard m_stBoard = null;

        public static void IsExpandedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Gets the ExpandableDecorator instance who sent the event
            AnimationDecorator expDecorator = (AnimationDecorator)sender;

            //Gets the new value of the 'IsExpanded' DP
            bool IsExpandedNewValue = (bool)e.NewValue;

            //Creates a new storyboard or stops it if it already exists
            if (expDecorator.m_stBoard == null)
                expDecorator.m_stBoard = new Storyboard();
            else
                expDecorator.m_stBoard.Stop(expDecorator);

            //Creates a new animation for a double value
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = (IsExpandedNewValue) ? 1.0 : 0.0;
            animation.Duration =
                (expDecorator.IsLoaded) ?
                TimeSpan.FromMilliseconds(expDecorator.ExpandOrCollapseDuration) :
                TimeSpan.FromMilliseconds(0.0);
            animation.AccelerationRatio = (IsExpandedNewValue) ? 0.0 : 0.33;
            animation.DecelerationRatio = (IsExpandedNewValue) ? 0.33 : 0.0;
            animation.FillBehavior = FillBehavior.HoldEnd;

            //Links it to the 'AnimationProgress' dependency property
            Storyboard.SetTargetProperty(animation, new PropertyPath(AnimationProgressProperty));

            //Clears previous animations and adds the new one to the storyboard
            expDecorator.m_stBoard.Children.Clear();
            expDecorator.m_stBoard.Children.Add(animation);

            //Starts the storyboard and sets it as controllable
            expDecorator.m_stBoard.Begin(expDecorator, true);
        }

        #region [AnimationProgress property]

        //===========================================================================
        /// <summary>
        /// Gets or sets a value indicating the progression of the animation (the 
        /// value goes from 0 to 1). This is a dependency property.
        /// </summary>
        /// <remarks>
        /// The default value is 0 (border collapsed).
        /// </remarks>
        //===========================================================================

        public double AnimationProgress
        {
            get { return (double)GetValue(AnimationDecorator.AnimationProgressProperty); }
            set { SetValue(AnimationDecorator.AnimationProgressProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="AnimationProgress"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimationProgressProperty =
            DependencyProperty.Register("AnimationProgress",
            typeof(double), typeof(AnimationDecorator),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure,
                new PropertyChangedCallback(OnAnimationProgressChanged),
                new CoerceValueCallback(CoerceAnimationProgress)));

        /// <summary>
        /// Invoked whenever the <c>AnimationProgress</c> dependency property value
        /// has been updated.
        /// </summary>
        /// <param name="sender">The <c>DependencyObject</c> on which the property
        /// has changed value.</param>
        /// <param name="e">Event data that is issued by any event that tracks changes 
        /// to the effective value of this property. </param>
        private static void OnAnimationProgressChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //Gets the ExpandableDecorator instance who sent the event
            AnimationDecorator expDecorator = (AnimationDecorator)sender;

            //Updates the instance Visibility
            expDecorator.Visibility =
                (expDecorator.AnimationProgress > 0.0) ? Visibility.Visible : Visibility.Hidden;

            //Updates the instance Opacity
            expDecorator.Opacity = expDecorator.AnimationProgress;
        }

        /// <summary>
        /// Invoked whenever the <c>AnimationProgress</c> dependency property value is 
        /// being re-evaluated, or coercion is specifically requested.
        /// </summary>
        /// <param name="sender">Not used here.</param>
        /// <param name="value">The new value of the property, prior to any coercion 
        /// attempt.</param>
        /// <returns>The coerced value.</returns>
        private static object CoerceAnimationProgress(DependencyObject sender, object value)
        {
            //Gets the value to coerce
            double animationProgress = (double)value;

            //Keeps the value between 0 and 1
            if (animationProgress < 0.0)
                animationProgress = 0.0;
            else if (animationProgress > 1.0)
                animationProgress = 1.0;

            return animationProgress;
        }

        #endregion

        public DoubleAnimation HeightAnimation
        {
            get { return (DoubleAnimation)GetValue(HeightAnimationProperty); }
            set { SetValue(HeightAnimationProperty, value); }
        }

        public static readonly DependencyProperty HeightAnimationProperty =
            DependencyProperty.Register("HeightAnimation",
            typeof(DoubleAnimation),
            typeof(AnimationDecorator),
            new UIPropertyMetadata(null));




        public Duration Duration
        {
            get { return (Duration)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Duration.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(Duration), typeof(AnimationDecorator), new UIPropertyMetadata(new Duration(new TimeSpan(0, 0, 0, 6000))));





        public Double YOffset
        {
            get { return (Double)GetValue(YOffsetProperty); }
            set { SetValue(YOffsetProperty, value); }
        }

        public static readonly DependencyProperty YOffsetProperty =
            DependencyProperty.Register("YOffset", typeof(Double), typeof(AnimationDecorator),
            new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsArrange
                | FrameworkPropertyMetadataOptions.AffectsMeasure));

        protected override Size MeasureOverride(Size availableSize)
        {
            //Gets the single child of the Border
            UIElement singleChild = Child;

            //Checks if it exists
            if (singleChild != null)
            {
                //Asks the child how big it wants to be within the available area
                singleChild.Measure(availableSize);

                //Evaluates the height of the element according to child item desired 
                //height and the animation progress
                double height = singleChild.DesiredSize.Height * AnimationProgress;

                //Returns the original width associated with the calculated height
                return new Size(availableSize.Width, height);
            }
            return new Size();
        }


        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //Gets the single child of the Border
            UIElement singleChild = Child;

            //Checks if it exists
            if (singleChild != null)
            {
                //Calculates the y-coordinate of the top left corner of the child
                double y = singleChild.DesiredSize.Height * (AnimationProgress - 1.0);

                //Tells the single child its location and size
                singleChild.Arrange(new Rect(new Point(0.0, y),
                    new Size(arrangeSize.Width, singleChild.DesiredSize.Height)));

                //Returns the original size
                return arrangeSize;
            }
            return new Size();
        }
    }
}
