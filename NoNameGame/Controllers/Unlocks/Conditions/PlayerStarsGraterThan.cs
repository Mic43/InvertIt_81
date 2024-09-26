using NoNameGame.Controllers.PlayerStats;

namespace NoNameGame.Controllers.Unlocks.Conditions
{
    public class PlayerStarsGreaterEqualThan:ICondition
    {
        private readonly IPlayerStatsProvider _provider;
        public int StarsCount { get; private set; }
        public PlayerStarsGreaterEqualThan(int starsCount,IPlayerStatsProvider provider)
        {
            _provider = provider;
            StarsCount = starsCount;
        }      
        public  bool IsTrue()
        {
            return _provider.GetCurrentPlayerStarsCount() >= StarsCount;
        }
        public string GetDescription()
        {
            return string.Format("{0}", StarsCount);
        }
    }
}