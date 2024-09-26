using System;
using GameLogic.Game;
using NoNameGame.Levels;


namespace NoNameGame.Controllers.GameLogic.GameWonActions
{
    class AssignSolveTimeAction : IGameWonAction
    {
        private readonly ILevelProgressStorer _levelProvider;
        public AssignSolveTimeAction(ILevelProgressStorer levelProvider)
        {
            if (levelProvider == null) throw new ArgumentNullException("levelProvider");
            _levelProvider = levelProvider;
        }
        public void Execute(GameWonData gameWonData)
        {
            var playedLevel = _levelProvider.Load(gameWonData.PlayedLevelId);

            if (playedLevel.FirstSolveDuration == TimeSpan.Zero)
                playedLevel.FirstSolveDuration = gameWonData.WonGameStats.SolveTime;
            _levelProvider.Save(playedLevel);
        }
    }
}