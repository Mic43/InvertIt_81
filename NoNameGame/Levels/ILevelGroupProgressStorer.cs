using NoNameGame.Levels.Entities;

namespace NoNameGame.Levels
{
    public interface ILevelGroupProgressStorer
    {
        void Save(LevelGroupProgressEntity levelProgressEntity);
        LevelGroupProgressEntity Load(int levelId); 
    }
}