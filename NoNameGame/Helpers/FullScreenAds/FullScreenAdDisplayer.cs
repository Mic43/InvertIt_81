using System;
using System.Windows.Navigation;
using GoogleAds;

namespace NoNameGame.Helpers.FullScreenAds
{
    public abstract class FullScreenAdDisplayer : IFullScreenAdDisplayer
    {
        private  Action _onDismissed;
        private readonly Action _onError;
        
        public FullScreenAdDisplayer(Action onDismissed, Action onError)
        {
            if (onDismissed == null) throw new ArgumentNullException("onDismissed");
            if (onError == null) throw new ArgumentNullException("onError");

            _onDismissed = onDismissed;
            _onError = onError;
        }
        public Action OnDismissed
        {
            get { return _onDismissed; }            
        }
        public Action OnError
        {
            get { return _onError; }
        }
        public abstract void Preload();
        public abstract void TryShowAsync();
    }
}