using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using AnimationLib.AnimationDSL;
using AnimationLib.AnimationsCreator;
using NoNameGame.Helpers;
using NoNameGame.Models.ChallengeGame;
using NoNameGame.Resources;

namespace NoNameGame.CustomControls.Popups.Challenge
{
    public class GameWonControlModel
    {
        public int Points { get; private set; }
        public bool CanStartNextLevel { get;private set; }
        public int MovesCount { get; private set; }
        public bool NewItemUnlocked{ get; private set; }

        public bool IsPerfect { get; set; }
        public string MovesCountCaption
        {
            get
            {                
                return string.Format("{0} {1} {2}.", AppResources.GameWonControl_MovesDescriptionTextBlock, MovesCount, (MovesCount > 1
                        ? AppResources.GameWonControl_MovesCaptionMany
                        : AppResources.GameWonControlModel_MovesCountCaption_Single));
            } 
        }

        public GameWonControlModel(int points, bool canStartNextLevel, int movesCount, bool newItemUnlocked,bool isPerfect)
        {
            Points = points;
            CanStartNextLevel = canStartNextLevel;
            MovesCount = movesCount;
            NewItemUnlocked = newItemUnlocked;
            this.IsPerfect = isPerfect;
        }
    }

    public partial class ChallengeFinishedControl : UserControl
    {        

        private ChallengeFinishedModel _challengeFinishedModel;

        public ChallengeFinishedModel ChallengeFinishedModel
        {
            get { return _challengeFinishedModel; }
            set
            {
                _challengeFinishedModel = value;
                DataContext = value;
            }
        }

        public ChallengeFinishedControl()
        {
            InitializeComponent();                 
            Themer.EnableThemesForControls(OkButton);                      
        }             
    }
}