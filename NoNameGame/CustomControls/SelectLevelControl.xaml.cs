using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using NoNameGame.Helpers;
using NoNameGame.Models;

namespace NoNameGame.CustomControls
{
    public class SelectLevelControlModel
    {
        public IList<LevelModel> Levels { get; set; }
        public SelectLevelControlModel(IList<LevelModel> levels)
        {
            Levels = levels;
        }
    }
    public partial class SelectLevelControl : UserControl
    {
        public event LevelChangedEventHandler LevelSelected;
        private SelectLevelControlModel _selectLevelControlModel;
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

            SelectLevelControlModel = new SelectLevelControlModel( Enumerable.Empty<LevelModel>().ToList());
        }
        private void ListSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListSelector.SelectedItem == null)
                return;            
            LevelSelected.Invoke(this,(LevelModel)ListSelector.SelectedItem);

            ListSelector.SelectedItem = null;
        }
    }

    public delegate void LevelChangedEventHandler(object sender, LevelModel selectedLevelModel);
}
