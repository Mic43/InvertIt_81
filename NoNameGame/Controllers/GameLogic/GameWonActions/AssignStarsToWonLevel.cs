using System;
using GameLogic.Game;
using NoNameGame.Levels;
using NoNameGame.Levels.Entities;


namespace NoNameGame.Controllers.GameLogic.GameWonActions
{
    public class AssignStarsToWonLevel : IGameWonAction
    {
        private readonly ILevelProgressStorer _levelProvider;
        public AssignStarsToWonLevel(ILevelProgressStorer levelProvider)
        {
            _levelProvider = levelProvider;
        }
        public void Execute(GameWonData gameWonData)
        {
            var playedLevel = _levelProvider.Load(gameWonData.PlayedLevelId);
            
            playedLevel.Stars = Math.Max(gameWonData.WonGameStats.Points, playedLevel.Stars);

            _levelProvider.Save(playedLevel);            
        }
    }
}