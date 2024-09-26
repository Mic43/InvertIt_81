using System;
using com.vserv.windows.ads;
using com.vserv.windows.ads.wp8;

namespace NoNameGame.Helpers.FullScreenAds.AdsProviders
{
    public class VServAdDisplayer : FullScreenAdDisplayer
    {
        private readonly string _zoneId;
        private VservAdView _adView;
        private bool isLoaded = false;
        private bool isLoading = false; 
        public VServAdDisplayer(string zoneId,Action onDismissed, Action onError) : base(onDismissed, onError)
        {
            if (zoneId == null) throw new ArgumentNullException("zoneId");
            _zoneId = zoneId;

            //​ To specify Interstitial ads.  adView.ZoneId = "MY_ZONE_ID
        }
        private void CreateAd()
        {
            _adView = new VservAdView {UX = VservAdUX.Interstitial, ZoneId = _zoneId};
            _adView.FailedToLoadAd += AdViewOnFailedToLoadAd;
            _adView.FailedToCacheAd += AdViewOnFailedToCacheAd;
            _adView.WillDismissOverlay += AdViewOnWillDismissOverlay;       
            _adView.DidCacheAd+=AdViewOnDidCacheAd;
        }
        private void AdViewOnDidCacheAd(object sender, EventArgs eventArgs)
        {
            isLoaded = true;
            isLoading = false;
        }     
        private void AdViewOnWillDismissOverlay(object sender, EventArgs eventArgs)
        {
            OnDismissed();
        }
        private void AdViewOnFailedToCacheAd(object sender, EventArgs eventArgs)
        {
            isLoaded = false;
            isLoading = false;
        }
        private void AdViewOnFailedToLoadAd(object sender, VservAdView.AdFailedEventArgs adFailedEventArgs)
        {
            isLoaded = false;
            isLoading = false;
            OnError();
        }
        public override void Preload()
        {
            //_adView.
            if (!isLoaded)
            {
                isLoading = true;
                CreateAd();
                _adView.CacheAd();
            }
        }
        public override void TryShowAsync()
        {
            if (isLoaded)
            {
                isLoaded = false;
                _adView.ShowAd();
            }
            else
                OnError();          
        }
    }
}