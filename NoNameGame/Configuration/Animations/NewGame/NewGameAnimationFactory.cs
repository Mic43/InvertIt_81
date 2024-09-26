using System;
using System.Windows.Media.Animation;
using AnimationLib;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using AnimationLib.AnimationsCreator.MutliAnimation;
using GameLogic.Board;

namespace NoNameGame.Configuration.Animations.NewGame
{
    public class NewGameAnimationFactory 
    {       
        public IMultiUIElementsAnimationCreator CreateAnimationCreator(bool showAnimation, double hostHeight, BoardSize boardSize)
        {
            if (!showAnimation)
                return new EmptyMultiAnimationCreator();

            return new MultiAnimationCreator(
                    new SingleAnimationCreator(
                        AnimationBuilder.Translate().Vertical()
                                        .WithEasingFunction(new QuadraticEase() { EasingMode = EasingMode.EaseInOut })
                                        .From(-hostHeight).To(0)
                                        .WithDuration(700).Build()),
                        SteppingAnimationDelayFuncion.CreateLinear(TimeSpan.FromMilliseconds(20), boardSize,
                                                                   true));
        }
    }
}