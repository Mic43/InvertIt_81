namespace NoNameGame.Helpers.FullScreenAds
{
    public interface IFullScreenAdDisplayer
    {
        void Preload();
        void TryShowAsync();
    }
}