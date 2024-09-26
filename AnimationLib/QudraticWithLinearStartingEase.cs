using System;
using System.Windows.Media.Animation;

namespace AnimationLib
{
    public class QudraticWithLinearStartingEase :EasingFunctionBase
    {
        private float _initialOffset = 0.2f;
        private double _linearCoefficent = 0.8;
        public double LinearCoefficent
        {
            get { return _linearCoefficent; }
            set { _linearCoefficent = value; }
        }
        public float QuadraticOffset
        {
            get { return _initialOffset; }
            set
            {
                if (value > 1.0f || value <0.0f)
                    throw new ArgumentOutOfRangeException("value must fall between 0.0 and 1.0");
                _initialOffset = value;
            }
        }
        protected override double EaseInCore(double normalizedTime)
        {
            double qudraticPart = Math.Max((normalizedTime - QuadraticOffset), 0.0f);
            return LinearCoefficent*normalizedTime + qudraticPart*qudraticPart;
        }
    }
}