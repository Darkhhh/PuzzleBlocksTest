using System.Collections;
using System.Collections.Generic;
using Source.Localization;
using TMPro;
using UnityEngine;

namespace Source.UI.Code
{
    public class PauseUIHandler : PageHandler
    {
        #region Const Strings

        private const string PageTag = "xml-pause";

        private const string PauseTextTag = "xml-pause-title";
        private const string ContinueTextTag = "xml-pause-continue";
        private const string RestartTextTag = "xml-pause-restart";
        private const string SettingsTextTag = "xml-pause-settings";
        private const string MainMenuTextTag = "xml-pause-main-menu";
        
        #endregion


        #region Serialize Fields

        [Header("Settings Page")] 
        [SerializeField] private PageHandler settingsPage;
        [Space]
        
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI continueText;
        [SerializeField] private TextMeshProUGUI restartText;
        [SerializeField] private TextMeshProUGUI settingsText;
        [SerializeField] private TextMeshProUGUI mainMenuText;

        #endregion
        
        
        public override void Prepare()
        {
            base.Prepare();
            StartCoroutine(SetAllTexts(GetLocalizationHandler()));
        }


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
            OnPageClose();
        }

        public void OnRestartButtonClick()
        {
            Debug.Log("Restart Button Clicked!");
        }

        public void OnSettingsButtonClick()
        {
            settingsPage.Prepare();
            settingsPage.gameObject.SetActive(true);
        }

        public void OnMainMenuButtonClick()
        {
            Debug.Log("Main Menu Button Clicked!");
        }

        #endregion
        
        
        private IEnumerator SetAllTexts(ILocalizationHandler localizationHandler)
        {
            while (!localizationHandler.IsLoaded())
            {
                yield return null;
            }

            var strings = new Dictionary<string, (string val, int fontSize)>();
            localizationHandler.GetPageStrings(PageTag, ref strings);
        
        
            var data = strings[PauseTextTag];
            titleText.text = data.val;
            titleText.fontSize = data.fontSize;

            data = strings[ContinueTextTag];
            continueText.text = data.val;
            continueText.fontSize = data.fontSize;
            
            data = strings[RestartTextTag];
            restartText.text = data.val;
            restartText.fontSize = data.fontSize;
            
            data = strings[SettingsTextTag];
            settingsText.text = data.val;
            settingsText.fontSize = data.fontSize;
            
            data = strings[MainMenuTextTag];
            mainMenuText.text = data.val;
            mainMenuText.fontSize = data.fontSize;
        }
    }
}