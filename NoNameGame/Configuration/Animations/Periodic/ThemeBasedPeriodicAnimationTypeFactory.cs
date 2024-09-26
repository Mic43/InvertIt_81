using System;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationsCreator.Interfaces;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration.Animations.Periodic.Interfaces;
using NoNameGame.Controllers.Themes;

namespace NoNameGame.Configuration.Animations.Periodic
{
    public class ThemeBasedPeriodicAnimationTypeFactory : IPeriodicAnimationTypeFactory
    {
        private readonly TimeSpan _singleAnimationDuration = TimeSpan.FromMilliseconds(350);
        private readonly ThemeController _controller;
        public ThemeBasedPeriodicAnimationTypeFactory(ThemeController controller)
        {
            if (controller == null) throw new ArgumentNullException("controller");
            _controller = controller;
        }
        public IUIElementAnimationCreator Create()
        {
            var currentTheme = _controller.CurrentTheme;

            if (currentTheme.IsFlat)
                return
                    new SingleAnimationCreator(
                        AnimationBuilder.Scale()
                            .Uniform()
                            .From(1.0)
                            .To(1.07)
                            .AutoReverse()
                            .WithDuration(_singleAnimationDuration)
                            .Build());                    

            return new SimultanousAnimationsCreator(
                new SingleAnimationCreator(
                    AnimationsRepository.CreateRadialGradientOffsetAnimation(0.10,
                        _singleAnimationDuration)),
                new SingleAnimationCreator(AnimationsRepository.CreateRadialGradientRadiusAnimation(
                    0.8,
                    _singleAnimationDuration)));
        }
    }
}