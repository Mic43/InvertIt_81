using System;
using System.Windows;

namespace GameLogic.Board
{
    public struct BoardCoordinate : IEquatable<BoardCoordinate>
    {
        private int x;
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        private int y;
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
   
        public BoardCoordinate(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
        public static explicit operator Point(BoardCoordinate boardCoordinate)  // implicit digit to byte conversion operator
        {
            return new Point(boardCoordinate.X,boardCoordinate.Y);  // implicit conversion
        }        

        public bool Equals(BoardCoordinate bc)
        {
            return (x == bc.x) && (y == bc.y);
        }
        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
                return false;

            var bc = (BoardCoordinate)obj;
            return Equals(bc);
        }
        public override int GetHashCode()
        {
            return x ^ y;
        }

        public bool IsValidOnBoard(BoardSize boardSize)
        {
            return (X >= 0 && X <= boardSize && Y >= 0 && Y < boardSize);
        }
        public override string ToString()
        {
            return string.Format("[X: {0} Y:{1}]", X, Y);
        }

        public static BoardCoordinate FromTuple(Tuple<int, int> tuple)
        {
            return new BoardCoordinate(tuple.Item1, tuple.Item2);
        }
      
    }
}