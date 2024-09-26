using System;

namespace NoNameGame.Models
{
    public class CurrentLevelDataModel
    {
        public string LevelDisplayName { get; private set; }
        public string LevelPackDislayName { get; set; }
        public string LevelGroupDisplayName { get; set; }
        public int PlayerMovesCount { get; set; }
        public int PerfectMovesCount { get; set; }
        public int LevelStars { get; set; }
        public int CurrentPlayerStars { get; set; }
        
        public CurrentLevelDataModel(string levelDisplayName, string levelPackDislayName, string levelGroupDisplayName,
            int playerMovesCount,int perfectMovesCount,int levelStars)
        {
            if (levelDisplayName == null) throw new ArgumentNullException("levelDisplayName");
            if (levelPackDislayName == null) throw new ArgumentNullException("levelPackDislayName");
            if (levelGroupDisplayName == null) throw new ArgumentNullException("levelGroupDisplayName");

            LevelDisplayName = levelDisplayName;
            LevelPackDislayName = levelPackDislayName;
            LevelGroupDisplayName = levelGroupDisplayName;
            PlayerMovesCount = playerMovesCount;
            PerfectMovesCount = perfectMovesCount;
            LevelStars = levelStars;
          
        }
    }
}