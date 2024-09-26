using Microsoft.Phone.Shell;
using NoNameGame.Controllers.Sound;

namespace NoNameGame.CustomControls.ClickSound
{
    public class ClickSoundApplicationBarMenuItem : ApplicationBarMenuItem
    {
        public ClickSoundApplicationBarMenuItem(string text) : base(text)
        {
            Click += (sender, args) => SoundEffectsPlayer.Current.ClickEffect.Play();
        }      
    }
}