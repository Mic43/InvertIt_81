using System;
using Microsoft.Phone.Reactive;
using NoNameGame.Helpers.FullScreenAds.AdsEnablers;

namespace NoNameGame.Helpers.FullScreenAds
{

    public class DisableableFullScreenAdDisplayer : IFullScreenAdDisplayer
    {
        public Action AfterNotShowed { get; private set; }
        private readonly IFullScreenAdDisplayer _innerAdDisplayer;
        private readonly IAdsEnabler _adEnabler;

        public DisableableFullScreenAdDisplayer(IFullScreenAdDisplayer innerAdDisplayer, IAdsEnabler adEnabler,Action afterNotShowed)
            
        {
            if (innerAdDisplayer == null) throw new ArgumentNullException("innerAdDisplayer");
            if (adEnabler == null) throw new ArgumentNullException("adEnabler");
            if (afterNotShowed == null) throw new ArgumentNullException("afterNotShowed");

            AfterNotShowed = afterNotShowed;
            _innerAdDisplayer = innerAdDisplayer;
            _adEnabler = adEnabler;            
        }
        public DisableableFullScreenAdDisplayer(FullScreenAdDisplayer innerAdDisplayer, Func<bool> adEnablerFunc, Action afterNotShowed)
            : this(innerAdDisplayer,new FuncAdsEnabler(adEnablerFunc),afterNotShowed)
        {            
        }
     
        public  void Preload()
        {
         //   if (_adEnabler.AreAdsEnabled())
                _innerAdDisplayer.Preload();
        }
        public  void TryShowAsync()
        {
            if (_adEnabler.AreAdsEnabled())
                _innerAdDisplayer.TryShowAsync();
            else
                AfterNotShowed();
        }
    }
}