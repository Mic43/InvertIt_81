using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using GameLogic;
using GameLogic.Board;
using GameLogic.Game;
using GameLogic.MovesSequentionGenerators;
using GameLogic.MovesSequentionGenerators.Evaluators;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.Helpers;

namespace NoNameGame
{
    public partial class SelectDifficultyPage : PhoneApplicationPage
    {
        public SelectDifficultyPage()
        {
            InitializeComponent();
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame(new UltraEasyDifficulty());
        }

        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame(new MediumDifficulty());
        }

        private void HardButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame(new HardDifficulty());
        }

        private void StartNewGame(GameDifficulty difficulty)
        {
            //this.CurrentApp()
            GoToGamePage();
        }

        private void GoToGamePage()
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }
    }
}