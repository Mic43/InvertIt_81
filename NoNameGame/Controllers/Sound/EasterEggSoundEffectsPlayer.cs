using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace NoNameGame.Controllers.Sound
{
    public class EasterEggSoundEffectsPlayer : SoundEffectsPlayer,IDisposable
    {
        private readonly SoundEffectsPlayer _defaultEffectsPlayer;
        private readonly string SoundsDirectory = @"Resources\Sounds\EasterEgg";
//        public EasterEggSoundEffectsPlayer(SoundEffectsPlayer defaultEffectsPlayer)
//        {
//            _defaultEffectsPlayer = defaultEffectsPlayer;
//        }
        private SoundEffect OpenEffect(string fileName)
        {
            return SoundEffect.FromStream(TitleContainer.OpenStream(Path.Combine(SoundsDirectory, fileName)));
        }
        protected override void LoadEffects()
        {
            _newMoveEffect = OpenEffect("fart.wav");
            _clickEffect = OpenEffect("berk.wav");
            _backClickEffect = OpenEffect("berk2.wav");
            _swypeEffect = OpenEffect("fart2.wav");
            _gameWonEffect = OpenEffect("fart_end.wav");
            _newGameEffect = OpenEffect("berk.wav");
            
        }
        public void Dispose()
        {
            _newMoveEffect.Dispose();
            _clickEffect.Dispose();
            _backClickEffect.Dispose();
            _swypeEffect.Dispose();
            _gameWonEffect.Dispose();
            _newGameEffect.Dispose();
        }
    }
}