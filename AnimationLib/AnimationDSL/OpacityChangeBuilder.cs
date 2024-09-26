using System;
using System.Windows;
using System.Windows.Media.Animation;
using AnimationLib.AnimationDSL.Helpers;

namespace AnimationLib.AnimationDSL
{
    public class OpacityChangeBuilder: IUIElementAnimationBuilder
    {
        private DoubleAnimationBuilder _animationBuilder;
        private readonly PropertyPath[] _propertyPaths = {PropertyPathsRepository.Opacity};    

        public DoubleAnimationBuilder Start()
        {
            _animationBuilder = new DoubleAnimationBuilder(new DoubleAnimation(), this);           
            return _animationBuilder;
        }
        public UIElementAnimation Build()
        {
            return new UIElementAnimation(element => {},
                new PropertyAnimation(() =>  AnimationCloner.Clone(_animationBuilder.GetValue()), _propertyPaths));
        }

        public OpacityChangeBuilder()
        {
            _animationBuilder = new DoubleAnimationBuilder(new DoubleAnimation() { Duration = TimeSpan.Zero }, this);
        }
       
    }
}