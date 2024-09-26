using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Common;
using GameLogic.Board;
using LevelsDataBase;
using LevelViewer;

namespace LevelEditor
{
    /// <summary>
    /// Interaction logic for EditLevel.xaml
    /// </summary>
    public partial class EditLevel : Window
    {
        private readonly DisabledAreas _disabledAreas;
        private GameBoardWrapper _gbw;
        private CollectionViewSource _levelDataViewSource;

        public LevelData LevelDataEdited { get;set; }
//        { get { return ((List<LevelData>)_levelDataViewSource.Source).Single(); } }
        public EditLevel(DisabledAreas disabledAreas)
        {            
            _disabledAreas = disabledAreas;
            InitializeComponent();
            _gbw = new GameBoardWrapper(BoardGrid);
            _gbw.IsReadonly = false;
            _gbw.OnMovesChanged+=   OnMovesChanged;

        }
        private void OnMovesChanged(List<BoardCoordinate> boardCoordinates)
        {
            LevelDataEdited.MovesCount = boardCoordinates.Count;
            LevelDataEdited.Moves = MovesCollectionFormatter.SerializeMovesCollection(boardCoordinates.Select(x=> new SingleMove(x.X,x.Y)).ToList());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _levelDataViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("levelDataViewSource")));
            _levelDataViewSource.Source = new List<LevelData>(){LevelDataEdited};

            _gbw.UpdateMoves(LevelDataEdited, _disabledAreas);
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {          
            this.DialogResult = true;
        }        
        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            _gbw.Clear(_disabledAreas);
        }
    }
}
