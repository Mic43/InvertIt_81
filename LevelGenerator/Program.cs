using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Combinatorics.Collections;
using Common;
using GameLogic;
using GameLogic.Board;
using GameLogic.MovesSequentionGenerators;
using GameLogic.MovesSequentionGenerators.DifficultyAssigners;
using GameLogic.WinVerifiers;
using LevelsDataBase;

namespace LevelGenerator
{

    #region Data

    [DataContract]
    public class SingleMoveStorageData
    {
        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }
    }

    [DataContract]
    public class LevelStorageData
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public List<SingleMoveStorageData> MovesList { get; set; }

        [DataMember]
        public int BoardSize { get; set; }

        public int Stars { get; set; }
        public bool IsAvailable { get; set; }

        public TimeSpan FirstSolveDuration { get; set; }

        [DataMember]
        public int OrderNo { get; set; }
    }

    #endregion

    internal class Program
    {
        
        private static readonly Random _random = new Random();
        private static void Main(string[] args)
        {
            //GenerateLevels();

            var levelsCount = 50;
            var groupId = 18;
            int startDifficulty = 25;
            int endDifficulty = 35;
            int boardSize = 9;

            int minMovesCount = 7;
            int maxMovesCount = 15;

            //GenerateLevels(boardSize,() => _random.Next(minMovesCount, maxMovesCount));
            ExportLevels(new ExportLevelsParams(levelsCount, groupId, startDifficulty, endDifficulty,minMovesCount,maxMovesCount), false, boardSize);

//            Func<IList<double>, double> varFunc = collection =>
//            {
//                if (!collection.Any())
//                    return 1;
//                var avg = collection.Average();
//                return collection.Select(el => (el - avg)*(el - avg)).Sum()/(double) collection.Count;
//            };
//
//            Func<IList<BoardCoordinate>, double> varFunc2 = collection =>
//            {
//                if (!collection.Any())
//                    return 1;
//
//                var avg = new BoardCoordinate((int) collection.Select(x => x.X).Average(), (int) collection.Select(x => x.Y).Average());
//                return collection.Select(x => LevelGenerator.heuristicFunc1(x, avg)*LevelGenerator.heuristicFunc1(x, avg))
//                    .Sum()/(float)collection.Count;
//                 
//            };
//            Func<IEnumerable<double>, double> difficultyHeuristic2 =
//               collection => collection.Count(x => x == 1) * 1.0 + collection.Count(x => x == 2);
//
//            Func<BoardCoordinate, BoardCoordinate, bool> areConnectedFunc =
//                (x1, x2) => LevelGenerator.heuristicFunc1(x1, x2) <= 2;
////
////            Func<BoardCoordinate, BoardCoordinate, bool> allAdges =
////                (x1, x2) => true;
//
//            using (var levelsDataContext = new LevelsDataContext())
//            {
//                List<Level> levels = levelsDataContext.Levels.ToList();
//
//                foreach (var level in levels)
//                {
//                    List<SingleMove> moves = MovesCollectionFormatter.ParseString(level.Moves);
//
//
//                    double value =
//                        new ClusteringDifficultyAssigner(areConnectedFunc).Assign(
//                            moves.Select(x => new BoardCoordinate(x.X, x.Y)).ToList());
//
//                    // var combinations = new Combinations<BoardCoordinate>(moves.Select(x=>new BoardCoordinate(x.X,x.Y)).ToList(), 2);
////                    var heuristicsCollection =
////                        combinations.Select(pair => LevelGenerator.heuristicFunc1(pair[0], pair[1])).ToList();
//////                   heuristicsCollection.RemoveAll(x => x == 3.0);
//////                        .Except(Enumerable.Repeat(3.0,1)).ToList();
////                    var value = difficultyHeuristic2(heuristicsCollection);
//                    level.NewDifficulty = value;
////                    //double odchylenie = Math.Sqrt(varFunc2.Invoke(moves.Select(x=>new BoardCoordinate(x.X,x.Y)).ToList()));
////                    Debug.WriteLine("Moves: " + level.Moves + " newDifficulty: " + value + " oldDifficulty: " + level.Difficulty) ;
//                }
//                levelsDataContext.SubmitChanges();
//            }

            #region chuj

                //            var res = new List<LevelStorageData>();
                //            for (int i = 0; i < levelsCount; i++)
                //            {
                //                Console.WriteLine(i);
                //                int minMovesCount = 5;
                //                int maxMovesCount = 9;
                //
                ////                var evaluatingGenerator = new EvaluatingGenerator(new RandomUniqueGenerator(),
                ////                    new SimpleMovesSequentionEvaluator(LevelGenerator.heuristicFunc1,
                ////                        new ComplexLevelGenerator().GetEstimator(minMovesCount, maxMovesCount,
                ////                            1 - i*(1/(float) levelsCount), out movesCount)));
                //
                //                Func<IEnumerable<double>, bool> newEstimator = list => list.Sum() == 20;
                //                var evaluatingGenerator = new EvaluatingGenerator(new TestMovesSequentionGenerator(),
                //                  new SimpleMovesSequentionEvaluator(LevelGenerator.heuristicFunc1,newEstimator));
                //                evaluatingGenerator.Generate(boardSize,7);

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
                //                        x => new SingleMoveStorageData {X = x.X, Y = x.Y}).ToList(),
                //                    OrderNo = i
                //                });

                // }

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

                #endregion
        }

        public class ExportLevelsParams
        {
            private int _levelsCount;
            private int _groupId;
            private int _startDifficulty;
            private int _endDifficulty;
            private int minMovesCount = 5;
            private int maxMovesCount = 9;

            public ExportLevelsParams(int levelsCount, int groupId, int startDifficulty, int endDifficulty, int minMovesCount, int maxMovesCount)
            {
                _levelsCount = levelsCount;
                _groupId = groupId;
                _startDifficulty = startDifficulty;
                _endDifficulty = endDifficulty;
                this.minMovesCount = minMovesCount;
                this.maxMovesCount = maxMovesCount;
            }
            public int LevelsCount
            {
                get { return _levelsCount; }
            }
            public int GroupId
            {
                get { return _groupId; }
            }
            public int StartDifficulty
            {
                get { return _startDifficulty; }
            }
            public int EndDifficulty
            {
                get { return _endDifficulty; }
            }
            public int MinMovesCount
            {
                get { return minMovesCount; }
            }
            public int MaxMovesCount
            {
                get { return maxMovesCount; }
            }
        }

        private static void ExportLevels(ExportLevelsParams exportLevelsParams, bool markAsUsed, int boardSize)
        {
            Func<int, int> linearDiffFunc = levelNo => (int) (((exportLevelsParams.EndDifficulty - exportLevelsParams.StartDifficulty)/(float) exportLevelsParams.LevelsCount)*levelNo + exportLevelsParams.StartDifficulty);
            using (var levelsDataContext = new LevelsDataContext())                  
            using (var levelsContext = new LevelsContext(ConnectionStringHelper.LevelsDbConnectionString))
            {              
                Collection<Level> levels =
                    new LevelsExtractorWithDisabledAreas(linearDiffFunc, x => x >= exportLevelsParams.MinMovesCount 
                                                                           && x <= exportLevelsParams.MaxMovesCount, 
                        levelsDataContext.Levels.Where(x=>x.BoardSize == boardSize), 
                        levelsContext.DisabledAreas.Where(x=>x.BoardSize == boardSize), boardSize)
                        .Extract(exportLevelsParams.LevelsCount);
                new LevelsExporter(levelsDataContext, levelsContext, markAsUsed).Export(levels, exportLevelsParams.GroupId, boardSize);
            }
        }
        private static void GenerateLevels(int boardSize,Func<int> movesCountFunc )
        {
            Func<IEnumerable<double>, double> difficultyHeuristic =
                collection => collection.Count(x => x == 1)*2 + collection.Count(x => x == 2);


            int movesCount = 9;
            int levelsCount = 50;
            var randomUniqueGenerator = new RandomUniqueGenerator();

            //double max = new LevelGenerator().MaxDistance(movesCount);
            //double min = new LevelGenerator().MinDistance(movesCount, LevelGenerator.DistanceFunc);

            //var desiredCount = (int) (max - min + 1);
            //int desiredTime = 10000;

            //            Stopwatch sw = new Stopwatch();
//            sw.Start();
            bool stop = false;

            Func<BoardCoordinate, BoardCoordinate, bool> areConnectedFunc =
               (x1, x2) => LevelGenerator.DistanceFunc(x1, x2) <= 2;

            var list = new List<LevelData>(10000);
            Func<HashSet<BoardCoordinate>, HashSet<BoardCoordinate>, double> movesCollectionDelta =
                (set1, set2) => ((set1.Union(set2)).Except(set1.Intersect(set2))).Count();
            IDifficultyAssigner assigner = new ClusteringDifficultyAssigner(areConnectedFunc);

            Task task = Task.Run(() =>
            {
                while (!stop)
                {
                    movesCount = movesCountFunc();
                    var boardCoordinates =
                        new List<BoardCoordinate>(randomUniqueGenerator.Generate(boardSize, movesCount));
                    double newdiff = assigner.Assign(boardCoordinates);


//
                    var combinations = new Combinations<BoardCoordinate>(boardCoordinates, 2);
                    var heuristicsCollection =
                        combinations.Select(pair => LevelGenerator.DistanceFunc(pair[0], pair[1])).ToList();
//
                    double sumDistances = heuristicsCollection.Sum();
//
                    var newSet = new HashSet<BoardCoordinate>(boardCoordinates);
                    if (list.All(x => movesCollectionDelta(newSet, x.MovesSet) >= (movesCount + x.MovesCount)/2.0))
//
                    {
                        var difficuty = ((int) difficultyHeuristic(heuristicsCollection));
                        list.Add(new LevelData(newSet, difficuty, movesCount, (int) newdiff));
                        Console.WriteLine(list.Count);
                    }
                }
            });
            Console.ReadKey();
            stop = true;
            task.Wait();

            SaveLevelsToDB(list, boardSize);
        }
        private static void SaveLevelsToDB(List<LevelData> list, int boardSize)
        {
            var levelsDataContext = new LevelsDataContext();
            levelsDataContext.Levels.InsertAllOnSubmit(list.Select(x =>
                new Level
                {
                    Difficulty = x.DifficultyHeuristic,
                    DistancesSum = x.DistaceSum,
                    IsUsed = false,
                    BoardSize = boardSize,
                    MovesCount = x.MovesCount,
                    Moves =
                        MovesCollectionFormatter.SerializeMovesCollection(
                            x.MovesSet.ToList().Select(BoardCooridnateConvetert.FromBoardCoordinate).ToList())
                }));
            levelsDataContext.SubmitChanges();
        }

        internal class LevelData
        {
            public HashSet<BoardCoordinate> MovesSet { get; set; }
            public int DifficultyHeuristic { get; set; }
            public int MovesCount { get; set; }
            public int DistaceSum { get; set; }

            public LevelData(HashSet<BoardCoordinate> movesSet, int difficultyHeuristic, int movesCount, int distaceSum)
            {
                MovesSet = movesSet;
                DifficultyHeuristic = difficultyHeuristic;
                MovesCount = movesCount;
                DistaceSum = distaceSum;
            }
        }
    }




    public class LevelsExtractor
    {
        private readonly Func<int, int> _difficultyForLevel;
        private readonly Func<int, bool> _movesCountPredicate;
        private readonly IEnumerable<Level> _source;
        private Random _random = new Random();
        public LevelsExtractor(Func<int, int> difficultyForLevel, Func<int, bool> movesCountPredicate,
            IEnumerable<Level> source)
        {
            _difficultyForLevel = difficultyForLevel;
            _movesCountPredicate = movesCountPredicate;
            _source = source;
        }

       

        public Collection<Level> Extract(int count)
        {
            var dictionary = _source.ToLookup(x => x.Difficulty, x => new LevelAndUsage {Level = x, IsUsed = false});
            var result = new List<Level>(count);

            for (int i = 0; i < count; i++)
            {
                int requestedDiff = _difficultyForLevel(i);
                //   int requesteMovesCount = _movesCountForLevel(i);
                if (!dictionary.Contains(requestedDiff))
                    throw new InvalidOperationException("Cannot find level with difficulty: " + requestedDiff);
                var levels = dictionary[requestedDiff].Where(x => !x.IsUsed)
                    .Where(x => _movesCountPredicate(x.Level.MovesCount)).ToList();
                var difficulties = levels.Select(x => x.Level.Difficulty)
                    .Distinct()
                    .ToList();

                var level = levels.First(x=> x.Level.Difficulty == difficulties[_random.Next(difficulties.Count)]);
                if (level == null)
                    throw new InvalidOperationException(
                        "There is no level that satisfy given predicate for difficulty " + requestedDiff);

                result.Add(level.Level);
                level.IsUsed = true;                
            }
            return new Collection<Level>(result);
        }
    }
    class LevelAndUsage
    {
        public Level Level { get; set; }
        public bool IsUsed { get; set; }
    }


    public class LevelsExtractorWithDisabledAreas
    {
        private Func<int, int> _difficultyForLevel;
        private Func<int, bool> _movesCountPredicate;
        private IEnumerable<Level> _source;
        private readonly Random _random = new Random();
        private readonly IEnumerable<DisabledAreas> _disabledAreases;
        private NeiborghoodFinder _neiborghoodFinder;

        private readonly Func<IList<SingleMove>, IList<SingleMove>, bool> _predicateFunc;
        
        private static readonly int upperLimit = 100;
        private  bool IsNeighbour(SingleMove disbaledArea, SingleMove move)
        {
            return
                _neiborghoodFinder.GetNeiborghood(new BoardCoordinate(move.X, move.Y))
                    .Contains(new BoardCoordinate(disbaledArea.X, disbaledArea.Y));
        }

        public LevelsExtractorWithDisabledAreas(Func<int, int> difficultyForLevel, Func<int, bool> movesCountPredicate, IEnumerable<Level> source, IEnumerable<DisabledAreas> disabledAreases, 
            int boardSize)
        {
            if (difficultyForLevel == null) throw new ArgumentNullException("difficultyForLevel");
            if (movesCountPredicate == null) throw new ArgumentNullException("movesCountPredicate");
            if (source == null) throw new ArgumentNullException("source");
            if (disabledAreases == null) throw new ArgumentNullException("disabledAreases");
            if (boardSize < 1) throw new ArgumentOutOfRangeException("boardSize");

            _difficultyForLevel = difficultyForLevel;
            _movesCountPredicate = movesCountPredicate;
            _source = source;
            _disabledAreases = disabledAreases;
            _neiborghoodFinder = new NeiborghoodFinder(boardSize);


            _predicateFunc =
                (movesCoordinates, disbaleAreasCoordinates) =>
                    !movesCoordinates.Intersect(disbaleAreasCoordinates).Any()
                    &&
                    movesCoordinates.Sum(
                        move => disbaleAreasCoordinates.Count(disbaledArea => IsNeighbour(disbaledArea, move))) < upperLimit;
        }
        public Collection<Level> Extract(int count)
        {
            var dictionary = _source.ToLookup(x => x.Difficulty, x => new LevelAndUsage { Level = x, IsUsed = false });
            var result = new List<Level>(count);

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("Current step: {0}",i);
                int requestedDiff = _difficultyForLevel(i);
                //   int requesteMovesCount = _movesCountForLevel(i);
                if (!dictionary.Contains(requestedDiff))
                    throw new InvalidOperationException("Cannot find level with difficulty: " + requestedDiff);
                var levels = dictionary[requestedDiff].Where(x => !x.IsUsed)
                    .Where(x => _movesCountPredicate(x.Level.MovesCount)).ToList();
                var difficulties = levels.Select(x => x.Level.Difficulty)
                    .Distinct()
                    .ToList();

                var levelsWithDifficulty = levels.Where(x => x.Level.Difficulty == difficulties[_random.Next(difficulties.Count)]);
                if (!levelsWithDifficulty.Any())
                    throw new InvalidOperationException(
                        "There is no level that satisfy given predicate for difficulty " + requestedDiff);
                             

                var selectMany = levelsWithDifficulty.SelectMany(
                    level => _disabledAreases.Where(disabled => _predicateFunc(MovesCollectionFormatter.ParseString(level.Level.Moves),
                                                                              MovesCollectionFormatter.ParseString(disabled.Coordinates))),
                    (levelandusage, areas) => new {Level = levelandusage, DisabledAreas = areas})
                                            .ToList();

                if (!selectMany.Any())
                    throw new InvalidOperationException(
                        "No level that can use any of the disabed areas");
                var levelChosen = selectMany[_random.Next(selectMany.Count)];
                result.Add(levelChosen.Level.Level);

                levelChosen.Level.IsUsed = true;
                levelChosen.Level.Level.DisbaledAreasId = levelChosen.DisabledAreas.Id;
            }
            return new Collection<Level>(result);
        }
    }
    public class LevelsExporter
    {
        private readonly LevelsDataContext _sourceContext;
        private readonly LevelsContext _targetContext;
        private readonly bool _markAsUsed;
        public LevelsExporter(LevelsDataContext sourceContext, LevelsContext targetContext,bool markAsUsed)
        {
            _sourceContext = sourceContext;
            _targetContext = targetContext;
            _markAsUsed = markAsUsed;
        }
        public void Export(IEnumerable<Level> levelsToEport, int targetGroupId, int boardSize)
        {
            var toExport = levelsToEport as IList<Level> ?? levelsToEport.ToList();
            _targetContext.LevelData.InsertAllOnSubmit(toExport.Select((x, index) =>
                new LevelData
                {
                    BoardSize = (byte) boardSize,
                    Difficulty = x.Difficulty,
                    OrderNo = index,
                    DisplayName = (index + 1).ToString(),
                    LevelGroupId = targetGroupId,
                    DisabledAreasId = x.DisbaledAreasId,
                    Moves = x.Moves,
                    MovesCount = x.MovesCount                    
                }));
            _targetContext.SubmitChanges();

            if (_markAsUsed)
            {
                foreach (var level in toExport)
                {
                    level.IsUsed = true;
                }
            }
            _sourceContext.SubmitChanges();
        }
    }
}