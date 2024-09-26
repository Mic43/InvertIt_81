using System;

namespace NoNameGame.Controllers.PlayerStats
{
    public class GameProgress
    {
        public double Value { get; private set; }

        public GameProgress(double value)
        {
            if (value < 0.0 || value > 1.0)
                throw  new ArgumentOutOfRangeException("value","Progress must be nonegative and less or equal than 1.0");
            Value = value;
        }
        public GameProgress(int currentStars, int maxStars)
            : this(currentStars / (double)maxStars)
        {            
        }
    }
}