using System;
using System.Windows.Media.Animation;

namespace AnimationLib.AnimationDSL
{  
    public class TimelineBuilder
    {
        private readonly Timeline _timeline;
        private readonly IUIElementAnimationBuilder _root;

        public TimelineBuilder AutoReverse()
        {
            _timeline.AutoReverse = true;
            return this;
        }
        public TimelineBuilder WithRepeatBehaviour(RepeatBehavior repeatBehavior)
        {
            _timeline.RepeatBehavior = repeatBehavior;
            return this;
        }
        public TimelineBuilder RepeatForever()
        {
            return WithRepeatBehaviour(RepeatBehavior.Forever);
        }
        public IUIElementAnimationBuilder WithDuration(TimeSpan duration)
        {
            _timeline.Duration= duration;
            return _root;
        }
        public IUIElementAnimationBuilder WithDuration(int miliseconds)
        {
            return WithDuration(TimeSpan.FromMilliseconds(miliseconds));
        }
        public TimelineBuilder WithBeginTime(TimeSpan timeSpan)
        {
            _timeline.BeginTime = timeSpan;
            return this;
        }
        public TimelineBuilder WithBeginTime(int miliseconds)
        {
            return WithBeginTime(TimeSpan.FromMilliseconds(miliseconds));
        }  
        public TimelineBuilder(Timeline timeLine, IUIElementAnimationBuilder root)
        {
            if (timeLine == null) throw new ArgumentNullException("timeLine");
            if (root == null) throw new ArgumentNullException("root");
            _timeline = timeLine;
            _root = root;
        }
    }

}