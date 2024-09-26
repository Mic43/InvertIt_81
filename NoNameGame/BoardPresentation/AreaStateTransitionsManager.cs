using System;
using System.Windows.Shapes;
using AnimationLib.AnimationsCreator.Interfaces;
using GameLogic.Areas;

namespace NoNameGame.BoardPresentation
{
    public class AreaStateTransitionsManager
    {
        public IUIElementAnimationCreator ToCheckStateAnimation { get; private set; }
        public IUIElementAnimationCreator ToUnCheckStateAnimation { get; private set; }
        public AreaStateTransitionsManager(IUIElementAnimationCreator toCheckStateAnimation, IUIElementAnimationCreator toUnCheckStateAnimation)
        {
            if (toCheckStateAnimation == null) throw new ArgumentNullException("toCheckStateAnimation");
            if (toUnCheckStateAnimation == null) throw new ArgumentNullException("toUnCheckStateAnimation");

            ToCheckStateAnimation = toCheckStateAnimation;
            ToUnCheckStateAnimation = toUnCheckStateAnimation;
        }
        public void BeginAnimation(Area area,Shape areaVisualisation)
        {
            if(area.Checked)
                ToCheckStateAnimation.Create(areaVisualisation).Begin();
            else
                ToUnCheckStateAnimation.Create(areaVisualisation).Begin();                            
        }
    }
}