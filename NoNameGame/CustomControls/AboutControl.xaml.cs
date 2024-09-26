using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using NoNameGame.Configuration;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Helpers;
using NoNameGame.Resources;

namespace NoNameGame.CustomControls
{
    public partial class AboutControl : UserControl
    {
        private Storyboard _storyboard;
        public AboutControl()
        {
            InitializeComponent();
            Themer.EnableThemesForControls(FeedbackButton,LikeButton,CreditsButton);
            _storyboard = new SingleAnimationCreator(AnimationBuilder.Scale().Uniform().WithEasingFunction(new QuadraticEase()).
                                                                      To(1.2).AutoReverse()
                                                                     .RepeatForever().WithDuration(500).Build()).Create(LikefbImage);
            Loaded+=OnLoaded;
            Unloaded+=OnUnloaded;
        }
        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _storyboard.Stop();
        }
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _storyboard.Begin();
        }
        public event EventHandler CreditsPageRequested;

        private void ShareTwitButton_OnClick(object sender, RoutedEventArgs e)
        {
            var task = new ShareLinkTask()
            {
                Title = string.Format(AppResources.AboutPage_ShareTaskTitle, Constants.GameName),
                LinkUri = Windows.ApplicationModel.Store.CurrentApp.LinkUri,
                Message = ""
            };
            task.Show();
        }
        private void RateButton_OnClick(object sender, RoutedEventArgs e)
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
            this.CurrentApp().EventsBus.Publish(new AppRated());

        }
        private void FeedbackButton_OnClick(object sender, RoutedEventArgs e)
        {
            var emailComposeTask = new EmailComposeTask()
            {
                Subject = AppResources.AboutPage_SendFeedback_EmailTitle,
                To = Constants.FeedbackEmail
            };
            emailComposeTask.Show();
        }

        private void MailShareButton_OnClick(object sender, RoutedEventArgs e)
        {
            var emailComposeTask = new EmailComposeTask()
            {
                Subject = string.Format(AppResources.AboutPage_ShareTaskTitle, Constants.GameName),
                Body = Windows.ApplicationModel.Store.CurrentApp.LinkUri.ToString()
            };
            emailComposeTask.Show();
        }
        private void LikeButton_OnClick(object sender, RoutedEventArgs e)
        {
            var wbt = new Microsoft.Phone.Tasks.WebBrowserTask { Uri = new Uri(Constants.FacebookFanPageUrl) };
            wbt.Show();
            this.CurrentApp().EventsBus.Publish(new FacebookLikeClicked());
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (CreditsPageRequested!=null)
                CreditsPageRequested.Invoke(sender,e);            
        }
        private async void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
        {



            string addr = "http://webservices3328.azurewebsites.net";
            var values = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("deviceId", "12345"),
                        new KeyValuePair<string, string>("registrationId", "123456")
                    };

            HttpClientHandler handler = new HttpClientHandler();
            var httpClient = new HttpClient(handler);
            httpClient.BaseAddress = new Uri(addr);
         //   httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            
            var response = await httpClient.PostAsync(@"/api/ContestRegistrationData/RegisterDevice/", new FormUrlEncodedContent(values));
           // var response = await httpClient.GetAsync(@"/api/ContestRegistrationData/IsContestActive/");
            response.EnsureSuccessStatusCode();
        }
    }
}
