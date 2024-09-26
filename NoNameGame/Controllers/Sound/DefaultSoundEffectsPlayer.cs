using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace NoNameGame.Controllers.Sound
{
    public sealed class DefaultSoundEffectsPlayer : SoundEffectsPlayer ,IDisposable
    {        
        private readonly string SoundsDirectory = @"Resources\Sounds";
        private SoundEffect OpenEffect(string fileName)
        {
            return SoundEffect.FromStream(TitleContainer.OpenStream(Path.Combine(SoundsDirectory, fileName)));
        }
        protected override void LoadEffects()
        {
            _newMoveEffect = OpenEffect( "bubble3.wav");            
            _clickEffect = OpenEffect("Click.wav");
            _backClickEffect = OpenEffect("backClick.wav");
            _swypeEffect = OpenEffect("swype3_low.wav");
            _gameWonEffect = OpenEffect(@"EndGame\success.wav");
            _newGameEffect = OpenEffect("cartoon_pop.wav");
            _gameLoadingEffect = OpenEffect("gameLoading.wav");
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