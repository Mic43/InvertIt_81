using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GameLogic;
using GameLogic.Game;
using Infrastructure;
using Microsoft.Phone.Controls;
using NoNameGame.Configuration;
using NoNameGame.CustomControls.AttachedProperties;
using NoNameGame.CustomControls.Popups;
using MessageBox = NoNameGame.CustomControls.Popups.MessageBox;

namespace NoNameGame.Helpers
{
    public static class PageExtensions
    {     
        public static App CurrentApp(this PhoneApplicationPage page)
        {
            return (App)Application.Current; 
        }
        public static App CurrentApp(this UserControl control)
        {
            return (App)Application.Current;
        }
//        public static Game Game(this PhoneApplicationPage page)
//        {
//            return page.CurrentApp().GameController.Game;
//        }
        public static Page CurrentPage()
        {
            return ((Page) ((PhoneApplicationFrame) Application.Current.RootVisual).Content);
        }
        public static void ShowMessageBox(this PhoneApplicationPage page,string caption, string text)
        {
            var messageBox = new MessageBox(caption,text);
            new PopupWindowService(page, messageBox, new UIElementWithTappedAction(messageBox.ResumeButton, (e) => { })).Show();
        }
    }
}