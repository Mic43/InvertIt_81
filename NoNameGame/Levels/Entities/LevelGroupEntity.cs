namespace NoNameGame.Levels.Entities
{
    public class LevelGroupEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public string Description { get; private set; }
        public int OrderNo { get; private set; }
        public bool AllLevelsInitiallyUnlocked { get; private set; }
        public int LevelPackId { get; private set; }

        public LevelGroupEntity(int id, string name, string description, int orderNo,bool allLevelsInitiallyUnlocked,int levelPackId
            )
        {
            Id = id;
            Name = name;
            Description = description;
            OrderNo = orderNo;
            AllLevelsInitiallyUnlocked = allLevelsInitiallyUnlocked;
            LevelPackId = levelPackId;
        }
    }
}