using System.Windows;
using GameLogic.Game;

namespace NoNameGame.Controllers.GameLogic.GameWonActions
{
    public interface IGameWonAction
    {
        void Execute(GameWonData gameWonData);
    }
}