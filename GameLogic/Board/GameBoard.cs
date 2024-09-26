using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GameLogic.Areas;
using GameLogic.BoardSetuper;
using GameLogic.Game;
using GameLogic.MovesSequentionGenerators;
using GameLogic.WinVerifiers;

namespace GameLogic.Board
{    
    public class GameBoard
    {
        private readonly IWinVerifier _vierifier;
        private readonly List<Area> _recordedAffectedAreas = new List<Area>();       
        public Level Level { get; private set; }
        private AreaMatrix _areaMatrix;

        public BoardSize Size
        {
            get { return _areaMatrix.Size; }            
        }

        public AreaMatrix AreaMatrix
        {
            get { return _areaMatrix; }
        }

        public List<BoardCoordinate> WinningMoves
        {
            get { return Level.WinningMovesSequention.ToList(); }           
        }

        public GameBoard(Level level, IWinVerifier vierifier)            
        {
            if (level == null) throw new ArgumentNullException("level");
            if (vierifier == null) throw new ArgumentNullException("vierifier");

            _vierifier = vierifier;
            Level = level;
            Setup();
        }

        private void Setup()
        {
            _areaMatrix = _vierifier.CreateWinningBoard(Level.BoardSize);
            Level.WinningMovesSequention.AsEnumerable().Reverse().ToList().ForEach(
                boardCoordinate => _areaMatrix.Areas[boardCoordinate.X, boardCoordinate.Y].OnEnter());
        }

        public static GameBoard Restore(IWinVerifier verifier,BoardData boardData)
        {
            var board = new GameBoard(new Level(boardData.WinningMoves, boardData.Size,boardData.LevelId),verifier)
            {
                _areaMatrix = new AreaMatrix(boardData.Size)
            };

            for (int i = 0; i < boardData.Size; i++)
            {
                for (int j = 0; j < boardData.Size; j++)
                {
                     board._areaMatrix.InsertArea(boardData.AreasData[i][j].AreaState,i,j);
                }
            }            
            return board;
        }

        public BoardData Serialize()
        {
            var boardData = new BoardData();
            boardData.AreasData = new List<List<AreaData>>();                     
            for (int i = 0; i < AreaMatrix.Areas.GetLength(0); i++)
            {
                boardData.AreasData.Add(new List<AreaData>());
                for (int j = 0; j < AreaMatrix.Areas.GetLength(1); j++)
                {
                    boardData.AreasData[i].Add(new AreaData() {AreaState = AreaMatrix.Areas[i, j].AreaState});
                }
            }
            boardData.LevelId = Level.Id;
            boardData.Size = Size;
            boardData.WinningMoves = Level.WinningMovesSequention.ToList();
            return boardData;
        }       

//        public BoardCoordinate GetAreaCoordinates(Area area)
//        {
//            if (area == null) throw new ArgumentNullException("area");
//            if(!AreaMatrix.Areas.OfType<Area>().Contains(area))
//                throw new ArgumentException("area","area not belongs to this board");
//            return area.BoardCoordinate;
//        }
        public Area GetArea(BoardCoordinate coordiante)
        {
            if (!coordiante.IsValidOnBoard(this.Size))
                throw new ArgumentException("Coordinate is not valid for this board", "coordiante");

            return AreaMatrix.Areas[coordiante.X, coordiante.Y];
        }

        public void OnEnter(BoardCoordinate areaCoordinate)
        {
            _recordedAffectedAreas.AddRange(AreaMatrix.Areas[areaCoordinate.X, areaCoordinate.Y].OnEnter());
        }

        public void ClearRecordedAffectedAreas()
        {
            _recordedAffectedAreas.Clear();
        }
        public IEnumerable<BoardCoordinate> GetRecordedAffectedAreas()
        {
            return _recordedAffectedAreas.Select(x => x.BoardCoordinate);
        }
        public float GetCompletenessRatio()
        {
            var allAreas = AreaMatrix.Areas.OfType<Area>().ToList();
            return (float)allAreas.Count(area => area.AreaState == AreaState.Checked) / (Size * Size - allAreas.Count(area => area.AreaState == AreaState.Disabled));
        }
    }
}
