using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelsDataBase
{
    public static class ConnectionStringHelper
    {
        private const string _levelsDbConnectionString = @"DataSource=C:\Michał\C#\WIndows Phone\NoNameGame\LevelsDataBase\Levels.sdf";
        public static string LevelsDbConnectionString
        {
            get { return _levelsDbConnectionString; }
        }
    }
}
