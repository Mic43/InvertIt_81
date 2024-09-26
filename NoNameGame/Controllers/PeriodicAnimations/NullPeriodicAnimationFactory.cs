using AnimationLib.AnimationsCreator.Interfaces;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Board;

namespace NoNameGame.Controllers.PeriodicAnimations
{
    public class NullPeriodicAnimationFactory : IPeriodicAnimationFactory
    {
        public IMultiUIElementsAnimationCreator Create(BoardSize boardSize)
        {
            return new EmptyMultiAnimationCreator();
        }
    }
}