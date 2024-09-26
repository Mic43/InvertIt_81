using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Infrastructure
{
   public static class ColorManipulation
    {
       public static Color DarkenColor(Color color, float correctionFactor)
        {
            if (correctionFactor < 0f || correctionFactor > 1.0f)
                throw new ArgumentOutOfRangeException("value must be between 0.0 and 1.0");
            
            float red = -(color.R) * correctionFactor + color.R;
            float green = -(color.G) * correctionFactor + color.G;
            float blue = -(color.B) * correctionFactor + color.B;
            Color lighterColor = Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);

            return lighterColor;
        }
       public static Color LightenColor(Color color, float correctionFactor)
       {
           if (correctionFactor < 0f || correctionFactor > 1.0f)
               throw new ArgumentOutOfRangeException("value must be between 0.0 and 1.0");

           float red = (255 - color.R) * correctionFactor + color.R;
           float green = (255-color.G) * correctionFactor + color.G;
           float blue = (255- color.B) * correctionFactor + color.B;
           Color lighterColor = Color.FromArgb(color.A, (byte)red, (byte)green, (byte)blue);

           return lighterColor;
       }

    }
}
