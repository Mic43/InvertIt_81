using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator.Interfaces;

namespace AnimationLib.AnimationsCreator.MutliAnimation
{
    public class RandomMultiAnimationCreator : CompositeMultiAnimationCreator
    {
        private readonly Random _random = new Random();
        public RandomMultiAnimationCreator(IEnumerable<IMultiUIElementsAnimationCreator> childCreators)
            : base(childCreators)
        {
        }
        public override Storyboard Create(UIElement[,] elementsToAnimate)
        {
            if (elementsToAnimate == null) throw new ArgumentNullException("elementsToAnimate");

            int randomIndex = _random.Next(_childCreators.Count());
            return _childCreators.ElementAt(randomIndex).Create(elementsToAnimate);
        }
    }
}