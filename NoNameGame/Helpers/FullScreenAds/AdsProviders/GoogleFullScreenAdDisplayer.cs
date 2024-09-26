using System;
using GoogleAds;

namespace NoNameGame.Helpers.FullScreenAds.AdsProviders
{
    class GoogleFullScreenAdDisplayer : FullScreenAdDisplayer
    {
        private readonly string _unitId;
        private bool isLoaded = false;
        private bool isLoading = false;     
        private InterstitialAd _interstitialAd;
        private AdRequest _adRequest;
        private bool leavingApp = false;

        public GoogleFullScreenAdDisplayer(string unitId)
            : this(unitId, () => {},() => {})
        {
            _unitId = unitId;
        }
        public GoogleFullScreenAdDisplayer(string unitId, Action onDismissed, Action onError)  : base(onDismissed,onError)
        {
            _unitId = unitId;
            if (unitId == null) throw new ArgumentNullException("unitId");

               
        }
        private void InterstitialAdOnFailedToReceiveAd(object sender, AdErrorEventArgs adErrorEventArgs)
        {
            isLoaded = false;
            isLoading = false;
            System.Diagnostics.Debug.WriteLine("Ad failed");   
          //  OnError();
        }
        private void OnAdReceived(object sender, AdEventArgs e)
        {            
            System.Diagnostics.Debug.WriteLine("Ad received successfully");
            isLoaded = true;
            isLoading = false;
        }

        public override void Preload()
        {
            System.Diagnostics.Debug.WriteLine("Preload called");    
            if (!isLoaded)
            {
                isLoading = true;
                System.Diagnostics.Debug.WriteLine("Inner Preload called");

                _interstitialAd = new InterstitialAd(_unitId);                
                _interstitialAd.ReceivedAd += OnAdReceived;
                 _interstitialAd.LeavingApplication+=InterstitialAdOnLeavingApplication;
                _interstitialAd.FailedToReceiveAd += InterstitialAdOnFailedToReceiveAd;
                _interstitialAd.DismissingOverlay += InterstitialAdOnDismissingOverlay;      
                _interstitialAd.LoadAd(new AdRequest());                
            }
        }
        private void InterstitialAdOnLeavingApplication(object sender, AdEventArgs adEventArgs)
        {
            leavingApp = true;
            dismissedAfterLeavingCount = 0;
        }
        private int dismissedAfterLeavingCount = 0;
        private void InterstitialAdOnDismissingOverlay(object sender, AdEventArgs adEventArgs)
        {
            dismissedAfterLeavingCount++;
            if (leavingApp && dismissedAfterLeavingCount <=2)
            {               
                return;
            }
            OnDismissed();
            dismissedAfterLeavingCount = 0;
            leavingApp = false;
        }
        public override void TryShowAsync()
        {
            if (isLoaded)
            {
                isLoaded = false;
                _interstitialAd.ShowAd();
            }
            else
                OnError();
        }
    }
}