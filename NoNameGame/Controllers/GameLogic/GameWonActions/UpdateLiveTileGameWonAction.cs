using System;
using System.Linq;
using System.Threading.Tasks;
using GameLogic.Game;
using Microsoft.Phone.Shell;
using NoNameGame.Controllers.PlayerStats;
using NoNameGame.Resources;

namespace NoNameGame.Controllers.GameLogic.GameWonActions
{
    public class UpdateLiveTileGameWonAction : IGameWonAction
    {
        private readonly IPlayerStatsProvider _playerStatsProvider;
        public UpdateLiveTileGameWonAction(IPlayerStatsProvider playerStatsProvider )
        {
            if (playerStatsProvider == null) throw new ArgumentNullException("playerStatsProvider");
            _playerStatsProvider = playerStatsProvider;
        }
        public void Execute(GameWonData gameWonData)
        {
            Task.Run(() =>
            {
                ShellTile shellTile = ShellTile.ActiveTiles.First();

                var flipTileData = new FlipTileData
                {
                    BackTitle = "",
                    WideBackContent =
                        string.Format("{0}: {1}%", AppResources.UpdateLiveTileGameWonAction_BackTitle,
                            GetGameProgressPercentage())
                };
//            var flipTileData = new FlipTileData
//            {
//                //WideBackContent = AppResources.UpdateLiveTileGameWonAction_BackContent_Temp,
//                WideBackContent = AppResources.UpdateLiveTileGameWonAction_BackContent_Temp + Environment.NewLine + string.Format(AppResources.UpdateLiveTileGameWonAction_BackTitle_Temp, gameWonData.WonGameStats.Points)
//            };
                shellTile.Update(flipTileData);
            });
        }
        private int GetGameProgressPercentage()
        {
            double progress = _playerStatsProvider.GetCurrentPlayerStarsCount()/
                              (double) _playerStatsProvider.GetAllLevelsStarsCount();
            int progressPercentage = (int)(progress*100);
            return progressPercentage;
        //    return 0;
        }
    }
}