using System;
using System.Windows;

namespace NoNameGame.BoardPresentation.Animations.CircularTransaltion
{
    public class CircularConstantTranslation : CircularTranslationMutliAnimation
    {
        private readonly int _translationLenght;
        public CircularConstantTranslation(int translationLenght)
        {
            _translationLenght = translationLenght;
        }
        public override Func<DistanceFromCircleCenter, Point> DistanceFromCenterToTranslationVector
        {
            get
            {
                return distance =>
                {                    
                    if (distance.Dx == 0 && distance.Dy == 0)
                        return new Point(0, 0);

                    double coeff =
                        Math.Sqrt((double) _translationLenght/(distance.Dx*distance.Dx + distance.Dy*distance.Dy));
                    return new Point(coeff*distance.Dx, coeff*distance.Dy);
                };
            }
        }
    }
}