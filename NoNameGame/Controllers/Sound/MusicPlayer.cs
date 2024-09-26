using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace NoNameGame.Controllers.Sound
{
    public static class MusicPlayer 
    {
        private const string MenuMusic = "menu_music";
        private static Song _menuMusic;
        private static DispatcherTimer _dispatcherTimer = new DispatcherTimer();
        private static bool _isFadingOut = false;
        private const int FadeResolution = 10;
        private static float _fadeInitialVolume;
        private static float _fadeDelta;
        
        static MusicPlayer()
        {          
            MediaPlayer.IsRepeating = true;
            _dispatcherTimer.Tick+=DispatcherTimerOnTick;
        }
        private static void DispatcherTimerOnTick(object sender, EventArgs eventArgs)
        {
            if (MediaPlayer.Volume > 0)
                MediaPlayer.Volume -= _fadeDelta;
            else
            {
                _dispatcherTimer.Stop();
                Stop();
                MediaPlayer.Volume = _fadeInitialVolume;
                _isFadingOut = false;
            }
        }

        public static void FadeOut(int fadeDurationMs)
        {
            if (_isFadingOut)
                return;

            _isFadingOut = true;
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(fadeDurationMs/(double) FadeResolution);
            _fadeInitialVolume = MediaPlayer.Volume;
            _fadeDelta = _fadeInitialVolume/FadeResolution;
            _dispatcherTimer.Start();
        }
        public static void Play()
        {
            FrameworkDispatcher.Update();
            if (!MediaPlayer.GameHasControl || MediaPlayer.Volume == 0.0f)
                            return;            

            if (_menuMusic != null)
                _menuMusic.Dispose();

            _menuMusic = Song.FromUri(MenuMusic, new Uri(@"Resources/Sounds/music/music.mp3", UriKind.Relative));
            MediaPlayer.Play(_menuMusic);            
        }
        public static void Stop()
        {         
           MediaPlayer.Stop();             
        }
        public static bool IsPlaying()
        {
            return MediaPlayer.State == MediaState.Playing;
        }
        public static void SetVolume(float volume)
        {
            if (volume < 0 || volume > 1.0)
                throw new ArgumentOutOfRangeException("volume");
            MediaPlayer.Volume = volume;
        }

    }
}