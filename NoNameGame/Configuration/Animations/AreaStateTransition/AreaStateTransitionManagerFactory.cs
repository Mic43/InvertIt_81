using System;
using AnimationLib;
using AnimationLib.AnimationsCreator;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Controllers.Themes;

namespace NoNameGame.Configuration.Animations.AreaStateTransition
{
    public class AreaStateTransitionManagerFactory : IAreaStateTransitionManagerFactory
    {
        private readonly TimeSpan _animationDuration = TimeSpan.FromMilliseconds(300);
        public AreaStateTransitionsManager Create(Theme theme)
        {
            UIElementAnimation toUnCheckedStateTransition = null;
            UIElementAnimation toCheckedStateTransition = null;

            if (!theme.IsFlat)
            {
                toUnCheckedStateTransition =
                    AnimationsRepository.CreateRadialGradientColorAnimation(GameResources.Instance.UnCheckedColor,
                        _animationDuration);

                toCheckedStateTransition =
                    AnimationsRepository.CreateRadialGradientColorAnimation(GameResources.Instance.CheckedColor,
                        _animationDuration);
                return
                    new AreaStateTransitionsManager(
                        new SingleAnimationCreator(toCheckedStateTransition),
                        new SingleAnimationCreator(toUnCheckedStateTransition));

            }
            else
            {
                TimeSpan animationHalfTime = TimeSpan.FromMilliseconds(150);
                toUnCheckedStateTransition =
                    AnimationsRepository.CreateFillColorDiscreteAnimation(GameResources.Instance.UnCheckedColor,
                        animationHalfTime);

                toCheckedStateTransition =
                    AnimationsRepository.CreateFillColorDiscreteAnimation(GameResources.Instance.CheckedColor,
                        animationHalfTime);

                return
                    new AreaStateTransitionsManager(
                        new SimultanousAnimationsCreator(
                            new SingleAnimationCreator(toCheckedStateTransition),
                            new SingleAnimationCreator(AnimationsRepository.CreateStrokeThicknessDiscreteAnimation(0,animationHalfTime)),
                            new SingleAnimationCreator(
                                AnimationsRepository.CreateRotationProjectionAnimation(_animationDuration))),
                        new SimultanousAnimationsCreator(
                            new SingleAnimationCreator(toUnCheckedStateTransition),
                            new SingleAnimationCreator(AnimationsRepository.CreateStrokeThicknessDiscreteAnimation(Constants.GameShapeStrokeWidth, animationHalfTime)),
                            new SingleAnimationCreator(
                                AnimationsRepository.CreateRotationProjectionAnimation(_animationDuration))));
            }
        }
    }
}