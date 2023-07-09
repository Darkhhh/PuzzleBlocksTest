using System;
using TMPro;
using UnityEngine;

namespace Source.UI.Code.Pages
{
    public class PauseUIHandler : PageHandler
    {
        #region Const Strings

        private const string PauseTextTag = "xml-pause-title";
        private const string ContinueTextTag = "xml-pause-continue";
        private const string RestartTextTag = "xml-pause-restart";
        private const string SettingsTextTag = "xml-pause-settings";
        private const string MainMenuTextTag = "xml-pause-main-menu";
        
        #endregion


        #region Serialize Fields
        
        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI continueText;
        [SerializeField] private TextMeshProUGUI restartText;
        [SerializeField] private TextMeshProUGUI settingsText;
        [SerializeField] private TextMeshProUGUI mainMenuText;

        #endregion


        public Action SettingsOpened, GameOpened, MenuOpened, RestartClicked;
        
        
        public override void OnPageOpen() => UpdateTexts();

        public override void OnPageClose() { }

        public override void UpdateTexts()
        {
            StartCoroutine(GetPageStrings(() =>
            {
                var data = PageStrings[PauseTextTag];
                titleText.text = data.val;
                titleText.fontSize = data.fontSize;

                data = PageStrings[ContinueTextTag];
                continueText.text = data.val;
                continueText.fontSize = data.fontSize;
            
                data = PageStrings[RestartTextTag];
                restartText.text = data.val;
                restartText.fontSize = data.fontSize;
            
                data = PageStrings[SettingsTextTag];
                settingsText.text = data.val;
                settingsText.fontSize = data.fontSize;
            
                data = PageStrings[MainMenuTextTag];
                mainMenuText.text = data.val;
                mainMenuText.fontSize = data.fontSize;
            }));
        }


        #region ButtonClickHandlers

        public void OnResumeButtonClick() => GameOpened?.Invoke();

        public void OnRestartButtonClick() => RestartClicked?.Invoke();

        public void OnSettingsButtonClick() => SettingsOpened?.Invoke();

        public void OnMainMenuButtonClick() => MenuOpened?.Invoke();

        #endregion
    }
}