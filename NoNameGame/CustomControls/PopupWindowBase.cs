using System.Windows;
using System.Windows.Controls;
using NoNameGame.Helpers;

namespace NoNameGame.CustomControls
{
    public class PopupWindowBase : ContentControl 
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof (string), typeof (PopupWindowBase), null);

        public PopupWindowBase()
        {
            DefaultStyleKey = typeof (PopupWindowBase);
            Themer.EnableThemesForControls(this);            
        }
        
        public string Header
        {
            get { return base.GetValue(HeaderProperty) as string; }
            set { base.SetValue(HeaderProperty, value); }
        }

    }
}
