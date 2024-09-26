using GameLogic;
using NoNameGame.Levels.Entities;

namespace NoNameGame.Levels.Converters
{
    public class LevelProgressConverter
    {
        public static LevelProgressEntity CreateEntity(LevelProgresDto levelData)
        {
            return new LevelProgressEntity(levelData.LevelId, levelData.Stars, levelData.IsAvailable, levelData.FirstSolveDuration);
        }
        public static LevelProgresDto FromEntity(LevelProgressEntity entity)
        {
            return new LevelProgresDto()
            {
                FirstSolveDuration = entity.FirstSolveDuration,
                IsAvailable = entity.IsAvailable,
                Stars = entity.Stars,
                LevelId = entity.LevelId
            };
        } 
    }
}