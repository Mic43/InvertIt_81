using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace NoNameGame.CustomControls.Challenges
{
    public class CurrentChallengesModel
    {       
        public List<CurrentChallengeModel> Challenges { get; set; }
     
        public CurrentChallengesModel()
        {
            Challenges = new List<CurrentChallengeModel>();
        }
    }

    public class CurrentChallengeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public partial class CurrentChallengesControl : UserControl
    {
        private CurrentChallengesModel _levelPacksListControlModel;

        public event ChallengeSelectedEventHandler ChallengeSelected;
        public CurrentChallengesModel CurrentChallengesModel
        {
            get { return _levelPacksListControlModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                DataContext = _levelPacksListControlModel = value;
            }
        }


        public CurrentChallengesControl()
        {
            _levelPacksListControlModel = new CurrentChallengesModel();
            InitializeComponent();         
        }
      
        private void ListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxChallenges.SelectedItem == null)
                return;

            if (ChallengeSelected != null)
                ChallengeSelected.Invoke(this, (CurrentChallengeModel)ListBoxChallenges.SelectedItem);

            ListBoxChallenges.SelectedItem = null;
        }
        
    }

    public delegate void ChallengeSelectedEventHandler(object sender, CurrentChallengeModel newTheme);
}
