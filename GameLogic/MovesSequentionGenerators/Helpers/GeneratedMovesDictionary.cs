using System;
using System.Collections.Generic;
using System.Linq;
using GameLogic.Board;

namespace GameLogic.MovesSequentionGenerators.Helpers
{
    public class GeneratedMovesDictionary
    {
        private readonly BoardSize _boardSize;
        private readonly Dictionary<BoardCoordinate, bool> _generatedMovesDictionary = new Dictionary<BoardCoordinate, bool>();

        public GeneratedMovesDictionary(BoardSize boardSize)
        {
            _boardSize = boardSize;            
            Reset();
        }

        public void MarkAsGenerated(BoardCoordinate boardCoordinate)
        {
            if (!_generatedMovesDictionary.ContainsKey(boardCoordinate))
                throw new ArgumentOutOfRangeException("boardCoordinate","boardCoordinate not valid on this boardSize");
            _generatedMovesDictionary[boardCoordinate] = true;
        }

        public bool AllPossibleMovesGenerated()
        {
            return _generatedMovesDictionary.Values.All(val => val == true);
        }

        public void Reset()
        {
            ResetAndExclude(Enumerable.Empty<BoardCoordinate>());
        }
        public void ResetAndExclude(IEnumerable<BoardCoordinate> movesToExclude )
        {
            _generatedMovesDictionary.Clear();
            for (int i = 0; i < _boardSize.Value; i++)
            {
                for (int j = 0; j < _boardSize.Value; j++)
                {                    
                    var boardCoordinate = new BoardCoordinate(i, j);

                    if (!movesToExclude.Contains(boardCoordinate))
                        _generatedMovesDictionary.Add(boardCoordinate, false);
                }
            }
        }
    }
}