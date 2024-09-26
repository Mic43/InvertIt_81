using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator.Interfaces;

namespace AnimationLib.AnimationsCreator.MutliAnimation
{
    public class SimultanousMultiAnimationCreator : CompositeMultiAnimationCreator
    {
        public SimultanousMultiAnimationCreator(IEnumerable<IMultiUIElementsAnimationCreator> childCreators)
            : base(childCreators)
        {
        }
        public SimultanousMultiAnimationCreator(params IMultiUIElementsAnimationCreator[] childCreators)
            : base(childCreators)
        {
        }
        public override Storyboard Create(UIElement[,] elementsToAnimate)
        {
            var returnStoryBoard = new Storyboard();

            var storyboards = _childCreators.Select(creator => creator.Create(elementsToAnimate)).ToList();
            storyboards.ForEach(storyboard => returnStoryBoard.Children.Add(storyboard));

            return returnStoryBoard;
        }
    }
}