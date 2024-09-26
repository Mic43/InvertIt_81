using System.Collections.Generic;
using System.Linq;

namespace NoNameGame.Helpers.FullScreenAds.AdsEnablers
{
    public class AllEnabled : CompositeAdsEnabler
    {
        public AllEnabled(IEnumerable<IAdsEnabler> adsEnablers) : base(adsEnablers)
        {
        }
        public AllEnabled(params IAdsEnabler[] adsEnablers)
            : this(adsEnablers.AsEnumerable())
        {
            
        }
        public override bool AreAdsEnabled()
        {
            return AdsEnablers.All(enabler => enabler.AreAdsEnabled());
        }
    }
}