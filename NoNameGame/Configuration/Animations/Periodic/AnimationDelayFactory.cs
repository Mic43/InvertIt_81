using System;
using AnimationLib;
using GameLogic.Board;
using NoNameGame.Configuration.Animations.Periodic.Interfaces;
using NoNameGame.Controllers.PeriodicAnimations;

namespace NoNameGame.Configuration.Animations.Periodic
{
    class CurrentAnimationDelayProvier : ICurrentAnimationDelayProvider
    {
        private AnimationDirectionController controller;
        public CurrentAnimationDelayProvier(AnimationDirectionController controller)
        {
            if (controller == null) throw new ArgumentNullException("controller");
            this.controller = controller;
        }

        public IAnimationDelayFunction Get(BoardSize boardSize)
        {
            return controller.CurrentAnimationDiretion.AnimationDelayFactory(boardSize);
        }
    }
}