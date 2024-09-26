using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using NoNameGame.Models;

namespace NoNameGame.CustomControls
{
    public class SelectThemeModel
    {       
        public ObservableCollection<ThemeModel> Themes { get; set; }

        public ThemeModel SelectedTheme { get; set; }

        public SelectThemeModel(ObservableCollection<ThemeModel> themes, ThemeModel selectedTheme)
        {
            Themes = themes;
            SelectedTheme = selectedTheme;          
            foreach (var themeModel in Themes)
            {
                themeModel.IndexInCollection = (int)(Themes.IndexOf(themeModel) /1.0f);
            }

        }
    }
    public partial class SelectThemeControl : UserControl
    {
        private SelectThemeModel _selectThemeModel;

        public SelectThemeModel SelectThemeModel
        {
            get { return _selectThemeModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");             
                DataContext = _selectThemeModel = value;
            }
        }

        public event ThemeChangedEventHandler ThemeChanged;

        public SelectThemeControl()
        {
            InitializeComponent();
            ListBoxThemes.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
            _selectThemeModel = new SelectThemeModel(new ObservableCollection<ThemeModel>(), null);         
        }

        void ItemContainerGenerator_ItemsChanged(object sender, System.Windows.Controls.Primitives.ItemsChangedEventArgs e)
        {
            //var listBoxItems = ListBoxThemes.Items.Select(item => ListBoxThemes.ItemContainerGenerator.ContainerFromItem(item)).OfType<ListBoxItem>();
            //Themer.EnableThemesForControls(listBoxItems.ToArray());
        }
        private bool sentinel = true;
        private void ListBoxThemes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;
            if (!sentinel)
                return;
            
            if ( ThemeChanged!= null)
                ThemeChanged.Invoke(this,SelectThemeModel.SelectedTheme);
            sentinel = false;
            ListBoxThemes.SelectedItem = null;            
            ListBoxThemes.SelectedItem = e.AddedItems[0];
            sentinel = true;
        }
    }

    public delegate void ThemeChangedEventHandler(object sender, ThemeModel newTheme);
}
