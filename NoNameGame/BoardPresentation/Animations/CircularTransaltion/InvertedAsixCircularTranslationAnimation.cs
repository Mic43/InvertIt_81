using System;
using System.Windows;

namespace NoNameGame.BoardPresentation.Animations.CircularTransaltion
{
    public class InvertedAsixCircularTranslation : CircularTranslationMutliAnimation
    {
        private readonly CircularTranslationMutliAnimation _circularTranslationAnimation;
        public InvertedAsixCircularTranslation(CircularTranslationMutliAnimation circularTranslationAnimation)
        {
            if (circularTranslationAnimation == null) throw new ArgumentNullException("circularTranslationAnimation");
            _circularTranslationAnimation = circularTranslationAnimation;
        }
        public override Func<DistanceFromCircleCenter, Point> DistanceFromCenterToTranslationVector
        {
            get
            {
                return
                    distance =>
                        _circularTranslationAnimation.DistanceFromCenterToTranslationVector(
                            new DistanceFromCircleCenter(distance.Dy, distance.Dx));
            }
        }
    }
}