using System;
using System.Windows;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Board;
using NoNameGame.Controllers.GameLogic;

namespace NoNameGame.Configuration.Animations.Reset
{
    public class EmptyResetAnimationFactory : IResetAnimationFactory
    {
        public IGenericMultiAnimationCreator CreateAnimationCreator(Func<UIElement, BoardCoordinate> areaVisualisationCoordinateFunc)
        {
            return new EmptyGenericMultiAnimationCreator();
        }
    }
}