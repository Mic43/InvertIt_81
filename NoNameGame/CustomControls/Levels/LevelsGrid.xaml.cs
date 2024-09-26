using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.Configuration;
using NoNameGame.CustomControls.AttachedProperties;
using NoNameGame.Helpers;
using NoNameGame.Models;

namespace NoNameGame.CustomControls.Levels
{
    public partial class LevelsGrid : UserControl
    {
      //  private readonly int _initialItemsCapacity;
        public int RowCount
        {
            get { return (int)GetValue(RowCountProperty); }
            set { SetValue(RowCountProperty, value); }
        }
        
        /// <summary>
        /// Identifier for the RowCount dependency property.
        /// </summary>
        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.Register("RowCount", 
            typeof(int), typeof(LevelsGrid), new PropertyMetadata(10));

        /// <summary>
        /// Gets or sets the number of columns on the board.
        /// </summary>
        public int ColumnCount
        {
            get { return (int)GetValue(ColumnCountProperty); }
            set { SetValue(ColumnCountProperty, value); }
        }

        /// <summary>
        /// Identifier for the ColumnCount dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.Register("ColumnCount", 
            typeof(int), typeof(LevelsGrid), new PropertyMetadata(5));

        public event LevelChangedEventHandler LevelSelected;
        private List<LevelModel> _selectLevelControlModel;
        public List<LevelModel> SelectLevelControlModel
        {
            get { return _selectLevelControlModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                DataContext = _selectLevelControlModel = value;
            }
        }

        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register(
            "ItemMargin", typeof (Thickness), typeof (LevelsGrid), new PropertyMetadata(default(Thickness)));

        public Thickness ItemMargin
        {
            get { return (Thickness) GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(
            "ItemWidth", typeof (int), typeof (LevelsGrid), new PropertyMetadata(50));

        public int ItemWidth
        {
            get { return (int) GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
            "ItemHeight", typeof (int), typeof (LevelsGrid), new PropertyMetadata(50));

        public int ItemHeight
        {
            get { return (int) GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }      
        public LevelsGrid()
        {
           // _initialItemsCapacity = initialItemsCapacity;
            InitializeComponent();
            Loaded += OnLoaded;

            SelectLevelControlModel = Enumerable.Empty<LevelModel>().ToList();
            PopulateGrid();
        }
        private void PopulateGrid()
        {
            var rows = Enumerable.Range(0, RowCount).ToArray();
            var columns = Enumerable.Range(0, ColumnCount).ToArray();

            foreach (var row in rows) LayoutRoot.RowDefinitions.Add(new RowDefinition() {Height = GridLength.Auto});
            foreach (var column in columns) LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition() {Width = GridLength.Auto});           
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {                              
//            for (int i = 0; i < RowCount; i++)
//            {
//                for (int j = 0; j < ColumnCount; j++)
//                {
//                    var levelModel = SelectLevelControlModel[i * ColumnCount + j];
//                    var levelControl = CreateLevelCotrol(levelModel);
//
//                    Grid.SetRow(levelControl, i);
//                    Grid.SetColumn(levelControl, j);
//                    LayoutRoot.Children.Add(levelControl);
//                }
//            }
        }
        private FrameworkElement CreateLevelCotrol(LevelModel levelModel)
        {
           // return new Button() {Content = "kwa"};
            var levelControl = new SelectLevelItem
            {
                Width = ItemWidth,
                Height = ItemHeight,
                Margin = ItemMargin,
                Tag = levelModel
            };
            levelControl.Click += levelControl_Click;
            levelControl.IsEnabled = levelControl.IsAvailable = levelModel.IsAvailable;
            levelControl.StarsCount = levelModel.Stars;
            levelControl.Text = levelModel.Id.ToString();

            TiltEffect.SetIsTiltEnabled(levelControl, true);

            var binding = new Binding()
            {
                Source = GetValue(AttachedProperties.ThemeMainColorExtension.ThemeMainBrushProperty)
            };
            levelControl.SetBinding(SelectLevelItem.BackgroundProperty, binding);
            return levelControl;
        }

        void levelControl_Click(object sender, RoutedEventArgs e)
        {
            LevelSelected.Invoke(this, (LevelModel)(((SelectLevelItem)sender).Tag));
        }
       
    }
}
