using System;
using System.Linq;

namespace NoNameGame.Configuration.InAppPurchase.Ads
{
    public class Any : IAdsRemovalProvider
    {
        private readonly IAdsRemovalProvider[] _adsRemovalProviders;
        public Any(params IAdsRemovalProvider[] adsRemovalProviders)
        {
            if (adsRemovalProviders == null) throw new ArgumentNullException("adsRemovalProviders");
            _adsRemovalProviders = adsRemovalProviders;
        }
        public bool AreRemoved()
        {
            return _adsRemovalProviders.Any(x => x.AreRemoved());
        }
    }
}