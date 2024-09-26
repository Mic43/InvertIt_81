using System.Collections.Generic;
using NoNameGame.Controllers.PeriodicAnimations;
using NoNameGame.Controllers.Unlocks.Actions;

namespace NoNameGame.Configuration.Animations.Periodic
{

    public class AnimationDirectionDataDictionary : Dictionary<AnimationDirectionType, AnimationDirectionData>
    {
        public AnimationDirectionDataDictionary()
        {
            Add(AnimationDirectionType.Arc, new AnimationDirectionData(false));
            Add(AnimationDirectionType.Circle, new AnimationDirectionData(false));
            Add(AnimationDirectionType.Diagonal, new AnimationDirectionData(false));
            Add(AnimationDirectionType.Horizontal, new AnimationDirectionData(false));
            Add(AnimationDirectionType.Square, new AnimationDirectionData(false));
            Add(AnimationDirectionType.Vertical, new AnimationDirectionData(false));
            Add(AnimationDirectionType.Linear, new AnimationDirectionData(false));
        }
    }
}