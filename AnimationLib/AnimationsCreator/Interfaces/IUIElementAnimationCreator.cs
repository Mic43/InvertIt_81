using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace AnimationLib.AnimationsCreator.Interfaces
{
    public interface IUIElementAnimationCreator
    {
        Storyboard Create(UIElement uiElement);
    }
    
}