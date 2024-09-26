using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NoNameGame.Controllers.Sound;

namespace NoNameGame.CustomControls.ClickSound
{
    public class ClickSoundContentControl : ContentControl
    {
        public static readonly DependencyProperty EffectFilePathProperty = DependencyProperty.Register(
            "EffectFilePath", typeof (string), typeof (ClickSoundContentControl),
            new PropertyMetadata(default(string), PropertyChangedCallback));
        private static void PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((ClickSoundContentControl) dependencyObject)._soundEffect =
                SoundEffect.FromStream(TitleContainer.OpenStream((string) dependencyPropertyChangedEventArgs.NewValue));
        }
        private SoundEffect _soundEffect;

        public string EffectFilePath
        {
            get { return (string) GetValue(EffectFilePathProperty); }
            set { SetValue(EffectFilePathProperty, value); }
        }
//        protected override void OnContentChanged(object oldContent, object newContent)
//        {
//            base.OnContentChanged(oldContent, newContent);
//            var uiElement = newContent as UIElement;
//            if (uiElement != null)
//            {
//                uiElement.
//            }
//        }
        protected override void OnTap(GestureEventArgs e)
        {
            SoundEffectsPlayer.Current.ClickEffect.Play();
//            if (_soundEffect == null)
//                throw new InvalidOperationException("No sound file path was provided. Set EffectFilePath property to valid sound file path");
//            _soundEffect.Play(1.0f, 0.0f, 0.0f);
            base.OnTap(e);
        }      
    }
    
}