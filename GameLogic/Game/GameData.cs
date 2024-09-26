using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using GameLogic.Areas;
using GameLogic.Board;

namespace GameLogic.Game
{

    [DataContract]
    public class GameData
    {
        [DataMember]
        public BoardData BoardData { get; set; }       
              
        [DataMember]
        public List<BoardCoordinate> UndoMoves { get; set; }
        [DataMember]
        public List<BoardCoordinate> RedoMoves { get; set; }
        [DataMember]
        public GameState State { get; set; }

        [DataMember]
        public long ElapsedGameTimeInMs { get; set; }
    }

    [DataContract]
    public class BoardData
    {     
        [DataMember]
        public List<List<AreaData>> AreasData { get; set; }
        [DataMember]
        public int Size { get; set; }
        [DataMember]
        public List<BoardCoordinate> WinningMoves { get; set; }
        [DataMember]
        public int LevelId { get; set; }
    }

    [DataContract]
    public class AreaData
    {
        [DataMember]
        public AreaState AreaState { get; set; }        
    }
}