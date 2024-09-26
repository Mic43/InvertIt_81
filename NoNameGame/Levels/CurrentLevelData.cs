using System;
using System.Collections.Generic;

namespace NoNameGame.Levels
{
    public class CurrentLevelData
    {
    
        public string LevelDisplayName { get; private set; }
        public string LevelGroupName { get; private set; }
        public string LevelPackName { get; private set; }

        public CurrentLevelData(string levelDisplayName, string levelGroupName, string levelPackName)
        {
            if (levelDisplayName == null) throw new ArgumentNullException("levelDisplayName");
            if (levelGroupName == null) throw new ArgumentNullException("levelGroupName");
            if (levelPackName == null) throw new ArgumentNullException("levelPackName");

            LevelDisplayName = levelDisplayName;
            LevelGroupName = levelGroupName;
            LevelPackName = levelPackName;
        }

        public Dictionary<string, string> ToDictionary()
        {
            var properties = new Dictionary<string, string>()
            {
                {"LevelDisplayName", LevelDisplayName},
                {"LevelGroupName", LevelGroupName},
                {"LevelPackName", LevelPackName}
            };
            return properties;
        }
    }
}