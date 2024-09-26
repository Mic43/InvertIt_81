using System.Threading;

namespace NoNameGame.Helpers
{
    class GameCurrentCultureProvider
    {
        public GameCulture Get()
        {
            switch (Thread.CurrentThread.CurrentUICulture.Parent.ToString())
            {
                case "pl": return GameCulture.Polish;
                case "en": return GameCulture.English;
                case "es": return GameCulture.Spanish;
                    
                default:return GameCulture.Other;
            }
        }
    }
}