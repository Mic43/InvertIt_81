using System;
using System.Windows.Media.Animation;

namespace NoNameGame.Levels.Entities
{
    public class LevelProgressEntity
    {      
        public int LevelId { get; private set; }
        public int Stars { get;  set; }
        public bool IsAvailable { get;  set; }
        public TimeSpan FirstSolveDuration { get; set; }
        public bool IsFinished { get { return !FirstSolveDuration.Equals(TimeSpan.Zero); }}

        public LevelProgressEntity(int levelId,int stars, bool isAvailable,TimeSpan firstSolveDuration)
        {
            FirstSolveDuration = firstSolveDuration;
            LevelId = levelId;
            Stars = stars;
            IsAvailable = isAvailable;
        }
    }
}