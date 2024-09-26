using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator.Interfaces;
using GameLogic.Board;

namespace NoNameGame.BoardPresentation.Animations
{
    public interface IEndGameAnimationFactory
    {
        IMultiUIElementsAnimationCreator CreateAnimationCreator(BoardCoordinate circleCenter);
    }
}