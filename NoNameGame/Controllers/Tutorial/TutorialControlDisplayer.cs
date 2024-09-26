using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Infrastructure;
using Microsoft.Phone.Controls;
using NoNameGame.CustomControls.Popups;
using NoNameGame.Levels;
using NoNameGame.Levels.Entities;

namespace NoNameGame.Controllers.Tutorial
{
    public class TutorialControlDisplayer
    {
        private readonly ITutorialController _appSettingsTutorialController;        
        private readonly ILevelProvider _levelProvider;
        public TutorialControlDisplayer(ITutorialController appSettingsTutorialController,ILevelProvider levelProvider)
        {
            _appSettingsTutorialController = appSettingsTutorialController;           
            _levelProvider = levelProvider;
        }
        public bool TryShowForLevel(int currentlevelId,PhoneApplicationPage page,Action<UIElement> onCloseAction)
        {          
            LevelDataEntity currentLevel = _levelProvider.GetLevel(currentlevelId);
            if (!currentLevel.TutorialStep.HasValue)
                return false ;
            if (!_appSettingsTutorialController.ShouldShowTutorial())
                return false ;


            int currentTutorialStep = currentLevel.TutorialStep.Value;
            PopupWindowService window = null;
            switch (currentTutorialStep)
            {
                case 1:
                {
                    window = CreateTutorialWindow<GameTutorialControl>(page, onCloseAction);
                    break;
                }
                case 2:
                {
                    window = CreateTutorialWindow<GameTutorialControlStep2>(page, onCloseAction);
                    break;                    
                }
                case 3:
                {
                    window = CreateTutorialWindow<GameTutorialControlStep3>(page, onCloseAction);
                    break;
                }
                case 4:
                {
                    window = CreateTutorialWindow<GameTutorialControlStep4>(page, onCloseAction);
                    break;
                }
                default:
                    throw new InvalidOperationException(
                        string.Format(
                            "TutorialStep stored in database for level with Id {0} has no define popup window.",
                            currentlevelId));
            }
            window.Show();
            return true;
        }
        private PopupWindowService CreateTutorialWindow<T>(PhoneApplicationPage page, Action<UIElement> onCloseAction)
            where T : UserControl, ITutorialControl,new()
        {
            var tutorialWindow = new T();
            return  new PopupWindowService(page, tutorialWindow,
                new UIElementWithTappedAction(tutorialWindow.ClosingButton, onCloseAction));
        }
        //  _appSettingsTutorialController.SetTutorialShowed();                    
    }
}