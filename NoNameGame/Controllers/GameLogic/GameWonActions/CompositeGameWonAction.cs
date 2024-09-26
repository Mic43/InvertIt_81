using System;
using GameLogic.Game;

namespace NoNameGame.Controllers.GameLogic.GameWonActions
{
    public class CompositeGameWonAction : IGameWonAction
    {
        private readonly IGameWonAction[] _gameWonActions;
        public CompositeGameWonAction(params IGameWonAction[] gameWonActions)
        {
            if (gameWonActions == null) throw new ArgumentNullException("gameWonActions");
            _gameWonActions = gameWonActions;
        }
        public void Execute(GameWonData gameWonData)
        {
            foreach (var gameWonAction in _gameWonActions)
            {
                gameWonAction.Execute(gameWonData);    
            }
        }
    }
}