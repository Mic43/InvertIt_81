using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Web.UI;
using InvertItService.DataObjects;

namespace InvertItService.Logic
{
    public interface ICurrentChallengesProvider
    {
        IEnumerable<Challenge> Get();
    }

    class ConstantChallengesProvider : ICurrentChallengesProvider
    {
        private static readonly List<Challenge> Challenges = new List<Challenge>()
        {
            new Challenge(Guid.NewGuid(), "1", Difficulty.Easy, PointsCalculatorType.Normal,
                new Board(7, "(3,3)", 1)),
            new Challenge(Guid.NewGuid(), "2", Difficulty.Medium, PointsCalculatorType.Normal,
                new Board(7, "(3,3)", 1)),
            new Challenge(Guid.NewGuid(), "3", Difficulty.Hard, PointsCalculatorType.Normal,
                new Board(7, "(3,3)", 1)),
        };
        public IEnumerable<Challenge> Get()
        {
            return Challenges;
        }
    }
    class FilteringChallengesProvider : ICurrentChallengesProvider
    {
        private ICurrentChallengesProvider _challengesProvider;
        private IChallengesFilterer _challengesFilterer;

        public FilteringChallengesProvider(ICurrentChallengesProvider challengesProvider, IChallengesFilterer challengesFilterer)
        {
            if (challengesProvider == null) throw new ArgumentNullException("challengesProvider");
            _challengesProvider = challengesProvider;
            _challengesFilterer = challengesFilterer;
        }
        public IEnumerable<Challenge> Get()
        {
            return _challengesProvider.Get().Where(challenge => !_challengesFilterer.IsFilteredOut(challenge));
        }
    }

    internal interface IChallengesFilterer
    {
        bool IsFilteredOut(Challenge challenge);
    }

    class IByCurrentUserGames : IChallengesFilterer
    {
        private readonly IGamesProvider _provider;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly Func<IEnumerable<Game>, bool> _condtition;
        public IByCurrentUserGames(IGamesProvider provider, ICurrentUserProvider currentUserProvider, 
                                  Func<IEnumerable<Game>, bool> condtition)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            _provider = provider;
            _currentUserProvider = currentUserProvider;
            _condtition = condtition;
        }
        public bool IsFilteredOut(Challenge challenge)
        {
            return _condtition(_provider.Get(x => x.Challenge == challenge && x.UserAccount == _currentUserProvider.Get()));
        }
    }

    internal interface ICurrentUserProvider
    {
        UserAccount Get();
    }

//    class AlreadyPlayed : IChallengesFilterer
//    {
//        public AlreadyPlayed(IGamesProvider )
//        {
//            
//        }
//        public bool IsFilteredOut(Challenge challenge)
//        {
//            throw new NotImplementedException();
//        }
//    }

    internal interface IGamesProvider
    {
        IEnumerable<Game> Get(Func<Game,bool> filter );
    }


    public interface IAllChallengesProvider
    {
        IEnumerable<Challenge> Get();
    }

    


   public class Game
    {
        public virtual UserAccount UserAccount { get;  set; }
        public Guid Id { get;  set; }
        public virtual Challenge Challenge { get;  set; }
        public int PointsAwarded { get;  set; }
        public GameState GameState { get; set; }
        public int MovesCount { get;  set; }
        public TimeSpan PlayDuration { get;  set; }
        
        
        private Game()
        {
            GameState = GameState.Started;
        }

       public Game(UserAccount userAccount, Guid id, Challenge challenge, int pointsAwarded, GameState gameState, int movesCount, TimeSpan playDuration)
       {
           UserAccount = userAccount;
           Id = id;
           Challenge = challenge;
           PointsAwarded = pointsAwarded;
           GameState = gameState;
           MovesCount = movesCount;
           PlayDuration = playDuration;
       }
    }

    public enum GameState
    {
        Started,Finished
    }

    public class Challenge
    {
        public Guid Id { get;  set; }
        public string Name { get;  set; }
        public Difficulty Difficulty { get;  set; }
        public PointsCalculatorType PointsCalculatorType { get;  set; }
        public virtual Board Board { get;  set; }

        public virtual ICollection<Game> Games { get; set; }

        private Challenge()
        {
            Board = new Board(0,string.Empty,0);
            Games = new List<Game>();
        }
        public Challenge(Guid id, string name, Difficulty difficulty, PointsCalculatorType pointsCalculatorType, Board board)
        {
            Id = id;
            Name = name;
            Difficulty = difficulty;
            PointsCalculatorType = pointsCalculatorType;
            Board = board;
            Games = new List<Game>();
        }
    }

    public enum PointsCalculatorType
    {
        Normal
    }

    public enum Difficulty
    {
        Easy,Medium,Hard
    }

    [ComplexType]
    public class Board
    {
        public int Size { get; set; }
        public string MovesCollection { get; set; }
        public int MovesCount { get; set; }

        private Board()
        {
            
        }
        public Board(int size, string movesCollection, int movesCount)
        {
            Size = size;
            MovesCollection = movesCollection;
            MovesCount = movesCount;
        }
    }
}