using System;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration.Animations.Periodic.Interfaces;

namespace NoNameGame.Configuration.Animations.Periodic
{
    public class SamplePeriodicAnimationFactory : IPeriodicAnimationTypeFactory
    {
        private readonly TimeSpan _animationDuration = TimeSpan.FromMilliseconds(250);
        public IUIElementAnimationCreator Create()
        {
//            return new SimultanousAnimationsCreator(
//                new SingleAnimationCreator(
//                    AnimationsRepository.CreateBounceAnimation(10,
//                        _animationDuration,false)));
            return new SimultanousAnimationsCreator(
                new SingleAnimationCreator(
                    AnimationsRepository.CreateRotationProjectionAnimation(_animationDuration)));
        }
    }
}