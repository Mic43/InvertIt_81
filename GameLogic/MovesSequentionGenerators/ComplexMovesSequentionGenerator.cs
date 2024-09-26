using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GameLogic.Board;
using GameLogic.MovesSequentionGenerators.Helpers;

namespace GameLogic.MovesSequentionGenerators
{
    public class ComplexGenerator : IMovesSequentionGenerator
    {        
        private readonly List<BoardCoordinate> _acceptedGeneratedMoves = new List<BoardCoordinate>();
        private readonly Random _random = new Random();
        private GeneratedMovesDictionary _generatedMovesDictionary;

        private readonly Func<BoardCoordinate, BoardCoordinate, double> _heuristicFunc;
        private readonly Func<IEnumerable<double>, bool> _newMoveEvaluator;
        private int _movesCount;
        private BoardSize _boardToSetupSize;

        public Func<BoardCoordinate, BoardCoordinate, double> HeuristicFunc
        {
            get { return _heuristicFunc; }
        }
        public Func<IEnumerable<double>, bool> NewMoveEvaluator
        {
            get { return _newMoveEvaluator; }
        }
        public  int MovesCount
        {
            get { return MovesCount; }
        }

        public ComplexGenerator(Func<BoardCoordinate, BoardCoordinate, double> heuristicFunc,
                                    Func<IEnumerable<double>, bool> newMoveEvaluator)
        {
            _newMoveEvaluator = newMoveEvaluator;
            //_movesCount = movesCount;
            _heuristicFunc = heuristicFunc;
        }
      
        public Collection<BoardCoordinate> Generate(BoardSize boardSize, int movesCount)
        {
            _movesCount = movesCount;
            _boardToSetupSize = boardSize;
            _generatedMovesDictionary = new GeneratedMovesDictionary(_boardToSetupSize);

            GenerateMoves();
            return new Collection<BoardCoordinate>(_acceptedGeneratedMoves);
        }

        private void GenerateMoves()
        {
            while (_acceptedGeneratedMoves.Count < MovesCount)
            {
                GenerateFirstMove();

                BoardCoordinate newMoveCoordinate;
                if (TryGenerateNextAcceptedMove(out newMoveCoordinate))                
                    _acceptedGeneratedMoves.Add(newMoveCoordinate);                      
                else                
                    _acceptedGeneratedMoves.Clear();
                
                _generatedMovesDictionary.ResetAndExclude(_acceptedGeneratedMoves);
            }            
        }
       
        private bool TryGenerateNextAcceptedMove(out BoardCoordinate newMoveCoordinate)
        {
            IEnumerable<double> heuristicValues;
            do
            {
                newMoveCoordinate = GenerateRandomUniqueMove();
                _generatedMovesDictionary.MarkAsGenerated(newMoveCoordinate);

                if (_generatedMovesDictionary.AllPossibleMovesGenerated())
                {
                    return false;
                }
                BoardCoordinate coordinate = newMoveCoordinate;
                heuristicValues =
                    _acceptedGeneratedMoves.Select(oldMoveCoordinate => HeuristicFunc(coordinate, oldMoveCoordinate));
            } while (!NewMoveEvaluator(heuristicValues));
            return true;
        }

        private void GenerateFirstMove()
        {
            var firstMove = GenerateRandomUniqueMove();
            _acceptedGeneratedMoves.Add(firstMove);
            _generatedMovesDictionary.MarkAsGenerated(firstMove);
        }

        private BoardCoordinate GenerateRandomUniqueMove()
        {
            BoardCoordinate boardCoordinate;
            do
            {
                boardCoordinate = new BoardCoordinate(GenerateRandomCoordinate(), GenerateRandomCoordinate());
            } while (_acceptedGeneratedMoves.Contains(boardCoordinate));            
            return boardCoordinate;
        }

        private int GenerateRandomCoordinate()
        {
            return _random.Next(0, _boardToSetupSize.Value);
        }
    }
}