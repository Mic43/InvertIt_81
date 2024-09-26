using AnimationLib;
using GameLogic.Board;

namespace NoNameGame.Controllers.PeriodicAnimations
{
    public delegate IAnimationDelayFunction AnimationDelayCreator(BoardSize boardSize);
}