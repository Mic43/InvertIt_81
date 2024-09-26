using System;
using System.Windows;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Board;
using NoNameGame.Controllers.GameLogic;

namespace NoNameGame.Configuration.Animations.Reset
{
    public interface IResetAnimationFactory
    {
        IGenericMultiAnimationCreator CreateAnimationCreator(Func<UIElement,BoardCoordinate> areaVisualisationCoordinateFunc);
    }
}