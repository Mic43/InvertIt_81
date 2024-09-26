using System;

namespace GameLogic.MovesSequentionGenerators.DifficultyAssigners
{
    internal class LevelDifficulty
    {
        private double _value;

        public double Value
        {
            get { return _value; }            
        }

        public LevelDifficulty(double value)
        {
            if (value < 0 || value > 1.0)
                throw new ArgumentOutOfRangeException("value");
            _value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        public static implicit operator double(LevelDifficulty levelDifficulty)  // implicit digit to byte conversion operator
        {
            return levelDifficulty.Value;  // implicit conversion
        }
        public static implicit operator LevelDifficulty(double value)  // implicit digit to byte conversion operator
        {
            return new LevelDifficulty(value);  // implicit conversion
        }

    }
}