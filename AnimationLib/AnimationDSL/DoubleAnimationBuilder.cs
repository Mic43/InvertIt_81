using System;
using System.Windows.Media.Animation;

namespace AnimationLib.AnimationDSL
{
    public class DoubleAnimationBuilder
    {
        private readonly DoubleAnimation _animation;
        private readonly IUIElementAnimationBuilder _rootAnimationBuilder;
        public TimelineBuilder To(double value)
        {        
            _animation.To = value;
            return new TimelineBuilder(_animation, _rootAnimationBuilder);
        }
        public DoubleAnimationBuilder WithEasingFunction(IEasingFunction function)
        {
            _animation.EasingFunction = function;
            return this;
        }
        public DoubleAnimationBuilder From(double value)
        {
            _animation.From = value;
            return this;
        }

        public DoubleAnimationBuilder(DoubleAnimation animation,IUIElementAnimationBuilder rootAnimationBuilder)
        {
            if (animation == null) throw new ArgumentNullException("animation");
            if (rootAnimationBuilder == null) throw new ArgumentNullException("rootAnimationBuilder");
            _animation = animation;
            _rootAnimationBuilder = rootAnimationBuilder;
        }

        public DoubleAnimation GetValue()
        {
            return _animation;
        }
    }
}