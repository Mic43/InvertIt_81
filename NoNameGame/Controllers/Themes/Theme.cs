using System;

namespace NoNameGame.Controllers.Themes
{
    public struct Theme
    {
        public string FileName { get; private set; }
        public string Name { get; private set; }
        public bool IsFlat { get; private set; }

        public ThemeType ThemeType { get; private set; }

        public Theme(string name, string fileName, bool isFlat, ThemeType themeType)
            : this()
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (name == null) throw new ArgumentNullException("name");

            IsFlat = isFlat;
            ThemeType = themeType;
            FileName = fileName;
            Name = name;           
        }
    }
}