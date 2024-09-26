using System;
using System.Windows.Controls;
using NoNameGame.Helpers.FullScreenAds;
using NoNameGame.Helpers.FullScreenAds.AdsEnablers;
using NoNameGame.Helpers.FullScreenAds.AdsProviders;

namespace NoNameGame.Configuration.InAppPurchase.Ads
{
    public class InGameFullSceenAdDisplayerFactory
    {
        private readonly IAdsEnabler _adsEnabler;
        public InGameFullSceenAdDisplayerFactory(IAdsEnabler adsEnabler)
        {
            if (adsEnabler == null) throw new ArgumentNullException("adsEnabler");
            _adsEnabler = adsEnabler;
        }
        public IFullScreenAdDisplayer Create(Action continueWith,Grid mainPageGrid)
        {
            var disableableFullScreenAdDisplayer =
              new DisableableFullScreenAdDisplayer(
                  new GoogleFullScreenAdDisplayer(Constants.FullScreenGoogleAdId, continueWith, continueWith),
                  _adsEnabler, continueWith);
            return disableableFullScreenAdDisplayer;

//            var disableableFullScreenAdDisplayer =
//              new DisableableFullScreenAdDisplayer(
//                  new AdDealsDisplayer(mainPageGrid, continueWith, continueWith),
//                  _adsEnabler, continueWith);
//            return disableableFullScreenAdDisplayer;

//            var disableableFullScreenAdDisplayer =
//                new DisableableFullScreenAdDisplayer(
//                    new VServAdDisplayer(Constants.VServZoneId, continueWith, continueWith),
//                    _adsEnabler, continueWith);
//            return disableableFullScreenAdDisplayer;

//               var disableableFullScreenAdDisplayer =
//                new DisableableFullScreenAdDisplayer(
//                    new SmatooAdDisplayer(continueWith, continueWith),
//                    _adsEnabler, continueWith);
//                    return disableableFullScreenAdDisplayer;


        }
    }
}