namespace NoNameGame.Controllers.PlayerStats
{
    public class PlayerStatsController
    {
        private readonly IPlayerStatsProvider _provider;
        public PlayerStatsController(IPlayerStatsProvider provider)
        {
            _provider = provider;
        }
        public StarsProgressModel GetStarsProgress()
        {
            return new StarsProgressModel(_provider.GetCurrentPlayerStarsCount(),_provider.GetAllLevelsStarsCount());
        }
    }

    public class StarsProgressModel
    {
        public int CurrentStarsCount { get; set; }
        public int AllStarsCount { get; set; }

        public StarsProgressModel(int currentStarsCount, int allStarsCount)
        {
            CurrentStarsCount = currentStarsCount;
            AllStarsCount = allStarsCount;
        }
    }
}