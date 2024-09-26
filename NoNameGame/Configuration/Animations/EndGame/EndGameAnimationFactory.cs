using System;
using System.Collections.Generic;
using System.Windows;
using AnimationLib;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Board;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.BoardPresentation.Animations.CircularTransaltion;

namespace NoNameGame.Configuration.Animations.EndGame
{
    class EndGameAnimationFactory: IEndGameAnimationFactory
    {
        public IMultiUIElementsAnimationCreator CreateAnimationCreator(BoardCoordinate circleCenter)
        {
            const int initialDelayMs = 0;

//            Func<int, int, IUIElementAnimationCreator> func = (x, y) =>
//            {
//                Point vectorToCenter = new Point(x - circleCenter.X, y - circleCenter.Y);
//                if (vectorToCenter.X ==0 && vectorToCenter.Y==0)
//                    return new EmptyAnimationCreator();
//
//                const int translationLen = 20;
//
//                double xPow2 = vectorToCenter.X*vectorToCenter.X;
//                double yPow2 = vectorToCenter.Y * vectorToCenter.Y;
//
//                Point ortogonal = new Point(Math.Sqrt(yPow2 / (xPow2 + yPow2)),
//                                            Math.Sqrt(xPow2 / (xPow2 + yPow2)));
//              
//                double dy = 2 * translationLen * ortogonal.X * ortogonal.Y / (ortogonal.X * ortogonal.X + ortogonal.Y * ortogonal.Y);
//                double dx = Math.Sqrt(translationLen * translationLen - dy * dy);
//                int duration = 1000;
//
//                return new SimultanousAnimationsCreator(
//                    new SingleAnimationCreator(
//                        AnimationBuilder.Translate().Horizontal().To(dx).AutoReverse().WithDuration(duration).Build()),
//                    new SingleAnimationCreator(
//                        AnimationBuilder.Translate().Vertical().To(dy).AutoReverse().WithDuration(duration).Build()));
//
//            };
//            return new MultiAnimationCreator(func,
//                SteppingAnimationDelayFuncion.CreateCircular(TimeSpan.FromMilliseconds(1000), (Point) circleCenter));


            var creators = new List<IMultiUIElementsAnimationCreator>()
            {
                new CircularConstantTranslation(100).Build(circleCenter,initialDelayMs),
                new IncreasingLenghtCircularTranslation(20).Build(circleCenter,initialDelayMs),
                new InvertedAsixCircularTranslation(new CircularConstantTranslation(100)).Build(
                    circleCenter,initialDelayMs),                     
                new MultiAnimationCreator(
                    new SingleAnimationCreator(AnimationsRepository.CreateExpandShrinkAnimation(0,
                        TimeSpan.FromMilliseconds(500))),
                    SteppingAnimationDelayFuncion.CreateCircular(TimeSpan.FromMilliseconds(100),
                        (Point) circleCenter).SetInitialDelay(TimeSpan.FromMilliseconds(initialDelayMs))),            
            };
            return new RandomMultiAnimationCreator(creators);
        }
    }
}