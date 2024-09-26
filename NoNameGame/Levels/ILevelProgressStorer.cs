using NoNameGame.Levels.Entities;

namespace NoNameGame.Levels
{
    public interface ILevelProgressStorer
    {
        void Save(LevelProgressEntity levelProgressEntity);
        LevelProgressEntity Load(int levelId);
    }
}