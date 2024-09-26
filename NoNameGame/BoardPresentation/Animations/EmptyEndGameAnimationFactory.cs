using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Board;

namespace NoNameGame.BoardPresentation.Animations
{
    public class EmptyEndGameAnimationFactory : IEndGameAnimationFactory
    {
        public IMultiUIElementsAnimationCreator CreateAnimationCreator(BoardCoordinate circleCenter)
        {
            return new EmptyMultiAnimationCreator();
        }
    }
}