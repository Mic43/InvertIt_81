using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameLogic;
using GameLogic.Board;
using GameLogic.WinVerifiers;

namespace LevelViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameBoard _gameBoard;
        private BoardGrid _boardGrid;
        public MainWindow()
        {
            InitializeComponent();

            Init();
        }
        private void Init()
        {
            _gameBoard = new GameBoard(new Level(ParseString(Moves.Text), 7, 0),
                new AllAreasMustBeChecked());
            _boardGrid = new BoardGrid(_gameBoard);
            _boardGrid.AreaTapped += boardGrid_AreaTapped;            
            Panel.Children.Remove(_boardGrid);
            Panel.Children.Add(_boardGrid);
            _boardGrid.Refresh();
            
        }
        public List<BoardCoordinate> ParseString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return Enumerable.Empty<BoardCoordinate>().ToList();
            string[] strings = s.Split(new char[]{'('});    
        
            return strings.Skip(1).Select(x => new BoardCoordinate(int.Parse(x[0].ToString()), int.Parse(x[2].ToString()))).ToList();
        }

        void boardGrid_AreaTapped(object sender, BoardCoordinate e)
        {
           _gameBoard.OnEnter(e);
          _boardGrid.Refresh();
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Init();
        }
    }
}
