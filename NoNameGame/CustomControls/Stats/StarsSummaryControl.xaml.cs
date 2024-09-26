using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;

namespace NoNameGame.CustomControls.Stats
{
    public partial class StarsSummaryControl : UserControl
    {
        public static readonly DependencyProperty StarsCountProperty = DependencyProperty.Register(
            "StarsCount", typeof (int), typeof (StarsSummaryControl), new PropertyMetadata(default(int)));
        private readonly SingleAnimationCreator _singleAnimationCreator;
        private Storyboard _storyboard;

        public int StarsCount
        {
            get { return (int) GetValue(StarsCountProperty); }
            set { SetValue(StarsCountProperty, value); }
        }

        public static readonly DependencyProperty PlayAnimationProperty = DependencyProperty.Register(
            "PlayAnimation", typeof (bool), typeof (StarsSummaryControl), new PropertyMetadata(default(bool)));

        public bool PlayAnimation
        {
            get { return (bool) GetValue(PlayAnimationProperty); }
            set { SetValue(PlayAnimationProperty, value); }
        }
        public StarsSummaryControl()
        {
            InitializeComponent();
            Loaded += StarsSummaryControl_Loaded;
            Unloaded += StarsSummaryControl_Unloaded;
            _singleAnimationCreator = new SingleAnimationCreator(
                AnimationBuilder.Scale()
                    .Uniform()
                    .WithEasingFunction(new QuadraticEase())
                    .To(1.15)
                    .AutoReverse()
                    .RepeatForever()
                    .WithBeginTime(TimeSpan.FromMilliseconds(0))
                    .WithDuration(500)
                    .Build());
        }

        private void StarsSummaryControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (PlayAnimation && _storyboard !=null)
            {
                _storyboard.Stop();
            }
        }

        private void StarsSummaryControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (PlayAnimation)
            {
                _storyboard = _singleAnimationCreator.Create(StarsControl);
                _storyboard.Begin();
            }
        }
    }
}