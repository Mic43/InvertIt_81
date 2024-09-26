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
using GameLogic;
using GameLogic.Areas;
using GameLogic.Board;
using GameLogic.WinVerifiers;
using Infrastructure;
using LevelViewer;

namespace LevelEditor
{
    /// <summary>
    /// Interaction logic for MoveLevelWindows.xaml
    /// </summary>
    public partial class DisabledAreasEditor : Window
    {

        private BoardGrid bg;
        private GameBoard _gameBoard;
        public DisabledAreasEditor(int boardSize)
        {
            InitializeComponent();
            _gameBoard = new GameBoard(new Level(Enumerable.Empty<BoardCoordinate>().ToList(), boardSize, 1), new AllAreasMustBeChecked());
            bg   = new BoardGrid(_gameBoard);
            bg.AreaTapped+=BgOnAreaTapped;
            StackPanel.Children.Add(bg);
            bg.Refresh();
        }
        private void BgOnAreaTapped(object sender, BoardCoordinate boardCoordinate)
        {
            var area = _gameBoard.AreaMatrix.Areas[boardCoordinate.X, boardCoordinate.Y];
            area.AreaState = area.AreaState == AreaState.Checked ? AreaState.Disabled : AreaState.Checked;
            bg.Refresh();
            DisabledFieldsTextBox.Text =
                MovesCollectionFormatter.SerializeMovesCollection(_gameBoard.AreaMatrix.Areas.OfType<Area>()
                    .Where(x => x.AreaState == AreaState.Disabled)
                    .Select(x => BoardCooridnateConvetert.FromBoardCoordinate(x.BoardCoordinate)).ToList());


        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
         
        }
        private void DisabledFieldsTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _gameBoard.AreaMatrix.Areas.ForEach(x=>x.AreaState = AreaState.Checked);
            MovesCollectionFormatter.ParseString(DisabledFieldsTextBox.Text).ForEach(
                coord => _gameBoard.AreaMatrix.Areas[coord.X,coord.Y].AreaState = AreaState.Disabled);
            bg.Refresh();
        }
    }
}
