using System.Windows.Media.Animation;

namespace AnimationLib.AnimationDSL.Helpers
{
    internal class AnimationCloner
    {
        public static DoubleAnimation Clone(DoubleAnimation source)
        {
            return new DoubleAnimation()
            {
                AutoReverse = source.AutoReverse,
                BeginTime = source.BeginTime,
                EasingFunction = source.EasingFunction,
                FillBehavior = source.FillBehavior,
                From = source.From,
                RepeatBehavior = source.RepeatBehavior,
                To = source.To,
                Duration = source.Duration,
                By = source.By,
                SpeedRatio = source.SpeedRatio
            };
        }
    }
}