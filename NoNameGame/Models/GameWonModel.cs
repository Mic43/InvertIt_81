using System.Runtime.Serialization;
using GameLogic.Board;

namespace NoNameGame.Models
{
    [DataContract]
    public class GameWonModel
    {
        public GameWonModel(bool isThemeUnlocked,BoardCoordinate playerLastMove)
        {
            IsThemeUnlocked = isThemeUnlocked;
            PlayerLastMove = playerLastMove;
        }
        public GameWonModel()
        {            
        }

        [DataMember]
        public bool IsThemeUnlocked { get;  set; }
         [DataMember]
        public BoardCoordinate PlayerLastMove { get;  set; }
    }

    public class LevelGroupFinishedModel 
    {
        public LevelGroupFinishedModel(bool isFinished)
        {
            
        }
    }
}