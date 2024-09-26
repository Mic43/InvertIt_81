using System;
using System.Collections.Generic;
using System.Data.Linq;

namespace NoNameGame.Helpers.FullScreenAds.AdsEnablers
{
    public abstract class CompositeAdsEnabler : IAdsEnabler
    {
        private readonly IEnumerable<IAdsEnabler> _adsEnablers;
        public CompositeAdsEnabler(IEnumerable<IAdsEnabler> adsEnablers )
        {
            if (adsEnablers == null) throw new ArgumentNullException("adsEnablers");
            _adsEnablers = adsEnablers;
        }
        public IEnumerable<IAdsEnabler> AdsEnablers
        {
            get { return _adsEnablers; }
        }
        public abstract bool AreAdsEnabled();

    }
}