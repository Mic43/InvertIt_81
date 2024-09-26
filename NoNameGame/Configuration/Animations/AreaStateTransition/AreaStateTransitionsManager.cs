using System;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using GameLogic.Areas;
using NoNameGame.Models;

namespace NoNameGame.Configuration.Animations.AreaStateTransition
{
    public class AreaStateTransitionsManager : IAreaStateTransitionsManager
    {
        public IUIElementAnimationCreator ToCheckStateAnimation { get; private set; }
        public IUIElementAnimationCreator ToUnCheckStateAnimation { get; private set; }
        public AreaStateTransitionsManager(IUIElementAnimationCreator toCheckStateAnimation, 
            IUIElementAnimationCreator toUnCheckStateAnimation)
        {
            if (toCheckStateAnimation == null) throw new ArgumentNullException("toCheckStateAnimation");
            if (toUnCheckStateAnimation == null) throw new ArgumentNullException("toUnCheckStateAnimation");

            ToCheckStateAnimation = toCheckStateAnimation;
            ToUnCheckStateAnimation = toUnCheckStateAnimation;
        }
        public IUIElementAnimationCreator GetAnimationCreatorForArea(AreaModel area)
        {
            if (area.AreaState == AreaState.Checked)
                return ToCheckStateAnimation;
            else if (area.AreaState == AreaState.UnChecked)
                return ToUnCheckStateAnimation;
            else
                return new EmptyAnimationCreator(); 
        }      
    }
}