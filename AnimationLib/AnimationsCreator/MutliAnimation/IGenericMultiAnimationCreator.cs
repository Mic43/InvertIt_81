using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace AnimationLib.AnimationsCreator.MutliAnimation
{
    public interface IGenericMultiAnimationCreator
    {
        Storyboard Create(IEnumerable<UIElement> elements);
    }
}