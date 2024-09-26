using NoNameGame.Helpers.Telemetry;

namespace NoNameGame.Levels
{
    public interface ICurrentLevelDataProvider
    {
        CurrentLevelData Get(int levelId);
    }
}