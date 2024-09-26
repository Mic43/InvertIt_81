using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
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
using AnimationLib.AnimationDSL.Helpers;
using Common;
using GameLogic.Board;
using Infrastructure.Storage;
using LevelsDataBase;
using LevelViewer;

namespace LevelEditor
{
    /// <summary>
    /// Interaction logic for LevelEditor.xaml
    /// </summary>
    public partial class LevelEditorWindow : Window
    {
        public static LevelsContext _levelsContext;
        private CollectionViewSource _levelGroupViewSource;
        private CollectionViewSource _levelDataViewSource;
        private CollectionViewSource _levelPackViewSource;
        private ObservableCollection<LevelPack> _LevelPacksCollection;
        private GameBoardWrapper _gbw;
        public LevelEditorWindow()
        {
            InitializeComponent();
            _gbw = new GameBoardWrapper(BoardGrid);
            _gbw.IsReadonly = true;
            _levelsContext =
                new LevelsContext(ConnectionStringHelper.LevelsDbConnectionString);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _levelPackViewSource =
                ((System.Windows.Data.CollectionViewSource) (this.FindResource("levelPackViewSource")));
            _levelGroupViewSource =
                ((System.Windows.Data.CollectionViewSource) (this.FindResource("levelGroupViewSource")));
            _levelDataViewSource =
                ((System.Windows.Data.CollectionViewSource) (this.FindResource("levelDataViewSource")));

            _levelPackViewSource.Source = _levelsContext.LevelPack;




        }
//
//        void _LevelPacksCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
//        {
//            switch (e.Action)
//            {
//                case NotifyCollectionChangedAction.Add:
//                    _levelsContext.LevelPack.InsertAllOnSubmit(e.NewItems.OfType<LevelPack>());
//                    _levelsContext.SubmitChanges();
//                    break;           
//                case NotifyCollectionChangedAction.Remove:
//                    _levelsContext.LevelPack.DeleteAllOnSubmit(e.NewItems.OfType<LevelPack>());
//                    break;
//            }
//            _levelsContext.SubmitChanges();
//        }

        private void levelPackListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selItem = e.AddedItems[0] as LevelPack;
            if (selItem != null)
                _levelGroupViewSource.Source =
                    _levelsContext.LevelGroup.Where(x => x.LevelPackId == selItem.Id).ToList();
        }

