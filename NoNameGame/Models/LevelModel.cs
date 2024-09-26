namespace NoNameGame.Models
{
    public class LevelModel
    {        
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public int Stars { get; set; }
        public bool IsAvailable { get; set; }

        public bool IsUnfinished
        {
            get { return Stars == 0 && IsAvailable; }
        }
        public bool PlayAnimation { get; set; }

        public LevelModel(int id,string displayName, int stars, bool isAvailable)
        {
            Id = id;
            DisplayName = displayName;
            Stars = stars;
            IsAvailable = isAvailable;
        }
    }
}