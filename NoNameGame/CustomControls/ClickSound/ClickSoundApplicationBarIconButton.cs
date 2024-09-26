using System;
using Microsoft.Phone.Shell;
using NoNameGame.Controllers.Sound;

namespace NoNameGame.CustomControls.ClickSound
{
    public class ClickSoundApplicationBarIconButton : ApplicationBarIconButton
    {
        public ClickSoundApplicationBarIconButton(Uri uri) :base(uri)
        {
            Click += (sender, args) => SoundEffectsPlayer.Current.ClickEffect.Play();
        }                
    }
}