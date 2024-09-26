using System;

namespace NoNameGame.Helpers.FullScreenAds.AdsEnablers
{
    public class FuncAdsEnabler : IAdsEnabler
    {
        private readonly Func<bool> _adEnablerFunc;
        public FuncAdsEnabler(Func<bool> adEnablerFunc)
        {
            if (adEnablerFunc == null) throw new ArgumentNullException("adEnablerFunc");
            _adEnablerFunc = adEnablerFunc;
        }
        public bool AreAdsEnabled()
        {
            return _adEnablerFunc();
        }
    }
}