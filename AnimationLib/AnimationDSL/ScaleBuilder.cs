using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AnimationLib.AnimationDSL.Helpers;

namespace AnimationLib.AnimationDSL
{
    public class ScaleBuilder : IUIElementAnimationBuilder
    {
        private DoubleAnimationBuilder _animationBuilder;
        private readonly PropertyPath[] _propertyPaths = {PropertyPathsRepository.ScaleX,PropertyPathsRepository.ScaleY};

        private readonly Action<UIElement> _scaleTransformInitAction = (element) =>
        {
            if (!(element.RenderTransform is ScaleTransform))
            {
                element.RenderTransform = new ScaleTransform();
            }            
            element.RenderTransformOrigin = new Point(0.5, 0.5);
        };

        public DoubleAnimationBuilder Uniform()
        {
            _animationBuilder = new DoubleAnimationBuilder(new DoubleAnimation(), this);           
            return _animationBuilder;
        }
        public UIElementAnimation Build()
        {
            return new UIElementAnimation(_scaleTransformInitAction,
                new PropertyAnimation(() =>  AnimationCloner.Clone(_animationBuilder.GetValue()), _propertyPaths));
        }

        public ScaleBuilder()
        {
            _animationBuilder = new DoubleAnimationBuilder(new DoubleAnimation() { Duration = TimeSpan.Zero }, this);
        }
    }
}