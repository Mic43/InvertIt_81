using System.Windows;
using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator.Interfaces;

namespace AnimationLib.AnimationsCreator.MutliAnimation
{
    public class EmptyMultiAnimationCreator : IMultiUIElementsAnimationCreator
    {
        public Storyboard Create(UIElement[,] elementsToAnimate)
        {
            return new Storyboard();
        }
    }
}