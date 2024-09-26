using System;

namespace NoNameGame.CustomControls.OverlayAnimatedBackground
{
    public struct NewShapeAppearanceTime
    {
        private TimeSpan _minimum;
        private TimeSpan _maximum;

        public TimeSpan Minimum
        {
            get { return _minimum; }
            private set { _minimum = value; }
        }
        public TimeSpan Maximum
        {
            get { return _maximum; }
            private set { _maximum = value; }
        }

        public NewShapeAppearanceTime(TimeSpan minimum, TimeSpan maximum)
        {
            if (maximum < minimum) throw new ArgumentException("maximum", "maximum must be grater or equal minimum");            
            _minimum = minimum;
            _maximum = maximum;
        }
    }
}