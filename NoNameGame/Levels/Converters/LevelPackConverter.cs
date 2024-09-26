using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Devices.Sensors;
using NoNameGame.Helpers;
using NoNameGame.Levels.DB;
using NoNameGame.Levels.Entities;

namespace NoNameGame.Levels.Converters
{
    public static class LevelPackConverter
    {

        private static string ExtractLevelPackDescription(LevelPack levelPack)
        {
            GameCulture gameCulture = new GameCurrentCultureProvider().Get();
            string name = "";
            switch (gameCulture)
            {
                case GameCulture.Polish:
                    name = levelPack.Description_PL;
                    break;
                case GameCulture.English:
                case GameCulture.Other:
                    name = levelPack.Description;
                    break;
                case GameCulture.Spanish:
                    name = levelPack.Description_ES;
                    break;
            }
            return name;
        }

        public static LevelPackEntity CreateEntity(LevelPack levelPack)
        {
            var gameCurrentCultureProvider = new GameCurrentCultureProvider();
            string decription = ExtractLevelPackDescription(levelPack);
            return new LevelPackEntity(levelPack.Id, levelPack.Name, decription, levelPack.OrderNo);
        }
    }
}