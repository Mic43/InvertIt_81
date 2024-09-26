using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using NoNameGame.Configuration;
using NoNameGame.Controllers.Themes;
using NoNameGame.Helpers;

namespace NoNameGame.Models
{
    public class ThemeModel 
    {
        public int IndexInCollection { get; set; }
        public ThemeType ThemeType { get; set; }
        public string Name { get; set; }
        public bool IsLocked { get; set; }
        public bool IsAvailable
        {
            get { return !IsLocked; }
        }
        public Brush MainBrush { get; set; }
        public Brush SecondBrush { get; set; }       
        public int StarsToUnlock { get; set; }        
        public ThemeModel(ThemeType themeType, string name, bool isLocked,Brush mainBrush,Brush secondBrush,int starsToUnlock)
        {
            ThemeType = themeType;
            Name = name;
            IsLocked = isLocked;
            MainBrush = mainBrush;
            SecondBrush = secondBrush;
            StarsToUnlock = starsToUnlock;
  
        }
    }
}