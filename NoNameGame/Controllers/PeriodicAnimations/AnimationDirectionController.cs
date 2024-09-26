using System;
using System.Collections.Generic;
using Infrastructure;
using Infrastructure.Storage;
using NoNameGame.Configuration.Animations.Periodic;
using NoNameGame.Helpers;

namespace NoNameGame.Controllers.PeriodicAnimations
{
    public class AnimationDirectionController
    {       
        private readonly AnimationDirectionDictionary _animationDirectionDictionary;

        public AnimationDirection CurrentAnimationDiretion
        {
            get
            {
                return _animationDirectionDictionary[AppSettingsAccessor.GetValueOrDefault("SelectedAnimationDirection",
                    AnimationDirectionDictionary.DefaultAnimationDirection)];
            }
        }
       
        public AnimationDirectionController(AnimationDirectionDictionary animationDirectionDictionary)
        {
            _animationDirectionDictionary = animationDirectionDictionary; 
        }

        public void ChangeAnimationDirection(AnimationDirectionType animationDirectionType)
        {
            if (!_animationDirectionDictionary.ContainsKey(animationDirectionType))
                throw new ArgumentException("Invalid animation direction type", "newThemeType");

            AppSettingsAccessor.AddOrUpdateValue("SelectedAnimationDirection", animationDirectionType);
            AppSettingsAccessor.Save();  
                   
        }
        public IEnumerable<AnimationDirection> GetAllDirections()
        {
            return _animationDirectionDictionary.Values;
        }

    }
}