using System.Windows.Media;

namespace AnimationLib.AnimationDSL
{
    public static class AnimationBuilder
    {
        public static ScaleBuilder Scale()
        {
            return new ScaleBuilder();
        }
        public static TranslationBuilder Translate()
        {
            return new TranslationBuilder();
        }

        public static OpacityChangeBuilder OpacityChange()
        {
            return new OpacityChangeBuilder();
        }


    }
}