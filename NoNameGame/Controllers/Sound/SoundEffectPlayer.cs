using Microsoft.Xna.Framework.Audio;

namespace NoNameGame.Controllers.Sound
{
    public abstract class SoundEffectsPlayer
    {
        private static readonly object padlock = new object();
        private static SoundEffectsPlayer _current;

        protected SoundEffect _newMoveEffect;
        protected SoundEffect _clickEffect;
        protected SoundEffect _backClickEffect;
        protected SoundEffect _swypeEffect;
        protected SoundEffect _gameWonEffect;
        protected SoundEffect _newGameEffect;
        protected SoundEffect _gameLoadingEffect;    

        public static SoundEffectsPlayer Current
        {
            get
            {
                lock (padlock)
                {
                    if (_current == null)
                    {
                        _current = new DefaultSoundEffectsPlayer();
                        _current.LoadEffects();
                    }
                    return _current;
                }
            }
            set
            {
                lock (padlock)
                {
                    _current = value;
                    _current.LoadEffects();
                }                
            }            
        }
        public SoundEffect NewMoveEffect
        {
            get { return _newMoveEffect; }
        }
        public SoundEffect ClickEffect
        {
            get { return _clickEffect; }
        }
        public SoundEffect BackClickEffect
        {
            get { return _backClickEffect; }
        }
        public SoundEffect SwypeEffect
        {
            get { return _swypeEffect; }
        }
        public SoundEffect GameWonEffect
        {
            get { return _gameWonEffect; }
        }
        public SoundEffect NewGameEffect
        {
            get { return _newGameEffect; }
        }
        public SoundEffect GameLoadingEffect
        {
            get { return _gameLoadingEffect; }
        }
        protected abstract void LoadEffects();                            
    }
}