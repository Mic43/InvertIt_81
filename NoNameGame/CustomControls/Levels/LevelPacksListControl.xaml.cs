using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using AnimationLib.AnimationsCreator;
using AnimationLib.AnimationDSL;

namespace NoNameGame.CustomControls.Levels
{
    public class LevelPacksListControlModel
    {
        public List<LevelPackModel> LevelPackModels { get; set; }
        public LevelPacksListControlModel(List<LevelPackModel> levelPackModels)
        {
            LevelPackModels = levelPackModels;
        }
    }
    
    public class LevelPackModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LevelsCount { get; set; }
        public int FinishedLevelsCount { get; set; }
        public string Number { get; set; }
        public bool IsLocked { get; private set; }

        public LevelPackModel(int id, string name, string description, int levelsCount, int finishedLevelsCount,string number,bool isLocked = false)
        {
            Description = description;
            Id = id;
            Name = name;
            LevelsCount = levelsCount;
            FinishedLevelsCount = finishedLevelsCount;
            Number = number;
            IsLocked = isLocked;
        }
    }

    public delegate void LevelPackSelectedEventHandler(object sender,LevelPackModel selectedLevelPackModel);

    public partial class LevelPacksListControl : UserControl
    {
        private LevelPacksListControlModel _levelPacksListControlModel;
        private Storyboard _begin;
        public LevelPacksListControlModel LevelPacksListControlModel
        {
            get { return _levelPacksListControlModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                DataContext = _levelPacksListControlModel = value;
            }
        }
        public event LevelPackSelectedEventHandler LevelPackSelected;

        public LevelPacksListControl()
        {
            InitializeComponent();
        }
        private void ListSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListSelector.SelectedItem == null)
                return;

            if (LevelPackSelected != null)
                LevelPackSelected.Invoke(this, (LevelPackModel)ListSelector.SelectedItem);

            ListSelector.SelectedItem = null;
        }
        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_begin == null)
            {
//                _begin = new SingleAnimationCreator(
//                    AnimationBuilder.Scale().Uniform().WithEasingFunction(new QuarticEase() { EasingMode = EasingMode.EaseOut})
//                                            .To(1.03).RepeatForever().AutoReverse().WithDuration(500).Build())
//                    .Create(sender as UIElement);
//                _begin.Begin();
            }
        }
    }
}
