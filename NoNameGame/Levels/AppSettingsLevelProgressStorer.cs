using System;
using Infrastructure.Storage;
using NoNameGame.Configuration;
using NoNameGame.Levels.Converters;
using NoNameGame.Levels.Entities;

namespace NoNameGame.Levels
{
    public class AppSettingsLevelProgressStorer : ILevelProgressStorer
    {
        public void Save(LevelProgressEntity levelProgressEntity)
        {
            AppSettingsAccessor.AddOrUpdateValue(AppSettingsKeys.LevelProgress + levelProgressEntity.LevelId,
                LevelProgressConverter.FromEntity(levelProgressEntity));
            AppSettingsAccessor.Save();
        }
        public LevelProgressEntity Load(int levelId)
        {
            return LevelProgressConverter.CreateEntity(AppSettingsAccessor.GetValueOrDefault(AppSettingsKeys.LevelProgress + levelId,
                new LevelProgresDto
                {
                    LevelId = levelId,
                    Stars = 0,
                    IsAvailable = false,
                    FirstSolveDuration = TimeSpan.Zero
                }));
        }
    }
}