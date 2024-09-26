using System;
using System.Windows.Controls;
using AdDealsSDKWP8;
using AdDealsSDKWP8.Views.UserControls;

namespace NoNameGame.Helpers.FullScreenAds.AdsProviders
{
    class AdDealsDisplayer : FullScreenAdDisplayer
    {
        private readonly Grid _mainPageLayout;

        private AdDealsSquare _mySquare;
        private bool _isLoaded;

        public AdDealsDisplayer(Grid mainPageLayout,Action onDismissed, Action onError)
            : base(onDismissed,onError)
        {
            if (mainPageLayout == null) throw new ArgumentNullException("mainPageLayout");            
            _mainPageLayout = mainPageLayout;            
        }
        private void CreateAdSquare()
        {
            _mySquare = AdManager.CreateAdDealsSquare(_mainPageLayout);
            _mySquare.SetSupportedInterstitialFormats(AdManager.AdKind.FULLSCREENADSONLY);
            _mySquare.OpenAdDealsWall -= MySquareOnOpenAdDealsWall;
            _mySquare.CloseAdDealsSquare -= MySquareOnCloseAdDealsSquare;
            _mySquare.NoAdAvailable -= MySquareOnNoAdAvailable;
            _mySquare.NoOrSlowInternetConnection -= MySquareOnNoOrSlowInternetConnection;   


            _mySquare.OpenAdDealsWall += MySquareOnOpenAdDealsWall;
            _mySquare.CloseAdDealsSquare += MySquareOnCloseAdDealsSquare;
            _mySquare.NoAdAvailable += MySquareOnNoAdAvailable;
            _mySquare.NoOrSlowInternetConnection += MySquareOnNoOrSlowInternetConnection;
        }
        private void MySquareOnNoOrSlowInternetConnection(object sender, EventArgs args)
        {
            OnError();
        }
        private void MySquareOnNoAdAvailable(object sender, EventArgs args)
        {
            OnError();
        }
        private void MySquareOnCloseAdDealsSquare(object sender, EventArgs args)
        {
            OnDismissed();
        }
        public override void Preload()
        {
            CreateAdSquare();
            _mySquare.Prefetch();
        }
        private void MySquareOnOpenAdDealsWall(object sender, EventArgs eventArgs)
        {
            PageExtensions.CurrentPage().
                NavigationService.Navigate(new Uri("/AdDealsSDKWP8;component/Views/MoreAdDeals.xaml", UriKind.Relative)); // DO NOT REMOVE THIS LINE.        
        }
        public override void TryShowAsync()
        {
            if (_mySquare == null || AdDealsSquare.IsPrefetchingSquareAd())
                OnError();
            else            
                _mySquare.Launch();                                         
        }      
    }
}