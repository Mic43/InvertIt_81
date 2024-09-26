using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using NoNameGame.Levels.DB;
using NoNameGame.Levels.Entities;

namespace NoNameGame.Levels.Converters
{
    public class LevelDataConverter
    {
        public static LevelDataEntity CreateEntity(LevelData currentLevelData)
        {
            List<Point> moves =
                Common.MovesCollectionFormatter.ParseString(currentLevelData.Moves).Select(x => new Point(x.X, x.Y)).ToList();
            return new LevelDataEntity(currentLevelData.Id, 
                currentLevelData.Difficulty,moves,
                currentLevelData.DisplayName, currentLevelData.OrderNo, currentLevelData.BoardSize, currentLevelData.TutorialStep, currentLevelData.LevelGroupId);
        }
    }
}