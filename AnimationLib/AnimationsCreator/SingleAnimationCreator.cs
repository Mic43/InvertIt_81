using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator.Interfaces;

namespace AnimationLib.AnimationsCreator
{
    public class SingleAnimationCreator : IUIElementAnimationCreator
    {
        private readonly UIElementAnimation _animation;

        public SingleAnimationCreator(UIElementAnimation animation)
        {
            if (animation == null) throw new ArgumentNullException("animation");
            _animation = animation;
        }

        public UIElementAnimation Animation
        {
            get { return _animation; }
        }

        public Storyboard Create(UIElement uiElement)
        {
            var storyboard = new Storyboard();

            Animation.AnimationInitAction(uiElement);            
            foreach (var propPath in Animation.PropertyAnimation.PropertyPaths)
            {
                var animation = Animation.PropertyAnimation.Timeline();
                
                storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, uiElement);
                Storyboard.SetTargetProperty(animation, propPath);
            }            
            return storyboard;
        }    
    }

  
}