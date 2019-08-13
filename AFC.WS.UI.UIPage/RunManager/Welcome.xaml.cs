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

namespace AFC.WS.UI.UIPage.RunManager
{
    using AFC.BOM2.UIController;
    using System.Windows.Media.Animation;
    using AFC.WS.BR;
    /// <summary>
    /// 作者：王冬欣 
    /// 日期：20110224
    /// 代码功能：登录后进入的欢迎界面。
    /// 修订记录：
    /// </summary>
    public partial class Welcome : UserControlBase
    {
        private Storyboard perChar = new Storyboard();

        public Welcome()
        {
            InitializeComponent();
        }

        public override void InitControls()
        {
            StartStoryBoard();
            this.dpOperation.ItemsSource = null;//BuinessRule.GetInstace().rm.GetDoublePrimissionOperation().DefaultView;
        }

        public override void UnLoadControls()
        {
            EndStoryBoard();
            //base.UnLoadControls();
        }

        private void StartStoryBoard()
        {
           
            _text.TextEffects = new TextEffectCollection();
            for (int i = 0; i < _text.Text.Length; i++)
            {
                TextEffect effect = new TextEffect();
                effect.Transform = new TranslateTransform();
                effect.PositionStart = i;
                effect.PositionCount = 1;
                _text.TextEffects.Add(effect);
                DoubleAnimation anim = new DoubleAnimation();
                anim.To = 5;
                anim.AccelerationRatio = .5;
                anim.DecelerationRatio = .5;
                anim.RepeatBehavior = RepeatBehavior.Forever;
                anim.AutoReverse = true;
                anim.Duration = TimeSpan.FromSeconds(1);
                anim.BeginTime = TimeSpan.FromMilliseconds(250 * i);
                Storyboard.SetTargetProperty(anim,
                new PropertyPath("TextEffects[" + i + "].Transform.Y"));
                Storyboard.SetTargetName(anim, _text.Name);
                perChar.Children.Add(anim);
            }
            perChar.Begin(this);
        }

        private void EndStoryBoard()
        {
            perChar.Stop();
        }

        
    }
}
