using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using Infrastructure;
using Microsoft.Phone.Controls;
using NoNameGame.Controllers.PlayerStats;
using NoNameGame.Helpers;
using NoNameGame.Helpers.LongList;
using NoNameGame.Models;

namespace NoNameGame.CustomControls.Levels
{
 
    public class SelectLevelControlModel
    {
        private readonly int _maxLevelStars;        
        private int currentElem = 0;
        private readonly DispatcherTimer _addingItemsTimer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(20) };      
        public ObservableCollection<LevelModel> Levels { get; set; }
        public List<LevelModel> AllLevels { get; private set; }

        public int CurrentStarsCount { get { return AllLevels.Select(x => x.Stars).Sum(); } }
        public int AllStarsCount { get { return _maxLevelStars*AllLevels.Count; } }
        public SelectLevelControlModel(List<LevelModel> levels,int maxLevelStars)
        {
            if (levels.Count == 0)
                throw new ArgumentException("levels collection should have at least 1 element","levels");
            _maxLevelStars = maxLevelStars;
            AllLevels = levels.ToList();
            Levels = new ObservableCollection<LevelModel>();
//            Levels.AddR(levels.Take(10).ToList());            
            _addingItemsTimer.Tick += (sender, args) =>
            {
                if (currentElem < AllLevels.Count)
                {
                    Levels.Add(AllLevels[currentElem]);
                    currentElem ++;
                }
                else
                {
                   // currentElem = 0;
                    _addingItemsTimer.Stop();
                }
            };         
        }
        public void StartAdding()
        {
            _addingItemsTimer.Start();
        }
        public void PauseAdding()
        {
            _addingItemsTimer.Stop();
        }
        ~SelectLevelControlModel()
        {
            Debug.WriteLine("SelectLevelControlModel desctructor");
        }
    }
    public partial class SelectLevelControl : UserControl
    {
        public double Threshold { get; set; }
        public event LevelChangedEventHandler LevelSelected;
        private SelectLevelControlModel _selectLevelControlModel;        
//        private ScrollBar _llsScrollBar;
//        private readonly int _offsetKnob = 1;
       
        public SelectLevelControlModel SelectLevelControlModel
        {
            get { return _selectLevelControlModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                DataContext = _selectLevelControlModel = value;              
            }
        }
        public SelectLevelControl()
        {
            InitializeComponent();
            Themer.EnableThemesForControls(this.ListSelector);           
          //  SelectLevelControlModel = new SelectLevelControlModel( Enumerable.Empty<LevelModel>().ToList());         
            Loaded += SelectLevelControl_Loaded;           
            ListSelector.Unloaded+=ListSelectorOnUnloaded;
        }
        private void ListSelectorOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
           SelectLevelControlModel.PauseAdding();
        }

        void SelectLevelControl_Loaded(object sender, RoutedEventArgs e)
        {
           //_selectLevelControlModel.StartAdding();
            StarsProgressBar.StarsProgressModel = new StarsProgressModel(SelectLevelControlModel.CurrentStarsCount,
                SelectLevelControlModel.AllStarsCount);
            //StarsProgressBar.PlayAnimation();
        }
        private void ListSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListSelector.SelectedItem == null)
                return;       
            if(LevelSelected!=null)
                LevelSelected.Invoke(this,(LevelModel)ListSelector.SelectedItem);

            ListSelector.SelectedItem = null;
        }
        public void PauseAnimations()
        {
            AllChildren<SelectLevelItem>(this.ListSelector).ForEach(x => x.PauseAnimation());
        }
        public void ResumeAnimations()
        {
            AllChildren<SelectLevelItem>(this.ListSelector).ForEach(x=>x.ResumeAnimation());
        }

        private List<T> AllChildren<T>(DependencyObject parent) where T : class
        {
            var _List = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var _Child = VisualTreeHelper.GetChild(parent, i);
                if (_Child is T)
                {
                    _List.Add(_Child as T);
                }
                _List.AddRange(AllChildren<T>(_Child));
            }
            return _List;
        } 

               
    }

    public delegate void LevelChangedEventHandler(object sender, LevelModel selectedLevelModel);
}
