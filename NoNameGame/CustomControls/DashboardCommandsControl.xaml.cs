using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.Helpers;
using NoNameGame.Resources;

namespace NoNameGame.CustomControls
{
    public class DashBoardCommandModel
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Action<PhoneApplicationPage> Command { get; private set; }
        public bool IsNew { get; set; }
        public ImageSource ImageSource { get; private set; }

        public DashBoardCommandModel(string name, string description, Action<PhoneApplicationPage> command, bool isNew, ImageSource imageSource)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (imageSource == null) throw new ArgumentNullException("imageSource");
            Name = name;
            Description = description;
            Command = command;
            IsNew = isNew;
            ImageSource = imageSource;
        }
    }
    public partial class DashboardCommandsControl : UserControl
    {
        public DashboardCommandsControl()
        {
            InitializeComponent();
            Loaded+=OnLoaded;
        }
        public event DashBoardCommandExecutedEvent DashboardCommandExecuted;
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            bool isNewItemUnlocked = this.CurrentApp().NewItemUnlockedStorer.IsNewItemUnlocked();
            DashboardCommandsList.ItemsSource = new ObservableCollection<DashBoardCommandModel>
            (
                new List<DashBoardCommandModel>()
                {
                    new DashBoardCommandModel(AppResources.DashboardList_ThemesTitle, AppResources.DashboardList_ThemesDescription,
                        (page) => page.NavigationService.Navigate(new Uri("/ThemesPage.xaml", UriKind.Relative)),isNewItemUnlocked,
                         new BitmapImage(new Uri("/Assets/AppBar/themes.png",UriKind.Relative))),
                    new DashBoardCommandModel(AppResources.DashboardList_AchievementsTitle,AppResources.DashBoardList_AchievementsDescription,
                        (page) => page.NavigationService.Navigate(new Uri("/AchievementsPage.xaml", UriKind.Relative)),false,
                         new BitmapImage(new Uri("/Assets/AppBar/TrophyBW2_white.png",UriKind.Relative))),
                    new DashBoardCommandModel(AppResources.DashboardList_SettingsTitle, AppResources.DashboardList_SettingsDescription,
                        (page) => page.NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative)),false,
                         new BitmapImage(new Uri("/Assets/AppBar/feature.settings.png",UriKind.Relative))),
                    new DashBoardCommandModel(AppResources.DashboardList_HelpTitle, AppResources.DashboardList_Helpescription,
                        (page) => page.NavigationService.Navigate(new Uri("/HelpPage.xaml", UriKind.Relative)),false,
                         new BitmapImage(new Uri("/Assets/AppBar/questionmark.png",UriKind.Relative))),                                                                            

                }
            );
           // FadeAnimation.Begin();
        }
        private void ListSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DashboardCommandExecuted != null)
                DashboardCommandExecuted.Invoke(this, (DashBoardCommandModel) DashboardCommandsList.SelectedItem);
            
        }
    }

    public delegate void DashBoardCommandExecutedEvent(object sender, DashBoardCommandModel executedCommand);
}
