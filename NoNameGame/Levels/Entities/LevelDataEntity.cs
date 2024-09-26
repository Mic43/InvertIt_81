using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NoNameGame.Levels.Entities
{
    public class LevelDataEntity
    {
        private int levelGroupId;
        private int _Id;      
        private int _Difficulty;
        private List<Point> _Moves;        
        private string _DisplayName;
        private int _OrderNo;
        private byte _BoardSize;
        private byte? _tutorialStep;
        public int Id
        {
            get { return _Id; }
        }       
        public int Difficulty
        {
            get { return _Difficulty; }
        }
        public List<Point> Moves
        {
            get { return _Moves; }
        }
        public string DisplayName
        {
            get { return _DisplayName; }
        }
        public int OrderNo
        {
            get { return _OrderNo; }
        }
        public byte BoardSize
        {
            get { return _BoardSize; }      
        }
        public byte? TutorialStep
        {
            get { return _tutorialStep; }
        }
        public int LevelGroupId
        {
            get { return levelGroupId; }
        }

        public LevelDataEntity(int id, int difficulty, List<Point> moves, string displayName, int orderNo, byte boardSize, byte? tutorialStep, int levelGroupId)
        {
            if (moves == null) throw new ArgumentNullException("moves");
            
            _Id = id;            
            _Difficulty = difficulty;
            _Moves = moves;
            _DisplayName = displayName;
            _OrderNo = orderNo;
            _BoardSize = boardSize;
            _tutorialStep = tutorialStep;
            this.levelGroupId = levelGroupId;
        }
    }
}