using NoNameGame.Controllers.DomainEvents.Events;
using NoNameGame.Controllers.DomainEvents.Handlers.Base;

namespace NoNameGame.Helpers.FullScreenAds.AdsEnablers
{
    public class OnNthGameWon : IAdsEnabler, IDomainEventHandler<GameWon>
    {
        private readonly int _cycleLen;
        private int _triesCount = 0;
        public OnNthGameWon(int cycleLen)
        {
            _cycleLen = cycleLen;
        }
        public bool AreAdsEnabled()
        {
            if (_triesCount < _cycleLen) 
                return false;

            _triesCount = 0;
            return true;
        }      
        public void Handle(GameWon domainEvent)
        {
            _triesCount++;
        }
    }
}