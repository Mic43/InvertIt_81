using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using GameLogic.Areas;
using NoNameGame.Models;

namespace NoNameGame.Configuration.Animations.AreaStateTransition
{
    public class EmptyAreaStateTransitionsManager : IAreaStateTransitionsManager
    {
        public IUIElementAnimationCreator GetAnimationCreatorForArea(AreaModel area)
        {
            return new EmptyAnimationCreator();
        }
    }
}