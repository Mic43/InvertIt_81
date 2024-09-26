using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator.Interfaces;

namespace AnimationLib.AnimationsCreator.MutliAnimation
{
    public abstract class CompositeMultiAnimationCreator : IMultiUIElementsAnimationCreator
    {
        protected readonly IEnumerable<IMultiUIElementsAnimationCreator> _childCreators;
        public IEnumerable<IMultiUIElementsAnimationCreator> ChildCreators
        {
            get { return _childCreators; }
        }
        protected CompositeMultiAnimationCreator(IEnumerable<IMultiUIElementsAnimationCreator> childCreators)
        {
            if (childCreators == null) throw new ArgumentNullException("childCreators");
            _childCreators = childCreators;
        }
        public abstract Storyboard Create(UIElement[,] elementsToAnimate);
    }
}