using System;
using System.Windows;
using System.Windows.Controls;
using Infrastructure;
using Infrastructure.Storage;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using NoNameGame.Configuration;
using NoNameGame.Configuration.NewAchievements;
using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Infrastructure;
using NoNameGame.CustomControls.Popups;
using NoNameGame.Resources;
using RateAppControl = NoNameGame.CustomControls.Popups.RateAppControl;

namespace NoNameGame.Helpers
{
    internal class AskForRatingDisplayer
    {
        private readonly IAppRunListener _appRunListener;
        private readonly IEventsBus _eventsBus;
        private readonly Func<int, bool> _showRatingForAppRunCount;
        private void AppRunListenerOnAppRan(object sender, EventArgs eventArgs)
        {
            AppRunCount++;
        }
        public AskForRatingDisplayer(IAppRunListener appRunListener, IEventsBus eventsBus,
            Func<int, bool> showRatingForAppRunCount)
        {
            if (appRunListener == null) throw new ArgumentNullException("appRunListener");
            if (eventsBus == null) throw new ArgumentNullException("eventsBus");
            if (showRatingForAppRunCount == null) throw new ArgumentNullException("showRatingForAppRunCount");

            _appRunListener = appRunListener;
            _eventsBus = eventsBus;
            _showRatingForAppRunCount = showRatingForAppRunCount;
            //_appRunListener.AppRan -= AppRunListenerOnAppRan;
            _appRunListener.AppRan += AppRunListenerOnAppRan;
        }
        public int AppRunCount
        {
            get { return AppSettingsAccessor.GetValueOrDefault(AppSettingsKeys.RunCount, 0); }
            private set
            {
                AppSettingsAccessor.AddOrUpdateValue(AppSettingsKeys.RunCount, value);
                AppSettingsAccessor.Save();
            }
        }
        public bool WasAppRatingShowed
        {
            get { return AppSettingsAccessor.GetValueOrDefault(AppSettingsKeys.AppRatingShowed, false); }
            private set
            {
                AppSettingsAccessor.AddOrUpdateValue(AppSettingsKeys.AppRatingShowed, value);
                AppSettingsAccessor.Save();
            }
        }

        public void ShowDialogIfNeeded(PhoneApplicationPage page)
        {
            if (WasAppRatingShowed || !_showRatingForAppRunCount(AppRunCount))
                return;

            var rateAppControl = new RateAppControl();
            new PopupWindowService(page, rateAppControl,
                new UIElementWithTappedAction(rateAppControl.OkButton, OnOkClicked),new UIElementWithTappedAction(rateAppControl.CancelButton,(a) => {})).Show();

//            if (MessageBox.Show(AppResources.AskForRating_Content, AppResources.AskForRating_Title,
//                MessageBoxButton.OKCancel) == MessageBoxResult.OK)
//            {
//                var marketplaceReviewTask = new MarketplaceReviewTask();
//                marketplaceReviewTask.Show();
//                WasAppRatingShowed = true;
//
//                _eventsBus.Publish(new AppRated());
//            }
        }
        private void OnOkClicked(UIElement uiElement)
        {
            var marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
            WasAppRatingShowed = true;
            
            _eventsBus.Publish(new AppRated());
        }
    }
}