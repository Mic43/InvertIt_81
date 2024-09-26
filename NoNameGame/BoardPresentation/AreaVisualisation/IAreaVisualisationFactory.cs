using System.Windows.Shapes;
using GameLogic.Areas;
using NoNameGame.Models;

namespace NoNameGame.BoardPresentation.AreaVisualisation
{
    public interface IAreaVisualisationFactory
    {
        Shape CreateAreaVisualization(AreaModel area);
    }
}