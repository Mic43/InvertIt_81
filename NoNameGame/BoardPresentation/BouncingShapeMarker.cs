using System;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AnimationLib.AnimationsCreator;
using Microsoft.Xna.Framework.Audio;
using NoNameGame.BoardPresentation.Animations;
using NoNameGame.Configuration;

namespace NoNameGame.BoardPresentation
{
    internal class BouncingShapeMarker
    {
        private readonly TimeSpan _bounceDuration;
   //     private readonly SoundEffect _effectToPlay;
        private DispatcherTimer _soundPlayer = new DispatcherTimer();
        private Storyboard _storyboard;
        private readonly int _maxHeight = Constants.GetHintBounceAnimationMaxHeight;
        private Shape _currentlyMarked = null;

        public BouncingShapeMarker(TimeSpan bounceDuration)
        {
            _bounceDuration = bounceDuration;         
            SetupTimer(bounceDuration);
        }

        private void SetupTimer(TimeSpan bounceDuration)
        {
            _soundPlayer = new DispatcherTimer() {Interval = bounceDuration.Add(bounceDuration)};
            _soundPlayer.Tick += _soundPlayer_Tick;            
        }

        void _soundPlayer_Tick(object sender, EventArgs e)
        {
            //_effectToPlay.Play();
        }

        public void UnMark()
        {
            if (_soundPlayer == null || _storyboard == null)
                return;

            _soundPlayer.Stop();
            _storyboard.Seek(TimeSpan.Zero);
            _storyboard.Pause();
            _currentlyMarked = null;
        }
        public void TryMark(Shape shape)
        {
            if (shape == null) throw new ArgumentNullException("shape");

            if (_currentlyMarked == shape)
            {
                return;
            }
            if (_storyboard!=null)
                UnMark();

            _storyboard = new SingleAnimationCreator(AnimationsRepository.CreateBounceAnimation(_maxHeight, _bounceDuration))
                .Create(shape);
            _storyboard.Completed += (sender, args) => _soundPlayer.Stop();
            _storyboard.Begin();
            _currentlyMarked = shape;
            //  _soundPlayer.Start();
        }
        public bool IsAnyShapeMarked()
        {
            return _currentlyMarked != null;
        }
    }
}