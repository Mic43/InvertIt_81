namespace NoNameGame.Levels.Entities
{
    public class LevelPackEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int OrderNo { get; private set; }     

        public LevelPackEntity(int id, string name, string description, int orderNo)
        {
            Id = id;
            Name = name;
            Description = description;
            OrderNo = orderNo;
          
        }
    }
}