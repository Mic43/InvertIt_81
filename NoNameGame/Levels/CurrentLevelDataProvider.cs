using System;

namespace NoNameGame.Levels
{
    public class CurrentLevelDataProvider : ICurrentLevelDataProvider
    {
        private readonly ILevelProvider _levelProvider;
        public CurrentLevelDataProvider(ILevelProvider provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            _levelProvider = provider;
        }
        public CurrentLevelData Get(int levelId)
        {
            var levelDataEntity = _levelProvider.GetLevel(levelId);
            var levelGroup = _levelProvider.GetLevelGroup(levelDataEntity.LevelGroupId);
            var levelPack = _levelProvider.GetLevelPack(levelGroup.LevelPackId);

            return new CurrentLevelData(levelDataEntity.DisplayName, levelGroup.Name, levelPack.Name);
        }
    }
}