using System;
using NoNameGame.Helpers;
using NoNameGame.Levels.DB;
using NoNameGame.Levels.Entities;

namespace NoNameGame.Levels.Converters
{
    public static class LevelGroupConverter
    {
        public static LevelGroupEntity CreateEntity(LevelGroup levelGroup)
        {
            var name = ExtractLevelGroupName(levelGroup);

            return new LevelGroupEntity(levelGroup.Id, name, levelGroup.Description, levelGroup.OrderNo,
                levelGroup.AllLevelsUnlocked, levelGroup.LevelPackId);
        }
        private static string ExtractLevelGroupName(LevelGroup levelGroup)
        {
            GameCulture gameCulture = new GameCurrentCultureProvider().Get();
            string name = "";
            switch (gameCulture)
            {
                case GameCulture.Polish:
                    name = levelGroup.Name_PL;
                    break;
                case GameCulture.English:
                case GameCulture.Other:
                    name = levelGroup.Name;
                    break;
                case GameCulture.Spanish:
                    name = levelGroup.Name_ES;
                    break;
            }
            return name;
        }
    }
}