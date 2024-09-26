using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using AnimationLib.AnimationsCreator.Interfaces;

namespace AnimationLib.AnimationsCreator
{
    //w pizdu nie działa :/
    public class SequentialAnimationCreator : IUIElementAnimationCreator
    {
           private readonly IEnumerable<IUIElementAnimationCreator> _creators;

        public SequentialAnimationCreator(IEnumerable<IUIElementAnimationCreator> creators )
        {
            if (creators == null) throw new ArgumentNullException("creators");
            _creators = creators;
        }

        public SequentialAnimationCreator(params IUIElementAnimationCreator[] creators)
            : this(creators.AsEnumerable())
        {
            
        }
        
        public Storyboard Create(UIElement uiElement)
        {
            var returnStoryBoard = new Storyboard();

            var containedStoryboards = _creators.Select(creator => creator.Create(uiElement)).ToList();
            for (int i = 1; i < containedStoryboards.Count; i++)
            {
                containedStoryboards[i].BeginTime += containedStoryboards[i - 1].BeginTime +
                                                     containedStoryboards[i - 1].Children.Select(GetAnimationTimeSpan)
                                                         .Aggregate((acc, newtimeline) => acc.Add(newtimeline));

            }
            foreach (var storyboard in containedStoryboards)
            {
                returnStoryBoard.Children.Add(storyboard);
            }
            
            return returnStoryBoard;
        }
        private TimeSpan GetAnimationTimeSpan(Timeline timeline)
        {
            if (timeline.Duration.HasTimeSpan)
                return timeline.Duration.TimeSpan;
            else if(timeline is Storyboard)
            {
                var storyboard = ((Storyboard) timeline);
                storyboard.SkipToFill();
                TimeSpan lenght = storyboard.GetCurrentTime();
                storyboard.Seek(TimeSpan.Zero);
               // storyboard.Stop();
                return lenght;
            }
            else
            {
                throw new InvalidOperationException("Timeline must be iether animation or StoryBoard");
            }
        }
    }
}