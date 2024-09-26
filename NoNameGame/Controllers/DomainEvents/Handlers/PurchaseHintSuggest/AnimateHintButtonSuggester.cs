using System.Windows;
using Microsoft.Phone.Controls;

namespace NoNameGame.Controllers.DomainEvents.Handlers.PurchaseHintSuggest
{
    public class AnimateHintButtonSuggester : IPurchaseHintsSuggester
    {
        public AnimateHintButtonSuggester()
        {
            
        }
        public void Suggest()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var phoneApplicationFrame = (PhoneApplicationFrame) Application.Current.RootVisual;
                var currentPage = phoneApplicationFrame.Content as GamePage;

                if (currentPage == null)
                    return;

                currentPage.AnimateHintsButton();
            });
                       
        }
    }
}