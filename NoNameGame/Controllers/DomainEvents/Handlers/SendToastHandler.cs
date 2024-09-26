using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Coding4Fun.Toolkit.Controls;
using Infrastructure;
using NoNameGame.Configuration;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;
using NoNameGame.Helpers;
using NoNameGame.Resources;

namespace NoNameGame.Controllers.DomainEvents.Handlers
{

    public class SendToastinetHandler : IDomainEventHandler<AchievementUnlocked>
    {
        public void Handle(AchievementUnlocked domainEvent)
        {
            if (domainEvent == null) throw new ArgumentNullException("domainEvent");
            Debug.WriteLine("Achievement unlocked,sending toast");

//            Deployment.Current.Dispatcher.BeginInvoke(() =>
//            {
//                var toast = new Toastinet.Toastinet.Toastinet()
//                {
//
//                };
////                var toast = new ToastPrompt
////                {
////                    Message = domainEvent.Achievement.Name,
////                    TextOrientation = System.Windows.Controls.Orientation.Vertical,
////                    //TextWrapping = TextWrapping.Wrap,
////                    Title = AppResources.SendToastHandler_Title,
////                    FontSize = 23,
////                    Background = new SolidColorBrush(Constants.PhoneChromeColor),
////                    Foreground = new SolidColorBrush(Constants.GameShapeStrokeColor),
////                    ImageSource = new BitmapImage(new Uri(@"Assets/prize_color_48.png", UriKind.Relative)),
////                    //ImageHeight = 40,ImageWidth = 48,
////                    //Stretch = Stretch.Uniform,                  
////
////                };
//                toast.Title = AppResources.SendToastHandler_Title;
//                toast.Message = domainEvent.Achievement.Name;
//            });
        }
    }

    public class SendToastPromptHandler :IDomainEventHandler<AchievementUnlocked>
    {
        public void Handle(AchievementUnlocked domainEvent)
        {            
            if (domainEvent == null) throw new ArgumentNullException("domainEvent");
            Debug.WriteLine("Achievement unlocked,sending toast");

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var toast = new ToastPrompt
                {                    
                    Message = domainEvent.Achievement.Name,
                    TextOrientation = System.Windows.Controls.Orientation.Vertical,
                    //TextWrapping = TextWrapping.Wrap,
                    Title = AppResources.SendToastHandler_Title,   
                    FontSize = 23,
                   // Background = new SolidColorBrush(Constants.PhoneChromeColor),
                   Background =  new SolidColorBrush(GameAccentColorProvider.GetLighter()),
                    Foreground = new SolidColorBrush(Constants.AppBarForegroundColor),
                    ImageSource = new BitmapImage(new Uri(@"Assets/prize_color_48.png", UriKind.Relative)),
                    //ImageHeight = 40,ImageWidth = 48,
                    //Stretch = Stretch.Uniform,                  
                    
                };
                toast.Show();
            });
        }
    }
}