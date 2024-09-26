using AnimationLib.AnimationsCreator.Interfaces;
using GameLogic.Board;

namespace NoNameGame.Controllers.PeriodicAnimations
{
    public interface IPeriodicAnimationFactory
    {
        IMultiUIElementsAnimationCreator Create(BoardSize boardSize);
    }
}