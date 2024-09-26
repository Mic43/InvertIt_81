using System;
using System.Windows;
using AnimationLib;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Board;

namespace NoNameGame.BoardPresentation.Animations.CircularTransaltion
{
    public abstract class CircularTranslationMutliAnimation
    {
        public abstract Func<DistanceFromCircleCenter, Point> DistanceFromCenterToTranslationVector { get; }        
        public  IMultiUIElementsAnimationCreator Build(BoardCoordinate circleCenter,int delayMs = 0)
        {
            Func<int, int, IUIElementAnimationCreator> func =
                (x, y) =>
                {
                    TimeSpan duration = TimeSpan.FromMilliseconds(400);

                    Point translationLen = DistanceFromCenterToTranslationVector(
                        new DistanceFromCircleCenter(x - circleCenter.X,y - circleCenter.Y));                  

                    return new SimultanousAnimationsCreator(
                        new SingleAnimationCreator(AnimationsRepository.CreateHorizontalTranslationAnimation(translationLen.X,
                            duration, 0, true)),
                        new SingleAnimationCreator(AnimationsRepository.CreateVerticalTranslationAnimation(translationLen.Y,
                            duration, 0, true)));
                };

            return new MultiAnimationCreator(func,
                SteppingAnimationDelayFuncion.CreateCircular(TimeSpan.FromMilliseconds(100),
                    new Point(circleCenter.X, circleCenter.Y)).SetInitialDelay(TimeSpan.FromMilliseconds(delayMs)));
        }
    }
}