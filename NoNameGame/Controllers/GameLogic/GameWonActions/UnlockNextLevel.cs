using System.Linq;
using GameLogic.Game;
using NoNameGame.Levels;

namespace NoNameGame.Controllers.GameLogic.GameWonActions
{
    public class UnlockNextLevel : IGameWonAction
    {
        private readonly ILevelProvider _levelProvider;
        private readonly ILevelProgressStorer _progressStorer;
        public UnlockNextLevel(ILevelProvider levelProvider,ILevelProgressStorer progressStorer)
        {
            _levelProvider = levelProvider;
            _progressStorer = progressStorer;        
        }
        public void Execute(GameWonData gameWonData)
        {
            _levelProvider.GetNextLevel(gameWonData.PlayedLevelId).ToList().ForEach(lev =>
            {
                var levelProgressEntity = _progressStorer.Load(lev.Id);
                levelProgressEntity.IsAvailable = true;
                _progressStorer.Save(levelProgressEntity);
            });            
        }
    }
}