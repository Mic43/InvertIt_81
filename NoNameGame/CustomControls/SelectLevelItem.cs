using System;
using System.Windows;
using System.Windows.Controls;
using NoNameGame.CustomControls.AttachedProperties;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls
{
    public class SelectLevelItem :Control
    {
        public static readonly DependencyProperty IsAvailableProperty = DependencyProperty.Register(
            "IsAvailable", typeof (bool), typeof (SelectLevelItem), new PropertyMetadata(default(bool),PropertyChangedCallback));
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((SelectLevelItem) dependencyObject).UpdateStates();
        }

        public bool IsAvailable
        {
            get { return (bool) GetValue(IsAvailableProperty); }
            set { SetValue(IsAvailableProperty, value); }
        }

        public static readonly DependencyProperty StarsCountProperty = DependencyProperty.Register(
            "StarsCount", typeof(int), typeof(SelectLevelItem), new PropertyMetadata(default(int), PropertyChangedCallback));

        public int StarsCount
        {
            get { return (int) GetValue(StarsCountProperty); }
            set { SetValue(StarsCountProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof (string), typeof (SelectLevelItem), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public SelectLevelItem()
        {
            Themer.EnableThemesForControls(this);           

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateStates();
        }

        private void UpdateStates()
        {
            if(IsAvailable)
                VisualStateManager.GoToState(this, "LevelAvailable", false);
            else
                VisualStateManager.GoToState(this, "LevelUnAvailable", false);

            if(StarsCount == 0 && IsAvailable)
                VisualStateManager.GoToState(this, "BorderVisible", false);
            else
                VisualStateManager.GoToState(this, "NoBorder", false);
        }
    }
    
}