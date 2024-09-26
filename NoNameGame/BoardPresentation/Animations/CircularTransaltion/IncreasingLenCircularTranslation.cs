using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace NoNameGame.BoardPresentation.Animations.CircularTransaltion
{
    public class IncreasingLenghtCircularTranslation : CircularTranslationMutliAnimation
    {
        private readonly int _startingTranslationLenght;
        public IncreasingLenghtCircularTranslation(int startingTranslationLenght)
        {
            _startingTranslationLenght = startingTranslationLenght;            
        }
        public override Func<DistanceFromCircleCenter, Point> DistanceFromCenterToTranslationVector
        {
            get { return (distance) => new Point(distance.Dx * _startingTranslationLenght, distance.Dy * _startingTranslationLenght); }
        }
    }
}