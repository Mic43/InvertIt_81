using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace AnimationLib.AnimationsCreator.MutliAnimation
{
    public class EmptyGenericMultiAnimationCreator : IGenericMultiAnimationCreator
    {
        public Storyboard Create(IEnumerable<UIElement> elements)
        {
            return new Storyboard();
        }
    }
}