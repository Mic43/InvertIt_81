using System.Windows;
using NoNameGame.Controllers.Sound;

namespace NoNameGame.Controllers
{
    public class EasterEggActivator
    {
        private int _hitCount = 0;
        private const int ActivateHitCount = 5;
        private const int DeactivateHitCount = 5;
        public void Hit()
        {
            _hitCount++;
            if (_hitCount == ActivateHitCount)
            {
                Activate();
                MessageBox.Show(
                    string.Format("You found easter egg :P. Tap another {0} times to bring back more appropriate sounds :P",
                        DeactivateHitCount));
            }
            else if (_hitCount == ActivateHitCount + DeactivateHitCount)
            {
                Deactivate();
                _hitCount = 0;
            }
        }
        private void Deactivate()
        {
            SoundEffectsPlayer.Current = new DefaultSoundEffectsPlayer();
        }
        private void Activate()
        {
            SoundEffectsPlayer.Current = new EasterEggSoundEffectsPlayer(); 
        }
    }
}