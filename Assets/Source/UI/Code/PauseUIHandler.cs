using Source.Localization;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Source.UI.Code
{
    public class PauseUIHandler : PageHandler
    {
        public override void OnPageOpen()
        {
            
        }

        public override void OnPageClose()
        {
            gameObject.SetActive(false);
        }

        public override void ChangeLanguage(Language newLanguage)
        {
            base.ChangeLanguage(newLanguage);
        }


        #region ButtonClickHandlers

        public void OnResumeButtonClick()
        {
            Debug.Log("Resume Button Clicked!");
            OnPageClose();
        }

        public void OnRestartButtonClick()
        {
            Debug.Log("Restart Button Clicked!");
        }

        public void OnSettingsButtonClick()
        {
            Debug.Log("Settings Button Clicked!");
        }

        public void OnMainMenuButtonClick()
        {
            Debug.Log("Main Menu Button Clicked!");
        }

        #endregion
    }
}