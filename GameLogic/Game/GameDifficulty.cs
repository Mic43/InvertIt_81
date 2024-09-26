using System.Runtime.Serialization;

namespace GameLogic.Game
{
    [DataContract]
    [KnownType(typeof(UltraEasyDifficulty))]
    [KnownType(typeof(EasyDifficulty))]
    [KnownType(typeof(HardDifficulty))]
    [KnownType(typeof(MediumDifficulty))]
    public abstract class GameDifficulty
    {
        private int movesCount;

        public int MovesCount
        {
            get { return movesCount; }        
        }
        public GameDifficulty(int movesCount)
        {
            this.movesCount = movesCount;
        }
                    
    }
    [DataContract]
    public class UltraEasyDifficulty : GameDifficulty
    {
        public UltraEasyDifficulty()
            : base(1)
        {
        }
    }
    [DataContract]
    public class EasyDifficulty : GameDifficulty
    {
        public EasyDifficulty(): base(3)
        {
        }
    }
    [DataContract]
    public class MediumDifficulty : GameDifficulty
    {
        public MediumDifficulty()
            : base(5)
        {
        }
    }
    [DataContract]
    public class HardDifficulty : GameDifficulty
    {
        public HardDifficulty()
            : base(10)
        {
        }
    }
}
