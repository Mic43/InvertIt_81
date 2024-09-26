using System.Windows;
using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator.Interfaces;

namespace AnimationLib.AnimationsCreator
{
    public class EmptyAnimationCreator : IUIElementAnimationCreator
    {
        public Storyboard Create(UIElement uiElement)
        {
            return new Storyboard();
        }
    }
}