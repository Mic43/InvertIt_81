using System.Windows;

namespace AnimationLib.AnimationDSL.Helpers
{
    public class PropertyPathsRepository
    {
        private static readonly PropertyPath _createScaleX = new PropertyPath("UIElement.RenderTransform.ScaleX");
        private static readonly PropertyPath _createTranslateY = new PropertyPath("UIElement.RenderTransform.Y");
        private static readonly PropertyPath _createScaleY = new PropertyPath("UIElement.RenderTransform.ScaleY");
        private static readonly PropertyPath _createTranslateX = new PropertyPath("UIElement.RenderTransform.X");
        private static readonly PropertyPath _opacity = new PropertyPath("UIElement.Opacity");
        public static PropertyPath ScaleX
        {
            get { return _createScaleX; }
        }
        public static PropertyPath ScaleY
        {
            get { return _createScaleY; }
        }
        public static PropertyPath TranslateY
        {
            get { return _createTranslateY; }
        }
        public static PropertyPath TranslateX
        {
            get { return _createTranslateX; }
        }
        public static PropertyPath Opacity
        {
            get { return _opacity; }
        }
    }
}