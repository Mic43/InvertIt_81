using System;
using System.Windows;

namespace AnimationLib
{
    public class UIElementAnimation
    {
        private Action<UIElement> _animationInitAction;
        private PropertyAnimation _propertyAnimation;

        public UIElementAnimation(Action<UIElement> animationInitAction, PropertyAnimation propertyAnimation)
        {
            if (animationInitAction == null) throw new ArgumentNullException("animationInitAction");
            if (propertyAnimation == null) throw new ArgumentNullException("propertyAnimation");

            _animationInitAction = animationInitAction;
            _propertyAnimation = propertyAnimation;
        }

        public Action<UIElement> AnimationInitAction
        {
            get { return _animationInitAction; }
        }

        public PropertyAnimation PropertyAnimation
        {
            get { return _propertyAnimation; }
        }
    }
}