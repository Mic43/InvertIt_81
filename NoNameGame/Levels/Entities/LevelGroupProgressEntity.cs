using System;
using System.Runtime.Serialization;

namespace NoNameGame.Levels.Entities
{   
    public class LevelGroupProgressEntity
    {   
        public int FinishedLevelsCount { get; private set; }
        public LevelGroupProgressEntity(int finishedLevelsCount)
        {
            if (finishedLevelsCount < 0 )
                throw new ArgumentOutOfRangeException("finishedLevelsCount", "finishedLevelsCount must be grater than zero");
            FinishedLevelsCount = finishedLevelsCount;
        }
    }
}