namespace NoNameGame.Controllers.Tutorial
{
    public class AlwaysShowedTutorialController : ITutorialController
    {
        public bool ShouldShowTutorial()
        {
            return true;
        }
        public void SetTutorialShowed()
        {
            ;
        }
    }
}