using AnimationLib.AnimationsCreator.Interfaces;
using GameLogic.Areas;
using NoNameGame.Models;

namespace NoNameGame.Configuration.Animations.AreaStateTransition
{
    public interface IAreaStateTransitionsManager
    {
        IUIElementAnimationCreator GetAnimationCreatorForArea(AreaModel area);
    }
}