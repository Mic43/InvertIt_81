using System.Collections;
using System.Windows;
using System.Windows.Media.Animation;

namespace AnimationLib.AnimationsCreator.Interfaces
{
    public interface IMultiUIElementsAnimationCreator
    {
        Storyboard Create(UIElement[,] elementsToAnimate);
    }
}