using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator.Interfaces;

namespace AnimationLib.AnimationsCreator
{
    public class SimultanousAnimationsCreator : IUIElementAnimationCreator
    {
        private readonly IEnumerable<IUIElementAnimationCreator> _creators;

        public SimultanousAnimationsCreator(IEnumerable<IUIElementAnimationCreator> creators )
        {
            if (creators == null) throw new ArgumentNullException("creators");
            _creators = creators;
        }

        public SimultanousAnimationsCreator(params IUIElementAnimationCreator[] creators)
            : this(creators.AsEnumerable())
        {
            
        }
        
        public Storyboard Create(UIElement uiElement)
        {
            var returnStoryBoard = new Storyboard();

            var storyboards = _creators.Select(creator => creator.Create(uiElement)).ToList();
            storyboards.ForEach(storyboard => returnStoryBoard.Children.Add(storyboard));

            return returnStoryBoard;
        }
    }
}