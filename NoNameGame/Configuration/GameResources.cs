using System;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Expression.Shapes;
using NoNameGame.CustomControls;
using Infrastructure;

namespace NoNameGame.Configuration
{
    public sealed class GameResources : INotifyPropertyChanged
    {
        
        private static readonly GameResources instance = new GameResources();

        static GameResources()
        {
        }

        private GameResources()
        {
        }

        public static GameResources Instance
        {
            get { return instance; }
        }


        private bool invertColors;

        public bool InvertColors
        {
            get { return invertColors; }
            set { invertColors = value;
                RefreshAfterThemeChange();
            }
        }
        #region Inner Colors Resources
        private Brush CheckedAreaGradientBrushInner
        {
            get { return (Brush)Application.Current.Resources[ThemesResoucesKeyNames.CheckedAreaGradientBrush]; }
        }
        private Brush UnCheckedAreaGradientBrushInner
        {
            get { return (Brush)Application.Current.Resources[ThemesResoucesKeyNames.UnCheckedAreaGradientBrush]; }
        }
        private Brush OverlayGradientBlueInner
        {
            get { return (Brush)Application.Current.Resources["OverlayGradientBlue"]; }
        }
        private Brush OverlayGradientRedInner
        {
            get { return (Brush)Application.Current.Resources["OverlayGradientRed"]; }
        }
        private static Color CheckedColorInner
        {
            get { return (Color)Application.Current.Resources[ThemesResoucesKeyNames.CheckedAreaColor]; }
        }
        private static Color UnCheckedColorInner
        {
            get { return (Color)Application.Current.Resources[ThemesResoucesKeyNames.UnCheckedAreaColor]; }
        } 
        #endregion

        public Brush CheckedAreaGradientBrush
        {
            get { return InvertColors ? UnCheckedAreaGradientBrushInner : CheckedAreaGradientBrushInner; }
        }

        public Brush UnCheckedAreaGradientBrush
        {
            get { return InvertColors ? CheckedAreaGradientBrushInner : UnCheckedAreaGradientBrushInner; }
        }
        public Brush OverlayGradientRed
        {
            get { return InvertColors ? OverlayGradientBlueInner : OverlayGradientRedInner; }
  
        }
        public Brush OverlayGradientBlue
        {
            get { return InvertColors ? OverlayGradientRedInner : OverlayGradientBlueInner; }
        }

        public Color CheckedColor
        {
            get { return InvertColors ? UnCheckedColorInner : CheckedColorInner; }
        }
        public Color UnCheckedColor
        {
            get { return InvertColors ? CheckedColorInner : UnCheckedColorInner; }

        }
        public Color AccentLightColor
        {
            get { return Helpers.GameAccentColorProvider.GetLighter(); }

        }
        public Color AccentDarkColor
        {
            get { return Helpers.GameAccentColorProvider.GetDarker(); }
        }
        public Color StarStrokeColor
        {
            get { return ColorManipulation.DarkenColor(GameResources.Instance.CheckedColor, 0.1f); }

        }


        public BitmapImage StareBitmapImage
        {
            get { return (BitmapImage)Application.Current.Resources["StarBitmapImage"]; }           
        }
        //public WriteableBitmap StarBitmap { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public void RefreshAfterThemeChange()
        {
            OnPropertyChanged("CheckedColor");
            OnPropertyChanged("UnCheckedColor");
            OnPropertyChanged("AccentLightColor");
            OnPropertyChanged("AccentDarkColor");
            OnPropertyChanged("CheckedAreaGradientBrush");
            OnPropertyChanged("UnCheckedAreaGradientBrush");
            OnPropertyChanged("StarStrokeColor");
            

//
//            string s = @"<Viewbox xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" 
//                       xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
//                       xmlns:es=""clr-namespace:Microsoft.Expression.Shapes;assembly=Microsoft.Expression.Drawing"">
//                <es:RegularPolygon Height=""77.853"" InnerRadius=""0.5"" PointCount=""5"" Stretch=""Fill"" 
//                    Stroke=""Black"" UseLayoutRounding=""False"" Width=""77.853""/>
//             </Viewbox>";
//            var starsControl = (Viewbox)XamlReader.Load(s);
//            var regularPolygon = (starsControl.Child as RegularPolygon);
//            regularPolygon.Stroke = new SolidColorBrush(Colors.Black);
//            regularPolygon.StrokeThickness = 1;
//            starsControl.Width = 50;
//            starsControl.Height = 50;
//
//            var grid = new Grid();
//            grid.Background = new SolidColorBrush(Colors.Magenta);
//            
//           // var starsControl = new StarsControl() {StarsCount = 1,Width = 50,Height = 50};
//           // var starsControl = new Button() {Content = "kwa",Width = 70,Height = 70 };
//          //  starsControl.Background = new SolidColorBrush(Colors.Cyan);
////            starsControl.Foreground = new SolidColorBrush(Colors.Green);
//            grid.Children.Add(starsControl);
//            Grid.SetRow(starsControl, 0);
//            Grid.SetColumn(starsControl, 0);
//            
//
//            grid.UpdateLayout(); 
//            grid.Measure(new Size(100,100));           
//            grid.Arrange(new Rect(new Point(0, 0), new Size(100,100)));
//
//            var StarBitmap = new WriteableBitmap(100, 100);
//
//            StarBitmap.Render(grid, new TranslateTransform());
//            StarBitmap.Invalidate();
//
//            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
//            {
//
//                IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile("kwa6.jpg", FileMode.CreateNew);
//                StarBitmap.SaveJpeg(fileStream, 100, 100, 0, 100);
//                fileStream.Close();
//            }

        }
    }
    public class GameResourcesProvider
    {
        public GameResources GameResources { get { return GameResources.Instance; } }
    }
   
}