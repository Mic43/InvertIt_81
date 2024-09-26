using System;
using Infrastructure;
using Infrastructure.Storage;
using NoNameGame.Controllers.PeriodicAnimations;
using NoNameGame.Helpers;

namespace NoNameGame.Controllers.Unlocks.Actions
{
    public class UnlockAnimationDirectionAction : IAction
    {
        private readonly AnimationDirectionType _animationDirectionType;
        private readonly IFiniteTypeStorer<AnimationDirectionType, AnimationDirectionData> _unlocker;
        public UnlockAnimationDirectionAction(AnimationDirectionType animationDirectionType,
            IFiniteTypeStorer<AnimationDirectionType, AnimationDirectionData> unlocker)
        {
            if (unlocker == null) throw new ArgumentNullException("unlocker");

            _animationDirectionType = animationDirectionType;
            _unlocker = unlocker;
        }
        public void Perform()
        {
            _unlocker.Write(_animationDirectionType, new AnimationDirectionData(false));
        }
    }
}