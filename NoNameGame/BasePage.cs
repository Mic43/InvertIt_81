using System.ComponentModel;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NoNameGame.Controllers.Sound;

namespace NoNameGame
{
    public class BasePage : PhoneApplicationPage
    {     
        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            SoundEffectsPlayer.Current.BackClickEffect.Play();
            base.OnBackKeyPress(e);
        }   
    }
}