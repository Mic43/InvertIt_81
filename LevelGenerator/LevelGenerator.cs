using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;
using GameLogic.Board;
using Infrastructure;

namespace LevelGenerator
{
 
    public class LevelGenerator
    {
        public static Func<BoardCoordinate, BoardCoordinate, double> DistanceFunc =
            (coord1, coord2) => Math.Min(3, Math.Max(Math.Abs(coord1.X - coord2.X), Math.Abs(coord1.Y - coord2.Y)));

        private BoardCoordinate[,] _boardIndexes;
        private static ConcurrentDictionary<int, double> minDistanceDictionary = new ConcurrentDictionary<int, double>();
        private static ConcurrentDictionary<int, double> maxDistanceDictionary = new ConcurrentDictionary<int, double>();

        public Func<IEnumerable<double>, bool> GetEstimator(double difficultyCoeff, int elementsCount)
        {
            if (difficultyCoeff < 0 && difficultyCoeff > 1)
                throw new ArgumentException("difficultyCoeff");
            return
                (collection =>
                    collection.Sum() ==  MinDistance(elementsCount, DistanceFunc) + 
                    (int)(difficultyCoeff*(MaxDistance(elementsCount) - MinDistance(elementsCount, DistanceFunc))));
        }

        public double MaxDistance(int elementsCount)
        {
            if (maxDistanceDictionary.ContainsKey(elementsCount))
                return maxDistanceDictionary[elementsCount];
            double value =  new Combinations<int>(Enumerable.Range(0, elementsCount).ToList(), 2).Count*3;
            maxDistanceDictionary[elementsCount] = value;
            return value;
        }
        public double MinDistance(int elementsCount,Func<BoardCoordinate,BoardCoordinate,double> herusticFunc )
        {
            if (elementsCount < 2) throw new ArgumentException("elemnts must be be grater than 1");

            if (minDistanceDictionary.ContainsKey(elementsCount))
                return minDistanceDictionary[elementsCount];

            _boardIndexes = new BoardCoordinate[10, 10];          
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    _boardIndexes[i, j] = new BoardCoordinate(i, j);                 
                }
            }
            List<BoardCoordinate> boardCoordinates = null;
            var center = new BoardCoordinate(4, 4);
            if (elementsCount <= 9)
            {
                boardCoordinates = GetNeigbours(1, center);
            }
            var coordinates = boardCoordinates.Take(elementsCount - 1).ToList();
            coordinates.Add(center);

            var permutations = new Combinations<BoardCoordinate>(coordinates,2);
            double minDistance = permutations.Select(set => herusticFunc(set[0], set[1])).Sum();
            minDistanceDictionary[elementsCount] = minDistance;
            return minDistance;
        }
        private List<BoardCoordinate> GetNeigbours(int radius, BoardCoordinate center)
        {
            var boardCoordinates = new List<BoardCoordinate>();
            _boardIndexes.ForEach(x =>
            {
                if (Math.Abs(center.X - x.X) <= radius
                    && Math.Abs(center.Y - x.Y) <= radius && !center.Equals(x))
                {
                  boardCoordinates.Add(x);
                }
            });
            return boardCoordinates;
        }
    }

    public class ComplexLevelGenerator
    {
        private Random random = new Random();
        private LevelGenerator _levelGenerator = new LevelGenerator();
        public Func<IEnumerable<double>, bool> GetEstimator(int minMovesCount, int maxMovesCount, double difficultyCoeff,
            out int movesCount,
            double movesDifficultyWeight = 0.5)
        {
            double movesDistanceDiffCoeff;
            movesCount = random.Next(minMovesCount, maxMovesCount);
            do
            {
               
                double diffCoeffMoves = 1 - (movesCount - minMovesCount)/(double) (maxMovesCount - minMovesCount);                

                movesDistanceDiffCoeff = (difficultyCoeff - diffCoeffMoves*movesDifficultyWeight);
                if (movesDistanceDiffCoeff < 0)
                    movesCount++;
                else if (movesDistanceDiffCoeff > 1)
                    movesCount--;

            } while (movesDistanceDiffCoeff < 0 || movesDistanceDiffCoeff >1);
          
            return _levelGenerator.GetEstimator(movesDistanceDiffCoeff, movesCount);
        }
        public void Gen()
        {
//            var movesCount = 5;
//            int levelsCount = 50;
//            var boardSize = 7;
//
//            var res = new List<LevelStorageData>();
//            for (int i = 0; i < levelsCount; i++)
//            {
//                var evaluatingGenerator = new EvaluatingGenerator(new RandomUniqueGenerator(),
//                    new SimpleMovesSequentionEvaluator(LevelGenerator.heuristicFunc1,
//                        new LevelGenerator().GetEstimator(1 - i * (1 / (float)levelsCount), movesCount)));
//
//                var hashSet = new HashSet<BoardCoordinate>();
//                List<BoardCoordinate> boardCoordinates;
//                do
//                {
//                    boardCoordinates = new List<BoardCoordinate>(
//                        evaluatingGenerator.Generate(boardSize, movesCount));
//
//                    hashSet = new HashSet<BoardCoordinate>(boardCoordinates);
//                } while (res.Any(x => hashSet.SetEquals(x.MovesList.Select(m => new BoardCoordinate(m.X, m.Y)))));
//
//                res.Add(new LevelStorageData
//                {
//                    BoardSize = boardSize,
//                    Id = i,
//                    IsAvailable = true,
//                    MovesList = boardCoordinates.Select(
//                            x => new SingleMoveStorageData { X = x.X, Y = x.Y }).ToList(),
//                    OrderNo = i
//                });
//
//            }
//
//            using (MemoryStream memStm = new MemoryStream())
//            {
//                var serializer = new DataContractSerializer(typeof(List<LevelStorageData>));
//                serializer.WriteObject(memStm, res);
//
//                memStm.Seek(0, SeekOrigin.Begin);
//
//                using (var streamReader = new StreamReader(memStm))
//                {
//                    string result = streamReader.ReadToEnd();
//                }
//            }
//
//        }
        }
    }
}