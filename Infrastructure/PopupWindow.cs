using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AnimationLib;
using AnimationLib.AnimationsCreator;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Infrastructure
{
    public class UIElementWithTappedAction
    {
        public FrameworkElement FrameworkElement { get; set; }
        public Action<UIElement> Action { get; set; }
        public bool CloseOnTap { get; set; }

        public UIElementWithTappedAction(FrameworkElement uiElement, Action<UIElement> action,bool closeOnTap = true)
        {
            FrameworkElement = uiElement;
            Action = action;
            CloseOnTap = closeOnTap;
        }
    }

    public class PopupWindowService
    {
        private readonly List<UIElementWithTappedAction> _closingUIElements;
        private  Grid _contentContainer;// = new PopupWindowContentContainer();
        private  Popup _popup;
        private static readonly TimeSpan _animationDuration =  TimeSpan.FromMilliseconds(500);

        private  Storyboard _opacityShowAnimation;
        private  Storyboard _contentShowAnimation;
        private  Storyboard _opacityHideAnimation;
        private  Storyboard _contentHideAnimation;
        private readonly PhoneApplicationPage _page;
        private bool _hasApplicationBar;

        public static double OuterBackgroundOpacity { get; private set; }
        public FrameworkElement Content { get; private set; }

        private  void PrepareDefaultAnimations()
        {
            _opacityShowAnimation = new SingleAnimationCreator(new UIElementAnimation((el) => { },
                new PropertyAnimation(() =>
                {
                    var colorAnimation1 = new ColorAnimation()
                    {
                        Duration = _animationDuration,
                        To = Color.FromArgb((byte) (255*OuterBackgroundOpacity), 0, 0, 0)
                    };
                    return colorAnimation1;
                }, new PropertyPath("(Control.Background).(SolidColorBrush.Color)")))).Create(_contentContainer);

            _contentShowAnimation = new SingleAnimationCreator(new UIElementAnimation(
                (el) =>
                {
                    el.RenderTransform = new ScaleTransform() {ScaleX = 0, ScaleY = 0};
                    el.RenderTransformOrigin = new Point(0.5, 0.5);
                }, new PropertyAnimation(() =>
                {
                    var doubleAnim1 = new DoubleAnimation()
                    {
                        From = 0.0,
                        Duration = _animationDuration,
                        To = 1.0,
                        EasingFunction = new BackEase() { Amplitude = 0.5}
                    };
                    return doubleAnim1;
                },
                    new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"),
                    new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)")))).Create(Content); ;
            _opacityHideAnimation = new SingleAnimationCreator(new UIElementAnimation((el) => { },
              new PropertyAnimation(() =>
              {
                  var colorAnimation1 = new ColorAnimation()
                  {
                      Duration = _animationDuration,
                      To = Color.FromArgb(0, 0, 0, 0)
                  };
                  return colorAnimation1;
              }, new PropertyPath("(Control.Background).(SolidColorBrush.Color)")))).Create(_contentContainer);

            _contentHideAnimation = new SingleAnimationCreator(new UIElementAnimation(
                (el) =>
                {
                    el.RenderTransform = new ScaleTransform() { ScaleX = 1, ScaleY = 1 };
                    el.RenderTransformOrigin = new Point(0.5, 0.5);
                }, new PropertyAnimation(() =>
                {
                    var doubleAnim1 = new DoubleAnimation()
                    {
                        Duration = _animationDuration,
                        To = 0
                    };
                    return doubleAnim1;
                },
                    new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"),
                    new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)")))).Create(Content); ;
        }

        private void BeginShowAnimations()
        {
            _contentShowAnimation.Begin();
         //   _opacityShowAnimation.Begin();
        }
        private void BeginHideAnimations(EventHandler onCompleted)
        {
//            var sb = _opacityHideAnimation.Create(_contentContainer);
//            sb.Children.Add(_contentHideAnimation.Create(Content));
//            sb.Completed +=onCompleted;
//            sb.Begin();
           // _opacityHideAnimation.Begin();
            //_contentHideAnimation.Begin();
            //_contentHideAnimation.Completed += onCompleted;
            onCompleted(this,EventArgs.Empty);
        }

        public PopupWindowService(PhoneApplicationPage page,FrameworkElement content, params UIElementWithTappedAction[] popupClosingUIElements)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (content == null) throw new ArgumentNullException("content");

            _page = page;
            Content = content;
            _closingUIElements = new List<UIElementWithTappedAction>(popupClosingUIElements);
            OuterBackgroundOpacity = 0.5;
            
            Init();
        }
        ~PopupWindowService()
        {
            Debug.WriteLine("Popup destructor");
        }
      
        private void Init()
        {
            
            _contentContainer = new Grid
            {
                //Opacity = OuterBackgroundOpacity,
                Background = new SolidColorBrush(Color.FromArgb(0x99, 0, 0, 0)),
                Height = Application.Current.Host.Content.ActualHeight,//- new ApplicationBar().DefaultSize,
                Width = Application.Current.Host.Content.ActualWidth,
            };
            _contentContainer.Children.Add(Content);         
            Content.CacheMode = new BitmapCache();
            _popup = new Popup { Child = _contentContainer };

            _page.BackKeyPress += PageOnBackKeyPress;

            _closingUIElements.ForEach(closingUIElements => closingUIElements.FrameworkElement.Tap += (sender, gestureEventArgs) =>
            {
                if (closingUIElements.CloseOnTap)
                    Close();
                closingUIElements.Action(closingUIElements.FrameworkElement);
            });
            PrepareDefaultAnimations();
        }
        private void PageOnBackKeyPress(object sender, CancelEventArgs cancelEventArgs)
        {
            Close();
        }

        public void Close()
        {
            BeginHideAnimations((s, a) => {
                                              _popup.IsOpen = false;                                                                                          
            });
            _page.BackKeyPress -= PageOnBackKeyPress;
            ShowAppBar();     
        }    
        private void ShowAppBar()
        {
            if (_hasApplicationBar)
            {
                _hasApplicationBar = false;

                // Application bar can be nulled during the Dismissed event
                // so a null check needs to be performed here.
                if (_page.ApplicationBar != null)
                {
                    EnableAppBar();                     
                }
            }
        }

        public void Show()
        {
            HideAppBar();
            _popup.IsOpen = true;
            BeginShowAnimations();
        }
        public bool IsShown { get { return _popup.IsOpen; } }
        private void HideAppBar()
        {
    
            if (_page.ApplicationBar != null)
            {
                // Cache the original visibility of the system tray.
                _hasApplicationBar = _page.ApplicationBar.IsVisible;

                // Hide it.
                if (_hasApplicationBar)
                {                 
                    DisableApplicationBar();
                }
            }
            else
            {
                _hasApplicationBar = false;
            }
        }
        private Color _defaultAppBarColor;
        private Color _defaultAppBarForegroundColor;

        private readonly List<bool> _buttonsDefaultEnability = new List<bool>();
        private bool _appBarMenuIsEnabledDefault = false;
        public void DisableApplicationBar()
        {
            _defaultAppBarColor = _page.ApplicationBar.BackgroundColor;
            _defaultAppBarForegroundColor = _page.ApplicationBar.ForegroundColor;

            //_page.ApplicationBar.BackgroundColor = Color.FromArgb(255, 89, 89, 89);
            _page.ApplicationBar.BackgroundColor = ColorManipulation.DarkenColor(_defaultAppBarColor, 0.594f);
            _page.ApplicationBar.ForegroundColor = ColorManipulation.DarkenColor(_defaultAppBarForegroundColor, 0.594f);

            _buttonsDefaultEnability.Clear();
            var applicationBarIconButtons = _page.ApplicationBar.Buttons.OfType<ApplicationBarIconButton>().ToList();
            for (int i = 0; i < applicationBarIconButtons.Count; i++)
            {
                _buttonsDefaultEnability.Add(applicationBarIconButtons[i].IsEnabled);
                applicationBarIconButtons[i].IsEnabled = false;
            }
            _appBarMenuIsEnabledDefault = _page.ApplicationBar.IsMenuEnabled;
            _page.ApplicationBar.IsMenuEnabled = false;
        }
        private void EnableAppBar()
        {
            _page.ApplicationBar.BackgroundColor = _defaultAppBarColor;
            _page.ApplicationBar.ForegroundColor = _defaultAppBarForegroundColor;

            var applicationBarIconButtons = _page.ApplicationBar.Buttons.OfType<ApplicationBarIconButton>().ToList();
            for (int i = 0; i < applicationBarIconButtons.Count; i++)
            {
                applicationBarIconButtons[i].IsEnabled = _buttonsDefaultEnability[i];
            }
            _page.ApplicationBar.IsMenuEnabled = _appBarMenuIsEnabledDefault;

        }

        
    }
}