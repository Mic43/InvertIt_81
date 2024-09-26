using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NoNameGame.Models;

namespace NoNameGame.CustomControls
{
    public class PurchaseHintsModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }

        public string ProductId { get; set; }

        public string DisplayName { get { return string.Format("{0} - {1}", Name, Price); } }

        public bool IsBestDeal { get; set; }

    }

    public partial class PurchaseHintsControl : UserControl
    {
        private List<PurchaseHintsModel> _model;

        public List<PurchaseHintsModel> Model
        {
            get { return _model; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _model = value;
                this.HintsList.ItemsSource = _model;
            }
        }
        public Action<string> WhnenClicked { get; set; }
        public PurchaseHintsControl()
        {
            InitializeComponent();
            HintsList.Loaded += (sender, args) => 
                HintsList.SelectedItem = null;
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            WhnenClicked((string) ((Button) sender).Tag);
        }
        private void HintsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HintsList.SelectedItem != null)
                WhnenClicked(((PurchaseHintsModel) HintsList.SelectedItem).ProductId);
        }
    }
}
