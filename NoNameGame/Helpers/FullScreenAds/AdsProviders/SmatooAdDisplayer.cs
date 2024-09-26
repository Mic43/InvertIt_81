using System;
using SOMAWP8;

namespace NoNameGame.Helpers.FullScreenAds.AdsProviders
{
    public class SmatooAdDisplayer :FullScreenAdDisplayer
    {
        private SomaInterstitialAd _innerAd;
        public SmatooAdDisplayer(Action onDismissed, Action onError) : base(onDismissed, onError)
        {
            _innerAd = new SomaInterstitialAd()
            {
                Adspace = 65852646,
                Pub = 923883823

            };            
            _innerAd.AdClick+=InnerAdOnAdClick;           
            _innerAd.AdError+=InnerAdOnAdError;   
            _innerAd.NewAdAvailable+=InnerAdOnNewAdAvailable;
        }
        private void InnerAdOnNewAdAvailable(object sender, EventArgs eventArgs)
        {
            ;
        }
        private void InnerAdOnAdError(object sender, string errorCode, string errorDescription)
        {
            OnError();
        }
        private void InnerAdOnAdClick(object sender, EventArgs eventArgs)
        {
            OnDismissed();
        }
        public override void Preload()
        {
           _innerAd.LoadInterstitial();
        }
        public override void TryShowAsync()
        {
            _innerAd.ShowInterstitial();
        }
    }
}