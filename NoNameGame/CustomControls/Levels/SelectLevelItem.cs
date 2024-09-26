using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls.Levels
{
    public class SelectLevelItem : ButtonBase
    {
        public static readonly DependencyProperty IsAvailableProperty = DependencyProperty.Register(
            "IsAvailable", typeof (bool), typeof (SelectLevelItem),
            new PropertyMetadata(default(bool), PropertyChangedCallback));
        private static void PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((SelectLevelItem) dependencyObject).UpdateStates();
        }

        public static readonly DependencyProperty PlayAnimationProperty = DependencyProperty.Register(
            "PlayAnimation", typeof(bool), typeof(SelectLevelItem), new PropertyMetadata(default(bool), PropertyChangedCallback));

        public bool PlayAnimation
        {
            get { return (bool) GetValue(PlayAnimationProperty); }
            set { SetValue(PlayAnimationProperty, value); }
        }
        public bool IsAvailable
        {
            get { return (bool) GetValue(IsAvailableProperty); }
            set { SetValue(IsAvailableProperty, value); }
        }

        public static readonly DependencyProperty StarsCountProperty = DependencyProperty.Register(
            "StarsCount", typeof (int), typeof (SelectLevelItem),
            new PropertyMetadata(default(int), PropertyChangedCallback));

        public int StarsCount
        {
            get { return (int) GetValue(StarsCountProperty); }
            set { SetValue(StarsCountProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof (string), typeof (SelectLevelItem), new PropertyMetadata(default(string)));
        private SingleAnimationCreator _fadeToViewAnimation;
        private Storyboard _storyboard;
        private VisualState animationState;

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public SelectLevelItem()
        {
            Themer.EnableThemesForControls(this);
            _fadeToViewAnimation = new SingleAnimationCreator(AnimationsRepository.CreateFadeToViewAnimation(1, TimeSpan.FromMilliseconds(400)));
            Loaded += SelectLevelItem_Loaded;   
            Unloaded+=OnUnloaded;                   
        }
        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_storyboard.GetCurrentState() != ClockState.Stopped)
                _storyboard.Stop();
        }

        private void SelectLevelItem_Loaded(object sender, RoutedEventArgs e)
        {
            //animationState = ((VisualState)VisualStateManager.GetVisualStateGroups(((FrameworkElement) VisualTreeHelper.GetChild(this, 0))).OfType<VisualStateGroup>()
               //.Single(x => x.Name == "AnimationStates").States[0]);

            _storyboard = _fadeToViewAnimation.Create(this);
            _storyboard.Begin();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateStates();
        }
        public void PauseAnimation()
        {
            //animationState.Storyboard.Pause();
        }
        public void ResumeAnimation()
        {
            //animationState.Storyboard.Resume();
        }

        private void UpdateStates()
        {
            if (IsAvailable)
                VisualStateManager.GoToState(this, "LevelAvailable", false);
            else
                VisualStateManager.GoToState(this, "LevelUnAvailable", false);

            if (PlayAnimation)
                VisualStateManager.GoToState(this, "PlayAnimation", false);
            else
                VisualStateManager.GoToState(this, "NoAnimation", false);
        }
    }
}