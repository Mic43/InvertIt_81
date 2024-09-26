using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AnimationLib.AnimationDSL.Helpers;

namespace AnimationLib.AnimationDSL
{
    public class TranslationBuilder :IUIElementAnimationBuilder
    {
        private PropertyPath _propertyPath = PropertyPathsRepository.TranslateX;
        private DoubleAnimationBuilder _animationBuilder;
        private readonly Action<UIElement> _animationInit = element =>
        {
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            if (!(element.RenderTransform is TranslateTransform))
                element.RenderTransform = new TranslateTransform() { };
        };
     

        public DoubleAnimationBuilder Vertical()
        {
            _animationBuilder = new DoubleAnimationBuilder(new DoubleAnimation(),this);
            _propertyPath = PropertyPathsRepository.TranslateY; 
            return _animationBuilder;
        }
        public DoubleAnimationBuilder Horizontal()
        {
            _animationBuilder = new DoubleAnimationBuilder(new DoubleAnimation(),this);
            _propertyPath = PropertyPathsRepository.TranslateX; 
            return _animationBuilder;
        }
        public UIElementAnimation Build()
        {            
            return new UIElementAnimation(_animationInit,
                new PropertyAnimation(() => AnimationCloner.Clone(_animationBuilder.GetValue()), _propertyPath));
        }
        public TranslationBuilder()
        {
            _animationBuilder = new DoubleAnimationBuilder(new DoubleAnimation() { Duration = TimeSpan.Zero },this);
        }
       
    }
}