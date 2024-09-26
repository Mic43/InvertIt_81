using System;
using System.Windows;
using NoNameGame.Resources;

namespace NoNameGame.Controllers.DomainEvents.Handlers.PurchaseHintSuggest
{
    class SuggestWithMessageBox : IPurchaseHintsSuggester
    {
        public void Suggest()
        {           
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var res = MessageBox.Show(AppResources.SuggestWithMessageBox_Text,
                    AppResources.SuggestWithMessageBox_Caption, MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.OK)
                {
                    App.RootFrame.Navigate(new Uri(@"/PurchaseHintsPage.xaml", UriKind.Relative));
                }
            });
            
        }
    }
}