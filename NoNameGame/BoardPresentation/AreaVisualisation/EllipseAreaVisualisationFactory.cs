using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;
using Windows.Phone.Speech.Recognition;
using GameLogic.Areas;
using Infrastructure;
using NoNameGame.Configuration;
using NoNameGame.Models;

namespace NoNameGame.BoardPresentation.AreaVisualisation
{
    public class EllipseAreaVisualisationFactory : IAreaVisualisationFactory
    {
        private readonly double _initialTranslation;
        private readonly bool _showDiabledAreaStroke;
        private readonly double _marginLen;
        public EllipseAreaVisualisationFactory(double initialTranslation, double marginLen, bool showDiabledAreaStroke = true)
        {
            _initialTranslation = initialTranslation;
            _showDiabledAreaStroke = showDiabledAreaStroke;
            _marginLen = marginLen;
        }
        public Shape CreateAreaVisualization(AreaModel area)
        {
            Shape shape;
            if (area.AreaState == AreaState.Disabled)
            {
                var rect = new Rectangle
                {
                    RadiusX = _marginLen*3.33,
                    RadiusY = _marginLen*3.33,
                    Margin = new Thickness(0.66*_marginLen),
                    Stroke = new SolidColorBrush(Constants.PhoneChromeColor),
                    Fill = new SolidColorBrush(Colors.Gray),
                    StrokeThickness = _showDiabledAreaStroke? 5 : 0
                };                
                shape = rect;
            }
            else
            {                
                Color color = ColorManipulation.DarkenColor( GameResources.Instance.UnCheckedColor, 0.1f);
                shape = new Ellipse
                {
                    Margin = new Thickness(_marginLen),
                    Stroke = new SolidColorBrush(color)
                };
            }            
           
            shape.Tag = area;
            shape.CacheMode = new BitmapCache();
            shape.RenderTransform = new TranslateTransform() { Y = _initialTranslation };

            if (area.AreaState == AreaState.Checked)
            {
                shape.Fill = GameResources.Instance.CheckedAreaGradientBrush;
                shape.StrokeThickness = 0;
            }
            else if (area.AreaState == AreaState.UnChecked)
            {
                shape.Fill = GameResources.Instance.UnCheckedAreaGradientBrush;
                shape.StrokeThickness = Constants.GameShapeStrokeWidth;
            }                                         
            return shape;
        }
    }
}