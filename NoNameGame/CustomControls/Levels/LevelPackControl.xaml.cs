using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GameLogic;
using Microsoft.Phone.Controls;
using NoNameGame.Controllers.Sound;
using NoNameGame.Helpers;
using NoNameGame.Models;

namespace NoNameGame.CustomControls.Levels
{
    public class LevelPackControlModel
    {
        public string Name { get; set; }
        public List<LevelGroupModel> LevelGroups { get; set; }

        public LevelPackControlModel(string name, List<LevelGroupModel> levelGroups)
        {
            Name = name;
            LevelGroups = levelGroups;
        }
    }

    public class LevelGroupModel
    {
        public string Name { get; set; }
        public SelectLevelControlModel SelectLevelControlModel { get; set; }

        public LevelGroupModel(string name, SelectLevelControlModel selectLevelControlModel)
        {
            Name = name;
            SelectLevelControlModel = selectLevelControlModel;
        }
    }

    public partial class LevelPackControl : UserControl
    {
        private LevelPackControlModel _levelPackControlModel;
        private int _oldSelectedIndex;
        public LevelPackControlModel LevelPackControlModel
        {
            get { return _levelPackControlModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _levelPackControlModel = value;                
                RecreatePivot();
            }
        }
//        private void UpdatePivot()
//        {
//            if(_levelPackControlModel.LevelGroups.Count!= _levelPackControlModel.LevelGroups.Count)
//                throw new ArgumentException("Cannot change pivot items count. It must be constant");
//            for (int i = 0; i <  _levelPackControlModel.LevelGroups.Count; i++)
//            {
//                var levelsGrid = (LevelsGrid) ((PivotItem)PivotControl.Items[i]).Content;
//                for (int j = 0; j < levelsGrid.SelectLevelControlModel.Levels.Count; j++)
//                {
//                    LevelModel newlevelModel = _levelPackControlModel.LevelGroups[i].SelectLevelControlModel.Levels[j];
//                    LevelModel currentModel = levelsGrid.SelectLevelControlModel.Levels[j];
//
//                    currentModel.Id = newlevelModel.Id;
//                    currentModel.IsAvailable = newlevelModel.IsAvailable;
//                    currentModel.Id = newlevelModel.Id;
//
//                }
//            }
////            _levelPackControlModel.LevelGroups.ForEach(groupModel =>
////            {
////               // if ( groupModel.SelectLevelControlModel.Levels.Count !=  PivotControl.Items.)
////
////            }
//        }
        public LevelPackControl()
        {
            InitializeComponent();              

          //  _levelPackControlModel = new LevelPackControlModel("Default", Enumerable.Empty<LevelGroupModel>().ToList());
            PivotControl.Loaded += PivotControl_Loaded;
          //  RecreatePivot();
        }

        void PivotControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (_oldSelectedIndex >= PivotControl.Items.Count)
                throw new InvalidOperationException("Control does not supports change of groups count at runtime");
            PivotControl.SelectedIndex = _oldSelectedIndex;
        }

        public event LevelChangedEventHandler LevelSelected;

        public void RecreatePivot()
        {
            PivotControl.Title = _levelPackControlModel.Name;
            _oldSelectedIndex = PivotControl.SelectedIndex;
            PivotControl.Items.Clear();
            _levelPackControlModel.LevelGroups.ForEach(groupModel =>
            {                            
                var pivotItem = new PivotItem()
                {
                    Header   =  groupModel.Name,
                    //   Content =  CreateLevelsControl(groupModel);
                };
                PivotControl.Items.Add(pivotItem);
            });
          
        }
        private UIElement CreateLevelsControl(LevelGroupModel groupModel)
        {
            var levelContol = new SelectLevelControl()
            {
                SelectLevelControlModel = groupModel.SelectLevelControlModel                
            };
            levelContol.LevelSelected += levelContol_LevelSelected;
            TurnstileFeatherEffect.SetFeatheringIndex(levelContol, 1);
            return levelContol;
        }    
        void levelContol_LevelSelected(object sender, LevelModel selectedLevelModel)
        {          
            if (LevelSelected!=null)
                LevelSelected.Invoke(this,selectedLevelModel);
        }
        private void GestureListener_OnFlick(object sender, FlickGestureEventArgs e)
        {
            if (e.Direction == Orientation.Horizontal)
                SoundEffectsPlayer.Current.SwypeEffect.Play();
        }
        private void PivotControl_OnLoadingPivotItem(object sender, PivotItemEventArgs e)
        {
            if (e.Item.Content != null)
                return;
            var pivot = (Pivot) sender;
            var pivotIndex = pivot.Items.IndexOf(e.Item);
            e.Item.Content = CreateLevelsControl(LevelPackControlModel.LevelGroups[pivotIndex]);
        }
        private void RecreatePivotItem(int pivotIndex)
        {
            if (pivotIndex < 0 || pivotIndex > PivotControl.Items.Count - 1)
                throw new ArgumentOutOfRangeException("pivotIndex");
            ((PivotItem)PivotControl.Items[pivotIndex]).Content = CreateLevelsControl(LevelPackControlModel.LevelGroups[pivotIndex]);
        }
        public void RecreateCurentPivotItem()
        {
            RecreatePivotItem(PivotControl.SelectedIndex);
        }
        private void PivotControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            e.RemovedItems.OfType<PivotItem>().Where(x=>x.Content != null)
                .Select(x => ((SelectLevelControl) x.Content)).ToList().ForEach(x=>
            {
                x.SelectLevelControlModel.PauseAdding();
                //x.PauseAnimations();
            });
            e.AddedItems.OfType<PivotItem>().Where(x=>x.Content != null)
               .Select(x => ((SelectLevelControl)x.Content)).ToList().ForEach(x =>
               {
                   x.SelectLevelControlModel.StartAdding();
                   //x.ResumeAnimations();
               });
        }
    }
}
