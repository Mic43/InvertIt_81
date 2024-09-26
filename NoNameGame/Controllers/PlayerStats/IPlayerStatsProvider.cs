using Infrastructure.Storage;

namespace NoNameGame.Controllers.PlayerStats
{
    public interface IPlayerStatsProvider
    {
        int GetCurrentPlayerStarsCount();
        int GetAllLevelsStarsCount();
    }

//    class AppSettinsgPlayerStatsProvider : IPlayerStatsProvider
//    {
//        private readonly IPlayerStatsProvider _defaultStatsValueProvider;
//        private readonly string CurrentPlayerStarsKeyName = "CurrentPlayersStarsCount";
//        public AppSettinsgPlayerStatsProvider(IPlayerStatsProvider defaultStatsValueProvider)
//        {
//            if (defaultStatsValueProvider == null) throw new ArgumentNullException("defaultStatsValueProvider");
//            _defaultStatsValueProvider = defaultStatsValueProvider;
//        }
//        public int GetCurrentPlayerStarsCount()
//        {
//           return AppSettingsAccessor.GetValueOrDefault(CurrentPlayerStarsKeyName,
//                _defaultStatsValueProvider.GetCurrentPlayerStarsCount());
//        }
//        public int GetAllLevelsStarsCount()
//        {
//            throw new System.NotImplementedException();
//        }
//    }
}