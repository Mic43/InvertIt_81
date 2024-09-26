using System;

namespace GameLogic.Board
{
    public struct BoardSize
    {
        private int _value;
        public const int MaxValue = 20;

        public int Value
        {
            get { return _value; }           
        }

        public BoardSize(int value)
        {
            if (value <= 0 || value > MaxValue)
                throw new ArgumentOutOfRangeException("value");
            _value = value;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        public static implicit operator int(BoardSize boardSize)  // implicit digit to byte conversion operator
        {
            return boardSize.Value;  // implicit conversion
        }
        public static implicit operator BoardSize(int value)  // implicit digit to byte conversion operator
        {
            return new BoardSize(value);  // implicit conversion
        }

    }
}