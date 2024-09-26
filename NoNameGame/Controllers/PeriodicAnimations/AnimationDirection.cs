using System;

namespace NoNameGame.Controllers.PeriodicAnimations
{
    public class AnimationDirection
    {
        public AnimationDelayCreator AnimationDelayFactory { get; set; }
        public AnimationDirectionType AnimationDirectionType { get; set; }
        public string Name { get; set; }
        public AnimationDirection(AnimationDelayCreator animationDelayFactory, AnimationDirectionType animationDirectionType, string name)
        {
            if (animationDelayFactory == null) throw new ArgumentNullException("animationDelayFactory");
            if (name == null) throw new ArgumentNullException("name");

            AnimationDelayFactory = animationDelayFactory;
            AnimationDirectionType = animationDirectionType;
            Name = name;
        }
    }
}