        private void levelGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
//            var selItem = e.AddedItems[0] as LevelGroup;
//            if (selItem != null)
//            _levelDataViewSource.Source = _levelsContext.LevelData.Where(x => x.LevelGroupId == selItem.Id).ToList();
            RefreshLevelList();
        }

        private void levelDataDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selItem = e.AddedItems[0] as LevelData;
                if (selItem != null)
                {
                    _gbw.UpdateMoves(selItem, _levelsContext.DisabledAreas.
                        SingleOrDefault(x => x.Id == selItem.DisabledAreasId));
                }
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            int currentGroupId = GetSelectedlevelGroup().Id;
            int max =
                _levelsContext.LevelData.Where(x => x.LevelGroupId == currentGroupId)
                    .Select(x => x.OrderNo)
                    .ToList()
                    .DefaultIfEmpty(0)
                    .Max();
            int orderNo = max + 1;
            _levelsContext.LevelData.InsertOnSubmit(new LevelData()
            {
                LevelGroupId = (levelGroupListView.SelectedItem as LevelGroup).Id,
                DisplayName = orderNo.ToString(),
                OrderNo = orderNo,
                Moves = string.Empty,BoardSize = 7                
            });
            _levelsContext.SubmitChanges();
            RefreshLevelList();
        }
        private void RefreshLevelList()
        {
            _levelDataViewSource.Source =
                _levelsContext.LevelData.Where(x => x.LevelGroupId == GetSelectedlevelGroup().Id)
                    .OrderBy(x => x.OrderNo)
                    .ToList();
        }
        private LevelGroup GetSelectedlevelGroup()
        {
            return ((LevelGroup) levelGroupListView.SelectedItem);
        }
        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            var levelData = GetSelectedLevelData();
            _levelsContext.LevelData.DeleteOnSubmit(_levelsContext.LevelData.Single(x =>
                x.Id == levelData.Id
                ));
            _levelsContext.SubmitChanges();
            RefreshLevelList();
        }
        private LevelData GetSelectedLevelData()
        {
            return ((LevelData) levelDataDataGrid.SelectedItem);
        }
        private void LevelDataDataGrid_OnRowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            ;
        }
        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            var current = _levelsContext.LevelData.Single(x => x.Id == GetSelectedLevelData().Id);
            var wnd = new EditLevel(current.DisabledAreas);
            wnd.LevelDataEdited = new LevelData();
            wnd.LevelDataEdited.Difficulty = current.Difficulty;
            wnd.LevelDataEdited.DisplayName = current.DisplayName;
            wnd.LevelDataEdited.LevelGroupId = current.LevelGroupId;

            wnd.LevelDataEdited.Moves = current.Moves;
            wnd.LevelDataEdited.MovesCount = current.MovesCount;
            wnd.LevelDataEdited.Id = current.Id;
            wnd.LevelDataEdited.BoardSize = current.BoardSize;
            wnd.LevelDataEdited.OrderNo = current.OrderNo;

            bool? showDialog = wnd.ShowDialog();
            if (showDialog.HasValue)
            {
                if (showDialog.Value)
                {
                    if (_levelsContext.LevelData
                        .Where(
                            x =>
                                x.LevelGroupId == ((LevelGroup) levelGroupListView.SelectedItem).Id &&
                                x.Id != wnd.LevelDataEdited.Id)
                        .Any(x => x.OrderNo == wnd.LevelDataEdited.OrderNo))

                    {
                        MessageBox.Show("Orer id is nto uniqe in this group");
                        return;
                    }

                    //LevelData newLevelData = wnd.LevelDataEdited;
                    var z = _levelsContext.LevelData.Single(x => x.Id == current.Id);
                    z.Difficulty = wnd.LevelDataEdited.Difficulty;
                    z.DisplayName = wnd.LevelDataEdited.DisplayName;
                    z.LevelGroupId = wnd.LevelDataEdited.LevelGroupId;
                    z.BoardSize = wnd.LevelDataEdited.BoardSize;
                    z.Moves = wnd.LevelDataEdited.Moves;
                    z.MovesCount = wnd.LevelDataEdited.MovesCount;
                    z.Id = wnd.LevelDataEdited.Id;
                    z.OrderNo = wnd.LevelDataEdited.OrderNo;

                    _levelsContext.SubmitChanges();
                }
            }
            RefreshLevelList();
        }
        private void MoveButton_OnClick(object sender, RoutedEventArgs e)
        {
//            var moveLevelWindows = new MoveLevelWindows();
//            bool? showDialog = moveLevelWindows.ShowDialog();
//            if (showDialog.HasValue && showDialog.Value)
//            {
//                //         LevelData selectedLevelData = GetSelectedLevelData();
//                //       selectedLevelData.OrderNo = moveLevelWindows.NewPosition;
//
//
//                //     ((List<LevelData>) _levelDataViewSource.Source).SingleOrDefault(x=>x.OrderNo ==  moveLevelWindows.NewPosition)
//
//                RefreshLevelList();
//            }
        }
        private void DisabledMovesButton_OnClick(object sender, RoutedEventArgs e)
        {
           var moveLevelWindows = new DisabledAreasEditor(7);
            bool? showDialog = moveLevelWindows.ShowDialog();

        }
        private void DisabledMoves9Button_OnClick(object sender, RoutedEventArgs e)
        {
            var moveLevelWindows = new DisabledAreasEditor(9);
            bool? showDialog = moveLevelWindows.ShowDialog();
        }
    }
}
