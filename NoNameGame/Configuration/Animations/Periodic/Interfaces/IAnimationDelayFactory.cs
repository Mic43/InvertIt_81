using AnimationLib;
using GameLogic.Board;

namespace NoNameGame.Configuration.Animations.Periodic.Interfaces
{
    public interface ICurrentAnimationDelayProvider
    {
        IAnimationDelayFunction Get(BoardSize boardSize);
    }
}